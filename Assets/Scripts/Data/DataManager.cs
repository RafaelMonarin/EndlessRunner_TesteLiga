using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] int levelsNumber;

    // Quando iniciar, chama StartData().
    private void Start()
    {
        StartData();
    }
    
    // Verifica se já possui o PlayerPrefs criado, se não, chama RecreateData().
    void StartData()
    {
        if (!PlayerPrefs.HasKey("LevelsUnlocked"))
        {
            RereateData();
        }
    }
    // Cria / zera o PlayerPrefs.
    public void RereateData()
    {
        // PlayerPrefs int com o número de níveis desbloquados.
        PlayerPrefs.SetInt("LevelsUnlocked", 1);
        // Loop que cria 2 Playerprefs para cada nível.
        for (int i = 0; i < levelsNumber; i++)
        {
            // PlayerPrefs int com o número de estrelas.
            PlayerPrefs.SetInt("Level" + (i + 1) + "Stars", 0);
            // Playerprefs string se o nível estiver completo.
            PlayerPrefs.SetString("Level" + (i + 1) + "Completed", "no");
        }
        // Salva o PlayerPrefs.
        PlayerPrefs.Save();
    }
}
