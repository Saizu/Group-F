using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ホーム画面へ遷移するためのボタンのためのスクリプト。
/// </summary>
/// <remarks>
/// このスクリプトを何らかのオブジェクトにアタッチし、buttonフィールドにボタンを登録してください。
/// </remarks>
public class ToHomeButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    void Start()
    {
        button.onClick.AddListener(() => {
            SceneManager.LoadScene("HomeScene");
        });
    }
}
