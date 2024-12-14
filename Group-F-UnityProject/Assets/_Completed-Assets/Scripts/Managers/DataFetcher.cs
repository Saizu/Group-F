using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataFetcher : MonoBehaviour
{
    

    [SerializeField] private string apiUrl = "http://localhost:63245"; // APIのURL（インスペクタから設定）
    public int currentUserId;
    private string lastLoginTime = "";

    private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

    public IEnumerator GetUserDetailByName(string userName)
    {
        string url = $"{apiUrl}/users/get-id-by-name/?name={userName}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<UserIdResponse>(webRequest.downloadHandler.text);

                if (response != null && response.userId > 0)
                {
                    currentUserId = response.userId;
                    Debug.Log($"ユーザーID: {currentUserId}");
                        // PlayerPrefsにユーザーIDを保存
                    PlayerPrefs.SetInt("currentUserId", currentUserId);
                    PlayerPrefs.Save(); // 保存を確定するために必ず呼び出す
                    StartCoroutine(GetItemsByUserId(currentUserId)); 
                    yield return StartCoroutine(GetLastLogin(currentUserId)); 
                    StartCoroutine(UpdateLastLogin(currentUserId));
                    StartCoroutine(GetUserStamina(currentUserId));
                    StartCoroutine(GetConsecutiveDays(currentUserId));
                }
                else
                {
                    Debug.LogError("ユーザー情報が空または無効です");
                }
            }
            else
            {
                Debug.LogError($"APIリクエスト失敗: {webRequest.error}");
            }
        }
    }

    public IEnumerator GetItemsByUserId(int userId)
    {
        string url = $"{apiUrl}/users-items/get-by-user/?usrid={userId}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"アイテム情報レスポンス: {webRequest.downloadHandler.text}");
                ProcessUserItemsData(webRequest.downloadHandler.text, userId);
            }
            else
            {
                Debug.LogError($"アイテム情報リクエスト失敗: {webRequest.error}");
            }
        }
    }

    public IEnumerator GetItems()
    {
        string url = $"{apiUrl}/items/get/";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("アイテム一覧取得成功: " + webRequest.downloadHandler.text);
                ProcessItemsData(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError("エラー: " + webRequest.error);
            }
        }
    }

    private void ProcessItemsData(string json)
    {
        var itemsResponse = JsonUtility.FromJson<ItemsResponse>(json);

        if (itemsResponse != null && itemsResponse.items != null)
        {
            itemDictionary.Clear();
            foreach (var item in itemsResponse.items)
            {
                itemDictionary[item.ID] = new Item { Item_ID = item.ID, Item_Name = item.Name };
            }

            Debug.Log($"合計 {itemDictionary.Count} 種類のアイテムを読み込みました");
        }
        else
        {
            Debug.LogError("アイテム情報が無効または空です");
        }
    }

   private void ProcessUserItemsData(string json, int userId)
    {
        var usersItemsResponse = JsonUtility.FromJson<UsersItemsResponse>(json);

        // アイテムを格納するリストを初期化
        List<string> userItemsList = new List<string>();

        // すべてのアイテムをチェック
        foreach (var item in itemDictionary)
        {
            int itemId = item.Key;
            string itemName = item.Value.Item_Name;
            int itemAmount = 0; // デフォルトで0に設定

            // ユーザーが所持しているアイテムがあれば数量を更新
            if (usersItemsResponse != null && usersItemsResponse.users_items != null)
            {
                var userItem = System.Array.Find(usersItemsResponse.users_items, 
                    ui => ui.Usrid == userId && ui.Itmid == itemId);

                if (userItem != null)
                {
                    itemAmount = userItem.Amount;
                }
            }

            // アイテム名と数量を文字列として保存
            string itemEntry = $"{itemName}:{itemAmount}";
            userItemsList.Add(itemEntry);

            // コンソールにログを出力
            Debug.Log($"アイテム名: {itemName}, 数量: {itemAmount}");
        }

        // PlayerPrefsにアイテムデータを保存
        PlayerPrefs.SetString("UserItems", string.Join(",", userItemsList));
        PlayerPrefs.Save();
    }
    public IEnumerator UpdateLastLogin(int userId)
    {
        string url = $"{apiUrl}/users/update-last-login/";
        string json = $"{{\"id\": {userId}}}";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"ユーザー {userId} の最終ログイン日時が更新されました");
            }
            else
            {
                Debug.LogError($"最終ログイン日時の更新に失敗: {webRequest.error}");
            }
        }
    }

    public IEnumerator GetLastLogin(int userId)
    {
        string url = $"{apiUrl}/users/get-last-login/?usrid={userId}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                lastLoginTime = webRequest.downloadHandler.text;
                Debug.Log($"ユーザー {userId} の最終ログイン日時: {lastLoginTime}");

                // PlayerPrefsに保存
                PlayerPrefs.SetString("lastLoginTime", lastLoginTime);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogError($"最終ログイン日時の取得に失敗: {webRequest.error}");
            }
        }
    }

    public string GetLastLoginTime()
    {
        return PlayerPrefs.GetString("lastLoginTime", "データなし");
    }

    // PlayerPrefsからアイテム情報を取得し、アイテム名と個数を返すメソッド
    public Dictionary<string, int> GetSavedUserItems()
    {
        // PlayerPrefsからアイテム情報を取得
        string savedItems = PlayerPrefs.GetString("UserItems", "");

        // アイテム情報を格納するDictionary
        Dictionary<string, int> userItems = new Dictionary<string, int>();

        // 保存されているアイテム情報があれば解析
        if (!string.IsNullOrEmpty(savedItems))
        {
            string[] items = savedItems.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            // 各アイテムを ":" で分割してアイテム名と個数を取得
            foreach (string item in items)
            {
                string[] itemParts = item.Split(':');
                if (itemParts.Length == 2)
                {
                    string itemName = itemParts[0];
                    if (int.TryParse(itemParts[1], out int itemAmount))
                    {
                        userItems[itemName] = itemAmount;
                    }
                    else
                    {
                        Debug.LogWarning($"アイテム '{itemName}' の個数が無効です。");
                    }
                }
                else
                {
                    Debug.LogWarning($"アイテム情報 '{item}' の形式が不正です。");
                }
            }
        }

        return userItems;
    }

    //id ,item_id, amountでアイテムを付与
    public IEnumerator PostItemToUser(int userId, int itemId, int amount)
    {
        string url = $"{apiUrl}/users-items/post-to/";
        string json = $"{{\"Usrid\": {userId}, \"Itmid\": {itemId}, \"Amount\": {amount}}}";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"アイテム {itemId} をユーザー {userId} に {amount} 個付与しました: {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"アイテム付与に失敗: {webRequest.error}");
            }
        }
    }

 public IEnumerator GetUserStamina(int userId)
    {
        string url = $"{apiUrl}/users/stamina/?id={userId}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string staminaResponse = webRequest.downloadHandler.text;

                try
                {
                    // JSON からスタミナをパース
                    StaminaResponse response = JsonUtility.FromJson<StaminaResponse>(staminaResponse);
                    int stamina = response.stamina;

                    // PlayerPrefs にスタミナを保存
                    PlayerPrefs.SetInt($"UserStamina_{userId}", stamina);
                    PlayerPrefs.Save();

                    Debug.Log($"ユーザー {userId} のスタミナ: {stamina}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"スタミナレスポンスのパースに失敗: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"スタミナ取得に失敗: {webRequest.error}");
            }
        }
    }
public IEnumerator UpdateUserStamina(int userId, int staminaValue)
{
    string url = $"{apiUrl}/users/update-stamina/";

    // サーバーが期待する形式のJSONオブジェクトを作成
    StaminaUpdateRequest requestBody = new StaminaUpdateRequest
    {
        id = userId,
        stamina = staminaValue // スタミナを設定
    };

    // JSONに変換
    string json = JsonUtility.ToJson(requestBody);

    // 送信するJSON内容をデバッグ
    Debug.Log($"送信するJSON: {json}");

    using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
    {
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"ユーザー {userId} のスタミナを {staminaValue} に更新しました");

            // PlayerPrefs にスタミナを保存
            PlayerPrefs.SetInt($"UserStamina_{userId}", staminaValue);
            PlayerPrefs.Save();
        }
        else
        {
            // サーバーからのエラーレスポンス全体をログに出力
            string errorResponse = webRequest.downloadHandler.text;
            Debug.LogError($"スタミナ更新に失敗: {webRequest.error}, レスポンス: {errorResponse}");
        }
    }
}

 public IEnumerator GetConsecutiveDays(int userId)
    {
        string url = $"{apiUrl}/users/consecutive-days/?id={userId}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string consecutiveDaysResponse = webRequest.downloadHandler.text;

                try
                {
                    // JSON から連続日数をパース
                    ConsecutiveDaysResponse response = JsonUtility.FromJson<ConsecutiveDaysResponse>(consecutiveDaysResponse);
                    int consecutiveDays = response.consecutive_days;

                    // PlayerPrefs に連続日数を保存
                    PlayerPrefs.SetInt($"UserConsecutiveDays_{userId}", consecutiveDays);
                    PlayerPrefs.Save();

                    Debug.Log($"ユーザー {userId} の連続日数: {consecutiveDays}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"連続日数レスポンスのパースに失敗: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"連続日数の取得に失敗: {webRequest.error}");
            }
        }
    }

    public IEnumerator UpdateConsecutiveDays(int userId, int consecutiveDaysValue)
{
    string url = $"{apiUrl}/users/update-consecutive-days/";

    // サーバーが期待する形式のJSONオブジェクトを作成
    ConsecutiveDaysUpdateRequest requestBody = new ConsecutiveDaysUpdateRequest
    {
        id = userId,
        ConsecutiveDays = consecutiveDaysValue // 連続日数を設定
    };

    // JSONに変換
    string json = JsonUtility.ToJson(requestBody);

    // 送信するJSON内容をデバッグ
    Debug.Log($"送信するJSON: {json}");

    using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
    {
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"ユーザー {userId} の連続日数を {consecutiveDaysValue} に更新しました");
            
            // PlayerPrefsに連続日数を更新
            PlayerPrefs.SetInt($"UserConsecutiveDays_{userId}", consecutiveDaysValue);
            PlayerPrefs.Save();
        }
        else
        {
            // サーバーからのエラーレスポンス全体をログに出力
            string errorResponse = webRequest.downloadHandler.text;
            Debug.LogError($"連続日数の更新に失敗: {webRequest.error}, レスポンス: {errorResponse}");
        }
    }
}


    // PlayerPrefsから保存されたスタミナを取得するメソッド
    public int GetSavedUserStamina(int userId)
    {
        return PlayerPrefs.GetInt($"UserStamina_{userId}", 0);
    }

    // PlayerPrefsから保存された連続日数を取得するメソッド
    public int GetSavedConsecutiveDays(int userId)
    {
        return PlayerPrefs.GetInt($"UserConsecutiveDays_{userId}", 0);
    }

    


}
// APIレスポンスとして返される userId を格納するクラス
[System.Serializable]
public class UserIdResponse
{
    public int userId; // ユーザーID
}

// ユーザーとアイテムの関連情報
[System.Serializable]
public class Users_Items
{
    public int Usrid;
    public int Itmid;
    public int Amount;
}

// ユーザーとアイテム情報のレスポンス
[System.Serializable]
public class UsersItemsResponse
{
    public Users_Items[] users_items; // ユーザーとアイテムの関連情報
}

// アイテム情報クラス
[System.Serializable]
public class Item
{
    public int Item_ID;
    public string Item_Name;
}

// アイテム情報レスポンスクラス
[System.Serializable]
public class ItemsResponse
{
    public ItemWithIdAndName[] items;
}

// アイテムのIDと名前を持つクラス
[System.Serializable]
public class ItemWithIdAndName
{
    public int ID;
    public string Name;
}

[System.Serializable]
    public class StaminaResponse
    {
        public int stamina;
    }
[System.Serializable]
    public class ConsecutiveDaysResponse
    {
        public int consecutive_days;
    }
    // スタミナ更新のためのリクエストボディクラス
[System.Serializable]
public class StaminaUpdateRequest
{
    public int id;      // ユーザーID
    public int stamina; // スタミナ
}

[System.Serializable]
public class ConsecutiveDaysUpdateRequest
{
    public int id;               // ユーザーID
    public int ConsecutiveDays;  // 連続日数
}