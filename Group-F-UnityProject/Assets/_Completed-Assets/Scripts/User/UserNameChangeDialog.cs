// ユーザー名変更のUI
using UnityEngine;
using UnityEngine.UI;

public class UserNameChangeDialog : MonoBehaviour
{
    public UserManager userManager;
    public TMPro.TMP_InputField nameInputField;
    public TMPro.TextMeshProUGUI feedbackText;

    public void OnChangeButtonClicked()
    {
        string newName = nameInputField.text;

        if (userManager.SetUserName(newName))
        {
            feedbackText.text = "名前が変更されました: " + newName;
            this.gameObject.SetActive(false); // ダイアログを閉じる
        }
        else
        {
            feedbackText.text = "無効な名前です。3〜15文字で記号を含めないでください。";
        }
    }
}
