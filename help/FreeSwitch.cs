using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FreeSwitch : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    public string info;
    public Button Btn;
    public GameObject textIndex;
    private Animator Anim;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    private void Start()
    {
        // load so luong cua freeswitch len text
        textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("FreeSwitch").ToString();
    }

    // khi bam xuong tro giup nay
    public void OnPointerDown(PointerEventData eventData)
    {
        if (board.instance.CurrentState == State.stop && SaveManager.instance.LoadChooseHelp("FreeSwitch") > 0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.input);
            HelpManager.instance.SetAnim(this.gameObject);
        }
        else
        {
            MusicManager.instance.PlaySound(MusicManager.instance.press);
        }
    }

    // khi nhac len khoi tro giup nay
    public void OnPointerUp(PointerEventData eventData)
    {
        if (board.instance.CurrentState == State.stop && !board.instance.EmptyMove() && SaveManager.instance.LoadChooseHelp("FreeSwitch") > 0)
        {
            board.instance.CurrentState = State.help;
            Anim.SetBool("run", true);
            HelpManager.instance.SetChoosePanel(transform.GetChild(0).GetComponent<Image>().sprite, info);
            AddEvent();
        }
    }

    // bat dau tro giup nay, se mien phi 1 luot di chuyen
    public void FreeSwitchHelp()
    {
        if (SaveManager.instance.LoadChooseHelp(this.name) > 0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.press);
            board.instance.FreeSwitch = true;
            SaveManager.instance.SaveHelp("FreeSwitch", -1);
            textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("FreeSwitch").ToString();
            Anim.SetBool("run", false);
            HelpManager.instance.BlackPanel.SetActive(false);
            HelpManager.instance.helpPanel.SetActive(false);
            board.instance.CurrentState = State.stop;
        }
        else
        {
            Off();
        }
    }

    // them su kien vao button Yes or No , su dung hay khong
    void AddEvent()
    {
        HelpManager.instance.yesBtn.onClick.AddListener(() => FreeSwitchHelp());
        HelpManager.instance.noBtn.onClick.AddListener(() => Off());
    }

    // tat panel tro giup nay
    void Off()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        Anim.SetBool("run", false);
        board.instance.CurrentState = State.stop;
        HelpManager.instance.helpPanel.SetActive(false);
    }
}
