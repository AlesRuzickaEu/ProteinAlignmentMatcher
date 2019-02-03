using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MASReader.Fundaments
{
    public class FundamentsMatcher
    {
        public List<ProteinMatch> FindInSequences(List<ProteinSequenceWithFundaments> sequence, int fundamentIndex, string expression, int maximumDistance)
        {
            return sequence.SelectMany((a) => FindInSequence(a, fundamentIndex, expression, maximumDistance)).ToList();
        }

        public List<ProteinMatch> FindInSequence(ProteinSequenceWithFundaments sequence, int fundamentIndex, string expression, int maximumDistance)
        {
            var result = new List<ProteinMatch>();
            var minimalDistance = maximumDistance;
            var startFrom = 0;
            //if (fundamentIndex > 1)
            //{
            //    var lastFundament = sequence[fundamentIndex - 1];
            //    if (lastFundament != null)
            //    {
            //        startFrom = lastFundament.Index + lastFundament.Length;
            //    }
            //}


            for (var i = startFrom; i < sequence.ProteinSequence.Sequence.Length; i++)
            {
                var match = ProteinMatcher.Match(sequence.ProteinSequence.Sequence, i, expression, maximumDistance);
                if (!match.HasMatch || match.Distance >= minimalDistance)
                {
                    continue;
                }

                if (match.Distance < minimalDistance)
                {
                    result.Clear();
                    minimalDistance = match.Distance;
                }

                if (match.Distance == minimalDistance)
                {
                    sequence[fundamentIndex] = match;
                }
            }
            return result;
        }
    }
}
