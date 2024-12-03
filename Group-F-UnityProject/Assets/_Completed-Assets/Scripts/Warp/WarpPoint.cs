using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    [SerializeField] private GameObject targetWarpPoint; // ワープ先のポイント
    [SerializeField] private float cooldownTime = 2f; // クールタイムの時間

    private bool canWarp = true;

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがプレイヤーであるか確認
        if (collision.collider.CompareTag("Player"))
        {
            // ワープ可能ならワープする
            if (canWarp)
            {
                // プレイヤーの位置をワープ先のX,Z座標を保持しつつY座標を0にする
                Vector3 newPosition = new Vector3(targetWarpPoint.transform.position.x, 0, targetWarpPoint.transform.position.z);
                collision.transform.position = newPosition;
                Debug.Log(collision.gameObject.name + " が " + gameObject.name + " に衝突しました。ワープ先: " + targetWarpPoint.name);

                // ワープを無効化
                canWarp = false;
                // ワープ先も無効化
                targetWarpPoint.GetComponent<WarpPoint>().Disable();

                StartCoroutine(WarpCooldown());
            }
            // ワープ不可ならメッセージ
            else
            {
                Debug.Log("ワープはクールタイム中です。");
            }
        }
        else
        {
            // プレイヤー以外のオブジェクトがワープポイントに衝突した場合のメッセージ
            Debug.Log("他のオブジェクトが " + gameObject.name + " に衝突しました: " + collision.gameObject.name);
        }
    }

    private System.Collections.IEnumerator WarpCooldown()
    {
        // クールタイム中は待機
        yield return new WaitForSeconds(cooldownTime);
        
        // ワープを再び可能にする
        canWarp = true; 
        // ワープ先も可能にする
        targetWarpPoint.GetComponent<WarpPoint>().Enable();

        Debug.Log("ワープが再び可能になりました。");
    }

    public void Enable() => canWarp = true;
    public void Disable() => canWarp = false;
}
