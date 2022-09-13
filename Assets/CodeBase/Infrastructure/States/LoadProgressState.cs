using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(
            GameStateMachine gameStateMachine,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgress();
            
            var payload = _progressService.progress.worldData.positionOnLevel.level;
            _gameStateMachine.Enter<LoadSceneState, string>(payload);
        }

        public void Exit()
        {
        }

        private void LoadProgress() =>
            _progressService.progress = _saveLoadService.LoadProgressOrInitNew();
    }
}