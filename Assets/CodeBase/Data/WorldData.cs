using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel positionOnLevel;
        public LootData lootData;

        public WorldData()
        {
            positionOnLevel = new PositionOnLevel();
            lootData = new LootData();
        }
    }
}