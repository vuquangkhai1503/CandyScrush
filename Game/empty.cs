
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class empty : MonoBehaviour
{
    public empty OtherDot;
    private Vector2 PosDown;
    private Vector2 PosUp;
    private int targetX;
    private int targetY;
    public int column;
    public int row;
    private float speed = 10f;
    private board board;
    private CheckAll CheckAlls;
    public bool duplicate = false;
    private Animator anim;
    private float lastColumn;
    private float lastRow;
    public bool IsBoomHoz = false;
    public bool IsBoomVer = false;
    public bool IsBoomAdjasted = false;
    public bool IsBoomColor = false;
    public bool move = false;
    bool line = false;
    bool press = false;
    private void Awake()
    {
        board = FindObjectOfType<board>();
        CheckAlls = FindObjectOfType<CheckAll>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX- transform.position.x) > 0.01f || Mathf.Abs(targetY - transform.position.y) > 0.01f)
        {
            move = true;
            Vector2 nextPos = new Vector2(column,row);
            transform.position = Vector2.Lerp(transform.position,nextPos, speed * Time.deltaTime);
            if (board.Emptys[column, row] != this.gameObject.transform.GetComponent<empty>())
            {
                board.Emptys[column, row] = this.gameObject.transform.GetComponent<empty>();
                this.name = ("(" +column + "," + row + ")").ToString();
            }
        }
        else
        {
            if (OtherDot != null && line == false && board.instance.CurrentDot != null && press == true)
            {
                CheckBoom();
            }
            move = false;
        }
        if (CheckMove())
        {
            press = false;
            CheckDuplicate();

        }
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PosDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        
    }

    private void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && board.CurrentState == State.stop && board.instance.Swipe > 0 && !board.instance.EmptyMove())
        {
            PosUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(PosDown, PosUp);
            if (distance >= 0.5f)
            {
                if (!board.FreeSwitch)
                {
                    board.Swipe -= 1;
                }
                else
                {
                    board.instance.FreeSwitch = false;
                }
                press = true;
                board.instance.offHint();
                board.instance.index = 0;
                board.instance.move = true;
                board.instance.moveCircle = true;
                GameManager.Instance.SwipeTxt.GetComponent<Text>().text = board.Swipe.ToString();
                board.timer = 0;
                PosUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Calculate();
                MusicManager.instance.PlaySound(MusicManager.instance.swipe);
            }
            else
            {
               /* int i = UnityEngine.Random.Range(0, 2);
                if (i == 0)
                {
                    CreateBoomColor();
                }
                else if (i == 1)
                {
                    CreateBoomColor();
                }
                else if (i == 2)
                {
                    CreateBoomVer();
                }*/
            }
        }
    }

    // tinh toan o di chuyen tiep theo
    void Calculate()
    {
        float angle = Mathf.Atan2(PosUp.y - PosDown.y, PosUp.x - PosDown.x) * Mathf.Rad2Deg;
        if(angle>45 && angle <=135 && row < board.height-1)
        {
            MovePiece(Vector2.up);
        }
        else if (angle <= -45 && angle > -135 && row >0)
        {
            MovePiece(Vector2.down);
        }
        else if (angle > -45 && angle <= 45 && column < board.width-1)
        {
            MovePiece(Vector2.right);
        }
        else if (((angle > 135 && angle <= 180) || (angle > -180&& angle <= -135)) && column > 0)
        {
            MovePiece(Vector2.left);
        }
    }

    // di chuyen den o tiep theo
    void MovePiece(Vector2 pos)
    {
        if (board.Emptys[column + (int)pos.x, row + (int)pos.y] != null)
        {
            board.CurrentState = State.move;
            OtherDot = board.Emptys[column + (int)pos.x, row + (int)pos.y];
            board.CurrentDot = this;
            GameObject thisParticle = Instantiate(board.swipeParticles, new Vector2(column + (int)pos.x, row + (int)pos.y), Quaternion.identity, board.Emptys[column + (int)pos.x, row + (int)pos.y].gameObject.transform);
            GameObject ortherParticle = Instantiate(board.swipeParticles, new Vector2(column , row ), Quaternion.identity, board.Emptys[column, row].gameObject.transform);
            if (OtherDot != null)
            {
                lastColumn = column;
                lastRow = row;
                OtherDot.column += -(int)pos.x;
                OtherDot.row += -(int)pos.y;
                column += (int)pos.x;
                row += (int)pos.y;
            }

            // check xem tro giup HandSwap co duoc dung hay khong
            if (board.instance.HandSwap)
            {
                board.DesAllItemDuplicate();
                board.instance.HandSwap = false;
            }
            else
            {
                // neu khong con phan tu nao di chuyen thi se check xem piece vua di chuyen co tao cap 3 hay khong
                if(CheckMove())
                {
                    StartCoroutine(ReturnPos());
                }
            }
            
        }
    }


    // kiem tra xem co tao cap 3 tro len hay khong
    void CheckDuplicate()
    {
        if(column > 0 && column < board.width -1)
        {
            if (board.Emptys[column -1,row] != null && board.Emptys[column +1,row] != null)
            {
                empty left = board.Emptys[column -1,row];
                empty right = board.Emptys [column +1,row];
                if(left.tag == this.gameObject.tag && right.tag == this.gameObject.tag)
                {
                    left.duplicate = true;
                    right.duplicate = true;
                    duplicate = true;
                }
            }
        }
        if(row>0 && row < board.height -1)
        {
            if (board.Emptys[column, row -1] != null && board.Emptys[column,row +1] != null)
            {
                empty down = board.Emptys[column , row -1];
                empty up = board.Emptys[column, row+1];
                if (up.tag == this.gameObject.tag && down.tag == this.gameObject.tag)
                {
                    up.duplicate = true;
                    down.duplicate = true;
                    duplicate = true;
                }
            }
        }
    }

    // neu piece khong tao cap 3 se tro ve vi tri cu
    IEnumerator ReturnPos()
    {
        yield return new WaitForSeconds(0.3f);
        if(OtherDot != null)
        {
            if (!duplicate && !OtherDot.duplicate)
            {
                OtherDot.column = column;
                OtherDot.row = row;
                column = (int)lastColumn;
                row = (int)lastRow;
                board.CurrentState = State.stop;
                if (board.instance.Swipe <= 0 && !mission.instance.FinishMission())
                {
                    board.instance.CurrentState = State.wait;
                    yield return new WaitForSeconds(1f);
                    MusicManager.instance.SoundGround.Stop();
                    MusicManager.instance.PlaySound(MusicManager.instance.lose);
                    GameManager.Instance.SetLevelLose();
                    TimeManager.instance.Lose();
                }

            }
            else
            {
                board.instance.createBoom = true;
                board.DesAllItemDuplicate();
            }
        }
    }

    // tra item ve pool khi destroy
    public void OffPoolEmpty()
    {
        duplicate = false;
        move = false;
        line = false;
        press = false;
        board.instance.Emptys[column, row] = null;
        this.gameObject.transform.GetChild(0).GetComponent<offAnim>().SetOffAnimHint();
        this.gameObject.SetActive(false);
    }

    // tao boom ngang
    public void CreateBoomHoz()
    {
        duplicate = false;
        IsBoomHoz = true;
        IsBoomVer = false;
        IsBoomAdjasted = false;
        IsBoomColor = false;


        MusicManager.instance.PlaySound(MusicManager.instance.createBoom);
        if(this.tag == "blue")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.blueHoz.GetComponent<SpriteRenderer>().sprite;
        }else if(this.tag == "green")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.greenHoz.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "red")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.redHoz.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "yellow")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.yellowHoz.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "orange")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.orangeHoz.GetComponent<SpriteRenderer>().sprite;
        }
        GameObject boom = board.instance.GetObjectPool("createBoom", column, row);
        QuestManager.instance.DameQuest(this.gameObject.tag + "Hoz");
        //boom.transform.SetParent(this.gameObject.transform);
        this.gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.7f,0.7f,0);
        this.gameObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
    }
    // tao boom doc
    public void CreateBoomVer()
    {
        duplicate = false;
        IsBoomHoz = false;
        IsBoomVer =  true;
        IsBoomAdjasted = false;
        IsBoomColor = false;
        MusicManager.instance.PlaySound(MusicManager.instance.createBoom);
        if (this.tag == "blue")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.blueVer.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "green")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.greenVer.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "red")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.redVer.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "yellow")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.yellowVer.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "orange")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.orangeVer.GetComponent<SpriteRenderer>().sprite;
        }
        GameObject boom = board.instance.GetObjectPool("createBoom", column, row);
        QuestManager.instance.DameQuest(this.gameObject.tag + "Ver");
        //boom.transform.SetParent(this.gameObject.transform);
        this.gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0);
        this.gameObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
    }
    // tao boom pham vi
    public void CreateBoomAdjast()
    {
        duplicate = false;
        IsBoomHoz = false;
        IsBoomVer = false;
        IsBoomAdjasted = true;
        IsBoomColor = false;
        MusicManager.instance.PlaySound(MusicManager.instance.createBoom);
        if (this.tag == "blue")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.blueAdjast.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "green")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.greenAdjast.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "red")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.redAdjast.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "yellow")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.yellowAdjast.GetComponent<SpriteRenderer>().sprite;
        }
        else if (this.tag == "orange")
        {
            this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.orangeAdjast.GetComponent<SpriteRenderer>().sprite;
        }
        GameObject boom = board.instance.GetObjectPool("createBoom", column, row);
        QuestManager.instance.DameQuest(this.gameObject.tag+"Adjast");
        //boom.transform.SetParent(this.gameObject.transform);
        this.gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0);
        this.gameObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
    }
    // tao boom mau
    public void CreateBoomColor()
    {
        duplicate = false;
        IsBoomHoz = false;
        IsBoomVer = false;
        IsBoomAdjasted = false;
        IsBoomColor = true;
        MusicManager.instance.PlaySound(MusicManager.instance.createBoom);
        this.gameObject.tag = "boomColor";
        GameObject boom = board.instance.GetObjectPool("createBoom", column, row);
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = board.instance.BoomColor.GetComponent<SpriteRenderer>().sprite;
        /*if (!PieceNotBoom() && !IsBoomColor)
        {
            this.gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0);
            Debug.Log(this.gameObject.transform.GetChild(0).gameObject.transform.localScale.x);
        }*/
        QuestManager.instance.DameQuest("boomColor");
    }

    // kiem tra xem piece nay khong phai la boom
    public bool PieceNotBoom()
    {
        if(IsBoomAdjasted)
            return false;
        if(IsBoomHoz)
            return false;
        if(IsBoomVer)
            return false;
        if (IsBoomColor)
            return false;

        return true;
    }

    // kiem tra xem 2 piece vua swap cos piece nao la boom hay khong
    bool CheckMove()
    {
        if (board.instance.CurrentDot != null && OtherDot != null)
        {
            if ((board.instance.CurrentDot.PieceNotBoom() && OtherDot.IsBoomColor) || (board.instance.CurrentDot.IsBoomColor && OtherDot.PieceNotBoom())
                || (!board.instance.CurrentDot.PieceNotBoom() && !OtherDot.PieceNotBoom())
                )
            {
                return false;
            }
        }
        return true;
    }
    //kiem tra xem 2 vien piece co phai la 2 vien deu la boom khong
    void CheckBoom()
    {
        line = true;
        //color and not boom
        if ((board.instance.CurrentDot.IsBoomColor && OtherDot.PieceNotBoom()) || (board.instance.CurrentDot.PieceNotBoom() && OtherDot.IsBoomColor))
        {
            if (board.instance.CurrentDot.IsBoomColor && OtherDot.PieceNotBoom())
            {
                DesPieceColorCo(OtherDot.gameObject.tag);
            }
            else if (OtherDot.IsBoomColor && board.instance.CurrentDot.PieceNotBoom())
            {
                line = true;
                OtherDot.DesPieceColorCo(board.instance.CurrentDot.gameObject.tag);
                
            }
            return;
        }

        //color and color
        if((board.instance.CurrentDot.IsBoomColor && OtherDot.IsBoomColor) || (board.instance.CurrentDot.IsBoomColor && OtherDot.IsBoomColor))
        {
            StartCoroutine(DesColorAndColor());
        }
        //color and adjast
        if ((board.instance.CurrentDot.IsBoomColor && OtherDot.IsBoomAdjasted) || (board.instance.CurrentDot.IsBoomAdjasted && OtherDot.IsBoomColor))
        {
            if (board.instance.CurrentDot.IsBoomColor && OtherDot.IsBoomAdjasted)
            {
                DesPieceColorAndAdjastCo(board.instance.CurrentDot.gameObject,OtherDot.tag,"adjast_");
            }
            else if (board.instance.CurrentDot.IsBoomAdjasted && OtherDot.IsBoomColor)
            {
                line = true;
                OtherDot.gameObject.tag = this.gameObject.tag;
                OtherDot.DesPieceColorAndAdjastCo(OtherDot.gameObject,board.instance.CurrentDot.tag, "adjast_");

            }
            return;
        }
        //color and hoz
        if ((board.instance.CurrentDot.IsBoomColor && OtherDot.IsBoomHoz) || (board.instance.CurrentDot.IsBoomHoz && OtherDot.IsBoomColor))
        {
            if (board.instance.CurrentDot.IsBoomColor && OtherDot.IsBoomHoz)
            {
                DesPieceColorAndAdjastCo(board.instance.CurrentDot.gameObject, OtherDot.tag, "column_");
            }
            else if (board.instance.CurrentDot.IsBoomHoz && OtherDot.IsBoomColor)
            {
                line = true;
                OtherDot.gameObject.tag = this.gameObject.tag;
                OtherDot.DesPieceColorAndAdjastCo(OtherDot.gameObject, board.instance.CurrentDot.tag, "column_");

            }
            return;
        }
        //color and ver
        //color and hoz
        if ((board.instance.CurrentDot.IsBoomColor && OtherDot.IsBoomVer) || (board.instance.CurrentDot.IsBoomVer && OtherDot.IsBoomColor))
        {
            if (board.instance.CurrentDot.IsBoomColor && OtherDot.IsBoomVer)
            {
                DesPieceColorAndAdjastCo(board.instance.CurrentDot.gameObject, OtherDot.tag, "row_");
            }
            else if (board.instance.CurrentDot.IsBoomVer && OtherDot.IsBoomColor)
            {
                line = true;
                OtherDot.gameObject.tag = this.gameObject.tag;
                OtherDot.DesPieceColorAndAdjastCo(OtherDot.gameObject, board.instance.CurrentDot.tag, "row_");

            }
            return;
        }

        //adjast and hoz or ver
        if ((board.instance.CurrentDot.IsBoomAdjasted && OtherDot.IsBoomAdjasted)
            || (board.instance.CurrentDot.IsBoomAdjasted && OtherDot.IsBoomHoz) || (board.instance.CurrentDot.IsBoomAdjasted && OtherDot.IsBoomVer)
           || (board.instance.CurrentDot.IsBoomHoz && OtherDot.IsBoomAdjasted) || (board.instance.CurrentDot.IsBoomVer && OtherDot.IsBoomAdjasted))
        {
            StartCoroutine(DesPieceAdjastAndHozOrVer());
            return;
        }

        //hoz and ver
        if ((board.instance.CurrentDot.IsBoomHoz && OtherDot.IsBoomHoz) || (board.instance.CurrentDot.IsBoomVer && OtherDot.IsBoomVer)
            || (board.instance.CurrentDot.IsBoomHoz && OtherDot.IsBoomVer) || (board.instance.CurrentDot.IsBoomVer && OtherDot.IsBoomHoz))
        {
            StartCoroutine(DesPieceColumnAndRow());
            return;
        }


    }

    //color and color
    IEnumerator DesColorAndColor()
    {
        List<GameObject> color = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null && board.instance.Emptys[i, j] != this.gameObject && board.instance.Emptys[i,j].PieceNotBoom())
                {
                    if (!color.Contains(board.instance.Emptys[i, j].gameObject))
                    {
                        board.instance.Emptys[i, j].GetComponent<empty>().CreateBoomColor();
                        color.Add(board.instance.Emptys[i, j].gameObject);
                    }
                }
                if(board.instance.BlankSpace[i, j])
                {
                    if(board.instance.BrickSpace[i, j] != null)
                    {
                        board.BrickSpace[i, j].GetComponent<brick>().TakeDame(1, i, j);
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < color.Count; i++)
        {
            if (color[i] != null)
            {
                MusicManager.instance.PlaySound(MusicManager.instance.desPiece);
                if (board.instance.AllJelly[color[i].GetComponent<empty>().column, color[i].GetComponent<empty>().row] != null)
                {
                    board.instance.AllJelly[color[i].GetComponent<empty>().column, color[i].GetComponent<empty>().row].gameObject.GetComponent<jelly>().TakeDame(1, color[i].GetComponent<empty>().column, color[i].GetComponent<empty>().row);
                }
                mission.instance.TakeALlItem(board.instance.Emptys[color[i].GetComponent<empty>().column, color[i].GetComponent<empty>().row].gameObject.tag);
                color[i].GetComponent<empty>().OffPoolEmpty();
            }
        }
        duplicate = true;
        board.instance.DesAllItemDuplicate();
    }

    //color and piece
    void DesPieceColorCo(string tag)
    {
        StartCoroutine(DesPieceColor(tag));
    }
    IEnumerator DesPieceColor(string tag)
    {
        line = true;
        List<GameObject> colors = new List<GameObject>();
        List<GameObject> lines = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                if (!board.instance.BlankSpace[i,j] && board.instance.Emptys[i,j] != null && board.instance.Emptys[i, j].gameObject.tag == tag && board.instance.Emptys[i, j].gameObject != this.gameObject)
                {
                    if (board.instance.Emptys[i,j].PieceNotBoom())
                    {
                        if(!colors.Contains(board.instance.Emptys[i, j].gameObject))
                        {
                            colors.Add(board.instance.Emptys[i, j].gameObject);
                        }
                    }
                    else if(!board.instance.Emptys[i, j].PieceNotBoom())
                    {
                        CheckAll.instance.CheckSingleBoom(board.instance.Emptys[i,j]);
                        if (!colors.Contains(board.instance.Emptys[i, j].gameObject))
                        {
                            colors.Add(board.instance.Emptys[i, j].gameObject);
                        }
                    }
                    GameObject line = Instantiate(board.instance.LineColor);
                    line.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, this.gameObject.transform.position);
                    line.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, new Vector3(i, j, 0));
                    lines.Add(line);
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }

        for (int i = 0; i < colors.Count; i++)
        {
            if (colors[i] != null)
            {
                if (i < lines.Count)
                {
                    Destroy(lines[i]);
                }
                    
                MusicManager.instance.PlaySound(MusicManager.instance.press);
                if (board.instance.AllJelly[colors[i].GetComponent<empty>().column, colors[i].GetComponent<empty>().row] != null)
                {
                    board.instance.AllJelly[colors[i].GetComponent<empty>().column, colors[i].GetComponent<empty>().row].gameObject.GetComponent<jelly>().TakeDame(1, colors[i].GetComponent<empty>().column, colors[i].GetComponent<empty>().row);
                }
                mission.instance.TakeALlItem(board.instance.Emptys[colors[i].GetComponent<empty>().column, colors[i].GetComponent<empty>().row].gameObject.tag);
                colors[i].GetComponent<empty>().OffPoolEmpty();
                yield return new WaitForSeconds(0.02f);
            }
        }
        this.duplicate = true;
        board.instance.DesAllItemDuplicate();
    }

    //color and adjast
    void DesPieceColorAndAdjastCo(GameObject obj,string tag ,string typeBoom)
    {
         StartCoroutine(DesPieceColorAndAdjast(obj,tag,typeBoom));
    }
    IEnumerator DesPieceColorAndAdjast(GameObject obj, string tag, string typeBoom)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                if (!board.instance.BlankSpace[i,j] && board.instance.Emptys[i,j] != null && board.instance.Emptys[i,j].gameObject.tag == tag)
                {
                    if( typeBoom =="adjast_")
                    {
                        board.instance.Emptys[i, j].CreateBoomAdjast();
                    }
                    else if(typeBoom == "column_")
                    {
                        board.instance.Emptys[i, j].CreateBoomHoz();
                    }
                    else if(typeBoom == "row_")
                    {
                        board.instance.Emptys[i, j].CreateBoomVer();
                    }
                   
                    if(!list.Contains(board.instance.Emptys[i, j].gameObject))
                    {
                        list.Add(board.instance.Emptys[i, j].gameObject);
                    }
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                List<empty> adjast = new List<empty>();
                if (typeBoom == "adjast_")
                {
                    adjast = CheckAll.instance.GetadjastedBoom(list[i].GetComponent<empty>().column, list[i].GetComponent<empty>().row);
                }
                else if (typeBoom == "column_")
                {
                    adjast = CheckAll.instance.GetColumnBoom(list[i].GetComponent<empty>().row);
                }
                else if (typeBoom == "row_")
                {
                    adjast = CheckAll.instance.GetRowBoom(list[i].GetComponent<empty>().column);
                }
                
                for (int j = 0; j < adjast.Count; j++)
                {
                    if (adjast[j].gameObject != obj)
                    {
                        MusicManager.instance.PlaySound(MusicManager.instance.Clear);
                        if (board.instance.AllJelly[adjast[j].GetComponent<empty>().column, adjast[j].GetComponent<empty>().row] != null)
                        {
                            board.instance.AllJelly[adjast[j].GetComponent<empty>().column, adjast[j].GetComponent<empty>().row].gameObject.GetComponent<jelly>().TakeDame(1, adjast[j].GetComponent<empty>().column, adjast[j].GetComponent<empty>().row);
                        }
                        mission.instance.TakeALlItem(board.instance.Emptys[adjast[j].GetComponent<empty>().column, adjast[j].GetComponent<empty>().row].gameObject.tag);
                        board.instance.CheckBrick(adjast[j].GetComponent<empty>().column, adjast[j].GetComponent<empty>().row);
                        adjast[j].OffPoolEmpty();
                    }
                    
                }
                yield return new WaitForSeconds(0.04f);
            }
        }

        duplicate = true;
        board.instance.DesAllItemDuplicate();
    }

    //adjast and ver or hoz
    IEnumerator DesPieceAdjastAndHozOrVer()
    {
        line = true;
        if (column>0)
        {
            CheckAll.instance.AllItemDestroy.Add(board.instance.Emptys[column-1,row]);
            CheckAll.instance.GetRowBoom(column-1);
        }
        CheckAll.instance.AllItemDestroy.Add(board.instance.Emptys[column, row]);
        CheckAll.instance.GetRowBoom(column );
        if (column<board.instance.width-1)
        {
            CheckAll.instance.AllItemDestroy.Add(board.instance.Emptys[column + 1, row]);
            CheckAll.instance.GetRowBoom(column +1);
        }
        if (row>0)
        {
            CheckAll.instance.GetColumnBoom(row-1);
        }
        CheckAll.instance.GetColumnBoom(row );
        if (row <board.instance.height-1)
        {
            CheckAll.instance.GetColumnBoom(row+1);
        }
        yield return null;
        duplicate = true;
        board.instance.DesAllItemDuplicate();
    }

    //boomhoz and boom ver
    IEnumerator DesPieceColumnAndRow()
    {
        line = true;
        CheckAll.instance.AllItemDestroy.Add(board.instance.CurrentDot);
        CheckAll.instance.AllItemDestroy.Add(OtherDot);
        yield return null;
        CheckAll.instance.GetColumnBoom(row);
        CheckAll.instance.GetRowBoom(column);
        for (int i = 0; i < CheckAll.instance.AllItemDestroy.Count; i++)
        {
            if (CheckAll.instance.AllItemDestroy[i] != null && CheckAll.instance.AllItemDestroy[i] != this.gameObject.GetComponent<empty>())
            {
                board.instance.Emptys[CheckAll.instance.AllItemDestroy[i].column, CheckAll.instance.AllItemDestroy[i].row].OffPoolEmpty();
            }
        }

        yield return null;
        duplicate = true;
        board.instance.DesAllItemDuplicate();
    }
}
