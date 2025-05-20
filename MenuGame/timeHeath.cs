using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class timeHeath : MonoBehaviour
{
    public Text timeTxt;
    public TMP_Text heathTxt;
    public GameObject hearth;
    public Sprite hearthBreak;
    public Sprite hearthNormal;
    private void Start()
    {
        // thiet lap thoi gian hoi Heath cho nguoi choi neu nguoi choi choi bi thua
        SetTimer();
    }

    // load du lieu thoi gian len Text
    void SetTime()
    {
        int timer = TimeManager.instance.LoadTimer();
        timer -= 1;
        TimeManager.instance.SaveTime(TimeManager.instance.LoadHeath(),timer);
        int minute = Mathf.FloorToInt((TimeManager.instance.LoadTimer() / 60)-1);
        int second = Mathf.FloorToInt(TimeManager.instance.LoadTimer() % 60);
        timeTxt.text = $"{minute:D2}:{second:D2}";
    }


    // thiet lap thoi gian hoi Heath cho nguoi choi neu nguoi choi choi bi thua
    void SetTimer()
    {
        int timer = TimeManager.instance.LoadTimer();
        if(timer > 0 )
        {
            string last = TimeManager.instance.LoadlastTime();
            DateTime lastTime = DateTime.Parse(last);
            TimeSpan pass = DateTime.Now - lastTime;
            int x = Mathf.FloorToInt((int)pass.TotalSeconds / TimeManager.instance.timeHeath);
            int y = (int)pass.TotalSeconds % TimeManager.instance.timeHeath;
            if(x>=1)
            {
                int heath = Mathf.Clamp(TimeManager.instance.LoadHeath() + x,0,5);
                TimeManager.instance.SaveTime(heath,y);
                if(heath==5)
                {
                    TimeManager.instance.SaveTime(5, 0);
                    return;
                }
            }
            else
            {
                int timePass = TimeManager.instance.LoadTimer() - (int)pass.TotalSeconds ;
                if(timePass > 0 )
                {
                    TimeManager.instance.SaveTime(TimeManager.instance.LoadHeath(),timePass);
                }
                else
                {
                    TimeManager.instance.SaveTime(5,0);
                    return;
                }
            }
            // load du lieu time len theo tung giay
            InvokeRepeating("SetTime",1f,1f);
            heathTxt.text = TimeManager.instance.LoadHeath().ToString();
            hearth.GetComponent<Image>().sprite = hearthBreak;
        }
        else
        {
            timeTxt.text = "full";
            hearth.GetComponent<Image>().sprite = hearthNormal;
        }

    }
}
