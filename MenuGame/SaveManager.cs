
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// save volume
[System.Serializable]
public class VolumeSetting
{
    public float musicVolume;
    public float sfxVolume;
}

// save level
[System.Serializable]
public class LevelStar
{
    public int level;
    public int star ;
}

//save all level
[System.Serializable]
public class Level
{
    public int sumLevel;
    public List<LevelStar> levels;
}

//save money
[System.Serializable]
public class PlayerData
{
    public int money;
    public int wheelIndex;
    public int adsIndex;
}

//save help
[System.Serializable]
public class help
{
    public string helpName;
    public int index;
}

[System.Serializable]
public class AllHelp
{
    public List<help> allHelps;
}

// lan dau vao game
[System.Serializable]
public class FirstGame
{
    public bool firstGame;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // neu lan dau vao game thi se thiet lap lai du lieu game
        if(FirstInGame())
        {
            PlayerPrefs.DeleteAll();
            SaveLevelStar(0, 0);
            SaveVolumeSetting(0, 0);
            SaveMoney(10000);
            SaveWheel(5);
            ResetHelp();
            SaveHelp("boost_1",5);
            SaveHelp("boost_2", 5);
            SaveHelp("boost_3", 5);
            SaveFirstGame();
            MenuManager.instance.SetMoney();
        }
    }

    //save volume
    public void SaveVolumeSetting(float muisc , float sfx)
    {
        VolumeSetting volumeSetting = new VolumeSetting();
        {
            volumeSetting.musicVolume = muisc;
            volumeSetting.sfxVolume = sfx;
        }

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSetting));
        PlayerPrefs.Save();
    }

    public VolumeSetting LoadVolumeSetting()
    {
        return JsonUtility.FromJson<VolumeSetting>(PlayerPrefs.GetString("Volume"));
    }


    // luu lai so star va level da mo khoa
    public void SaveLevelStar(int level , int star )
    {
        var levelMap = LoadLevelMap();
        if( levelMap == null )
        {
            levelMap = new Level()
            {
                sumLevel = 1,
                levels = new List<LevelStar>()
            };
            var levelStars = new LevelStar()
            {
                level = level,
                star = star
            };
            levelMap.levels.Add(levelStars);
        }
        var levelStar = levelMap.levels.Find(l => l.level == level);
        if (levelStar == null)
        {
            levelStar = new LevelStar()
            {
                level = level,
                star = star
            };
            levelMap.levels.Add(levelStar);
        }
        else
        {
            levelStar.level = level;
            if( star > levelStar.star )
                levelStar.star = star;
        }

        if ( levelMap.sumLevel <= level +1)
        {
            levelMap.sumLevel = level + 1;
        }
        PlayerPrefs.SetString("Maps",JsonUtility.ToJson(levelMap));
        PlayerPrefs.Save();
    }
    

    // lay ra du lieu cua map
    public Level LoadLevelMap()
    {
        return JsonUtility.FromJson<Level>(PlayerPrefs.GetString("Maps"));
    }

    // lay ra so sao cua level
    public int LoadStarLevel(int index)
    {
        Level data = LoadLevelMap();
        if (data != null)
        {
            for (int i = 0; i < data.levels.Count; i++)
            {
                if (data.levels[i].level == index)
                    return data.levels[i].star;
            }
        }

        return 0;
    }

    // lay ra so level cao nhat da mo khoa duoc
    public int LoadisLockedLevel()
    {
        Level data = LoadLevelMap();
        if (data != null)
        {
            return data.sumLevel >= 1 ? data.sumLevel : 1;
        }
        return 1;
    }
    // luu lai du lieu PlayerData
    //luu lai so money cua nguoi choi
    public void SaveMoney(int index)
    {
        PlayerData Money = LoadPlayerData("Money");
        if(Money == null)
            Money = new PlayerData();
        Money.money += index;

        PlayerPrefs.SetString("Money",JsonUtility.ToJson(Money));
        PlayerPrefs.Save();
    }

    //luu lai so luot quay Wheel cua nguoi choi
    public void SaveWheel(int index)
    {
        PlayerData wheel = LoadPlayerData("Wheel");
        if (wheel == null)
            wheel = new PlayerData();
        wheel.wheelIndex += index;

        PlayerPrefs.SetString("Wheel", JsonUtility.ToJson(wheel));
        PlayerPrefs.Save();
    }
    // luu lai so luong Ads da xem
    public void SaveAds(int index)
    {
        PlayerData ads = LoadPlayerData("Ads");
        if (ads == null)
            ads = new PlayerData();
        ads.adsIndex += index;

        PlayerPrefs.SetString("Ads", JsonUtility.ToJson(ads));
        PlayerPrefs.Save();
    }
    //lay du lieu Playerdate
    public PlayerData LoadPlayerData(string name)
    {
        return JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(name));
    }

    //luu lai so luong cua tro giup va ten tro giup

    public void SaveHelp(string name , int index)
    {
        var AllHelp = LoadHelp();
        if (AllHelp == null)
        {
            AllHelp = new AllHelp()
            {
                allHelps = new List<help>()
            };

            var helpp = new help()
            {
                helpName = name,
                index = 0
            };
            AllHelp.allHelps.Add(helpp);
        }
            
        var help = AllHelp.allHelps.Find(i=>i.helpName == name);
        if(help != null)
        {
            help.index += index;
        }else
        {
            help = new help()
            {
                helpName = name,
                index = index
            };
            AllHelp.allHelps.Add(help);
        }
        
        if(help.index <=0)
            help.index = 0;
        PlayerPrefs.SetString("help", JsonUtility.ToJson(AllHelp));
        Debug.Log(JsonUtility.ToJson(AllHelp));
        PlayerPrefs.Save();

    }

    public AllHelp LoadHelp()
    {
        return JsonUtility.FromJson<AllHelp>(PlayerPrefs.GetString("help"));
    }

    public int LoadChooseHelp(string name)
    {
        var help = LoadHelp().allHelps.Find(i=>i.helpName == name);
        return help != null ? help.index : 0;
    }

    // luu lai xem co phai nguoi choi lan dau vao game khong
    public void SaveFirstGame()
    {
        FirstGame firstgame = LoadFirstGame();
        if(firstgame == null)
        {
            firstgame = new FirstGame();
            firstgame.firstGame = false;
        }

        PlayerPrefs.SetString("firstGame",JsonUtility.ToJson(firstgame));
        PlayerPrefs.Save();
    }

    public FirstGame LoadFirstGame()
    {
        return JsonUtility.FromJson<FirstGame>(PlayerPrefs.GetString("firstGame"));
    }
    // lay ra co dung nguoi choi vao game lan dau hay khong
    bool FirstInGame()
    {
        return LoadFirstGame() != null ? LoadFirstGame().firstGame : true; 
    }

    // Reset tat ca du lieu cua game
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        SaveLevelStar(0, 0);
        SaveVolumeSetting(0, 0);
        SaveMoney(10000);
        ResetHelp();
        SceneManager.LoadScene("StartGame");
    }

    //luu lai so luong tro giup sau khi reset
    void ResetHelp()
    {
        SaveHelp("UFO", 10);
        SaveHelp("LollipopHammer", 10);
        SaveHelp("ExtraMove", 10);
        SaveHelp("FreeSwitch", 10);
        SaveHelp("HandSwap", 10);
    }
}
