using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SelectBooster : MonoBehaviour
{
    public static SelectBooster instance;
    [Header("play btn")]
    public GameObject SelectBoost;
    public GameObject LevelTxt;
    public Button ExitBtn;
    public Button Play;
    [Header("booster")] // thong tin cua cac booster
    public Button boost_1;
    public GameObject panel_1;
    public GameObject text_1;
    public GameObject purchase1;
    public bool choose_1 = false;

    public Button boost_2;
    public GameObject panel_2;
    public GameObject text_2;
    public GameObject purchase2;
    public bool choose_2 = false;

    public Button boost_3;
    public GameObject panel_3;
    public GameObject text_3;
    public GameObject purchase3;
    public bool choose_3 = false;

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
    private void Start()
    {
        SelectBoost.SetActive(false);
        ExitBtn.onClick.AddListener(() =>SelectBoost.SetActive(false));
        boost_1.onClick.AddListener(() => SetSelect_1());
        boost_2.onClick.AddListener(() => SetSelect_2());
        boost_3.onClick.AddListener(() => SetSelect_3());
    }


    // open panel shop 
    void OpenShop()
    {
        SelectBoost.SetActive(false);
        ButtonMenuManager.instance.OnShop();
    }

    // thiet lap nguoi choi co chon booster_1 hay khong
    void SetSelect_1()
    {
        if(SaveManager.instance.LoadChooseHelp("boost_1")>0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.input);
            choose_1 = !choose_1;
            panel_1.SetActive(choose_1);
        }
    }

    // thiet lap nguoi choi co chon booster_2 hay khong
    void SetSelect_2()
    {
        if (SaveManager.instance.LoadChooseHelp("boost_2") > 0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.input);
            choose_2 = !choose_2;
            panel_2.SetActive(choose_2);
        }
    }
    // thiet lap nguoi choi co chon booster_3 hay khong
    void SetSelect_3()
    {
        if (SaveManager.instance.LoadChooseHelp("boost_3") > 0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.input);
            choose_3 = !choose_3;
            panel_3.SetActive(choose_3);
        }
    }

    // load so luong cua moi booster len Text
    public void SetIndex(GameObject txt,int index)
    {
        txt.GetComponent<TMP_Text>().text = index.ToString();
    }


    // neu booster nay co so luong <= 0 thi se hien button mua den shop
    public void SetBoostPurchase()
    {
        if (SaveManager.instance.LoadChooseHelp("boost_1") <= 0)
        {
            purchase1.gameObject.SetActive(true);
        }
        else
        {
            purchase1.gameObject.SetActive(false);
        }
        if (SaveManager.instance.LoadChooseHelp("boost_2") <= 0)
        {
            purchase2.gameObject.SetActive(true);
        }
        else
        {
            purchase2.gameObject.SetActive(false);
        }
        if (SaveManager.instance.LoadChooseHelp("boost_3") <= 0)
        {
            purchase3.gameObject.SetActive(true);
        }
        else
        {
            purchase3.gameObject.SetActive(false);
        }
    }
}
