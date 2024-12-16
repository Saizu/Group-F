using UnityEngine;

public class ChangeNameButton : MonoBehaviour
{
    public GameObject changeUserNameDialog; // ダイアログの参照

    public void OnChangeNameButtonClicked()
    {
        if (changeUserNameDialog != null)
        {
            changeUserNameDialog.SetActive(true); // ダイアログを表示
        }
        else
        {
            Debug.LogError("ChangeUserNameDialog is not assigned!");
        }
    }
}
