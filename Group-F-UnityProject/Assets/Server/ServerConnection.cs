using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class ServerConnection : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(FetchDataFromServer());
    }

    private IEnumerator FetchDataFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:63245/api/data");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response from server: " + request.downloadHandler.text);
            // 必要ならレスポンスを処理
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }
}
