using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LastLogin
{
    public string Time;
    public bool Valid;
}

[System.Serializable]
public class LastLoginWrapper
{
    public LastLogin last_login;
}

public class LoginManager : MonoBehaviour
{
    private ItemList itemList;
    public TMP_InputField userNameInput; // ユーザー名入力フィールド
    public TMP_InputField passwordInput; // パスワード入力フィールド
    public TMP_Text errorMessageText; // エラーメッセージ表示用

    public GameObject loginPage; // ログインページ
    public GameObject bonusPage; // ボーナスページ
    public GameObject mainMenuPage; // メインメニューページ

    private string currentUser; // 現在ログイン中のユーザー名
    private DataFetcher dataFetcher;
    //private bool test = true;
    void Start()
    {
        dataFetcher = GetComponent<DataFetcher>();
        AppState.CurrentPage = "Login";
        //Debug.Log($"Current Page: {AppState.CurrentPage}");
        loginPage.SetActive(true);
        bonusPage.SetActive(false);
        mainMenuPage.SetActive(false); // メインメニューは最初は非表示に
        errorMessageText.text = ""; // 初期状態ではエラーメッセージを非表示
    }

    public void OnLoginButtonClicked()
    {
        string userName = userNameInput.text;
        string password = passwordInput.text;

        if (ValidateUser(userName, password))
        {
            errorMessageText.text = "";

            // 現在のユーザー名を保存
            currentUser = userName;
            PlayerPrefs.SetString("currentUser", currentUser);
            PlayerPrefs.Save();
            
            //サーバーからログインしようとしている人の情報の取得
            if (dataFetcher != null)
            {
                StartCoroutine(GetUserIdAndItems(userName));
            }
            else
            {
                Debug.LogError("DataFetcher コンポーネントが見つかりません");
            }
        }
        else
        {
            errorMessageText.text = "Invalid username or password.";
        }
    }






    //ここにアカウント登録できる仕組みを作ってください。
    public void OnRegisterButtonClicked()
    {
        string userName = userNameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            errorMessageText.text = "Please enter a username and password.";
            return;
        }

        if (PlayerPrefs.HasKey(userName + "_password"))
        {
            errorMessageText.text = "Username already exists.";
        }
        else
        {
            PlayerPrefs.SetString(userName + "_password", password);
            PlayerPrefs.SetString(userName + "_lastLogin", "");
            PlayerPrefs.SetInt(userName + "_" + itemList.item1, 3);
            PlayerPrefs.SetInt(userName + "_" + itemList.item2, 3);
            PlayerPrefs.Save();

            errorMessageText.text = "Account created successfully. Please log in.";
            userNameInput.text = "";
            passwordInput.text = "";
        }
    }

    //ここにサーバーからどんなパスワードか取ってきて正しいか確認できるようにしてください。
    private bool ValidateUser(string userName, string password)
    {   
        //これはサーバーを通していません。ここをサーバーを通してパスワードを取得して格納してください。
        string savedPassword = PlayerPrefs.GetString(userName + "_password", "");

        return savedPassword == password;
    }







// ユーザーIDを取得し、そのIDを使ってアイテム情報を取得する
    private IEnumerator GetUserIdAndItems(string userName)
    {
        Debug.Log($"ユーザー名: {userName} の情報を取得中...");

        // まずすべてのアイテム情報を非同期で取得
        yield return StartCoroutine(dataFetcher.GetItems());

        // ユーザーIDを非同期で取得
        yield return StartCoroutine(dataFetcher.GetUserDetailByName(userName));

        // last_login JSONから"Time"部分を取得
        string lastLoginJson = PlayerPrefs.GetString("lastLoginTime", "データなし");

        // lastLoginTimeがデータなしでない場合、日付を比較
        if (lastLoginJson != "データなし")
        {
            // JSON文字列をLastLoginWrapperオブジェクトに変換
            LastLoginWrapper lastLoginWrapper = JsonUtility.FromJson<LastLoginWrapper>(lastLoginJson);

            // last_loginのTime部分を取得
            string lastLoginTime = lastLoginWrapper.last_login.Time;

            if (!string.IsNullOrEmpty(lastLoginTime))
            {
                // Timeの部分を抽出して日付部分（yyyy-MM-dd）を取得
                string lastLoginDate = ExtractDate(lastLoginTime);

                // 今日の日付を取得
                string today = System.DateTime.UtcNow.ToString("yyyy-MM-dd");
                Debug.Log($"Today:{today}");

                // 日付が一致するかを確認し、結果を表示
                if (lastLoginDate == today)
                {   
                    yield return new WaitForSeconds(0.1f);
                    // 1日以内に再ログインしている場合はメインメニューに遷移
                    mainMenuPage.SetActive(true);
                    loginPage.SetActive(false);
                    Debug.Log("Bonus not applicable today. Showing Main Menu.");
                    Debug.Log($"APIからの最後のログイン日は今日です。 ({lastLoginDate})");
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                    // ボーナスページへ遷移
                    bonusPage.SetActive(true);
                    bonusPage.GetComponent<BonusManager>().OnBonusPageShown();
                    loginPage.SetActive(false);
                    Debug.Log($"APIからの最後のログイン日は今日ではありません。 ({lastLoginDate})");
                }
            }
            else
            {
                Debug.Log("lastLoginTimeが空です。");
            }
        }
        else
        {
            Debug.Log("lastLoginTimeが設定されていません。");
        }
    }

    // last_login JSONのTime部分から日付部分（yyyy-MM-dd）を抽出するヘルパーメソッド
    private string ExtractDate(string dateTime)
    {
        // 例: "2024-12-12T12:44:13.386378Z" から "2024-12-12" を抽出
        // 'T'を区切り文字として日付部分を抽出する
        int endIndex = dateTime.IndexOf("T");
        if (endIndex != -1)
        {
            // 日付部分（yyyy-MM-dd）を抽出
            return dateTime.Substring(0, endIndex);
        }
        return string.Empty; // "T"が見つからない場合
    }
    
}
