// ユーザー情報を取得、UIを更新する
using UnityEngine;
using TMPro;

public class UserHeaderUI : MonoBehaviour
{
    public UserManager userManager; // UserManagerの参照
    public TextMeshProUGUI userIdText; // ユーザーID表示用
    public TextMeshProUGUI userNameText; // ユーザー名表示用

    void Start()
    {
        UpdateUserInfo();
    }

    // ユーザー情報を更新
    public void UpdateUserInfo()
    {
        if (userManager != null)
        {
            userIdText.text = "User ID: " + (userManager.UserId ?? "None");
            userNameText.text = "User Name: " + (userManager.UserName ?? "NoName");
        }
    }
}
