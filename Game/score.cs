using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;
    private float Score;
    private mission Mission;
    private float scoreIndex;
    public Slider ScoreSlider;


    private void Awake()
    {
        Mission = FindObjectOfType<mission>();   
    }

    public void MakeScore(int index)
    {
        SetSliderValue(index);
    }

    // thiet lap trang thai slider theo score
    void SetSliderValue(float index)
    {
        scoreIndex += index;
        ScoreSlider.value = scoreIndex / Score;
        if (scoreIndex >= Score * 0.5f && scoreIndex < Score * 0.75f)
        {
            Star1.transform.GetChild(0).gameObject.SetActive(true);
            board.instance.starLevel = 1;
        }
        if(scoreIndex >= Score *0.75f && scoreIndex < Score)
        {
            Star2.transform.GetChild(0).gameObject.SetActive(true);
            board.instance.starLevel = 2;
        }
        if(scoreIndex >= Score && Mission.FinishMission())
        {
            Star3.transform.GetChild(0).gameObject.SetActive(true);
            board.instance.starLevel = 3;
        }

    }

    void SetPosStar()
    {

    }


    // khi bat dau level se tat trang thai star
    public void SetBeginScore(int index)
    {
        Score = index;
        scoreIndex = 0;
        ScoreSlider.value = scoreIndex / Score;
        Star1.transform.GetChild(0).gameObject.SetActive(false);
        Star2.transform.GetChild(0).gameObject.SetActive(false);
        Star3.transform.GetChild(0).gameObject.SetActive(false);
    }
}
