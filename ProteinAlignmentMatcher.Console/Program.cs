using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MASReader.FileReader;
using MASReader.Fundaments;
using MASReader.ResultExporter;
using Mono.Options;

namespace MASReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = null;
            string fileType = "FASTA";
            int maximalDistance = 10;
            string exporter = "EXCEL";
            IFileReader fileReader;
            IExport export;

            var fundamentPatterns = new List<string>();

            var p = new OptionSet() {
               { "f|file=",              a => fileName = a },
               { "t|fileType=",          a => fileType = a },
               { "e|exporter=",          a => exporter = a },
               { "d|maximalDistance=",   (int a) => maximalDistance = a },
               { "p|pattern=", a => {
                   if(!string.IsNullOrEmpty(a))
                     fundamentPatterns.Add(a);
                   }
               },
            };

            p.Parse(args); //List<string> extra = 

            if (fundamentPatterns.Count == 0)
                throw new ArgumentNullException(nameof(fundamentPatterns), "No paterns defined");

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found", fileName);

            if (string.Equals(fileType, "FASTA", StringComparison.OrdinalIgnoreCase))
                fileReader = new FastaFileReader();
            else
                throw new ArgumentOutOfRangeException(nameof(fileType), "Unknown file type");

            var fileContent = fileReader.Read(fileName).Select((a) => new ProteinSequenceWithFundaments(a)).ToList();

            var matcher = new FundamentsMatcher();

            foreach (var fundamentPattern in fundamentPatterns.Select((a, i) => Tuple.Create(a, i + 1)))
            {
                matcher.FindInSequences(fileContent, fundamentPattern.Item2, fundamentPattern.Item1, maximalDistance);
            }

            if (string.Equals(exporter, "CONSOLE", StringComparison.OrdinalIgnoreCase))
                export = new ConsoleExport(fundamentPatterns.Count);
            else if (string.Equals(exporter, "EXCEL", StringComparison.OrdinalIgnoreCase))
                export = new ExcelExport(fundamentPatterns.Count);
            else
                throw new ArgumentOutOfRangeException(nameof(fileType), "Unknown exporter type");

            export.Export(fileContent);
        }
    }
}
