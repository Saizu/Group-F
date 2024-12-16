using UnityEngine;

public class ChangeNameButton : MonoBehaviour
{
    public GameObject changeUserNameDialog; // ダイアログの参照

    public void OnChangeNameButtonClicked()
    {
        if (changeUserNameDialog != null)
        {
            // 現在のアクティブ状態を反転させる
            bool isActive = changeUserNameDialog.activeSelf;
            changeUserNameDialog.SetActive(!isActive);
        }
    }
}
