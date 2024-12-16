using UnityEngine;
using TMPro;

public class UserInfoDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text userInfoText;

    private string userID = "None";
    private string userName = "NoName";

    // Start is called before the first frame update
    void Start()
    {
        UpdateUserInfoText();
    }

    public void SetUserInfo(string id, string name)
    {
        userID = id;
        userName = name;
        UpdateUserInfoText();
    }

    private void UpdateUserInfoText()
    {
        if (userInfoText != null)
        {
            userInfoText.text = $"User ID: {userID}, User Name: {userName}";
        }
    }
}
