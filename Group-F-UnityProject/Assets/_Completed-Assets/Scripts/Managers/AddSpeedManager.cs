using UnityEngine;
using Complete;  // TankMovement クラスを使うために必要

public class AddSpeedManager : MonoBehaviour
{
    private bool isMonitoring = true;

    // インスペクタで速度を変更できるようにする
    [SerializeField] private float speed = 15f;

    void Update()
    {
        // Playerタグを検索し、そのオブジェクトを持ってくる
        if (isMonitoring && AppState.CurrentPage == "Game")
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Debug.Log("Playerオブジェクトを見つけました: " + playerObject.name);

                // TankMovement コンポーネントを取得して m_Speed を変更
                TankMovement movement = playerObject.GetComponent<TankMovement>();
                if (movement != null)
                {
                    // m_Speed をインスペクタで設定したspeedに変更
                    movement.m_Speed = speed;
                    Debug.Log("PlayerのSpeedを" + speed + "に変更しました。");
                }
                else
                {
                    Debug.LogWarning("TankMovement コンポーネントが見つかりませんでした。");
                }

                 Destroy(this.gameObject); // このスクリプトがアタッチされているオブジェクトを削除
            }
        }
    }
}
