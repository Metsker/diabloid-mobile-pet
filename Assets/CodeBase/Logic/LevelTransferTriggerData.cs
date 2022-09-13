using System;
using UnityEngine;

namespace CodeBase.Logic
{
    [Serializable]
    public class LevelTransferTriggerData
    {
        public Vector3 position;
        public Vector3 colliderSize;
        public string transferTo;

        public LevelTransferTriggerData(Vector3 position, Vector3 colliderSize, string transferTo)
        {
            this.position = position;
            this.colliderSize = colliderSize;
            this.transferTo = transferTo;
        }
    }
}