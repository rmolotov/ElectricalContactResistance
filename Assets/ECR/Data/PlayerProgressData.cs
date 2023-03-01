using System;
using System.Collections.Generic;

namespace ECR.Data
{
    [Serializable]
    public class PlayerProgressData
    {
        public HashSet<string> CompletedStages { get; set; }
    }
}