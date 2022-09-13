using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using Unity.Example;
using UnityEngine;

namespace CodeBase.UI.Services
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adsService;

        private Transform _uiRoot;

        public UIFactory(
            IAssetProvider assetProvider,
            IStaticDataService staticData,
            IPersistentProgressService progressService,
            IAdsService adsService)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
        }
        
        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            ShopWindow window = Object.Instantiate(config.prefab, _uiRoot) as ShopWindow;
            window.Construct(_adsService, _progressService);
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _assetProvider.Instantiate(AssetAddress.UIRoot);
            _uiRoot = root.transform;
        }
    }
}