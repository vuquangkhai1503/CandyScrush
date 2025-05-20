using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// save trang thai hoan thanh hay chua
[System.Serializable]
public class Quest_
{
    public string name_;
    public bool finish;
    public bool claim;
}

[System.Serializable]
public class QuestSave
{
    public List<Quest_> quests;
}

// save cac quest trong ngay
[System.Serializable]
public class QuestDaily
{
    public List<QuestDaily_> questDaily;
}

// save info quest
[System.Serializable]
public class Quest
{
    public GameObject mission;
    public int indexMission;
    public GameObject reward;
    public int indexReward;
    public int current;
}

// save quest
// save info quest
[System.Serializable]
public class QuestDaily_
{
    public int indexItem;
    public int indexMission;
    public int indexReward;
    public int current;
}


public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> QuestMission; // list tong hop cac Quest co trong game
    public GameObject questPrefab;
    public int indexQuest;
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

    // khi nguoi choi bam vao quest button thi se load cac quest len UI
    public void OpenQuest()
    {
        string lastTime = TimeManager.instance.LoadlastTime();
        DateTime lasttime = DateTime.Parse(lastTime);
        if(DateTime.Now.Date > lasttime.Date)
        {
            SetQuestInfo();
            TimeManager.instance.SaveTime(TimeManager.instance.LoadHeath(), TimeManager.instance.LoadTimer());
        }
        else
        {
            if(LoadQuestDaily() != null)
            {
                SetQuest();
            }
            else
            {
                SetQuestInfo();
                TimeManager.instance.SaveTime(TimeManager.instance.LoadHeath(), TimeManager.instance.LoadTimer());
            }
            
        }
    }

    //save thong tin quest
    void SaveQuest(string name , bool finish, bool claim)
    {
        QuestSave questSave = LoadQuest();
        if (questSave == null)
        {
            questSave = new QuestSave()
            {
                quests = new List<Quest_>()
            };
            var quest = new Quest_()
            {
                name_ = name,
                finish = finish,
                claim = claim
            };
            questSave.quests.Add(quest);

        }
        else
        {
            var check = questSave.quests.Find(i=>i.name_ == name);
            if(check != null)
            {
                check.name_ = name;
                check.finish = finish;
                check.claim = claim;
            }
            else
            {
                var quest = new Quest_()
                {
                    name_ = name,
                    finish = finish,
                    claim= claim
                };
                questSave.quests.Add(quest);
            }
        }
        PlayerPrefs.SetString("quest",JsonUtility.ToJson(questSave));
        PlayerPrefs.Save();
    }

    QuestSave LoadQuest()
    {
        return JsonUtility.FromJson<QuestSave>(PlayerPrefs.GetString("quest"));
    }
    bool LoadFinish(string name)
    {
        var check = LoadQuest().quests.Find(i => i.name_ == name);
        return check != null ? check.finish : false;
    }

    bool LoadClaim(string name)
    {
        var check = LoadQuest().quests.Find(i => i.name_ == name);
        return check != null ? check.claim : false;
    }
    //luu lai cac quest co trong ngay hom nay
    void SaveQuestDaily(int indexItem ,GameObject mission, int indexMission , GameObject reward , int indexreward, int current)
    {
        QuestDaily quest = LoadQuestDailyCo();
        if(quest == null)
        {
            quest = new QuestDaily()
            {
                questDaily = new List<QuestDaily_>()
            };
            QuestDaily_ x = new QuestDaily_()
            {
                indexItem = indexItem,
                indexMission = indexMission,
                indexReward = indexreward,
                current = current
            };
            quest.questDaily.Add(x);
        }
        else
        {
            QuestDaily_ quest_ = quest.questDaily.Find(i => i.indexItem == indexItem);
            if(quest_ != null)
            {
                quest_.indexItem = indexItem;
                quest_.indexMission = indexMission;
                quest_.indexReward = indexreward;
                quest_.current = current;

            }
            else
            {
                QuestDaily_ x = new QuestDaily_()
                {
                    indexItem = indexItem,
                    indexMission = indexMission,
                    indexReward = indexreward,
                    current = current
                };
                quest.questDaily.Add(x);
            }
        }
        PlayerPrefs.SetString("QuestDaily_",JsonUtility.ToJson(quest));
        PlayerPrefs.Save();
    }
    
    QuestDaily LoadQuestDailyCo()
    {
        return JsonUtility.FromJson<QuestDaily>(PlayerPrefs.GetString("QuestDaily_"));
    }

    List<QuestDaily_> LoadQuestDaily()
    {
        return LoadQuestDailyCo() != null ? LoadQuestDailyCo().questDaily : null;
    }

    // tao ra list quest moi neu da qua ngay moi
    void SetQuestInfo()
    {
       if(indexQuest <= QuestMission.Count)
       {
            List<Quest> listQuest = new List<Quest>();
            List<int> indexItem = new List<int>();
            while (listQuest.Count < indexQuest)
            {
                int index = UnityEngine.Random.Range(0, QuestMission.Count);
                if(!listQuest.Contains(QuestMission[index]))
                {
                    listQuest.Add(QuestMission[index]);
                    indexItem.Add(index);
                }
            }
            PlayerPrefs.DeleteKey("quest");
            PlayerPrefs.DeleteKey("QuestDaily_");
            PlayerPrefs.Save();

            int i = 0;
            while(i <listQuest.Count)
            {
                SaveQuest(listQuest[i].mission.name, false, false);
                SaveQuestDaily(indexItem[i],listQuest[i].mission, listQuest[i].indexMission, listQuest[i].reward, listQuest[i].indexReward, 0);
                i++;
            }
            SetQuest();
        }
    }

    // lay du lieu quest hom nay da luu va load len UI
    void SetQuest()
    {
         List<QuestDaily_> listQuest = LoadQuestDaily().Select(m => new QuestDaily_
         {

             indexItem = m.indexItem,
             indexMission = m.indexMission,
             indexReward = m.indexReward,
             current = m.current,
         }).ToList();
        if(listQuest != null && MenuManager.instance.PanelQuest.transform.childCount == 0)
        {
            for (int i = 0; i < listQuest.Count; i++)
            {
                if (listQuest[i] != null)
                {
                    GameObject quest = Instantiate(questPrefab, MenuManager.instance.PanelQuest);
                    quest.name = listQuest[i].indexItem.ToString();
                    quest.transform.GetChild(0).GetComponent<Image>().sprite = QuestMission[listQuest[i].indexItem].mission.GetComponent<SpriteRenderer>().sprite;
                    quest.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "x" + listQuest[i].indexMission.ToString();
                    quest.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().sprite = QuestMission[listQuest[i].indexItem].reward.GetComponent<SpriteRenderer>().sprite;
                    quest.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = listQuest[i].indexReward.ToString();
                    quest.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = listQuest[i].current + "/" + listQuest[i].indexMission.ToString();
                }
            }

            CheckQuest();
        }
    }

    // kiem tra trang thai quest nay da hoan thanh chua va thiet lap tren UI
    void CheckQuest()
    {

        for (int i = 0; i < LoadQuestDaily().Count; i++)
        {
            if (LoadQuestDaily()[i].current < LoadQuestDaily()[i].indexMission && !LoadClaim(QuestMission[LoadQuestDaily()[i].indexItem].mission.name))
            {
                MenuManager.instance.PanelQuest.transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(true);
                MenuManager.instance.PanelQuest.transform.GetChild(i).transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "claim";
            }
            else if(LoadQuestDaily()[i].current == LoadQuestDaily()[i].indexMission && !LoadClaim(QuestMission[LoadQuestDaily()[i].indexItem].mission.name))
            {
                MenuManager.instance.PanelQuest.transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(false);
                string name = QuestMission[LoadQuestDaily()[i].indexItem].mission.name;
                GameObject obj = QuestMission[LoadQuestDaily()[i].indexItem].reward;
                int index = LoadQuestDaily()[i].indexReward;
                MenuManager.instance.PanelQuest.transform.GetChild(i).transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                MenuManager.instance.PanelQuest.transform.GetChild(i).transform.GetChild(3).GetComponent<Button>().onClick.AddListener(()
                    => Reward(name,obj,index));
                
            }
            else if(LoadQuestDaily()[i].current == LoadQuestDaily()[i].indexMission && LoadClaim(QuestMission[LoadQuestDaily()[i].indexItem].mission.name))
            {
                MenuManager.instance.PanelQuest.transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(true);
                MenuManager.instance.PanelQuest.transform.GetChild(i).transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "finish";
            }

        }
    }

    //nguoi choi da hoan thanh quest
    void Reward(string name, GameObject obj,int index)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.input);
        SaveQuest(name, true, true);
        if (obj.name == "money")
        {
            SaveManager.instance.SaveMoney(index);
        }
        else
        {
            SaveManager.instance.SaveHelp(obj.name, index);
        }
        CheckQuest();
    }

    // thiet lap so luong da hoan thanh cua 1 quest
    public void DameQuest(string name)
    {
        if (LoadQuestDaily() != null)
        {
            for (int i = 0; i < LoadQuestDaily().Count; i++)
            {
                if (QuestMission[LoadQuestDaily()[i].indexItem].mission.name == name && LoadQuestDaily()[i]!= null && LoadQuestDaily()[i].indexMission > LoadQuestDaily()[i].current)
                {
                   
                    int index = LoadQuestDaily()[i].current += 1;
                    if(LoadQuestDaily()[i].current == LoadQuestDaily()[i].indexMission)
                    {
                        SaveQuest(name,true,false);
                    }
       
                    SaveQuestDaily(LoadQuestDaily()[i].indexItem, QuestMission[LoadQuestDaily()[i].indexItem].mission, LoadQuestDaily()[i].indexMission, QuestMission[LoadQuestDaily()[i].indexItem].reward, LoadQuestDaily()[i].indexReward, index);
                }
            }
        }
    }

}
