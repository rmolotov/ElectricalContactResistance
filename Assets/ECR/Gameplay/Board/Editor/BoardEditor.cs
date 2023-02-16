using System;
using System.Collections.Generic;
using System.Linq;
using ECR.StaticData;
using ECR.StaticData.Board;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace ECR.Gameplay.Board.Editor
{
    public class BoardEditor : OdinEditorWindow
    {
        private const string EditorSceneNameCondition = "@SceneManager.GetActiveScene().name == \"StageEditor\"";
        private const string EditorScenePath = "Assets/ECR/Gameplay/Board/Editor/StageEditor.unity";
        private const string EnemySpawnMarkerPrefabPath = "Assets/ECR/Gameplay/Logic/EnemySpawnerMarker.prefab";

        private Tilemap _tilemap;
        private string _output;
        private Vector2 _scroll;

        #region Editor window lifecicle

        [MenuItem("Tools/ECR/Board editor")]
        private static void ShowWindow() =>
            GetWindow<BoardEditor>("Board editor").Show();

        protected override void OnEnable()
        {
            base.OnEnable();
            EditorSceneManager.activeSceneChangedInEditMode += ResetWindow;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EditorSceneManager.activeSceneChangedInEditMode -= ResetWindow;
        }

        #endregion


        [HideIf(EditorSceneNameCondition)]
        [Button("Open editor scene")]
        public void OpenEditorScene() =>
            EditorSceneManager.OpenScene(EditorScenePath);


        [ShowIfGroup("Stage properties", Condition = EditorSceneNameCondition)] [SerializeField]
        private string stageKey, stageTitle;

        [ShowIfGroup("Stage properties", Condition = EditorSceneNameCondition)] [MultiLineProperty] [SerializeField]
        private string stageDescription;


        [ShowIfGroup("Player spawn", Condition = EditorSceneNameCondition)] [SerializeField]
        private Vector3 playerSpawnPoint;

        [PropertySpace]
        [ShowIfGroup("Enemy spawners", Condition = EditorSceneNameCondition)]
        [TableList(AlwaysExpanded = true, NumberOfItemsPerPage = 5, ShowPaging = true)]
        [OnCollectionChanged("OnRemoveMarker", "OnAddMarker")]
        [SerializeField]
        private List<EnemySpawnerEditorStruct> enemySpawners;

        [ShowIf(EditorSceneNameCondition + " && !UnityEngine.Application.isPlaying")]
        [Button("Generate stage JSON", ButtonSizes.Large), GUIColor(0, 1, 0)]
        public void GenerateStage()
        {
            var staticData = GenerateStageStaticData();
            GenerateOutput(staticData);
        }

        [ShowIf(EditorSceneNameCondition + " && !string.IsNullOrEmpty(_output)")]
        [TextArea(10, 10)]
        [HideLabel]
        [Header("Generated stage static data")]
        public string JsonOutput;


        private StageStaticData GenerateStageStaticData()
        {
            var staticData = new StageStaticData();
            var board = SaveBoard();

            staticData.StageKey = stageKey;
            staticData.StageTitle = stageTitle;
            staticData.StageDescription = stageDescription;

            staticData.PlayerSpawnPoint = playerSpawnPoint;
            staticData.BoardTiles = board;

            staticData.EnemySpawners = enemySpawners.Select(x => new EnemySpawnerStaticData
            {
                EnemyType = x.enemyType,
                Position = x.gameObject.transform.position
            }).ToArray();

            return staticData;
        }

        private void ResetWindow(Scene previous, Scene current)
        {
            stageKey = stageTitle = stageDescription = string.Empty;
            _tilemap = null;
            _output = string.Empty;
            enemySpawners.Clear();

            JsonOutput = _output;
            Repaint();
        }

        private BoardTileStaticData[] SaveBoard()
        {
            _tilemap = FindObjectOfType<Tilemap>();

            var result = new List<BoardTileStaticData>();

            foreach (var pos in _tilemap.cellBounds.allPositionsWithin)
            {
                var tile = _tilemap.GetTile<BoardTile>(pos);
                if (tile == null) continue;

                var staticData = new BoardTileStaticData
                {
                    Position = new Vector2Int(pos.x, pos.y),
                    Tile = tile.tileType,
                    TileRotation = (BoardTileRotation) (int) tile.transform.rotation.z
                };
                result.Add(staticData);
            }

            return result.ToArray();
        }

        private void GenerateOutput<T>(T obj)
        {
            _output = JsonConvert.SerializeObject(obj, Formatting.Indented);
            JsonOutput = _output;
        }

        #region SpawnerListHelpers

        private void OnAddMarker(CollectionChangeInfo info, object value)
        {
            if (info.ChangeType is not CollectionChangeType.Add)
                return;

            var data = (EnemySpawnerEditorStruct) info.Value;
            
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(EnemySpawnMarkerPrefabPath);
            var marker = Instantiate(prefab);
            data.gameObject = marker;
            data.gameObject.name = $"Spawner : {data.enemyType}";
        }

        private void OnRemoveMarker(CollectionChangeInfo info, object value)
        {
            if (info.ChangeType is not CollectionChangeType.RemoveIndex)
                return;

            var marker = ((List<EnemySpawnerEditorStruct>) value)[info.Index].gameObject;
            DestroyImmediate(marker);
        }        

        #endregion
    }

    [Serializable]
    public class EnemySpawnerEditorStruct
    {
        [HideInTables]
        public GameObject gameObject;
        
        [EnumToggleButtons] [OnValueChanged("ChangeType")]
        public EnemyType enemyType = EnemyType.Capacitor;

        [ResponsiveButtonGroup("Select")] [Button(SdfIconType.ArrowsMove, "", Expanded = false)] [PropertyOrder(-1)]
        public void Select() => 
            Selection.SetActiveObjectWithContext(gameObject, null);

        private void ChangeType()
        {
            gameObject.name = $"Spawner : {enemyType}";
            EditorWindow.GetWindow<SceneView>().Repaint();
        }
    }
}