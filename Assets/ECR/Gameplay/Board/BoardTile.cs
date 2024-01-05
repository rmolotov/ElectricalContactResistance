using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using ECR.StaticData.Board;

namespace ECR.Gameplay.Board
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Board Tile", menuName = "2D/Tiles/Board Tile")]
    public class BoardTile : Tile
    {
        [EnumToggleButtons]
        public BoardTileType tileType;
    }
}