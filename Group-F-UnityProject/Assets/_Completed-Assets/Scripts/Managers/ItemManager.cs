using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    public TMP_Text item1Text; // アイテム1のテキスト
    public TMP_Text item2Text; // アイテム2のテキスト
    public GameObject mainMenuPage; // メインメニューのページ

    private string currentUser; // 現在のユーザー名

    void Start()
    {
        AppState.CurrentPage = "Item";
        Debug.Log($"Current Page: {AppState.CurrentPage}");
        // 現在ログイン中のユーザーを取得
        currentUser = PlayerPrefs.GetString("currentUser", "defaultUser");
        Debug.Log("Current User: " + currentUser);

        // アイテムの表示を更新
        UpdateItemDisplay();
    }

    // アイテム1を使用する
    public void OnUseItem1ButtonClicked()
    {
        int item1 = PlayerPrefs.GetInt(currentUser + "_item1", 0);
        if (item1 > 0)
        {
            PlayerPrefs.SetInt(currentUser + "_item1", item1 - 1);
            PlayerPrefs.Save();
            UpdateItemDisplay();
        }
        else
        {
            Debug.Log("Not enough Item 1.");
        }
    }

    // アイテム2を使用する
    public void OnUseItem2ButtonClicked()
    {
        int item2 = PlayerPrefs.GetInt(currentUser + "_item2", 0);
        if (item2 > 0)
        {
            PlayerPrefs.SetInt(currentUser + "_item2", item2 - 1);
            PlayerPrefs.Save();
            UpdateItemDisplay();
        }
        else
        {
            Debug.Log("Not enough Item 2.");
        }
    }

    // アイテムの表示を更新
    private void UpdateItemDisplay()
    {
        int item1 = PlayerPrefs.GetInt(currentUser + "_item1", 0);
        int item2 = PlayerPrefs.GetInt(currentUser + "_item2", 0);

        if (item1Text != null)
        {
            item1Text.text = "Item 1: " + item1;
        }
        else
        {
            Debug.LogError("Item 1 Text is not assigned in the inspector.");
        }

        if (item2Text != null)
        {
            item2Text.text = "Item 2: " + item2;
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
}
