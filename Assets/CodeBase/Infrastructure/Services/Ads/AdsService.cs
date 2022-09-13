using System;
using System.ComponentModel;
using CodeBase.Infrastructure.Services.PersistentProgress;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;

namespace Unity.Example
{
    public class AdsService : IAdsService
    {
        private const string ADUnitId = "Rewarded_Android";
        private const string AndroidGameId = "4922841";
        private const string IOSGameId = "4922840";

        public bool isReady => _ad?.AdState == AdState.Loaded;
        
        public event Action AdLoadedCall;
        public event Action UserRewardedCall;
        
        private IRewardedAd _ad;

        public async void Initialize()
        {
            try
            {
                var initializationOptions = new InitializationOptions();
                initializationOptions.SetGameId(GameID());
                await UnityServices.InitializeAsync(initializationOptions);

                InitializationComplete();
            }
            catch (Exception e)
            {
                InitializationFailed(e);
            }
        }

        public async void ShowRewardedAd()
        {
            if (!isReady) 
                return;
            try
            {
                var showOptions = new RewardedAdShowOptions();
                showOptions.AutoReload = true;
                await _ad.ShowAsync(showOptions);
                AdShown();
            }
            catch (ShowFailedException e)
            {
                AdFailedShow(e);
            }
        }

        private void InitializationComplete()
        {
            SetupAd();
            LoadAd();
        }

        private void SetupAd()
        {
            _ad = MediationService.Instance.CreateRewardedAd(ADUnitId);

            _ad.OnClosed += AdClosed;
            _ad.OnClicked += AdClicked;
            _ad.OnLoaded += AdLoaded;
            _ad.OnFailedLoad += AdFailedLoad;
            _ad.OnUserRewarded += UserRewarded;
            
            MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
        }

        private async void LoadAd()
        {
            try
            {
                await _ad.LoadAsync();
            }
            catch (LoadFailedException)
            {
                // We will handle the failure in the OnFailedLoad callback
            }
        }

        private void InitializationFailed(Exception e)
        {
            Debug.Log("Initialization Failed: " + e.Message);
        }

        private void AdLoaded(object sender, EventArgs e)
        {
            AdLoadedCall?.Invoke();
            Debug.Log("Ad loaded");
        }

        private void AdFailedLoad(object sender, LoadErrorEventArgs e) =>
            Debug.Log($"Failed to load ad: {e.Message}");

        private void AdShown() =>
            Debug.Log("Ad shown!");

        private void AdClosed(object sender, EventArgs e) =>
            Debug.Log("Ad has closed");

        private void AdClicked(object sender, EventArgs e) =>
            Debug.Log("Ad has been clicked");

        private void AdFailedShow(ShowFailedException e) =>
            Debug.Log(e.Message);

        private void UserRewarded(object sender, RewardEventArgs e)
        {
            UserRewardedCall?.Invoke();
            Debug.Log($"Received reward: type:{e.Type}; amount:{e.Amount}");
        }

        private void ImpressionEvent(object sender, ImpressionEventArgs args)
        {
            var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
            Debug.Log($"Impression event from ad unit id {args.AdUnitId} {impressionData}");
        }

        private static string GameID()
        {
            Debug.Log(Application.platform);
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return AndroidGameId;
                case RuntimePlatform.IPhonePlayer:
                    return IOSGameId;
                case RuntimePlatform.WindowsEditor:
                    throw new InvalidEnumArgumentException(nameof(Application.platform));
                    return AndroidGameId;
                default:
                    throw new InvalidEnumArgumentException(nameof(Application.platform));
            }
        }
    }
}