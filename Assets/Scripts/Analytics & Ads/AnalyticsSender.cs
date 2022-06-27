using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsSender : MonoBehaviour
{
    public void LevelStarted(int levelNumber)
    {
        Analytics.CustomEvent("Levels Attempts", new Dictionary<string, object>
        {
            { "Level", levelNumber }
        });
    }

    public void LevelCompleted(int levelNumber, float time, int lives, int stars)
    {
        Analytics.CustomEvent("Levels Attempts", new Dictionary<string, object>
        {
            { "Level", levelNumber },
            { "Time Needed", time },
            { "Lives Left", lives },
            { "Stars", stars }
        });
    }
}
