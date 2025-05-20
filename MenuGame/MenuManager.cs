using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [Header("UP Panel In Game")]
    public TMP_Text moneyTxt;
    public TMP_Text heathTxt;
    public TMP_Text timeHeathTxt;
    [Header("LevelUi")]
    [HideInInspector] public List<GameObject> Maps;
    public int SumMaps;
    public GameObject content;
    public GameObject MapPrefab;
    public List<Sprite> mapSprite;
    [Header("contetn Quest")]
    public Transform PanelQuest;

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

    private void Start()
    {
        CreateMaps();
        SetMoney();
        MusicManager.instance.PlayMusic(MusicManager.instance.BackGround);
    }

    // tao so luong map co trong game
    void CreateMaps()
    {
        for (int i = 0; i < SumMaps; i++)
        {
            GameObject map = Instantiate(MapPrefab,content.transform);
            map.name = map.name.Replace("(Clone)"," ").Trim();
            map.name = map.name + (i+1).ToString();
            int index = int.Parse(map.name.Replace("Map"," "));
            int sprite = Random.Range(0, mapSprite.Count);
            map.GetComponent<Image>().sprite = mapSprite[sprite];
            Maps.Add(map);
            SetLevels(index);
        }
    }

    // thiet lap tinh nang cho tung level
    void SetLevels(int index)
    {
        for (int j = 0; j < 10; j++)
        {
            int level = 10 * (index - 1) + j + 1;
            Maps[index - 1].transform.GetChild(j).GetComponent<Levell>().SetLevel(level);
        }
    }

    // load money len Ui
    public void SetMoney()
    {
        moneyTxt.text = SaveManager.instance.LoadPlayerData("Money").money.ToString();
    }

}
