using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    public TMP_Text item1Text;
    public TMP_Text item2Text;
    public TMP_Text errorMessageText;
    public GameObject mainMenuPage;
    public TMP_Text staminaText;

    private string currentUser;
    //public GameObject item1Effect;
    //public GameObject item2Effect;

    private DataFetcher dataFetcher;
    public ItemList itemList;
    int userId = 0;

    void Start()
    {
        AppState.CurrentPage = "Item";
        Debug.Log($"Current Page: {AppState.CurrentPage}");

        dataFetcher = GetComponent<DataFetcher>();

        currentUser = PlayerPrefs.GetString("currentUser", "defaultUser");
        userId = PlayerPrefs.GetInt("currentUserId", -1);
        StartCoroutine(UpdateStaminaText(userId));
        StartCoroutine(GetItemsById(userId));
        ShowErrorMessage("");
    }


    private IEnumerator UpdateStaminaText(int userId)
    {
        // データをフェッチして少し待つ
        yield return null; // 必要に応じてここで待機処理を追加

        int nowStamina = dataFetcher.GetSavedUserStamina(userId);

        // テキストを更新
        staminaText.text = $"Now Stamina {nowStamina} / 5";
    }
    public void OnUseItem1ButtonClicked()
    {
        Dictionary<string, int> userItems = dataFetcher.GetSavedUserItems();

        if (userItems.Count > 0)
        {
            string item1Name = userItems.First().Key;
            int item1Amount = userItems.First().Value;

            // Check if effect is already active
            if (ItemEffectManager.IsItemEffectActive(1))
            {
                Debug.Log("Item 1 effect is already active!");
                ShowErrorMessage(item1Name + " effect is already active!");
                return;
            }

            if (item1Amount > 0)
            {
                // Activate the effect
                //GameObject spawnedObject = Instantiate(item1Effect, Vector3.zero, Quaternion.identity);
                //Debug.Log("オブジェクトを生成しました: " + spawnedObject.name);

                // Mark the effect as active
                ItemEffectManager.ActivateItemEffect(1);

                // Reduce item amount
                item1Amount -= 1;
                StartCoroutine(dataFetcher.PostItemToUser(userId, 1, -1));
                ShowErrorMessage("You have used "+ item1Name + "!");

                // Update saved items
                UpdateSavedItems(item1Name, item1Amount);

                // Update display
                UpdateItemDisplay();

                Debug.Log($"{item1Name}の残り数: {item1Amount}");
            }
            else
            {
                ShowErrorMessage("Not enough " + item1Name + ".");
                Debug.Log("Not enough " + item1Name + ".");
            }
        }
    }

    public void OnUseItem2ButtonClicked()
    {
        Dictionary<string, int> userItems = dataFetcher.GetSavedUserItems();
        int nowStamina = dataFetcher.GetSavedUserStamina(userId);

        if (userItems.Count > 1)
        {
            string item2Name = userItems.Skip(1).First().Key;
            int item2Amount = userItems.Skip(1).First().Value;
            if(nowStamina == 5)
            {
                ShowErrorMessage("Your stamina is full!");
                return;
            }
            // Check if effect is already active
            /*
            if (ItemEffectManager.IsItemEffectActive(2))
            {
                ShowErrorMessage(item2Name + " effect is already active!");
                Debug.Log("Item 2 effect is already active!");
                return;
            }
            */

            if (item2Amount > 0)
            {
                // Activate the effect
                //GameObject spawnedObject = Instantiate(item2Effect, Vector3.zero, Quaternion.identity);
                //Debug.Log("オブジェクトを生成しました: " + spawnedObject.name);

                // Mark the effect as active
                ItemEffectManager.ActivateItemEffect(2);

                // Reduce item amount
                item2Amount -= 1;
                StartCoroutine(dataFetcher.PostItemToUser(userId, 2, -1));
                ShowErrorMessage("You have used "+ item2Name + "!");

                // Update saved items
                UpdateSavedItems(item2Name, item2Amount);
                
                nowStamina += 1;
                StartCoroutine(dataFetcher.UpdateUserStamina(userId,nowStamina));

                // Update display
                UpdateItemDisplay();

                Debug.Log($"{item2Name}の残り数: {item2Amount}");
            }
            else
            {
                ShowErrorMessage("Not enough " + item2Name + ".");
                Debug.Log("Not enough " + item2Name + ".");
            }
        }
    }

    // Helper method to update saved items
    private void UpdateSavedItems(string itemName, int itemAmount)
    {
        string savedItems = PlayerPrefs.GetString("UserItems", "");
        List<string> items = new List<string>();

        if (!string.IsNullOrEmpty(savedItems))
        {
            items.AddRange(savedItems.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
        }

        bool itemUpdated = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].StartsWith(itemName + ":"))
            {
                items[i] = $"{itemName}:{itemAmount}";
                itemUpdated = true;
                break;
            }
        }

        if (!itemUpdated)
        {
            items.Add($"{itemName}:{itemAmount}");
        }

        string updatedItems = string.Join(",", items);
        PlayerPrefs.SetString("UserItems", updatedItems);
        PlayerPrefs.Save();
    }


    // アイテムの表示を更新
    private void UpdateItemDisplay()
    {
        // アイテム情報を取得
        Dictionary<string, int> userItems = dataFetcher.GetSavedUserItems();

        // 最初のアイテム情報を取得
        string item1Name = "No Item";
        int item1Amount = 0;
        string item2Name = "No Item";
        int item2Amount = 0;

        if (userItems.Count > 0)
        {
            var firstItem = userItems.First();
            item1Name = firstItem.Key;
            item1Amount = firstItem.Value;

            // もし2番目のアイテムがあれば取得
            if (userItems.Count > 1)
            {
                var secondItem = userItems.Skip(1).First();
                item2Name = secondItem.Key;
                item2Amount = secondItem.Value;
            }
        }

        // アイテム情報を表示
        if (item1Text != null)
        {
            item1Text.text = $"{item1Name}: {item1Amount}";
        }
        else
        {
            Debug.LogError("Item 1 Text is not assigned in the inspector.");
        }

        if (item2Text != null)
        {
            item2Text.text = $"{item2Name}: {item2Amount}";
        }
        else
        {
            Debug.LogError("Item 2 Text is not assigned in the inspector.");
        }
    }

    // メインメニューに戻る
    public void OnBackToMainMenuButtonClicked()
    {
        if (mainMenuPage != null)
        {
            gameObject.SetActive(false);
            mainMenuPage.SetActive(true);
        }
        else
        {
            Debug.LogError("Main Menu Page is not assigned in the inspector.");
        }
    }


        private IEnumerator GetItemsById(int userId)
    {
        // dataFetcher.GetItems() の終了を待つ
        yield return StartCoroutine(dataFetcher.GetItems());
        // dataFetcher.GetItemsByUserId(userId) の終了を待つ
        yield return StartCoroutine(dataFetcher.GetItemsByUserId(userId));
        // アイテムの表示を更新
        UpdateItemDisplay();
    }

        private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
        }
        else
        {
            Debug.LogError("Error Message Text is not assigned in the inspector.");
        }
    }

}