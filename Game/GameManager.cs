using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject SwipeTxt;
    private score score;
    private board board;
    [Header("Panel Win")]
    public GameObject GameWinOverPanel;
    public GameObject LevelTxt;
    public GameObject[] Stars;
    [Header("Panel Lose")]
    public GameObject GameLoseOverPanel;
    public GameObject LoseTxt;
    public GameObject loseContent;
    public GameObject loseItemPrefab;
    [Header("khac")]
    public GameObject SwappingPanel;

    private void Awake()
    {
        score= FindAnyObjectByType<score>();
        board= FindAnyObjectByType<board>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // tat panel win and lose
        GameWinOverPanel.SetActive(false);
        GameLoseOverPanel.SetActive(false);
    }

    public IEnumerator LoadAndApplySetting()
    {
        yield return new WaitForSeconds(0.1f);
    }

    // bat panel win neu nguoi choi chien thang
    public void SetLevelWin(int level, int star)
    {
        GameWinOverPanel.SetActive(true);
        LevelTxt.gameObject.GetComponent<Text>().text = "Level "+ level.ToString();
        for (int i = 1; i <= Stars.Length; i++)
        {
            if(i <= star)
            {
                Stars[i-1].gameObject.SetActive(true);
            }
        }
    }

    // bat len khi nguoi choi thua , set du lieu cac nhiem vu chua hoan thanh
    public void SetLevelLose()
    {
        GameLoseOverPanel.SetActive(true);
        if(!mission.instance.FinishMission())
        {
            for (int i = 0; i < mission.instance.GetMissionLose().Count; i++)
            {
                GameObject lose = Instantiate(loseItemPrefab,loseContent.transform);
                if(mission.instance.GetMissionLose()[i].Items.tag == "jelly" || mission.instance.GetMissionLose()[i].Items.tag == "brick")
                {
                    lose.transform.GetChild(0).GetComponent<Image>().sprite = mission.instance.GetMissionLose()[i].Items.GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    lose.transform.GetChild(0).GetComponent<Image>().sprite = mission.instance.GetMissionLose()[i].Items.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                }
                lose.transform.GetChild(1).GetComponent<Text>().text = mission.instance.GetMissionLose()[i].Index.ToString();
            }

        }

    }
}
