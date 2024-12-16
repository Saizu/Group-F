using UnityEngine;
using Complete;  // TankHealth クラスを使うために必要

public class AddHPManager : MonoBehaviour
{
    private bool isMonitoring = true;

    // インスペクタに表示するために[SerializedField]を使う
    [SerializeField] private float maxHealth = 130f;

    void Update()
    {
        // Playerタグを検索し、そのオブジェクトを持ってくる
        if (isMonitoring && AppState.CurrentPage == "Game")
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Debug.Log("Playerオブジェクトを見つけました: " + playerObject.name);

                // TankHealth コンポーネントを取得
                TankHealth health = playerObject.GetComponent<TankHealth>();
                if (health != null)
                {
                    // TankHealthの初期値をmaxHealthで設定
                    health.m_StartingHealth = maxHealth;
                    Debug.Log("PlayerのStartingHealthを " + maxHealth + " に変更しました。");
                }
                else
                {
                    Debug.LogWarning("TankHealth コンポーネントが見つかりませんでした。");
                }
                 Destroy(this.gameObject); // このスクリプトがアタッチされているオブジェクトを削除
                
            }
        }
    }
}
