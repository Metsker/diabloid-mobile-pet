using System;
using CodeBase.Infrastructure.Services;

namespace Unity.Example
{
    public interface IAdsService : IService
    {
        event Action AdLoadedCall;
        event Action UserRewardedCall;
        bool isReady { get; }
        void Initialize();
        void ShowRewardedAd();
    }
}