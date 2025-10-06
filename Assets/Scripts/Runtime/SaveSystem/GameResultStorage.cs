using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public static class GameResultStorage 
{
    private const string GAME_RESULT_KEY_TEMPLATE = "GameResult_{0}";
    private const string GAME_COUNT_KEY = "GameCount";
    private const string PLAYER_MONEY_AMOUNT = "PlayerMoney";

    public static void SaveGameResult(int moneyAmount, GameResult result)
    {
        int count = PlayerPrefs.GetInt(GAME_COUNT_KEY, 0) + 1;
        result.RoundNumber = count;

        string json = JsonUtility.ToJson(result);
        
        string key = string.Format(GAME_RESULT_KEY_TEMPLATE,count);

        PlayerPrefs.SetInt(PLAYER_MONEY_AMOUNT, moneyAmount);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.SetInt(GAME_COUNT_KEY, count);
        PlayerPrefs.Save();

        Debug.Log($"Game {count} save : {json}");
    }

    public static List<GameResult> LoadAllGameResult()
    {
        var results = new List<GameResult>();
        int count = PlayerPrefs.GetInt(GAME_COUNT_KEY, 0);

        for (int i = 1; i <= count; i++)
        {
            string key = string.Format(GAME_RESULT_KEY_TEMPLATE, i);
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                GameResult result = JsonUtility.FromJson<GameResult>(json);
                results.Add(result);
            }
        }
        return results;
    }
    public static void ClearAllResults()
    {
        int count = PlayerPrefs.GetInt(GAME_COUNT_KEY, 0);

        for (int i = 1;i <= count;i++)
        {
            string key = string.Format(GAME_RESULT_KEY_TEMPLATE,i);
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.DeleteKey(GAME_COUNT_KEY);
        PlayerPrefs.Save();
        Debug.Log("results cleared");
    }
    public static int GetGameCount()
    {
        return PlayerPrefs.GetInt(GAME_COUNT_KEY);
    }
}
