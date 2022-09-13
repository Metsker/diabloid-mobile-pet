using System.Collections.Generic;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using Unity.Example;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI skullCountText;
        [SerializeField] private RewardedAdItem[] adItems;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            base.Construct(progressService);
            
            foreach (RewardedAdItem adItem in adItems)
                adItem.Construct(adsService, progressService);
        }
        
        protected override void Initialize()
        {
            foreach (RewardedAdItem adItem in adItems)
                adItem.Initialize();
            
            RefreshSkullCountText();
        }

        protected override void SubscribeUpdates()
        {
            foreach (RewardedAdItem adItem in adItems)
                adItem.Subscribe();
            
            progress.worldData.lootData.Changed += RefreshSkullCountText;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            foreach (RewardedAdItem adItem in adItems)
                adItem.Cleanup();
            
            progress.worldData.lootData.Changed -= RefreshSkullCountText;
        }

        private void RefreshSkullCountText() =>
            skullCountText.text = progress.worldData.lootData.collected.ToString();
    }
}