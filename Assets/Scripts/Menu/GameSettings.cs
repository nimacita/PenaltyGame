using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }


    //сохраненное значение громкости музыки
    public float MusicVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("MusicVolume"))
            {
                PlayerPrefs.SetFloat("MusicVolume", 1f);
            }
            return PlayerPrefs.GetFloat("MusicVolume");
        }

        set
        {
            if (value > 1f) value = 1f;
            if (value < 0f) value = 0f;
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }

    //сохраненное значение громкости звуков
    public float SoundVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                PlayerPrefs.SetFloat("SoundVolume", 1f);
            }
            return PlayerPrefs.GetFloat("SoundVolume");
        }

        set
        {
            if (value > 1f) value = 1f;
            if (value < 0f) value = 0f;
            PlayerPrefs.SetFloat("SoundVolume", value);
        }
    }

    //сохраненное значение монет
    public int Coins
    {
        get
        {
            if (!PlayerPrefs.HasKey("Coins"))
            {
                PlayerPrefs.SetInt("Coins", 0);
            }
            return PlayerPrefs.GetInt("Coins");
        }

        set
        {
            PlayerPrefs.SetInt("Coins", value);
        }
    }

    //сохраненное значение выбранного скина
    public int HairSkinId
    {
        get
        {
            if (!PlayerPrefs.HasKey("CurrentHairSkin"))
            {
                PlayerPrefs.SetInt("CurrentHairSkin", 0);
            }
            return PlayerPrefs.GetInt("CurrentHairSkin");
        }

        set
        {
            PlayerPrefs.SetInt("CurrentHairSkin", value);
        }
    }

    public int GlassSkinId
    {
        get
        {
            if (!PlayerPrefs.HasKey("CurrentGlassSkin"))
            {
                PlayerPrefs.SetInt("CurrentGlassSkin", 0);
            }
            return PlayerPrefs.GetInt("CurrentGlassSkin");
        }

        set
        {
            PlayerPrefs.SetInt("CurrentGlassSkin", value);
        }
    }

    public int BackpackSkinId
    {
        get
        {
            if (!PlayerPrefs.HasKey("CurrentBackpackSkin"))
            {
                PlayerPrefs.SetInt("CurrentBackpackSkin", 0);
            }
            return PlayerPrefs.GetInt("CurrentBackpackSkin");
        }

        set
        {
            PlayerPrefs.SetInt("CurrentBackpackSkin", value);
        }
    }

    //сохраненное значение 
    public int CurrentGoals
    {
        get
        {
            if (!PlayerPrefs.HasKey("CurrentGoals"))
            {
                PlayerPrefs.SetInt("CurrentGoals", 0);
            }
            return PlayerPrefs.GetInt("CurrentGoals");
        }

        set
        {
            if (value < 0) value = 0;
            PlayerPrefs.SetInt("CurrentGoals", value);
        }
    }

    public bool IsAchiveClaimed(string name)
    {
        if (!PlayerPrefs.HasKey($"Avhive{name}"))
        {
            PlayerPrefs.SetInt($"Avhive{name}", 0);
        }

        if (PlayerPrefs.GetInt($"Avhive{name}") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAchiveClaimed(string name)
    {
        PlayerPrefs.SetInt($"Avhive{name}", 1);
    }
}
