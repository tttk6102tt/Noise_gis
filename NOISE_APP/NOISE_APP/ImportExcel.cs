using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOISE_APP
{
    public partial class ImportExcel : Form
    {
        private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public ImportExcel()
        {
            InitializeComponent();
        }

        private void ImportExcel_Load(object sender, EventArgs e)
        {
          
           
        }
        private object SetValueFromExcel(object valueOfExcel)
        {
            if (valueOfExcel != null && valueOfExcel.ToString().ToUpper() != "NULL" && !string.IsNullOrEmpty(valueOfExcel?.ToString()))
            {
                return valueOfExcel.ToString();
            }
            else
            {
                return null;
            }
        }
        private bool Create(string query)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = string.Format(@"INSERT INTO [dbo].[NOISE]
                                                       ([ID]
                                                       ,[TIME]
                                                       ,[LAT]
                                                       ,[LONG]
                                                       ,[dB]
                                                       ,[NAME]
                                                       ,[VEHICLE]
                                                       ,[DENSITY]
                                                       ,[LOCATION2])
                                                         VALUES
                                                               ({0})", query);
                        cmd.ExecuteNonQuery();

                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) ||
                string.IsNullOrEmpty(txtdB.Text) ||
                string.IsNullOrEmpty(txtLAT.Text) ||
                string.IsNullOrEmpty(txtLOCATION.Text) ||
                 string.IsNullOrEmpty(txtLONG.Text) ||
                 string.IsNullOrEmpty(txtTIME.Text))
            {
                _NoiseMesageBox.ShowErrorMessage("Nhập đầy đủ vị trí các cột dùm!");
                return;
            }
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(open.FileName)))
                {
                    var workbook = package.Workbook;
                    var worksheets = workbook.Worksheets;

                    var colID = int.Parse(txtID.Text);
                    var colLAT = int.Parse(txtLAT.Text);
                    var colLONG = int.Parse(txtLONG.Text);
                    var colTIME = int.Parse(txtTIME.Text);
                    var colLOCATION = int.Parse(txtLOCATION.Text);
                    var coldB = int.Parse(txtdB.Text);

                    foreach (var worksheet in worksheets)
                    {
                        int totalRows = worksheet.Dimension.End.Row;
                        int totalCols = worksheet.Dimension.End.Column;

                        string ID, TIME, LAT, LONG, dB, LOCATION;

                        for (int i = 3; i <= totalRows; i++)
                        {
                            try
                            {
                                ID = SetValueFromExcel(worksheet.Cells[i, colID].Value).ToString().Replace(",", ".");
                                TIME = SetValueFromExcel(worksheet.Cells[i, colTIME].Value).ToString().Replace(",", ".");
                                LAT = SetValueFromExcel(worksheet.Cells[i, colLAT].Value).ToString().Replace(",", ".");
                                LONG = SetValueFromExcel(worksheet.Cells[i, colLONG].Value).ToString().Replace(",", ".");
                                dB = SetValueFromExcel(worksheet.Cells[i, coldB].Value).ToString().Replace(",",".");
                                LOCATION = SetValueFromExcel(worksheet.Cells[i, colLOCATION].Value).ToString();
                                Save("Lỗi:", string.Format("'{0}','{1}','{2}','{3}','{4}','','','',N'{5}'", ID, TIME, LAT, LONG, dB, LOCATION), "");
                                if (ID == "")
                                {
                                    break;
                                }


                                string query = string.Format("'{0}','{1}','{2}','{3}','{4}','','','',N'{5}'", ID, TIME, LAT, LONG, dB, LOCATION);
                               
                                //Create(query);
                            }
                            catch (Exception ex)
                            {
                                Save("Lỗi:", ex.Message, "");
                                throw;
                            }
                            
                        }
                    break;
                }
            }
                _NoiseMesageBox.ShowErrorMessage("xong file " + open.FileName);
            }
        }

        public void Save(string content, string exceptionMessage, string stackTrace)
        {
            string path = Application.StartupPath + @"\\checkLog.txt";
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("*** Create by DoanhLV ***");
                        sw.WriteLine("--- Cuộn xuống dưới để thấy được dữ liệu mới nhất -------------------------------------------------");
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("Content: " + content);
                    sw.WriteLine("ExceptionMessage: " + exceptionMessage);
                    sw.WriteLine("ExceptionStackTrace: " + stackTrace);
                    sw.WriteLine("CreateTime: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"));
                    sw.WriteLine("-------------------------------------------------------------------------------------------------------");
                    sw.WriteLine("");
                }
            }
            catch
            {
            }
        }
    }
}
