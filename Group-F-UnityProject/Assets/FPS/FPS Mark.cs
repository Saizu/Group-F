using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput : MonoBehaviour
{
  private void Start()
  {
    Application.targetFrameRate = 40;
  }
    void Update()
  {
    float fps = 1f / Time.deltaTime;
    Debug.Log("fps: " + fps);
  }
}