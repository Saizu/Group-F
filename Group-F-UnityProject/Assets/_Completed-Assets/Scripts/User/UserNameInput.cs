using UnityEngine;
using UnityEngine.UI;

public class UserNameInput : MonoBehaviour
{
    public UserManager userManager;
    public TMPro.TMP_InputField nameInputField; // ユーザー名入力フィールド
    public TMPro.TextMeshProUGUI feedbackText; // ユーザーへのフィードバック表示

    public void OnConfirmButtonClicked()
    {
        string newName = nameInputField.text;

        if (userManager.SetUserName(newName))
        {
            feedbackText.text = "名前が設定されました: " + newName;
        }
        else
        {
            feedbackText.text = "名前の設定に失敗しました。3〜15文字で、特殊文字は使えません。";
        }
    }
}
