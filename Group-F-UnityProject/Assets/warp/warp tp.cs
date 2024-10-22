using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    [SerializeField] private Transform targetWarpPoint; // ワープ先のポイント
    [SerializeField] private float cooldownTime = 2f; // クールタイムの時間
    private static bool isWarp1Active = true; // warp1のワープを制御するフラグ
    private static bool isWarp2Active = true; // warp2のワープを制御するフラグ
    private bool canWarp = true; // ワープ可能かどうかのフラグ

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがプレイヤーであるか確認
        if (collision.collider.CompareTag("Player"))
        {
            // warp1の場合
            if (gameObject.name == "warp1" && isWarp1Active && canWarp)
            {
                // プレイヤーの位置をワープ先のX,Z座標を保持しつつY座標を0にする
                Vector3 newPosition = new Vector3(targetWarpPoint.position.x, 0, targetWarpPoint.position.z);
                collision.transform.position = newPosition;
                Debug.Log(collision.gameObject.name + " が " + gameObject.name + " に衝突しました。ワープ先: " + targetWarpPoint.name);
                // ワープを無効化
                canWarp = false;

                // warp2のワープを無効化
                isWarp2Active = false;

                StartCoroutine(WarpCooldown("warp1"));
            }
            // warp2の場合
            else if (gameObject.name == "warp2" && isWarp2Active && canWarp)
            {
                // プレイヤーの位置をワープ先のX,Z座標を保持しつつY座標を0にする
                Vector3 newPosition = new Vector3(targetWarpPoint.position.x, 0, targetWarpPoint.position.z);
                collision.transform.position = newPosition;
                Debug.Log(collision.gameObject.name + " が " + gameObject.name + " に衝突しました。ワープ先: " + targetWarpPoint.name);
                // warp1を無効化
                isWarp1Active = false;

                StartCoroutine(WarpCooldown("warp2"));
            }
            // warp1からwarp2へのワープ中、warp1が無効の場合のメッセージ
            else if (gameObject.name == "warp2" && !isWarp1Active)
            {
                Debug.Log("warp1へのワープはクールタイム中です。");
            }
        }
        else
        {
            // プレイヤー以外のオブジェクトがワープポイントに衝突した場合のメッセージ
            Debug.Log("他のオブジェクトが " + gameObject.name + " に衝突しました: " + collision.gameObject.name);
        }
    }

    private System.Collections.IEnumerator WarpCooldown(string warpPoint)
    {
        // クールタイム中は待機
        yield return new WaitForSeconds(cooldownTime);
        
        // ワープポイントによって再び有効化
        if (warpPoint == "warp1")
        {
            isWarp2Active = true; 
            Debug.Log("warp2へのワープが再び可能になりました。");
        }
        else if (warpPoint == "warp2")
        {
            isWarp1Active = true;
            Debug.Log("warp1へのワープが再び可能になりました。");
        }

        // ワープを再び可能にする
        canWarp = true; 
    }
}
