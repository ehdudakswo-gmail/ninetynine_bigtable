using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace NinetyNine
{
    internal class ExcelEPPlusManager
    {
        internal Task<DataTable> GetDataTable(string fileName)
        {
            return Task.Run(() =>
            {
                FileInfo fileInfo = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fileInfo);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                DataTable dataTable = new DataTable();

                SetColumnHeaderName(dataTable, worksheet);
                SetDataTable(dataTable, worksheet);

                return dataTable;
            });
        }

        private void SetColumnHeaderName(DataTable dataTable, ExcelWorksheet worksheet)
        {
            const int OFFSET = 26;
            int columnCount = worksheet.Dimension.End.Column;
            string[] columns = new string[columnCount];

            for (int i = 0; i < columns.Length; i++)
            {
                int roopCnt = (i / OFFSET);
                int seq = (i % OFFSET);
                string name = ((char)('A' + seq)).ToString();

                if (roopCnt == 0)
                {
                    columns[i] = name;
                }
                else
                {
                    string front = ((char)('A' + roopCnt - 1)).ToString();
                    columns[i] = front + name;
                }
            }

            for (int i = 0; i < columnCount; i++)
            {
                string name = columns[i];
                dataTable.Columns.Add(name);
            }
        }

        private void SetDataTable(DataTable dataTable, ExcelWorksheet worksheet)
        {
            int rowCount = worksheet.Dimension.End.Row;
            int colCount = worksheet.Dimension.End.Column;

            for (int row = 1; row <= rowCount; row++)
            {
                DataRow newRow = dataTable.NewRow();
                for (int col = 1; col <= colCount; col++)
                {
                    var excelValue = worksheet.Cells[row, col].Value;
                    if (excelValue == null)
                    {
                        continue;
                    }

                    int idx = col - 1;
                    newRow[idx] = excelValue.ToString();
                }

                dataTable.Rows.Add(newRow);
            }
        }

        internal Task<string> Save(string fileName, DataTable dataTable)
        {
            return Task.Run(() =>
            {
                FileInfo fileInfo = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fileInfo);
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(fileName);

                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                package.Save();

                return fileName;
            });
        }
    }
}