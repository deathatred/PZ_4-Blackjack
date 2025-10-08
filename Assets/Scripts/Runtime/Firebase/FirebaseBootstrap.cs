using UnityEngine;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using System;
using System.Threading;
using Firebase;
using Firebase.Auth;

public class FirebaseBootstrap
{
    public const string DATABASE_URL
        = "https://pz-4-31371-default-rtdb.europe-west1.firebasedatabase.app/";

    public static string Uid { get; private set; }
    public static DatabaseReference Db { get; private set; }
    public static bool IsReady { get; private set; }

    public async UniTask Init(Action onReady = null, bool force = false,
        CancellationTokenSource cts = default)
    {
        if (IsReady && !force)
        {
            onReady?.Invoke();
            return;
        }
        IsReady = false;

        try
        {
            var deps = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (deps != DependencyStatus.Available)
            {
                Debug.LogError($"Fb deps: {deps}");
            }

            var app = FirebaseApp.DefaultInstance;
            var auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser == null)
            {
                var signIn = auth.SignInAnonymouslyAsync();
                if (cts != null)
                {
                    await signIn.AsUniTask().AttachExternalCancellation(cts.Token);
                }
                else
                {
                    await signIn.AsUniTask();
                }
                if (signIn.IsFaulted) return;
            }
            Uid = auth.CurrentUser.UserId;

            var db = FirebaseDatabase.GetInstance(app, DATABASE_URL);
            db.SetPersistenceEnabled(false);
            Db = db.RootReference;

            IsReady = true;
            onReady?.Invoke();
            Debug.Log($"FB Ready, uid = {Uid}");
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("FB InitAsync cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"FB InitAsync error {ex}");
        }
    }
}
