// タイトル画面スクリプト
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public UserManager userManager; 
    public TMPro.TextMeshProUGUI userIdText; 

    void Start()
    {
        if (userManager.IsUserCreated())
        {
            userIdText.text = "User ID: " + userManager.UserId; 
        }
        else
        {
            userIdText.text = ""; 
        }
    }

    // 「Tap To Start」がタップされたとき
    public void OnTapToStart()
    {
        if (!userManager.IsUserCreated())
        {
            userManager.CreateUser(); 
        }

        SceneManager.LoadScene(""); 
    }
}
