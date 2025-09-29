using UnityEngine;
using Zenject;

public class GameContext : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<DeckManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GameStateMachine>().FromNew().AsSingle().NonLazy();
    }
}
