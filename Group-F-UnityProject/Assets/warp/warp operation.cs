using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warpoperation : MonoBehaviour
{
    [SerializeField] private GameObject warp1;
    [SerializeField] private GameObject warp2;

    private GameObject player1;
    private GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        // 1フレーム待ってからプレイヤーを取得する
        StartCoroutine(DelayedPlayerInitialization());
    }

    // プレイヤーを1フレーム遅れて取得するコルーチン
    private IEnumerator DelayedPlayerInitialization()
    {
        // 1フレーム待つ
        yield return new WaitForEndOfFrame();

        // "Player" タグがついているすべてのオブジェクトを取得する
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // プレイヤーが2つ存在するか確認して、それぞれに割り当てる
        if (players.Length >= 2)
        {
            player1 = players[0];  // 最初のプレイヤー
            player2 = players[1];  // 2番目のプレイヤー
            // プレイヤーが正常に取得できた場合、デバッグメッセージを表示
            Debug.Log("Player1 と Player2 が正常に取得されました: " + player1.name + ", " + player2.name);
        }
        else
        {
            Debug.LogError("プレイヤーが2体見つかりません。正しいタグが付いているか確認してください。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ワープポイントにプレイヤーが触れた場合に呼び出される
    private void OnTriggerEnter(Collider other)
    {

        // 通過したオブジェクトがプレイヤーかどうかを確認
        if (other.gameObject == player1 || other.gameObject == player2)
        {
            Debug.Log("プレイヤーがワープポイントを通過しました: " + other.gameObject.name);
        }
        else
        {
            // プレイヤー以外のオブジェクトがワープポイントに触れた場合のメッセージ
            Debug.Log("他のオブジェクトがワープポイントに触れました: " + other.gameObject.name);
        }



        if (player1 != null && player2 != null)
        {
            // player1がwarp1に触れた場合
            if (other.gameObject == player1 && this.gameObject == warp1)
            {
                player1.transform.position = warp2.transform.position; // warp2に瞬時に移動
                Debug.Log("Player 1 が Warp 1 に触れました。Warp 2 に移動します。");
            }
            // player1がwarp2に触れた場合
            else if (other.gameObject == player1 && this.gameObject == warp2)
            {
                player1.transform.position = warp1.transform.position; // warp1に瞬時に移動
                Debug.Log("Player 1 が Warp 2 に触れました。Warp 1 に移動します。");
            }
            // player2がwarp1に触れた場合
            else if (other.gameObject == player2 && this.gameObject == warp1)
            {
                player2.transform.position = warp2.transform.position; // warp2に瞬時に移動
                Debug.Log("Player 2 が Warp 1 に触れました。Warp 2 に移動します。");
            }
            // player2がwarp2に触れた場合
            else if (other.gameObject == player2 && this.gameObject == warp2)
            {
                player2.transform.position = warp1.transform.position; // warp1に瞬時に移動
                Debug.Log("Player 2 が Warp 2 に触れました。Warp 1 に移動します。");
            }
        }
    }
}
