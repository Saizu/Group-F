using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject hpBar;
    const float HpBar_WidthRatio = 0.9f;
    private Image hpBarImage;

    void Start()
    {
        hpBarImage = hpBar.GetComponent<Image>();
        UpdateHP(1.0f);
    }

    public void UpdateWinCount(int count)
    {
        win.GetComponent<TextMeshProUGUI>().text = "Win:" + count.ToString();
    }

    public void UpdateHP(float rate)
    {
        var panelRectTransform = panel.GetComponent<RectTransform>();
        var newWidth = panelRectTransform.rect.width * panelRectTransform.localScale.x * rate * HpBar_WidthRatio;
        var sizeDelta = hpBar.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x = newWidth;
        hpBar.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        // HPの残量によってHPバーの色を変更する
        if (rate > 0.5f)
        {
            hpBarImage.color = Color.green; // 緑色
        }
        else if (rate > 0.2f)
        {
            hpBarImage.color = Color.yellow; // 黄色
        }
        else
        {
            hpBarImage.color = Color.red; // 赤色
        }
    }
}
