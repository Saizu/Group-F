using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    public TMP_Text item1Text; // アイテム1のテキスト
    public TMP_Text item2Text; // アイテム2のテキスト
    public GameObject mainMenuPage; // メインメニューのページ

    private string currentUser; // 現在のユーザー名
    public GameObject item1Effect; // アイテム1の効果を発生させるオブジェクト
    public GameObject item2Effect; // アイテム2の効果を発生させるオブジェクト

    [SerializeField] private ItemList itemList; // ItemListを参照

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
        int item1 = PlayerPrefs.GetInt(currentUser + "_" + itemList.item1, 0);
        if (item1 > 0)
        {
            // アイテム1の効果を発生させる
            GameObject spawnedObject = Instantiate(item1Effect, Vector3.zero, Quaternion.identity);
            Debug.Log("オブジェクトを生成しました: " + spawnedObject.name);
            PlayerPrefs.SetInt(currentUser + "_" + itemList.item1, item1 - 1); // アイテムの数を減らす
            PlayerPrefs.Save();
            UpdateItemDisplay(); // アイテム表示の更新
        }
        else
        {
            Debug.Log("Not enough " + itemList.item1 + ".");
        }
    }

    // アイテム2を使用する
    public void OnUseItem2ButtonClicked()
    {
        int item2 = PlayerPrefs.GetInt(currentUser + "_" + itemList.item2, 0);
        if (item2 > 0)
        {
            // アイテム2の効果を発生させる
            GameObject spawnedObject = Instantiate(item2Effect, Vector3.zero, Quaternion.identity);
            Debug.Log("オブジェクトを生成しました: " + spawnedObject.name);
            PlayerPrefs.SetInt(currentUser + "_" + itemList.item2, item2 - 1); // アイテムの数を減らす
            PlayerPrefs.Save();
            UpdateItemDisplay(); // アイテム表示の更新
        }
        else
        {
            Debug.Log("Not enough " + itemList.item2 + ".");
        }
    }

    // アイテムの表示を更新
    private void UpdateItemDisplay()
    {
        int item1 = PlayerPrefs.GetInt(currentUser + "_" + itemList.item1, 0);
        int item2 = PlayerPrefs.GetInt(currentUser + "_" + itemList.item2, 0);

        if (item1Text != null)
        {
            item1Text.text = itemList.item1 + ": " + item1; // アイテム1の数を表示
        }
        else
        {
            Debug.LogError("Item 1 Text is not assigned in the inspector.");
        }

        if (item2Text != null)
        {
            item2Text.text = itemList.item2 + ": " + item2; // アイテム2の数を表示
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
