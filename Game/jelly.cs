using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class jelly : MonoBehaviour
{
    private int SumHeath;
    int currentHeath;
    bool move = false;

    private void Start()
    {
        this.gameObject.name = this.gameObject.name.Replace("(Clone)", " ").Trim();
        this.tag = "jelly";
        string name = this.gameObject.name.Replace("jelly", " ").Trim();
        string heath = name.Split(' ')[0];
        SumHeath = int.Parse(heath);
        currentHeath = SumHeath;
    }

    private void Update()
    {
        // neu heath cua jelly nay <= 0 thi se di chuyen len o chua mission tuong ung
        if (move)
        {
            MoveToMission();
        }
    }

    // gay dame vao jelly nay
    public void TakeDame(int dame, int column, int row)
    {
        currentHeath -= dame;
        SetImage(column, row);
        mission.instance.TakeALlItem(this.gameObject.tag);
        if (currentHeath <= 0)
        {
            //mission.instance.TakeALlItem(this.gameObject.tag);
            board.instance.AllJelly[column, row] = null;
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
            
            if (mission.instance.AllItemMission[i].Items.gameObject.tag == this.gameObject.tag)
            {
                Vector2 pos = new Vector2(mission.instance.CanvasMission.GetChild(i).transform.position.x, mission.instance.CanvasMission.GetChild(i).transform.position.y);
                if (Mathf.Abs(transform.position.x - mission.instance.CanvasMission.GetChild(i).transform.position.x) > 0.01f)
                {
                    transform.localScale = new Vector2(0.5f, 0.5f);
                    transform.position = Vector2.Lerp(transform.position, pos, 4f * Time.deltaTime);
                }
                else
                {
                    move = false;
                    GameObject color = board.instance.GetObjectPool("jelly",
                        Mathf.RoundToInt(mission.instance.CanvasMission.GetChild(i).transform.position.x), Mathf.RoundToInt(mission.instance.CanvasMission.GetChild(i).transform.position.y));
                    Destroy(gameObject);
                }
            }
            else
            {
                index++;
            }
        }

        if (index == mission.instance.AllItemMission.Count )
        {
            Destroy(gameObject);
        }

    }

    // thiet lap image tuong ung voi so heath
    void SetImage(int column, int row)
    {
        if (currentHeath >= 2)
        {
            GameObject brick = board.instance.GetObjectPool("jelly", column, row);
        }
        else if (currentHeath <= 1)
        {
            GameObject brick = board.instance.GetObjectPool("jelly", column, row);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = board.instance.jelly1.GetComponent<SpriteRenderer>().sprite;
            Color color = this.GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Clamp01(0.55f);
            this.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
