using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UFO : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    public string info;
    public GameObject textIndex;
    private Button Btn;
    float timer;
    private Animator Anim;
    private void Awake()
    {
        
        Anim = GetComponent<Animator>();
        Btn = this.gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        // load so luong len text
        textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("UFO").ToString();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (board.instance.CurrentState == State.stop && SaveManager.instance.LoadChooseHelp("UFO") > 0)
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
        if (board.instance.CurrentState == State.stop && !board.instance.EmptyMove() && SaveManager.instance.LoadChooseHelp("UFO") > 0)
        {
            board.instance.CurrentState = State.help;
            Anim.SetBool("run", true);
            HelpManager.instance.SetChoosePanel(transform.GetChild(0).GetComponent<Image>().sprite, info);
            SetChoose();

        }
    }

    // them su kien cho button Yes and No
    void SetChoose()
    {
        HelpManager.instance.yesBtn.onClick.AddListener(() =>SetYes());
        HelpManager.instance.noBtn.onClick.AddListener(() =>Off());
    }

    // neu nguoi choi chon Yes thi se tao ra 3 boom ngau nhiem o 3 o sau do kich hoat chung
    void SetYes()
    {
        if(SaveManager.instance.LoadChooseHelp(this.name)>0)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.press);
            SaveManager.instance.SaveHelp("UFO", -1);
            textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("UFO").ToString();
            Anim.SetBool("run", false);
            board.instance.CurrentState = State.stop;
            board.instance.UFO = true;
            HelpManager.instance.BlackPanel.SetActive(false);
            HelpManager.instance.helpPanel.SetActive(false);
            List<GameObject> list = new List<GameObject>();
            List<GameObject> boom = new List<GameObject>();
            for (int i = 0; i < board.instance.width; i++)
            {
                for (int j = 0; j < board.instance.height; j++)
                {
                    if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null && board.instance.Emptys[i, j].PieceNotBoom())
                    {
                        if (!list.Contains(board.instance.Emptys[i, j].gameObject))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                        }
                    }
                }
            }

            int sum = 3;
            while (sum > 0)
            {
                int index = Random.Range(0, 2);
                int random = Random.Range(0, list.Count);
                if (index == 0)
                {
                    board.instance.Emptys[list[random].GetComponent<empty>().column, list[random].GetComponent<empty>().row].CreateBoomHoz();
                }
                else
                {
                    board.instance.Emptys[list[random].GetComponent<empty>().column, list[random].GetComponent<empty>().row].CreateBoomVer();
                }
                sum--;
                boom.Add(list[random]);
                list.Remove(list[random]);
            }
            StartCoroutine(OpenBoom(boom));
        }
        else
        {
            Off();
        }

    }

    // tat panel neu nguoi choi khong dung
    void Off()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        Anim.SetBool("run", false);
        board.instance.CurrentState = State.stop;
        HelpManager.instance.BlackPanel.SetActive(false);
        HelpManager.instance.helpPanel.SetActive(false);
    }

    // tao 3 boom ngau nhien va kich hoat chung
    IEnumerator OpenBoom(List<GameObject> boom)
    {
        yield return new WaitForSeconds(1f);
        if (board.instance.CurrentState != State.wait)
        {
            foreach (GameObject go in boom)
            {
                if (go != null)
                {
                    if (go.GetComponent<empty>().IsBoomHoz)
                    {
                        SetBoomHoz(go.GetComponent<empty>().column, go.GetComponent<empty>().row);
                    }
                    else if (go.GetComponent<empty>().IsBoomVer)
                    {
                        SetBoomVer(go.GetComponent<empty>().column, go.GetComponent<empty>().row);
                    }
                }
            }

            for (int i = 0; i < board.instance.width; i++)
            {
                for (int j = 0; j < board.instance.height; j++)
                {
                    if (!board.instance.BlankSpace[i,j] && board.instance.Emptys[i,j] != null)
                    {
                        if (board.instance.Emptys[i,j].duplicate)
                        {
                            
                            
                            board.instance.MakeDameMission(i,j);
                            board.instance.Emptys[i, j].OffPoolEmpty();
                            yield return new WaitForSeconds(0.02f);
                        }
                    }
                }
            }

            board.instance.AfterDestroyCo();
            board.instance.UFO = false;
        }
    }

    // xoa cac item trong hang ngang
    void SetBoomHoz(int x , int y)
    {
        if (board.instance.Emptys[x, y].IsBoomHoz)
        {
            board.instance.Emptys[x, y].duplicate = true;
            for (int i = 0; i < board.instance.width; i++)
            {
                board.instance.GetObjectPool("clearItemHoz", i, y);
                if (!board.instance.BlankSpace[i, y])
                {
                    if (board.instance.Emptys[i, y] != null)
                    {
                        if (board.instance.Emptys[i, y].PieceNotBoom() && !board.instance.Emptys[i, y].duplicate)
                        {
                            MusicManager.instance.PlaySound(MusicManager.instance.Clear);
                            board.instance.Emptys[i, y].duplicate = true;
                        }
                        else
                        {
                            SetBoomVer(i, y);
                        }
                    }
                }
                else
                {
                    if (board.instance.BrickSpace[i, y] != null)
                    {
                        board.instance.BrickSpace[i, y].GetComponent<brick>().TakeDame(1, i, y);
                    }
                }
            }
        }
    }

    // xoa cac item trong hang doc
    void SetBoomVer(int x , int y)
    {
        if (board.instance.Emptys[x, y].IsBoomVer)
        {
            board.instance.Emptys[x, y].duplicate = true;
            for (int i = 0; i < board.instance.height; i++)
            {
                board.instance.GetObjectPool("clearItemVer", x, i);
                if (!board.instance.BlankSpace[x, i])
                {
                    if (board.instance.Emptys[x, i] != null)
                    {
                        if (board.instance.Emptys[x, i].PieceNotBoom() && !board.instance.Emptys[x, i].duplicate)
                        {
                            MusicManager.instance.PlaySound(MusicManager.instance.Clear);
                            
                            board.instance.Emptys[x,i].duplicate = true;
                        }
                        else
                        {
                            SetBoomHoz(x, i);
                        }
                    }
                }
                else
                {
                    if (board.instance.BrickSpace[x, i] != null)
                    {
                        board.instance.BrickSpace[x, i].GetComponent<brick>().TakeDame(1, x, i);
                    }
                }
            }
        }
    }
}
