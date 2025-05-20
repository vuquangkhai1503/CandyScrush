using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class randomReward : MonoBehaviour
{
    public static randomReward instance;
    public int moneyReward;
    public Sprite moneySprite;
    public List<GameObject> rewardNormal;
    public List<GameObject> rewardVip;
    public GameObject rewardPrefab;
    public Transform rewardPanel;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    // random ra reward neu nguoi choi chien thang level
    public void Reward()
    {
        int check = Random.Range(0, 3);
        if(check==2)
        {
            // type == vip || normal
            int index = Random.Range(1, 11);
            if (0 < index && index <8)
            {
                CreateReward(1,"normal");
            }
            else if (index >= 8 && index <= 9)
            {
                CreateReward(2, "normal");
            }
            else if (index == 10)
            {
                CreateReward(2, "normal");
                CreateReward(1, "vip");
            }
        }
        GameObject reward = Instantiate(rewardPrefab, rewardPanel);
        reward.transform.GetChild(0).GetComponent<Image>().sprite = moneySprite;
        reward.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = moneyReward.ToString();
        reward.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().fontSize = 30;
        SaveManager.instance.SaveMoney(moneyReward);
    }

    // tao reward va load len panel
    void CreateReward(int index, string type)
    {
        List<GameObject> list = new List<GameObject>();
        while(list.Count < index)
        {
            if(type == "normal")
            {
                int reward = Random.Range(0, rewardNormal.Count);
                if (!list.Contains(rewardNormal[reward]))
                {
                    list.Add(rewardNormal[reward]);
                }
            }
            else if(type == "vip")
            {
                int reward = Random.Range(0, rewardVip.Count);
                if (!list.Contains(rewardVip[reward]))
                {
                    list.Add(rewardVip[reward]);
                }
            }
            
        }
        for (int i = 0; i < list.Count; i++)
        {
            GameObject reward = Instantiate(rewardPrefab,rewardPanel);
            reward.transform.GetChild(0).GetComponent<Image>().sprite = list[i].GetComponent<SpriteRenderer>().sprite;
            reward.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = "1";
            SaveManager.instance.SaveHelp(list[i].name,1);
        }
    }

}
