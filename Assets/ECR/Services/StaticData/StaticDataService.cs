using System;
using System.Threading.Tasks;
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
        
        private struct UserAttributes
        {
        }

        private struct AppAttributes
        {
        }
        
        public async void Initialize()
        {
            if (Utilities.CheckForInternetConnection())
                await InitializeRemoteConfigAsync();
            
            RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigLoaded;
            RemoteConfigService.Instance.SetEnvironmentID(ConfigEnvironmentId);

            // load remote config
            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        }

        public void ForHero()
        {
            throw new NotImplementedException();
        }

        public void ForEnemy()
        {
            throw new NotImplementedException();
        }

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
            //var hero = DeserializeObject<string>(RemoteConfigService.Instance.appConfig.GetJson("PlayerID"));
            
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