using System.Collections.Generic;
using System.Linq;
using ECR.StaticData.Board;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ECR.Gameplay.Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private BoardTile[] tileAssets;
        
        // TODO: get Tiles (BoardTile) from config and addressables?
        private Dictionary<BoardTileType, BoardTile> _boardTiles;
        
        public void InitBoard(IEnumerable<BoardTileStaticData> data)
        {
            _boardTiles ??= tileAssets.ToDictionary(t => t.tileType, t => t);

            foreach (var boardTileStaticData in data)
            {
                var tile = _boardTiles[boardTileStaticData.Tile];
                SetTile(tile, boardTileStaticData.Position, boardTileStaticData.TileRotation);
            }
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
