using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateBoomAuto: MonoBehaviour
{
    public static CreateBoomAuto instance;
    public List<List<empty>> AllList;
    public Dictionary<empty , List<empty>> PosBoom;
    public List<List<empty>> column;
    public List<List<empty>> row;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void SetColumnBoom()
    {
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                List<empty> listColumn = new List<empty>();
                if (!board.instance.BlankSpace[i,j] && board.instance.Emptys[i,j] != null)
                {
                    empty middle = board.instance.Emptys[i,j];
                    empty left = null;
                    empty right = null;
                    if(i >0 && !board.instance.BlankSpace[i-1,j] && board.instance.Emptys[i-1,j] != null)
                    {
                        left = board.instance.Emptys[i-1,j];
                    }
                    if (i < board.instance.width -1 && !board.instance.BlankSpace[i + 1, j] && board.instance.Emptys[i + 1, j] != null)
                    {
                        right = board.instance.Emptys[i + 1, j];
                    }
                    if (middle != null && left != null && right != null && left.gameObject.tag == middle.gameObject.tag && right.gameObject.tag == middle.gameObject.tag && middle.duplicate && left.duplicate && right.duplicate)
                    {
                        listColumn.Add(middle);
                        listColumn.Add(left);
                        listColumn.Add(right);
                        bool check = false;
                        for (int m = 0; m < column.Count; m++)
                        {
                            for (int n = 0; n < column[m].Count; n++)
                            {
                                if (column[m][n] == middle || column[m][n] == left || column[m][n] == right)
                                {
                                    check = true;
                                    if (!column[m].Contains(middle))
                                    {
                                        column[m].Add(middle);
                                    }
                                    if (!column[m].Contains(left))
                                    {
                                        column[m].Add(left);
                                    }
                                    if (!column[m].Contains(right))
                                    {
                                        column[m].Add(right);
                                    }
                                    break;
                                }
                            }
                        }
                        if(!check)
                        {
                            column.Add(listColumn);
                        }
                    }
                }
            }
        }
    }
    void SetRowBoom()
    {
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                List<empty> listRow = new List<empty>();
                if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null)
                {
                    empty middle = board.instance.Emptys[i, j];
                    empty up = null;
                    empty down = null;
                    if (j > 0 && !board.instance.BlankSpace[i,j-1] && board.instance.Emptys[i,j-1] != null)
                    {
                        down = board.instance.Emptys[i,j-1];
                    }
                    if (j < board.instance.height - 1 && !board.instance.BlankSpace[i , j+1] && board.instance.Emptys[i , j+1] != null)
                    {
                        up = board.instance.Emptys[i , j+1];
                    }
                    if (middle != null && up != null && down != null && up.gameObject.tag == middle.gameObject.tag && down.gameObject.tag == middle.gameObject.tag && middle.duplicate && up.duplicate && down.duplicate)
                    {
                        listRow.Add(middle);
                        listRow.Add(up);
                        listRow.Add(down);
                        bool check = false;
                        for (int m = 0; m < row.Count; m++)
                        {
                            for (int n = 0; n < row[m].Count; n++)
                            {
                                if (row[m][n] == middle || row[m][n] == up || row[m][n] == down)
                                {
                                    check = true;
                                    if (!row[m].Contains(middle))
                                    {
                                        row[m].Add(middle);
                                    }
                                    if (!row[m].Contains(up))
                                    {
                                        row[m].Add(up);
                                    }
                                    if (!row[m].Contains(down))
                                    {
                                        row[m].Add(down);
                                    }
                                    break;
                                }
                            }
                        }
                        if (!check)
                        {
                            row.Add(listRow);
                        }
                    }
                }
            }
        }
    }

    void SetAllList()
    {
        if(row.Count > column.Count)
        {
            List<List<empty>> three = new List<List<empty>>();
            three = column;
            column = row;
            row = three;
        }

        for (int i = 0; i < column.Count; i++)
        {
            bool check = false;
            for (int j = 0; j < row.Count; j++)
            {
                List<empty> fix = new List<empty>();
                if(column.Count>=i && row.Count >=j && column.Count>0 && row.Count>0)
                {
                    Debug.Log(i + " " + j);
                    bool tron = column[i].Intersect(row[j]).Any();
                   
                    if (tron == true)
                    {
                        check = true;
                        fix = column[i].Union(row[j]).ToList();
                        var pos = column[i].Intersect(row[j]).ToList();
                        column.Remove(column[i]);
                        row.Remove(row[j]);
                        AllList.Add(fix);
                    }
                }
               
            }
            if(!check && column.Count>0)
            {
                AllList.Add(column[i]);
            }
        }
        if(row.Count>0)
        {
            for(int i = 0;  i < row.Count;i++)
            {
                AllList.Add(row[i]);
            }
        }
    }

    public void GetAllBoom()
    {
        AllList = new List<List<empty>>();
        column = new List<List<empty>>();
        row = new List<List<empty>>();

        SetColumnBoom();
        SetRowBoom();
        SetAllList();
        for (int i = 0; i < AllList.Count; i++)
        {
            if(AllList[i].Count>= 4)
            {
                if (AllList[i].Count == 4)  // create boom hoz or boom ver
                {
                    if (AllList[i][0].column == AllList[i][1].column)
                    {
                        for (int k = 0; k < AllList[i].Count; k++)
                        {
                            if((board.instance.Emptys[AllList[i][k].column, AllList[i][k].row].PieceNotBoom()))
                            {
                                board.instance.Emptys[AllList[i][k].column, AllList[i][k].row].CreateBoomVer();
                                break;
                            }
                        }
                       
                    }
                    else
                    {
                        for (int k = 0; k < AllList[i].Count; k++)
                        {
                            if ((board.instance.Emptys[AllList[i][k].column, AllList[i][k].row].PieceNotBoom()))
                            {
                                board.instance.Emptys[AllList[i][k].column, AllList[i][k].row].CreateBoomHoz();
                                break;
                            }
                        }
                    }
                }
                else if(AllList[i].Count >= 5)
                {
                    if (CheckBoomColor(AllList[i]))
                    {
                        for (int k = 0; k < AllList[i].Count; k++)
                        {
                            if ((board.instance.Emptys[AllList[i][k].column, AllList[i][k].row].PieceNotBoom()))
                            {
                                board.instance.Emptys[AllList[i][k].column, AllList[i][k].row].CreateBoomColor();
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < AllList[i].Count; k++)
                        {
                            if ((board.instance.Emptys[AllList[i][k].column, AllList[i][k].row].PieceNotBoom()))
                            {
                                board.instance.Emptys[AllList[i][k].column, AllList[i][k].row].CreateBoomAdjast();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    bool CheckBoomColor(List<empty> list)
    {
        int column = 1;
        int row = 1;
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].column == list[0].column)
                column++;
            if(list[i].row == list[0].row)
                row++;
        }

        return (column== 5 || row== 5) ? true : false;    
    }
}