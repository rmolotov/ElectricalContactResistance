using System.Collections.Generic;
using ECR.Data;
using ECR.Infrastructure.States.Interfaces;
using ECR.Services.PersistentData;
using ECR.Services.SaveLoad;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentDataService _progressService;
        private readonly ISaveLoadService _saveLoadProgressService;

        public LoadProgressState(
            GameStateMachine gameStateMachine, 
            IPersistentDataService progressService,
            ISaveLoadService saveLoadProgressService)
        {
            _stateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadProgressService = saveLoadProgressService;
        }

        public void Enter()
        {
            Debug.Log(typeof(LoadProgressState));
            
            LoadProgressOrInitNew();
            
            _stateMachine.Enter<LoadMetaState>();
        }

        public void Exit()
        {
            
        }
        
        private async void LoadProgressOrInitNew()
        {
            _progressService.Settings = 
                await _saveLoadProgressService.LoadSettings() 
                ?? NewSettings();
            
            _progressService.Progress = 
                await _saveLoadProgressService.LoadProgress() 
                ?? NewProgress();
            
            _progressService.Economy = 
                await _saveLoadProgressService.LoadEconomy() 
                ?? NewEconomy();
        }

        private PlayerEconomyData NewEconomy() =>
            new()
            {
                PlayerCurrency = 100,
                InventoryItems = new Dictionary<string, int>()
            };

        private PlayerProgressData NewProgress() =>
            new()
            {
                CompletedStages = new HashSet<string>()
            };

        private PlayerSettingsData NewSettings() =>
            new()
            {
                MusicVolume = 100,
                SfxVolume = 100,
                DebugEnabled = false,
                HapticEnabled = true
            };
    }
}