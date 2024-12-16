using UnityEngine;
using TMPro;

public class UserNameChangeDialog : MonoBehaviour
{
    public UserManager userManager; // ユーザー情報を管理
    public TMPro.TMP_InputField nameInputField; // ユーザー名入力フィールド
    public TMPro.TextMeshProUGUI feedbackText; // フィードバック表示
    public UserHeaderUI userHeaderUI; // ユーザー情報UI更新用

    public void OnChangeButtonClicked()
    {
        string newName = nameInputField.text;

        if (userManager.SetUserName(newName))
        {
            // 成功時のフィードバックとUI更新
            feedbackText.text = $"The name has been changed: {newName}";
            userHeaderUI.UpdateUserInfo();
        }
        else
        {
            // エラーメッセージを表示
            feedbackText.text = "Failed to set the name. Please use 3 to 15 characters without special symbols.";
        }
    }
}
