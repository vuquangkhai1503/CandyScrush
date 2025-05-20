using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class itemWheel
{
    public GameObject item;
    public int index;
}

public class wheel : MonoBehaviour
{
    public RectTransform wheelPanel;
    public GameObject spinPrefab;
    public Text WheelTxt;
    public Button spinBtn;
    public Button closeBtn;
    public Button nextSpin;
    public float radius;
    public float circle;
    public float speed;
    public float time;
    private bool spin;
    public List<itemWheel> wheelItems;
    private List<GameObject> spinObjPool;
    public Dictionary<int , float> angleItems; // toa do goc cua cac item ban dau 
    [Header("UI reward item")]
    public GameObject rewardPanel;
    public Image itemRewardImage;
    public Text indexRewardTxt;
    public Button NextRewardBtn;
    private void Start()
    {
        SetItemSpin();
        NextRewardBtn.onClick.RemoveAllListeners();
        NextRewardBtn.onClick.AddListener(() => rewardPanel.SetActive(false));
    }

    // thiet lap trang thai ban dau cua cac phan tu trong Wheel
    void SetItemSpin()
    {
        int wheelIndex = SaveManager.instance.LoadPlayerData("Wheel") != null? SaveManager.instance.LoadPlayerData("Wheel").wheelIndex:0;
        WheelTxt.text = "Spin x" + wheelIndex.ToString();
        if (wheelPanel.transform.childCount <= 0)
        {
            angleItems = new Dictionary<int, float> ();
            for (int i = 0; i < wheelItems.Count; i++)
            {
                float angle = (360 / wheelItems.Count) * i;
                GameObject obj = Instantiate(spinPrefab, wheelPanel.transform);
                obj.transform.GetChild(0).GetComponent<Image>().sprite = wheelItems[i].item.GetComponent<SpriteRenderer>().sprite;
                obj.transform.GetChild(1).GetComponent<Text>().text = wheelItems[i].index.ToString() + "x";
                RectTransform rect = obj.GetComponent<RectTransform>();
                obj.transform.localRotation = Quaternion.Euler(0,0,angle);
                rect.anchoredPosition = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
                if(!angleItems.ContainsKey(i))
                {
                    angleItems[i] = angle;
                }
                
            }

            closeBtn.onClick.AddListener(()=>SetWheelPanel(false));
            nextSpin.onClick.AddListener(() => SetWheelPanel(false));
            spinBtn.onClick.AddListener(() => StartCoroutine(Spin()));

        }
    }


    // load so luong wheel len Text
    public void SetWheelPanel(bool open)
    {
        if(!spin)
        {
            this.gameObject.SetActive(open);
            if (open == true)
            {
                int wheelIndex = SaveManager.instance.LoadPlayerData("Wheel") != null ? SaveManager.instance.LoadPlayerData("Wheel").wheelIndex : 0;
                WheelTxt.text = "Spin x" + wheelIndex.ToString();
            }
        }
    }

    // spin va quay ra 1 item
    IEnumerator Spin()
    {
        int wheelIndex = SaveManager.instance.LoadPlayerData("Wheel") != null ? SaveManager.instance.LoadPlayerData("Wheel").wheelIndex : 0;
        if (wheelIndex > 0 && !spin)
        {
            spin = true;
            SaveManager.instance.SaveWheel(-1);
            WheelTxt.text ="Spin x" + SaveManager.instance.LoadPlayerData("Wheel").wheelIndex.ToString();
            float angle = Random.Range(circle * 5, circle * 8);
            float speed_ = speed;
            float timer = 0;
            while (timer < time)
            {
                speed_ = Mathf.SmoothStep(speed, 0, timer / time);
                float delta = speed_ * Time.deltaTime;
                wheelPanel.transform.Rotate(0, 0, -delta);
                timer += Time.deltaTime;
                yield return null;
            }

            GetItem(wheelPanel.transform.eulerAngles.z);
        }  
    }

    // luu lai item da quay ra duoc
    void GetItem(float currentAngle)
    {
        float max = float.MaxValue;
        int index = -1;
        for (int i = 0; i < angleItems.Count; i++)
        {
            float angle = (angleItems[i] + currentAngle) % 360;
            float minAgnle = Mathf.Abs(Mathf.DeltaAngle(angle,90));
            if(minAgnle < max)
            {
                max = minAgnle;
                index = i;
            }
        }
        spin = false;
        StartCoroutine(LoadItemUI(index));
    }

    // ui nhan item 
    IEnumerator LoadItemUI(int index)
    {
        yield return new WaitForSeconds(0.3f);
        rewardPanel.SetActive(true);
        itemRewardImage.GetComponent<Image>().sprite = wheelItems[index].item.GetComponent<SpriteRenderer>().sprite;
        indexRewardTxt.text = "x"+wheelItems[index].index.ToString();
        yield return new WaitForSeconds(4f);
        rewardPanel.SetActive(false);
    }
}
