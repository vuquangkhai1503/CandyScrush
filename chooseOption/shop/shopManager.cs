using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// thong tin cua phan tu trong shop
[System.Serializable]
public struct HelpItem
{
    public GameObject HelpPrefab;
    public int index;
}

// thong tin cua 2 phan tu trong 1 itemSingle shop
[System.Serializable]
public struct HelpItemSingle
{
    public GameObject HelpPrefab_1;
    public int index_1;
    public int price_1;
    public GameObject HelpPrefab_2;
    public int index_2;
    public int price_2;
}

public class shopManager : MonoBehaviour
{
    public static shopManager instance;

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

    [Header("empty")]
    public GameObject emptyShop;
    public GameObject choosePanel;
    public GameObject content;
    public Button Yes;
    public Button No;
    [Header("bestValue")]
    public GameObject PriceBestValueBtn;
    public GameObject bestContent;
    public int priceBestValue;
    public List<HelpItem> allItemBestValue;
    [Header("weekValue")]
    public GameObject PriceWeekValueBtn;
    public int priceWeekValue;
    public GameObject weekContent;
    public List<HelpItem> allItemWeekValue;
    [Header("dailyValue")]
    public GameObject PriceDailyValueBtn;
    public int priceDailyValue;
    public GameObject dailyContent;
    public List<HelpItem> allItemDailyValue;
    [Header("singleValue")]
    public List<HelpItemSingle> allItemSingleValue;
    public GameObject singlePrefab;

    private void Start()
    {
        BestValue();
        WeekValue();
        DailyValue();
        SingleValue();
    }

    // thiet lap thong tin cua phan tu best value
    void BestValue()
    {
        if(allItemBestValue.Count >0)
        {
            for (int i = 0; i < allItemBestValue.Count; i++)
            {
                GameObject item = Instantiate(emptyShop,Vector3.zero,Quaternion.identity,bestContent.transform);
                item.transform.GetChild(0).GetComponent<Image>().sprite = allItemBestValue[i].HelpPrefab.transform.GetComponent<SpriteRenderer>().sprite;
                item.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = allItemBestValue[i].index.ToString();
            }

            PriceBestValueBtn.GetComponent<Button>().onClick.AddListener(() => ShowChoose(allItemBestValue, priceBestValue, PriceBestValueBtn));
            PriceBestValueBtn.transform.GetChild(0).GetComponent<Text>().text = priceBestValue.ToString();
        }
    }

    // thiet lap thong tin cua phan tu week value
    void WeekValue()
    {
        if (allItemWeekValue.Count > 0)
        {
            for (int i = 0; i < allItemWeekValue.Count; i++)
            {
                GameObject item = Instantiate(emptyShop, Vector3.zero, Quaternion.identity, weekContent.transform);
                item.transform.GetChild(0).GetComponent<Image>().sprite = allItemWeekValue[i].HelpPrefab.transform.GetComponent<SpriteRenderer>().sprite;
                item.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = allItemWeekValue[i].index.ToString();
            }

            PriceWeekValueBtn.GetComponent<Button>().onClick.AddListener(() => ShowChoose(allItemWeekValue, priceWeekValue,PriceWeekValueBtn));
            PriceWeekValueBtn.transform.GetChild(0).GetComponent<Text>().text = priceWeekValue.ToString();
        }
    }

    // thiet lap thong tin cua phan tu daily value
    void DailyValue()
    {
        if (allItemDailyValue.Count > 0)
        {
            for (int i = 0; i < allItemDailyValue.Count; i++)
            {
                GameObject item = Instantiate(emptyShop, Vector3.zero, Quaternion.identity, dailyContent.transform);
                item.transform.GetChild(0).GetComponent<Image>().sprite = allItemDailyValue[i].HelpPrefab.transform.GetComponent<SpriteRenderer>().sprite;
                item.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = allItemDailyValue[i].index.ToString();
            }

            PriceDailyValueBtn.GetComponent<Button>().onClick.AddListener(() => ShowChoose(allItemDailyValue, priceDailyValue, PriceDailyValueBtn));
            PriceDailyValueBtn.transform.GetChild(0).GetComponent<Text>().text = priceDailyValue.ToString();
        }
    }

    // thiet lap thong tin cua phan tu single value
    void SingleValue()
    {
        if(allItemSingleValue.Count > 0)
        {
            for (int i = 0; i < allItemSingleValue.Count; i++)
            {
                GameObject single = Instantiate(singlePrefab,content.transform);
                GameObject slot1 = Instantiate(emptyShop, single.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform);
                GameObject slot2 = Instantiate(emptyShop, single.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform);
                slot1.transform.GetChild(0).GetComponent<Image>().sprite = allItemSingleValue[i].HelpPrefab_1.transform.GetComponent<SpriteRenderer>().sprite;
                slot2.transform.GetChild(0).GetComponent<Image>().sprite = allItemSingleValue[i].HelpPrefab_2.transform.GetComponent<SpriteRenderer>().sprite;
                slot1.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = allItemSingleValue[i].index_1.ToString();
                slot2.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = allItemSingleValue[i].index_2.ToString();
                single.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = allItemSingleValue[i].price_1.ToString();
                single.transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = allItemSingleValue[i].price_2.ToString();

                string name1 = allItemSingleValue[i].HelpPrefab_1.name;
                int price1 = allItemSingleValue[i].price_1;
                int index1 = allItemSingleValue[i].index_1;
                GameObject btn1 = single.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;

                string name2 = allItemSingleValue[i].HelpPrefab_2.name;
                int price2 = allItemSingleValue[i].price_2;
                int index2 = allItemSingleValue[i].index_2;
                GameObject btn2 = single.transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).gameObject;

                single.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                ShowChooseSingle(name1,price1,index1,btn1));

                single.transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                ShowChooseSingle(name2, price2, index2, btn2));
            }
        }
    }

    // hien panel xem nguoi choi co quyet dinh mua item nay hay khong
    void ShowChoose(List<HelpItem> name, int price,GameObject btn)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.buyItem);
        choosePanel.SetActive(true);
        btn.GetComponent<Button>().enabled = false;
        Yes.onClick.RemoveAllListeners();
        No.onClick.RemoveAllListeners();
        Yes.onClick.AddListener(() => AddValuePurchase(name,price,btn));
        No.onClick.AddListener(()=>NoBtn(btn));
    }

    // hien panel xem nguoi choi co quyet dinh mua item nay hay khong cua phan tu don
    void ShowChooseSingle(string name, int price,int index, GameObject btn)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.buyItem);
        choosePanel.SetActive(true);
        btn.GetComponent<Button>().enabled = false;
        Yes.onClick.RemoveAllListeners();
        No.onClick.RemoveAllListeners();
        Yes.onClick.AddListener(() => AddValuePurchaseSingle(name, price,index, btn));
        No.onClick.AddListener(() => NoBtn(btn));
    }

    // luu lai so luong nguoi choi mua duoc
    void AddValuePurchaseSingle(string name, int price,int index,GameObject btn)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.buyItem);
        choosePanel.SetActive(false);
        btn.GetComponent<Button>().enabled = true;
        if (SaveManager.instance.LoadPlayerData("Money").money >= price)
        {
            SaveManager.instance.SaveMoney(-price);
            MenuManager.instance.SetMoney();
            SaveManager.instance.SaveHelp(name,index);
        }

    }

    // tat panel khi nguoi choi khong mua
    void NoBtn(GameObject btn)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.buyItem);
        btn.GetComponent<Button>().enabled = true;
        choosePanel.SetActive(false);
    }

    // load lai tien va thong tin khac khi nguoi choi quyet dinh mua item
    void AddValuePurchase(List<HelpItem> name, int price,GameObject btn)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.buyItem);
        choosePanel.SetActive(false);
        btn.GetComponent<Button>().enabled = true;
        if (SaveManager.instance.LoadPlayerData("Money").money >= price)
        {
            SaveManager.instance.SaveMoney(-price);
            MenuManager.instance.SetMoney();
            for (int i = 0; i < name.Count; i++)
            {
                SaveManager.instance.SaveHelp(name[i].HelpPrefab.name, name[i].index);
            }
        }
        
    }
}
