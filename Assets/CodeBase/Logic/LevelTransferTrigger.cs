using System;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Logic
{
    [Serializable]
    public class LevelTransferTrigger : MonoBehaviour
    {
        private const string Player = "Player";
        
        public string TransferTo { get; set; }
        
        private IGameStateMachine _stateMachine;
        private bool _triggered;

        public void Construct(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            print("enter");
            if (!CanTransfer(other))
                return;
            
            _stateMachine.Enter<LoadLevelState, string>(TransferTo);
            _triggered = true;
        }

        private bool CanTransfer(Collider other) =>
            !_triggered && other.CompareTag(Player);
    }
}