using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public struct level
{
    public Sprite backGround;
    public GameObject groundTittle;
    public empty[,] Emptys;
    public bool[,] BlankSpace;
    public GameObject[,] BrickSpace;
    public List<GameObject> dott;
    public Piece[] AllTypePieces;
    public List<ItemMission> AllMissions;
    public List<corner> AllCorners;
    public int height;
    public int width;
    public int score;
    public int Swipe;
    public int jelly1;
    public int jelly2;
}


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    // danh sach toan bo level co trong game
    public List<level> Alllevels;

    [HideInInspector] public int LevelIndex;
    [HideInInspector] public bool booster_1;
    [HideInInspector] public bool booster_2;
    [HideInInspector] public bool booster_3;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // reset lai cac booster de dung khi bat dau level moi 
    public void ResetBooter()
    {
        booster_1 = false;
        booster_2 = false;
        booster_3 = false;
    }
}
