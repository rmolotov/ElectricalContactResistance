﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using ECR.StaticData;
using ECR.Services.Logging;

using static Newtonsoft.Json.JsonConvert;

namespace ECR.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string ConfigEnvironmentId =
            "d7f17096-1635-42aa-b0d3-c7892f39d67c"; // development
            //"prod"; //production
            
        private const string StagesList    = "StagesList";
        private const string ItemsList     = "ItemsList";
        private const string EnemiesList   = "EnemiesList";
        private const string HeroConfigKey = "Hero";
        
        private readonly ILoggingService _logger;

        private Dictionary<EnemyType, EnemyStaticData> _enemies;
        private Dictionary<string, StageStaticData> _stages;
        private Dictionary<string, InventoryItemStaticData> _items;
        private HeroStaticData _heroStaticData;
        private TaskCompletionSource<ConfigResponse> _fetchCompletionSource;

        #region Attributes structs

        private struct UserAttributes
        {
        }

        private struct AppAttributes
        {
        }

        #endregion

        public StaticDataService(ILoggingService loggingService) =>
            _logger = loggingService;
        

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            _fetchCompletionSource = new TaskCompletionSource<ConfigResponse>();
            
            var connection = Application.internetReachability;
            if (connection != NetworkReachability.NotReachable)
                await InitializeRemoteConfigAsync();
            
            RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigLoaded;
            RemoteConfigService.Instance.SetEnvironmentID(ConfigEnvironmentId);
            
            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
            await _fetchCompletionSource.Task;
        }

        public StageStaticData ForStage(string stageKey) =>
            _stages.TryGetValue(stageKey, out var stageData)
                ? stageData
                : null;

        public List<StageStaticData> GetAllStages =>
            _stages.Values.ToList();

        public InventoryItemStaticData ForInventoryItem(string itemKey) =>
            _items.TryGetValue(itemKey, out var itemData)
                ? itemData
                : null;

        public List<InventoryItemStaticData> GetAllItems => 
            _items.Values.ToList();

        public HeroStaticData ForHero() =>
            _heroStaticData;

        public EnemyStaticData ForEnemy(EnemyType enemyType) => 
            _enemies.TryGetValue(enemyType, out var staticData)
                ? staticData
                : null;

        public void ForWindow() => 
            throw new NotImplementedException();


        private void OnRemoteConfigLoaded(ConfigResponse configResponse)
        {
            LoadStagesData();
            LoadItemsData();
            LoadHeroData();
            LoadEnemiesData();
            
            LogConfigsResponseResult(configResponse);
            
            RemoteConfigService.Instance.FetchCompleted -= OnRemoteConfigLoaded;
            _fetchCompletionSource.SetResult(configResponse);
        }

        /* TODO:
         * Used in case of EconomyLocalService; when Remote SD provided by UGS Economy
         * Mark methods and props [Obsolete] if needed
         */
        private void LoadStagesData()
        {
            _stages = new Dictionary<string, StageStaticData>();
            
            var list = DeserializeObject<List<string>>(
                RemoteConfigService.Instance.appConfig.GetJson(StagesList));

            foreach (var stageKey in list)
                _stages[stageKey] = DeserializeObject<StageStaticData>(
                    RemoteConfigService.Instance.appConfig.GetJson(stageKey));
        }

        private void LoadItemsData() =>
            _items = (DeserializeObject<List<InventoryItemStaticData>>(
                    RemoteConfigService.Instance.appConfig.GetJson(ItemsList)
                ) ?? new List<InventoryItemStaticData>())
                .ToDictionary(it => it.ItemId, it => it);

        private void LoadHeroData() => 
            _heroStaticData = DeserializeObject<HeroStaticData>(RemoteConfigService.Instance.appConfig.GetJson(HeroConfigKey));

        private void LoadEnemiesData() =>
            _enemies = (DeserializeObject<List<EnemyStaticData>>(
                    RemoteConfigService.Instance.appConfig.GetJson(EnemiesList)
                ) ?? new List<EnemyStaticData>())
                .ToDictionary(e => e.EnemyType, e => e);

        private void LogConfigsResponseResult(ConfigResponse configResponse)
        {
            var message = configResponse.requestOrigin switch
            {
                ConfigOrigin.Default => "No configs loaded; using default config",
                ConfigOrigin.Cached  => "No configs loaded; using cached config",
                ConfigOrigin.Remote  => "New configs loaded; updating cached config...",
                _                    => throw new ArgumentOutOfRangeException()
            };
            _logger.LogMessage(message);
        }

        private static async Task InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}