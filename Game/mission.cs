using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemMission
{
    public bool Finished ;
    public GameObject Items;
    public int Index;
}


public class mission : MonoBehaviour
{
    public static mission instance;
    public List<ItemMission> AllItemMission; // danh sach tat ca cac nhiem vu cac hoan thanh trong level
    private board Board;
    [HideInInspector] public List<GameObject> Items;
    public GameObject MissionPrefab;
    public Transform CanvasMission;

    private void Awake()
    {
        Board = FindObjectOfType<board>();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // load du lieu cac nhiem vu can hoan thanh trong level len panel mission
    public void SetItem(List<ItemMission> missions)
    {
        if(missions.Count > 0)
        {
            AllItemMission = missions.Select(m => new ItemMission
            {
                Finished = m.Finished,
                Items = m.Items,
                Index = m.Index
            }).ToList();
            for (int i = 0; i < AllItemMission.Count; i++)
            {
                GameObject item = Instantiate(MissionPrefab, CanvasMission);
                if (AllItemMission[i].Items.tag == "brick" || AllItemMission[i].Items.tag == "jelly")
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = AllItemMission[i].Items.GetComponent<SpriteRenderer>().sprite;
                    item.transform.GetChild(1).GetComponent<Text>().text = AllItemMission[i].Index.ToString();
                }
                else
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = AllItemMission[i].Items.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    item.transform.GetChild(1).GetComponent<Text>().text = AllItemMission[i].Index.ToString();
                }
                
                Items.Add(item);
                
            }
        }

    }

    // gay dame vao o chua phan tu da xoa
    public void TakeALlItem(string name)
    {
        if (AllItemMission.Count > 0)
        {
            for (int i = 0; i < AllItemMission.Count; i++)
            {
                if (AllItemMission[i].Items.gameObject.tag == name)
                {
                    if (AllItemMission[i].Index > 0)
                    {
                        AllItemMission[i].Index -= 1;
                        Items[i].transform.GetChild(1).GetComponent<Text>().text = AllItemMission[i].Index.ToString();
                        if (AllItemMission[i].Index <= 0)
                        {
                            Items[i].transform.GetChild(1).GetComponent<Text>().text = ("V").ToString();
                            AllItemMission[i].Finished = true;
                        }
                    }
                }
            }
        }
    }


    // kiem tra xem cac nhiem vu trong level nay da hoan thanh hay chua
    public bool FinishMission()
    {
        if (AllItemMission != null)
        {
            for (int i = 0; i < AllItemMission.Count; i++)
            {
                if (!AllItemMission[i].Finished)
                    return false;
            }
        }
        return true;
    }


    // lay ra cac item mission trong level chua hoan thanh
    public List<ItemMission> GetMissionLose()
    {
        List<ItemMission> list = new List<ItemMission>();
        if (AllItemMission != null)
        {
            for (int i = 0; i < AllItemMission.Count; i++)
            {
                if (!AllItemMission[i].Finished)
                {
                    list.Add(AllItemMission[i]);
                }
            }
        }
        return list;
    }
}
