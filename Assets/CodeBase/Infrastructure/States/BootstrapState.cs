using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using CodeBase.UI.Services;
using CodeBase.UI.Services.Windows;
using Unity.Example;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "Initial";
        
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            
            RegisterServices();
        }

        public void Enter() =>
            _sceneLoader.Load(InitialScene, onLoaded: EnterLoadLevel);

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadProgressState>();

        public void Exit(){}

        private void RegisterServices()
        {
            _services.RegisterSingle(InputService());
            _services.RegisterSingle(InitedStaticDataService());
            
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            
            _services.RegisterSingle(InitedAdsService());

            _services.RegisterSingle<IUIFactory>(new UIFactory(
                _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IAdsService>()));
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
            
            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IWindowService>(),
                _services.Single<IGameStateMachine>()));

            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
                _services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>()));
        }

        private static IAdsService InitedAdsService()
        {
            IAdsService adsService = new AdsService();
            adsService.Initialize();
            return adsService;
        }

        private static IStaticDataService InitedStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            return staticData;
        }

        private static IInputService InputService() =>
            Application.isEditor ? new StandaloneInputService() : new MobileInputService();
    }
}