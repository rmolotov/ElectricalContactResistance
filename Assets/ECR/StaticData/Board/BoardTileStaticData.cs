using System;
using ECR.StaticData.Converters;
using Newtonsoft.Json;
using UnityEngine;

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