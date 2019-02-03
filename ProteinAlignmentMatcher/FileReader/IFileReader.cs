using System.Collections.Generic;

namespace MASReader.FileReader
{
    public interface IFileReader
    {
        List<ProteinSequence> Read(string fileName);
    }
}