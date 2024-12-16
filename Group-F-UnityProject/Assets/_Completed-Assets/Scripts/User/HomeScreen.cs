using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("_Complete-Game"); // ゲーム画面のシーン名
    }
}
