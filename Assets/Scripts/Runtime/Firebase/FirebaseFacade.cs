using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class FirebaseManager : IDisposable
{
    private FirebaseBootstrap _firebase;
    private FirebaseSaveLoadManager _saveLoad;
    private CancellationTokenSource _cts;
    public bool IsReady { get; private set; }

    private FirebaseManager()
    {
        _firebase = new FirebaseBootstrap();
        _firebase.Init(SetIsReadyTrue, false,_cts).Forget();
        _saveLoad = new FirebaseSaveLoadManager(_firebase);
    }
    public async UniTask SaveRoundDataAsync(int moneyAmount,GameResult result)
    {
       await _saveLoad.SaveGameResultToFirebaseAsync(moneyAmount,result,_cts);
    }
    public async UniTask<int> LoadMoneyAmountAsync()
    {
        return await _saveLoad.LoadMoneyAmount(_cts);
    }
    public async UniTask<List<GameResult>> LoadAllGamesDataAsync()
    {
        List<GameResult> result =  await _saveLoad.LoadAllGameResultsAsync(_cts);
        return result;
    }
    private void SetIsReadyTrue()
    {
        IsReady = true;
    }
    public void Dispose()
    {
        _cts.Cancel();
    }
}
