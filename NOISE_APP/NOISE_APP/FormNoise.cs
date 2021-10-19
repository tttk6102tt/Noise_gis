﻿using DevExpress.DXCore.Controls.Data.Linq;
using DevExpress.XtraEditors.Camera;
using DevExpress.XtraEditors.Design;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.GISClient;
using ESRI.ArcGIS.Server;
using FrameWork.Data.DB;
using Noise.Common.GIS.Classes;
using NOISE_APP.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOISE_APP
{
    public partial class FormNoise : Form
    {
        private IWorkspaceFactory2 mInMemWsFactory;
        private IWorkspaceFactory2 mGDBWsFactory;
        private IRasterWorkspaceEx mGDBRasterWs;
        private IRasterWorkspaceEx mInMemRasterWs;
        private IWorkspaceName2 mInMemWsName;
        private IWorkspaceName2 mInMemRasterWsName;
        private IWorkspaceName2 mGDBWsName;
        private IFeatureWorkspace mInMemWs;
        private IFeatureWorkspace mGDBWs;
        private IWorkspaceEdit2 mGDBWsEdit;
        private IWorkspaceEdit2 mGDBWsSeverEdit;
        private IFeatureWorkspace mGDBWsSever;
        public FormNoise()
        {
            InitializeComponent();
        }

      
        private void SetGDBServer()
        {
            string server = ConfigurationSettings.AppSettings["SERVER"];
            string instance = ConfigurationSettings.AppSettings["INSTANCE"];
            string database = ConfigurationSettings.AppSettings["DATABASE"];
            string user = ConfigurationSettings.AppSettings["USERNAME"];
            string password = ConfigurationSettings.AppSettings["PASSWORD"];
            string version = ConfigurationSettings.AppSettings["VERSION"];

            mGDBWsSever = ESRIDBHelper.Instance.OpenSDEDBWorkspace(server, user, password, database) as IFeatureWorkspace;

        }

        public void TinhToan()
        {
            //
            var paramConfig = GetDataForForm();

            var radius = Convert.ToDouble(paramConfig.FirstOrDefault(s => s.TenThamSo == "RADIUS_IDW").GiaTri);// 12;// string.IsNullOrEmpty(txtRad.Text) ? 12 : Convert.ToInt16(txtRad.Text);
            var cellSize = Convert.ToDouble(paramConfig.FirstOrDefault(s => s.TenThamSo == "CELL_SIZE_IDW").GiaTri);
            var cellHeight = Convert.ToDouble(paramConfig.FirstOrDefault(s => s.TenThamSo == "SIZE_HEIGHT").GiaTri);// 12;// string.IsNullOrEmpty(txtRad.Text) ? 12 : Convert.ToInt16(txtRad.Text);
            var cellWidth = Convert.ToDouble(paramConfig.FirstOrDefault(s => s.TenThamSo == "SIZE_WIDTH").GiaTri);
            try
            {
                //
                SetGDBServer();
                if (mGDBWsSever == null)
                {
                    txtProcess.Text += "Không kết nối được tới dữ liệu sever";
                    return;
                }
                else
                {
                    txtProcess.Text += "0. Kết nối tới dữ liệu sever";
                }
                var thoiGianTinhToanHHMM = DateTime.Now.ToString("yyyyMMddHHmmss");// "";

                //if (DateTime.Now.Minute > 30)
                //{
                //    thoiGianTinhToanHHMM = DateTime.Now.ToString("yyyyMMddHH") + "_" + "00" + "_" + "30";
                //}
                //else
                //{
                //    thoiGianTinhToanHHMM = DateTime.Now.ToString("yyyyMMdd") + ((DateTime.Now.Hour - 1) > 10 ? (DateTime.Now.Hour - 1).ToString() : "0" + (DateTime.Now.Hour - 1).ToString()) + "_" + "00" + "_" + "30";
                //}


                string thoiGianTinhToan = DateTime.Now.ToString("yyyyMMddHHmm");
                //string filegdb = paramConfig.FirstOrDefault(s => s.TenThamSo == "FILE_GDB").GiaTri;//) btnSelectGDB.EditValue.ToString();
                                                                                                   string filegdb = @"E:\20210825\CSDLTest\CSDL_ONhiemTiengOn.gdb";

                mGDBWsFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                mGDBWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IFeatureWorkspace;
                mGDBWsName = (IWorkspaceName2)((IDataset)mGDBWs).FullName;
                mGDBWsEdit = (IWorkspaceEdit2)mGDBWs;

                mInMemWsFactory = new ESRI.ArcGIS.DataSourcesGDB.InMemoryWorkspaceFactory() as IWorkspaceFactory2;
                mInMemWsName = mInMemWsFactory.Create("", $"{Guid.NewGuid().ToString()}_Ws", null, 0) as IWorkspaceName2;
                mInMemWs = (mInMemWsName as IName).Open() as IFeatureWorkspace;


                mInMemRasterWsName = mInMemWsFactory.Create("", $"{Guid.NewGuid().ToString()}_Ws", null, 0) as IWorkspaceName2;
                mInMemRasterWs = (mInMemRasterWsName as IName).Open() as IRasterWorkspaceEx;

                mGDBRasterWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IRasterWorkspaceEx;

                string tempFC = paramConfig.FirstOrDefault(s => s.TenThamSo == "FC_TEMP").GiaTri;

                var data = GetDataFromSever();

                if (data.Count <= 1)
                {
                    txtProcess.Text += "\n\nKhông đủ dữ liệu " + data.Count;
                    Save("Lỗi dữ liệu", "Không có dữ liệu", "");
                    return;
                }
                else
                {
                    txtProcess.Text += "\n\nĐã có dữ liệu";
                }

             
                //chuyển bảng dữ liệu => layer

                mGDBWsSeverEdit = (IWorkspaceEdit2)mGDBWsSever;


             

                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                NoiseGISCommon.AddFieldToTable(fieldsEdit, "ID", "ID", esriFieldType.esriFieldTypeString);
                NoiseGISCommon.AddFieldToTable(fieldsEdit, "tenDiemQuanTrac", "tenDiemQuanTrac", esriFieldType.esriFieldTypeString);
                NoiseGISCommon.AddFieldToTable(fieldsEdit, "dB", "dB", esriFieldType.esriFieldTypeDouble);


                NoiseGISCommon.AddFieldToTable(fieldsEdit, "ngayLay", "ngayLay", esriFieldType.esriFieldTypeString);
                NoiseGISCommon.AddFieldToTable(fieldsEdit, "gioLay", "gioLay", esriFieldType.esriFieldTypeString);

                NoiseGISCommon.AddFieldToTable(fieldsEdit, "LONG", "LONG", esriFieldType.esriFieldTypeDouble);
                NoiseGISCommon.AddFieldToTable(fieldsEdit, "LA", "LA", esriFieldType.esriFieldTypeDouble);

                string strBangDuLieuDongBo = "BangDuLieuDongBo_" + DateTime.Now.ToString("yyyyMMdd") + thoiGianTinhToanHHMM;

                if (NoiseGISCommon.DoesTableExist((IWorkspace)mGDBWs, strBangDuLieuDongBo, false))
                {
                    IFeatureWorkspace ws2 = (IFeatureWorkspace)mGDBWs;
                    ITable tbl = ws2.OpenTable(strBangDuLieuDongBo);
                    ((IDataset)tbl).Delete();
                }

                ITable tblBangDuLieu = NoiseGISCommon.CreateTable(mGDBWs, strBangDuLieuDongBo, fields);

                foreach (var item in data)
                {
                    var newRow = tblBangDuLieu.CreateRow();
                    newRow.Value[0] = item.ID;
                    newRow.Value[1] = item.DiaDiem;
                    newRow.Value[2] = item.dB;
                    newRow.Value[3] = item.ngayLay;
                    newRow.Value[4] = item.gioLay;
                    newRow.Value[5] = item.LONG;
                    newRow.Value[6] = item.LAT;
                    newRow.Store();
                }

                IFeatureLayer flBangDuLieu = new FeatureLayer()
                {
                    FeatureClass = NoiseGISCommon.XYTableToFeatureClass(tblBangDuLieu, "LONG", "LA", "", NoiseGISCommon.CreateSpatialRefGCS(esriSRGeoCSType.esriSRGeoCS_WGS1984))
                };

                txtProcess.Text += "\n 1. Hoàn thành đồng bộ dữ liệu";

                string tranferName = "DuLieuDongBo_tranf_" + DateTime.Now.ToString("yyyyMMddHHmm");

                NoiseGISCommon.GeographicTranfer((IWorkspace)mGDBWs, flBangDuLieu.FeatureClass, tranferName);

                IFeatureLayer flDiemDuLieuSave = new FeatureLayer()
                {
                    FeatureClass = mGDBWs.OpenFeatureClass(tranferName)
                };

                var flTramDo = mGDBWsSever.OpenFeatureClass("TramQuanTracCoDinh");

                int maTramOrgIdx = flDiemDuLieuSave.FeatureClass.FindField("ID");
                int TenTramOrgIdx = flDiemDuLieuSave.FeatureClass.FindField("tenDiemQuanTrac");
                int dBOrgIdx = flDiemDuLieuSave.FeatureClass.FindField("dB");

                int thoiGianOrgIdx = flBangDuLieu.FeatureClass.FindField("gioLay");
                int ngayOrgIdx = flBangDuLieu.FeatureClass.FindField("ngayLay");

                int maTramDesIdx = flTramDo.FindField("ID");
                int TenTramDesIdx = flTramDo.FindField("tenDiemQuanTrac");
                int thoiGianDesIdx = flTramDo.FindField("thoiGianQuanTrac");
                int ngayDesIdx = flTramDo.FindField("ngayQuanTrac");
                int dBDesIdx = flTramDo.FindField("dB");

                IFeature feature = null;
                IFeatureCursor featureCursor = flDiemDuLieuSave.FeatureClass.Search(null, false);
                try
                {
                    if (mGDBWsSeverEdit.IsInEditOperation)
                        mGDBWsSeverEdit.AbortEditOperation();
                    if (mGDBWsSeverEdit.IsBeingEdited())
                        mGDBWsSeverEdit.StopEditing(false);
                    mGDBWsSeverEdit.StartEditing(false);
                    mGDBWsSeverEdit.StartEditOperation();

                    //((ITableWrite2)flTramDo).Truncate();

                    while ((feature = featureCursor.NextFeature()) != null)
                    {
                        IFeature newFeature = flTramDo.CreateFeature();
                        newFeature.Value[maTramDesIdx] = feature.Value[maTramOrgIdx].ToString();
                        newFeature.Value[TenTramDesIdx] = feature.Value[TenTramOrgIdx].ToString();
                        newFeature.Value[dBDesIdx] = feature.Value[dBOrgIdx].ToString();
                        newFeature.Value[ngayDesIdx] = feature.Value[ngayOrgIdx].ToString();// DateTime.Now.AddHours(-1).ToString("MM/dd/yyyy");
                        newFeature.Value[thoiGianDesIdx] = feature.Value[thoiGianOrgIdx].ToString();// DateTime.Now.AddHours(-2).Hour;
                        newFeature.Shape = feature.Shape;
                        newFeature.Store();
                    }
                    mGDBWsSeverEdit.StopEditOperation();
                    mGDBWsSeverEdit.StopEditing(true);
                }
                catch (Exception ex)
                {

                }
                /*
                IFeatureLayer flRTemp = new FeatureLayer()
                {
                    FeatureClass = mGDBWs.OpenFeatureClass(tempFC)
                };

                string FishnetName = "fisnet_" + thoiGianTinhToan;

                IFeatureClass fcFishnet = NoiseGISCommon.CreateFishnet((IWorkspace)mGDBWs, FishnetName, flRTemp, cellHeight, cellWidth);

                IFeatureLayer fcFishnetLabel = new FeatureLayer()
                {
                    FeatureClass = mGDBWs.OpenFeatureClass(FishnetName + "_label")
                };

                txtProcess.Text += "\n 2. Hoàn thành tạo lưới tính toán tiếng ồn tổng hợp";

                var strBangDuLieuKhoangCachSave = "BangDuLieuKhoangCachSave_" + DateTime.Now.ToString("yyyyMMddHHmm");

                ITable tblDistance = NoiseGISCommon.PointDistance((IWorkspace)mGDBWs, FishnetName + "_label", tranferName, strBangDuLieuKhoangCachSave);

                txtProcess.Text += "\n 3. Hoàn thành tính toán khoảng cách từ các trạm tới các điểm lưới";

                if (tblDistance == null)
                {
                    MessageBox.Show("Lỗi rồi");
                    return;
                }

                NoiseGISCommon.StopEditOp(true, mGDBWsEdit);

                int idxCheck = tblDistance.FindField("TongHop");
                if (idxCheck < 0)
                {
                    ISchemaLock schemaLock = (ISchemaLock)tblDistance;
                    try
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                        // Add your field.
                        IFieldEdit2 field = new FieldClass() as IFieldEdit2;
                        field.Name_2 = "TongHop";
                        field.Type_2 = esriFieldType.esriFieldTypeDouble;
                        field.DefaultValue_2 = "TongHop";
                        tblDistance.AddField(field);
                    }
                    catch (System.Runtime.InteropServices.COMException comExc)
                    {
                        // Handle the exception appropriately for the application.
                    }
                    finally
                    {
                        // Demote the exclusive lock to a shared lock.
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    }
                }


                ITable tblBangDulieu = mGDBWs.OpenTable(tranferName);

                ITable flJoinDistance = NoiseGISCommon.TableToTableJoin(tblDistance, $"NEAR_FID", tblBangDulieu, "OBJECTID", esriRelCardinality.esriRelCardinalityOneToMany, esriJoinType.esriLeftInnerJoin, null);

                int IDTramIdx = fcFishnetLabel.FeatureClass.FindField("OID");
                int oInputFidIdx = flJoinDistance.FindField(string.Format("{0}.INPUT_FID", strBangDuLieuKhoangCachSave));
                int onearFidIdx = flJoinDistance.FindField(string.Format("{0}.NEAR_FID", strBangDuLieuKhoangCachSave));
                var distanceIdx = flJoinDistance.FindField(string.Format("{0}.DISTANCE", strBangDuLieuKhoangCachSave));
                var tongIDx = flJoinDistance.FindField(string.Format("{0}.TongHop", strBangDuLieuKhoangCachSave));
                var dbIdx = flJoinDistance.FindField(string.Format("{0}.dB", tranferName));

                NoiseGISCommon.calculate(flJoinDistance, string.Format("{0}.TongHop", strBangDuLieuKhoangCachSave), string.Format("math.pow(10,(!{0}.dB! - 20*math.log10(!{1}.DISTANCE!))/10)", tranferName, strBangDuLieuKhoangCachSave));

                txtProcess.Text += "\n 4. Hoàn thành tính toán dữ liệu tiếng ồn tổng hợp trên các điểm lưới";

                IGpValueTableObject valTbl = new GpValueTableObjectClass();
                valTbl.SetColumns(2);

                object row1 = "TongHop Sum";
                object row2 = "INPUT_FID Count";
                valTbl.AddRow(ref row1);
                valTbl.AddRow(ref row2);

                string tblSummary = "TBLSummary_" + DateTime.Now.ToString("yyyyMMddHHmm");

                NoiseGISCommon.SummaryStatic((IWorkspace)mGDBWs, strBangDuLieuKhoangCachSave, valTbl, "INPUT_FID", tblSummary);

                ITable tblDuLieu = mGDBWs.OpenTable(tblSummary);

                NoiseGISCommon.AddFieldToTable(tblDuLieu, "total");

                NoiseGISCommon.calculate(tblDuLieu, "total", "10*math.log10( !SUM_TongHop! )");

                //txtProcess.Text += "\n Hoàn thành tính toán lớp lưới";

                IGeoFeatureLayer joinSummaryWithTestLabel = NoiseGISCommon.TableToLayerJoin(tblDuLieu, "INPUT_FID", fcFishnetLabel, "OID", esriRelCardinality.esriRelCardinalityOneToOne, esriJoinType.esriLeftInnerJoin, "");

                IGeoDataset geoDataset = NoiseGISCommon.CreateGeoDataset(joinSummaryWithTestLabel.DisplayFeatureClass, string.Format("{0}.total", tblSummary));

                IRaster idwRaster = NoiseGISCommon.IDW((IWorkspace)mGDBWs, geoDataset, 2, cellSize);

                txtProcess.Text += "\n 5. Hoàn thành nội suy Raster tiếng ồn tổng hợp";

                var rasterName = "Raster_";

                //if (DateTime.Now.Minute > 30)
                //{
                //    rasterName += DateTime.Now.ToString("yyyyMMddHH") + "_" + "00" + "_" + "30";
                //}
                //else
                //{
                //    rasterName += DateTime.Now.ToString("yyyyMMdd") + ((DateTime.Now.Hour - 1) > 10 ? (DateTime.Now.Hour - 1).ToString() : "0" + (DateTime.Now.Hour - 1).ToString()) + "_" + "30" + "_" + "59";
                //}

                var hour =

                rasterName = "Raster_" + DateTime.Now.AddHours(-1).ToString("yyyyMMdd") + "_" + (DateTime.Now.AddHours(-1).Hour).ToString();

                //((ISaveAs2)idwRaster).SaveAs(rasterName, (IWorkspace)mGDBWs, "FGDBR");

                txtProcess.Text += "\n 6. Hoàn thành kết quả Raster tiếng ồn theo ranh giới.";

            

                NoiseGISCommon.Clip((IWorkspace)mGDBWs, idwRaster, flRTemp.FeatureClass, rasterName);

                var mxdPath = System.Configuration.ConfigurationManager.AppSettings["MXDPATH"].ToString();

                //var content = System.IO.File.ReadAllBytes(mxdPath);

                var mxdResult = System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FOLDERMXDPATH"].ToString(), string.Format("{0}.mxd", "bando_" + DateTime.Now.AddHours(-1).ToString("yyyyMMdd") + "_" + (DateTime.Now.AddHours(-1).Hour).ToString()));

                System.IO.File.Copy(mxdPath, mxdResult);
                */
                //NoiseGISCommon.SetDataSource(mxdResult, ((IWorkspace)mGDBWs).PathName, mGDBRasterWs.OpenRasterDataset(rasterName));
            }
            catch (COMException comEx)
            {
                txtProcess.Text += "\n Lỗi :" + comEx.Message;
                Save("Tính toán", comEx.Message, "");

            }
            catch (Exception ex)
            {
                txtProcess.Text += "\n Lỗi :" + ex.Message;
                Save("Tính toán", ex.Message, ex.InnerException.Message);
            }

        }

       
      
        private void button1_Click(object sender, EventArgs e)
        {
            

            try
            {

                mLocker = new _ProgressCancellable.Locker();
                mBgWorker = new BackgroundWorker();
                mBgWorker.WorkerSupportsCancellation = true;
                mBgWorker.DoWork += (sender2, e2) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        button1.Enabled = false;
                        txtProcess.Text = "Bắt đầu lập bản đồ lần thứ " + dem + "\n";
                        txtProcess.Text += DateTime.Now.ToString();


                        TinhToan();
                        //button1_Click(null, null);
                        txtProcess.Text += "\n\nHoàn thành lập bản đồ \n";
                        txtProcess.Text += DateTime.Now.ToString();
                        txtProcess.Text += "\n-------------------------------\n";
                    }));
                };
                mBgWorker.RunWorkerCompleted += (sender2, e2) =>
                {
                    //_ProgressCancellable.locker.IsCanceled = false;
                };
                mBgWorker.RunWorkerAsync(mLocker);



                //label2.Text = DateTime.Now.ToString();
                button1.Enabled = true;

            }
            catch (COMException comEx)
            {
                Save("TinhToan_Click", comEx.Message, "");
            }
            catch (Exception ex)
            {
                Save("TinhToan_Click", ex.Message, ex.InnerException.Message);
            }

            //Application.Exit();
        }

        //private string _ConnectionString = "Server=DESKTOP-1V8JKKD;Database=NOISE_CONFIG;User Id=sa;Password=Abc@123456;MultipleActiveResultSets=true";

        private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private List<NOISE> GetDataFromSever()
        {
            //<add name="ConnectionString" connectionString="Server=DESKTOP-1V8JKKD;Database=NOISE_CONFIG;User Id=sa;Password=Abc@123456;MultipleActiveResultSets=true" />
            DataSet ds = new DataSet();
            var result = new List<NOISE>();
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
                        //string startHour = "000000";
                        //string endHour = "003000";
                        //var dateCal = new DateTime(2021, 09, 01);
                        var today = DateTime.Now.AddHours(-8);

                        //20210610
                        //switch (today.Hour)
                        //{
                        //    case 0:
                        //        startHour = "000000";
                        //        endHour = "003000";
                        //        break;
                        //    case 1:
                        //        startHour = "010000";
                        //        endHour = "013000";
                        //        break;
                        //    case 2:
                        //        startHour = "020000";
                        //        endHour = "023000";
                        //        break;
                        //    case 3:
                        //        startHour = "030000";
                        //        endHour = "033000";
                        //        break;
                        //    case 4:
                        //        startHour = "040000";
                        //        endHour = "043000";
                        //        break;
                        //    case 5:
                        //        startHour = "090000";
                        //        endHour = "110000";
                        //        break;
                        //    case 6:
                        //        startHour = "110000";
                        //        endHour = "130000";
                        //        break;
                        //    case 7:
                        //        startHour = "130000";
                        //        endHour = "150000";
                        //        break;
                        //    case 8:
                        //        startHour = "150000";
                        //        endHour = "170000";
                        //        break;
                        //    case 9:
                        //        startHour = "070000";
                        //        endHour = "090000";
                        //        break;
                        //    case 10:
                        //        startHour = "090000";
                        //        endHour = "110000";
                        //        break;
                        //    case 11:
                        //        startHour = "110000";
                        //        endHour = "130000";
                        //        break;
                        //    case 12:
                        //        startHour = "130000";
                        //        endHour = "150000";
                        //        break;
                        //    case 13:
                        //        startHour = "150000";
                        //        endHour = "170000";
                        //        break;
                        //    case 14:
                        //        startHour = "070000";
                        //        endHour = "090000";
                        //        break;
                        //    case 15:
                        //        startHour = "090000";
                        //        endHour = "110000";
                        //        break;
                        //    case 16:
                        //        startHour = "110000";
                        //        endHour = "130000";
                        //        break;
                        //    case 17:
                        //        startHour = "130000";
                        //        endHour = "150000";
                        //        break;
                        //    case 18:
                        //        startHour = "150000";
                        //        endHour = "170000";
                        //        break;
                        //    case 19:
                        //        startHour = "070000";
                        //        endHour = "090000";
                        //        break;
                        //    case 20:
                        //        startHour = "090000";
                        //        endHour = "110000";
                        //        break;
                        //    case 21:
                        //        startHour = "110000";
                        //        endHour = "130000";
                        //        break;
                        //    case 22:
                        //        startHour = "130000";
                        //        endHour = "150000";
                        //        break;
                        //    case 23:
                        //        startHour = "150000";
                        //        endHour = "170000";
                        //        break;
                        //    default:
                        //        break;
                        //}
                        /*
                        if (today.Minute < 30)
                        {
                            var hour = (today.Hour + 1) > 10 ? (today.Hour - 1).ToString() : "0" + (today.Hour - 1).ToString();
                            startHour = hour + "3000";
                            endHour = hour + "5900";
                        }
                        else
                        {
                            var hour = today.Hour > 10 ? today.Hour.ToString() : "0" + today.Hour.ToString();
                            startHour = hour + "3000";
                            endHour = hour + "5900";
                        }
                        */

                        string startTime = today.ToString("yyyyMMddHH") + "0000";// startHour;
                        string endTime = today.ToString("yyyyMMddHH") + "5959";// endHour;

                        cmd.Connection = conn;

                        cmd.CommandTimeout = 5 * 60 * 60;
                        //cmd.CommandText = string.Format(@"
                        //        SELECT n.ID, 
                        //                d.DiaDiem,   
                        //                sum(CAST(n.db as numeric(9,6)))/count(n.ID) as dB,
                        //                sum(CAST(n.LONG as numeric(9,6)))/count(n.ID) as Long,
                        //                sum(CAST(n.LAT as numeric(9,6)))/count(n.ID) as LAT	
                        //                FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  
                        //                WHERE n.ID <> ''  AND n.dB <> '' AND (ISNUMERIC(TIME) = 1 AND (CAST(n.TIME as DECIMAL(16,2)) < {0} AND CAST(n.TIME as DECIMAL(16,2)) > {1})) 
                        //                group by n.ID, d.DiaDiem
                        //        ", endTime, startTime);
                        cmd.CommandText = @" SELECT n.ID, 
                                        d.DiaDiem,   
                                        sum(CAST(n.db as numeric(9, 6)))/ count(n.ID) as dB,
                                        sum(CAST(n.LONG as numeric(9, 6)))/ count(n.ID) as Long,
                                        sum(CAST(n.LAT as numeric(9, 6)))/ count(n.ID) as LAT,	
										  SUBSTRING(TIME, 9, 2) as hour2,
                                                        SUBSTRING(TIME, 7, 2) as day2,
                                                        SUBSTRING(TIME, 5, 2) as month2,
                                                        SUBSTRING(TIME, 1, 4) as year2
                                        FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID
                                       where ISNUMERIC(TIME) = 1 and ISNUMERIC(dB) = 1 and ISNUMERIC(LONG) = 1 and ISNUMERIC(LAT) = 1 and TIME > '20210915000000'
                                        group by n.ID, d.DiaDiem,
                                          SUBSTRING(TIME, 9, 2),
                                                        SUBSTRING(TIME, 7, 2),
                                                        SUBSTRING(TIME, 5, 2),
                                                        SUBSTRING(TIME, 1, 4)";

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                var year = Convert.ToInt16(row["year2"].ToString());
                                var month = Convert.ToInt16(row["month2"].ToString());
                                var day = Convert.ToInt16(row["day2"].ToString());
                                var hour = Convert.ToInt16(row["hour2"].ToString());
                                var datetime = new DateTime(year,month,day).AddHours(7).AddHours(hour);

                                result.Add(new NOISE()
                                {
                                    dB = row["dB"].ToString(),
                                    ID = row["ID"].ToString(),
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    LAT = row["LAT"].ToString(),
                                    LONG = row["LONG"].ToString(),
                                    ngayLay = datetime.ToString("MM/dd/yyyy"),
                                    gioLay  = row["hour2"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (COMException comEx)
                {
                    Save("Lấy dữ liệu", comEx.Message, "");
                }
                catch (Exception ex)
                {
                    Save("Lấy dữ liệu", ex.Message, ex.InnerException.Message);
                }
                finally
                {
                    conn.Close();
                }

                return result;
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
        private void btnSelectGDB_Click(object sender, EventArgs e)
        {
            try
            {
                IGxDialog pGxDialog = new GxDialog();
                //IGxObjectFilter pGxObjFilter = new GxFilterRasterDatasets();
                IGxObjectFilter gxObjectFilter = new GxFilterFileGeodatabases();
                IEnumGxObject pEnumGxObj;
                pGxDialog.ObjectFilter = gxObjectFilter;
                pGxDialog.Title = "Chọn gdb";
                pGxDialog.ButtonCaption = "OK";
                if (pGxDialog.DoModalOpen(0, out pEnumGxObj))
                {
                    IGxObject pGxObj = pEnumGxObj.Next();
                    IGxDataset pGxDataset = pGxObj as IGxDataset;
                    btnSelectGDB.EditValue = pGxObj.FullName;
                }
            }
            catch (COMException comEx)
            {
                Save("SetGDB", comEx.Message, "");
            }
            catch (Exception ex)
            {
                Save("SetGDB", ex.Message, ex.InnerException.Message);
            }

        }

        private void btnSelectGDB_EditValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = !string.IsNullOrEmpty(btnSelectGDB.EditValue.ToString());
        }


        List<TBLConfig> GetDataForForm()
        {
            DataSet ds = new DataSet();
            var result = new List<TBLConfig>();
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
                        cmd.CommandText = @"SELECT [ID]
                                              ,[TenThamSo]
                                              ,[GiaTri]
                                              ,[Param_Config]
                                          FROM [TBLCONFIG]";


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {

                                result.Add(new TBLConfig()
                                {
                                    TenThamSo = row["TenThamSo"].ToString(),
                                    GiaTri = row["GiaTri"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (COMException comEx)
                {
                    Save("Lấy config", comEx.Message, "");
                }
                catch (Exception ex)
                {
                    Save("Lấy config", ex.Message, ex.InnerException.Message);
                }
                finally
                {
                    conn.Close();
                }
                return result;
            }
        }
        private int dem = 0;
        private void FormNoise_Load(object sender, EventArgs e)
        {
            dem++;
        
            timer1.Interval = 60 * 60 * 1000;// 
            timer1.Start();
        }

        private BackgroundWorker mBgWorker;
        private _ProgressCancellable.Locker mLocker;
        private void FormNoise_Shown(object sender, EventArgs e)
        {
            button1_Click(null, null);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dem++;
            txtProcess.Text += DateTime.Now.ToString();
            button1_Click(null, null);
        }

        


        private void button2_Click(object sender, EventArgs e)
        {
            //var fileTempMxd = @"E:\Noise_Data\temp.mxd";



            /*
            string filegdb = @"E:\20210825\CSDLTest\CSDL_ONhiemTiengOn.gdb";

            mGDBWsFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;
            mGDBWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IFeatureWorkspace;


            mGDBWsFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;

            mGDBRasterWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IRasterWorkspaceEx;

            Clip((IWorkspace)mGDBWs, "Raster_2021092914_00_30", "NghiaTan", "Raster_2021092914_00_30_Clip");
            */
            /*
           IRasterDataset heSoRRaster = mGDBRasterWs.OpenRasterDataset("LTotal48816");

           SetDataSource(@"E:\Noise_Data\temp.mxd", ((IWorkspace)mGDBWs).PathName, heSoRRaster);
           */
            //Publish("http://tiengontructuyen.vn/arcgis/admin", "siteadmin", "Noise@gs1", "bando", "", "");
            //AddMapService("tiengontructuyen.vn", "bando", @"E:\Noise_Data\temp.mxd", 1);
        }

    }
}
