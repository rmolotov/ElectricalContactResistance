using System;
using Newtonsoft.Json;
using UnityEngine;
using ECR.StaticData.Converters;

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