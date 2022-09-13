using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    [RequireComponent(typeof(UniqueId))]
    public class SpawnMarker : MonoBehaviour
    {
        [field: SerializeField]
        public MonsterTypeId typeId { get; private set; }
        
        [field: SerializeField]
        public UniqueId uniqueId { get; private set; }
    }
}