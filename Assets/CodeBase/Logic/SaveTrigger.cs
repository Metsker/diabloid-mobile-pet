using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        public BoxCollider boxCollider;
        
        private ISaveLoadService _saveLoadService;
        
        private void Awake()
        {
            _saveLoadService = AllServices.container.Single<ISaveLoadService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            _saveLoadService.SaveProgress();
            Debug.Log("Saved");
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if(!boxCollider) 
                return;
            
            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
        }
    }
}