using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour{
    public GameObject ammoPrefab;        // CartridgePrefab
    public float spawnInterval = 8f;
    public Vector3 spawnArea = new Vector3(50f, 0f, 50f);  // スポーン範囲

    private void Start(){
        InvokeRepeating(nameof(SpawnAmmo), 0f, spawnInterval); // 一定間隔で呼びだす
    }

    private void SpawnAmmo(){
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x), 0.5f, Random.Range(-spawnArea.z, spawnArea.z) // 高さは0.5で固定
        );
        Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);
    }
}