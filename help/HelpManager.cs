using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    public static HelpManager instance;
    public GameObject BlackPanel;
    GameObject btn;
    [Header("choose Panel")]
    public GameObject helpPanel;
    public GameObject avatarHelp;
    public TMP_Text infoHelp;
    public Button yesBtn;
    public Button noBtn;
    public int ExtraMoveIndex = 5;

    //UFO , FreeSwitch , HandSwap , ExtraMove  , LollipopHammer
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

    private void Start()
    {
        BlackPanel.SetActive(false);
    }

    // panel tro giup xem nguoi choi co quyet dinh dung hay khong
    public void SetChoosePanel(Sprite sprite , string info)
    {
        helpPanel.gameObject.SetActive(true);
        avatarHelp.GetComponent<Image>().sprite= sprite;
        infoHelp.text = info;
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();
    }

    // mo hoac tat anim 
    public void SetAnim(GameObject help)
    {
        if (btn != null)
        {
            btn.GetComponent<Animator>().SetBool("run", false);
            btn = help;
        }
        else
        {
            btn = help;
        }
    }
}
