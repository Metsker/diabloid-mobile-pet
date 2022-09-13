using System;
using System.Collections;
using CodeBase.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        private const float DestroyTime = 1.5f;
        
        [SerializeField] private GameObject skull;
        [SerializeField] private GameObject pickupFxPrefab;
        [SerializeField] private GameObject pickupPopup;
        [SerializeField] private TextMeshPro lootText;
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;
        private Sequence _skullAnimation;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }
        
        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void Start()
        {
            _skullAnimation = DOTween.Sequence();
            _skullAnimation
                .Append(SkullRotator())
                .Join(SkullMover())
                .SetLoops(-1);
        }

        private void OnTriggerEnter(Collider other) =>
            Pickup();

        private Tween SkullRotator()
        {
            return skull.transform
                .DORotate(Vector3.up * 60, 1)
                .SetRelative()
                .SetLoops(999, LoopType.Incremental)
                .SetEase(Ease.Linear);
        }

        private Tween SkullMover()
        {
            return skull.transform
                .DOMoveY(0.25f, 1)
                .SetRelative()
                .SetLoops(999, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void Pickup()
        {
            if (_picked)
                return;
            
            _picked = true;

            UpdateWorldData();
            HideSkull();
            PlayPickupFx();
            ShowText();
            StartCoroutine(DestroyTimer());
        }

        private void UpdateWorldData() =>
            _worldData.lootData.Collect(_loot);

        private void HideSkull()
        {
            _skullAnimation.Kill();
            skull.SetActive(false);
        }

        private void PlayPickupFx() =>
            Instantiate(pickupFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            lootText.text = _loot.value.ToString();
            pickupPopup.SetActive(true);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(DestroyTime);
            
            Destroy(gameObject);
        }
    }
}