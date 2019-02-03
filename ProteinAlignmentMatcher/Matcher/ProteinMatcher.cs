using System.Collections.Generic;
using System.Text;

namespace MASReader
{
    public static class ProteinMatcher
    {

        public static ProteinMatch Match(string genomeSequence, int startPosition, string pattern, int maximumDistance)
        {
            var distance = 0;
            var position = startPosition;
            var distances = new List<int>();
            var fundamentals = new List<int>();
            var number = new StringBuilder(); ;

            foreach (var patternChar in pattern)
            {
                if (char.IsNumber(patternChar))
                {
                    number.Append(patternChar);
                    continue;
                }
                if (number.Length > 0)
                {
                    position += int.Parse(number.ToString());
                    number.Clear();
                }

                if (position >= genomeSequence.Length)
                {
                    return new ProteinMatch();
                }

                if (patternChar != genomeSequence[position])
                {
                    distances.Add(position);
                    distance++;
                    position++;

                    if (distance > maximumDistance)
                    {
                        return new ProteinMatch();
                    }
                }
                else
                {
                    fundamentals.Add(position);
                    position++;
                }
            }

            var result = new ProteinMatch()
            {
                HasMatch = true,
                Distance = distance,
                Index = startPosition,
                Length = position - startPosition,
                DistanceIndexes = distances,
                FundamentalIndexes = fundamentals,
            };
            return result;
        }
    }
}