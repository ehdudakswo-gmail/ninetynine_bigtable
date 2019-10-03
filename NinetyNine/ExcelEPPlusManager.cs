using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace NinetyNine
{
    internal class ExcelEPPlusManager
    {
        private ExcelDataManager excelDataManager = new ExcelDataManager();

        internal Task<DataSet> GetDataSet(string fileName)
        {
            return Task.Run(() =>
            {
                FileInfo fileInfo = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fileInfo);
                ExcelWorksheets worksheets = package.Workbook.Worksheets;
                DataSet dataSet = new DataSet();

                foreach (ExcelWorksheet worksheet in worksheets)
                {
                    DataTable dataTable = excelDataManager.GetDataTable(worksheet);
                    dataSet.Tables.Add(dataTable);
                }

                return dataSet;
            });
        }

        internal Task<string> Save(string fileName, DataSet dataSet)
        {
            return Task.Run(() =>
            {
                FileInfo fileInfo = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fileInfo);
                ExcelWorksheets worksheets = package.Workbook.Worksheets;

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    string tableName = dataTable.TableName;
                    ExcelWorksheet worksheet = worksheets.Add(tableName);
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, false);
                }

                package.Save();
                return fileName;
            });
        }
    }
}