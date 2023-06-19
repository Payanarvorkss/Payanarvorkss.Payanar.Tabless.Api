using Microsoft.AspNetCore.Mvc.RazorPages;
using Payanarvorkss.Payanar.Tabless.Api.DataModelss;

namespace Payanarvorkss.Payanar.Tabless.Api.FileSystemss
{
    public static class PayanarFile
    {
        public static void Save(PayanarTable table)
        {
            string rowValue = string.Empty;
            int rowNum = 0;
            try
            {
                using (BinaryWriter binWriter = new BinaryWriter(File.Open(Path.Combine("C:\\Vbhu\\CS\\Payanarss\\PayanarVorkss\\Payanarvorkss.PayanarTabless.WinForms\\DatabaseFiless", table.Name), FileMode.Append)))
                {
                    foreach (var row in table.Rows)
                    {
                        rowNum++;
                        rowValue = string.Empty;
                        foreach (var cell in row.Cells)
                        {
                            rowValue = string.Format("{0} {1}</>{2}", rowNum, rowValue, cell.Value.Value);
                        }
                        binWriter.Write(rowValue + "\n");
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
