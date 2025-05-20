using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingLevel : MonoBehaviour
{
    public static loadingLevel instance;
    public List<Sprite> backGround;
    [TextArea]
    public List<string> Suggest;
    [Header("canvas")]
    public Transform bg;
    public Text suggestTxt;
    public GameObject Canvas;
    private AsyncOperation Async;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetPanel(bool open)
    {
        Canvas.SetActive(open);
        if (open)
        {
            int randomBg = Random.Range(0, backGround.Count);
            int randomTxt = Random.Range(0, Suggest.Count);
            bg.GetComponent<Image>().sprite = backGround[randomBg];
            suggestTxt.text = Suggest[randomTxt];
        }
    }

    public void LoadLevel(string scene,int index)
    {
        if(scene == "Game")
        {
            SetPanel(true);
            MusicManager.instance.PlaySound(MusicManager.instance.press);
            LevelManager.Instance.LevelIndex = index - 1;
            SceneManager.LoadScene("Game");
        }
        else if( scene =="MainMenu")
        {
            StartCoroutine(LoadSceneAsync(scene));
        }
        
    }

    IEnumerator LoadSceneAsync(string name)
    {
        Async = SceneManager.LoadSceneAsync(name);
        Async.allowSceneActivation = false;
        SetPanel(true);
        while(!Async.isDone)
        {
            if (Async.progress >= 0.9f)
            {
                break;
            }
                
            yield return null;
        }
        Async.allowSceneActivation = true;
        yield return new WaitForSeconds(0.4f);
        SetPanel(false);
    }
}
