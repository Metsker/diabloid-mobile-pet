using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void SetValue(float current, float max) =>
            image.fillAmount = current / max;
    }
}