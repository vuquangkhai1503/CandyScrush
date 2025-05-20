using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckAll : MonoBehaviour
{
    public static CheckAll instance;
    public List<empty> AllItemDestroy; // luu cac phan tu Empty[] duplicate
    public board board;
    public List<empty> circle; //luu cac phan tu da check qua roi
    private void Awake()
    {
        board = FindObjectOfType<board>();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    // tron danh sach cac empty hang ngang vao danh sach tong AllitemDestroy
    void IsBoomHoz(empty dot , empty dot2, empty dot1)
    {
        if (dot != null &&  dot.IsBoomHoz)
        {
            AllItemDestroy = AllItemDestroy.Union(GetColumnBoom(dot.row)).ToList();
        }
        if (dot1 != null && dot1.IsBoomHoz)
        {
            AllItemDestroy = AllItemDestroy.Union(GetColumnBoom(dot1.row)).ToList();
        }
        if (dot2 != null && dot2.IsBoomHoz)
        {
            AllItemDestroy = AllItemDestroy.Union(GetColumnBoom(dot2.row)).ToList();
        }
    }

    // tron danh sach cac empty hang doc vao danh sach tong AllitemDestroy
    void IsBoomVer(empty dot, empty dot2, empty dot1)
    {
        if (dot != null && dot.IsBoomVer)
        {
            AllItemDestroy = AllItemDestroy.Union(GetRowBoom(dot.column)).ToList();
        }
        if (dot1 != null && dot1.IsBoomVer)
        {
            AllItemDestroy = AllItemDestroy.Union(GetRowBoom(dot1.column)).ToList();
        }
        if (dot2 != null && dot2.IsBoomVer)
        {
            AllItemDestroy = AllItemDestroy.Union(GetRowBoom(dot2.column)).ToList();
        }
    }

    // tron danh sach cac empty pham vi o vao danh sach tong AllitemDestroy
    void IsBoomAdjasted(empty dot, empty dot2, empty dot1)
    {
        if (dot != null && dot.IsBoomAdjasted)
        {
            AllItemDestroy = AllItemDestroy.Union(GetadjastedBoom(dot.column,dot.row)).ToList();
        }
        if (dot1 != null && dot1.IsBoomAdjasted)
        {
            AllItemDestroy = AllItemDestroy.Union(GetadjastedBoom(dot1.column, dot1.row)).ToList();
        }
        if (dot2 != null && dot2.IsBoomAdjasted)
        {
            AllItemDestroy = AllItemDestroy.Union(GetadjastedBoom(dot2.column, dot2.row)).ToList();
        }
    }

    // tron danh sach cac empty color vao danh sach tong AllitemDestroy
    public void IsBoomColor(empty dot, empty dot2, empty dot1)
    {
        if (dot != null && dot.IsBoomColor)
        {
            AllItemDestroy.Union(GetColorBoom(dot.gameObject.tag));
        }
        if (dot1 != null && dot1 != null && dot1.IsBoomColor)
        {
            AllItemDestroy.Union(GetColorBoom(dot1.gameObject.tag));
        }
        if (dot2 != null && dot2 != null && dot2.IsBoomColor)
        {
            AllItemDestroy.Union(GetColorBoom(dot2.gameObject.tag));
        }
    }

    // lay ra danh sach cac phan tu pham vi o
    public List<empty> GetadjastedBoom(int column, int row)
    {
        List<empty>  adjasted = new List<empty>();
        GameObject boom = board.instance.GetObjectPool("breakAdjast",column,row);
        for (int i = column-1; i < column +2; i++)
        {
            for (int j = row-1; j < row +2; j++)
            {
                if (i >= 0 && i <= board.width - 1 && j >= 0 && j <= board.height - 1)
                {
                    if (!board.BlankSpace[i,j] && board.Emptys[i, j] != null )
                    {
                        if (!adjasted.Contains(board.Emptys[i, j]))
                        {
                            adjasted.Add(board.Emptys[i, j]);
                            board.Emptys[i, j].duplicate = true;
                            MusicManager.instance.PlaySound(MusicManager.instance.Clear);
                        }
                    }
                    if (board.BlankSpace[i,j])
                    {
                        if(board.BrickSpace[i,j] != null)
                        {
                            board.BrickSpace[i,j].GetComponent<brick>().TakeDame(1,i,j);
                        }
                    }
                }
            }
        }
        return adjasted ;
    }

    // lay ra danh sach cac phan tu cung mau voi boom color
    List<empty> GetColorBoom(string name)
    {
        List<empty> color = new List<empty>();
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (!board.BlankSpace[i,j] && board.Emptys[i,j]!= null)
                {
                    if (board.Emptys[i,j].tag == name)
                    {
                        color.Add(board.Emptys[i,j]);
                        board.Emptys[i,j].duplicate = true;
                        MusicManager.instance.PlaySound(MusicManager.instance.Clear);
                        GameObject clear = board.GetObjectPool("breakpiece", i, j);
                    }
                }
            }
        }

        return color ;
    }

    // lay ra danh sach cac phan tu hang ngang
    public List<empty> GetColumnBoom(int row)
    {
        List<empty> list = new List<empty>();
        for (int i = 0; i < board.width; i++)
        {
            GameObject clear = board.GetObjectPool("clearItemHoz", i, row);
            if (!board.BlankSpace[i,row] && board.Emptys[i,row] != null)
            {
                if (board.Emptys[i, row].IsBoomVer && !circle.Contains(board.Emptys[i,row])  && !AllItemDestroy.Contains(board.Emptys[i,row]))
                {
                    circle.Add(board.Emptys[i,row]);
                    list = list.Union(GetRowBoom(board.Emptys[i, row].column)).ToList();
                }
                if(!list.Contains(board.Emptys[i,row]))
                {
                    list.Add(board.Emptys[i, row]);
                    board.Emptys[i, row].duplicate = true;
                    MusicManager.instance.PlaySound(MusicManager.instance.Clear);
                }
                
            }
            if (board.BlankSpace[i, row])
            {
                if (board.BrickSpace[i, row] != null)
                {
                    board.BrickSpace[i, row].GetComponent<brick>().TakeDame(1, i, row);
                }
            }
        }


        return list;
    }

    // lay ra danh sach cac phan tu hang doc
    public List<empty> GetRowBoom(int column)
    {
        List<empty> list = new List<empty>();
        for (int i = 0; i < board.height; i++)
        {
            GameObject clear = board.GetObjectPool("clearItemVer", column, i);
            if (!board.BlankSpace[column,i] && board.Emptys[column,i] != null )
            {
                if (board.Emptys[column, i].IsBoomHoz && !circle.Contains(board.Emptys[column,i]) && !AllItemDestroy.Contains(board.Emptys[column,i]))
                {
                    circle.Add(board.Emptys[column,i]);
                    list = list.Union(GetRowBoom(board.Emptys[column, i].row)).ToList();
                }
                if(!list.Contains(board.Emptys[column, i]))
                {
                    list.Add(board.Emptys[column, i]);
                    board.Emptys[column, i].duplicate = true;
                   
                    MusicManager.instance.PlaySound(MusicManager.instance.Clear);
                }
                
            }
            if (board.BlankSpace[column, i])
            {
                if (board.BrickSpace[column, i] != null)
                {
                    board.BrickSpace[column, i].GetComponent<brick>().TakeDame(1, column, i);
                }
            }
        }
        return list;
    }

    // kiem tra cac phan tu xem co phan tu nao la boom khong thi se lay ra item trong pham vi boom do
    public void CheckAllDes()
    {
        for (int i = 0; i < board.height; i++)
        {
            for (int j = 0; j < board.width; j++)
            {
                if (!board.BlankSpace[i, j])
                {
                    if (board.Emptys[i, j] != null)
                    {
                        empty dot = board.Emptys[i, j];
                        if (dot.duplicate)
                        {
                            if (i > 0 && i < board.width - 1)
                            {
                                if (board.Emptys[i - 1, j] != null && board.Emptys[i + 1, j] != null)
                                {
                                    empty dot1 = board.Emptys[i - 1, j];
                                    empty dot2 = board.Emptys[i + 1, j];
                                    if (dot1.duplicate && dot2.duplicate)
                                    {
                                        IsBoomHoz(dot, dot1, dot2);
                                        IsBoomVer(dot, dot1, dot2);
                                        IsBoomAdjasted(dot, dot1, dot2);
                                        IsBoomColor(dot, dot1, dot2);
                                        GetNearByPiece(dot, dot1, dot2);
                                    }
                                }
                            }

                            if (j > 0 && j < board.height - 1)
                            {
                                if (board.Emptys[i, j - 1] != null && board.Emptys[i, j + 1] != null)
                                {
                                    empty dot1 = board.Emptys[i, j - 1];
                                    empty dot2 = board.Emptys[i, j + 1];
                                    if (dot1.duplicate && dot2.duplicate)
                                    {
                                        IsBoomHoz(dot, dot1, dot2);
                                        IsBoomVer(dot, dot1, dot2);
                                        IsBoomAdjasted(dot, dot1, dot2);
                                        IsBoomColor(dot, dot1, dot2);
                                        GetNearByPiece(dot, dot1, dot2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
    }

    // kiem tra phan tu don xem co phai la boom khong thi se lay ra item trong pham vi boom do
    public void CheckSingleBoom(empty empty)
    {
        IsBoomHoz(empty , null , null);
        IsBoomVer(empty , null , null);
        IsBoomAdjasted(empty , null , null);
        IsBoomColor(empty , null , null);
        if (!AllItemDestroy.Contains(empty))
        {
            AllItemDestroy.Add(empty);
        }
    }
    //them phan tu vao list tong
    void GetNearByPiece(empty dot, empty dot1, empty dot2)
    {
        AddToList(dot);
        AddToList(dot1);
        AddToList(dot2);
    }

    // them phan tu vao list tong
    void AddToList(empty dot)
    {
        if(!AllItemDestroy.Contains(dot))
        {
            AllItemDestroy.Add(dot);
        }
    }
}
