using System;
using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;
        private readonly IGameStateMachine _gameStateMachine;

        public LoadLevelState(
            GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory,
            IPersistentProgressService progressService,
            IStaticDataService staticData,
            IUIFactory uiFactory,
            IGameStateMachine gameStateMachine)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private void OnLoaded()
        {
            InitUIRoot();
            
            InitGameWorld();

            InformProgressReaders();
            
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitUIRoot() =>
            _uiFactory.CreateUIRoot();

        private void InitGameWorld()
        {
            var levelData = LevelStaticData();

            InitSpawners(levelData);
            InitLevelTransfers(levelData);
            
            var hero = InitHero(levelData);
            CameraFollow(hero);

            InitHud(hero);
        }

        private LevelStaticData LevelStaticData() =>
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        private void InitSpawners(LevelStaticData levelStaticData)
        {
            foreach (EnemySpawnerData spawnerData in levelStaticData.enemySpawners)
                _gameFactory.CreateSpawner(spawnerData.position, spawnerData.id, spawnerData.monsterTypeId);
        }

        private void InitLevelTransfers(LevelStaticData levelData)
        {
            foreach (LevelTransferTriggerData transfer in levelData.transferTriggers) 
                _gameFactory.CreateLevelTransfer(transfer.position, transfer.colliderSize, transfer.transferTo);
        }

        private GameObject InitHero(LevelStaticData levelStaticData) =>
            _gameFactory.CreateHero(levelStaticData.initialHeroPosition);

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            hud.GetComponent<ActorUI>().Construct(hero.GetComponent<HeroHealth>());           
        }

        private static void CameraFollow(GameObject followTarget)
        {
            Camera.main
                .GetComponent<CameraFollower>()
                .Follow(followTarget);
        }

        private void InformProgressReaders() =>
            _gameFactory.progressReaders.ForEach(reader => reader.LoadProgress(_progressService.progress));
    }
}