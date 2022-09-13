using System;
using System.Linq;
using CodeBase.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Application = UnityEngine.Application;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(UniqueId))]
    public class UniqueIdEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var uniqueId = (UniqueId) target;
            
            if (IsPrefab(uniqueId.gameObject))
                return;

            if (string.IsNullOrEmpty(uniqueId.id))
            {
                Generate(uniqueId);
                return;
            }
            
            if (IsNotUnique(uniqueId)) 
                Generate(uniqueId);
        }

        private static bool IsPrefab(GameObject uniqueIdGameObject) =>
            uniqueIdGameObject.scene.rootCount == 0 || uniqueIdGameObject.scene.name == uniqueIdGameObject.name;

        private static bool IsNotUnique(UniqueId uniqueId) =>
            FindObjectsOfType<UniqueId>().Any(other => other != uniqueId && other.id == uniqueId.id);

        private static void Generate(UniqueId uniqueId)
        {
            if (Application.isPlaying) 
                return;
            
            uniqueId.id = Guid.NewGuid().ToString();
            
            EditorUtility.SetDirty(uniqueId);
            EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
        }
    }
}