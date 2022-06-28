using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsSender : MonoBehaviour
{
    public void LevelStarted(int levelNumber)
    {
        //Manda um Acalytics CustomEvent chamado "Levels Attempts".
        Analytics.CustomEvent("Levels Attempts", new Dictionary<string, object>
        {
            // Passando o número do nível.
            { "Level", levelNumber }
        });
    }

    public void LevelCompleted(int levelNumber, float time, int lives, int stars)
    {
        //Manda um Acalytics CustomEvent chamado "Levels Completed".
        Analytics.CustomEvent("Levels Completed", new Dictionary<string, object>
        {
            // passando o número do nível, tempo levado, vidas e estrelas.
            { "Level", levelNumber },
            { "Time Needed", time },
            { "Lives Left", lives },
            { "Stars", stars }
        });
    }
}
