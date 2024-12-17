using UnityEngine;

public class ResetUser : MonoBehaviour
{
    public UserManager userManager;
    public UserHeaderUI userHeaderUI; // ユーザー情報UI更新用


    public void OnResetUserButtonClicked()
    {
        userManager.ResetUser();
        userHeaderUI.UpdateUserInfo();
    }
}
