using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public bool canResetData;

    private void Start()
    {
        StartData();

        if (canResetData)
        {
            CreateData();
        }
    }

    void StartData()
    {
        if (!PlayerPrefs.HasKey("LevelsUnlocked"))
        {
            CreateData();
        }
    }

    void CreateData()
    {
        PlayerPrefs.SetInt("LevelsUnlocked", 1);
        for (int i = 0; i < 8; i++)
        {
            PlayerPrefs.SetInt("Level" + (i + 1) + "Stars", 0);
            PlayerPrefs.SetString("Level" + (i + 1) + "Completed", "no");
        }
        PlayerPrefs.Save();
    }
}
