using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }
        
        public void SaveProgress()
        {
            foreach (var progressWriter in _gameFactory.progressWriters)
                progressWriter.UpdateProgress(_progressService.progress);
            
            PlayerPrefs.SetString(ProgressKey, _progressService.progress.ToJson());
        }

        public PlayerProgress LoadProgressOrInitNew()
        {
            var progress = PlayerPrefs.GetString(ProgressKey);
            return string.IsNullOrEmpty(progress) ?  NewProgress() : progress.ToDeserialized<PlayerProgress>();
        }
         
        private static PlayerProgress NewProgress()
        {
            var playerProgress = new PlayerProgress();
            playerProgress.heroStats.maxHP = 50;
            playerProgress.heroState.currentHP = playerProgress.heroStats.maxHP;
            playerProgress.heroStats.damage = 10;
            playerProgress.heroStats.damageRadius = 0.5f;
            
            return playerProgress;
        }
    }
}