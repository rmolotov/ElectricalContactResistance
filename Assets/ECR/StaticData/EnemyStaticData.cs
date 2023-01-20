using System;

namespace ECR.StaticData
{
    [Serializable]
    public record EnemyStaticData
    {
        public EnemyType EnemyType { get; set; }
        public int Capacity { get; set; }
        public int Current { get; set; }
        public int Voltage { get; set; }
        public int Resistance { get; set; }
    }
}