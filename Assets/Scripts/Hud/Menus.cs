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

    // Método para o menu inicial.
    public void MainMenu()
    {
        // Chama "CloseAll()" e ativa o menu inicial.
        CloseAll();
        mainMenu.SetActive(true);
    }

    // Método para o menu de seleção de níveis.
    public void LevelMenu()
    {
        // Chama "CloseAll()", chama "ResetPages()" e ativa o menu de seleção de níveis.
        CloseAll();
        levelSelector.ResetPages();
        levelMenu.SetActive(true);
    }

    // Método para o menu de sair.
    public void ExitMenu()
    {
        // Chama "CloseAll()" e ativa o menu de sair.
        CloseAll();
        exitMenu.SetActive(true);
    }

    // Método que desativa todos os menus.
    void CloseAll()
    {
        // Desativa todos os menus.
        mainMenu.SetActive(false);
        levelMenu.SetActive(false);
        exitMenu.SetActive(false);
        resetAllMenu.SetActive(false);
    }

    // Método que fecha o aplicativo.
    public void ExitGame()
    {
        // Fecha o aplicativo.
        Application.Quit();
    }

    // Método para o menu de resetar tudo.
    public void ResetAllMenu()
    {
        // Ativa o menu de resetar tudo.
        resetAllMenu.SetActive(true);
    }
}
