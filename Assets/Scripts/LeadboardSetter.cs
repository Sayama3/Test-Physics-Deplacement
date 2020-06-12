using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LeadboardSetter
{
    public static string baseLevelKey = "BestScoreLevel";
    public static void LevelFinish(int levelNumber, float duration)
    {
        var key = baseLevelKey + levelNumber;
        //Debug.LogWarning("La clé est : " + key);
        //Debug.LogWarning("Duration : " + duration);
        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetFloat(key) >= duration)
            {
                PlayerPrefs.SetFloat(key,duration);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(key,duration);
        }
    }
    
    /// <summary>
    /// Rounding float value with defined digit precision.
    /// </summary>
    /// <param name ="num">Number to be rounded</param>
    /// <param name ="precision">Number of digit after comma (100 will be 0.00, 1000 will be 0.000 etc..)</param>
    /// <returns> Rounded float value </returns>
    public static float RoundValue(float num, float precision)
    {
        return Mathf.Floor(num * precision) / precision;
    }
}
