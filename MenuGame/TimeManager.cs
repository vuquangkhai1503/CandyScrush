using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// hearth va thoi gian cua nguoi choi
[System.Serializable]
public class time
{
    public int heath;
    public int timer;
    public string lastTime;
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public int heathIndex = 5;
    public int timeHeath = 300;
    [HideInInspector]public int timer;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public time LoadTime()
    {
        return JsonUtility.FromJson<time>(PlayerPrefs.GetString("lastTimee"));
    }

    // luu lai thoi gian 
    public void SaveTime(int heath,int timer)
    {
        
        time obj = LoadTime();
        if(obj == null)
        {
            obj = new time();
            {
                obj.timer = timer;
                obj.heath= heath;
                obj.lastTime = DateTime.Now.ToString();
            }
        }else
        {
            obj.heath = heath;
            obj.timer = timer;
            obj.lastTime = DateTime.Now.ToString();
        }
        
        PlayerPrefs.SetString("lastTimee",JsonUtility.ToJson(obj));
        Debug.Log(DateTime.Now + "save");
        PlayerPrefs.Save();

    }

    // load ra so hearth cua nguoi choi
    public int LoadHeath()
    {
        return LoadTime()!= null ? LoadTime().heath : 5;
    }

    public int LoadTimer()
    {
        return LoadTime() != null ? LoadTime().timer : 0;
    }

    // load thoi gian da luu
    public string LoadlastTime()
    {
        return LoadTime() != null ? LoadTime().lastTime : DateTime.Now.ToString();
    }

    // khi nguoi choi choi thua se tru 1 hearth
    public void Lose()
    {
        if(heathIndex>0)
        {
            heathIndex = LoadHeath()-1;
            timer = LoadTimer() + timeHeath;
            SaveTime(heathIndex,timer);
        }
    }


    // khi dong app se luu lai thoi gian
    private void OnApplicationQuit()
    {
        SaveTime(LoadHeath(),LoadTimer()); 
    }
}
