using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField userNameInput; // ユーザー名入力フィールド
    public TMP_InputField passwordInput; // パスワード入力フィールド
    public TMP_Text errorMessageText; // エラーメッセージ表示用

    public GameObject loginPage; // ログインページ
    public GameObject bonusPage; // ボーナスページ
    public GameObject mainMenuPage; // メインメニューページ

    private string currentUser; // 現在ログイン中のユーザー名

    void Start()
    {
        AppState.CurrentPage = "Login";
        Debug.Log($"Current Page: {AppState.CurrentPage}");
        loginPage.SetActive(true);
        bonusPage.SetActive(false);
        mainMenuPage.SetActive(false); // メインメニューは最初は非表示に
        errorMessageText.text = ""; // 初期状態ではエラーメッセージを非表示
    }

    public void OnLoginButtonClicked()
    {
        string userName = userNameInput.text;
        string password = passwordInput.text;

        if (ValidateUser(userName, password))
        {
            errorMessageText.text = "";

            // 現在のユーザー名を保存
            currentUser = userName;
            PlayerPrefs.SetString("currentUser", currentUser);

            // 前回のログイン日付を取得
            string lastLogin = PlayerPrefs.GetString(userName + "_lastLogin", "");
            string currentLogin = System.DateTime.Now.ToString();

            // 初ログインまたは日付が異なる場合はボーナスページへ
            if (string.IsNullOrEmpty(lastLogin) || lastLogin.Split(' ')[0] != currentLogin.Split(' ')[0])
            {
                // ボーナスページへ遷移
                bonusPage.SetActive(true);
                bonusPage.GetComponent<BonusManager>().OnBonusPageShown();
                loginPage.SetActive(false);
            }
            else
            {
                // 1日以内に再ログインしている場合はメインメニューに遷移
                mainMenuPage.SetActive(true);
                loginPage.SetActive(false);
                Debug.Log("Bonus not applicable today. Showing Main Menu.");
            }

            // ログイン日付を更新
            PlayerPrefs.SetString(userName + "_lastLogin", currentLogin);
            PlayerPrefs.Save();
        }
        else
        {
            errorMessageText.text = "Invalid username or password.";
        }
    }

    public void OnRegisterButtonClicked()
    {
        string userName = userNameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            errorMessageText.text = "Please enter a username and password.";
            return;
        }

        if (PlayerPrefs.HasKey(userName + "_password"))
        {
            errorMessageText.text = "Username already exists.";
        }
        else
        {
            PlayerPrefs.SetString(userName + "_password", password);
            PlayerPrefs.SetString(userName + "_lastLogin", "");
            PlayerPrefs.SetInt(userName + "_item1", 3);
            PlayerPrefs.SetInt(userName + "_item2", 3);
            PlayerPrefs.Save();

            errorMessageText.text = "Account created successfully. Please log in.";
            userNameInput.text = "";
            passwordInput.text = "";
        }
    }

    private bool ValidateUser(string userName, string password)
    {
        string savedPassword = PlayerPrefs.GetString(userName + "_password", "");
        return savedPassword == password;
    }
}
