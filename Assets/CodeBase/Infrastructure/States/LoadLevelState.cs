using System;
using System.Threading.Tasks;
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

        public LoadLevelState(
            GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory,
            IPersistentProgressService progressService,
            IStaticDataService staticData,
            IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public async void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            await _gameFactory.WarmUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();

            InformProgressReaders();
            
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot() =>
            await _uiFactory.CreateUIRoot();

        private async Task InitGameWorld()
        {
            var levelData = LevelStaticData();

            await InitSpawners(levelData);
            await InitLevelTransfers(levelData);
            
            var hero = await InitHero(levelData);
            CameraFollow(hero);

            await InitHud(hero);
        }

        private LevelStaticData LevelStaticData() =>
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        private async Task InitSpawners(LevelStaticData levelStaticData)
        {
            foreach (EnemySpawnerData spawnerData in levelStaticData.enemySpawners)
                await _gameFactory.CreateSpawner(spawnerData.position, spawnerData.id, spawnerData.monsterTypeId);
        }

        private async Task InitLevelTransfers(LevelStaticData levelData)
        {
            foreach (LevelTransferTriggerData transfer in levelData.transferTriggers) 
                await _gameFactory.CreateLevelTransfer(transfer.position, transfer.colliderSize, transfer.transferTo);
        }

        private async Task<GameObject> InitHero(LevelStaticData levelStaticData) =>
            await _gameFactory.CreateHero(levelStaticData.initialHeroPosition);

        private async Task InitHud(GameObject hero)
        {
            GameObject hud =  await _gameFactory.CreateHud();
            hud.GetComponent<ActorUI>().Construct(hero.GetComponent<HeroHealth>());           
        }

        private static void CameraFollow(GameObject followTarget)
        {
            Camera.main
                .GetComponent<CameraFollower>()
                .Follow(followTarget);
        }

        private void InformProgressReaders() =>
            _gameFactory.ProgressReaders.ForEach(reader => reader.LoadProgress(_progressService.progress));
    }
}