using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class AnnouncePageManager : MonoBehaviour
{
    [SerializeField] private GameObject m_mainMenuPage;
    [SerializeField] private GameObject m_text;

    private const string HOST_URL = "http://localhost:63245";

    void Start()
    {
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{HOST_URL}/announces/get/"))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"お知らせの取得に失敗: {webRequest.error}");
                yield break;
            }

            var response = JsonUtility.FromJson<Announces>(webRequest.downloadHandler.text);
            if (response == null || response.announces == null)
            {
                Debug.LogError("取得したお知らせのパースに失敗。");
                yield break;
            }
            if (response.announces.Length == 0)
            {
                m_text.GetComponent<TextMeshProUGUI>().text = "お知らせがありません。";
                yield break;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var n in response.announces)
            {
                sb.Append($"{n.Title} ({n.Time})\n{n.Body}\n\n");
            }
            m_text.GetComponent<TextMeshProUGUI>().text = sb.ToString();
        }
    }

    /// 閉じるボタンが押されたときに呼ばれるメソッド。
    ///
    /// このスクリプトがアタッチされているオブジェクトを無効にし、メインメニュー画面を有効にする。
    public void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
        m_mainMenuPage.SetActive(true);
    }
}

[System.Serializable]
public class Announce
{
    public int ID;
    public string Title;
    public string Body;
    public string Time;
}

[System.Serializable]
public class Announces
{
    public Announce[] announces;
}
