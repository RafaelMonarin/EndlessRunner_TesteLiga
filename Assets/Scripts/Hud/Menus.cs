using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject exitMenu;
    public GameObject resetAllMenu;

    public LevelSelector levelSelector;

    public void MainMenu()
    {
        CloseAll();
        mainMenu.SetActive(true);
    }

    public void LevelMenu()
    {
        levelSelector.ResetPages();
        CloseAll();
        levelMenu.SetActive(true);
    }

    public void ExitMenu()
    {
        CloseAll();
        exitMenu.SetActive(true);
    }

    void CloseAll()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(false);
        exitMenu.SetActive(false);
        resetAllMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetAllMenu()
    {
        resetAllMenu.SetActive(true);
    }
}
