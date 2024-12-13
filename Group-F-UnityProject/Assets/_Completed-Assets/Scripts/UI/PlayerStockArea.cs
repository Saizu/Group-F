using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStockArea : MonoBehaviour
{
    [SerializeField] private Image[] singleShells; // Shell1, Shell2, ..., Shell10
    [SerializeField] private Image[] groupedShells; // Shells10, Shells20, Shells30, Shells40
    [SerializeField] private Image[] mines; // Mine1, Mine2, Mine3

    public void UpdatePlayerStockArea(Dictionary<string, WeaponStockData> weaponStockDictionary)
    {
        if (weaponStockDictionary.TryGetValue("Shell", out WeaponStockData shellData))
        {
            for (int i = 0; i < singleShells.Length; i++)
            {
                if (i < shellData.CurrentCount)
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
                int groupThreshold = (i + 1) * 10;

                if (shellData.CurrentCount >= groupThreshold)
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
