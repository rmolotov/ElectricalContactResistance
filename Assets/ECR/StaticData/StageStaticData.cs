using System;
using ECR.StaticData.Board;
using ECR.StaticData.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace ECR.StaticData
{
    [Serializable]
    public record StageStaticData
    {
        public string StageKey { get; set; }
        public string StageTitle{ get; set; }
        public string StageDescription{ get; set; }

        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 PlayerSpawnPoint { get; set; }

        public EnemySpawnerStaticData[] EnemySpawners { get; set; }
        
        public BoardTileStaticData[] BoardTiles { get; set; }
    }
}