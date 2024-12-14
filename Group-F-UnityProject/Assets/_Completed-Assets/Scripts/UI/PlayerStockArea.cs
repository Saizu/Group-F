using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStockArea : MonoBehaviour
{
    [SerializeField] private Image[] singleShells; // Shell1, Shell2, ..., Shell10
    [SerializeField] private Image[] groupedShells; // Shells10, Shells20, Shells30, Shells40
    [SerializeField] private Image[] mines; // Mine1, Mine2, Mine3
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
        if (hpBarImage == null)
        {
            return;
        }
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

    public void UpdatePlayerStockArea(Dictionary<string, WeaponStockData> weaponStockDictionary)
    {
        if (weaponStockDictionary.TryGetValue("Shell", out WeaponStockData shellData))
        {
            for (int i = 0; i < singleShells.Length; i++)
            {
                int shellNum;
                if(shellData.CurrentCount == 0){
                    shellNum = 0;
                }else{
                    shellNum = shellData.CurrentCount - (shellData.CurrentCount - 1) / 10 * 10;
                }

                if (i < shellNum)
                {
                    singleShells[i].gameObject.SetActive(true);
                }
                else
                {
                    singleShells[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < groupedShells.Length; i++)
            {
                int groupThreshold = (shellData.CurrentCount - 1) / 10;

                if (i < groupThreshold)
                {
                    groupedShells[i].gameObject.SetActive(true);
                }
                else
                {
                    groupedShells[i].gameObject.SetActive(false);
                }
            }
        }
        if (weaponStockDictionary.TryGetValue("Mine", out WeaponStockData mineData))
        {
            for (int i = 0; i < mines.Length; i++)
            {
                if (i < mineData.CurrentCount)
                {
                    mines[i].gameObject.SetActive(true);
                }
                else
                {
                    mines[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
