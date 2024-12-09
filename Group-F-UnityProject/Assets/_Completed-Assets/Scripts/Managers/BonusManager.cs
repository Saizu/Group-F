using UnityEngine;
using TMPro;

public class BonusManager : MonoBehaviour
{
    public TMP_Text bonusText; // ボーナステキスト
    public TMP_Text item1Text; // アイテム1のテキスト
    public TMP_Text item2Text; // アイテム2のテキスト
    public GameObject mainMenuPage; // メインメニュー

    [SerializeField] private string bonusMessage = "You received your login bonus!";
    [SerializeField] private string bonusItem1Name = "Add HP Item";
    [SerializeField] private int addNumItem1 = 1;
    [SerializeField] private string bonusItem2Name = "Add Speed Item";
    [SerializeField] private int addNumItem2 = 1;

    void Start()
    {
        AppState.CurrentPage = "Bonus";

    }

    // ログインボーナス画面
    public void OnBonusPageShown()
    {


        string userName = PlayerPrefs.GetString("currentUser", "defaultUser");
        int item1 = PlayerPrefs.GetInt(userName + "_item1", 0);
        int item2 = PlayerPrefs.GetInt(userName + "_item2", 0);



        // アイテム増加処理
        PlayerPrefs.SetInt(userName + "_item1", item1 + addNumItem1);
        PlayerPrefs.SetInt(userName + "_item2", item2 + addNumItem2);
        PlayerPrefs.Save();



        // アイテム数を表示
        item1Text.text = "Get " + bonusItem1Name + " : "+ addNumItem1;
        item2Text.text = "Get " + bonusItem2Name + " : "+ addNumItem2;

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
