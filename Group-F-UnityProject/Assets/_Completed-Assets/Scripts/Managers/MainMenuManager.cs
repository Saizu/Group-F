using UnityEngine;
using TMPro;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public GameObject itemPage; // アイテム表示画面
    public GameObject gamePage; // ゲーム画面
    public TMP_Text staminaText;
    public GameObject mainMenuPage; // メインメニュー画面

    private DataFetcher dataFetcher;
    int userId = -1;
    void Start()
    {
        dataFetcher = GetComponent<DataFetcher>();
        AppState.CurrentPage = "Main";
        Debug.Log($"Current Page: {AppState.CurrentPage}");

        userId = PlayerPrefs.GetInt("currentUserId", -1);
        StartCoroutine(UpdateStaminaText(userId));
    }

    void Update(){
        StartCoroutine(UpdateStaminaText(userId));
    }


    private IEnumerator UpdateStaminaText(int userId)
    {
        // データをフェッチして少し待つ
        yield return null; // 必要に応じてここで待機処理を追加

        int nowStamina = dataFetcher.GetSavedUserStamina(userId);

        // テキストを更新
        staminaText.text = $"Now Stamina {nowStamina} / 5";
    }

    // アイテムページへ移動するボタン
    public void OnItemsButtonClicked()
    {
        // メインメニューを非表示にして、アイテムページを表示
        mainMenuPage.SetActive(false);
        itemPage.SetActive(true);
    }

    // ゲームへ移動するボタン
    public void OnGameButtonClicked()
    {
        // メインメニューを非表示にして、ゲーム画面を表示
        mainMenuPage.SetActive(false);
        gamePage.SetActive(true);
    }

    // ゲームを終了するボタン
    public void OnExitButtonClicked()
    {
        // エディタで実行中の場合、エディタを停止
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // ビルドされたアプリケーションで終了
            Application.Quit();
        #endif
    }
}
