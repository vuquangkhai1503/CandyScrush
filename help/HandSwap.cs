using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class HandSwap : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
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
        // load so luong cua handswap len ui_text

        textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("HandSwap").ToString();
    }

    // khi bam vao tro giup nay
    public void OnPointerDown(PointerEventData eventData)
    {
        if(board.instance.CurrentState == State.stop)
        {
            HelpManager.instance.SetAnim(this.gameObject);
            MusicManager.instance.PlaySound(MusicManager.instance.input);
        }
        else
        {
            MusicManager.instance.PlaySound(MusicManager.instance.press);
        }
    }

    // khi nhac len khoi tro giup nay
    public void OnPointerUp(PointerEventData eventData)
    {
        if (board.instance.CurrentState == State.stop && !board.instance.EmptyMove())
        {
            board.instance.CurrentState = State.help;
            board.instance.HandSwap = true;
            Anim.SetBool("run", true);
            HelpManager.instance.SetChoosePanel(transform.GetChild(0).GetComponent<Image>().sprite, info);
            AddEvent();
        }
    }

    // them su kien cho button neu bam Yes hoac No de su dung tro giup nay hay khong
    void AddEvent()
    {
        HelpManager.instance.yesBtn.onClick.AddListener(() => HandSwapHelp());
        HelpManager.instance.noBtn.onClick.AddListener(() => Off());
    }


    // bat dau tro giup handswap , neu su dung thi se di chuyen ma khong can tao cap 3
    public void HandSwapHelp()
    {
        if (SaveManager.instance.LoadChooseHelp(this.name) > 0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.press);
            SaveManager.instance.SaveHelp("HandSwap", -1);
            textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("HandSwap").ToString();
            board.instance.CurrentState = State.stop;
            Anim.SetBool("run", false);
            board.instance.HandSwap = true;
            HelpManager.instance.BlackPanel.SetActive(false);
            HelpManager.instance.helpPanel.SetActive(false);
        }
        else
        {
            Off();
        }
    }

    // tat panel tro giup handSwap
    void Off()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        Anim.SetBool("run", false);
        board.instance.CurrentState = State.stop;
        HelpManager.instance.helpPanel.SetActive(false);
    }
}
