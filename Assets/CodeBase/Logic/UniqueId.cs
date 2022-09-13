using CodeBase.CustomAttributes;
using UnityEngine;

namespace CodeBase.Logic
{
    public class UniqueId : MonoBehaviour
    {
        [ReadOnlyInspector]
        public string id = "";

        [ContextMenu("Clear")]
        private void Clear() =>
            id = "";
    }
}