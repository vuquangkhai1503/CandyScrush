using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// kieu loai button trong scene menu
public enum TypeButtonMenu
{
    settingMenu,
    shopMenu,
    eventMenu,
    friendMenu,
    mapMenu,
    avatar,
    reset,
}

public class EventBtnMenu : MonoBehaviour
{
    public TypeButtonMenu button;
    private void Start()
    {
        // dang ki su kien cho button
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => ButtonMenuManager.instance.TriggerButton(button,this.gameObject));
    }
}
