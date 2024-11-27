using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject hpBar;

    void Start()
    {
        UpdateHP(1.0f);
    }

    public void UpdateHP(float rate)
    {
        var panelRectTransform = panel.GetComponent<RectTransform>();
        var newWidth = panelRectTransform.rect.width * panelRectTransform.localScale.x * rate * 0.9f;
        var sizeDelta = hpBar.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x = newWidth;
        hpBar.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }
}
