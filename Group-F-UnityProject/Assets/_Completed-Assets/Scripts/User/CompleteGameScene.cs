//　デバッグ用
using UnityEngine;

public class CompleteGameScene : MonoBehaviour
{
    public UserManager userManager;

    void Start()
    {
        Debug.Log($"CompleteGameScene: UserID = {userManager.UserId}, UserName = {userManager.UserName}");
    }
}