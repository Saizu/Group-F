using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponStockData
{
    [SerializeField] private int initialCount; // 初期所持数
    [SerializeField] private int maxCount;    // 最大所持数
    [SerializeField] private int replenishCount;  // 補充数
    private int currentCount; // 現在の所持数
    public int CurrentCount => currentCount; // 現在の所持数を取得する
    public int MaxCount => maxCount;
    public int ReplenishCount => replenishCount;

    // 所持数を初期化
    public void InitializeCount()
    {
        currentCount = initialCount;
    }

    // 所持数を増やす
    public void Add(int count)
    {
        currentCount = Mathf.Clamp(currentCount + count, 0, maxCount);
    }

    // 所持数を減らす
    public void Decrement()
    {
        if (currentCount > 0)
        {
            currentCount--;
        }
    }
}