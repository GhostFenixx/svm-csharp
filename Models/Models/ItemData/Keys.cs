using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greed.Models.ItemData
{
   public class Keys
    {
        public bool EnableKeys { get; set; }
        public bool AvoidSingleKeyCards { get; set; }
        public bool IgnoreAccessCard { get; set; }
        public bool AvoidSingleKeys { get; set; }
        public bool AvoidMarkedKeys { get; set; }
        public bool AvoidResidential { get; set; }
        public bool AvoidOddKeys { get; set; }
        public double KeyUseMult { get; set; } = 1;
        public double KeycardUseMult { get; set; } = 1;
        public int KeyDurabilityThreshold { get; set; } = 40;
        public int KeyCardDurabilityThreshold { get; set; } = 10;
        public bool InfiniteKeys { get; set; }
        public bool InfiniteKeycards { get; set; }
    }
}
