// デバッグ用　実際には使わない
using UnityEngine;

public class UserExample : MonoBehaviour
{
    [SerializeField] private UserInfoDisplay userInfoDisplay;

    void Start()
    {
        userInfoDisplay.SetUserInfo("12345", "Taro Yamada");
    }
}
