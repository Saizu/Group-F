using UnityEngine;
using UnityEngine.UI;

public class PlayerStockArea : MonoBehaviour{
    // Shell1～Shell10のImageコンポーネントを参照するための配列
    [SerializeField] private Image[] shellImages = new Image[10];

    // Shells10～Shell40のImageコンポーネントを参照するための配列
    [SerializeField] private Image[] groupShellImages = new Image[4];
    public void UpdatePlayerStockArea(int stockCount){
      // 10発のアイコン (groupShellImages) の表示・非表示を制御
      int groupShellCount = stockCount / 10; // 10発ごとに1アイコン
      for (int i = 0; i < groupShellImages.Length; i++){
        if (groupShellImages[i] != null) {
            // 10発単位で表示する（10発ごとに1アイコンを表示）
            groupShellImages[i].gameObject.SetActive(i < groupShellCount);
        }
    }

    // 1発のアイコン (shellImages) の表示・非表示を制御
    int remainingShellCount = stockCount % 10; // 10発ごとに残りの1発のアイコンを表示
    for (int i = 0; i < shellImages.Length; i++){
        if (shellImages[i] != null) {
            // 1発ずつ表示する
            shellImages[i].gameObject.SetActive(i < remainingShellCount);
        }
    }
}

}