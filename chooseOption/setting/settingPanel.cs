using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingPanel : MonoBehaviour
{
    [Header("button")]
    public Button HowtoPlaybtn;
    public Button MusicBtn;
    [Header("how to play")]
    public Button returnBtn;
    public Button nextBtn;
    public GameObject Steps;
    public GameObject HowtoPlayPanel;
    int index = 0;
    [Header("music")]
    public GameObject AudioPanel;
    public Slider music;
    public Slider sound;
    public Button back;

    private void Start()
    {
        HowtoPlaybtn.onClick.AddListener(() => setPanel(true));
        MusicBtn.onClick.AddListener(() => SetMusic(true));
        back.onClick.AddListener(() => SetMusic(false));
        music.onValueChanged.AddListener(MusicManager.instance.SetValueMusicVolume);
        sound.onValueChanged.AddListener(MusicManager.instance.SetValueSoundVolume);
    }


    // next den huong dan choi game tiep theo trong setting o scene menu
    void nextSteps()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        if (index < Steps.transform.childCount - 1)
            index++;
        for (int i = 0; i < Steps.transform.childCount; i++)
        {
            if (i == index)
            {
                Steps.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Steps.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (index == Steps.transform.childCount - 1)
        {
            nextBtn.transform.GetChild(0).GetComponent<Text>().text = "return";
            nextBtn.onClick.RemoveAllListeners();
            nextBtn.onClick.AddListener(() => setPanel(false));
        }
        else
        {
            nextBtn.transform.GetChild(0).GetComponent<Text>().text = "next";
        }
    }

    // quay tro lai huong dan choi game trong setting o scene menu
    void returnStep()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        if (index >0)
        {
            index--;
            for (int i = 0; i < Steps.transform.childCount; i++)
            {
                if (i != index)
                {
                    Steps.transform.GetChild(i).gameObject.SetActive(false);
                }
                else if (i == index)
                {
                    Steps.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            if (index == Steps.transform.childCount - 1)
            {
                nextBtn.transform.GetChild(0).GetComponent<Text>().text = "return";
                nextBtn.onClick.RemoveAllListeners();
                nextBtn.onClick.AddListener(() => setPanel(false));
            }
            else
            {
                nextBtn.onClick.RemoveAllListeners();
                nextBtn.onClick.AddListener(() => nextSteps());
                nextBtn.transform.GetChild(0).GetComponent<Text>().text = "next";
            }
        }
        else if(index == 0)
        {
            index = -1;
            HowtoPlayPanel.SetActive(false);
        }
            
    }

    //mo hoac tat panel setting
    void setPanel(bool open)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        index = -1;
        if (open == true)
        {
            nextSteps();
            nextBtn.onClick.RemoveAllListeners();
            nextBtn.onClick.AddListener(() => nextSteps());
            returnBtn.onClick.RemoveAllListeners();
            returnBtn.onClick.AddListener(() => returnStep());
        }
        HowtoPlayPanel.SetActive(open);

    }

    //  thiet lap panel volume
    void SetMusic(bool open)
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        AudioPanel.SetActive(open);
        Debug.Log(open + " music slider");
        if(open == true)
        {
            music.value = SaveManager.instance.LoadVolumeSetting().musicVolume;
            sound.value = SaveManager.instance.LoadVolumeSetting().sfxVolume;
        }
        else
        {
            SaveManager.instance.SaveVolumeSetting(music.value,sound.value);
        }
            
    }
}
