
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// trang thai man choi
public enum State
{
    move,
    stop,
    wait,
    help,
    ui,
}

// cac kieu o
public enum StateTypePiece
{
    none,
    blank,
    brick1,
    brick2,
    brick3,
    padding,
}

[System.Serializable]
public struct Piece 
{
    public StateTypePiece TypePiece;
    public int column;
    public int row;
}

[System.Serializable]
public struct PrefabPooling
{
    public GameObject prefab;
    public string name_;
    public int index;
}

[System.Serializable]
public struct corner
{
    public int column;
    public int row;
}

public class board : MonoBehaviour
{
    public static board instance;
    // doi tuong trong hierarchy
    public GameObject BackGround;
    public GameObject LevelTxt;
    public GameObject swipeParticles;
    public GameObject clearEffecHoztprefab;
    public GameObject clearEffectVerprefab;
    public GameObject x; // gameObject Parent de chua cac piece khi tao ra;
    public GameObject boomAdjastEffect; // hieu ung boom pham vi;
    private GameObject groundTittle; // nen background cua piece
    public GameObject paddingMove; // nen background cua o Move;
    public float TimeHint;
    [HideInInspector] public empty CurrentDot;
    [HideInInspector] public GameObject[,] AllJelly;
    [HideInInspector] public empty[,] Emptys;
    [HideInInspector] public bool[,] BlankSpace;
    [HideInInspector] public GameObject[,] BrickSpace;
    [HideInInspector] private List<GameObject> dott;
    // pooling prefab piece particle
    [HideInInspector] public Piece[] AllTypePieces; // cac loai piece
    public List<PrefabPooling> AllPools;
    [HideInInspector] public Dictionary<string, GameObject> PoolQueuePrefab;
    // clearItem , clearItemHoz , clearItemVer
    [HideInInspector] public Dictionary<string,Queue<GameObject>> PoolQueue ;

    //pooling empty
    [HideInInspector] public List<GameObject> poolEmpty = new List<GameObject>()
        ;
    [HideInInspector] public int height;
    [HideInInspector] public int width;
    [HideInInspector] public int starLevel = 0;
    [HideInInspector] public bool HandSwap = false;
    [HideInInspector] public bool FreeSwitch = false;
    [HideInInspector] public bool UFO = false;

    public float offset; // khoang do cao khi create piece
    [HideInInspector] private CameraPos cameraPos;
    private score Score;
    private mission Mission;
    private CheckAll CheckAlls;
    private List<corner> AllCorners;  // danh sach cac goc neu man choi co di chuyen theo vong tron sau khi swap piece
    private int jelly1_index;
    private int jelly2_index;
    public bool move = false;
    public bool moveCircle = false;
    public bool createBoom = false;
    private bool create = false;
    private bool firstGame = true; // lan dau vao game
    public GameObject bonusText; // open bonus text
    public int index = 0; // index bonus
    // cac loai birick , jelly , cac loai piece , boom co trong game
    [Header("brick")]
    public GameObject brick0;
    public GameObject brick1;
    public GameObject brick2;
    public GameObject brick3;
    [Header("jelly")]
    public GameObject jelly1;
    public GameObject jelly2;
    public int Swipe;
    [Header("type piece")]
    [Header("blue")]
    public GameObject blueHoz;
    public GameObject blueVer;
    public GameObject blueAdjast;
    [Header("green")]
    public GameObject greenHoz;
    public GameObject greenVer;
    public GameObject greenAdjast;
    [Header("red")]
    public GameObject redHoz;
    public GameObject redVer;
    public GameObject redAdjast;
    [Header("yellow")]
    public GameObject yellowHoz;
    public GameObject yellowVer;
    public GameObject yellowAdjast;
    [Header("orange")]
    public GameObject orangeHoz;
    public GameObject orangeVer;
    public GameObject orangeAdjast;
    [Header("boom color and line")]
    public GameObject BoomColor;
    public GameObject LineColor;
    public State CurrentState = State.stop;
    public float timer = 0;

    [Header("hint ")]
    public List<GameObject> listHint = new List<GameObject>();
    public List<GameObject> listHintAfter = new List<GameObject>();
    public List<GameObject> listHintIndex = new List<GameObject>();
    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            cameraPos = FindObjectOfType<CameraPos>();
            CheckAlls = FindObjectOfType<CheckAll>();
            Score = FindObjectOfType<score>();
            Mission = FindObjectOfType<mission>();
        }
    }

    
    private void Start()
    {
        SetLevelGame();
        MusicManager.instance.PlayMusicInGame();
    }

    private void Update()
    {
        if(!firstGame)
        {
            if (CurrentState == State.stop)
            {
                timer += Time.deltaTime;
            }
            if (timer >= TimeHint && CurrentState == State.stop && UFO == false)
            {
                StartHint();

            }
        }
    }

    public void SetLevelGame()
    {
        StartCoroutine(SetGameinLevel(LevelManager.Instance.LevelIndex));
    }


    IEnumerator SetGameinLevel(int levelIndex)
    {
        if (levelIndex < LevelManager.Instance.Alllevels.Count)
        {
            LevelTxt.GetComponent<Text>().text =  (levelIndex+1).ToString();
            Score.SetBeginScore(LevelManager.Instance.Alllevels[levelIndex].score);
            BackGround.GetComponent<Image>().sprite = LevelManager.Instance.Alllevels[levelIndex].backGround;
            groundTittle = LevelManager.Instance.Alllevels[levelIndex].groundTittle;
            dott = LevelManager.Instance.Alllevels[levelIndex].dott;
            AllTypePieces = LevelManager.Instance.Alllevels[levelIndex].AllTypePieces;
            AllCorners = LevelManager.Instance.Alllevels[levelIndex].AllCorners;
            height = LevelManager.Instance.Alllevels[levelIndex].height;
            width = LevelManager.Instance.Alllevels[levelIndex].width;
            Swipe = LevelManager.Instance.Alllevels[levelIndex].Swipe;
            Mission.SetItem(LevelManager.Instance.Alllevels[levelIndex].AllMissions);
            jelly1_index = LevelManager.Instance.Alllevels[levelIndex].jelly1;
            jelly2_index = LevelManager.Instance.Alllevels[levelIndex].jelly2;
            GameManager.Instance.SwipeTxt.GetComponent<Text>().text = Swipe.ToString();
            yield return new WaitForSeconds(0.2f);
            Emptys = new empty[width, height];
            BlankSpace = new bool[width, height];
            BrickSpace = new GameObject[width, height];
            AllJelly = new GameObject[width, height];
            cameraPos.SetPos(width, height);
            SetBlankSpace();  // tao cac o trong
            CreateGround(); // tao nen cua piece
            SetBrickPiece(); // tao cac o chua brick
            CreatObjectPooling(); // tao ra pool cac item
            CreateAllEmpty(); // tao ra piece
            DesAllItemDuplicate();// kiem tra xem co piece nao duplicate khong thi xoa di
            /*StartCoroutine(CreateBooster());  // neu nguoi choi co chon booster thi se kich hoat chung*/

        }
    }

    // tao pool chua cac object
    private void CreatObjectPooling()
    {
        PoolQueue = new Dictionary<string, Queue<GameObject>>();
        PoolQueuePrefab = new Dictionary<string, GameObject>();
        if (AllPools != null)
        {
            for (int m = 0; m < AllPools.Count; m++)
            {
                Queue<GameObject> pool = new Queue<GameObject>();
                for (int i = 0; i < AllPools[m].index; i++)
                {
                    if (AllPools[m].prefab != null)
                    {
                        GameObject obj = Instantiate(AllPools[m].prefab);
                        obj.name = AllPools[m].name_;
                        obj.gameObject.SetActive(false);
                        pool.Enqueue(obj);
                        if (!PoolQueuePrefab.ContainsKey(AllPools[m].name_))
                        {
                            PoolQueuePrefab[AllPools[m].name_] = obj;
                        }
                    }
                }
                if (!PoolQueue.ContainsKey(AllPools[m].name_))
                {
                    PoolQueue[AllPools[m].name_] = pool;
                }
            }
        }
    }

    public GameObject GetObjectPool(string name, int i , int j)
    {
        if(PoolQueue.ContainsKey(name))
        {
            if (PoolQueue[name].Count > 0)
            {
                GameObject obj = PoolQueue[name].Dequeue();
                if(obj !=null)
                {
                    obj.SetActive(true);
                    obj.transform.position = new Vector2(i, j);
                    return obj;
                }
            }
            else
            {
                if (PoolQueuePrefab.ContainsKey(name))
                {
                    GameObject obj1 = Instantiate(PoolQueuePrefab[name]);
                    obj1.name = name;
                    obj1.SetActive(true);
                    obj1.transform.position = new Vector2(i, j);
                    return obj1;
                }
            }
        }
        return null;
    }

    // lay ra hoac dua lai vao pool
    public void SetObjectPool(GameObject obj)
    {
        if (PoolQueue.ContainsKey(obj.name))
        {
            obj.SetActive(false);
            PoolQueue[obj.name].Enqueue(obj);
        }
    }


    // create nen cua piece
    void CreateGround()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                if (!BlankSpace[i, j])
                {
                    GameObject ground = Instantiate(groundTittle, new Vector2(i, j), Quaternion.identity, x.transform) as GameObject;
                }
                else
                {
                    for (int k = 0; k < AllTypePieces.Length; k++)
                    {
                        if (AllTypePieces[k].column == i && AllTypePieces[k].row == j)
                        {
                            if (AllTypePieces[k].TypePiece != StateTypePiece.blank)
                            {
                                GameObject ground = Instantiate(groundTittle, new Vector2(i, j), Quaternion.identity, x.transform) as GameObject;
                            }
                        }

                    }
                
                }
                
            }
        }
        CreateJelly(); // tao cac o jelly neu co
    }

    // create jelly neu level nay co
    void CreateJelly()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                bool check = true;
                if (!BlankSpace[j,i])
                {
                    if (jelly2_index > 0)
                    {
                        GameObject pad = Instantiate(jelly2, new Vector2(j,i), Quaternion.identity, x.transform);
                        AllJelly[j,i] = pad;
                        jelly2_index--;
                    }
                    else
                    {
                        check = false;
                    }
                    if (jelly2_index <= 0 && jelly1_index > 0 && !check)
                    {
                        GameObject pad = Instantiate(jelly1, new Vector2(j,i), Quaternion.identity, x.transform);
                        AllJelly[j,i] = pad;
                        jelly1_index--;
                    }
                    
                }
            }
        }


        CreatePaddingMove(); // tao cac o di chuyen hinh tron neu co
    }

    // create padding move neu level nay co
    void CreatePaddingMove()
    {
        if(AllCorners.Count>=4)
        {
            //up
            for (int i = AllCorners[0].column; i < AllCorners[1].column; i++)
            {
                GameObject padding = Instantiate(paddingMove, new Vector2(i, AllCorners[0].row),Quaternion.identity,x.transform);
            }

            //down
            for (int i = AllCorners[3].column; i < AllCorners[2].column; i++)
            {
                GameObject padding = Instantiate(paddingMove, new Vector2(i, AllCorners[3].row), Quaternion.identity, x.transform);
            }
            //right
            for (int i = AllCorners[2].row; i <= AllCorners[1].row; i++)
            {
                GameObject padding = Instantiate(paddingMove, new Vector2(AllCorners[2].column,i), Quaternion.identity, x.transform);
            }
            //left
            for (int i = AllCorners[3].row+1; i <= AllCorners[0].row-1; i++)
            {
                GameObject padding = Instantiate(paddingMove, new Vector2(AllCorners[3].column, i), Quaternion.identity, x.transform);
            }
        }
    }

    // thiet lap cac o trong Blank
    void SetBlankSpace()
    {
        for (int i = 0; i < AllTypePieces.Length; i++)
        {
            if (AllTypePieces[i].TypePiece != StateTypePiece.none && AllTypePieces[i].TypePiece != StateTypePiece.padding)
            {
                BlankSpace[AllTypePieces[i].column, AllTypePieces[i].row] = true;
            }
        }
    }

    // create cac o chua brick
    private void SetBrickPiece()
    {
        for (int i = 0; i < AllTypePieces.Length; i++)
        {
            if (AllTypePieces[i].TypePiece == StateTypePiece.brick1)
            {
                GameObject brick = Instantiate(brick1,new Vector2(AllTypePieces[i].column, AllTypePieces[i].row),Quaternion.identity,this.transform);
                BrickSpace[AllTypePieces[i].column, AllTypePieces[i].row] = brick;
            }
            else if (AllTypePieces[i].TypePiece == StateTypePiece.brick2)
            {
                GameObject brick = Instantiate(brick2, new Vector2(AllTypePieces[i].column, AllTypePieces[i].row), Quaternion.identity, this.transform);
                BrickSpace[AllTypePieces[i].column, AllTypePieces[i].row] = brick;
            }
            else if (AllTypePieces[i].TypePiece == StateTypePiece.brick3)
            {
                GameObject brick = Instantiate(brick3, new Vector2(AllTypePieces[i].column, AllTypePieces[i].row), Quaternion.identity, this.transform);
                BrickSpace[AllTypePieces[i].column, AllTypePieces[i].row] = brick;
            }
        }
    }

    // tao ra tat ca cac piece trong level
    void CreateAllEmpty()
    {

        for (int i = 0; i < (height+2)*(height+2) ; i++)
        {
            int index = Random.Range(0, dott.Count);
            GameObject empty = Instantiate(dott[index], new Vector2(0,0), Quaternion.identity, this.transform);
            string tag = dott[index].name.Split(' ')[0];
            empty.tag = tag;
            poolEmpty.Add(empty);
            empty.SetActive(false);
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GetEmptyPool(i,j,false);
            }
        }
    }

    // tao ra piece neu pool het item
    void CreateEmptys(int i , int j,bool check)
    {
        if (!BlankSpace[i, j] && Emptys[i, j] == null)
        {
            int index = Random.Range(0, dott.Count);
            if (check)
            {
                
                int count = 0;
                while (CheckDuplicateCo(i, j, dott[index].name) && count < 100)
                {
                    index = Random.Range(0, dott.Count);
                    count++;
                }
                count = 0;
            }
            GameObject empty = Instantiate(dott[index], new Vector2(i, height - 1 + offset), Quaternion.identity, this.transform);
            string tag = dott[index].name.Split(' ')[0];
            empty.tag = tag;
            empty.name = ("(" + i + "," + j + ")").ToString();
            empty.GetComponent<empty>().column = i;
            empty.GetComponent<empty>().row = j;
            Emptys[i, j] = empty.GetComponent<empty>();
            poolEmpty.Add(empty);
        }
    }

    // lay ra item trong pool
    public void GetEmptyPool(int i , int j,bool check)
    {
        if (!BlankSpace[i, j] && Emptys[i, j] == null)
        {
            int index = Random.Range(0,dott.Count);
            int count = 0;
            while (CheckDuplicateCo(i, j, dott[index].name) && count < 100)
            {
                index = Random.Range(0, dott.Count);
                count++;
            }
            count = 0;
            int random = Random.Range(0, 4);
            foreach (var item in poolEmpty)
            {
                if(random == 0)
                {
                    if (item != null && !item.activeInHierarchy && item.tag == dott[index].name)
                    {
                        item.SetActive(true);
                        item.transform.GetChild(0).GetComponent<offAnim>().SetOffAnimHint();
                        item.transform.GetChild(0).transform.localPosition = Vector3.zero;
                        item.name = ("(" + i + "," + j + ")").ToString();
                        item.transform.localPosition = new Vector2(i, height - 1 + offset);
                        item.GetComponent<empty>().column = i;
                        item.GetComponent<empty>().row = j;
                        Emptys[i, j] = item.GetComponent<empty>();
                        return;
                    }
                }
                else
                {
                    if (item != null && !item.activeInHierarchy && item.tag == dott[index].name && item.GetComponent<empty>().PieceNotBoom())
                    {
                        item.SetActive(true);
                        item.transform.GetChild(0).GetComponent<offAnim>().SetOffAnimHint();
                        item.transform.GetChild(0).transform.localPosition = Vector3.zero;
                        item.name = ("(" + i + "," + j + ")").ToString();
                        item.transform.position = new Vector2(i, height - 1 + offset);
                        item.GetComponent<empty>().column = i;
                        item.GetComponent<empty>().row = j;
                        Emptys[i, j] = item.GetComponent<empty>();
                        return;
                    }
                }
                
            }
            CreateEmptys(i, j, false);
        }
    }

    // kick hoat booster theo nguoi choi da chon
    IEnumerator CreateBooster()
    {
        yield return new WaitForSeconds(1f);
        if (LevelManager.Instance.booster_1 && SaveManager.instance.LoadChooseHelp("boost_1") > 0)
        {
           
            SaveManager.instance.SaveHelp("boost_1", -1);
            bool find = false;
            // create color boom
            for (int i = (int)(width / 2); i < width; i++)
            {
                for (int j = (int)(height / 2); j < height; j++)
                {
                    if (find)
                        break;
                    if (!BlankSpace[i, j] && Emptys[i, j] != null && Emptys[i, j].PieceNotBoom())
                    {
                        Emptys[i, j].CreateBoomColor();
                        find = true;
                        break;
                    }

                }
            }
        }

        if (LevelManager.Instance.booster_2 && SaveManager.instance.LoadChooseHelp("boost_2") > 0)
        {
            // add random quality help in game
            Swipe += 5;
            GameManager.Instance.SwipeTxt.GetComponent<Text>().text = Swipe.ToString();
            SaveManager.instance.SaveHelp("boost_2", -1);
        }

        if (LevelManager.Instance.booster_3 && SaveManager.instance.LoadChooseHelp("boost_3") > 0)
        {
            SaveManager.instance.SaveHelp("boost_3", -1);
            // best booster
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!BlankSpace[i, j] && Emptys[i, j] != null)
                    {
                        Emptys[i, j].duplicate = true;
                    }

                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        DesAllItemDuplicate();
    }


    // kiem tra xem co tao cap 3 hay khong
    bool CheckDuplicateCo(int i, int j, string tag)
    {
        if (Emptys[i,j]!= null)
        {
            if (i > 1)
            {
                if (Emptys[i - 1, j] != null && Emptys[i - 2, j] != null)
                {
                    empty left1 = Emptys[i - 1, j];
                    empty left2 = Emptys[i - 2, j];
                    if (left1.tag == tag && left2.tag == tag)
                    {
                        return true;
                    }
                }

            }
            if (j > 1)
            {
                if (Emptys[i, j - 1] != null && Emptys[i, j - 2] != null)
                {
                    empty down1 = Emptys[i, j - 1];
                    empty down2 = Emptys[i, j - 2];
                    if (down1.tag == tag && down2.tag == tag)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // tao boom vao vi tri da swap
    void CreateBoom()
    {
        // tao boom ngang hoac doc
        if (CheckAlls.AllItemDestroy.Count == 4 || CheckAlls.AllItemDestroy.Count == 7)
        {
            
            if(CurrentDot != null)
            {
                if (CheckAlls.AllItemDestroy[0].row == CheckAlls.AllItemDestroy[1].row)
                {
                    if (CurrentDot.duplicate)
                    {
                        CurrentDot.CreateBoomHoz();
                    }
                    else if (CurrentDot.OtherDot.duplicate && CurrentDot.PieceNotBoom())
                    {
                        CurrentDot.OtherDot.CreateBoomHoz();
                    }


                }
                else 
                {
                    if (CurrentDot.duplicate)
                    {
                        CurrentDot.CreateBoomVer();

                    }
                    else if (CurrentDot.OtherDot.duplicate && CurrentDot.PieceNotBoom())
                    {
                        CurrentDot.OtherDot.CreateBoomVer();
     

                    }


                }
            }
        }
        else if(CheckAlls.AllItemDestroy.Count == 5 || CheckAlls.AllItemDestroy.Count >= 8 || (CheckAlls.AllItemDestroy.Count == 6))
        {
            if (CurrentDot != null)
            {
                if(CheckDuplicateFivePiece() || CheckAlls.AllItemDestroy.Count >= 8) // tao boom color
                {
                    if (CurrentDot.duplicate)
                    {
                        CurrentDot.CreateBoomColor();

                    }
                    else if (CurrentDot.OtherDot.duplicate && CurrentDot.PieceNotBoom())
                    {
                        CurrentDot.OtherDot.CreateBoomColor();

                    }
                }
                else    // tap boom pham vi
                {
                    if (CurrentDot.duplicate)
                    {
                        CurrentDot.CreateBoomAdjast();

                    }
                    else if (CurrentDot.OtherDot.duplicate && CurrentDot.PieceNotBoom())
                    {
                        CurrentDot.OtherDot.CreateBoomAdjast();
                    }

                }
                
            }
        }
    }

    // kiem tra xem co tao ra 5 hang ngang hay 5 hang doc khong
    public bool CheckDuplicateFivePiece()
    {
        int horizontal = 0;
        int vertical = 0;
        empty dot = CheckAlls.AllItemDestroy[0];
        for (int i = 0; i < CheckAlls.AllItemDestroy.Count; i++)
        {
            if(CheckAlls.AllItemDestroy [i].column == dot.column)
                horizontal ++;
            if(CheckAlls.AllItemDestroy[i].row == dot.row)
                vertical ++;
        }

        return (horizontal == 5|| vertical ==5);
    }


    //xoa piece duplicate co trong bang
    void MakeToBoom(int i, int j)
    {
        if (Emptys[i, j].duplicate)
        {
            if(!firstGame)
            {
                MakeDameMission(i, j);
                if (AllJelly[i, j] == null)
                {
                    GameObject desItem = GetObjectPool("break" + Emptys[i, j].gameObject.tag, i, j);
                }
                if (Emptys[i, j].PieceNotBoom() && CurrentState != State.wait)
                {
                    QuestManager.instance.DameQuest(Emptys[i, j].gameObject.tag);
                }
                Score.MakeScore(50);
            }
            
            Emptys[i, j].OffPoolEmpty();
        }
    }

    // neu piece bij destroy co trong mission cua level nay thi se gay dame
    public void MakeDameMission(int i , int j)
    {
        if (CurrentState != State.wait)
        {
            if (AllJelly[i, j] != null)
            {
                AllJelly[i, j].gameObject.GetComponent<jelly>().TakeDame(1,i,j);
            }
            Mission.TakeALlItem(Emptys[i, j].gameObject.tag);
            CheckBrick(i, j);
        }
    }


    //xoa tat ca piece duplicate co trong bang
    public void DesAllItemDuplicate()
    {
        // open bonus text
        if (!firstGame)
        {
            OpenText();
        }
        // kiem tra co lon hon 3 piece khong thi tao boom
        if (CurrentState != State.wait && !firstGame)
        {
            CheckAlls.CheckAllDes();
            // tao boom sau khi swap
            if (createBoom)
            {
                if (CheckAlls.AllItemDestroy.Count >= 4 && !CheckBoomIsRow() && createBoom)
                {
                    CreateBoom();
                    createBoom = false;
                }
            }
            else // tao boom sau khi vien piece tu roi
            {
                CreateBoomAuto.instance.GetAllBoom();
            }
            
        }
        if(CheckItemDuplicate() && !firstGame)
        {
            MusicManager.instance.PlaySound(MusicManager.instance.desPiece);
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i, j])
                {
                    if (Emptys[i, j] != null)
                    {
                        MakeToBoom(i, j);
                    }
                }
            }
        }
        CheckAlls.AllItemDestroy.Clear();
        CheckAlls.circle.Clear();
        CurrentDot = null;
        AfterDestroyCo(); //di chuyen piece xuong sau khi destroy
    }

    // xet bonusText sau khi dat du so luot xo aitem lien tiep
    public void OpenText()
    {
        index++;
        if (index >= 4)
        {
            if (index == 4)
            {
                bonusText.GetComponent<Text>().text = "Divine";
                bonusText.SetActive(true);
                bonusText.GetComponent<Animator>().SetTrigger("run");
            }
            else if (index == 6)
            {
                bonusText.GetComponent<Text>().text = "Perfect";
                bonusText.SetActive(true);
                bonusText.GetComponent<Animator>().SetTrigger("run");
            }
            else if (board.instance.index == 8)
            {
                bonusText.GetComponent<Text>().text = "Dilicious";
                index = 0;
                bonusText.SetActive(true);
                bonusText.GetComponent<Animator>().SetTrigger("run");
            }
            
        }
    }

    // kiem tra xem trong cac piece destroy co piece nao la boom hay khong , neu khong thi moi tao boom
    bool CheckBoomIsRow()
    {
        for (int i = 0; i < CheckAlls.AllItemDestroy.Count; i++)
        {
            if (CheckAlls.AllItemDestroy[i] != null && !CheckAlls.AllItemDestroy[i].GetComponent<empty>().PieceNotBoom())
            {
                return true;
            }
        }
        return false;
    }

    // di chuyen piece xuong theo duong thang nhung bi gioi han boi brick
    public IEnumerator DeCreaseRow1()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i,j])
                {
                    if (Emptys[i, j] == null)
                    {
                        for (int k = j + 1; k < height; k++)
                        {
                            if (BrickSpace[i, k])
                                break;
                            if (Emptys[i, k] != null)
                            {
                                Emptys[i, k].column = i;
                                Emptys[i, k].row = j;
                                Emptys[i, k] = null;
                                break;
                            }
                            
                        }
                    }
                }
            }
        }
        create = true;
        yield return new WaitForSeconds(0.001f);
    }
    // di chuyen piece xuong theo duong thang xuong lap day tat ca cac o
    public IEnumerator DeCreaseRowFull()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i, j])
                {
                    if (Emptys[i, j] == null)
                    {
                        for (int k = j + 1; k < height; k++)
                        {
                            if (Emptys[i, k] != null)
                            {
                                Emptys[i, k].column = i;
                                Emptys[i, k].row = j;
                                Emptys[i, k] = null;
                                break;
                            }

                        }
                    }
                }
            }
        }
        create = true;
        yield return new WaitForSeconds(0.001f);
    }

    // giam row piece xuong va co check di chuyen cheo
    public void DeCreaseRow()
    {
        StartCoroutine(DeCreaseRow2());
    }
    public IEnumerator DeCreaseRow2()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (BrickSpace[i, j])
                    break;
                if (!BlankSpace[i, j] && Emptys[i,j] != null)
                {
                    GameObject empty = Emptys[i, j].gameObject;
                    bool move = true;
                    while (move)
                    {
                        if (empty != null && empty.GetComponent<empty>().row > 0)
                        {
                            if (!BlankSpace[empty.GetComponent<empty>().column, empty.GetComponent<empty>().row - 1] && Emptys[empty.GetComponent<empty>().column, empty.GetComponent<empty>().row - 1] == null)
                            {
                                int x = empty.GetComponent<empty>().column;
                                int y = empty.GetComponent<empty>().row;
                                empty.GetComponent<empty>().row--;
                                Emptys[x, y] = null;
                            }
                            else
                            {
                                bool check = false;
                                if (empty.GetComponent<empty>().column > 0)
                                {
                                    if (!BlankSpace[empty.GetComponent<empty>().column - 1, empty.GetComponent<empty>().row - 1] && Emptys[empty.GetComponent<empty>().column - 1, empty.GetComponent<empty>().row - 1] == null)
                                    {
                                        int x = empty.GetComponent<empty>().column;
                                        int y = empty.GetComponent<empty>().row;
                                        empty.GetComponent<empty>().column--;
                                        empty.GetComponent<empty>().row--;
                                        Emptys[x, y] = null;
                                        check = true;
                                    }
                                }
                                if (empty.GetComponent<empty>().column < width - 1 && !check)
                                {
                                    if (!BlankSpace[empty.GetComponent<empty>().column + 1, empty.GetComponent<empty>().row - 1] && Emptys[empty.GetComponent<empty>().column + 1, empty.GetComponent<empty>().row - 1] == null)
                                    {
                                        int x = empty.GetComponent<empty>().column;
                                        int y = empty.GetComponent<empty>().row;
                                        empty.GetComponent<empty>().column++;
                                        empty.GetComponent<empty>().row--;
                                        Emptys[x, y] = null;
                                        check = true;
                                    }
                                }

                                if (!check)
                                {
                                    move = false;
                                }
                            }
                        }
                        else
                        {
                            move = false;
                        }
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
        }
        create = true;
        yield return new WaitForSeconds(0.1f);
        AfterDestroyCo();
    }
    

    // kiem tra xem o (row - 1) ben trai phai co trong hay khong
    public bool CheckMove()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(j>0)
                {
                    if (!BlankSpace[i,j] && Emptys[i,j] != null)
                    {
                        if (BlankSpace[i, j - 1] || Emptys[i, j - 1] != null)
                        {
                            if (i > 0)
                            {
                                if (!BlankSpace[i - 1, j - 1] && Emptys[i - 1, j - 1] == null)
                                {
                                    return true;
                                }
                            }

                            if (i < width - 1)
                            {
                                if (!BlankSpace[i + 1, j - 1] && Emptys[i + 1, j - 1] == null)
                                {
                                    return true;
                                }
                            }
                            
                        }
                    }
                }
            }
        }
        return false;
    }


    //tao piece moi sau khi da di chuyen xuong xong
    public IEnumerator CreateEmtpyAfterDecrease()
    {
        for (int i = width-1; i >=0; i--)
        {
            for (int j = height-1; j >=0; j--)
            {
                if (BrickSpace[i, j])
                    break;
                GetEmptyPool(i,j,false);
            }
        }
        create = false;
        yield return new WaitForSeconds(0.001f);
    }

    // di chuyen piece xuong va tao piece moi
    public void AfterDestroyCo()
    {
        StartCoroutine(AfterDestroy());
    }
    IEnumerator AfterDestroy()
    {
        timer = 0;
        if(!firstGame)
        {
            yield return StartCoroutine(DeCreaseRow1()); // di chuyen piece theo duong thang nhung se bij gioi han boi brick
        }
        else
        {
            yield return StartCoroutine(DeCreaseRowFull()); // di chuyen piece theo duong thang xuong lap day tat ca cac o
        }
        yield return CreateEmtpyAfterDecrease(); // tao piece moi
        if (CheckItemDuplicate()) // neu con cap 3 thi destroy tiep
        {
            yield return new WaitForSeconds(0.4f);
            DesAllItemDuplicate();
        }
        else 
        {
            if (CurrentState != State.wait)
            {
                if (CheckMove()) // neu o ben duoi trai hoac phai trong thi se di chuyen xuong
                {
                    DeCreaseRow();
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                    SetIsLocked();  // kiem tra xem man choi con nuoc di nao nua khong
                }
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                SetIsLocked();
            }
           
        }

    }

    // check xem co piece nao duplicate khong
    public bool CheckItemDuplicate()
    {

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(Emptys[i,j] != null)
                {
                    if (Emptys[i,j].duplicate)
                        return true;
                }
            }
        }

        return false;
    }

    // kiem tra xem o xung quanh co phai la brick hay khong
   public void CheckBrick(int column, int row)
    {
        if (column > 0)
        {
            if (BlankSpace[column - 1, row])
            {
                if (BrickSpace[column - 1, row] != null)
                {
                    BrickSpace[column - 1, row].GetComponent<brick>().TakeDame(1, column - 1, row);
                }
            }
        }
        if(column < width - 1)
        { 
            if (BlankSpace[column + 1, row])
            {
                if (BrickSpace[column + 1, row] != null)
                {
                    BrickSpace[column + 1, row].GetComponent<brick>().TakeDame(1, column + 1, row);
                }
            }
        }

        if (row > 0)
        {
            if (BlankSpace[column, row - 1])
            {
                if (BrickSpace[column, row - 1] != null)
                {
                    BrickSpace[column, row - 1].GetComponent<brick>().TakeDame(1, column, row - 1);
                }
            }
        }
        if(row < height - 1)
        { 
            if (BlankSpace[column, row + 1])
            {
                if (BrickSpace[column, row + 1] != null)
                {
                    BrickSpace[column, row + 1].GetComponent<brick>().TakeDame(1, column, row + 1);
                }
            }
        }
    }

    // hoan doi piece theo huong chi dinh
    void SwapPiece(int i , int j , Vector2 direction)
    {
        if (!BlankSpace[i + (int)direction.x, j + (int)direction.y]) 
        {
            if (Emptys[i + (int)direction.x, j + (int)direction.y] != null)
            {
                empty nextPiece = Emptys[i + (int)direction.x, j + (int)direction.y];
                Emptys[i + (int)direction.x, j + (int)direction.y] = Emptys[i, j];
                Emptys[i, j] = nextPiece;
            }
            
        }
    }

    // kiem tra xem co tao cap 3 nao tro len hay khong
    bool CheckDuplicatePiece()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i, j])
                {
                    if (Emptys[i,j]!= null)
                    {
                        if (i < width - 2)
                        {
                            if (Emptys[i + 1, j] != null && Emptys[i + 2, j] != null)
                            {
                                if (Emptys[i + 1, j].gameObject.tag == Emptys[i, j].gameObject.tag && Emptys[i + 2, j].gameObject.tag == Emptys[i, j].gameObject.tag)
                                    return true;
                            }
                        }
                        if (j < height - 2)
                        {
                            if (Emptys[i, j + 1] != null && Emptys[i, j + 2] != null)
                            {
                                if (Emptys[i, j + 1].gameObject.tag == Emptys[i, j].gameObject.tag && Emptys[i, j + 2].gameObject.tag == Emptys[i, j].gameObject.tag)
                                    return true;
                            }
                        }
                    }
                }
            }
        }
        

        return false;
    }

    // di chuyen piece thoe huong chi dinh va check xem co tao cap 3 tro len hay khong
    bool SwapAndCheck(int i, int j,Vector2 direction)
    {
        SwapPiece(i, j, direction);
        if(CheckDuplicatePiece())
        {
            SwapPiece(i, j, direction);
            return true;
        }
        SwapPiece(i, j, direction);
        return false;
    }

    // kiem tra xem man choi hien co nuoc di nao hop le khong
    bool isLocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i, j])
                {
                    if (Emptys[i,j]!= null)
                    {
                        if (i < width - 1)
                        {
                            if (SwapAndCheck(i, j, Vector2.right))
                                return false;
                        }
                        if (j < height - 1)
                        {
                            if (SwapAndCheck(i, j, Vector2.up))
                                return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    // kiem tra xem man choi hien co nuoc di nao hop le khong
    void SetIsLocked()
    {
        if (CurrentState != State.wait)
        {
            if (isLocked()) // neu khong con nuoc di nao hop le se hoan doi piece bao gio co nuoc di hop le thi thoi
            {
                StartCoroutine(ArrangePiece());
                GameManager.Instance.SwappingPanel.SetActive(true);
            }
            else
            {
                if(firstGame)
                {
                    firstGame = false;
                    loadingLevel.instance.SetPanel(false);
                    StartCoroutine(CreateBooster());  // neu nguoi choi co chon booster thi se kich hoat chung
                }
                
                StartCoroutine(FinishAllMission());  // kiem tra xem nguoi choi da hoan thanh het mission cua man game nay hay chua
            }
        }
        else
        {
            StartCoroutine(FinishAllMission());
        }
    }

    // sap xep lai piece bao gio tim ra nuoc di hop le thi thoi
    IEnumerator ArrangePiece()
    {
        List<empty> list = new List<empty>();
        timer = 0;
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i, j])
                {
                    if (Emptys[i, j] != null)
                    {
                        if (!list.Contains(Emptys[i, j]))
                        {
                            list.Add(Emptys[i, j]);
                        }
                    } 
                }
            }
        }

        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(!BlankSpace[i, j])
                {
                    if(Emptys[i, j] != null)
                    {
                        if (list.Count > 0)
                        {
                            int index = Random.Range(0, list.Count);
                            int interac = 0;
                            while (CheckDuplicateCo(i, j, list[index].tag) && interac < 100)
                            {
                                index = Random.Range(0, list.Count);
                                interac++;
                            }
                            interac = 0;
                            list[index].gameObject.GetComponent<empty>().column = i;
                            list[index].gameObject.GetComponent<empty>().row = j;
                            list.Remove(list[index]);
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (isLocked())
        {
            StartCoroutine(ArrangePiece());
        }
        else
        {
            GameManager.Instance.SwappingPanel.SetActive(false);
            DesAllItemDuplicate();
        }
    }


    //kiem tra xem co empty nao dang di chuyen hay khong
    public bool EmptyMove()
    {

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i,j] && Emptys[i,j] != null && Emptys[i,j].move)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //neu khong co boom nao hop le thi se check ra 2 piece de goi y
    void ListCheckHintAfter()
    {
        listHintIndex = new List<GameObject>();
        // boom color
        List<GameObject> pieceColor = new List<GameObject>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i, j] && Emptys[i, j] != null && Emptys[i, j].IsBoomColor)
                {
                    // right
                    if (i < width - 1 && j>0)
                    {
                        if( !BlankSpace[i + 1, j] && Emptys[i + 1, j] != null)
                        {
                            List<GameObject> Color = PieceDupliCateBoom(Emptys[i + 1, j].gameObject.tag);
                            if (Color.Count > pieceColor.Count)
                            {
                                pieceColor = Color;
                                listHintIndex.Clear();
                                listHintIndex.Add(Emptys[i, j].gameObject);
                                listHintIndex.Add(Emptys[i + 1, j].gameObject);
                            }
                        }
                        
                    }
                    //left
                    if (i > 0 && j>0)
                    {
                        if ( !BlankSpace[i - 1, j] && Emptys[i,j-1] != null)
                        {
                            List<GameObject> Color = PieceDupliCateBoom(Emptys[i,j-1].gameObject.tag);
                            if (Color.Count > pieceColor.Count)
                            {
                                pieceColor = Color;
                                listHintIndex.Clear();
                                listHintIndex.Add(Emptys[i, j].gameObject);
                                listHintIndex.Add(Emptys[i,j-1].gameObject);
                            }
                        }
                        
                    }
                    //up

                    if (j < height - 1)
                    {
                        if( !BlankSpace[i, j + 1] && Emptys[i, j + 1] != null)
                        {
                            List<GameObject> Color = PieceDupliCateBoom(Emptys[i, j + 1].gameObject.tag);
                            if (Color.Count > pieceColor.Count)
                            {
                                pieceColor = Color;
                                listHintIndex.Clear();
                                listHintIndex.Add(Emptys[i, j].gameObject);
                                listHintIndex.Add(Emptys[i, j + 1].gameObject);
                            }
                        }
                        
                    }
                    //down
                    if (j > 0)
                    {
                        if ( !BlankSpace[i, j - 1] && Emptys[i, j - 1] != null)
                        {
                            List<GameObject> Color = PieceDupliCateBoom(Emptys[i, j - 1].gameObject.tag);
                            if (Color.Count > pieceColor.Count)
                            {
                                pieceColor = Color;
                                listHintIndex.Clear();
                                listHintIndex.Add(Emptys[i, j].gameObject);
                                listHintIndex.Add(Emptys[i, j - 1].gameObject);
                            }
                        }

                    }

                    if (listHintIndex.Count > 1)
                    {
                        return;
                    }
                }
            }
        }
            // check item no boom
            for (int i = 0; i < width; i++)
        {
            for (int j = 0; j <height; j++)
            {
                if (!BlankSpace[i,j] && Emptys[i,j] != null)
                {
                    if (i < width - 1)
                    {
                        
                        SwapPiece(i, j, Vector2.right);
                        if (CheckDuplicatePiece())
                        {
                            
                            if (PieceDupliCate(i, j, Vector2.right).Count > listHint.Count)
                            {
                                listHintIndex.Clear();
                                listHintIndex.Add(Emptys[i, j].gameObject);
                                listHintIndex.Add(Emptys[i + 1, j].gameObject);
                                listHint =new List<GameObject>();
                                listHint = listHintAfter;
                            }

                        }
                        else
                            SwapPiece(i, j, Vector2.right);

                    }
                    if (j < height - 1)
                    {
                        
                        SwapPiece(i, j, Vector2.up);
                        if (CheckDuplicatePiece())
                        {
                            
                            if (PieceDupliCate(i, j, Vector2.up).Count > listHint.Count)
                            {
                                listHintIndex.Clear();
                                listHintIndex.Add(Emptys[i, j].gameObject);
                                listHintIndex.Add(Emptys[i, j + 1].gameObject);
                                listHint = new List<GameObject>();
                                listHint = listHintAfter;
                            }
                        }
                        else
                            SwapPiece(i, j, Vector2.up);
                    }
                }
            }
        }

    }

    // lay ra cac piece co mau ma boom color da swap
    List<GameObject> PieceDupliCateBoom(string tag)
    {
        List<GameObject> color = new List<GameObject>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i,j] && Emptys[i,j] != null && Emptys[i,j].tag == tag)
                {
                    if (!color.Contains(Emptys[i,j].gameObject))
                    {
                        color.Add(Emptys[i,j].gameObject);
                    }
                }
            }
        }
        return color;
    }

    // lay ra it nhat 3 piece tao cap 3
    List<GameObject> PieceDupliCate(int m , int n , Vector2 direction)
    {
        
        listHintAfter = new List<GameObject>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!BlankSpace[i, j])
                {
                    if(Emptys[i, j]!= null)
                    {
                        empty dot = Emptys[i, j];
                        if (i < width - 2)
                        {
                            if (Emptys[i + 1, j] != null && Emptys[i + 2, j] != null)
                            {
                                empty dot1 = Emptys[i + 1, j];
                                empty dot2 = Emptys[i + 2, j];
                                if (dot1.gameObject.tag == dot.gameObject.tag && dot2.gameObject.tag == dot.gameObject.tag)
                                {
                                    if (!listHintAfter.Contains(dot.gameObject))
                                        listHintAfter.Add(dot.gameObject);
                                    if (!listHintAfter.Contains(dot1.gameObject))
                                        listHintAfter.Add(dot1.gameObject);
                                    if (!listHintAfter.Contains(dot2.gameObject))
                                        listHintAfter.Add(dot2.gameObject);
                                    HintCheckAllBoomHoz(dot, dot1, dot2);
                                    HintCheckAllBoomVer(dot, dot1, dot2);

                                }
                            }
                        }
                        if (j < height - 2)
                        {
                            if (Emptys[i, j + 1] != null && Emptys[i, j + 2] != null)
                            {
                                empty dot1 = Emptys[i, j + 1];
                                empty dot2 = Emptys[i, j + 2];
                                if (dot1.gameObject.tag == dot.gameObject.tag && dot2.gameObject.tag == dot.gameObject.tag)
                                {


                                    if (!listHintAfter.Contains(dot.gameObject))
                                        listHintAfter.Add(dot.gameObject);
                                    if (!listHintAfter.Contains(dot1.gameObject))
                                        listHintAfter.Add(dot1.gameObject);
                                    if (!listHintAfter.Contains(dot2.gameObject))
                                        listHintAfter.Add(dot2.gameObject);
                                    HintCheckAllBoomHoz(dot, dot1, dot2);
                                    HintCheckAllBoomVer(dot, dot1, dot2);
                                }
                            }
                        }
                    }

                }
            }
        }
        SwapPiece(m, n, direction);
        return listHintAfter;
    }

    // check xem 3 piece da lay tren co piece nao la boom doc hay khong
    void HintCheckAllBoomVer(empty one , empty two , empty three)
    {
        if(one.IsBoomVer)
        {
            HintCheckBoomVer(one);
        }
        if (two.IsBoomVer)
        {
            HintCheckBoomVer(two);
        }
        if (three.IsBoomVer)
        {
            HintCheckBoomVer(three); 
        }
    }

    // check xem 3 piece da lay tren co piece nao la boom ngang hay khong
    void HintCheckAllBoomHoz(empty one, empty two, empty three)
    {
        if (one.IsBoomVer)
        {
            HintCheckBoomHoz(one);
        }
        if (two.IsBoomVer)
        {
            HintCheckBoomHoz(two);
        }
        if (three.IsBoomVer)
        {
            HintCheckBoomHoz(three);
        }
    }

    // 
    void HintCheckBoomVer(empty empty)
    {
        List<GameObject> boomVer = new List<GameObject>();
        for (int i = 0; i < height; i++)
        {
            if (!BlankSpace[empty.column,i] && Emptys[empty.column,i] != null)
            {

                if (Emptys[empty.column, i].IsBoomHoz)
                {
                    HintCheckBoomHoz(Emptys[empty.column, i]);
                }
                else
                {
                    if (!boomVer.Contains(Emptys[empty.column,i].gameObject))
                    {
                        boomVer.Add(Emptys[empty.column, i].gameObject);
                    }
                }
            }
        }
        List<GameObject> union = listHintAfter.Union(boomVer).ToList();
        listHintAfter.Clear();
        listHintAfter = union;
    }

    void HintCheckBoomHoz(empty empty)
    {
        List<GameObject> boomHoz = new List<GameObject>();
        for (int i = 0; i < width; i++)
        {
            if (!BlankSpace[i,empty.row] && Emptys[i, empty.row] != null)
            {

                if (Emptys[i, empty.row].IsBoomVer)
                {
                    HintCheckBoomVer(Emptys[i, empty.row]);
                }
                else
                {
                    if (!boomHoz.Contains(Emptys[i, empty.row].gameObject))
                    {
                        boomHoz.Add(Emptys[i, empty.row].gameObject);
                    }
                }
            }
        }

        List<GameObject> union = listHintAfter.Union(boomHoz).ToList();
        listHintAfter.Clear();
        listHintAfter = union;
    }

    // bat dau tao goi y
    void StartHint()
    {
        List<GameObject> hint = HintManager.instance.ReturnBoomHint();
        if(hint == null) // neu khong co piece nao la boom ma tao ra hieu ung dac biet thi se check cac piece binh thuong
        {
            ListCheckHintAfter();
        }
        else
        {
            listHintIndex = hint;
        }
        
        if (listHintIndex.Count >0)
        {
            for (int i = 0; i < listHintIndex.Count; i++)
            {
                if (listHintIndex[i] != null && Emptys[listHintIndex[i].GetComponent<empty>().column, listHintIndex[i].GetComponent<empty>().row] != null)
                {
              
                    if (Emptys[listHintIndex[i].GetComponent<empty>().column, listHintIndex[i].GetComponent<empty>().row].gameObject.transform.childCount > 0)
                    {
                        if (Emptys[listHintIndex[i].GetComponent<empty>().column, listHintIndex[i].GetComponent<empty>().row].gameObject.transform.GetChild(0).GetComponent<Animator>())
                        {
                            Emptys[listHintIndex[i].GetComponent<empty>().column, listHintIndex[i].GetComponent<empty>().row].gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("hint", true);
                        }
                    }
                }
            }
        }
        timer = 0;

    }

    // tat goi y
    public void offHint()
    {
        timer = 0;
        if (listHintIndex.Count > 0)
        {
            for (int i = 0; i < listHintIndex.Count; i++)
            {
                if (listHintIndex[i] != null && listHintIndex[i].transform.childCount>0)
                {
                    if(listHintIndex[i].transform.GetChild(0).GetComponent<Animator>())
                    {
                        listHintIndex[i].transform.GetChild(0).GetComponent<offAnim>().SetOffAnimHint();
                    }
                    
                }
            }
        }
        listHintIndex = new List<GameObject>();
    }
     // end game
    public IEnumerator FinishAllMission()
    {
        
        if (Mission.FinishMission() && Swipe >= 0 )//&& CheckJelly())
        {
            CurrentState = State.wait;
            yield return new WaitForSeconds(0.5f);
            List<empty> listBoom1 = new List<empty>();
            List<GameObject> listBoom2 = new List<GameObject>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!BlankSpace[i, j])
                    {
                        if (Emptys[i, j] != null)
                        {
                            if (Emptys[i, j].PieceNotBoom())
                            {
                                if (!listBoom1.Contains(Emptys[i, j]))
                                {
                                    listBoom1.Add(Emptys[i, j]);
                                }

                            }
                        }
                    }
                }
            }

            while (Swipe > 0)
            {
                int piece = Random.Range(0, listBoom1.Count);
                int boom = Random.Range(1, 3);
                if (Emptys[listBoom1[piece].column, listBoom1[piece].row].PieceNotBoom())
                {
                    if (boom == 1)
                    {
                        if (Emptys[listBoom1[piece].column, listBoom1[piece].row] != null && listBoom1[piece]!= null)
                        {
                            listBoom1[piece].GetComponent<empty>().CreateBoomHoz();
                            listBoom2.Add(listBoom1[piece].gameObject);
                        }
                    }
                    else
                    {
                        if (Emptys[listBoom1[piece].column, listBoom1[piece].row] != null && listBoom1[piece] != null)
                        {
                            listBoom1[piece].GetComponent<empty>().CreateBoomVer();
                            listBoom2.Add(listBoom1[piece].gameObject);
                        }
                    }
                }
                listBoom1.Remove(listBoom1[piece]);
                Swipe -= 2;
                if (Swipe <= 0)
                    Swipe = 0;
                GameManager.Instance.SwipeTxt.GetComponent<Text>().text = Swipe.ToString();

                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
            while (listBoom2.Count > 0)
            {
                int i = Random.Range(0, listBoom2.Count);
                if (listBoom2[i] != null)
                {
                    if (listBoom2[i].GetComponent<empty>().IsBoomHoz)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if (listBoom2[i] != null)
                            {
                                GameObject clear = GetObjectPool("clearItemHoz", j, listBoom2[i].GetComponent<empty>().row);
                                if (!BlankSpace[j, listBoom2[i].GetComponent<empty>().row])
                                {
                                    if (Emptys[j, listBoom2[i].GetComponent<empty>().row] != null)
                                    {
                                        Emptys[j, listBoom2[i].GetComponent<empty>().row].duplicate = true;
                                        
                                        yield return new WaitForSeconds(0.05f);
                                    }

                                }
                            }
                        }
                    }
                    else if (listBoom2[i].GetComponent<empty>().IsBoomVer)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            if (listBoom2[i] != null)
                            {
                                GameObject clear = GetObjectPool("clearItemVer", listBoom2[i].GetComponent<empty>().column, j);
                                if (!BlankSpace[listBoom2[i].GetComponent<empty>().column, j])
                                {
                                    if (Emptys[listBoom2[i].GetComponent<empty>().column, j] != null)
                                    {
                                        Emptys[listBoom2[i].GetComponent<empty>().column, j].duplicate = true;
                                        
                                        yield return new WaitForSeconds(0.05f);
                                    }

                                }
                            }
                        }
                    }
                }
                listBoom2.Remove(listBoom2[i]);

                for (int m = 0; m < width; m++)
                {
                    for (int n = 0; n < height; n++)
                    {
                        if (!BlankSpace[m, n])
                        {
                            if (Emptys[m, n] != null)
                            {
                                if (Emptys[m, n].duplicate)
                                {
                                    for (int k = 0; k < listBoom2.Count; k++)
                                    {
                                        if (listBoom2[k] != null)
                                        {
                                            if (listBoom2[k].GetComponent<empty>().column == m && listBoom2[k].GetComponent<empty>().row == n)
                                            {
                                                listBoom2.Remove(listBoom2[k]);
                                            }
                                        }
                                    }
                                    GameObject desItem = GetObjectPool("break" + Emptys[m,n].gameObject.tag, m,n);
                                    Score.MakeScore(50);
                                    MusicManager.instance.PlaySound(MusicManager.instance.Clear);
                                    Destroy(Emptys[m, n].gameObject);
                                    Emptys[m, n] = null;
                                }
                            }
                        }
                    }
                }

                yield return StartCoroutine(DeCreaseRow1());
                yield return CreateEmtpyAfterDecrease();
            }
            if (CheckItemDuplicate())
            {
                DesAllItemDuplicate();
            }
            else
            {
                yield return new WaitForSeconds(1f);
                MusicManager.instance.SoundGround.Stop();
                MusicManager.instance.PlaySound(MusicManager.instance.win);
                SaveManager.instance.SaveLevelStar(LevelManager.Instance.LevelIndex + 1, starLevel);
                GameManager.Instance.SetLevelWin(LevelManager.Instance.LevelIndex + 1, starLevel);
                randomReward.instance.Reward();
            }
            

        }
        else if ((Swipe <= 0 && !Mission.FinishMission()))
        {
            CurrentState = State.wait;
            yield return new WaitForSeconds(1f);
            MusicManager.instance.SoundGround.Stop();
            MusicManager.instance.PlaySound(MusicManager.instance.lose);
            GameManager.Instance.SetLevelLose();
            TimeManager.instance.Lose();
        }
        else
        {
            if(moveCircle)
            {
                StartCoroutine(MovePieceForCircle());
            }
            else
            {
                index = 0;
                CurrentState = State.stop;
                yield return new WaitForSeconds(0.5f);
                if(!firstGame)
                    StartHint();
            }
        }
    }

    // kiem tra xem man choi  nay con jelly hay khong
    public bool CheckJelly()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (AllJelly[i, j] != null)
                    return false;
            }
        }

        return true;
    }

    // di chuyen piece theo hinh tron sau khi swap
    IEnumerator MovePieceForCircle()
    {
        yield return new WaitForSeconds(1f);
        if(AllCorners.Count >=4  && move )
        {
            //up
            for (int i = AllCorners[0].column; i < AllCorners[1].column; i++)
            {
                if (Emptys[i, AllCorners[0].row] != null)
                {
                    for (int j = i + 1; j <= AllCorners[1].column; j++)
                    {
                        if (!BlankSpace[j, AllCorners[1].row] /*&& Emptys[j, AllCorners[1].row] != null*/)
                        {
                            /*Emptys[i, AllCorners[0].row].column = Emptys[j, AllCorners[1].row].column;*/
                            Emptys[i, AllCorners[0].row].column = j;
                            Emptys[i, AllCorners[0].row] = null;
                            break;
                        }
                    }
                }
            }
            //right
            for (int i = AllCorners[1].row; i > AllCorners[2].row; i--)
            {

                if (Emptys[AllCorners[1].column, i] != null)
                {
                    for (int j = i - 1; j >= AllCorners[2].row; j--)
                    {
                        if (!BlankSpace[AllCorners[1].column,j]/* && Emptys[AllCorners[1].column, j] != null*/)
                        {
                            /* Emptys[AllCorners[1].column, i].row = Emptys[AllCorners[1].column, j].row;*/
                            Emptys[AllCorners[1].column, i].row = j;
                            Emptys[AllCorners[1].column, i] = null;
                            break;
                        }
                    }
                }
            }
            //down
            for (int i = AllCorners[2].column; i > AllCorners[3].column; i--)
            {
                if (Emptys[i, AllCorners[2].row] != null)
                {
                    for (int j = i-1; j >= AllCorners[3].column; j--)
                    {
                        if (!BlankSpace[j, AllCorners[3].row] /*&& Emptys[j, AllCorners[3].row] != null*/)
                        {
                            /*Emptys[i, AllCorners[3].row].column = Emptys[j, AllCorners[3].row].column;*/
                            Emptys[i, AllCorners[2].row].column = j;
                            Emptys[i, AllCorners[2].row] = null;
                            break;
                        }
                    }
                }
            }
            //left
            for (int i = AllCorners[3].row; i < AllCorners[0].row; i++)
            {
                if (Emptys[AllCorners[3].column, i] != null)
                {
                    for (int j = i + 1; j <= AllCorners[0].row; j++)
                    {
                        if (!BlankSpace[AllCorners[3].column, j] /*&& Emptys[AllCorners[3].column, j] != null*/)
                        {
                            /*Emptys[AllCorners[3].column, i].row = Emptys[AllCorners[3].column, j].row;*/
                            Emptys[AllCorners[3].column, i].row = j;
                            Emptys[AllCorners[3].column, i] = null;
                            break;
                        }
                    }
                }
            }
            move = false;
            moveCircle = false;
            yield return new WaitForSeconds(0.3f);
            AfterDestroyCo();
        }
        else
        {
            move = false;
            moveCircle = false;
            yield return new WaitForSeconds(0.3f);
            CurrentState = State.stop;
            yield return new WaitForSeconds(0.5f);
            StartHint();
        }

    }
  
}
