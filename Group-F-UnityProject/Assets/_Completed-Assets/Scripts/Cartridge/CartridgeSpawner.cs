using UnityEngine;
using System.Collections;

namespace Complete
{
    public class CartridgeSpawner : MonoBehaviour
    {
        [SerializeField] private CartridgeData[] cartridgeDataArray; // Cartridgeプレハブを割り当てる変数
        public Vector3 spawnAreaMin = new Vector3(-10, 0, -10);    // スポーンエリアの最小座標
        public Vector3 spawnAreaMax = new Vector3(10, 0, 10);      // スポーンエリアの最大座標
        private Coroutine m_SpawnRoutine; // コルーチンの参照を保持する変数
        private GameManager m_GameManager; // GameManagerオブジェクトの参照を保持

        private void Start()
        {
            // GameManagerオブジェクトを探し、参照を取得
            m_GameManager = FindObjectOfType<GameManager>();

            // GameManagerのGameStateChangedイベントにHandleGameStateChangedメソッドを登録
            if (m_GameManager != null)
            {
                m_GameManager.GameStateChanged += HandleGameStateChanged;
            }
        }

        private void OnDestroy()
        {
            // イベントの解除
            if (m_GameManager != null)
            {
                m_GameManager.GameStateChanged -= HandleGameStateChanged;
            }
        }

        // ランダムな位置にShellCartridgeプレハブを生成
        private void SpawnCartridge(CartridgeData cartridgeData)
        {
            // ランダムな位置を生成
            cartridgeData.spawnPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                spawnAreaMin.y,
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            // プレハブの生成
            Instantiate(cartridgeData.cartridgePrefab, cartridgeData.spawnPosition, Quaternion.identity);
        }

        // 定期的にSpawnCartridgeメソッドを呼び出すコルーチン
        private IEnumerator SpawnRoutine(CartridgeData cartridgeData)
        {
            while (true)
            {
                // 指定されたスポーン頻度で待機
                yield return new WaitForSeconds(cartridgeData.spawnFrequency);

                // カートリッジをスポーン
                SpawnCartridge(cartridgeData);
            }
        }

        //ゲーム状態が変わったときにコルーチンを開始・停止するメソッド
        private void HandleGameStateChanged(Complete.GameManager.GameState newState)
        {
            if (newState == Complete.GameManager.GameState.RoundPlaying)
            {
                // ゲームがプレイ中なら、SpawnRoutineを開始
                if (m_SpawnRoutine == null && cartridgeDataArray != null)
                {
                    foreach (var cartridgeData in cartridgeDataArray)
                    {
                        if (cartridgeData != null && cartridgeData.cartridgePrefab != null)
                        {
                            m_SpawnRoutine = StartCoroutine(SpawnRoutine(cartridgeData));
                        }
                    }
                }
            }
            else
            {
                // ゲームがプレイ中でないなら、SpawnRoutineを停止
                if (m_SpawnRoutine != null)
                {
                    StopCoroutine(m_SpawnRoutine);
                    m_SpawnRoutine = null;
                }
            }
        }
    }
}