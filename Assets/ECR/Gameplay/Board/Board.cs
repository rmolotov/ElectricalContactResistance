using System.Collections.Generic;
using System.Linq;
using ECR.Services.Logging;
using ECR.StaticData.Board;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Zenject;

namespace ECR.Gameplay.Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private BoardTile[] tileAssets;
        [SerializeField] private NavMeshSurface navigationSurface;
        
        // TODO: get Tiles (BoardTile) from config and addressables?
        private Dictionary<BoardTileType, BoardTile> _boardTiles;
        private ILoggingService _logger;

        [Inject]
        private void Construct(ILoggingService loggingService) =>
            _logger = loggingService;
        
        public void InitializeAndBake(IEnumerable<BoardTileStaticData> data)
        {
            _boardTiles ??= tileAssets.ToDictionary(t => t.tileType, t => t);

            foreach (var boardTileStaticData in data)
                SetTile(
                    _boardTiles[boardTileStaticData.Tile],
                    boardTileStaticData.Position,
                    boardTileStaticData.TileRotation);

            navigationSurface.BuildNavMesh();
            
            _logger.LogMessage($"initialized with {_boardTiles.Count} tiles and baked {navigationSurface.size} navmesh", this);
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
