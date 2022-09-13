using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IWindowService _windowService;
        
        private GameObject _heroGameObject;
        public List<ISavedProgressReader> progressReaders { get; } = new();
        public List<ISavedProgress> progressWriters { get; } = new();

        public GameFactory(
            IAssetProvider assetProvider,
            IStaticDataService staticData,
            IPersistentProgressService progressService,
            IWindowService windowService)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
            _windowService = windowService;
        }

        public GameObject CreateHero(Vector3 at)
        {
            var hero = InstantiateRegistered(AssetPath.Hero, at);
            _heroGameObject = hero;
            return hero;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.Hud);
            hud
                .GetComponentInChildren<LootCounter>()
                .Construct(_progressService.progress.worldData);
            
            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(_windowService);

            return hud;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegistered(AssetPath.Loot)
                .GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.progress.worldData);
            
            return lootPiece;
        }

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            GameObject monster = Object.Instantiate(monsterData.prefab, parent.position, Quaternion.identity, parent);

            var health = monster.GetComponent<IHealth>();
            health.Current = monsterData.hp;
            health.Max = monsterData.hp;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<Follow>().Construct(_heroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.moveSpeed;

            var attack = monster.GetComponent<Attack>();
            attack.Construct(_heroGameObject.transform);
            attack.damage = monsterData.damage;
            attack.cleavage = monsterData.cleavage;
            attack.effectiveDistance = monsterData.effectiveDistance;

            var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(monsterData.minLoot, monsterData.maxLoot);
            lootSpawner.Construct(this);

            return monster;
        }

        public void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            SpawnPoint spawner = InstantiateRegistered(AssetPath.Spawner, at)
                .GetComponent<SpawnPoint>();
            
            spawner.Construct(this);
            spawner.id = spawnerId;
            spawner.monsterTypeId = monsterTypeId;
        }

        public void Cleanup()
        {
            progressReaders.Clear();
            progressWriters.Clear();
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                progressWriters.Add(progressWriter);

            progressReaders.Add(progressReader);
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var gameObject = _assetProvider.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            var gameObject = _assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
    }
}