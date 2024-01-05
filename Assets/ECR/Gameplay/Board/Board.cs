using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using Zenject;
using ECR.StaticData.Board;
using ECR.Services.Logging;

namespace ECR.Gameplay.Board
{
    public class Board : SerializedMonoBehaviour
    {
        [DictionaryDrawerSettings(KeyLabel = "Type", ValueLabel = "Tile")]
        [SerializeField] private readonly Dictionary<BoardTileType, BoardTile> tileAssets = new();
        [PropertySpace]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private NavMeshSurface navigationSurface;

        private ILoggingService _logger;

        [Inject]
        private void Construct(ILoggingService loggingService) =>
            _logger = loggingService;
        
        public void InitializeAndBake(BoardTileStaticData[] data)
        {
            foreach (var boardTileStaticData in data)
                SetTile(
                    tileAssets[boardTileStaticData.Tile],
                    boardTileStaticData.Position,
                    boardTileStaticData.TileRotation);

            navigationSurface.BuildNavMesh();
            
            _logger.LogMessage($"initialized with {data.Length} tiles from {tileAssets.Count} assets and baked {navigationSurface.size} navmesh", this);
        }

        private void SetTile(TileBase tile, Vector2Int position, BoardTileRotation rotation)
        {
            var pos = new Vector3Int(position.x, position.y, 0);
            var rot = Quaternion.Euler(0,0, (int)rotation);
            
            tilemap.SetTile(pos, tile);
            tilemap.SetTransformMatrix(pos, Matrix4x4.TRS(Vector3.zero, rot, Vector3.one));
        }
    }
}
