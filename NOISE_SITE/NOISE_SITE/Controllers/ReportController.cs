using Microsoft.Ajax.Utilities;
using NOISE_SITE.Enums;
using NOISE_SITE.Helpers;
using NOISE_SITE.Models;
using NOISE_SITE.Repository;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOISE_SITE.Controllers
{
    public class ReportController : Controller
    {
        private string _ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        private IDMTramDoRepository _dMTramDoRepository;
        // GET: Report
        public ReportController(DMTramDoRepository dMTramDoRepository)
        {
            _dMTramDoRepository = dMTramDoRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetReportMin(string dateReport)
        {
            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
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
                        var today = DateTime.Today;
                        var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));
                        cmd.Connection = conn;
                        var query = "";
                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format(@"select id, 
                                                        LOCATION as DiaDiem,
                                                        SUBSTRING(TIME,9,2) as hour2,
                                                        SUBSTRING(TIME,7,2) as day2,
                                                        SUBSTRING(TIME,5,2) as month2,
                                                        SUBSTRING(TIME,1,4)as year2,
                                                        sum(cast(dB as float)) as Total,
                                                        count(ID) as soluong,
                                                        sum(cast(dB as float)) /count(ID) as trungbinh,
                                                        min(db) as min2,
                                                        max(db) as max2
                                                        from noise n
                                                        --inner join DMTramDo d on d.MaTramDo = n.ID
                                                        where TIME LIKE '{0}%' 
                                                        group by id,
                                                        SUBSTRING(TIME,9,2),
                                                        SUBSTRING(TIME,7,2),
                                                        LOCATION,
                                                        SUBSTRING(TIME,5,2),
                                                        SUBSTRING(TIME,1,4)
                                                        ", dateReport); ;//order by id

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);
                        var dataReports = new List<DataReport>();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                double soluong = 0.0;
                                double.TryParse(row["soluong"].ToString(), out soluong);
                                double total = 0.0;
                                double.TryParse(row["Total"].ToString(), out total);
                                double minVal = 0.0;
                                double.TryParse(row["min2"].ToString(), out minVal);
                                double maxVal = 0.0;
                                double.TryParse(row["max2"].ToString(), out maxVal);
                                double average = 0.0;
                                double.TryParse(row["trungbinh"].ToString(), out average);

                                dataReports.Add(new DataReport()
                                {
                                    day = row["day2"].ToString(),
                                    month = row["month2"].ToString(),
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    hour = row["hour2"].ToString(),
                                    MaTramDo = row["ID"].ToString(),
                                    soluong = soluong,
                                    total = Math.Round(total, 2),
                                    year = row["year2"].ToString(),
                                    min = Math.Round(minVal, 2),
                                    max = Math.Round(maxVal, 2),
                                    average = Math.Round(average, 2)
                                });
                            }
                        }
                        dataReports = dataReports.OrderByDescending(s => s.MaTramDo).ToList();
                        var file = CreateReportMin(dataReports, dateReport.Substring(6, 2), dateReport.Substring(4, 2), dateReport.Substring(0, 4));

                        return File(file, "application/octet-stream", "BaoCaoTiengOn.xlsx");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }

                return File(new byte[0], "application/octet-stream");
                //return File(fullPath, "application/octet-stream", fileName);
            }
        }
        public ActionResult GetReportForMonthMin(string dateReport)
        {
            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
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
                        var today = DateTime.Today;
                        var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format(@"select id, 
                                                        LOCATION as DiaDiem,
                                                        SUBSTRING(TIME,9,2) as hour2,
                                                        SUBSTRING(TIME,5,2) as month2,
                                                        SUBSTRING(TIME,1,4)as year2,
                                                        sum(cast(dB as float)) as Total,
                                                        count(ID) as soluong,
                                                        sum(cast(dB as float)) /count(ID) as trungbinh,
                                                        min(db) as min2,
                                                        max(db) as max2
                                                        from noise n
                                                        --inner join DMTramDo d on d.MaTramDo = n.ID
                                                        where TIME LIKE '{0}%' 
                                                        group by id,
                                                        SUBSTRING(TIME,9,2),
                                                        LOCATION,
                                                        SUBSTRING(TIME,5,2),
                                                        SUBSTRING(TIME,1,4)
                                                        ", dateReport); //order by id

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);
                        var dataReports = new List<DataReport>();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                double soluong = 0.0;
                                double.TryParse(row["soluong"].ToString(), out soluong);
                                double total = 0.0;
                                double.TryParse(row["Total"].ToString(), out total);
                                double minVal = 0.0;
                                double.TryParse(row["min2"].ToString(), out minVal);
                                double maxVal = 0.0;
                                double.TryParse(row["max2"].ToString(), out maxVal);
                                double average = 0.0;
                                double.TryParse(row["trungbinh"].ToString(), out average);

                                dataReports.Add(new DataReport()
                                {
                                    month = row["month2"].ToString(),
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    hour = row["hour2"].ToString(),
                                    MaTramDo = row["ID"].ToString(),
                                    soluong = soluong,
                                    total = Math.Round(total, 2),
                                    year = row["year2"].ToString(),
                                    min = Math.Round(minVal, 2),
                                    max = Math.Round(maxVal, 2),
                                    average = Math.Round(average, 2)
                                });
                            }

                        }
                        dataReports = dataReports.OrderByDescending(s => s.MaTramDo).ToList();
                        var file = CreateReportMonthMin(dataReports, dateReport.Substring(4, 2), dateReport.Substring(0, 4));

                        return File(file, "application/octet-stream", "BaoCaoTiengOn.xlsx");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return File(new byte[0], "application/octet-stream");
            }
        }
        public ActionResult GetReportForYearMin(string dateReport)
        {
            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
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
                        var today = DateTime.Today;
                        var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format(@"select id, 
                                                        LOCATION as DiaDiem,
                                                        SUBSTRING(TIME,9,2) as hour2,
                                                        SUBSTRING(TIME,1,4)as year2,
                                                        sum(cast(dB as float)) as Total,
                                                        count(ID) as soluong,
                                                        sum(cast(dB as float)) /count(ID) as trungbinh,
                                                        min(db) as min2,
                                                        max(db) as max2
                                                        from noise n
                                                       -- inner join DMTramDo d on d.MaTramDo = n.ID
                                                        where TIME LIKE '{0}%' 
                                                        group by id,
                                                        SUBSTRING(TIME,9,2),
                                                        LOCATION,
                                                        SUBSTRING(TIME,1,4)
                                                        ", dateReport);//order by id

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);
                        var dataReports = new List<DataReport>();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                double soluong = 0.0;
                                double.TryParse(row["soluong"].ToString(), out soluong);
                                double total = 0.0;
                                double.TryParse(row["Total"].ToString(), out total);
                                double minVal = 0.0;
                                double.TryParse(row["min2"].ToString(), out minVal);
                                double maxVal = 0.0;
                                double.TryParse(row["max2"].ToString(), out maxVal);
                                double average = 0.0;
                                double.TryParse(row["trungbinh"].ToString(), out average);

                                dataReports.Add(new DataReport()
                                {
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    hour = row["hour2"].ToString(),
                                    MaTramDo = row["ID"].ToString(),
                                    soluong = soluong,
                                    total = Math.Round(total, 2),
                                    year = row["year2"].ToString(),
                                    min = Math.Round(minVal, 2),
                                    max = Math.Round(maxVal, 2),
                                    average = Math.Round(average, 2)
                                });
                            }

                        }
                        dataReports = dataReports.OrderByDescending(s => s.MaTramDo).ToList();
                        var file = CreateReportYearMin(dataReports, dateReport);

                        return File(file, "application/octet-stream", "BaoCaoTiengOn.xlsx");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return File(new byte[0], "application/octet-stream");
            }
        }
        public ActionResult GetReport(string dateReport)
        {
            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
            using (var conn = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var year = Convert.ToInt32(dateReport.Substring(0, 4));
                    var month = Convert.ToInt32(dateReport.Substring(4, 2));
                    var day = Convert.ToInt32(dateReport.Substring(6, 2));

                    using (var cmd = new SqlCommand())
                    {


                        var today = new DateTime(year, month, day);

                        var startDay = today.AddHours(-7);
                        var endDay = today.AddDays(1).AddHours(-7);

                        var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));
                        cmd.Connection = conn;
                        var query = "";
                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format(@"select id, 
                                                        LOCATION as DiaDiem,
                                                        SUBSTRING(TIME,9,2) as hour2,
                                                        SUBSTRING(TIME,7,2) as day2,
                                                        SUBSTRING(TIME,5,2) as month2,
                                                        SUBSTRING(TIME,1,4)as year2,
                                                        sum(cast(dB as float)) as Total,
                                                        count(ID) as soluong,
                                                        sum(cast(dB as float)) /count(ID) as trungbinh,
                                                        min(db) as min2,
                                                        max(db) as max2
                                                        from noise n
                                                        --inner join DMTramDo d on d.MaTramDo = n.ID
                                                        where TIME LIKE '{0}%' 
                                                        group by id,
                                                        SUBSTRING(TIME,9,2),
                                                        SUBSTRING(TIME,7,2),
                                                        LOCATION,
                                                        SUBSTRING(TIME,5,2),
                                                        SUBSTRING(TIME,1,4)
                                                        ", dateReport); ;//order by id


                        //cmd.CommandText = string.Format(@"			   
                        //                                select id, 
                        //                                DiaDiem,
                        //                                SUBSTRING(TIME,9,2) as hour2,
                        //                                SUBSTRING(TIME,7,2) as day2,
                        //                                SUBSTRING(TIME,5,2) as month2,
                        //                                SUBSTRING(TIME,1,4)as year2,
                        //                                from noise n
                        //                                inner join DMTramDo d on d.MaTramDo = n.ID
                        //                                where TIME > '{0}' and TIME < '{1}'
                        //                               ", startDay.ToString("yyyyMMddHHmmss"), endDay.ToString("yyyyMMddHHmmss"));

                        //sum(cast(dB as float)) as Total,
                        //                                count(ID) as soluong,
                        //                                sum(cast(dB as float)) / count(ID) as trungbinh,
                        //                                min(db) as min2,
                        //                                max(db) as max2

                        //group by id,
                        //                                SUBSTRING(TIME, 9, 2),
                        //                                SUBSTRING(TIME, 7, 2),
                        //                                DiaDiem,
                        //                                SUBSTRING(TIME, 5, 2),
                        //                                SUBSTRING(TIME, 1, 4)

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);
                        var dataReports = new List<DataReport>();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                double soluong = 0.0;
                                double.TryParse(row["soluong"].ToString(), out soluong);
                                double total = 0.0;
                                double.TryParse(row["Total"].ToString(), out total);
                                double minVal = 0.0;
                                double.TryParse(row["min2"].ToString(), out minVal);
                                double maxVal = 0.0;
                                double.TryParse(row["max2"].ToString(), out maxVal);
                                double average = 0.0;
                                double.TryParse(row["trungbinh"].ToString(), out average);

                                var datetime = new DateTime(int.Parse(row["year2"].ToString()), int.Parse(row["month2"].ToString()), int.Parse(row["day2"].ToString())).AddHours(int.Parse(row["hour2"].ToString())).AddHours(7);
                                

                                dataReports.Add(new DataReport()
                                {
                                    day = datetime.Day.ToString(),
                                    month = datetime.Month.ToString(),
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    hour = datetime.Hour.ToString(),
                                    MaTramDo = row["ID"].ToString(),
                                    soluong = soluong,
                                    total = Math.Round(total, 2),
                                    year = datetime.Year.ToString(),
                                    min = Math.Round(minVal, 2),
                                    max = Math.Round(maxVal, 2),
                                    average = Math.Round(average, 2)
                                });
                            }
                        }


                        dataReports = dataReports.OrderBy(s => s.MaTramDo).ToList();

                        var file = CreateReport(dataReports, dateReport.Substring(6, 2), dateReport.Substring(4, 2), dateReport.Substring(0, 4));
                        //20210730
                        return File(file, "application/octet-stream", "BaoCaoTiengOn.xlsx");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }

                return File(new byte[0], "application/octet-stream");
                //return File(fullPath, "application/octet-stream", fileName);
            }
        }
        public ActionResult GetReportForMonth(string dateReport)
        {
            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
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
                        var today = DateTime.Today;
                        var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));
                        cmd.Connection = conn;

                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format(@"select id, 
                                                        LOCATION AS DiaDiem,
                                                        SUBSTRING(TIME,9,2) as hour2,
                                                        SUBSTRING(TIME,5,2) as month2,
                                                        SUBSTRING(TIME,1,4)as year2,
                                                        sum(cast(dB as float)) as Total,
                                                        count(ID) as soluong,
                                                        sum(cast(dB as float)) /count(ID) as trungbinh,
                                                        min(db) as min2,
                                                        max(db) as max2
                                                        from noise n
                                                        --inner join DMTramDo d on d.MaTramDo = n.ID
                                                        where TIME LIKE '{0}%' 
                                                        group by id,
                                                        SUBSTRING(TIME,9,2),
                                                        LOCATION,
                                                        SUBSTRING(TIME,5,2),
                                                        SUBSTRING(TIME,1,4)
                                                        ", dateReport); ;//order by id

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);
                        var dataReports = new List<DataReport>();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                double soluong = 0.0;
                                double.TryParse(row["soluong"].ToString(), out soluong);
                                double total = 0.0;
                                double.TryParse(row["Total"].ToString(), out total);
                                double minVal = 0.0;
                                double.TryParse(row["min2"].ToString(), out minVal);
                                double maxVal = 0.0;
                                double.TryParse(row["max2"].ToString(), out maxVal);
                                double average = 0.0;
                                double.TryParse(row["trungbinh"].ToString(), out average);

                                dataReports.Add(new DataReport()
                                {
                                    month = row["month2"].ToString(),
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    hour = row["hour2"].ToString(),
                                    MaTramDo = row["ID"].ToString(),
                                    soluong = soluong,
                                    total = Math.Round(total, 2),
                                    year = row["year2"].ToString(),
                                    min = Math.Round(minVal, 2),
                                    max = Math.Round(maxVal, 2),
                                    average = Math.Round(average, 2)
                                });
                            }

                        }

                        dataReports = dataReports.OrderByDescending(s => s.MaTramDo).ToList();

                        var file = CreateReportForMonth(dataReports, dateReport.Substring(4, 2), dateReport.Substring(0, 4));

                        return File(file, "application/octet-stream", "BaoCaoTiengOn.xlsx");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return File(new byte[0], "application/octet-stream");
            }
        }

        public ActionResult GetReport_2(string startTime, string endTime)
        {
            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
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
                        var today = DateTime.Today;
                        var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));
                        cmd.Connection = conn;

                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format(@"select id, 
                                                        LOCATION AS DiaDiem,
                                                        SUBSTRING(TIME,9,2) as hour2,
                                                        SUBSTRING(TIME,5,2) as month2,
                                                        SUBSTRING(TIME,1,4)as year2,
                                                        sum(cast(dB as float)) as Total,
                                                        count(ID) as soluong,
                                                        sum(cast(dB as float)) /count(ID) as trungbinh,
                                                        min(db) as min2,
                                                        max(db) as max2
                                                        from noise n
                                                        --inner join DMTramDo d on d.MaTramDo = n.ID
                                                        where TIME > '{0}' and TIME < '{1}' 
                                                        group by id,
                                                        SUBSTRING(TIME,9,2),
                                                        LOCATION,
                                                        SUBSTRING(TIME,5,2),
                                                        SUBSTRING(TIME,1,4)
                                                        ", startTime,endTime); ;//order by id

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);
                        var dataReports = new List<DataReport>();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                double soluong = 0.0;
                                double.TryParse(row["soluong"].ToString(), out soluong);
                                double total = 0.0;
                                double.TryParse(row["Total"].ToString(), out total);
                                double minVal = 0.0;
                                double.TryParse(row["min2"].ToString(), out minVal);
                                double maxVal = 0.0;
                                double.TryParse(row["max2"].ToString(), out maxVal);
                                double average = 0.0;
                                double.TryParse(row["trungbinh"].ToString(), out average);

                                int hour = 0;
                                int.TryParse(row["hour2"].ToString(), out hour);
                            
                                int month = 0;
                                int.TryParse(row["month2"].ToString(), out month);
                                int year = 0;
                                int.TryParse(row["year2"].ToString(), out year);

                                int day = 0;
                                var dayold = new DateTime(year, month, 1).AddDays(-1).AddHours(17);
                                //int.TryParse(row["trungbinh"].ToString(), out day);

                                var date = new DateTime(year,month,day);

                                var dateCur = dayold.AddHours(hour);

                                dataReports.Add(new DataReport()
                                {
                                    month = dateCur.Month.ToString(),
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    hour = dateCur.Hour.ToString(),
                                    MaTramDo = row["ID"].ToString(),
                                    soluong = soluong,
                                    total = Math.Round(total, 2),
                                    year = dateCur.Year.ToString(),
                                    min = Math.Round(minVal, 2),
                                    max = Math.Round(maxVal, 2),
                                    average = Math.Round(average, 2)
                                });
                            }

                        }

                        dataReports = dataReports.OrderByDescending(s => s.MaTramDo).ToList();

                        //var file = CreateReportForMonth(dataReports, dateReport.Substring(4, 2), dateReport.Substring(0, 4));
                        var file = CreateReportForMonth(dataReports, endTime.Substring(4, 2), endTime.Substring(0, 4));

                        return File(file, "application/octet-stream", "BaoCaoTiengOn.xlsx");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return File(new byte[0], "application/octet-stream");
            }
        }

        public ActionResult GetReportForYear(string dateReport)
        {
            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
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
                        var today = DateTime.Today;
                        var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format(@"select id, 
                                                        LOCATION AS DiaDiem,
                                                        SUBSTRING(TIME,9,2) as hour2,
                                                        SUBSTRING(TIME,1,4)as year2,
                                                        sum(cast(dB as float)) as Total,
                                                        count(ID) as soluong,
                                                        sum(cast(dB as float)) /count(ID) as trungbinh,
                                                        min(db) as min2,
                                                        max(db) as max2
                                                        from noise n
                                                        --inner join DMTramDo d on d.MaTramDo = n.ID
                                                        where TIME LIKE '{0}%' 
                                                        group by id,
                                                        SUBSTRING(TIME,9,2),
                                                        LOCATION,
                                                        SUBSTRING(TIME,1,4)
                                                        ", dateReport);//order by id

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);
                        var dataReports = new List<DataReport>();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                double soluong = 0.0;
                                double.TryParse(row["soluong"].ToString(), out soluong);
                                double total = 0.0;
                                double.TryParse(row["Total"].ToString(), out total);
                                double minVal = 0.0;
                                double.TryParse(row["min2"].ToString(), out minVal);
                                double maxVal = 0.0;
                                double.TryParse(row["max2"].ToString(), out maxVal);
                                double average = 0.0;
                                double.TryParse(row["trungbinh"].ToString(), out average);

                                dataReports.Add(new DataReport()
                                {
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    hour = row["hour2"].ToString(),
                                    MaTramDo = row["ID"].ToString(),
                                    soluong = soluong,
                                    total = Math.Round(total, 2),
                                    year = row["year2"].ToString(),
                                    min = Math.Round(minVal, 2),
                                    max = Math.Round(maxVal, 2),
                                    average = Math.Round(average, 2)
                                });
                            }


                        }
                        dataReports = dataReports.OrderByDescending(s => s.MaTramDo).ToList();
                        var file = CreateReportForYear(dataReports, dateReport);

                        return File(file, "application/octet-stream", "BaoCaoTiengOn.xlsx");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                //return File(new byte[0], "application/octet-stream");
            }
        }
        public class DataReport
        {
            public string MaTramDo { get; set; }
            public string DiaDiem { get; set; }
            public string hour { get; set; }
            public string month { get; set; }
            public string day { get; set; }
            public string year { get; set; }
            public double total { get; set; }
            public double soluong { get; set; }
            public double average { get; set; }
            public double min { get; set; }
            public double max { get; set; }
        }
        private MemoryStream CreateReportMin(List<DataReport> datas, string day, string month, string year)
        {
            var stt = 0;


            var maTrams = _dMTramDoRepository.FindAll().ToList();

            using (ExcelPackage package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("report");
                ExcelRange cell;


                workSheet.DefaultColWidth = 15;

                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells[1, 1].Value = string.Format("THỐNG KÊ MỨC ĐỘ TIẾNG ỒN CỰC ĐAI, CỰC TIỂU TẠI CÁC KHUNG THỜI GIAN NGÀY {0} THÁNG {1} NĂM {2}", day, month, year);

                cell = workSheet.Cells[1, 1];
                cell.Style.Font.Size = 18;
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD);

                string cellMerge0 = workSheet.Cells[1, 1] + ":" + workSheet.Cells[1, 51];
                ExcelRange rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[2, 48].Value = "Đơn vị tính: dBA";

                cellMerge0 = workSheet.Cells[2, 48] + ":" + workSheet.Cells[2, 51];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 1].Value = "STT";
                cell = workSheet.Cells[3, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 1] + ":" + workSheet.Cells[4, 1];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 2].Value = "Mã trạm đo";
                cell = workSheet.Cells[3, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 2] + ":" + workSheet.Cells[4, 2];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 3].Value = "Địa điểm";
                cell = workSheet.Cells[3, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 3] + ":" + workSheet.Cells[4, 3];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 4].Value = "0h-1h";
                cell = workSheet.Cells[3, 4];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 5];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 4] + ":" + workSheet.Cells[3, 5];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 6].Value = "1h-2h";
                cell = workSheet.Cells[3, 6];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 7];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 6] + ":" + workSheet.Cells[3, 7];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 8].Value = "2h-3h";
                cell = workSheet.Cells[3, 8];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 9];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 8] + ":" + workSheet.Cells[3, 9];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 10].Value = "3h-4h";
                cell = workSheet.Cells[3, 10];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 11];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 10] + ":" + workSheet.Cells[3, 11];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 12].Value = "4h-5h";
                cell = workSheet.Cells[3, 12];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 13];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 12] + ":" + workSheet.Cells[3, 13];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 14].Value = "5h-6h";
                cell = workSheet.Cells[3, 14];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 15];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 14] + ":" + workSheet.Cells[3, 15];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 16].Value = "6h-7h";
                cell = workSheet.Cells[3, 16];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 17];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 16] + ":" + workSheet.Cells[3, 17];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 18].Value = "7h-8h";
                cell = workSheet.Cells[3, 18];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 19];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 18] + ":" + workSheet.Cells[3, 19];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 20].Value = "8h-9h";
                cell = workSheet.Cells[3, 20];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 21];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 20] + ":" + workSheet.Cells[3, 21];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 22].Value = "9h-10h";
                cell = workSheet.Cells[3, 22];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 23];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 22] + ":" + workSheet.Cells[3, 23];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 24].Value = "10h-11h";
                cell = workSheet.Cells[3, 24];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 25];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 24] + ":" + workSheet.Cells[3, 25];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 26].Value = "11h-12h";
                cell = workSheet.Cells[3, 26];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 27];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 26] + ":" + workSheet.Cells[3, 27];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;


                workSheet.Cells[3, 28].Value = "12h-13h";
                cell = workSheet.Cells[3, 28];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 29];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 28] + ":" + workSheet.Cells[3, 29];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 30].Value = "13h-14h";
                cell = workSheet.Cells[3, 30];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 31];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 30] + ":" + workSheet.Cells[3, 31];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 32].Value = "14-15h";
                cell = workSheet.Cells[3, 32];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 33];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 32] + ":" + workSheet.Cells[3, 33];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;


                workSheet.Cells[3, 34].Value = "15h-16h";
                cell = workSheet.Cells[3, 34];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 35];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 34] + ":" + workSheet.Cells[3, 35];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 36].Value = "16h-17h";
                cell = workSheet.Cells[3, 36];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 37];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 36] + ":" + workSheet.Cells[3, 37];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 38].Value = "17h-18h";
                cell = workSheet.Cells[3, 38];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 39];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 38] + ":" + workSheet.Cells[3, 39];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 40].Value = "18h-19h";
                cell = workSheet.Cells[3, 40];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 41];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 40] + ":" + workSheet.Cells[3, 41];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 42].Value = "19h-20h";
                cell = workSheet.Cells[3, 42];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 43];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 42] + ":" + workSheet.Cells[3, 43];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 44].Value = "20h-21h";
                cell = workSheet.Cells[3, 44];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 45];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 44] + ":" + workSheet.Cells[3, 45];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 46].Value = "21h-22h";
                cell = workSheet.Cells[3, 46];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 47];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 46] + ":" + workSheet.Cells[3, 47];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 48].Value = "22h-23h";
                cell = workSheet.Cells[3, 48];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 49];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 48] + ":" + workSheet.Cells[3, 49];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 50].Value = "23h-24h";
                cell = workSheet.Cells[3, 50];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 51];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 50] + ":" + workSheet.Cells[3, 51];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                for (int i = 4; i <= 51; i++)
                {
                    if ((i % 2) == 1)
                    {
                        workSheet.Cells[4, i].Value = "Nhỏ nhất";
                        cell = workSheet.Cells[4, i];
                        OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                    }
                    else
                    {
                        workSheet.Cells[4, i].Value = "Lớn nhất";
                        cell = workSheet.Cells[4, i];
                        OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                    }
                }

                var rowIdx = 5;

                foreach (var station in maTrams.ToList())
                {
                    var data = datas.Where(s => s.MaTramDo == station.MaTramDo).ToList();
                    var colIdx = 1;
                    workSheet.Cells[rowIdx, colIdx].Value = ++stt;
                    cell = workSheet.Cells[rowIdx, 1];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 2].Value = station.MaTramDo;
                    cell = workSheet.Cells[rowIdx, 2];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 3].Value = station.DiaDiem;
                    cell = workSheet.Cells[rowIdx, 3];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    //00
                    workSheet.Cells[rowIdx, 4].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 4];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 5].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 5];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //01
                    workSheet.Cells[rowIdx, 6].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 6];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 7].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 7];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //02
                    workSheet.Cells[rowIdx, 8].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 8];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 9].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 9];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //03
                    workSheet.Cells[rowIdx, 10].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 10];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 11].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 11];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //04
                    workSheet.Cells[rowIdx, 12].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 12];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 13].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 13];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //05
                    workSheet.Cells[rowIdx, 14].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 14];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 15].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 15];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //06
                    workSheet.Cells[rowIdx, 16].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 16];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 17].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 17];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //07
                    workSheet.Cells[rowIdx, 18].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 18];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 19].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 19];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //08
                    workSheet.Cells[rowIdx, 20].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 20];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 21].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 21];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //09
                    workSheet.Cells[rowIdx, 22].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 22];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 23].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 23];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //10
                    workSheet.Cells[rowIdx, 24].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 24];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 25].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 25];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //11
                    workSheet.Cells[rowIdx, 26].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 26];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 27].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 27];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //12
                    workSheet.Cells[rowIdx, 28].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 28];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 29].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 29];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //13
                    workSheet.Cells[rowIdx, 30].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 30];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 31].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 31];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //14
                    workSheet.Cells[rowIdx, 32].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 32];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 33].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 33];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //15
                    workSheet.Cells[rowIdx, 34].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 34];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 35].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 35];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //16
                    workSheet.Cells[rowIdx, 36].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 36];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 37].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 37];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //17
                    workSheet.Cells[rowIdx, 38].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 38];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 39].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 39];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //18
                    workSheet.Cells[rowIdx, 40].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 40];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 41].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 41];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //19
                    workSheet.Cells[rowIdx, 42].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 42];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 43].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 43];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //20
                    workSheet.Cells[rowIdx, 44].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 44];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 45].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 45];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //21
                    workSheet.Cells[rowIdx, 46].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 46];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 47].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 47];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //22
                    workSheet.Cells[rowIdx, 48].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 48];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 49].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 49];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //23
                    workSheet.Cells[rowIdx, 50].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 50];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 51].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 51];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    rowIdx++;
                }

                package.Save();
                return new MemoryStream(package.GetAsByteArray());
            }
        }
        private MemoryStream CreateReport(List<DataReport> datas, string day, string month, string year)
        {
            var stt = 0;


            var maTrams = _dMTramDoRepository.FindAll().ToList();

            using (ExcelPackage package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("report");
                ExcelRange cell;


                workSheet.DefaultColWidth = 15;

                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells[1, 1].Value = string.Format("THỐNG KÊ MỨC ĐỘ TIẾNG ỒN TRUNG BÌNH TẠI CÁC KHUNG THỜI GIAN NGÀY {0} THÁNG {1} NĂM {2}", day, month, year);

                cell = workSheet.Cells[1, 1];
                cell.Style.Font.Size = 18;
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER);

                string cellMerge0 = workSheet.Cells[1, 1] + ":" + workSheet.Cells[1, 27];
                ExcelRange rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[2, 24].Value = "Đơn vị tính: dBA";

                cellMerge0 = workSheet.Cells[2, 24] + ":" + workSheet.Cells[2, 27];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 1].Value = "STT";
                cell = workSheet.Cells[3, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 2].Value = "Mã trạm đo";
                cell = workSheet.Cells[3, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 3].Value = "Địa điểm";
                cell = workSheet.Cells[3, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 4].Value = "0h-1h";
                cell = workSheet.Cells[3, 4];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 5].Value = "1h-2h";
                cell = workSheet.Cells[3, 5];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 6].Value = "2h-3h";
                cell = workSheet.Cells[3, 6];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 7].Value = "3h-4h";
                cell = workSheet.Cells[3, 7];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 8].Value = "4h-5h";
                cell = workSheet.Cells[3, 8];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 9].Value = "5h-6h";
                cell = workSheet.Cells[3, 9];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 10].Value = "6h-7h";
                cell = workSheet.Cells[3, 10];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 11].Value = "7h-8h";
                cell = workSheet.Cells[3, 11];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 12].Value = "8h-9h";
                cell = workSheet.Cells[3, 12];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 13].Value = "9h-10h";
                cell = workSheet.Cells[3, 13];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 14].Value = "10h-11h";
                cell = workSheet.Cells[3, 14];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 15].Value = "11h-12h";
                cell = workSheet.Cells[3, 15];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 16].Value = "12h-13h";
                cell = workSheet.Cells[3, 16];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 17].Value = "13h-14h";
                cell = workSheet.Cells[3, 17];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 18].Value = "14-15h";
                cell = workSheet.Cells[3, 18];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 19].Value = "15h-16h";
                cell = workSheet.Cells[3, 19];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 20].Value = "16h-17h";
                cell = workSheet.Cells[3, 20];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 21].Value = "17h-18h";
                cell = workSheet.Cells[3, 21];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 22].Value = "18h-19h";
                cell = workSheet.Cells[3, 22];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 23].Value = "19h-20h";
                cell = workSheet.Cells[3, 23];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 24].Value = "20h-21h";
                cell = workSheet.Cells[3, 24];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 25].Value = "21h-22h";
                cell = workSheet.Cells[3, 25];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 26].Value = "22h-23h";
                cell = workSheet.Cells[3, 26];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 27].Value = "23h-24h";
                cell = workSheet.Cells[3, 27];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                var rowIdx = 4;
                foreach (var station in maTrams.ToList())
                {
                    var data = datas.Where(s => s.MaTramDo == station.MaTramDo).ToList();
                    var colIdx = 1;
                    workSheet.Cells[rowIdx, colIdx].Value = ++stt;
                    cell = workSheet.Cells[rowIdx, 1];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 2].Value = station.MaTramDo;
                    cell = workSheet.Cells[rowIdx, 2];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 3].Value = station.DiaDiem;
                    cell = workSheet.Cells[rowIdx, 3];

                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 4].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 4];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 5].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 5];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 6].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 6];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 7].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 7];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 8].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 8];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 9].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 9];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 10].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 10];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 11].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 11];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 12].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 12];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 13].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 13];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 14].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 14];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 15].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 15];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 16].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 16];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 17].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 17];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 18].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 18];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 19].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 19];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 20].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 20];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 21].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 21];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 22].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 22];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 23].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 23];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 24].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 24];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 25].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 25];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 26].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 26];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 27].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 27];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    rowIdx++;
                }
                package.Save();
                return new MemoryStream(package.GetAsByteArray());
            }
        }
        private MemoryStream CreateReportMonthMin(List<DataReport> datas, string month, string year)
        {
            var stt = 0;


            var maTrams = _dMTramDoRepository.FindAll().ToList();

            using (ExcelPackage package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("report");
                ExcelRange cell;


                workSheet.DefaultColWidth = 15;
                #region Header

                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells[1, 1].Value = string.Format("THỐNG KÊ MỨC ĐỘ TIẾNG ỒN CỰC ĐAI, CỰC TIỂU TẠI CÁC KHUNG THỜI GIAN THÁNG {0} NĂM {1}", month, year);

                cell = workSheet.Cells[1, 1];
                cell.Style.Font.Size = 18;
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD);

                string cellMerge0 = workSheet.Cells[1, 1] + ":" + workSheet.Cells[1, 27];
                ExcelRange rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[2, 48].Value = "Đơn vị tính: dBA";

                cellMerge0 = workSheet.Cells[2, 48] + ":" + workSheet.Cells[2, 51];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 1].Value = "STT";
                cell = workSheet.Cells[3, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 1] + ":" + workSheet.Cells[4, 1];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 2].Value = "Mã trạm đo";
                cell = workSheet.Cells[3, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 2] + ":" + workSheet.Cells[4, 2];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 3].Value = "Địa điểm";
                cell = workSheet.Cells[3, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 3] + ":" + workSheet.Cells[4, 3];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 4].Value = "0h-1h";
                cell = workSheet.Cells[3, 4];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 5];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 4] + ":" + workSheet.Cells[3, 5];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 6].Value = "1h-2h";
                cell = workSheet.Cells[3, 6];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 7];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 6] + ":" + workSheet.Cells[3, 7];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 8].Value = "2h-3h";
                cell = workSheet.Cells[3, 8];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 9];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 8] + ":" + workSheet.Cells[3, 9];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 10].Value = "3h-4h";
                cell = workSheet.Cells[3, 10];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 11];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 10] + ":" + workSheet.Cells[3, 11];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 12].Value = "4h-5h";
                cell = workSheet.Cells[3, 12];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 13];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 12] + ":" + workSheet.Cells[3, 13];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 14].Value = "5h-6h";
                cell = workSheet.Cells[3, 14];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 15];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 14] + ":" + workSheet.Cells[3, 15];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 16].Value = "6h-7h";
                cell = workSheet.Cells[3, 16];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 17];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 16] + ":" + workSheet.Cells[3, 17];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 18].Value = "7h-8h";
                cell = workSheet.Cells[3, 18];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 19];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 18] + ":" + workSheet.Cells[3, 19];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 20].Value = "8h-9h";
                cell = workSheet.Cells[3, 20];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 21];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 20] + ":" + workSheet.Cells[3, 21];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 22].Value = "9h-10h";
                cell = workSheet.Cells[3, 22];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 23];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 22] + ":" + workSheet.Cells[3, 23];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 24].Value = "10h-11h";
                cell = workSheet.Cells[3, 24];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 25];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 24] + ":" + workSheet.Cells[3, 25];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 26].Value = "11h-12h";
                cell = workSheet.Cells[3, 26];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 27];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 26] + ":" + workSheet.Cells[3, 27];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;


                workSheet.Cells[3, 28].Value = "12h-13h";
                cell = workSheet.Cells[3, 28];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 29];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 28] + ":" + workSheet.Cells[3, 29];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 30].Value = "13h-14h";
                cell = workSheet.Cells[3, 30];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 31];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 30] + ":" + workSheet.Cells[3, 31];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 32].Value = "14-15h";
                cell = workSheet.Cells[3, 32];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 33];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 32] + ":" + workSheet.Cells[3, 33];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;


                workSheet.Cells[3, 34].Value = "15h-16h";
                cell = workSheet.Cells[3, 34];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 35];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 34] + ":" + workSheet.Cells[3, 35];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 36].Value = "16h-17h";
                cell = workSheet.Cells[3, 36];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 37];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 36] + ":" + workSheet.Cells[3, 37];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 38].Value = "17h-18h";
                cell = workSheet.Cells[3, 38];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 39];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 38] + ":" + workSheet.Cells[3, 39];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 40].Value = "18h-19h";
                cell = workSheet.Cells[3, 40];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 41];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 40] + ":" + workSheet.Cells[3, 41];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 42].Value = "19h-20h";
                cell = workSheet.Cells[3, 42];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 43];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 42] + ":" + workSheet.Cells[3, 43];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 44].Value = "20h-21h";
                cell = workSheet.Cells[3, 44];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 45];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 44] + ":" + workSheet.Cells[3, 45];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 46].Value = "21h-22h";
                cell = workSheet.Cells[3, 46];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 47];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 46] + ":" + workSheet.Cells[3, 47];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 48].Value = "22h-23h";
                cell = workSheet.Cells[3, 48];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 49];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 48] + ":" + workSheet.Cells[3, 49];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 50].Value = "23h-24h";
                cell = workSheet.Cells[3, 50];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 51];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 50] + ":" + workSheet.Cells[3, 51];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                for (int i = 4; i <= 51; i++)
                {
                    if ((i % 2) == 1)
                    {
                        workSheet.Cells[4, i].Value = "Nhỏ nhất";
                        cell = workSheet.Cells[4, i];
                        OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                    }
                    else
                    {
                        workSheet.Cells[4, i].Value = "Lớn nhất";
                        cell = workSheet.Cells[4, i];
                        OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                    }
                }
                #endregion

                var rowIdx = 5;

                foreach (var station in maTrams.ToList())
                {
                    var data = datas.Where(s => s.MaTramDo == station.MaTramDo).ToList();
                    var colIdx = 1;
                    workSheet.Cells[rowIdx, colIdx].Value = ++stt;
                    cell = workSheet.Cells[rowIdx, 1];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 2].Value = station.MaTramDo;
                    cell = workSheet.Cells[rowIdx, 2];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 3].Value = station.DiaDiem;
                    cell = workSheet.Cells[rowIdx, 3];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    //00
                    workSheet.Cells[rowIdx, 4].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 4];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 5].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 5];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //01
                    workSheet.Cells[rowIdx, 6].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 6];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 7].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 7];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //02
                    workSheet.Cells[rowIdx, 8].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 8];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 9].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 9];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //03
                    workSheet.Cells[rowIdx, 10].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 10];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 11].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 11];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //04
                    workSheet.Cells[rowIdx, 12].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 12];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 13].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 13];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //05
                    workSheet.Cells[rowIdx, 14].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 14];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 15].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 15];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //06
                    workSheet.Cells[rowIdx, 16].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 16];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 17].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 17];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //07
                    workSheet.Cells[rowIdx, 18].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 18];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 19].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 19];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //08
                    workSheet.Cells[rowIdx, 20].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 20];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 21].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 21];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //09
                    workSheet.Cells[rowIdx, 22].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 22];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 23].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 23];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //10
                    workSheet.Cells[rowIdx, 24].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 24];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 25].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 25];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //11
                    workSheet.Cells[rowIdx, 26].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 26];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 27].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 27];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //12
                    workSheet.Cells[rowIdx, 28].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 28];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 29].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 29];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //13
                    workSheet.Cells[rowIdx, 30].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 30];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 31].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 31];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //14
                    workSheet.Cells[rowIdx, 32].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 32];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 33].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 33];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //15
                    workSheet.Cells[rowIdx, 34].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 34];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 35].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 35];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //16
                    workSheet.Cells[rowIdx, 36].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 36];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 37].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 37];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //17
                    workSheet.Cells[rowIdx, 38].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 38];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 39].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 39];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //18
                    workSheet.Cells[rowIdx, 40].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 40];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 41].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 41];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //19
                    workSheet.Cells[rowIdx, 42].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 42];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 43].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 43];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //20
                    workSheet.Cells[rowIdx, 44].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 44];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 45].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 45];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //21
                    workSheet.Cells[rowIdx, 46].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 46];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 47].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 47];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //22
                    workSheet.Cells[rowIdx, 48].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 48];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 49].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 49];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //23
                    workSheet.Cells[rowIdx, 50].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 50];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 51].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 51];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    rowIdx++;
                }

                //ExcelLineChart lineChart = workSheet.Drawings.AddChart("lineChart", eChartType.Line) as ExcelLineChart;

                //lineChart.Title.Text = "LineChart Example";

                //var rangeLabel = workSheet.Cells["B1:K1"];
                //var range1 = workSheet.Cells["B2:K2"];
                //var range2 = workSheet.Cells["B3:K3"];

                //lineChart.Series.Add(range1, rangeLabel);
                //lineChart.Series.Add(range2, rangeLabel);

                //lineChart.Series[0].Header = workSheet.Cells["A2"].Value.ToString();
                //lineChart.Series[1].Header = workSheet.Cells["A3"].Value.ToString();

                //lineChart.Legend.Position = eLegendPosition.Right;

                //lineChart.SetSize(600, 300);
                //lineChart.SetPosition(5, 0, 1, 0);

                package.Save();
                return new MemoryStream(package.GetAsByteArray());
            }
        }
        private MemoryStream CreateReportYearMin(List<DataReport> datas, string year)
        {
            var stt = 0;


            var maTrams = _dMTramDoRepository.FindAll().ToList();

            using (ExcelPackage package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("report");
                ExcelRange cell;


                workSheet.DefaultColWidth = 15;

                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells[1, 1].Value = string.Format("THỐNG KÊ MỨC ĐỘ TIẾNG ỒN CỰC ĐAI, CỰC TIỂU TẠI CÁC KHUNG THỜI GIAN NĂM {0}", year);

                cell = workSheet.Cells[1, 1];
                cell.Style.Font.Size = 18;
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD);

                string cellMerge0 = workSheet.Cells[1, 1] + ":" + workSheet.Cells[1, 27];
                ExcelRange rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[2, 48].Value = "Đơn vị tính: dBA";

                cellMerge0 = workSheet.Cells[2, 48] + ":" + workSheet.Cells[2, 51];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 1].Value = "STT";
                cell = workSheet.Cells[3, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 1] + ":" + workSheet.Cells[4, 1];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 2].Value = "Mã trạm đo";
                cell = workSheet.Cells[3, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 2] + ":" + workSheet.Cells[4, 2];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 3].Value = "Địa điểm";
                cell = workSheet.Cells[3, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[4, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 3] + ":" + workSheet.Cells[4, 3];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 4].Value = "0h-1h";
                cell = workSheet.Cells[3, 4];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 5];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cellMerge0 = workSheet.Cells[3, 4] + ":" + workSheet.Cells[3, 5];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 6].Value = "1h-2h";
                cell = workSheet.Cells[3, 6];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 7];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 6] + ":" + workSheet.Cells[3, 7];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 8].Value = "2h-3h";
                cell = workSheet.Cells[3, 8];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 9];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 8] + ":" + workSheet.Cells[3, 9];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 10].Value = "3h-4h";
                cell = workSheet.Cells[3, 10];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 11];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 10] + ":" + workSheet.Cells[3, 11];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 12].Value = "4h-5h";
                cell = workSheet.Cells[3, 12];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 13];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 12] + ":" + workSheet.Cells[3, 13];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 14].Value = "5h-6h";
                cell = workSheet.Cells[3, 14];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 15];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);


                cellMerge0 = workSheet.Cells[3, 14] + ":" + workSheet.Cells[3, 15];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 16].Value = "6h-7h";
                cell = workSheet.Cells[3, 16];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 17];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 16] + ":" + workSheet.Cells[3, 17];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 18].Value = "7h-8h";
                cell = workSheet.Cells[3, 18];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 19];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 18] + ":" + workSheet.Cells[3, 19];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 20].Value = "8h-9h";
                cell = workSheet.Cells[3, 20];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 21];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 20] + ":" + workSheet.Cells[3, 21];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 22].Value = "9h-10h";
                cell = workSheet.Cells[3, 22];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 23];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 22] + ":" + workSheet.Cells[3, 23];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 24].Value = "10h-11h";
                cell = workSheet.Cells[3, 24];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 25];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 24] + ":" + workSheet.Cells[3, 25];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 26].Value = "11h-12h";
                cell = workSheet.Cells[3, 26];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 27];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 26] + ":" + workSheet.Cells[3, 27];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;


                workSheet.Cells[3, 28].Value = "12h-13h";
                cell = workSheet.Cells[3, 28];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 29];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 28] + ":" + workSheet.Cells[3, 29];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 30].Value = "13h-14h";
                cell = workSheet.Cells[3, 30];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 31];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 30] + ":" + workSheet.Cells[3, 31];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 32].Value = "14-15h";
                cell = workSheet.Cells[3, 32];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 33];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 32] + ":" + workSheet.Cells[3, 33];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;


                workSheet.Cells[3, 34].Value = "15h-16h";
                cell = workSheet.Cells[3, 34];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 35];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 34] + ":" + workSheet.Cells[3, 35];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 36].Value = "16h-17h";
                cell = workSheet.Cells[3, 36];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 37];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 36] + ":" + workSheet.Cells[3, 37];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 38].Value = "17h-18h";
                cell = workSheet.Cells[3, 38];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 39];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 38] + ":" + workSheet.Cells[3, 39];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 40].Value = "18h-19h";
                cell = workSheet.Cells[3, 40];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 41];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 40] + ":" + workSheet.Cells[3, 41];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 42].Value = "19h-20h";
                cell = workSheet.Cells[3, 42];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 43];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 42] + ":" + workSheet.Cells[3, 43];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 44].Value = "20h-21h";
                cell = workSheet.Cells[3, 44];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 45];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 44] + ":" + workSheet.Cells[3, 45];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 46].Value = "21h-22h";
                cell = workSheet.Cells[3, 46];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 47];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 46] + ":" + workSheet.Cells[3, 47];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 48].Value = "22h-23h";
                cell = workSheet.Cells[3, 48];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 49];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 48] + ":" + workSheet.Cells[3, 49];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 50].Value = "23h-24h";
                cell = workSheet.Cells[3, 50];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                cell = workSheet.Cells[3, 51];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);

                cellMerge0 = workSheet.Cells[3, 50] + ":" + workSheet.Cells[3, 51];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                for (int i = 4; i <= 51; i++)
                {
                    if ((i % 2) == 1)
                    {
                        workSheet.Cells[4, i].Value = "Nhỏ nhất";
                        cell = workSheet.Cells[4, i];
                        OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                    }
                    else
                    {
                        workSheet.Cells[4, i].Value = "Lớn nhất";
                        cell = workSheet.Cells[4, i];
                        OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                    }
                }

                var rowIdx = 5;

                foreach (var station in maTrams.ToList())
                {
                    var data = datas.Where(s => s.MaTramDo == station.MaTramDo).ToList();
                    var colIdx = 1;
                    workSheet.Cells[rowIdx, colIdx].Value = ++stt;
                    cell = workSheet.Cells[rowIdx, 1];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 2].Value = station.MaTramDo;
                    cell = workSheet.Cells[rowIdx, 2];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 3].Value = station.DiaDiem;
                    cell = workSheet.Cells[rowIdx, 3];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    //00
                    workSheet.Cells[rowIdx, 4].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 4];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 5].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 5];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //01
                    workSheet.Cells[rowIdx, 6].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 6];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 7].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 7];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //02
                    workSheet.Cells[rowIdx, 8].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 8];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 9].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 9];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //03
                    workSheet.Cells[rowIdx, 10].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 10];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 11].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 11];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //04
                    workSheet.Cells[rowIdx, 12].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 12];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 13].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 13];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //05
                    workSheet.Cells[rowIdx, 14].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 14];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 15].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 15];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //06
                    workSheet.Cells[rowIdx, 16].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 16];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 17].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 17];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //07
                    workSheet.Cells[rowIdx, 18].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 18];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 19].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 19];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //08
                    workSheet.Cells[rowIdx, 20].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 20];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 21].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 21];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //09
                    workSheet.Cells[rowIdx, 22].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 22];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 23].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 23];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //10
                    workSheet.Cells[rowIdx, 24].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 24];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 25].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 25];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //11
                    workSheet.Cells[rowIdx, 26].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 26];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 27].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 27];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //12
                    workSheet.Cells[rowIdx, 28].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 28];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 29].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 29];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //13
                    workSheet.Cells[rowIdx, 30].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 30];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 31].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 31];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //14
                    workSheet.Cells[rowIdx, 32].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 32];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 33].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 33];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //15
                    workSheet.Cells[rowIdx, 34].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 34];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 35].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 35];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //16
                    workSheet.Cells[rowIdx, 36].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 36];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 37].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 37];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //17
                    workSheet.Cells[rowIdx, 38].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 38];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 39].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 39];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //18
                    workSheet.Cells[rowIdx, 40].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 40];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 41].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 41];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //19
                    workSheet.Cells[rowIdx, 42].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 42];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 43].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 43];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //20
                    workSheet.Cells[rowIdx, 44].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 44];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 45].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 45];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //21
                    workSheet.Cells[rowIdx, 46].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 46];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 47].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 47];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //22
                    workSheet.Cells[rowIdx, 48].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 48];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 49].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 49];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    //23
                    workSheet.Cells[rowIdx, 50].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").max.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 50];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 51].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").min.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 51];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    rowIdx++;
                }

                package.Save();
                return new MemoryStream(package.GetAsByteArray());
            }
        }
        private MemoryStream CreateReportForMonth(List<DataReport> datas, string month, string year)
        {
            var stt = 0;


            var maTrams = _dMTramDoRepository.FindAll().ToList();

            using (ExcelPackage package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("report");
                ExcelRange cell;


                workSheet.DefaultColWidth = 15;

                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells[1, 1].Value = string.Format("THỐNG KÊ MỨC ĐỘ TIẾNG ỒN TRUNG BÌNH TẠI CÁC KHUNG THỜI GIAN THÁNG {0} NĂM {1}", month, year);

                cell = workSheet.Cells[1, 1];
                cell.Style.Font.Size = 18;
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER);

                string cellMerge0 = workSheet.Cells[1, 1] + ":" + workSheet.Cells[1, 27];
                ExcelRange rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[2, 24].Value = "Đơn vị tính: dBA";

                cellMerge0 = workSheet.Cells[2, 24] + ":" + workSheet.Cells[2, 27];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 1].Value = "STT";
                cell = workSheet.Cells[3, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 2].Value = "Mã trạm đo";
                cell = workSheet.Cells[3, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 3].Value = "Địa điểm";
                cell = workSheet.Cells[3, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 4].Value = "0h-1h";
                cell = workSheet.Cells[3, 4];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 5].Value = "1h-2h";
                cell = workSheet.Cells[3, 5];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 6].Value = "2h-3h";
                cell = workSheet.Cells[3, 6];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 7].Value = "3h-4h";
                cell = workSheet.Cells[3, 7];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 8].Value = "4h-5h";
                cell = workSheet.Cells[3, 8];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 9].Value = "5h-6h";
                cell = workSheet.Cells[3, 9];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 10].Value = "6h-7h";
                cell = workSheet.Cells[3, 10];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 11].Value = "7h-8h";
                cell = workSheet.Cells[3, 11];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 12].Value = "8h-9h";
                cell = workSheet.Cells[3, 12];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 13].Value = "9h-10h";
                cell = workSheet.Cells[3, 13];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 14].Value = "10h-11h";
                cell = workSheet.Cells[3, 14];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 15].Value = "11h-12h";
                cell = workSheet.Cells[3, 15];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 16].Value = "12h-13h";
                cell = workSheet.Cells[3, 16];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 17].Value = "13h-14h";
                cell = workSheet.Cells[3, 17];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 18].Value = "14-15h";
                cell = workSheet.Cells[3, 18];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 19].Value = "15h-16h";
                cell = workSheet.Cells[3, 19];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 20].Value = "16h-17h";
                cell = workSheet.Cells[3, 20];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 21].Value = "17h-18h";
                cell = workSheet.Cells[3, 21];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 22].Value = "18h-19h";
                cell = workSheet.Cells[3, 22];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 23].Value = "19h-20h";
                cell = workSheet.Cells[3, 23];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 24].Value = "20h-21h";
                cell = workSheet.Cells[3, 24];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 25].Value = "21h-22h";
                cell = workSheet.Cells[3, 25];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 26].Value = "22h-23h";
                cell = workSheet.Cells[3, 26];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 27].Value = "23h-24h";
                cell = workSheet.Cells[3, 27];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                var rowIdx = 4;
                foreach (var station in maTrams.ToList())
                {
                    var data = datas.Where(s => s.MaTramDo == station.MaTramDo).ToList();
                    var colIdx = 1;
                    workSheet.Cells[rowIdx, colIdx].Value = ++stt;
                    cell = workSheet.Cells[rowIdx, 1];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 2].Value = station.MaTramDo;
                    cell = workSheet.Cells[rowIdx, 2];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 3].Value = station.DiaDiem;
                    cell = workSheet.Cells[rowIdx, 3];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 4].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 4];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 5].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 5];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 6].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 6];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 7].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 7];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 8].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 8];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 9].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 9];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 10].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 10];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 11].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 11];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 12].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 12];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 13].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 13];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 14].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 14];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 15].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 15];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 16].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 16];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 17].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 17];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 18].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 18];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 19].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 19];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 20].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 20];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 21].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 21];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 22].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 22];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 23].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 23];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 24].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 24];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 25].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 25];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 26].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 26];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 27].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 27];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    rowIdx++;
                }
                package.Save();
                return new MemoryStream(package.GetAsByteArray());
            }
        }
        private MemoryStream CreateReportForYear(List<DataReport> datas, string year)
        {
            var stt = 0;
            var maTrams = _dMTramDoRepository.FindAll().ToList();

            using (ExcelPackage package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("report");
                ExcelRange cell;


                workSheet.DefaultColWidth = 15;

                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells[1, 1].Value = string.Format("THỐNG KÊ MỨC ĐỘ TIẾNG ỒN TRUNG BÌNH TẠI CÁC KHUNG THỜI GIAN NĂM {0}", year);

                cell = workSheet.Cells[1, 1];
                cell.Style.Font.Size = 18;
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER);

                string cellMerge0 = workSheet.Cells[1, 1] + ":" + workSheet.Cells[1, 27];
                ExcelRange rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[2, 24].Value = "Đơn vị tính: dBA";

                cellMerge0 = workSheet.Cells[2, 24] + ":" + workSheet.Cells[2, 27];
                rng0 = workSheet.Cells[cellMerge0];
                rng0.Merge = true;

                workSheet.Cells[3, 1].Value = "STT";
                cell = workSheet.Cells[3, 1];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 2].Value = "Mã trạm đo";
                cell = workSheet.Cells[3, 2];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 3].Value = "Địa điểm";
                cell = workSheet.Cells[3, 3];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 4].Value = "0h-1h";
                cell = workSheet.Cells[3, 4];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 5].Value = "1h-2h";
                cell = workSheet.Cells[3, 5];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 6].Value = "2h-3h";
                cell = workSheet.Cells[3, 6];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 7].Value = "3h-4h";
                cell = workSheet.Cells[3, 7];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 8].Value = "4h-5h";
                cell = workSheet.Cells[3, 8];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 9].Value = "5h-6h";
                cell = workSheet.Cells[3, 9];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 10].Value = "6h-7h";
                cell = workSheet.Cells[3, 10];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 11].Value = "7h-8h";
                cell = workSheet.Cells[3, 11];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 12].Value = "8h-9h";
                cell = workSheet.Cells[3, 12];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 13].Value = "9h-10h";
                cell = workSheet.Cells[3, 13];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 14].Value = "10h-11h";
                cell = workSheet.Cells[3, 14];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 15].Value = "11h-12h";
                cell = workSheet.Cells[3, 15];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 16].Value = "12h-13h";
                cell = workSheet.Cells[3, 16];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 17].Value = "13h-14h";
                cell = workSheet.Cells[3, 17];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 18].Value = "14-15h";
                cell = workSheet.Cells[3, 18];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 19].Value = "15h-16h";
                cell = workSheet.Cells[3, 19];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 20].Value = "16h-17h";
                cell = workSheet.Cells[3, 20];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 21].Value = "17h-18h";
                cell = workSheet.Cells[3, 21];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 22].Value = "18h-19h";
                cell = workSheet.Cells[3, 22];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 23].Value = "19h-20h";
                cell = workSheet.Cells[3, 23];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 24].Value = "20h-21h";
                cell = workSheet.Cells[3, 24];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 25].Value = "21h-22h";
                cell = workSheet.Cells[3, 25];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 26].Value = "22h-23h";
                cell = workSheet.Cells[3, 26];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                workSheet.Cells[3, 27].Value = "23h-24h";
                cell = workSheet.Cells[3, 27];
                OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.BOLD | EnumFormat.BORDER | EnumFormat.MIDDLE);
                var rowIdx = 4;
                foreach (var station in maTrams.ToList())
                {
                    var data = datas.Where(s => s.MaTramDo == station.MaTramDo).ToList();
                    var colIdx = 1;
                    workSheet.Cells[rowIdx, colIdx].Value = ++stt;
                    cell = workSheet.Cells[rowIdx, 1];
                    OfficeHelper.setStyle(ref cell, EnumFormat.CENTER | EnumFormat.MIDDLE | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 2].Value = station.MaTramDo;
                    cell = workSheet.Cells[rowIdx, 2];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 3].Value = station.DiaDiem;
                    cell = workSheet.Cells[rowIdx, 3];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);
                    workSheet.Cells[rowIdx, 4].Value = data.FirstOrDefault(s => s.hour == "00") != null ? data.FirstOrDefault(s => s.hour == "00").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 4];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 5].Value = data.FirstOrDefault(s => s.hour == "01") != null ? data.FirstOrDefault(s => s.hour == "01").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 5];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 6].Value = data.FirstOrDefault(s => s.hour == "02") != null ? data.FirstOrDefault(s => s.hour == "02").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 6];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 7].Value = data.FirstOrDefault(s => s.hour == "03") != null ? data.FirstOrDefault(s => s.hour == "03").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 7];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 8].Value = data.FirstOrDefault(s => s.hour == "04") != null ? data.FirstOrDefault(s => s.hour == "04").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 8];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 9].Value = data.FirstOrDefault(s => s.hour == "05") != null ? data.FirstOrDefault(s => s.hour == "05").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 9];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 10].Value = data.FirstOrDefault(s => s.hour == "06") != null ? data.FirstOrDefault(s => s.hour == "06").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 10];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 11].Value = data.FirstOrDefault(s => s.hour == "07") != null ? data.FirstOrDefault(s => s.hour == "07").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 11];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 12].Value = data.FirstOrDefault(s => s.hour == "08") != null ? data.FirstOrDefault(s => s.hour == "08").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 12];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 13].Value = data.FirstOrDefault(s => s.hour == "09") != null ? data.FirstOrDefault(s => s.hour == "09").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 13];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 14].Value = data.FirstOrDefault(s => s.hour == "10") != null ? data.FirstOrDefault(s => s.hour == "10").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 14];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 15].Value = data.FirstOrDefault(s => s.hour == "11") != null ? data.FirstOrDefault(s => s.hour == "11").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 15];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 16].Value = data.FirstOrDefault(s => s.hour == "12") != null ? data.FirstOrDefault(s => s.hour == "12").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 16];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 17].Value = data.FirstOrDefault(s => s.hour == "13") != null ? data.FirstOrDefault(s => s.hour == "13").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 17];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 18].Value = data.FirstOrDefault(s => s.hour == "14") != null ? data.FirstOrDefault(s => s.hour == "14").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 18];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 19].Value = data.FirstOrDefault(s => s.hour == "15") != null ? data.FirstOrDefault(s => s.hour == "15").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 19];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 20].Value = data.FirstOrDefault(s => s.hour == "16") != null ? data.FirstOrDefault(s => s.hour == "16").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 20];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 21].Value = data.FirstOrDefault(s => s.hour == "17") != null ? data.FirstOrDefault(s => s.hour == "17").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 21];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 22].Value = data.FirstOrDefault(s => s.hour == "18") != null ? data.FirstOrDefault(s => s.hour == "18").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 22];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 23].Value = data.FirstOrDefault(s => s.hour == "19") != null ? data.FirstOrDefault(s => s.hour == "19").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 23];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 24].Value = data.FirstOrDefault(s => s.hour == "20") != null ? data.FirstOrDefault(s => s.hour == "20").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 24];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 25].Value = data.FirstOrDefault(s => s.hour == "21") != null ? data.FirstOrDefault(s => s.hour == "21").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 25];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 26].Value = data.FirstOrDefault(s => s.hour == "22") != null ? data.FirstOrDefault(s => s.hour == "22").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 26];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    workSheet.Cells[rowIdx, 27].Value = data.FirstOrDefault(s => s.hour == "23") != null ? data.FirstOrDefault(s => s.hour == "23").average.ToString() : "--";
                    cell = workSheet.Cells[rowIdx, 27];
                    OfficeHelper.setStyle(ref cell, EnumFormat.LEFT | EnumFormat.BORDER);

                    rowIdx++;
                }
                package.Save();
                return new MemoryStream(package.GetAsByteArray());
            }
        }
    }
}