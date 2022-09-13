using DG.Tweening;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            canvasGroup
                .DOFade(0, 1)
                .OnComplete(() => 
                    gameObject.SetActive(false));
        }
    }
}
