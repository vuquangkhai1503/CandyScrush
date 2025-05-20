using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levell : MonoBehaviour
{
    // thiet lap cac phan tu trong level prefab
    public GameObject levelTxt;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject LockPanel;

    // neu ham nay duoc goi thi se load scene game va bat dau tro choi
    void LoadLevelGame(int index)
    {
        if (TimeManager.instance.LoadHeath() > 0)
        {
            loadingLevel.instance.LoadLevel("Game", index );
            LevelManager.Instance.booster_1 = SelectBooster.instance.choose_1;
            LevelManager.Instance.booster_2 = SelectBooster.instance.choose_2;
            LevelManager.Instance.booster_3 = SelectBooster.instance.choose_3;
        }
        else
        {

        }
    }

    // thiet lap cac thuoc tinh cho level prefab
    public void SetLevel(int index)
    {
        this.gameObject.name = ("Level" + index).ToString();     
        levelTxt.transform.GetChild(0).GetComponent<Text>().text = index.ToString();
        levelTxt.GetComponent<Button>().onClick.AddListener(() => SetOpenBooster(index));

        int star = SaveManager.instance.LoadStarLevel(index);
        int IsLocked = SaveManager.instance.LoadisLockedLevel();
        if (index <= IsLocked)
        {
            LockPanel.SetActive(false);
            if(star >= 1)
            {
                star1.gameObject.SetActive(true);
            }
            if (star >= 2)
                {
                
                star2.gameObject.SetActive(true);
                }
            if (star >= 3)
                {
                star3.gameObject.SetActive(true);
                }
        }
    }

    // set cac thuoc tinh cho panel booster de load vao game
    void SetOpenBooster(int index)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.buyItem);
        SelectBooster.instance.SelectBoost.SetActive(true);
        SelectBooster.instance.LevelTxt.GetComponent<TMP_Text>().text = "Level" + " " + index.ToString();
        SelectBooster.instance.SetIndex(SelectBooster.instance.text_1, SaveManager.instance.LoadChooseHelp("boost_1"));
        SelectBooster.instance.SetIndex(SelectBooster.instance.text_2, SaveManager.instance.LoadChooseHelp("boost_2"));
        SelectBooster.instance.SetIndex(SelectBooster.instance.text_3, SaveManager.instance.LoadChooseHelp("boost_3"));
        SelectBooster.instance.SetBoostPurchase();
        SelectBooster.instance.Play.onClick.RemoveAllListeners();
        SelectBooster.instance.Play.onClick.AddListener(() => LoadLevelGame(index));
    }
}
