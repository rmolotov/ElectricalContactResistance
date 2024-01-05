using System;
using Newtonsoft.Json;
using UnityEngine;
using ECR.StaticData.Converters;

namespace ECR.StaticData.Board
{
    [Serializable]
    [JsonConverter(typeof(BoardTileConverter))]
    public record BoardTileStaticData
    {
        public BoardTileType Tile { get; set; }
        public BoardTileRotation TileRotation { get; set; }
        public Vector2Int Position { get; set; }
    }
}