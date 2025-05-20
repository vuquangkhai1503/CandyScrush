using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public GameObject PlayerBtn;
    public Slider ProgressSlider;
    private AsyncOperation Async;

    private void Start()
    {
        PlayerBtn.SetActive(false);
        StartCoroutine(Loading("MainMenu"));
    }

    // load scene theo thanh loading
    IEnumerator Loading(string name)
    {
        MusicManager.instance.PlayMusic(MusicManager.instance.Loading);
        Async = SceneManager.LoadSceneAsync(name);
        Async.allowSceneActivation = false;
        while(!Async.isDone)
        {
            float progress = Mathf.Clamp01(Async.progress/0.9f);
            ProgressSlider.value = progress;
            if (Async.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        ProgressSlider.gameObject.SetActive(false);
        PlayerBtn.SetActive(true);
        PlayerBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        PlayerBtn.GetComponent<Button>().onClick.AddListener(()=>PLAY_());
    }

    // load scene
    void LoadScene(string name)
    {
        Async = SceneManager.LoadSceneAsync(name);
        Async.allowSceneActivation = false;
        while(!Async.isDone)
        {
            if(Async.progress >=0.9f)
                break;
        }
    }

    // neu bam play thi se chuyen sang scene menu
    void PLAY_()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        Async.allowSceneActivation = true;
    }
}
