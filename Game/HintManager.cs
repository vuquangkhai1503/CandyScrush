using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // tra ra danh sach cac item de goi y
    public List<GameObject> ReturnBoomHint()
    {
        List<GameObject> list = new List<GameObject>();
        list = ReturnBoomColorAndColor();
        if(list!= null)
        {
            return list;
        }

        list = ReturnBoomColorAndAdjast();
        if (list != null)
        {
            return list;
        }
        list = ReturnBoomColorAndVerOrHoz();
        if (list != null)
        {
            return list;
        }
        list = ReturnBoomAdjastAndAdjast();
        if (list != null)
        {
            return list;
        }
        list = ReturnBoomAdjastAndVerOrHoz();
        if (list != null)
        {
            return list;
        }
        list = ReturnBoomHozAndVer();
        if (list != null)
        {
            return list;
        }

        return null;
    }

    // corlor and color
    List<GameObject> ReturnBoomColorAndColor()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                // boom color and color
                if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null && board.instance.Emptys[i, j].IsBoomColor)
                {
                    if (i < board.instance.width - 1)
                    {
                        if (!board.instance.BlankSpace[i + 1, j] && board.instance.Emptys[i + 1, j] != null && board.instance.Emptys[i + 1, j].IsBoomColor)
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i + 1, j].gameObject);
                            return list;
                        }
                    }
                    if (j < board.instance.height - 1)
                    {
                        if (!board.instance.BlankSpace[i, j + 1] && board.instance.Emptys[i, j + 1] != null && board.instance.Emptys[i, j + 1].IsBoomColor)
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j + 1].gameObject);
                            return list;
                        }
                    }
                }
            }
        }

        return null;
    }

    //color and adjast
    List<GameObject> ReturnBoomColorAndAdjast()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                // boom color and color
                if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null && board.instance.Emptys[i, j].IsBoomColor)
                {
                    if (i > 0)
                    {
                        if (!board.instance.BlankSpace[i - 1, j] && board.instance.Emptys[i - 1, j] != null && board.instance.Emptys[i - 1, j].IsBoomAdjasted)
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i + 1, j].gameObject);
                            return list;
                        }
                    }
                    if (j >0)
                    {
                        if (!board.instance.BlankSpace[i, j - 1] && board.instance.Emptys[i, j - 1] != null && board.instance.Emptys[i, j - 1].IsBoomAdjasted)
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j + 1].gameObject);
                            return list;
                        }
                    }
                    if (i < board.instance.width - 1)
                    {
                        if (!board.instance.BlankSpace[i + 1, j] && board.instance.Emptys[i + 1, j] != null && board.instance.Emptys[i + 1, j].IsBoomAdjasted)
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i + 1, j].gameObject);
                            return list;
                        }
                    }
                    if (j < board.instance.height - 1)
                    {
                        if (!board.instance.BlankSpace[i, j + 1] && board.instance.Emptys[i, j + 1] != null && board.instance.Emptys[i, j + 1].IsBoomAdjasted)
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j + 1].gameObject);
                            return list;
                        }
                    }
                }
            }
        }

        return null;
    }

    // color and ver or hoz
    List<GameObject> ReturnBoomColorAndVerOrHoz()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                // boom color and color
                if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null && board.instance.Emptys[i, j].IsBoomColor)
                {
                    if (i > 0)
                    {
                        if (!board.instance.BlankSpace[i - 1, j] && board.instance.Emptys[i - 1, j] != null && (board.instance.Emptys[i - 1, j].IsBoomVer || board.instance.Emptys[i - 1, j].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i -1, j].gameObject);
                            return list;
                        }
                    }
                    if (j > 0)
                    {
                        if (!board.instance.BlankSpace[i, j - 1] && board.instance.Emptys[i, j - 1] != null && (board.instance.Emptys[i , j-1].IsBoomVer || board.instance.Emptys[i , j-1].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j - 1].gameObject);
                            return list;
                        }
                    }
                    if (i < board.instance.width - 1)
                    {
                        if (!board.instance.BlankSpace[i + 1, j] && board.instance.Emptys[i + 1, j] != null && (board.instance.Emptys[i+1, j].IsBoomVer || board.instance.Emptys[i+1, j].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i + 1, j].gameObject);
                            return list;
                        }
                    }
                    if (j < board.instance.height - 1)
                    {
                        if (!board.instance.BlankSpace[i, j + 1] && board.instance.Emptys[i, j + 1] != null && (board.instance.Emptys[i, j + 1].IsBoomVer || board.instance.Emptys[i, j + 1].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j + 1].gameObject);
                            return list;
                        }
                    }
                }
            }
        }

        return null;
    }
    //adjast and adjast
    List<GameObject> ReturnBoomAdjastAndAdjast()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                // boom color and color
                if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null && board.instance.Emptys[i, j].IsBoomAdjasted)
                {
                    if (i < board.instance.width - 1)
                    {
                        if (!board.instance.BlankSpace[i + 1, j] && board.instance.Emptys[i + 1, j] != null && board.instance.Emptys[i + 1, j].IsBoomAdjasted)
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i + 1, j].gameObject);
                            return list;
                        }
                    }
                    if (j < board.instance.height - 1)
                    {
                        if (!board.instance.BlankSpace[i, j + 1] && board.instance.Emptys[i, j + 1] != null && board.instance.Emptys[i, j + 1].IsBoomAdjasted)
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j + 1].gameObject);
                            return list;
                        }
                    }
                }
            }
        }

        return null;
    }
    //adjast and hoz or ver
    List<GameObject> ReturnBoomAdjastAndVerOrHoz()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                // boom color and color
                if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null && board.instance.Emptys[i, j].IsBoomAdjasted)
                {
                    if (i > 0)
                    {
                        if (!board.instance.BlankSpace[i - 1, j] && board.instance.Emptys[i - 1, j] != null && (board.instance.Emptys[i - 1, j].IsBoomVer || board.instance.Emptys[i - 1, j].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i - 1, j].gameObject);
                            return list;
                        }
                    }
                    if (j > 0)
                    {
                        if (!board.instance.BlankSpace[i, j - 1] && board.instance.Emptys[i, j - 1] != null && (board.instance.Emptys[i, j - 1].IsBoomVer || board.instance.Emptys[i, j - 1].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j - 1].gameObject);
                            return list;
                        }
                    }
                    if (i < board.instance.width - 1)
                    {
                        if (!board.instance.BlankSpace[i + 1, j] && board.instance.Emptys[i + 1, j] != null && (board.instance.Emptys[i + 1, j].IsBoomVer || board.instance.Emptys[i + 1, j].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i + 1, j].gameObject);
                            return list;
                        }
                    }
                    if (j < board.instance.height - 1)
                    {
                        if (!board.instance.BlankSpace[i, j + 1] && board.instance.Emptys[i, j + 1] != null && (board.instance.Emptys[i, j + 1].IsBoomVer || board.instance.Emptys[i, j + 1].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j + 1].gameObject);
                            return list;
                        }
                    }
                }
            }
        }

        return null;
    }
    //hoz and ver
    List<GameObject> ReturnBoomHozAndVer()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < board.instance.width; i++)
        {
            for (int j = 0; j < board.instance.height; j++)
            {
                // boom color and color
                if (!board.instance.BlankSpace[i, j] && board.instance.Emptys[i, j] != null && (board.instance.Emptys[i , j].IsBoomVer || board.instance.Emptys[i , j].IsBoomHoz))
                {
                    if (i > 0)
                    {
                        if (!board.instance.BlankSpace[i - 1, j] && board.instance.Emptys[i - 1, j] != null && (board.instance.Emptys[i - 1, j].IsBoomVer || board.instance.Emptys[i - 1, j].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i -1, j].gameObject);
                            return list;
                        }
                    }
                    if (j > 0)
                    {
                        if (!board.instance.BlankSpace[i, j - 1] && board.instance.Emptys[i, j - 1] != null && (board.instance.Emptys[i, j - 1].IsBoomVer || board.instance.Emptys[i, j - 1].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j - 1].gameObject);
                            return list;
                        }
                    }
                    if (i < board.instance.width - 1)
                    {
                        if (!board.instance.BlankSpace[i + 1, j] && board.instance.Emptys[i + 1, j] != null && (board.instance.Emptys[i + 1, j].IsBoomVer || board.instance.Emptys[i + 1, j].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i + 1, j].gameObject);
                            return list;
                        }
                    }
                    if (j < board.instance.height - 1)
                    {
                        if (!board.instance.BlankSpace[i, j + 1] && board.instance.Emptys[i, j + 1] != null && (board.instance.Emptys[i, j + 1].IsBoomVer || board.instance.Emptys[i, j + 1].IsBoomHoz))
                        {
                            list.Add(board.instance.Emptys[i, j].gameObject);
                            list.Add(board.instance.Emptys[i, j + 1].gameObject);
                            return list;
                        }
                    }
                }
            }
        }

        return null;
    }

}
