using System;
using System.Collections.Generic;
using System.Linq;

namespace MASReader.ResultExporter
{
    public class ConsoleExport : IExport
    {
        private readonly int _fundamentCount;

        public ConsoleExport(int fundamentCount)
        {
            _fundamentCount = fundamentCount;
        }

        public void Export(List<ProteinSequenceWithFundaments> fileContent)
        {
            for (var i = 1; i <= _fundamentCount; i++)
            {
                PrintToConsole(i, ExtractList(fileContent, i));
            }
        }

        private static List<ProteinSequenceMatchDuo> ExtractList(List<ProteinSequenceWithFundaments> fileContent, int fundamentIndex)
        {
            return fileContent.Select((a) => new ProteinSequenceMatchDuo(a.ProteinSequence, a[fundamentIndex])).Where((a) => a.ProteinMatch != null).ToList();
        }

        private static void PrintToConsole(int index, List<ProteinSequenceMatchDuo> list)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("{0} Fundament, {1} found", index, list.Count);
            Console.WriteLine();
            Console.ResetColor();

            foreach (var item in list)
            {
                var proteinSequence = item.ProteinSequence;
                var proteinMatch = item.ProteinMatch;

                Console.WriteLine("Index: {0}, Name: {1}, Position: [{3}-{4}/{5}], Distance: {2}",
                    proteinSequence.Index, proteinSequence.ShortName,
                    proteinMatch.Distance, proteinMatch.Index, proteinMatch.Index + proteinMatch.Length, proteinSequence.Sequence.Length);
                const int howMuch = 5;

                Console.ForegroundColor = ConsoleColor.DarkGray;
                for (var i = -howMuch; i < 0; i++)
                {
                    if (proteinSequence.Sequence.Length <= proteinMatch.Index + i || proteinMatch.Index + i < 0)
                    {
                        break;
                    }
                    var ch = proteinSequence.Sequence[proteinMatch.Index + i];
                    Console.Write(ch);
                }

                Console.ForegroundColor = ConsoleColor.White;
                for (var i = 0; i < proteinMatch.Length; i++)
                {
                    if (proteinSequence.Sequence.Length <= proteinMatch.Index + i || proteinMatch.Index + i < 0)
                    {
                        break;
                    }

                    var ch = proteinSequence.Sequence[proteinMatch.Index + i];
                    if (proteinMatch.DistanceIndexes.Contains(proteinMatch.Index + i))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(ch);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (proteinMatch.FundamentalIndexes.Contains(proteinMatch.Index + i))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(ch);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(ch);
                    }
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                for (var i = 1; i <= howMuch; i++)
                {
                    if (proteinSequence.Sequence.Length <= proteinMatch.Index + i || proteinMatch.Index + i < 0)
                    {
                        break;
                    }

                    var ch = proteinSequence.Sequence[proteinMatch.Index + i];
                    Console.Write(ch);
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.ResetColor();
            }
        }

        private class ProteinSequenceMatchDuo
        {
            public ProteinSequence ProteinSequence { get; }
            public ProteinMatch ProteinMatch { get; }

            public ProteinSequenceMatchDuo(ProteinSequence proteinSequence, ProteinMatch proteinMatch)
            {
                ProteinSequence = proteinSequence;
                ProteinMatch = proteinMatch;
            }

        }
    }
}
