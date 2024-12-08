using UnityEngine;
using TMPro;

public class BonusManager : MonoBehaviour
{
    public TMP_Text bonusText; // ボーナステキスト
    public TMP_Text item1Text; // アイテム1のテキスト
    public TMP_Text item2Text; // アイテム2のテキスト
    public GameObject mainMenuPage; // メインメニュー

    void Start(){
        AppState.CurrentPage = "Bonus";
        Debug.Log($"Current Page: {AppState.CurrentPage}");
    }
    // ログインボーナス画面
    public void OnBonusPageShown()
    {
        Debug.Log("OnBonusPageShown called.");

        string userName = PlayerPrefs.GetString("currentUser", "defaultUser");
        int item1 = PlayerPrefs.GetInt(userName + "_item1", 0);
        int item2 = PlayerPrefs.GetInt(userName + "_item2", 0);

        Debug.Log($"Before bonus: Item1 = {item1}, Item2 = {item2}");

        // アイテム増加処理
        PlayerPrefs.SetInt(userName + "_item1", item1 + 1);
        PlayerPrefs.SetInt(userName + "_item2", item2 + 1);
        PlayerPrefs.Save();

        Debug.Log($"After bonus: Item1 = {item1 + 1}, Item2 = {item2 + 1}");

        // アイテム数を表示
        item1Text.text = "Get Item1! Add HP Item: " + (item1 + 1);
        item2Text.text = "Get Item2! Add Speed Item: " + (item2 + 1);

        // ボーナスメッセージ
        bonusText.text = "You received your login bonus!";
        Debug.Log("Bonus message updated.");
    }

    // メインメニューへ戻る
    public void OnMainMenuButtonClicked()
    {
        mainMenuPage.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

