using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private void Start()
    {
        StartData();
    }

    void StartData()
    {
        if (!PlayerPrefs.HasKey("LevelsUnlocked"))
        {
            RereateData();
        }
    }

    public void RereateData()
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
