using UnityEngine;
using Complete;  // TankMovement クラスを使うために必要

public class ItemEffectHandler : MonoBehaviour
{
    private bool isMonitoring = true;

    [SerializeField] private float maxHealth = 130f;
    [SerializeField] private float speed = 15f;

    void Update()
    {
        // アイテム効果1がアクティブかどうかを確認
        if (ItemEffectManager.IsItemEffectActive(1)) 
        {
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


                }
            }
        }

        // アイテム効果2がアクティブかどうかを確認
        if (ItemEffectManager.IsItemEffectActive(2)) 
        {
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
                }
            }
        }
    // スクリプトのオブジェクトを削除
    Destroy(this.gameObject);
    }
}
