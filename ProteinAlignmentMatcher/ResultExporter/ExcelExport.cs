using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using System.Drawing;

namespace MASReader.ResultExporter
{
    public class ExcelExport : IExport
    {
        private readonly int _fundamentCount;

        public ExcelExport(int fundamentCount)
        {
            _fundamentCount = fundamentCount;
        }

        public void Export(List<ProteinSequenceWithFundaments> fileContent)
        {
            using (var excel = new ExcelPackage())
            {
                for (var i = 1; i <= _fundamentCount; i++)
                {
                    ExcelWorksheet sheet = excel.Workbook.Worksheets.Add($"Fundament {i}");

                    sheet.Column(1).Width = 30;
                    sheet.Column(2).Width = 10;
                    for (var j = 3; j <= 60; j++)
                    {
                        sheet.Column(j).Width = 3;
                    }




                    PrintToSheet(i, ExtractList(fileContent, i), sheet);
                }






                excel.SaveAs(new FileInfo(@"C:\Users\aruzicka\Desktop\export.xlsx"));
            }
        }

        private static List<ProteinSequenceMatchDuo> ExtractList(List<ProteinSequenceWithFundaments> fileContent, int fundamentIndex)
        {
            return fileContent.Select((a) => new ProteinSequenceMatchDuo(a.ProteinSequence, a[fundamentIndex])).Where((a) => a.ProteinMatch != null).ToList();
        }

        private static void PrintToSheet(int index, List<ProteinSequenceMatchDuo> list, ExcelWorksheet sheet)
        {
            int currentCol = 1;
            int currentRow = 1;
            ExcelRange currentCell;

            currentCell = sheet.Cells[currentRow, currentCol, currentRow, currentCol];
            currentCell.Value = list.Count;
            currentCol += 1;
            currentCell = sheet.Cells[currentRow, currentCol, currentRow, currentCol];
            currentCell.Value = "found";
            currentRow += 1;


            foreach (var item in list)
            {
                currentCol = 2;
                currentRow += 1;

                var proteinSequence = item.ProteinSequence;
                var proteinMatch = item.ProteinMatch;

                Write(sheet, currentRow, ref currentCol, proteinMatch.Index + 1 - 5, Color.DarkOrange);
                currentCol += 1;

                //Console.WriteLine("Index: {0}, Name: {1}, Position: [{3}-{4}/{5}], Distance: {2}",
                //    proteinSequence.Index, proteinSequence.ShortName,
                //    proteinMatch.Distance, proteinMatch.Index, proteinMatch.Index + proteinMatch.Length, proteinSequence.Sequence.Length);

                const int howMuch = 5;

                for (var i = -howMuch; i < 0; i++)
                {
                    if (proteinSequence.Sequence.Length <= proteinMatch.Index + i || proteinMatch.Index + i < 0)
                    {
                        currentCol += 1;
                        continue;
                    }
                    var ch = proteinSequence.Sequence[proteinMatch.Index + i];

                    Write(sheet, currentRow, ref currentCol, ch, Color.Gray);

                }

                for (var i = 0; i < proteinMatch.Length; i++)
                {
                    if (proteinSequence.Sequence.Length <= proteinMatch.Index + i || proteinMatch.Index + i < 0)
                    {
                        currentCol += 1;
                        continue;
                    }

                    var ch = proteinSequence.Sequence[proteinMatch.Index + i];
                    if (proteinMatch.DistanceIndexes.Contains(proteinMatch.Index + i))
                    {
                        Write(sheet, currentRow, ref currentCol, ch, Color.Red);
                    }
                    else if (proteinMatch.FundamentalIndexes.Contains(proteinMatch.Index + i))
                    {
                        Write(sheet, currentRow, ref currentCol, ch, Color.Green);
                    }
                    else
                    {
                        Write(sheet, currentRow, ref currentCol, ch, Color.Black);
                    }
                }

                for (var i = 0; i < howMuch; i++)
                {
                    if (proteinSequence.Sequence.Length <= proteinMatch.Index + proteinMatch.Length + i)
                    {
                        currentCol += 1;
                        continue;
                    }

                    var ch = proteinSequence.Sequence[proteinMatch.Index + proteinMatch.Length + i];
                    Write(sheet, currentRow, ref currentCol, ch, Color.Gray);
                }

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

        private static void Write(ExcelWorksheet sheet, int currentRow, ref int currentCol, object value, Color color)
        {
            var currentCell = sheet.Cells[currentRow, currentCol, currentRow, currentCol];
            currentCell.Value = value.ToString();
            if (!color.IsEmpty)
            {
                currentCell.Style.Font.Color.SetColor(color);
            }
            currentCol += 1;
        }

    }
}
