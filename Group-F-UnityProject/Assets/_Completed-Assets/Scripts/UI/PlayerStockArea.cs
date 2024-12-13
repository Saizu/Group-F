using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStockArea : MonoBehaviour
{
    [SerializeField] private Image[] singleShells; // Shell1, Shell2, ..., Shell10
    [SerializeField] private Image[] groupedShells; // Shells10, Shells20, Shells30, Shells40

    public void UpdatePlayerStockArea(int stockCount)
    {
        for (int i = 0; i < singleShells.Length; i++)
        {
            if (i < stockCount)
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

            if (stockCount >= groupThreshold)
            {
                groupedShells[i].gameObject.SetActive(true);
            }
            else
            {
                groupedShells[i].gameObject.SetActive(false);
            }
        }
    }
}
