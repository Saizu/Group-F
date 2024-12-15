using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class InquiryPageManager : MonoBehaviour
{
    [SerializeField] private GameObject m_mainMenuPage;

    [SerializeField] private GameObject m_form;
    [SerializeField] private GameObject m_title;
    [SerializeField] private GameObject m_body;

    [SerializeField] private GameObject m_scrollView;
    [SerializeField] private GameObject m_text;

    private const string HOST_URL = "http://localhost:63245";

    private IEnumerator PostInquiry()
    {
        using (UnityWebRequest webRequest = new UnityWebRequest($"{HOST_URL}/inquiries/post/", "POST"))
        {
            string title = m_title.GetComponent<TMP_InputField>().text;
            string body = m_body.GetComponent<TMP_InputField>().text;
            // TODO: ユーザIDを適切に設定する。
            string json = $"{{\"usrid\":{1},\"title\":\"{title}\",\"body\":\"{body}\"}}";
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"問い合わせの送信に失敗: {webRequest.error}");
                yield break;
            }

            Debug.Log("問い合わせの送信に成功");

            // NOTE: 本来ならこのメソッドの範囲外だが、成功・失敗の判定が面倒なので。
            m_title.GetComponent<TMP_InputField>().text = "";
            m_body.GetComponent<TMP_InputField>().text = "";
        }
    }

    private IEnumerator GetInquiries()
    {
        // TODO: 適切にユーザIDを設定する。
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{HOST_URL}/inquiries/get-by-usrid/?usrid={1}"))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"問い合わせの送信に失敗: {webRequest.error}");
                yield break;
            }

            var response = JsonUtility.FromJson<Inquiries>(webRequest.downloadHandler.text);
            if (response == null || response.inquiries == null)
            {
                Debug.LogError("取得したお問い合わせのパースに失敗。");
                yield break;
            }
            if (response.inquiries.Length == 0)
            {
                m_text.GetComponent<TextMeshProUGUI>().text = "No inquiries.";
                yield break;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var n in response.inquiries)
            {
                sb.Append($"{n.Title} ({n.Time})\n{n.Body}\n");
                if (n.Reply.Valid)
                {
                    sb.Append($"[[Reply]]\n{n.Reply.String}\n");
                }
                sb.Append("\n");
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

    /// Newボタンが押されたときに呼ばれるメソッド。
    ///
    /// リストを無効にし、フォームを有効にする。
    public void OnNewButtonClicked()
    {
        m_scrollView.SetActive(false);
        m_form.SetActive(true);
    }

    /// Sendボタンが押されたときに呼ばれるメソッド。
    ///
    /// お問い合わせを送信し、入力をクリアする。
    public async void OnSendButtonClicked()
    {
        StartCoroutine(PostInquiry());
    }

    /// Newボタンが押されたときに呼ばれるメソッド。
    ///
    /// フォームを無効にし、リストを有効にし、お問い合わせを取得する。
    public void OnListButtonClicked()
    {
        m_form.SetActive(false);
        m_scrollView.SetActive(true);
        StartCoroutine(GetInquiries());
    }
}


[System.Serializable]
public class InquiryReply
{
    public string String;
    public bool Valid;
}

[System.Serializable]
public class Inquiry
{
    public int ID;
    public int Usrid;
    public string Title;
    public string Body;
    public string Time;
    public InquiryReply Reply;
}

[System.Serializable]
public class Inquiries
{
    public Inquiry[] inquiries;
}
