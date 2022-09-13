using CodeBase.Infrastructure.Services.PersistentProgress;
using Unity.Example;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class RewardedAdItem : MonoBehaviour
    {
        private const int Reward = 13;
        
        [SerializeField] private Button showAdButton;
        [SerializeField] private GameObject[] adActiveObjects;
        [SerializeField] private GameObject[] adInactiveObjects;

        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            _adsService = adsService;
            _progressService = progressService;
        }
        
        public void Initialize()
        {
            showAdButton.onClick.AddListener(OnShowAdClicked);

            RefreshAvailableAd();
        }

        public void Subscribe()
        {
            _adsService.AdLoadedCall += RefreshAvailableAd;
            _adsService.UserRewardedCall += OnVideoShown;
        }

        public void Cleanup()
        {
            _adsService.AdLoadedCall -= RefreshAvailableAd;
            _adsService.UserRewardedCall -= OnVideoShown;
        }

        private void OnShowAdClicked() =>
            _adsService.ShowRewardedAd();

        private void OnVideoShown() =>
            _progressService.progress.worldData.lootData.Add(Reward);

        private void RefreshAvailableAd()
        {
            bool adReady = _adsService.isReady;
            
            foreach (GameObject adActiveObject in adActiveObjects)
                adActiveObject.SetActive(adReady);

            foreach (GameObject adInactiveObject in adInactiveObjects)
                adInactiveObject.SetActive(!adReady);
        }
    }
}