using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MASReader.FileReader
{
    public class FastaFileReader : IFileReader
    {
        public List<ProteinSequence> Read(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            
            var fileContent = File.ReadAllLines(fileName, Encoding.ASCII);
            var allSequenceRdRps = new List<ProteinSequence>();

            for (int i = 0; i < fileContent.Length; i += 2)
            {
                var name = fileContent[i].TrimStart('>');
                var shortName = name;
                if (name.IndexOf(')') > -1)
                {
                    shortName = shortName.Substring(0, name.IndexOf(')') + 1);
                }
                allSequenceRdRps.Add(new ProteinSequence()
                {
                    Index = i / 2 + 1,
                    Name = name,
                    ShortName = shortName,
                    Sequence = fileContent[i + 1].Replace("-", "").ToUpperInvariant(),
                });
            }
            return allSequenceRdRps;
        }
    }
}
