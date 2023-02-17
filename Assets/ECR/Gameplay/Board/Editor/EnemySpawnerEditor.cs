using System;
using ECR.StaticData;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ECR.Gameplay.Board.Editor
{
    [Serializable]
    public class EnemySpawnerEditor
    {
        [HideInTables][ReadOnly]
        public GameObject gameObject;
        
        [EnumToggleButtons] [OnValueChanged("ChangeType")]
        public EnemyType enemyType = EnemyType.Capacitor;

        [ReadOnly]
        [TableColumnWidth(150, resizable: false)]
        public Vector3 position;

        [TableColumnWidth(50, resizable: false)]
        [Button(SdfIconType.ArrowsMove, "", Expanded = false)]
        [PropertyOrder(-1)]
        public void Select() => 
            Selection.SetActiveObjectWithContext(gameObject, null);

        private void ChangeType()
        {
            gameObject.name = $"Spawner : {enemyType}";
            EditorWindow.GetWindow<SceneView>().Repaint();
        }
    }
}