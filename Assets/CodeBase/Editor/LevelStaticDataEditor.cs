using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPoint = "InitialPoint";
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LevelStaticData levelData = (LevelStaticData) target;

            if (GUILayout.Button("Collect"))
            {
                levelData.enemySpawners = FindObjectsOfType<SpawnMarker>()
                    .Select(x => new EnemySpawnerData(x.uniqueId.id, x.typeId, x.transform.position))
                    .ToList();
                levelData.transferTriggers = FindObjectsOfType<LevelTransferMarker>()
                    .Select(x => new LevelTransferTriggerData(x.transform.position, x.GetComponent<BoxCollider>().size, x.transferTo))
                    .ToList();
                
                levelData.levelKey = SceneManager.GetActiveScene().name;
                levelData.initialHeroPosition = GameObject.FindWithTag(InitialPoint).transform.position;
            }
            EditorUtility.SetDirty(target);
        }
    }
}