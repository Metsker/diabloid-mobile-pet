using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadSceneState : IPayLoadedState<string>
    {
        private const string InitialPoint = "InitialPoint";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        public LoadSceneState(
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
            InitSpawners();
            
            var hero = InitHero();
            CameraFollow(hero);

            InitHud(hero);
        }

        private void InitSpawners()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticData.ForLevel(sceneKey);
            foreach (EnemySpawnerData spawnerData in levelData.enemySpawners)
            {
                _gameFactory.CreateSpawner(spawnerData.position, spawnerData.id, spawnerData.monsterTypeId);
            }
        }

        private GameObject InitHero()
        {
            GameObject initialPoint = GameObject.FindWithTag(InitialPoint);
            GameObject hero = _gameFactory.CreateHero(initialPoint.transform.position);
            return hero;
        }

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