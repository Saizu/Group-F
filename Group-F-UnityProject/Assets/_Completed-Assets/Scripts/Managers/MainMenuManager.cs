using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject itemPage; // アイテム表示画面
    public GameObject gamePage; // ゲーム画面
    public GameObject mainMenuPage; // メインメニュー画面


    void Start(){
        AppState.CurrentPage = "Main";
        Debug.Log($"Current Page: {AppState.CurrentPage}");
        
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
