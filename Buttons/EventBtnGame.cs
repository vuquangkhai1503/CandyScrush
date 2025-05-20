using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// button trong scene game
public enum TypeButtonGame
{
    Setting,
    Hint,
    MainMenu,
    ReStart,
    ReturnGame,
    nextLevel,
}

public class EventBtnGame : MonoBehaviour
{
    public TypeButtonGame typeButtonGame;
    private void Start()
    {
        // them su kien vao cac button co trong scene game
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => ButtonGameManager.instance.TriggerEvent(typeButtonGame));
    }
}
