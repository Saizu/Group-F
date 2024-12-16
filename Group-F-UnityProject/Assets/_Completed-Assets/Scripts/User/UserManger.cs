// ユーザー管理スクリプト
using UnityEngine;

public class UserManager : MonoBehaviour
{
    // ユーザー情報保存
    private const string UserIdKey = "UserId";
    private const string UserNameKey = "UserName";

    public string UserId { get; private set; }
    public string UserName { get; private set; }

    private void Awake()
    {
        // ユーザー情報の複数生成を防ぐ
        if (FindObjectsOfType<UserManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // ロード
        LoadUser();
    }

    // 新規ユーザー作成
    public void CreateUser()
    {
        if (string.IsNullOrEmpty(UserId))
        {
            UserId = System.Guid.NewGuid().ToString();
        }

        if (string.IsNullOrEmpty(UserName))
        {
            UserName = "NoName";
        }

        SaveUser();
    }

    // ユーザー情報読み込み
    private void LoadUser()
    {
        UserId = PlayerPrefs.GetString(UserIdKey, null);
        UserName = PlayerPrefs.GetString(UserNameKey, "NoName");
    }

    // ユーザー情報保存
    public void SaveUser()
    {
        PlayerPrefs.SetString(UserIdKey, UserId);
        PlayerPrefs.SetString(UserNameKey, UserName);
        PlayerPrefs.Save();
    }

    public bool IsUserCreated()
    {
        return !string.IsNullOrEmpty(UserId);
    }

    public bool SetUserName(string newName)
    {
        if (ValidateUserName(newName))
        {
            UserName = newName;
            SaveUser();
            return true;
        }

        return false;
    }
    
    // ユーザー名の検証
    private bool ValidateUserName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        if (name.Length < 3 || name.Length > 15) return false;

        foreach (char c in name)
        {
            if (!char.IsLetterOrDigit(c) && c != ' ' && !char.IsHighSurrogate(c))
            {
                return false; // 記号を排除
            }
        }

        return true;
    }
}
