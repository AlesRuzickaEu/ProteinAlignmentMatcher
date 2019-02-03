using System.Collections.Generic;

namespace MASReader
{
    public class ProteinMatch
    {
        public bool HasMatch { get; set; }
        public int Distance { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
        public List<int> FundamentalIndexes { get; set; }
        public List<int> DistanceIndexes { get; set; }
    }
}