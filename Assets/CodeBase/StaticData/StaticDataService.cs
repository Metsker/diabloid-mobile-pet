using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataMonstersPath = "StaticData/Monsters";
        private const string StaticDataLevelsPath = "StaticData/Levels";
        private const string StaticDataWindowsPath = "StaticData/UI/WindowStaticData";
        
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public void Load()
        {
            _monsters = LoadMonsters();
            _levels = LoadLevels();
            _windowConfigs = LoadWindows();
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters.TryGetValue(typeId, out MonsterStaticData staticData)
                ? staticData
                : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;

        public WindowConfig ForWindow(WindowId windowId) =>
                _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
                    ? windowConfig
                    : null;

        private static Dictionary<MonsterTypeId, MonsterStaticData> LoadMonsters() =>
            Resources
                .LoadAll<MonsterStaticData>(StaticDataMonstersPath)
                .ToDictionary(key => key.monsterTypeId, data => data);

        private static Dictionary<string, LevelStaticData> LoadLevels() =>
            Resources
                .LoadAll<LevelStaticData>(StaticDataLevelsPath)
                .ToDictionary(key => key.levelKey, data => data);
        
        private static Dictionary<WindowId, WindowConfig> LoadWindows() =>
            Resources
                .Load<WindowStaticData>(StaticDataWindowsPath)
                .configs
                .ToDictionary(key => key.windowId, data => data);
    }
}