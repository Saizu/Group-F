using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCartridge : MonoBehaviour{
    public float lifeTime = 10f;         // 消滅時間
    public float blinkTime = 7f;         // 点滅開始時間

    private Renderer re;
    private bool isBlinking;

    private void Start(){
        re = GetComponent<Renderer>();
        Invoke(nameof(StartBlinking), blinkTime);  // 一定時間後点滅開始(文字列,秒数)
        Destroy(gameObject, lifeTime);             // 自動消滅
    }

    private void StartBlinking(){
        isBlinking = true;
    }

    private void Update(){
        if (isBlinking){
            float blink = Mathf.PingPong(Time.time * 5, 1);  // 点滅
            Color color = re.material.color;
            color.a = blink;  // アルファ値更新
            re.material.color = color;
        }
    }
}