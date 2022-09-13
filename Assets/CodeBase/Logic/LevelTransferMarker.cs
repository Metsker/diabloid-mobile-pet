using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelTransferMarker : MonoBehaviour
    {
        public string transferTo;
    }
}