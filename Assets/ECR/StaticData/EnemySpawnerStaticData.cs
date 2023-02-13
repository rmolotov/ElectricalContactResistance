using System;
using ECR.StaticData.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace ECR.StaticData
{
    [Serializable]
    public record EnemySpawnerStaticData
    {
        public EnemyType EnemyType { get; set; }
        
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 Position { get; set; }
    }
}