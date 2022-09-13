using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId monsterTypeId { get; set; }
        public string id { get; set; }

        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;
        private bool _slain;

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
        }
        
        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.killData.clearedSpawners.Contains(id))
                _slain = true;
            else
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain) 
                progress.killData.clearedSpawners.Add(id);
        }

        private void Spawn()
        {
            GameObject monster = _factory.CreateMonster(monsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null) 
                _enemyDeath.Happened -= Slay;
            
            _slain = true;
        }
    }
}