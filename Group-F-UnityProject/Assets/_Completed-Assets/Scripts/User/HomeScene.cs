using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScene : MonoBehaviour
{
    public void OnStartGameButtonClicked(){

        SceneManager.LoadScene("_Complete-Game");
        
    }
}
