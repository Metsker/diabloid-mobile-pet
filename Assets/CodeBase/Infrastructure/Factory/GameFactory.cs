using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IWindowService _windowService;
        private readonly IGameStateMachine _gameStateMachine;
        
        private GameObject _heroGameObject;
        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();

        public GameFactory(
            IAssetProvider assetProvider,
            IStaticDataService staticData,
            IPersistentProgressService progressService,
            IWindowService windowService,
            IGameStateMachine gameStateMachine)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
            _windowService = windowService;
            _gameStateMachine = gameStateMachine;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(AssetAddress.Spawner);
            await _assetProvider.Load<GameObject>(AssetAddress.LevelTransfer);
            await _assetProvider.Load<GameObject>(AssetAddress.Loot);
        }
        
        public async Task<GameObject> CreateHero(Vector3 at)
        {
            GameObject hero = await InstantiateRegisteredAsync(AssetAddress.Hero, at);
            _heroGameObject = hero;
            return hero;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.Hud);
            hud
                .GetComponentInChildren<LootCounter>()
                .Construct(_progressService.progress.worldData);
            
            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(_windowService);

            return hud;
        }

        public async Task<LootPiece> CreateLoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.Loot);
            LootPiece lootPiece = InstantiateRegistered(prefab)
                .GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.progress.worldData);
            
            return lootPiece;
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            
            GameObject prefab = await _assetProvider.Load<GameObject>(monsterData.prefabReference);
            GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            IHealth health = monster.GetComponent<IHealth>();
            health.Current = monsterData.hp;
            health.Max = monsterData.hp;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<Follow>().Construct(_heroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.moveSpeed;

            EnemyAttack attack = monster.GetComponent<EnemyAttack>();
            attack.Construct(_heroGameObject.transform);
            attack.damage = monsterData.damage;
            attack.cleavage = monsterData.cleavage;
            attack.effectiveDistance = monsterData.effectiveDistance;

            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(monsterData.minLoot, monsterData.maxLoot);
            lootSpawner.Construct(this);

            return monster;
        }

        public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.Spawner);
            SpawnPoint spawner = InstantiateRegistered(prefab, at)
                .GetComponent<SpawnPoint>();
            
            spawner.Construct(this);
            spawner.ID = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;
        }

        public async Task CreateLevelTransfer(Vector3 at, Vector3 colliderSize, string to)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.LevelTransfer);
            LevelTransferTrigger transfer = InstantiateRegistered(prefab, at)
                .GetComponent<LevelTransferTrigger>();
            
            transfer.Construct(_gameStateMachine);
            transfer.GetComponent<BoxCollider>().size = colliderSize;
            transfer.TransferTo = to;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            
            _assetProvider.Cleanup();
        }

        private void RegisterProgressWatchers(GameObject watcher)
        {
            foreach (var progressReader in watcher.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
        
        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assetProvider.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
    }
}