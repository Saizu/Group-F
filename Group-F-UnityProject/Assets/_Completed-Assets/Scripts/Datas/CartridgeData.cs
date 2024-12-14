using System;
using UnityEngine;

[Serializable]
public class CartridgeData
{
    // カートリッジのプレハブ参照
    public GameObject cartridgePrefab;

    // 生成位置
    public Vector3 spawnPosition;

    // 生成頻度 (秒単位)
    public float spawnFrequency;
}
