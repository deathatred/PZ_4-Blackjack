using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameContext : MonoInstaller
{
    [SerializeField] private List<Canvas> _viewsList;
    public override void InstallBindings()
    {
        Container.Bind<EventBus>().AsSingle().NonLazy();
        Container.Bind<MoneyManager>().FromNew().AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<Dealer>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<DeckManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GameStateMachine>().FromNew().AsSingle().NonLazy();
        Container.Bind<List<Canvas>>().FromInstance(_viewsList).AsTransient();
        Container.Bind<ViewManager>().FromNew().AsSingle().WithArguments(_viewsList).NonLazy();
        Container.Bind<GameStateFactory>().AsSingle();
   
    }
}
