using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonGameManager : MonoBehaviour
{
    public static ButtonGameManager instance;
    // luu cac loai button co trong scene game
    private Dictionary<TypeButtonGame,Action> ButtonGames = new Dictionary<TypeButtonGame,Action>();
    //
    public GameObject settingGamePanel;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    

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
        // load du lieu cua volume 
        LoadAndSetVolume();
        musicVolumeSlider.onValueChanged.AddListener(MusicManager.instance.SetValueMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(MusicManager.instance.SetValueSoundVolume);

        // dang ky cac su kien cho button trong scene game
        RegisterActionBtn(TypeButtonGame.ReturnGame, ReturnGame);
        RegisterActionBtn(TypeButtonGame.Setting,OpenSettingGame);
        RegisterActionBtn(TypeButtonGame.ReStart,returnLevel);
        RegisterActionBtn(TypeButtonGame.MainMenu, OpenMainMenu);
        RegisterActionBtn(TypeButtonGame.nextLevel, NextLevel);
    }


    public void RegisterActionBtn(TypeButtonGame type , Action action)
    {
        if(!ButtonGames.ContainsKey(type))
        {
            ButtonGames[type] = action;
        }
    }


    public void TriggerEvent(TypeButtonGame type)
    {
        if(ButtonGames.TryGetValue(type, out Action action))
        {
            action?.Invoke();
        }
    }


    // mo panel setting trong scene game
    public void OpenSettingGame()
    {
        board.instance.CurrentState = State.ui;
        settingGamePanel.SetActive(true);
        MusicManager.instance.PlaySound(MusicManager.instance.button);
    }

    // quay tro lai scene menu
    public void OpenMainMenu()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        loadingLevel.instance.LoadLevel("MainMenu",0);
        SaveManager.instance.SaveVolumeSetting(musicVolumeSlider.value, sfxVolumeSlider.value);
    }

    // quay tro lai game
    public void ReturnGame()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        board.instance.CurrentState = State.stop;
        settingGamePanel.SetActive(false);
        SaveManager.instance.SaveVolumeSetting(musicVolumeSlider.value,sfxVolumeSlider.value);
    }

    // load du lieu cua volume len
    public void LoadAndSetVolume()
    {
        VolumeSetting volumeSetting = SaveManager.instance.LoadVolumeSetting();
        musicVolumeSlider.value = volumeSetting.musicVolume;
        sfxVolumeSlider.value = volumeSetting.sfxVolume;
    }

    // chuyen level tiep theo
    public void NextLevel()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        LevelManager.Instance.LevelIndex += 2;
        LevelManager.Instance.ResetBooter();
        loadingLevel.instance.LoadLevel("Game", LevelManager.Instance.LevelIndex);
    }

    // choi lai level nay
    public void returnLevel()
    {
        MusicManager.instance.PlaySound(MusicManager.instance.press);
        LevelManager.Instance.LevelIndex += 0;
        LevelManager.Instance.ResetBooter();
        loadingLevel.instance.LoadLevel("Game",LevelManager.Instance.LevelIndex);
    }
}
