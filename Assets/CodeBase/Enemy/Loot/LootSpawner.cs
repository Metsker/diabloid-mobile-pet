using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] 
        private EnemyDeath enemyDeath;
        
        private IGameFactory _factory;
        private int _lootMax;
        private int _lootMin;

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
        }

        private void Start() =>
            enemyDeath.Happened += SpawnLoot;

        private void SpawnLoot()
        {
            LootPiece loot = _factory.CreateLoot();
            loot.transform.position = gameObject.transform.position;

            Loot lootItem = GenerateLoot();
            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            return new Loot
            {
                value = Random.Range(_lootMin, _lootMax)
            };
        }

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
    }
}