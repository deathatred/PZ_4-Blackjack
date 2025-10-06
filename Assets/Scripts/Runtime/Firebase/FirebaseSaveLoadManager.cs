using Cysharp.Threading.Tasks;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FirebaseSaveLoadManager
{
    private const string GAME_RESULT_KEY_TEMPLATE = "GameResult_{0}";
    private const string GAME_COUNT_KEY = "GameCount";
    private const string PLAYER_MONEY_AMOUNT = "MoneyAmount";

    private FirebaseBootstrap _firebase;

    public FirebaseSaveLoadManager(FirebaseBootstrap bootstrap)
    {
        _firebase = bootstrap;
    }

    public async UniTask SaveGameResultToFirebaseAsync(int moneyAmount, GameResult gameResult,
        CancellationTokenSource cts = default)
    {
        try
        {
            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;

            string gameCountKey = string.Format(GAME_RESULT_KEY_TEMPLATE, gameResult.RoundNumber);
            string jsonKey = JsonUtility.ToJson(gameResult);
            if (cts != null)
            {
                await db.Child($"users/{uid}/games/{gameResult.RoundNumber}")
                 .SetRawJsonValueAsync(jsonKey)
                   .AsUniTask().AttachExternalCancellation(cts.Token);
                await db.Child($"users/{uid}/{PLAYER_MONEY_AMOUNT}")
                 .SetValueAsync(moneyAmount)
                   .AsUniTask().AttachExternalCancellation(cts.Token);
            }
            else
            {
                await db.Child($"users/{uid}/games/{gameResult.RoundNumber}")
              .SetRawJsonValueAsync(jsonKey)
                .AsUniTask();
                await db.Child($"users/{uid}/{PLAYER_MONEY_AMOUNT}")
                 .SetValueAsync(moneyAmount)
                   .AsUniTask();
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("Saving cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Saving failed, reason: {ex}");
        }
    }
    public async UniTask<int> LoadMoneyAmount(CancellationTokenSource cts = default)
    {
        int result = 0;
        try
        {
            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;
            DataSnapshot snapshot;
            if (cts != null)
            {
                snapshot = await db.Child($"users/{uid}/{PLAYER_MONEY_AMOUNT}").GetValueAsync()
                    .AsUniTask().AttachExternalCancellation(cts.Token);
            }
            else
            {
                snapshot = await db.Child($"users/{uid}/{PLAYER_MONEY_AMOUNT}")
                .GetValueAsync()
                .AsUniTask();
            }
            if (snapshot.Exists && snapshot.Value != null)
            {
                if (int.TryParse(snapshot.Value.ToString(), out int res))
                {
                    result = res;
                }
            }  
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("Loading cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Loading failed, reason: {ex}");
        }
        return result;
    }
    public async UniTask<List<GameResult>> LoadAllGameResultsAsync(CancellationTokenSource cts = default)
    {
        var results = new List<GameResult>();

        try
        {
            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;

            DataSnapshot snapshot;
            if (cts != null)
            {
                snapshot = await db.Child($"users/{uid}/games")
                    .GetValueAsync()
                    .AsUniTask()
                    .AttachExternalCancellation(cts.Token);
            }
            else
            {
                snapshot = await db.Child($"users/{uid}/games")
                    .GetValueAsync()
                    .AsUniTask();
            }

            if (!snapshot.Exists)
            {
                Debug.Log("No saved games found in Firebase.");
                return results;
            }

            foreach (var child in snapshot.Children)
            {
                string json = child.GetRawJsonValue();

                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        GameResult result = JsonUtility.FromJson<GameResult>(json);
                        results.Add(result);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to parse GameResult for child {child.Key}: {e.Message}");
                    }
                }
            }

            Debug.Log($"Loaded {results.Count} game results from Firebase.");
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Loading all results cancelled.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while loading all results: {ex}");
        }

        return results;
    }
    public async UniTask ClearAllDataFromFirebaseAsync(CancellationTokenSource cts = default)
    {
        var db = FirebaseBootstrap.Db;
        var uid = FirebaseBootstrap.Uid;

        var t = db.Child($"users/{uid}").RemoveValueAsync();
        if (cts != null)
        {
            await t.AsUniTask().AttachExternalCancellation(cts.Token);
        }
        else
        {
            await t.AsUniTask();
        }
    }
}

