using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId monsterTypeId;
        
        [Header("Stats")]
        [Range(1, 100)]
        public int hp;
        
        [Range(1f, 50)]
        public float damage;

        [Range(1f, 50)] 
        public float moveSpeed;

        [Range(0.5f, 1)]
        public float effectiveDistance = 0.6f;

        [Range(0.5f, 1)]
        public float cleavage;
        
        [Min(0)]
        public int minLoot;
        
        [Min(0)]
        public int maxLoot;
        
        [Space]
        public AssetReferenceGameObject prefabReference;
    }
}