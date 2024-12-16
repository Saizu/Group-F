using UnityEngine;
using NativeWebSocket;
using System.Text;
using System;

public class WebSocketClient : MonoBehaviour{
    private WebSocket websocket;
    public delegate void OnReadyReceived(bool isReady);
    public event OnReadyReceived ReadyReceived;
    public delegate void OnStampReceived(int stampId);
    public event OnStampReceived StampReceived;

    [System.Serializable]
    public class MessageData{
        public string type; //stampかreadyか
        public int id; //stamp番号
        public bool state; // readyが0か1か
    }

    void Start(){
        websocket = new WebSocket("ws://localhost:63245/ws");

        websocket.OnOpen += () => Debug.Log("WebSocket Connected!");
        websocket.OnMessage += (bytes) =>{
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log($"Received message: {message}"); // ここで受け取ったメッセージを表示

         try{
            //json文字列をオブジェクト型に変換
            MessageData data = JsonUtility.FromJson<MessageData>(message);

            if (data.type == "stamp"){
                // StampReceivedイベントを発火
                StampReceived?.Invoke(data.id);
            }
            else if (data.type == "ready"){
                // ReadyReceivedイベントを発火
                ReadyReceived?.Invoke(data.state);
            }
            else{
                Debug.LogWarning($"Unknown message type: {data.type}");
            }
         }
         catch (System.Exception ex){
            Debug.LogError($"Failed to parse message: {ex.Message}");
         }
        };
        Connect();
        // StampReceivedイベントに対してイベントハンドラを追加
        StampReceived += HandleStampReceived;
        // ReadyReceivedイベントに対してイベントハンドラを追加
        ReadyReceived += HandleReadyReceived;
    }

    async void Connect(){
        await websocket.Connect();
    }

    public void SendStamp(int stampId) {    
        // MessageDataインスタンスを作成
        MessageData messageData = new MessageData {
            type = "stamp",   // typeを "stamp" に設定
            id = stampId,     // stampIdを設定
            state = false     // stateを false に設定
        };
            //json形式の文字列に変換
            string json = JsonUtility.ToJson(messageData);
            websocket.SendText(json);
    }
    public async void SendReadyState(bool readyState){
        string json = JsonUtility.ToJson(new { type = "ready", state = readyState });
        await websocket.SendText(json);
    }

     public void OnStamp1ButtonPressed(){
        SendStamp(1);
    }

     public void OnStamp2ButtonPressed(){
        SendStamp(2);
    }

    // StampReceivedイベントを処理するハンドラ
    private void HandleStampReceived(int stampId){
        Debug.Log($"Received Stamp ID: {stampId}");
    }

    // ReadyReceivedイベントを処理するハンドラ
    private void HandleReadyReceived(bool isReady){
        Debug.Log($"Ready state: {isReady}");
    }

    void Update(){
        #if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
        #endif
    }

    private async void OnApplicationQuit(){
        if (websocket != null){
            await websocket.Close();
        }
    }
}
