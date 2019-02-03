using System.Collections.Generic;

namespace MASReader.ResultExporter
{
    public interface IExport
    {
        void Export(List<ProteinSequenceWithFundaments> fileContent);
    }
}