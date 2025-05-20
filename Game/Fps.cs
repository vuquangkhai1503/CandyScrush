using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
    public Text fpsTxt;
    float deltatime;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        deltatime += (Time.unscaledDeltaTime - deltatime) * 0.1f;
        float fps = 1.0f / deltatime;
        fpsTxt.text = "Fps :" + Mathf.Ceil(fps);
    }
}
