using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        protected IPersistentProgressService ProgressService;
        protected PlayerProgress progress => ProgressService.progress;

        public void Construct(IPersistentProgressService progressService)
        {
            ProgressService = progressService;
        }
        
        private void Awake() =>
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        protected virtual void OnAwake() =>
            closeButton.onClick.AddListener(() => Destroy(gameObject));

        private void OnDestroy() =>
            Cleanup();

        protected virtual void Initialize(){}
        protected virtual void SubscribeUpdates(){}
        protected virtual void Cleanup(){}
    }
}