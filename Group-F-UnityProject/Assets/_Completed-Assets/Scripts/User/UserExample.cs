using UnityEngine;

public class UserExample : MonoBehaviour
{
    [SerializeField] private UserInfoDisplay userInfoDisplay;

    void Start()
    {
        // 値を動的に設定
        userInfoDisplay.SetUserInfo("12345", "Taro Yamada");
    }
}
