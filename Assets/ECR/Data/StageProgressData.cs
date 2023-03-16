using System.Collections.Generic;
using ECR.Gameplay.Logic;
using UnityEngine;

namespace ECR.Data
{
    public class StageProgressData
    {
        public GameObject Hero { get; set; }
        public List<EnemySpawner> EnemySpawners { get; } = new();
    }
}