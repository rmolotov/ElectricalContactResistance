using System.Collections.Generic;
using UnityEngine;
using ECR.Gameplay.Logic;

namespace ECR.Data
{
    public class StageProgressData
    {
        public GameObject Hero { get; set; }
        public List<EnemySpawner> EnemySpawners { get; } = new();
    }
}