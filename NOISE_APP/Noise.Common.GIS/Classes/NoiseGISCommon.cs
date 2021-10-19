using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noise.Common.GIS.Classes
{
    public class NoiseGISCommon
    {
        public static bool DoesTableExist(IWorkspace workspace, string tableName, bool isNameFullyQualified)
        {
            IEnumDatasetName eDSNames = workspace.get_DatasetNames(esriDatasetType.esriDTTable);
            IDatasetName DSName = eDSNames.Next();
            while (DSName != null)
            {
                if (!isNameFullyQualified)
                {
                    string[] nameParts = DSName.Name.Split('.');
                    if (nameParts[nameParts.Length - 1].ToLower() == tableName.ToLower())
                        return true;
                }
                else
                {
                    if (DSName.Name.ToLower() == tableName.ToLower())
                        return true;
                }
                DSName = eDSNames.Next();
            }
            return false;
        }
        public static void DeleteFeatureClass(IWorkspace2 workspace, string featureName, bool isNameFullyQualified, IFeatureWorkspace featureWs)
        {
            if (workspace.get_NameExists(esriDatasetType.esriDTFeatureClass, featureName))
            {
                IFeatureClass f = featureWs.OpenFeatureClass(featureName);
                ((IDataset)f).Delete();
            }
        }
        public static bool DoesFeatureClassExist(IWorkspace2 workspace, string featureClassName, bool isNameFullyQualified)
        {
            return workspace.get_NameExists(esriDatasetType.esriDTFeatureClass, featureClassName);
        }
        public static IFeatureClass FeatureToFeatureByGP(IWorkspace ws, string pInputFeatureClass, string outName, string outFeature, string sqlQuery, string[] field = null, string featureDataset = "", string filePath = "")
        {
            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
            if (DoesFeatureClassExist((IWorkspace2)ws2, outName, false))
            {
                DeleteFeatureClass((IWorkspace2)ws2, outName, false, ws2);
            }
            if (DoesTableExist(ws, outName, false))
            {
                ITable tbl = ws2.OpenTable(outName);
                ((IDataset)tbl).Delete();
            }
            try
            {
                ESRI.ArcGIS.ConversionTools.FeatureClassToFeatureClass featureToFeature = new ESRI.ArcGIS.ConversionTools.FeatureClassToFeatureClass();
                featureToFeature.in_features = ws.PathName + @"\" + pInputFeatureClass;
                featureToFeature.out_name = outName; //"" +
                featureToFeature.field_mapping = field;
                featureToFeature.out_path = ws.PathName + $@"\{featureDataset}\";
                featureToFeature.where_clause = sqlQuery;
                GP.Execute(featureToFeature, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {
            }

            return (ws as IFeatureWorkspace).OpenFeatureClass($"{outName}");
        }

        public static IFeatureClass FeatureToFeatureByGP(IWorkspace ws, string wsDesPath, string pInputFeatureClass, string outName, string outFeature, string sqlQuery, string[] field = null, string featureDataset = "", string filePath = "")
        {
            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
            if (DoesFeatureClassExist((IWorkspace2)ws2, outName, false))
            {
                DeleteFeatureClass((IWorkspace2)ws2, outName, false, ws2);
            }
            if (DoesTableExist(ws, outName, false))
            {
                ITable tbl = ws2.OpenTable(outName);
                ((IDataset)tbl).Delete();
            }
            try
            {
                ESRI.ArcGIS.ConversionTools.FeatureClassToFeatureClass featureToFeature = new ESRI.ArcGIS.ConversionTools.FeatureClassToFeatureClass();
                featureToFeature.in_features = ws.PathName + @"\" + pInputFeatureClass;
                featureToFeature.out_name = outName; //"" +
                featureToFeature.field_mapping = field;
                featureToFeature.out_path = wsDesPath + $@"\{featureDataset}\";
                featureToFeature.where_clause = sqlQuery;
                GP.Execute(featureToFeature, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {
            }

            return (ws as IFeatureWorkspace).OpenFeatureClass($"{outName}");
        }

        public static IFeatureClass FeatureToFeatureByGP(IWorkspace ws, object pInputFeatureClass, string outName, string outFeature, string sqlQuery, string[] field = null, string featureDataset = "", string filePath = "")
        {
            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
            if (DoesFeatureClassExist((IWorkspace2)ws2, outName, false))
            {
                DeleteFeatureClass((IWorkspace2)ws2, outName, false, ws2);
            }
            if (DoesTableExist(ws, outName, false))
            {
                ITable tbl = ws2.OpenTable(outName);
                ((IDataset)tbl).Delete();
            }
            try
            {
                ESRI.ArcGIS.ConversionTools.FeatureClassToFeatureClass featureToFeature = new ESRI.ArcGIS.ConversionTools.FeatureClassToFeatureClass();
                featureToFeature.in_features = pInputFeatureClass;
                featureToFeature.out_name = outName; //"" +
                featureToFeature.field_mapping = field;
                featureToFeature.out_path = ws.PathName + $@"\{featureDataset}\";
                featureToFeature.where_clause = sqlQuery;
                GP.Execute(featureToFeature, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {
            }

            return (ws as IFeatureWorkspace).OpenFeatureClass($"{outName}");
        }

        public static IFeatureClass CreateFishnet(IWorkspace ws, string outName, IFeatureLayer fcTemp, double cellHeight, double cellWidth)
        {
            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
            try
            {

                if (DoesFeatureClassExist((IWorkspace2)ws2, outName, false))
                {
                    DeleteFeatureClass((IWorkspace2)ws2, outName, false, ws2);
                }
                if (DoesTableExist(ws, outName, false))
                {
                    ITable tbl = ws2.OpenTable(outName);
                    ((IDataset)tbl).Delete();
                }

                if (DoesFeatureClassExist((IWorkspace2)ws2, outName + "_label", false))
                {
                    DeleteFeatureClass((IWorkspace2)ws2, outName + "_label", false, ws2);
                }
                if (DoesTableExist(ws, outName + "_label", false))
                {
                    ITable tbl = ws2.OpenTable(outName + "_label");
                    ((IDataset)tbl).Delete();
                }
                var en = GetExtent(fcTemp);

                var x = en.XMax;
                var xm = en.XMin;
                var y = en.YMax;
                var ym = en.YMin;

                ESRI.ArcGIS.DataManagementTools.CreateFishnet createFishnet = new ESRI.ArcGIS.DataManagementTools.CreateFishnet();
                createFishnet.out_feature_class = ws.PathName + @"\" + outName;
                createFishnet.cell_height = cellHeight;
                createFishnet.cell_width = cellWidth;

                createFishnet.geometry_type = "POLYGON";
                createFishnet.origin_coord = string.Format("{0} {1}", xm, ym);
                createFishnet.y_axis_coord = string.Format("{0} {1}", xm, ym + 10);
                createFishnet.corner_coord = string.Format("{0} {1}", x, y);

                createFishnet.template = fcTemp.FeatureClass;

                GP.Execute(createFishnet, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception ex)
            {
            }

            return (ws as IFeatureWorkspace).OpenFeatureClass($"{outName}");
        }
        public static ITable PointDistance(IWorkspace ws, string inName, string nearName, string outName)
        {
            Geoprocessor GP = new Geoprocessor();
            GP.OverwriteOutput = true;
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;

            try
            {
                if (DoesFeatureClassExist((IWorkspace2)ws2, outName, false))
                {
                    DeleteFeatureClass((IWorkspace2)ws2, outName, false, ws2);
                }
                if (DoesTableExist(ws, outName, false))
                {
                    ITable tbl = ws2.OpenTable(outName);
                    ((IDataset)tbl).Delete();
                }
                ESRI.ArcGIS.AnalysisTools.PointDistance pointDistance = new ESRI.ArcGIS.AnalysisTools.PointDistance();
                pointDistance.in_features = ws.PathName + @"\" + inName;
                pointDistance.near_features = ws.PathName + @"\" + nearName;
                pointDistance.out_table = ws.PathName + @"\" + outName;

                GP.Execute(pointDistance, null);

            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception exx)
            {
                //MessageBox.Show(exx.Message);
            }
            return (ws as IFeatureWorkspace).OpenTable($"{outName}");//ws.PathName + @"\" + outName;//

        }
        public static IFeatureClass GeographicTranfer(IWorkspace ws, object inName, string outName)
        {
            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
            try
            {
                if (DoesFeatureClassExist((IWorkspace2)ws2, outName, false))
                {
                    DeleteFeatureClass((IWorkspace2)ws2, outName, false, ws2);
                }
                if (DoesTableExist(ws, outName, false))
                {
                    ITable tbl = ws2.OpenTable(outName);
                    ((IDataset)tbl).Delete();
                }

                //esriSRGeoCS2Type.esriSRGeoCS_VN2000
                ESRI.ArcGIS.DataManagementTools.Project project = new ESRI.ArcGIS.DataManagementTools.Project();

                project.in_dataset = inName;// ws.PathName + @"\" + inName;
                project.in_coor_system = CreateSpatialRefGCS(esriSRGeoCSType.esriSRGeoCS_WGS1984);
                project.out_coor_system = GetSpatialReference("VN_2000_UTM_Zone_48N");
                project.out_dataset = ws.PathName + @"\QuanTracONhiemTiengOn\" + outName;

                GP.Execute(project, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception ex)
            {
            }

            return (ws as IFeatureWorkspace).OpenFeatureClass($"{outName}");
        }
        private static ISpatialReference GetSpatialReference(string srName)
        {
            var srProjCSArray = Enum.GetValues(typeof(esriSRProjCSType));
            var srEnvirnonment = new SpatialReferenceEnvironment();

            foreach (var item in srProjCSArray)
            {
                var sr = srEnvirnonment.CreateProjectedCoordinateSystem((int)item);

                System.Console.WriteLine(sr.Name);

                if (sr.Name == srName)
                    return sr;
            }

            srProjCSArray = Enum.GetValues(typeof(esriSRProjCS2Type));

            foreach (var item in srProjCSArray)
            {
                var sr = srEnvirnonment.CreateProjectedCoordinateSystem((int)item);

                System.Console.WriteLine(sr.Name);

                if (sr.Name == srName)
                    return sr;
            }

            srProjCSArray = Enum.GetValues(typeof(esriSRProjCS4Type));

            foreach (var item in srProjCSArray)
            {
                var sr = srEnvirnonment.CreateProjectedCoordinateSystem((int)item);

                System.Console.WriteLine(sr.Name);

                if (sr.Name == srName)
                    return sr;
            }

            srProjCSArray = Enum.GetValues(typeof(esriSRProjCS3Type));

            foreach (var item in srProjCSArray)
            {
                var sr = srEnvirnonment.CreateProjectedCoordinateSystem((int)item);

                System.Console.WriteLine(sr.Name);

                if (sr.Name == srName)
                    return sr;
            }

            srProjCSArray = Enum.GetValues(typeof(esriSRGeoCSType));

            foreach (var item in srProjCSArray)
            {
                var sr = srEnvirnonment.CreateProjectedCoordinateSystem((int)item);

                System.Console.WriteLine(sr.Name);

                if (sr.Name == srName)
                    return sr;
            }

            return null;
        }
        public static IFeatureClass XYTableToFeatureClass(ITable xyTable, string xFieldName, string yFieldName, string zFieldName, ISpatialReference spatialReference)
        {
            return (IFeatureClass)CreateXYEventSourceFromTable(xyTable, xFieldName, yFieldName, zFieldName, spatialReference);
        }

        public static IXYEventSource CreateXYEventSourceFromTable(ITable xyTable, string xFieldName, string yFieldName, string zFieldName, ISpatialReference spatialReference)
        {
            // Validate arguments
            if (xyTable == null)
                throw new ArgumentNullException("xyTable", "XY table has not been set.");
            if (string.IsNullOrEmpty(xFieldName))
                throw new ArgumentNullException("xFieldName", "X field name has not been set.");
            if (string.IsNullOrEmpty(yFieldName))
                throw new ArgumentNullException("yFieldName", "Y field name has not been set.");
            if (spatialReference == null)
                throw new ArgumentNullException("spatialReference", "Spatial reference has not been set.");

            // QI to get the dataset
            IDataset dataset = xyTable as IDataset;

            // Verify fields exist and are of a numeric type
            Int32 index = xyTable.FindField(xFieldName);
            if (index == -1)
                throw new ArgumentException(string.Format("{0} field does not exist in table {1}.", xFieldName, dataset.Name));
            index = xyTable.FindField(yFieldName);
            if (index == -1)
                throw new ArgumentException(string.Format("{0} field does not exist in table {1}.", yFieldName, dataset.Name));

            // Set the field properties for the XY event theme
            IXYEvent2FieldsProperties xyEvent2FieldsProperties = new XYEvent2FieldsPropertiesClass();
            xyEvent2FieldsProperties.XFieldName = xFieldName;
            xyEvent2FieldsProperties.YFieldName = yFieldName;
            xyEvent2FieldsProperties.ZFieldName = zFieldName;

            // Create XY event theme
            IXYEventSourceName xyEventSourceName = new XYEventSourceNameClass();
            xyEventSourceName.EventProperties = xyEvent2FieldsProperties;
            xyEventSourceName.SpatialReference = spatialReference;
            xyEventSourceName.EventTableName = dataset.FullName;

            // Create XY event source. 
            IName name = (xyEventSourceName as IName);
            IXYEventSource xyEventSource = (name.Open() as IXYEventSource);

            // Return value
            return xyEventSource;
        }
        public static void XYEvent(IWorkspace ws, string inTable, string outName, string x, string y)
        {

            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
            if (DoesFeatureClassExist((IWorkspace2)ws2, outName, false))
            {
                DeleteFeatureClass((IWorkspace2)ws2, outName, false, ws2);
            }
            if (DoesTableExist(ws, outName, false))
            {
                ITable tbl = ws2.OpenTable(outName);
                ((IDataset)tbl).Delete();
            }
            try
            {
                ESRI.ArcGIS.DataManagementTools.MakeXYEventLayer makeXY = new ESRI.ArcGIS.DataManagementTools.MakeXYEventLayer();
                makeXY.table = ws.PathName + @"\" + inTable;
                makeXY.out_layer = ws.PathName + @"\" + outName;
                makeXY.in_x_field = x;
                makeXY.in_y_field = y;
                GP.Execute(makeXY, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {
            }

            //return (ws as IFeatureWorkspace).OpenFeatureClass($"{outName}");
        }
        public static void Clip(IWorkspace ws, object pInputRaster, object pClipFeatureClass, string outFeature, IRasterWorkspaceEx rasterWs = null)
        {
            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
            //if (DoesFeatureClassExist((IWorkspace2)ws2, outFeature, false))
            //{
            //    DeleteFeatureClass((IWorkspace2)ws2, outFeature, false, ws2);
            //}
            //if (DoesTableExist(ws, outFeature, false))
            //{
            //    ITable tbl = ws2.OpenTable(outFeature);
            //    ((IDataset)tbl).Delete();
            //}
            //Geoprocessor GP = new Geoprocessor();
            if (DoesRasterExist(ws, outFeature, false) && rasterWs != null)
            {
                DeleteRaster((IWorkspace)ws, outFeature, false, (IRasterWorkspaceEx)rasterWs);

            }
            try
            {
                ESRI.ArcGIS.DataManagementTools.Clip clip = new ESRI.ArcGIS.DataManagementTools.Clip();
                clip.in_raster = pInputRaster;
                clip.in_template_dataset = pClipFeatureClass;
                clip.clipping_geometry = "ClippingGeometry";
                clip.maintain_clipping_extent = "NO_MAINTAIN_EXTENT";
                clip.out_raster = ws.PathName + @"\" + outFeature;
                GP.Execute(clip, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                for (int i = 0; i < GP.MessageCount; i++)
                {
                    var x = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {

                throw;
            }


            //return (ws as IFeatureWorkspace).OpenFeatureClass($"{outFeature}");
        }
        public static void DeleteRaster(IWorkspace workspace, string rasterName, bool isNameFullyQualified, IRasterWorkspaceEx rasterWs)
        {
            IEnumDatasetName eDSNames = workspace.get_DatasetNames(esriDatasetType.esriDTRasterDataset);
            IDatasetName DSName = eDSNames.Next();
            while (DSName != null)
            {
                if (!isNameFullyQualified)
                {
                    string[] nameParts = DSName.Name.Split('.');
                    if (nameParts[nameParts.Length - 1].ToLower() == rasterName.ToLower())
                    {
                        IRasterDataset raster = rasterWs.OpenRasterDataset(rasterName);
                        ((IDataset)raster).Delete();
                    }
                }
                else
                {
                    if (DSName.Name.ToLower() == rasterName.ToLower())
                    {
                        IRasterDataset raster = rasterWs.OpenRasterDataset(rasterName);
                        ((IDataset)raster).Delete();
                    }
                }
                DSName = eDSNames.Next();
            }
        }
        public static void Clip(IWorkspace ws, string pInputRaster, string pClipFeatureClass, string outFeature, IRasterWorkspaceEx rasterWs = null)
        {
            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
            //if (DoesFeatureClassExist((IWorkspace2)ws2, outFeature, false))
            //{
            //    DeleteFeatureClass((IWorkspace2)ws2, outFeature, false, ws2);
            //}
            //if (DoesTableExist(ws, outFeature, false))
            //{
            //    ITable tbl = ws2.OpenTable(outFeature);
            //    ((IDataset)tbl).Delete();
            //}
            //Geoprocessor GP = new Geoprocessor();
            if (DoesRasterExist(ws,outFeature,false) && rasterWs != null)
            {
                DeleteRaster((IWorkspace)ws, outFeature, false, (IRasterWorkspaceEx)rasterWs);
                
            }
            try
            {
                ESRI.ArcGIS.DataManagementTools.Clip clip = new ESRI.ArcGIS.DataManagementTools.Clip();
                clip.in_raster = ws.PathName + @"\" + pInputRaster;
                clip.in_template_dataset = ws.PathName + @"\" + pClipFeatureClass;
                clip.clipping_geometry = "ClippingGeometry";
                clip.maintain_clipping_extent = "NO_MAINTAIN_EXTENT";
                clip.out_raster = ws.PathName + @"\" + outFeature;
                GP.Execute(clip, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                for (int i = 0; i < GP.MessageCount; i++)
                {
                    var x = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {

                throw;
            }


            //return (ws as IFeatureWorkspace).OpenFeatureClass($"{outFeature}");
        }

        public static IRasterLayer SetStretchRenderer(IRaster pRaster)
        {
            try
            {
                //创建着色类和QI栅格着色
                IRasterStretchColorRampRenderer pStretchRen = new RasterStretchColorRampRendererClass();
                IRasterRenderer pRasRen = pStretchRen as IRasterRenderer;
                //为着色和更新设置栅格数据
                pRasRen.Raster = pRaster;
                pRasRen.Update();
                //定义起点和终点颜色
                IColor pFromColor = new RgbColorClass();
                IRgbColor pRgbColor = pFromColor as IRgbColor;
                pRgbColor.Red = 255;
                pRgbColor.Green = 0;
                pRgbColor.Blue = 0;
                IColor pToColor = new RgbColorClass();
                pRgbColor = pToColor as IRgbColor;
                pRgbColor.Red = 0;
                pRgbColor.Green = 255;
                pRgbColor.Blue = 0;
                //创建颜色分级
                IAlgorithmicColorRamp pRamp = new AlgorithmicColorRampClass();
                pRamp.Size = 255;
                pRamp.FromColor = pFromColor;
                pRamp.ToColor = pToColor;
                bool ok = true;
                pRamp.CreateRamp(out ok);
                //把颜色分级插入着色中并选择一个波段
                pStretchRen.BandIndex = 0;
                pStretchRen.ColorRamp = pRamp;
                pRasRen.Update();
                IRasterLayer pRLayer = new RasterLayerClass();
                pRLayer.CreateFromRaster(pRaster);
                pRLayer.Renderer = pStretchRen as IRasterRenderer;
                return pRLayer;


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public static void ExportToExcel(ITable tbl, string pathExcel)
        {
            var GP = new Geoprocessor();
            try
            {
                var calculateField = new ESRI.ArcGIS.ConversionTools.TableToExcel()
                {
                    Input_Table = tbl,
                    Output_Excel_File = pathExcel

                };


                GP.Execute(calculateField, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception ex1)
            {

                throw;
            }
        }

        public static void AddFieldToTable(ITable inObj, string FieldName)
        {
            int idxCheck = inObj.FindField(FieldName);
            if (idxCheck >= 0)
            {
                return;
            }
            ISchemaLock schemaLock = (ISchemaLock)inObj;
            try
            {
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                // Add your field.
                IFieldEdit2 field = new FieldClass() as IFieldEdit2;
                field.Name_2 = FieldName;
                field.Type_2 = esriFieldType.esriFieldTypeDouble;
                field.DefaultValue_2 = FieldName;
                inObj.AddField(field);
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

        private static void AddFieldToLayer(IFeatureLayer inObj, string FieldName)
        {
            int idxCheck = inObj.FeatureClass.FindField(FieldName);
            if (idxCheck >= 0)
            {
                return;
            }
            ISchemaLock schemaLock = (ISchemaLock)inObj;
            try
            {
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                // Add your field.
                IFieldEdit2 field = new FieldClass() as IFieldEdit2;
                field.Name_2 = FieldName;
                field.Type_2 = esriFieldType.esriFieldTypeDouble;
                field.DefaultValue_2 = FieldName;
                inObj.FeatureClass.AddField(field);
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

        public static void SummaryStatic(IWorkspace ws, string inTable, object static_field, string case_field, string outTable)
        {
            var GP = new Geoprocessor();
            try
            {
                IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;
                if (DoesFeatureClassExist((IWorkspace2)ws2, outTable, false))
                {
                    DeleteFeatureClass((IWorkspace2)ws2, outTable, false, ws2);
                }
                if (DoesTableExist(ws, outTable, false))
                {
                    ITable tbl = ws2.OpenTable(outTable);
                    ((IDataset)tbl).Delete();
                }

                var calculateField = new ESRI.ArcGIS.AnalysisTools.Statistics()
                {
                    in_table = ws.PathName + @"\" + inTable,
                    statistics_fields = static_field,
                    case_field = case_field,
                    out_table = ws.PathName + @"\" + outTable,
                };

                GP.Execute(calculateField, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception ex1)
            {

                throw;
            }

        }

        public static void calculate(object inTable, string field, string expression, string codeBlock = "", bool isVB = false)
        {
            var GP = new Geoprocessor();
            try
            {
                var calculateField = new CalculateField()
                {
                    in_table = inTable,
                    field = field,
                    expression = expression,
                    expression_type = isVB ? "VB" : "PYTHON",
                    code_block = codeBlock
                };


                GP.Execute(calculateField, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception ex1)
            {

                throw;
            }

        }

        public static ITable TableToTableJoin(ITable tbl, string joinField, ITable tblTo, string joinFieldTo, esriRelCardinality relType, esriJoinType joinType, string relName)
        {
            Geoprocessor GP = new Geoprocessor();
            IMemoryRelationshipClassFactory pMemRelFact = new MemoryRelationshipClassFactoryClass();
            Type memRelClassFactoryType = Type.GetTypeFromProgID("esriGeodatabase.MemoryRelationshipClassFactory");
            IMemoryRelationshipClassFactory memRelClassFactory = (IMemoryRelationshipClassFactory)Activator.CreateInstance(memRelClassFactoryType);
            string x = ((IDataset)tbl).Name;
            string y = ((IDataset)tblTo).Name;
            IRelationshipClass pRelClass = pMemRelFact.Open(relName ?? $"{((IDataset)tbl).Name}_{((IDataset)tblTo).Name}", (IObjectClass)tbl, joinField, (IObjectClass)tblTo, joinFieldTo, "ForwardPath", "BackwardPath", relType);

            Type rqtFactoryType = Type.GetTypeFromProgID("esriGeodatabase.RelQueryTableFactory");
            IRelQueryTableFactory rqtFactory = Activator.CreateInstance(rqtFactoryType) as IRelQueryTableFactory;
            //
            return (ITable)rqtFactory.Open(pRelClass, true, null, null, string.Empty, true, true);
        }

        public static void AddFieldToTable(IFieldsEdit fieldsEdit, string aliasName, string fieldName, esriFieldType type)
        {
            IField field = new Field();
            IFieldEdit OMFieldEdit = (IFieldEdit)field;
            OMFieldEdit.AliasName_2 = aliasName;
            OMFieldEdit.Name_2 = fieldName;
            OMFieldEdit.Type_2 = type;
            OMFieldEdit.Editable_2 = true;
            fieldsEdit.AddField(field);
        }

        public static ITable CreateTable(IFeatureWorkspace featureWorkspace, string name, IFields fields)
        {

            if (featureWorkspace == null)
            {
                throw new ArgumentNullException("Error creating table. The reference to IFeatureWorkspace cannot be NULL.");
            }
            UID pCLSID;
            pCLSID = new UID();
            pCLSID.Value = "esriGeoDatabase.Object";
            IFieldsEdit pFieldsEdit;

            if ((fields == null) || (fields.FieldCount == 0))
            {
                fields = new Fields();
                pFieldsEdit = fields as IFieldsEdit;

                IField pFieldOID = new Field();
                IFieldEdit pFieldOIDEdit = pFieldOID as IFieldEdit;
                pFieldOIDEdit.Name_2 = "OBJECTID";
                pFieldOIDEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                pFieldOIDEdit.IsNullable_2 = false;

                pFieldsEdit.AddField(pFieldOID);
            }
            ITable table = null;
            try
            {
                table = featureWorkspace.CreateTable(name, fields, pCLSID, null, "");
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                throw new System.Runtime.InteropServices.COMException("An unexpected error occurred while trying to create a table in the geodatabase", ex);
            }
            // Set the alias
            IClassSchemaEdit classSchemaEdit = (IClassSchemaEdit)table;
            classSchemaEdit.AlterAliasName(GetUnQualifiedName(name));

            // Return the table
            return table;
        }
        public static string GetUnQualifiedName(string browsename)
        {

            if (browsename.Contains("."))
            {

                string[] arName = browsename.Split('.');
                return arName[arName.Length - 1];
            }

            else
            {
                return browsename;
            }
        }
        public static IGeoFeatureLayer TableToLayerJoin(ITable tbl, string tblJoinField, IFeatureLayer layer, string layerJoinField, esriRelCardinality relType, esriJoinType joinType, string relName)
        {
            IMemoryRelationshipClassFactory pMemRelFact = new MemoryRelationshipClassFactoryClass();
            Type memRelClassFactoryType = Type.GetTypeFromProgID("esriGeodatabase.MemoryRelationshipClassFactory");
            IMemoryRelationshipClassFactory memRelClassFactory = (IMemoryRelationshipClassFactory)Activator.CreateInstance(memRelClassFactoryType);
            IGeoFeatureLayer gfLayer = layer as IGeoFeatureLayer;
            IRelationshipClass pRelClass = pMemRelFact.Open(relName ?? $"{((IDataset)tbl).Name}_{layer.Name}", (IObjectClass)tbl, tblJoinField, gfLayer.DisplayFeatureClass, layerJoinField, "forward", "backward", relType);

            IDisplayRelationshipClass pDispRC = (IDisplayRelationshipClass)gfLayer;
            pDispRC.DisplayRelationshipClass(pRelClass, joinType);

            return gfLayer;
        }
        public static void StartEditOp(IWorkspaceEdit2 edit2)
        {
            try
            {
                if (edit2.IsInEditOperation)
                    edit2.AbortEditOperation();
                if (edit2.IsBeingEdited())
                    edit2.StopEditing(false);
                edit2.StartEditing(false);
                edit2.StartEditOperation();
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static void StopEditOp(bool saveEdits, IWorkspaceEdit2 edit2)
        {
            if (edit2.IsInEditOperation)
                edit2.AbortEditOperation();
            if (edit2.IsBeingEdited())
                edit2.StopEditing(saveEdits);
            edit2.StopEditOperation();
            edit2.StopEditing(saveEdits);
        }

        public static ISpatialReference CreateSpatial(int gcsType)
        {
            ISpatialReferenceFactory spatialRefFactory = new SpatialReferenceEnvironmentClass();
            IGeographicCoordinateSystem geoCS = spatialRefFactory.CreateGeographicCoordinateSystem((int)gcsType);
            return (ISpatialReference)geoCS;
        }

        public static ISpatialReference CreateSpatialRefGCS(ESRI.ArcGIS.Geometry.esriSRGeoCSType gcsType)
        {

            ISpatialReferenceFactory spatialRefFactory = new SpatialReferenceEnvironmentClass();
            IGeographicCoordinateSystem geoCS = spatialRefFactory.CreateGeographicCoordinateSystem((int)gcsType);

            return (ISpatialReference)geoCS;
        }


        public static IEnvelope GetExtent(IFeatureLayer PolygonLayer)
        {


            IEnvelope envelope = PolygonLayer.AreaOfInterest.Envelope;
            envelope.Project(CreateSpatialRefGCS(esriSRGeoCSType.esriSRGeoCS_WGS1984));
            return PolygonLayer.AreaOfInterest.Envelope;
        }

        public static IGeoDataset CreateGeoDataset(IFeatureClass featureClass, string fieldName, IQueryFilter filter = null)
        {
            IFeatureClassDescriptor fcDescr = default(IFeatureClassDescriptor);
            fcDescr = new FeatureClassDescriptor() as IFeatureClassDescriptor;
            fcDescr.Create(featureClass, filter, fieldName);
            return fcDescr as IGeoDataset;
        }

        public static IRaster IDW(IWorkspace ws, IGeoDataset ds, double power, double cellSize, IRasterRadius radius = null)
        {
            IInterpolationOp3 interpOp = default(IInterpolationOp3);
            interpOp = new RasterInterpolationOp() as IInterpolationOp3;
            // Create an Op (RasterMaker operator).
            RasterSurfaceOp rasterSurfaceOp = interpOp as RasterSurfaceOp;
            IRasterAnalysisEnvironment rasterAnalysisEnvironment = rasterSurfaceOp;
            // Set output workspace for the Op.
            rasterAnalysisEnvironment.OutWorkspace = ws;
            // Set cell size for the Op.
            object object_cellSize = cellSize;
            rasterAnalysisEnvironment.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, ref object_cellSize);
            if (radius == null)
            {
                radius = new RasterRadius();
                radius.SetVariable(12);
            }
            // create surface
            return (IRaster)(interpOp.IDW(ds, power, radius));
        }

        public static void SetDataSource(string sMxdPath, IWorkspace pWorkspace, IRasterDataset rasterDataset = null, string rasterName = "", string ngayTinh = "", string gioTinh = "")
        {//sTargetPath
            IMapDocument pMapDocument = new MapDocumentClass();
            pMapDocument.Open(sMxdPath, "");
            //IWorkspaceFactory pWorkFactory = new FileGDBWorkspaceFactory() as IWorkspaceFactory2;
            //IWorkspace pWorkspace = pWorkFactory.OpenFromFile(sTargetPath, 0);

            IFeatureWorkspace pFeaClsWks = pWorkspace as IFeatureWorkspace;

            //IFeatureWorkspace pFeaClsWksSever = pWorkspaceServer as IFeatureWorkspace;

            for (int i = 0; i < pMapDocument.MapCount; i++)
            {
                IMap map = pMapDocument.Map[i];
                for (int j = 0; j < map.LayerCount; j++)
                {
                    ILayer layer = map.Layer[j] as IFeatureLayer;
                    ICompositeLayer cpLayer = map.Layer[j] as ICompositeLayer;
                    IRasterLayer rLayer = map.Layer[j] as IRasterLayer;
                    if (rLayer != null)
                    {
                        IRasterLayer pFeaLyr = rLayer as IRasterLayer;
                        pFeaLyr.Name = rasterName;
                        pFeaLyr.CreateFromDataset(rasterDataset);
                    }
                    if (layer != null)
                    {
                        IFeatureLayer pFeaLyr = layer as IFeatureLayer;
                        string sDsName = ((pFeaLyr as IDataLayer).DataSourceName as IDatasetName).Name;
                        if ((pWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, sDsName))
                        {
                            switch (sDsName)
                            {
                                case "NOISE_ARCGIS.DBO.TramQuanTracCoDinh":
                                case "TramQuanTracCoDinh":


                                    IQueryDef qDef = pFeaClsWks.CreateQueryDef();

                                    qDef.WhereClause = string.Format("thoiGianQuanTrac = '{0}' AND ngayQuanTrac = '{1}'", gioTinh, ngayTinh);
                                   
                                    qDef.Tables = "NOISE_ARCGIS.DBO.TramQuanTracCoDinh";

                                    IFeatureDataset featureDataset = pFeaClsWks.OpenFeatureQuery("NOISE_ARCGIS.DBO.TramQuanTracCoDinh", qDef);
                                    IFeatureClassContainer featureClassContainer = (IFeatureClassContainer)featureDataset;
                                    IFeatureClass featureClass = featureClassContainer.get_ClassByName("NOISE_ARCGIS.DBO.TramQuanTracCoDinh");

                                    //IFeatureLayer layerForMap = (IFeatureLayer)featureClass;

                                    pFeaLyr.FeatureClass = featureClass;// pFeaClsWks.OpenFeatureClass(sDsName);
                                    pFeaLyr.Name = featureClass.AliasName;// pFeaLyr.FeatureClass.AliasName;


                                    break;
                                case "NOISE_ARCGIS.DBO.NhaDangVung":
                                case "NOISE_ARCGIS.DBO.TramDienDangVung":
                                case "NOISE_ARCGIS.DBO.KhuChucNangDangVung":
                                case "NOISE_ARCGIS.DBO.NghiaTan_RanhGioi":
                                case "NOISE_ARCGIS.DBO.DuongDiaGioiCapHuyen":
                                case "NOISE_ARCGIS.DBO.DuongDiaGioiCapXa":
                                case "NOISE_ARCGIS.DBO.MatDuongBo":
                                case "NOISE_ARCGIS.DBO.DoanTimDuongBo":
                                case "NhaDangVung":
                                case "TramDienDangVung":
                                case "KhuChucNangDangVung":
                                case "NghiaTan_RanhGioi":
                                case "DuongDiaGioiCapHuyen":
                                case "DuongDiaGioiCapXa":
                                case "MatDuongBo":
                                case "DoanTimDuongBo":
                                    pFeaLyr.FeatureClass = pFeaClsWks.OpenFeatureClass(sDsName);
                                    pFeaLyr.Name = pFeaLyr.FeatureClass.AliasName;
                                    break;
                                default:
                                    pFeaLyr.FeatureClass = pFeaClsWks.OpenFeatureClass(sDsName);
                                    pFeaLyr.Name = pFeaLyr.FeatureClass.AliasName;// + " " + kyTinh;
                                    break;
                            }

                        }
                    }
                    if (cpLayer != null)
                    {
                        for (int k = 0; k < cpLayer.Count; k++)
                        {
                            layer = cpLayer.Layer[k];
                            IFeatureLayer pFeaLyr = cpLayer.Layer[k] as IFeatureLayer;
                            string sDsName = ((pFeaLyr as IDataLayer).DataSourceName as IDatasetName).Name;
                            if ((pWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, sDsName))
                            {
                                pFeaLyr.FeatureClass = pFeaClsWks.OpenFeatureClass(sDsName);
                                switch (sDsName)
                                {
                                    case "DuongDiaGioiCapXa":
                                    case "DuongDiaGioiCapHuyen":
                                    case "DuongDiaGioiCapTinh":
                                        pFeaLyr.Name = pFeaLyr.FeatureClass.AliasName;
                                        break;
                                    default:
                                        pFeaLyr.Name = pFeaLyr.FeatureClass.AliasName;// + " " + kyTinh;
                                        break;
                                }

                            }
                        }
                    }
                }
            }
            pMapDocument.Save(true, true);

            pMapDocument.Close();


            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pMapDocument);
        }
        
        public static void RasterToPolygon(IWorkspace ws, string inFc, string outFc)
        {
            Geoprocessor GP = new Geoprocessor();
            try
            {
                ESRI.ArcGIS.ConversionTools.RasterToPolygon rtl = new ESRI.ArcGIS.ConversionTools.RasterToPolygon();
                rtl.in_raster = ws.PathName + @"\" + inFc;// 
                rtl.out_polygon_features = ws.PathName + @"\" + outFc;
                rtl.raster_field = "VALUE";
                GP.Execute(rtl, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
             {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {
            }
        }
        //
        public static IRasterDataset ReclassifyRasterLayerAndSave(IRasterLayer pRasterLayer, int pClassesNo, string pMethodName, IWorkspace ppWorkspace)//, string strRasterName
        {
            try
            {
                object varNewValues, varNewCounts;//declare variable to store pixel values
                int mClassesNo = 4;//declare reclassify count
                IClassifyGEN mClassify =  new EqualIntervalClass(); ;

                //mClassesNo = Convert.ToInt16(this.txtClassifyCount.Text.Trim());

                IRaster pRaster = pRasterLayer.Raster;
                IRasterProps rasterProps = (IRasterProps)pRaster;
                //Define the dictionary collection to store pixel values ​​and statistical frequencies

                Dictionary<double, long> ValueFrequence = new Dictionary<double, long>();

                //Set the starting point of raster data  
                IPnt pBlockSize = new Pnt();
                pBlockSize.SetCoords(rasterProps.Width, rasterProps.Height);
                //Select the entire range  
                IPixelBlock pPixelBlock = pRaster.CreatePixelBlock(pBlockSize);
                //The coordinates of the upper left point  
                IPnt tlp = new Pnt();
                tlp.SetCoords(0, 0);
                //Read into the grid  
                IRasterBandCollection pRasterBands = pRaster as IRasterBandCollection;
                IRasterBand pRasterBand = pRasterBands.Item(0);
                IRawPixels pRawRixels = pRasterBands.Item(0) as IRawPixels;
                pRawRixels.Read(tlp, pPixelBlock);


                //Group the values ​​of PixBlock into an array  
                //System.Array pSafeArray = pPixelBlock.get_SafeArray(0) as System.Array;

                //for (int y = 0; y < rasterProps.Height; y++)
                //{
                //    for (int x = 0; x < rasterProps.Width; x++)
                //    {
                //        double value = Convert.ToDouble(Convert.ToDouble(pSafeArray.GetValue(x, y)).ToString("##.0000000000"));


                //        if (!ValueFrequence.ContainsKey(value))
                //        {
                //            ValueFrequence.Add(value, 1);
                //        }
                //        else if (ValueFrequence.ContainsKey(value))
                //        {
                //            ValueFrequence[value] = ValueFrequence[value] + 1;
                //        }


                //    }
                //}

                //DataTable dt = initialDatatable();

                //foreach (double dValue in ValueFrequence.Keys)
                //{
                //    DataRow pRow = dt.NewRow();
                //    pRow["PixelValue"] = dValue;
                //    pRow["PixelFrequence"] = long.Parse(ValueFrequence[dValue].ToString());
                //    dt.Rows.Add(pRow);
                //}

                //Sorting is very important
                //dt.DefaultView.Sort = "PixelValue asc,PixelFrequence";

                //DataTable ddt = dt.DefaultView.ToTable();
                //int iCount = ddt.Rows.Count;

                //double[] dValues = new double[iCount];
                //long[] lFrequency = new long[iCount];

                //for (int ii = 0; ii < iCount; ii++)
                //{
                //    dValues[ii] = Convert.ToDouble(ddt.Rows[ii]["PixelValue"].ToString());
                //    lFrequency[ii] = long.Parse(ddt.Rows[ii]["PixelFrequence"].ToString());
                //}

                //varNewValues = dValues as object;
                //varNewCounts = lFrequency as object;


                //if (mClassify == null) return;

                ////mClassify is the IClassifyGEN interface object,
                ////Initialize through its subclasses to achieve mClassify = new NaturalBreaksClass();
                ////Classify (varNewValues--array of pixel values; varNewCounts--array of corresponding values;
                ////pClassesNo--Classification level)
                //mClassify.Classify(varNewValues, varNewCounts, ref pClassesNo);

                ////Classification is complete, get a breakpoint
                //double[] ClassNum = (double[])mClassify.ClassBreaks;

                IRasterDescriptor pRD = new RasterDescriptorClass();
                IReclassOp pReclassOp = new RasterReclassOpClass();
                IGeoDataset pGeodataset = pRaster as IGeoDataset;//pRasterLayer.Raster 

                //IRasterAnalysisEnvironment pEnv = pReclassOp as IRasterAnalysisEnvironment;
                //pEnv.OutWorkspace = pWorkspace;
                //object objSnap = null;
                //object objExtent = pGeodataset.Extent;
                //pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, ref objExtent, ref objSnap);
                //pEnv.OutSpatialReference = pGeodataset.SpatialReference;


                IRasterLayer pRLayer = new RasterLayerClass();

                //INumberRemap pNumRemap = new NumberRemapClass();
                //for (int i = 0; i < mClassesNo; i++)
                //{
                //    pNumRemap.MapRange(ClassNum[i], ClassNum[i + 1], i + 1);
                //}
                //IRemap pRemap = pNumRemap as IRemap;

                //IRaster pOutRaster =  pReclassOp.Reclass(pGeodataset, pRemap, ) as IRaster;

                INumberRemap pNumRemap = new NumberRemapClass();
                pNumRemap.MapRange(0, 45,1);
                pNumRemap.MapRange(45.000001, 55, 2);
                pNumRemap.MapRange(55.000001, 70, 3);
                pNumRemap.MapRange(70, 120, 4);
                IRemap pRemap = pNumRemap as IRemap;

                IRaster pOutRaster = pReclassOp.ReclassByRemap(pGeodataset, pRemap, false) as IRaster;

                pRLayer.CreateFromRaster(pOutRaster);

                IRasterBandCollection pRasterBandCol = pOutRaster as IRasterBandCollection;
                IRasterBand pBand = pRasterBandCol.Item(0);
                IRasterDataset pOutRasterDataset = pBand.RasterDataset;

                //IWorkspaceFactory pp = new FileGDBWorkspaceFactoryClass();
                //IWorkspace ppWorkspace = pp.OpenFromFile(strOutputPath, 0);

                return pOutRasterDataset;

              
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }

        }

        private static DataTable initialDatatable()
        {
            try
            {
                DataTable dt = new DataTable();
                DataColumn col = new DataColumn();
                col.ColumnName = "PixelValue";
                col.DataType = System.Type.GetType("System.Double");
                dt.Columns.Add(col);

                col = new DataColumn();
                col.ColumnName = "PixelFrequence";
                col.DataType = System.Type.GetType("System.Int64");
                dt.Columns.Add(col);

                return dt;
            }
            catch
            {
                return null;
            }
        }

        public static void MakeFeatureLayer(IWorkspace ws, object inFc, string outFc)
        {
            Geoprocessor GP = new Geoprocessor();
            try
            {
                ESRI.ArcGIS.DataManagementTools.MakeFeatureLayer make = new ESRI.ArcGIS.DataManagementTools.MakeFeatureLayer();
                make.in_features = inFc;//ws.PathName + @"\" + inFc;
                make.out_layer = outFc;
                make.workspace = ws;
                GP.Execute(make, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {
            }

            //return (ws as IFeatureWorkspace).OpenFeatureClass($"{outFc}");
        }

        public static void SelectByAttribute(IWorkspace ws, object pInputFeatureClass, string sqlQuery)// string pInputFeatureClass, IFeatureClass outName
        {

            Geoprocessor GP = new Geoprocessor();
            IFeatureWorkspace ws2 = (IFeatureWorkspace)ws;

            try
            {
                ESRI.ArcGIS.DataManagementTools.SelectLayerByAttribute selectByAttr = new ESRI.ArcGIS.DataManagementTools.SelectLayerByAttribute();
                selectByAttr.in_layer_or_view = pInputFeatureClass;
                selectByAttr.where_clause = sqlQuery;
                selectByAttr.selection_type = "NEW_SELECTION";
                GP.Execute(selectByAttr, null);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                for (int i = 0; i < GP.MessageCount; i++)
                {
                    string a = GP.GetMessage(i);
                }
            }
            catch (Exception)
            {
            }

            //return (ws as IFeatureWorkspace).OpenFeatureClass($"{outName}");
        }


        public static bool ExportLayerToShapefile(
    string shapePath,
    string shapeName,
    IFeatureLayer sfeatlayer)//source,
                             //out IEnumFieldError fieldErrors,
                             //out IEnumInvalidObject invalidObjects)
        {
            //IGeoFeatureLayer sourceFeatLayer = (IGeoFeatureLayer)source;
            //IFeatureLayer sfeatlayer = (IFeatureLayer)sourceFeatLayer;
            IFeatureClass sfeatClass = sfeatlayer.FeatureClass;
            IDataset sdataset = (IDataset)sfeatClass;
            IDatasetName sdatasetName = (IDatasetName)sdataset.FullName;

            ISelectionSet sSelectionSet = (
                (ITable)sfeatClass).Select(new QueryFilter(),
                esriSelectionType.esriSelectionTypeHybrid,
                esriSelectionOption.esriSelectionOptionNormal,
                sdataset.Workspace);

            IWorkspaceFactory factory;
            factory = new ShapefileWorkspaceFactory();
            IWorkspace targetWorkspace = factory.OpenFromFile(shapePath, 0);
            IDataset targetDataset = (IDataset)targetWorkspace;

            IName targetWorkspaceName = targetDataset.FullName;
            IWorkspaceName tWorkspaceName = (IWorkspaceName)targetWorkspaceName;

            IFeatureClassName tFeatClassname = (IFeatureClassName)new FeatureClassName();
            IDatasetName tDatasetName = (IDatasetName)tFeatClassname;
            tDatasetName.Name = shapeName;
            tDatasetName.WorkspaceName = tWorkspaceName;

            IFieldChecker fieldChecker = new FieldChecker();
            IFields sFields = sfeatClass.Fields;
            IFields tFields = null;

            fieldChecker.InputWorkspace = sdataset.Workspace;
            fieldChecker.ValidateWorkspace = targetWorkspace;
            IEnumFieldError fieldErrors = null;
            fieldChecker.Validate(sFields, out fieldErrors, out tFields);
            if (fieldErrors != null)
            {
                IFieldError fieldError = null;
                while ((fieldError = fieldErrors.Next()) != null)
                {
                    Console.WriteLine(fieldError.FieldError + " : " + fieldError.FieldIndex);
                }
                Console.WriteLine("[ExportDataViewModel.cs] Errors encountered during field validation");
            }

            string shapefieldName = sfeatClass.ShapeFieldName;
            int shapeFieldIndex = sfeatClass.FindField(shapefieldName);
            IField shapefield = sFields.get_Field(shapeFieldIndex);
            IGeometryDef geomDef = shapefield.GeometryDef;
            IClone geomDefClone = (IClone)geomDef;
            IClone targetGeomDefClone = geomDefClone.Clone();
            IGeometryDef tGeomDef = (IGeometryDef)targetGeomDefClone;

            IFeatureDataConverter2 featDataConverter = (IFeatureDataConverter2)new FeatureDataConverter();
            IEnumInvalidObject invalidObjects = featDataConverter.ConvertFeatureClass(
                sdatasetName,
                null,
                sSelectionSet,
                null,
                tFeatClassname,
                tGeomDef,
                tFields,
                "",
                1000, 0);

            string fullpath = System.IO.Path.Combine(shapePath, shapeName);
            return System.IO.File.Exists(fullpath);
        }

        public static bool DoesRasterExist(IWorkspace workspace, string rasterName, bool isNameFullyQualified)
        {
            IEnumDatasetName eDSNames = workspace.get_DatasetNames(esriDatasetType.esriDTRasterDataset);
            IDatasetName DSName = eDSNames.Next();
            while (DSName != null)
            {
                if (!isNameFullyQualified)
                {
                    string[] nameParts = DSName.Name.Split('.');
                    if (nameParts[nameParts.Length - 1].ToLower() == rasterName.ToLower())
                        return true;
                }
                else
                {
                    if (DSName.Name.ToLower() == rasterName.ToLower())
                        return true;
                }
                DSName = eDSNames.Next();
            }

            return false;
        }

       
    }
}
