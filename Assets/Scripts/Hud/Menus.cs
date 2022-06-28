using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    // Menus.
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject exitMenu;
    public GameObject resetAllMenu;

    // Script.
    public LevelSelector levelSelector;

    // M�todo para o menu inicial.
    public void MainMenu()
    {
        // Chama "CloseAll()" e ativa o menu inicial.
        CloseAll();
        mainMenu.SetActive(true);
    }

    // M�todo para o menu de sele��o de n�veis.
    public void LevelMenu()
    {
        // Chama "CloseAll()", chama "ResetPages()" e ativa o menu de sele��o de n�veis.
        CloseAll();
        levelSelector.ResetPages();
        levelMenu.SetActive(true);
    }

    // M�todo para o menu de sair.
    public void ExitMenu()
    {
        // Chama "CloseAll()" e ativa o menu de sair.
        CloseAll();
        exitMenu.SetActive(true);
    }

    // M�todo que desativa todos os menus.
    void CloseAll()
    {
        // Desativa todos os menus.
        mainMenu.SetActive(false);
        levelMenu.SetActive(false);
        exitMenu.SetActive(false);
        resetAllMenu.SetActive(false);
    }

    // M�todo que fecha o aplicativo.
    public void ExitGame()
    {
        // Fecha o aplicativo.
        Application.Quit();
    }

    // M�todo para o menu de resetar tudo.
    public void ResetAllMenu()
    {
        // Ativa o menu de resetar tudo.
        resetAllMenu.SetActive(true);
    }
}
