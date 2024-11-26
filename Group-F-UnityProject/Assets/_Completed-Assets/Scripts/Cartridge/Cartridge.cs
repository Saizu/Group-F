using UnityEngine;
using System.Collections;

public class Cartridge : MonoBehaviour{
    private Renderer m_Renderer;        // Rendererコンポーネントの参照
    public float blinkDuration = 3.0f;  // 明滅させる期間
    public float blinkInterval = 1.0f;  // 明滅の間隔

    private void Start(){
        // Rendererコンポーネントを取得
        m_Renderer = GetComponent<Renderer>();
        
        // 明滅させるコルーチンを開始
        StartCoroutine(BlinkAndDestroy());
    }

    private IEnumerator BlinkAndDestroy(){
        float elapsedTime = 0f;

        // 指定した期間中、明滅を繰り返す
        while (elapsedTime < blinkDuration){
            // Rendererを有効・無効を切り替え
            m_Renderer.enabled = !m_Renderer.enabled;
            
            // 次の切り替えまで待機
            yield return new WaitForSeconds(blinkInterval);

            // 経過時間を更新
            elapsedTime += blinkInterval;
        }

        // 最後にRendererを有効にして消滅
        m_Renderer.enabled = true;
        Destroy(gameObject);
    }
}
