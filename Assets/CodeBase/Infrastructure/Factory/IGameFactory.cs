using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgress> ProgressWriters { get; }
        List<ISavedProgressReader> ProgressReaders { get; }
        
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        Task<LootPiece> CreateLoot();
        Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        Task CreateLevelTransfer(Vector3 at, Vector3 colliderSize, string to);

        Task WarmUp();
        void Cleanup();
    }
}