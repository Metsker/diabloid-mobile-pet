using System.Collections.Generic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string levelKey;

        public List<EnemySpawnerData> enemySpawners;
        public List<LevelTransferTriggerData> transferTriggers;

        public Vector3 initialHeroPosition;
    }
}