using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brick : MonoBehaviour
{
    private int SumHeath;
    private mission Mission;
    private board board;
    int currentHeath;
    bool move = false;
    private void Awake()
    {
        board = FindAnyObjectByType<board>();
        Mission = FindObjectOfType<mission>();
    }
    private void Start()
    {
        this.gameObject.name = this.gameObject.name.Replace("(Clone)", " ").Trim();
        this.tag = "brick";
        string name = this.gameObject.name.Replace("brick", " ").Trim();
        string heath = name.Split(' ')[0];
        SumHeath = int.Parse(heath);
        currentHeath = SumHeath;
    }

    private void Update()
    {

        // neu heath birck nay <= 0 thi se di chuyen den mission chua item tuong ung
        if(move)
        {
            MoveToMission();
        }
    }

    //gay dame vao brick nay
    public void TakeDame(int dame,int column , int row)
    {
        currentHeath -= dame;
        SetImage(column,row);
        Mission.TakeALlItem(this.gameObject.tag);
        if (currentHeath <= 0)
        {
            GameObject brick = board.instance.GetObjectPool("brick1", column, row);
            board.BlankSpace[column, row] = false;
            board.Emptys[column, row] = null;
            board.BrickSpace[column, row] = null;
            this.transform.parent = null;
            this.GetComponent<SpriteRenderer>().sortingOrder = 10;
            move = true;
        }
    }

    // di chuyen den mission chua item tuong ung
    void MoveToMission()
    {
        int index = 0;
        for (int i = 0; i < mission.instance.AllItemMission.Count; i++)
        {
            
            if (mission.instance.AllItemMission[i].Items.gameObject.tag== this.gameObject.tag)
            {
                Vector2 pos = new Vector2(mission.instance.CanvasMission.GetChild(i).transform.position.x, mission.instance.CanvasMission.GetChild(i).transform.position.y);
                if(Mathf.Abs(transform.position.x - mission.instance.CanvasMission.GetChild(i).transform.position.x) > 0.01f)
                {
                    transform.localScale = new Vector2(0.3f,0.3f);
                    transform.position = Vector2.Lerp(transform.position, pos, 6f * Time.deltaTime);
                }
                else
                {
                    move = false;
                    GameObject color = board.instance.GetObjectPool("brick1",
                        Mathf.RoundToInt(mission.instance.CanvasMission.GetChild(i).transform.position.x),Mathf.RoundToInt(mission.instance.CanvasMission.GetChild(i).transform.position.y));
                    Destroy(gameObject);
                }
            }
            else
            {
                index++;
            }
        }
        if(index == mission.instance.AllItemMission.Count)
        {
            Destroy(gameObject);
        }
    }

    // thiet lap image tuong ung voi heath
    void  SetImage(int column , int row)
    {
        if(currentHeath >=3)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = board.instance.brick3.GetComponent<SpriteRenderer>().sprite;
        }
        else if(currentHeath ==2)
        {
            GameObject brick = board.instance.GetObjectPool("brick3", column, row);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = board.instance.brick2.GetComponent<SpriteRenderer>().sprite;
        }
        else if(currentHeath ==1)
        {
            GameObject brick = board.instance.GetObjectPool("brick2", column, row);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = board.instance.brick1.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
