using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StampManager : MonoBehaviour{
    [SerializeField] private Button[] stampButtons;
    [SerializeField] private Image displayStampArea;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float displayDuration = 5.0f;
    [SerializeField] private Image[] stampImages;

    private Coroutine currentFadeRoutine;

    private void Start(){
        // スタンプボタンにクリックイベントを登録
        for (int i = 0; i < stampButtons.Length; i++){
            int index = i;
            stampButtons[i].onClick.AddListener(() => OnStampClicked(index));
        }

        // 初期状態を非表示に
        displayStampArea.gameObject.SetActive(false);
    }

    private void OnStampClicked(int stampId){
        ShowStamp(stampImages[stampId].sprite);
    }

    private void ShowStamp(Sprite stampSprite){
        if (currentFadeRoutine != null){
            StopCoroutine(currentFadeRoutine);
        }

        displayStampArea.sprite = stampSprite;
        displayStampArea.color = new Color(1, 1, 1, 1);
        displayStampArea.gameObject.SetActive(true);

        currentFadeRoutine = StartCoroutine(FadeOutStamp());
    }

    private IEnumerator FadeOutStamp(){
        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0f;
        Color startColor = displayStampArea.color;
        while (elapsedTime < fadeDuration){
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            displayStampArea.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        displayStampArea.gameObject.SetActive(false);
    }
}
