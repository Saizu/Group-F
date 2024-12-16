using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject win;
    [SerializeField]
    private GameObject hpBar;
    [SerializeField]
    private GameObject overHpBar;

    [SerializeField]
    private GameObject shellImage;
    [SerializeField]
    private GameObject shellGroupImage;

    private GameObject[] shellImages;
    private GameObject[] shellGroupImages;

    void Start()
    {
        UpdateHP(1.0f);
    }

    public void UpdateWinCount(int count)
    {
        win.GetComponent<TextMeshProUGUI>().text = "Win:" + count.ToString();
    }

    public void UpdateHP(float rate)
    {
        var panelRectTransform = panel.GetComponent<RectTransform>();
        var newWidth = panelRectTransform.rect.width * panelRectTransform.localScale.x * rate * 0.9f;
        var sizeDelta = hpBar.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x = newWidth;
        hpBar.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }

    public void UpdateOverHP(float rate)
    {
        var panelRectTransform = panel.GetComponent<RectTransform>();
        var newWidth = panelRectTransform.rect.width * panelRectTransform.localScale.x * rate * 0.9f;
        var sizeDelta = overHpBar.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x = newWidth;
        overHpBar.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }

    public void UpdateStock(int stockCount)
    {
        if (shellImages != null)
        {
            foreach (var n in shellImages)
            {
                Destroy(n);
            }
        }
        if (shellGroupImages != null)
        {
            foreach (var n in shellGroupImages)
            {
                Destroy(n);
            }
        }

        shellImages = null;
        shellGroupImages = null;

        if (stockCount % 10 > 0)
        {
            shellImages = new GameObject[stockCount % 10];
            for (int i = 0; i < shellImages.Length; ++i)
            {
                var n = Instantiate(shellImage);
                n.transform.SetParent(this.gameObject.transform, false);
                n.GetComponent<RectTransform>().anchoredPosition = new Vector3(-170.0f + 19.0f * i, -25.0f, 0.0f);
                shellImages[i] = n;
            }
        }
        if (stockCount / 10 > 0)
        {
            shellGroupImages = new GameObject[stockCount / 10];
            for (int i = 0; i < shellGroupImages.Length; ++i)
            {
                var n = Instantiate(shellGroupImage);
                n.transform.SetParent(this.gameObject.transform, false);
                n.GetComponent<RectTransform>().anchoredPosition = new Vector3(20.0f + 25.0f * i, -25.0f, 0.0f);
                shellGroupImages[i] = n;
            }
        }
    }
}
