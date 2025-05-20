using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMenuManager : MonoBehaviour
{
    public static ButtonMenuManager instance;
    // cac button co trong scene menu
    public GameObject SettingMenuPanel;
    public GameObject ShopPanel;
    public GameObject FriendPanel;
    public GameObject EventPanel;
    [Header("avatar")] // 
    public GameObject avatarPrefab;
    public GameObject avatarPanel;
    public GameObject avatarMiddle;
    public GameObject offAvatarBtn;
    public List<GameObject> avatarList;
    GameObject Btn;
    GameObject Panel;
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
        // them su kien vao cac button o scene menu
        RegisterButtonAction(TypeButtonMenu.settingMenu,OnSettingMenu);
        RegisterButtonAction(TypeButtonMenu.shopMenu, OnShop);
        RegisterButtonAction(TypeButtonMenu.mapMenu, OnMaps);
        RegisterButtonAction(TypeButtonMenu.friendMenu, OnFriend);
        RegisterButtonAction(TypeButtonMenu.eventMenu, OnEvent);
        RegisterButtonAction(TypeButtonMenu.reset, ResetAllData);
        RegisterButtonAction(TypeButtonMenu.avatar, OnAvatar);
        
        // tat avatar panel
        offAvatarBtn.GetComponent<Button>().onClick.AddListener(() => avatarPanel.SetActive(false));
        offAvatarBtn.GetComponent<Button>().onClick.AddListener(() => MusicManager.instance.PlaySound(MusicManager.instance.press));
    }

    private Dictionary<TypeButtonMenu, Action> buttonActions = new Dictionary<TypeButtonMenu, Action>();

    // dang ki su kien cho cac button co trong scene menu
    public void RegisterButtonAction(TypeButtonMenu typeButton, Action action)
    {
        if(!buttonActions.ContainsKey(typeButton))
        {
            buttonActions[typeButton] = action;
        }
    }

    public void TriggerButton(TypeButtonMenu typeButton,GameObject btn)
    {
        if(buttonActions.TryGetValue(typeButton, out Action action))
        {
            action?.Invoke();
            if(Btn != null)
            {
                if(Btn.GetComponent<Animator>() != null)
                {
                    Btn.GetComponent<Animator>().SetBool("run",false);
                }
            }
            Btn = btn;
            if (Btn.GetComponent<Animator>() != null)
                Btn.GetComponent<Animator>().SetBool("run", true);
        }
    }

    // mo cai dat trong game
    public void OnSettingMenu()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        bool isActive = SettingMenuPanel.activeSelf;
        SettingMenuPanel.SetActive(!isActive);
    }

    // open shop panel
    public void OnShop()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
        SettingMenuPanel.SetActive(false);
        avatarPanel.SetActive(false);
        Panel = ShopPanel.gameObject;
        ShopPanel.SetActive(true);
        SelectBooster.instance.SelectBoost.gameObject.SetActive(false);
    }

    // open fiend panel
    public void OnFriend()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
        Panel = FriendPanel.gameObject;
        FriendPanel.SetActive(true);
    }

    // open quest panel
    public void OnEvent()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
        Panel = EventPanel.gameObject;
        EventPanel.SetActive(true);
        QuestManager.instance.OpenQuest();
    }

    // open map panel

    public void OnMaps()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
        Panel = EventPanel.gameObject;
    }
    
    // open button avatar
    public void OnAvatar()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        avatarPanel.SetActive(true);
        CreateItemAvatar();
    }

    // thiet lap avatar o scene menu
    void CreateItemAvatar()
    {
        if (avatarMiddle.transform.childCount < avatarList.Count)
        {
            for (int i = 0; i < avatarList.Count; i++)
            {
                GameObject item = Instantiate(avatarPrefab, avatarMiddle.transform);
            }
        }
        for (int i = 0; i < avatarMiddle.transform.childCount; i++)
        {
            avatarMiddle.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = avatarList[i].GetComponent<SpriteRenderer>().sprite;
            avatarMiddle.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp(avatarList[i].name).ToString();
            if (SaveManager.instance.LoadChooseHelp(avatarList[i].name) <= 0)
            {
                avatarMiddle.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                avatarMiddle.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    // xoa toan bo du lieu co trong game
    public void ResetAllData()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        SaveManager.instance.ResetAllData();
    }
}
