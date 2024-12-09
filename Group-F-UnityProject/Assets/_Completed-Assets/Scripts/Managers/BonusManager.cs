using UnityEngine;
using TMPro;

public class BonusManager : MonoBehaviour
{
    public TMP_Text bonusText; // ボーナステキスト
    public TMP_Text item1Text; // アイテム1のテキスト
    public TMP_Text item2Text; // アイテム2のテキスト
    public GameObject mainMenuPage; // メインメニュー

    [SerializeField] private ItemList itemList; // ItemListを参照
    [SerializeField] private string bonusMessage = "You received your login bonus!";
    [SerializeField] private int addNumItem1 = 1;
    [SerializeField] private int addNumItem2 = 1;

    void Start()
    {
        AppState.CurrentPage = "Bonus";
        Debug.Log($"Current Page: {AppState.CurrentPage}");
    }

    // ログインボーナス画面
    public void OnBonusPageShown()
    {
        string userName = PlayerPrefs.GetString("currentUser", "defaultUser");
        int item1 = PlayerPrefs.GetInt(userName + "_" + itemList.item1, 0);
        int item2 = PlayerPrefs.GetInt(userName + "_" + itemList.item2, 0);

        Debug.Log($"Before bonus: {itemList.item1} = {item1}, {itemList.item2} = {item2}");

        // アイテム増加処理
        PlayerPrefs.SetInt(userName + "_" + itemList.item1, item1 + addNumItem1);
        PlayerPrefs.SetInt(userName + "_" + itemList.item2, item2 + addNumItem2);
        PlayerPrefs.Save();

        Debug.Log($"After bonus: {itemList.item1} = {item1 + addNumItem1}, {itemList.item2} = {item2 + addNumItem2}");

        // アイテム数を表示
        item1Text.text = $"Get {itemList.item1}: +{addNumItem1}";
        item2Text.text = $"Get {itemList.item2}: +{addNumItem2}";

        // ボーナスメッセージ
        bonusText.text = bonusMessage;
    }

    // メインメニューへ戻る
    public void OnMainMenuButtonClicked()
    {
        mainMenuPage.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
