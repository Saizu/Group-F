using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BonusManager : MonoBehaviour
{   
    int userId = 0;
    private ItemList itemList;
    public TMP_Text bonusText; // ボーナステキスト
    public TMP_Text item1Text; // アイテム1のテキスト
    public TMP_Text item2Text; // アイテム2のテキスト
    public GameObject mainMenuPage; // メインメニュー
    private DataFetcher dataFetcher;

    private string bonusMessage; 
    [SerializeField] private int addNumItem1 = 1;
    [SerializeField] private int addNumItem2 = 1;

    void Start()
    {
        AppState.CurrentPage = "Bonus";
        Debug.Log($"Current Page: {AppState.CurrentPage}");
    }

    public void OnBonusPageShown()
    {
        //Debug.Log("OnBonusPageShown() is called.");

        // 現在のユーザー名をPlayerPrefsから取得
        string userName = PlayerPrefs.GetString("currentUser", "defaultUser");

        dataFetcher = GetComponent<DataFetcher>();

        // PlayerPrefsから保存されているアイテム情報を取得
        Dictionary<string, int> userItems = dataFetcher.GetSavedUserItems();

        //ログインボーナスを変更するためのプログラム
        int user_id = PlayerPrefs.GetInt("currentUserId",-1);
        int consecutiveDays = dataFetcher.GetSavedConsecutiveDays(user_id);
        consecutiveDays = consecutiveDays % 7;
        if(consecutiveDays == 0){
            consecutiveDays += 7;
        }
        bonusMessage = "You received the "+consecutiveDays+"day's login bonus!";

        // userItemsの内容をログに出力
        if (userItems.Count > 0)
        {
            foreach (var item in userItems)
            {
                Debug.Log($"Item: {item.Key}, Amount: {item.Value}");
            }
        }
        else
        {
            Debug.LogWarning("ユーザーアイテムが保存されていません。");
        }

        // アイテム1とアイテム2の名前と個数を初期化
        string item1Name = "No Item";
        int item1Amount = 0;
        string item2Name = "No Item";
        int item2Amount = 0;

        // userItemsから最初のアイテムを取得
        if (userItems.Count > 0)
        {
            var firstItem = userItems.First(); // 最初のアイテム (KeyValuePair)
            item1Name = firstItem.Key; // アイテム名
            item1Amount = firstItem.Value; // アイテムの個数
            Debug.Log($"Item 1 - Name: {item1Name}, Amount: {item1Amount}");

            // もし2番目のアイテムがあれば取得
            if (userItems.Count > 1)
            {
                var secondItem = userItems.Skip(1).First(); // 2番目のアイテム (KeyValuePair)
                item2Name = secondItem.Key;
                item2Amount = secondItem.Value;
                Debug.Log($"Item 2 - Name: {item2Name}, Amount: {item2Amount}");
            }
            else
            {
                Debug.Log("2番目のアイテムは存在しません。");
            }
        }
        
        addNumItem1 += consecutiveDays;
        addNumItem2 += consecutiveDays;

        // アイテム数を表示
        item1Text.text = $"Get {item1Name}: +{addNumItem1}";
        item2Text.text = $"Get {item2Name}: +{addNumItem2}";

        // アイテムの増加
        item1Amount += addNumItem1;
        item2Amount += addNumItem2;

        // 増加後のアイテム数を表示
        item1Text.text = $"Get {item1Name}: +{addNumItem1} (Total: {item1Amount})";
        item2Text.text = $"Get {item2Name}: +{addNumItem2} (Total: {item2Amount})";

        // ボーナスメッセージを表示
        bonusText.text = bonusMessage;

        // 増加後のアイテム数をログに表示
        Debug.Log($"Updated Item 1: {item1Name}: +{addNumItem1}, Total: {item1Amount}");
        Debug.Log($"Updated Item 2: {item2Name}: +{addNumItem2}, Total: {item2Amount}");

        // PlayerPrefs からユーザーIDを取得する
        userId = PlayerPrefs.GetInt("currentUserId", -1);  // デフォルト値として -1 を設定
        //1,2を付与するとして今回設計
        StartCoroutine(dataFetcher.PostItemToUser(userId,1,addNumItem1));
        StartCoroutine(dataFetcher.PostItemToUser(userId,2,addNumItem2));
        
    }

  



    // メインメニューへ戻る
    public void OnMainMenuButtonClicked()
    {
        mainMenuPage.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
