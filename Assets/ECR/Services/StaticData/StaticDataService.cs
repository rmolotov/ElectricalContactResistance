using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECR.StaticData;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

using static Newtonsoft.Json.JsonConvert;

namespace ECR.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string ConfigEnvironmentId =
            "d7f17096-1635-42aa-b0d3-c7892f39d67c"; // development
            //"prod"; //production

        private Dictionary<EnemyType, EnemyStaticData> _enemies;
        private HeroStaticData _heroStaticData;

        #region Attributes structs

        private struct UserAttributes
        {
        }

        private struct AppAttributes
        {
        }

        #endregion

        public Action Initialized { get; set; }

        public async void Initialize()
        {
            if (Utilities.CheckForInternetConnection())
                await InitializeRemoteConfigAsync();
            
            RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigLoaded;
            RemoteConfigService.Instance.SetEnvironmentID(ConfigEnvironmentId);
            
            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        }

        public HeroStaticData ForHero() =>
            _heroStaticData;

        public EnemyStaticData ForEnemy(EnemyType enemyType) => 
            _enemies.TryGetValue(enemyType, out var staticData)
                ? staticData
                : null;

        public void ForLevel()
        {
            throw new NotImplementedException();
        }

        public void ForWindow()
        {
            throw new NotImplementedException();
        }


        private void OnRemoteConfigLoaded(ConfigResponse configResponse)
        {
            LoadHeroData();
            LoadEnemiesData();
            
            LogConfigsResponseResult(configResponse);
            
            Initialized?.Invoke();
        }

        private void LoadHeroData()
        {
            _heroStaticData = DeserializeObject<HeroStaticData>(RemoteConfigService.Instance.appConfig.GetJson("Hero"));
        }

        private void LoadEnemiesData()
        {
            _enemies = new Dictionary<EnemyType, EnemyStaticData>();
            foreach (var enemyType in (EnemyType[]) Enum.GetValues(typeof(EnemyType)))
            {
                var enemyStaticData = DeserializeObject<EnemyStaticData>(
                    RemoteConfigService.Instance.appConfig.GetJson(enemyType.ToString())
                );
                _enemies.Add(enemyType, enemyStaticData);
            }
        }

        private static void LogConfigsResponseResult(ConfigResponse configResponse)
        {
            switch (configResponse.requestOrigin)
            {
                case ConfigOrigin.Default:
                    Debug.Log("No configs loaded; using default config");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log("No configs loaded; using cached config");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log("New configs loaded; updating cached config...");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}