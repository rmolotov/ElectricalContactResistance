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
        private Transform _playerSpawner;
        
        private string _output;
        private Vector2 _scroll;

        #region Editor window lifecycle

        [MenuItem("Tools/ECR/Board editor")]
        private static void ShowWindow() =>
            GetWindow<BoardEditor>("Board editor").Show();

        protected override void OnEnable()
        {
            base.OnEnable();
            EditorSceneManager.activeSceneChangedInEditMode += ResetWindow;
            ResetWindow();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EditorSceneManager.activeSceneChangedInEditMode -= ResetWindow;
            ResetWindow();
        }

        private void OnInspectorUpdate()
        {
            if (SceneManager.GetActiveScene().name is not "StageEditor")
                return;
            
            playerSpawnPoint = _playerSpawner.position;
            enemySpawners.ForEach(sp => sp.position = sp.gameObject.transform.position);
            Repaint();
        }

        #endregion


        [HideIf(EditorSceneNameCondition)]
        [Button("Open editor scene")]
        public void OpenEditorScene() =>
            EditorSceneManager.OpenScene(EditorScenePath);


        [ShowIfGroup("Stage properties", Condition = EditorSceneNameCondition)]
        [PropertyOrder(-2)]
        [SerializeField] private string stageKey, stageTitle;


        [ShowIfGroup("Stage properties", Condition = EditorSceneNameCondition)]
        [TextArea(5, 5)]
        [SerializeField] private string stageDescription;


        [ShowIfGroup("Player spawn", Condition = EditorSceneNameCondition)]
        [BoxGroup("Player spawn/Player spawn point")]
        [HorizontalGroup("Player spawn/Player spawn point/Box", Width = 42)]
        [PropertyOrder(-1)]
        [Button(SdfIconType.ArrowsMove, "", Expanded = false)]
        public void SelectPlayerSpawner() => 
            Selection.SetActiveObjectWithContext(_playerSpawner, null);


        [ShowIfGroup("Player spawn", Condition = EditorSceneNameCondition)]
        [BoxGroup("Player spawn/Player spawn point")]
        [HorizontalGroup("Player spawn/Player spawn point/Box")]
        [HideLabel] [ReadOnly]
        [SerializeField] private Vector3 playerSpawnPoint;


        [PropertySpace]
        
        
        [ShowIfGroup("Enemy spawners", Condition = EditorSceneNameCondition)]
        [TableList(AlwaysExpanded = true, NumberOfItemsPerPage = 5, ShowPaging = true, DefaultMinColumnWidth = 20)]
        [OnCollectionChanged("OnRemoveMarker", "OnAddMarker")]
        [SerializeField] private List<EnemySpawnerEditor> enemySpawners = new();


        [ShowIf(EditorSceneNameCondition + " && !UnityEngine.Application.isPlaying")]
        [Button("Generate stage JSON", ButtonSizes.Large), GUIColor(0, 1, 0)]
        public void GenerateStage()
        {
            var staticData = GenerateStageStaticData();
            GenerateOutput(staticData);
        }


        [ShowIf(EditorSceneNameCondition + " && !string.IsNullOrEmpty(_output)")]
        [Header("Generated stage static data")] 
        [HideLabel] [TextArea(10, 10)]
        [SerializeField] private string jsonOutput;


        private StageStaticData GenerateStageStaticData()
        {
            var staticData = new StageStaticData();
            var board = SaveBoard();

            staticData.StageKey = stageKey;
            staticData.StageTitle = stageTitle;
            staticData.StageDescription = stageDescription;

            staticData.PlayerSpawnPoint = _playerSpawner.position;
            staticData.BoardTiles = board;

            staticData.EnemySpawners = enemySpawners
                .Select(x => new EnemySpawnerStaticData
                {
                    EnemyType = x.enemyType,
                    Position = x.gameObject.transform.position
                })
                .ToArray();

            return staticData;
        }

        private void ResetWindow(Scene previous, Scene current) => 
            ResetWindow();

        private void ResetWindow()
        {
            if (SceneManager.GetActiveScene().name is not "StageEditor")
                return;
            
            _tilemap = FindObjectOfType<Tilemap>();
            _playerSpawner = GameObject.FindGameObjectWithTag("Player").transform;
            enemySpawners.ForEach(sp => DestroyImmediate(sp.gameObject));
            enemySpawners.Clear();
            
            stageKey = stageTitle = stageDescription = string.Empty;
            _output = string.Empty;
            jsonOutput = _output;

            Repaint();
        }

        private BoardTileStaticData[] SaveBoard()
        {
            var result = new List<BoardTileStaticData>();

            foreach (var pos in _tilemap.cellBounds.allPositionsWithin)
            {
                var tile = _tilemap.GetTile<BoardTile>(pos);
                if (tile == null) continue;

                var staticData = new BoardTileStaticData
                {
                    Position = new Vector2Int(pos.x, pos.y),
                    Tile = tile.tileType,
                    TileRotation = (BoardTileRotation) (int) _tilemap.GetTransformMatrix(pos).rotation.eulerAngles.z
                };
                result.Add(staticData);
            }

            return result.ToArray();
        }

        private void GenerateOutput<T>(T obj)
        {
            _output = JsonConvert.SerializeObject(obj, Formatting.Indented);
            jsonOutput = _output;
        }

        private void UpdatePlayerSpawner() =>
            playerSpawnPoint = _playerSpawner.position;

        #region SpawnerListHelpers

        private void OnAddMarker(CollectionChangeInfo info, object value)
        {
            if (info.ChangeType is not CollectionChangeType.Add)
                return;

            var data = (EnemySpawnerEditor) info.Value;
            
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(EnemySpawnMarkerPrefabPath);
            var marker = Instantiate(prefab);
            data.gameObject = marker;
            data.gameObject.name = $"Spawner : {data.enemyType}";
        }

        private void OnRemoveMarker(CollectionChangeInfo info, object value)
        {
            if (info.ChangeType is not CollectionChangeType.RemoveIndex)
                return;

            DestroyImmediate(((List<EnemySpawnerEditor>) value)[info.Index].gameObject);
        }

        #endregion
    }
}