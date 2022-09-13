using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] 
        private HpBar hpBar;
        private IHealth _health;

        public void Construct(IHealth heroHealth)
        {
            _health = heroHealth;
            
            if(_health != null)
                _health.HealthChanged += UpdateHpBar;
        }

        private void Start()
        {
            IHealth health = GetComponent<IHealth>();
            
            if (health != null) 
                Construct(health);
        }

        private void OnDestroy()
        {
            if(_health != null)
                _health.HealthChanged -= UpdateHpBar;
        }

        private void UpdateHpBar() =>
            hpBar.SetValue(_health.Current, _health.Max);
    }
}