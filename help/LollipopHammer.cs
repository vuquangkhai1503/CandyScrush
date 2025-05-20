
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LollipopHammer : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public string info;
    public Button Btn;
    public GameObject textIndex;
    private Animator Anim;
    bool check = false;
    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    private void Start()
    {
        textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("LollipopHammer").ToString();
    }

    private void Update()
    {
        if (check)
        {
            LollipopHamerHelp();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(board.instance.CurrentState == State.stop && SaveManager.instance.LoadChooseHelp("LollipopHammer") >0)
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
        if (board.instance.CurrentState == State.stop && !board.instance.EmptyMove() && SaveManager.instance.LoadChooseHelp("LollipopHammer") > 0)
        {
            board.instance.CurrentState = State.help;
            Anim.SetBool("run", true);
            HelpManager.instance.SetChoosePanel(transform.GetChild(0).GetComponent<Image>().sprite, info);
            AddEvent();
        }
    }

    // ham nay se xu li khi nguoi choi bam xoa vao bat cu piece nao
    void LollipopHamerHelp()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin,ray.direction);
            if (hit.point!=null && hit.collider != null)
            {
                
                Vector2 pos = hit.point;
                MusicManager.instance.PlaySound(MusicManager.instance.buyItem);
                if (!board.instance.BlankSpace[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)] && check )
                {
                    SaveManager.instance.SaveHelp("LollipopHammer", -1);
                    textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("LollipopHammer").ToString();
                    check = false;
                    if (board.instance.AllJelly[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)] != null)
                    {
                        board.instance.AllJelly[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)].GetComponent<jelly>().TakeDame(1, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
                    }
                    else
                    {
                        if (board.instance.Emptys[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)] != null)
                        {
                            board.instance.Emptys[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)].OffPoolEmpty();
                            board.instance.Emptys[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)] = null;
                        }
                    }

                    Off();
                    board.instance.DesAllItemDuplicate();
                }
                else if(board.instance.BlankSpace[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)] && board.instance.BrickSpace[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)] != null && check)
                {
                    check = false;  
                    SaveManager.instance.SaveHelp("LollipopHammer", -1);
                    textIndex.GetComponent<TMP_Text>().text = SaveManager.instance.LoadChooseHelp("LollipopHammer").ToString();
                    board.instance.BrickSpace[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)].gameObject.GetComponent<brick>().TakeDame(1, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
                    Off();
                    board.instance.DesAllItemDuplicate();
                }
                else if (Mathf.RoundToInt(pos.x)<0 || Mathf.RoundToInt(pos.y) <0 || Mathf.RoundToInt(pos.x) > board.instance.width || Mathf.RoundToInt(pos.y) > board.instance.height)
                {
                    check = false;
                    Off();
                }
            }
            else
            {
                check = false;
                Off();
            }
        }
    }

    // neu nguoi choi bam Yes thi se kich hoat tinh nang
    void AddEvent()
    {
        HelpManager.instance.yesBtn.onClick.AddListener(() => SetYes());
        HelpManager.instance.yesBtn.onClick.AddListener(() => HelpManager.instance.helpPanel.SetActive(false));
        HelpManager.instance.noBtn.onClick.AddListener(() => Off());
    }

    // tat neu nguoi choi khongh dung
    void Off()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        Anim.SetBool("run", false);
        board.instance.CurrentState = State.stop;
        HelpManager.instance.BlackPanel.SetActive(false);
        HelpManager.instance.helpPanel.SetActive(false);
    }
    

    // thiet lap tinh nang neu nguoi choi chon Yes
    void SetYes()
    {
        if (SaveManager.instance.LoadChooseHelp(this.name) > 0)
        {
            check = true;
            HelpManager.instance.BlackPanel.SetActive(true);
            MusicManager.instance.PlaySound(MusicManager.instance.press);
        }
        else
        {
            Off();
        }
    }
}
