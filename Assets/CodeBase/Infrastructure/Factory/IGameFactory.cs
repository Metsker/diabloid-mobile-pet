using System;
using System.Collections.Generic;
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
        List<ISavedProgress> progressWriters { get; }
        List<ISavedProgressReader> progressReaders { get; }
        
        GameObject CreateHero(Vector3 at);
        GameObject CreateHud();
        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot();
        void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        void CreateLevelTransfer(Vector3 at, Vector3 colliderSize, string to);

        void Cleanup();
    }
}