using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData worldData;
        public HeroState heroState;
        public HeroStats heroStats;
        public KillData killData;

        public PlayerProgress()
        {
            worldData = new WorldData();
            heroStats = new HeroStats();
            heroState = new HeroState();
            killData = new KillData();
        }
    }
}