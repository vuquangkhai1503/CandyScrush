using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExtraMove : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
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
        textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("ExtraMove").ToString();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (board.instance.CurrentState == State.stop && SaveManager.instance.LoadChooseHelp("ExtraMove") > 0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.input);
            HelpManager.instance.SetAnim(this.gameObject);
        }
        else
        {
            MusicManager.instance.PlaySound(MusicManager.instance.press);
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (board.instance.CurrentState == State.stop && !board.instance.EmptyMove() && SaveManager.instance.LoadChooseHelp("ExtraMove") > 0)
        {
            board.instance.CurrentState = State.help;
            Anim.SetBool("run", true);
            HelpManager.instance.SetChoosePanel(transform.GetChild(0).GetComponent<Image>().sprite, info);
            AddEvent();
        }
    }

    // them su kien cho button Yes and No
    void AddEvent()
    {
        HelpManager.instance.yesBtn.onClick.AddListener(() => StartCoroutine(AddSwipe()));
        HelpManager.instance.noBtn.onClick.AddListener(() => Off());
    }

    // them 5 luot di chuyen cho nguoi choi
    IEnumerator AddSwipe()
    {
        if (SaveManager.instance.LoadChooseHelp(this.name) > 0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.press);
            Anim.SetBool("run", false);
            HelpManager.instance.helpPanel.SetActive(false);
            SaveManager.instance.SaveHelp("ExtraMove", -1);
            textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("ExtraMove").ToString();
            yield return new WaitForSeconds(0.5f);
            board.instance.Swipe += HelpManager.instance.ExtraMoveIndex;
            GameManager.Instance.SwipeTxt.GetComponent<Text>().text = board.instance.Swipe.ToString();
            board.instance.CurrentState = State.stop;
        }
        else
        {
            Off();
        }
    }

    // tat neu nguoi  choi khong dung
    void Off()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        Anim.SetBool("run", false);
        HelpManager.instance.helpPanel.SetActive(false);
        board.instance.CurrentState = State.stop;
    }
}
