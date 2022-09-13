using System;
using UnityEngine;

namespace CodeBase.StaticData
{
    [Serializable]
    public class EnemySpawnerData
    {
        public string id;
        public MonsterTypeId monsterTypeId;
        public Vector3 position;

        public EnemySpawnerData(string id, MonsterTypeId monsterTypeId, Vector3 position)
        {
            this.id = id;
            this.monsterTypeId = monsterTypeId;
            this.position = position;
        }
    }
}