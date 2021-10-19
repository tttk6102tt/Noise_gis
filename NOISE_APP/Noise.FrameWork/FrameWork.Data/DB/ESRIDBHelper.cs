using System;
using System.Configuration;
using System.Text;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork.Data.DB
{
    public class ESRIDBHelper
    {
        private ESRIDBHelper()
        {
            Init();
        }

        public static ESRIDBHelper Instance
        {
            get
            {
                return Nested.ESRIDBHelper;
            }
        }
        private GeoDB _geoDB;
        public GeoDB GeoDatabase
        {
            get { return _geoDB; }
        }
        private class Nested
        {
            static Nested() { }
            internal static readonly ESRIDBHelper ESRIDBHelper = new ESRIDBHelper();
        }

        private void Init()
        {
            if (_geoDB == null)
            {
                string DBType = ConfigurationSettings.AppSettings["ESRIDB"];
                IPropertySet propertySet = new PropertySetClass();
                IWorkspace ws;
                IWorkspaceFactory wsf;
                IFeatureWorkspace featureWS = null;
                if (DBType == "SDE")
                {
                    propertySet.SetProperty("SERVER", ConfigurationSettings.AppSettings["SERVER"]);
                    propertySet.SetProperty("INSTANCE", ConfigurationSettings.AppSettings["INSTANCE"]);
                    propertySet.SetProperty("DATABASE", ConfigurationSettings.AppSettings["DATABASE"]);
                    propertySet.SetProperty("USER", ConfigurationSettings.AppSettings["USER"]);
                    propertySet.SetProperty("PASSWORD", ConfigurationSettings.AppSettings["PASSWORD"]);
                    propertySet.SetProperty("VERSION", ConfigurationSettings.AppSettings["VERSION"]);
                    wsf = new SdeWorkspaceFactoryClass();
                    ws = wsf.Open(propertySet, 0);
                    featureWS = ws as IFeatureWorkspace;
                }
                else if (DBType == "MDB")
                {
                    propertySet.SetProperty("DATABASE", ConfigurationSettings.AppSettings["DATABASE"]);
                    wsf = new AccessWorkspaceFactoryClass();
                    ws = wsf.Open(propertySet, 0);
                    featureWS = ws as IFeatureWorkspace;
                }
                else if (DBType == "SHP")
                {
                    propertySet.SetProperty("DATABASE", ConfigurationSettings.AppSettings["DATABASE"]);
                    wsf = new ShapefileWorkspaceFactoryClass();
                    ws = wsf.Open(propertySet, 0);
                    featureWS = ws as IFeatureWorkspace;
                }
                else if (DBType == "FGDB")
                {
                }
                _geoDB = new GeoDB(featureWS);
            }

        }

        public IFeatureLayer GetFeatureLayer(string sFtrName, IWorkspace pWorkspc)
        {
            try
            {
                IFeatureWorkspace pFeatWsp = (IFeatureWorkspace)pWorkspc;
                IFeatureClass pFeatCls = pFeatWsp.OpenFeatureClass(sFtrName);
                IFeatureLayer pFeatLyr = new FeatureLayerClass();
                pFeatLyr.FeatureClass = pFeatCls;
                return pFeatLyr;
            }
            catch
            {
                return null;
            }
        }

        public IWorkspace OpenSDEDBWorkspace(string server, string UserName, string Password, string Database)
        {
            IPropertySet pPropertySet = new PropertySetClass();
            IWorkspaceFactory pWorkspaceFactory = new SdeWorkspaceFactoryClass();
            //pPropertySet.SetProperty("dbclient", "SQLServer");
            //pPropertySet.SetProperty("serverinstance", "117.4.242.159");
            //pPropertySet.SetProperty("database", Database);
            //pPropertySet.SetProperty("user", UserName);
            //pPropertySet.SetProperty("password", Password);
            //pPropertySet.SetProperty("VERSION", "sde.default");
            //pPropertySet.SetProperty("authentication_mode", "RDBMS");

            pPropertySet.SetProperty("SERVER", server);
            pPropertySet.SetProperty("INSTANCE", "sde:sqlserver:" + server);  //< db type > is like "sqlserver"
            pPropertySet.SetProperty("DATABASE", Database);
            pPropertySet.SetProperty("VERSION", "dbo.Default");

            //and either
            pPropertySet.SetProperty("USER", UserName);
            pPropertySet.SetProperty("PASSWORD", Password);

            try
            {
                IWorkspace lWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
                


                if (lWorkspace == null)
                {
                    //VIDAMessageBox.ShowInfoMessage("Can not open database.");
                    return null;
                }
                return lWorkspace;


            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public void ClassBreakRenderer(ILayer pLayer, int numClass, string FieldName, AxMapControl axMapControl)
        {
            IGeoFeatureLayer m_pGeoFeatureLayer = (IGeoFeatureLayer)pLayer;
            ITable pTable;
            IClassifyGEN pClassify;
            ITableHistogram pTableHistogram1;
            //ESRI.ArcGIS.CartoUI.IHistogram pHistogram1;
            IBasicHistogram pBasic = new BasicTableHistogramClass();
            object dataFrequency, dataValues;

            //We're going to retrieve frequency data from a population field
            //and then classify this data
            pTable = (ITable)m_pGeoFeatureLayer;
            pTableHistogram1 = new BasicTableHistogramClass();
            //pHistogram1 = (IHistogram)pTableHistogram1;
            pBasic = (IBasicHistogram)pTableHistogram1;
            //Get values and frequencies for the population field
            //into a table histogram object
            pTableHistogram1.Field = FieldName;
            pTableHistogram1.Table = pTable;
            //pHistogram1.GetHistogram(out dataValues,out dataFrequency);
            pBasic.GetHistogram(out dataValues, out dataFrequency);

            //Put the values and frequencies into an Equal Interval classify object
            pClassify = new EqualIntervalClass();
            //pClassify.SetHistogramData(dataValues, dataFrequency);

            //Now a generate the classes
            //Note:
            //1/ The number of classes returned may be different from requested
            //(depends on classification algorithm)
            //2/ The classes array starts at index 0 and has datavalues starting
            //from the minumum value, going to maximum

            Double[] Classes;
            int ClassesCount;
            int numDesiredClasses = numClass;
            pClassify.Classify(dataValues, dataFrequency, ref numDesiredClasses);
            Classes = (Double[])pClassify.ClassBreaks;
            ClassesCount = Classes.Length;

            //Initialise a new class breaks renderer and supply the number of
            //class breaks and the field to perform the class breaks on.

            IClassBreaksRenderer pClassBreaksRenderer = new ClassBreaksRendererClass();
            pClassBreaksRenderer.Field = FieldName;
            pClassBreaksRenderer.BreakCount = ClassesCount;
            pClassBreaksRenderer.SortClassesAscending = true;


            //Use an algorithmic color ramp to generate an range of colors between
            //yellow to red (taken from ArcMaps colorramp properties)

            //Set the initial color to yellow
            IHsvColor pFromColor = new HsvColor();
            pFromColor.Hue = 0; //Yellow
            pFromColor.Saturation = 100;
            pFromColor.Value = 96;

            //Set the final color to be red
            IHsvColor pToColor = new HsvColor();
            pToColor.Hue = 60; //Red
            pToColor.Saturation = 100;
            pToColor.Value = 96;

            //Set up the HSV colour ramp to span from yellow to red
            IAlgorithmicColorRamp pRamp = new AlgorithmicColorRamp();
            IEnumColors pEnumColors;
            bool ok = true;
            pRamp = new AlgorithmicColorRamp();
            pRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            pRamp.FromColor = pFromColor;
            pRamp.ToColor = pToColor;
            pRamp.Size = ClassesCount + 2;
            pRamp.CreateRamp(out ok);
            pEnumColors = pRamp.Colors;
            pEnumColors.Reset();

            //Iterate through each class brake, setting values and corresponding
            //fill symbols for each polygon, note we skip the minimum value (classes(0))
            IColor pColor;
            int breakIndex;
            SimpleMarkerSymbol pMarkerSymbol;
            for (breakIndex = 0; breakIndex < ClassesCount - 1; breakIndex++)
            {
                pColor = pEnumColors.Next();
                pMarkerSymbol = new SimpleMarkerSymbol();
                pMarkerSymbol.Color = pColor;
                pMarkerSymbol.Size = 8;

                pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pMarkerSymbol);
                pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);
                //Store each break value for user output
            }

            //Assign the renderer to the layer and update the display
            m_pGeoFeatureLayer.Renderer = (IFeatureRenderer)pClassBreaksRenderer;

            //axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }
        public void ClassBreakRenderer(ILayer pLayer, int numClass, string FieldName, IColor fromColor, IColor toColor, AxMapControl axMapControl) //,int[,] RGBColor)
        {
            IGeoFeatureLayer m_pGeoFeatureLayer = (IGeoFeatureLayer)pLayer;
            ITable pTable;
            IClassifyGEN pClassify;
            ITableHistogram pTableHistogram1;
            IBasicHistogram pHistogram1;
            object dataFrequency, dataValues;

            //We're going to retrieve frequency data from a population field
            //and then classify this data
            pTable = (ITable)m_pGeoFeatureLayer;
            pTableHistogram1 = new BasicTableHistogramClass();
            pHistogram1 = (IBasicHistogram)pTableHistogram1;

            //Get values and frequencies for the population field
            //into a table histogram object
            pTableHistogram1.Field = FieldName;
            pTableHistogram1.Table = pTable;
            pHistogram1.GetHistogram(out dataValues, out dataFrequency);

            //Put the values and frequencies into an Equal Interval classify object
            pClassify = new EqualIntervalClass();
            //pClassify.SetHistogramData(dataValues, dataFrequency);

            //Now a generate the classes
            //Note:
            //1/ The number of classes returned may be different from requested
            //(depends on classification algorithm)
            //2/ The classes array starts at index 0 and has datavalues starting
            //from the minumum value, going to maximum

            Double[] Classes;
            int ClassesCount;
            int numDesiredClasses = numClass;
            pClassify.Classify(dataValues, dataFrequency, ref numDesiredClasses);
            Classes = (Double[])pClassify.ClassBreaks;
            ClassesCount = Classes.Length;

            //Initialise a new class breaks renderer and supply the number of
            //class breaks and the field to perform the class breaks on.

            IClassBreaksRenderer pClassBreaksRenderer = new ClassBreaksRendererClass();
            pClassBreaksRenderer.Field = FieldName;
            pClassBreaksRenderer.BreakCount = ClassesCount;
            pClassBreaksRenderer.SortClassesAscending = true;

            //Set up the HSV colour ramp to span from yellow to red
            IAlgorithmicColorRamp pRamp = new AlgorithmicColorRamp();
            IEnumColors pEnumColors;
            //			bool ok;
            //
            //			pRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            //			pRamp.FromColor = fromColor;
            //			pRamp.ToColor = toColor;
            //			pRamp.Size = ClassesCount;
            //			pRamp.CreateRamp(out ok);
            //			pEnumColors = pRamp.Colors;

            bool ok = false;
            pRamp = new AlgorithmicColorRamp();
            pRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            pRamp.FromColor = fromColor;
            pRamp.ToColor = toColor;
            pRamp.Size = ClassesCount + 1;
            pRamp.CreateRamp(out ok);
            pEnumColors = pRamp.Colors;
            pEnumColors.Reset();

            //Iterate through each class brake, setting values and corresponding
            //fill symbols for each polygon, note we skip the minimum value (classes(0))
            IColor pColor;
            int breakIndex;
            SimpleMarkerSymbol pMarkerSymbol;
            for (breakIndex = 0; breakIndex < ClassesCount - 1; breakIndex++)
            {
                pColor = pEnumColors.Next();
                pMarkerSymbol = new SimpleMarkerSymbol();
                pMarkerSymbol.Color = pColor;
                pMarkerSymbol.Size = 8;

                pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pMarkerSymbol);
                pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);
                //Store each break value for user output
            }
            //Assign the renderer to the layer and update the display
            m_pGeoFeatureLayer.Renderer = (IFeatureRenderer)pClassBreaksRenderer;

            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        public ITable GetITable(string tblName, IWorkspace wsp)
        {
            if (wsp == null) return null;
            IFeatureWorkspace pFeatureWorkspace;
            pFeatureWorkspace = (IFeatureWorkspace)wsp;
            ITable pTable;
            pTable = pFeatureWorkspace.OpenTable(tblName);
            return pTable;
        }
        public ITable CreateQueryTable(IWorkspace workspace, IQueryDef queryDef, String tableName)
        {
            // Create a reference to a TableQueryName object.
            IQueryName2 queryName2 = new TableQueryNameClass();
            queryName2.PrimaryKey = "";

            // Specify the query definition.
            queryName2.QueryDef = queryDef;

            // Get a name object for the workspace.
            IDataset dataset = (IDataset)workspace;
            IWorkspaceName workspaceName = (IWorkspaceName)dataset.FullName;

            // Cast the TableQueryName object to the IDatasetName interface and open it.
            IDatasetName datasetName = (IDatasetName)queryName2;
            datasetName.WorkspaceName = workspaceName;
            datasetName.Name = tableName;
            IName name = (IName)datasetName;

            // Open the name object and get a reference to a table object.
            ITable table = (ITable)name.Open();
            return table;
        }

        public DataTable ConvertITableToDataTable(ITable tbl, string tblName, ArrayList isArray)
        {
            // Create a new DataTable titled 'tempNames.'
            DataTable pDataTable = new DataTable(tblName);
            //IClass cls = (IClass) tbl;
            IStandaloneTable pSTable = new StandaloneTableClass();
            pSTable.Table = tbl;
            //ITableFields tableFields = tbl as ITableFields;
            ITableFields tableFields = (ITableFields)pSTable;
            try
            {

                for (int i = 0; i < tableFields.FieldCount; i++)
                {
                    IField pField = tableFields.get_Field(i);
                    string fieldName = pField.Name.ToString();
                    foreach (string stGetValue in isArray)
                    {
                        if (fieldName == stGetValue)
                        {

                            if (fieldName.Length >= 5)
                            {
                                //Do not add columns with name = Shape
                                if (string.Compare(fieldName.Substring(0, 5).ToUpper(), "SHAPE") == 0)
                                {
                                }
                                else
                                {
                                    DataColumn col = new DataColumn();
                                    col.DataType = GetSystemFieldType(pField.Type);
                                    col.ColumnName = fieldName;
                                    pDataTable.Columns.Add(col);
                                }
                            }

                            else
                            {
                                DataColumn col = new DataColumn();
                                col.DataType = GetSystemFieldType(pField.Type);
                                col.ColumnName = fieldName;
                                pDataTable.Columns.Add(col);
                            }
                            break;
                        }
                    }
                }

                ICursor pCursor = tbl.Search(null, false);
                IRow pRow = pCursor.NextRow();
                while (pRow != null)
                {
                    DataRow pDataRow = pDataTable.NewRow();
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                        string sFieldName = pRow.Fields.get_Field(i).Name;
                        foreach (string stGetValue in isArray)
                        {
                            if (sFieldName == stGetValue)
                            {
                                if (sFieldName.Length >= 5)
                                {
                                    if (string.Compare(sFieldName.Substring(0, 5).ToUpper(), "SHAPE") == 0)
                                    {
                                    }
                                    else
                                    {
                                        pDataRow[stGetValue] = pRow.get_Value(pRow.Fields.FindField(stGetValue));
                                    }
                                }
                                else
                                {
                                    pDataRow[stGetValue] = pRow.get_Value(pRow.Fields.FindField(stGetValue));
                                }
                                //i ++  ;
                                break;
                            }
                        }
                    }
                    pDataTable.Rows.Add(pDataRow);
                    pRow = pCursor.NextRow();
                }
                pDataTable.AcceptChanges();

            }
            catch (Exception ex) { throw ex; }

            // Return the new DataTable.
            return pDataTable;

        }

        private System.Type GetSystemFieldType(esriFieldType type)
        {
            if (type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDate)
            {
                return System.Type.GetType("System.DateTime");
            }
            else if (type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
            {
                return System.Type.GetType("System.Double");
            }
            else if (type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSingle)
            {
                return System.Type.GetType("System.Double");
            }
            else if (type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger)
            {
                return System.Type.GetType("System.Int32");
            }
            else if (type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger)
            {
                return System.Type.GetType("System.Int32");
            }
            else if (type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID)
            {
                return System.Type.GetType("System.Int32");
            }
            else if (type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString)
            {
                return System.Type.GetType("System.String");
            }
            else
            {
                return System.Type.GetType("System.String");
            }
        }


        private esriFieldType GetEsriFieldType(System.Type type)
        {
            if (type == System.Type.GetType("System.String"))
            {
                return esriFieldType.esriFieldTypeString;
            }
            else if (type == System.Type.GetType("System.Double"))
            {
                return esriFieldType.esriFieldTypeDouble;
            }
            else if (type == System.Type.GetType("System.Decimal"))
            {
                return esriFieldType.esriFieldTypeDouble;
            }
            else if (type == System.Type.GetType("System.DateTime"))
            {
                return esriFieldType.esriFieldTypeDate;
            }
            else if (type == System.Type.GetType("System.Int16"))
            {
                return esriFieldType.esriFieldTypeInteger;
            }
            else if (type == System.Type.GetType("System.Int32"))
            {
                return esriFieldType.esriFieldTypeInteger;
            }
            else if (type == System.Type.GetType("System.Int64"))
            {
                return esriFieldType.esriFieldTypeInteger;
            }
            else if (type == System.Type.GetType("System.Single"))
            {
                return esriFieldType.esriFieldTypeSingle;
            }
            else
            {
                return esriFieldType.esriFieldTypeString;
            }
        }
        public ITable ConvertDataTableToITable(DataTable tbl)
        {
            ITable pTable = null;
            IRowBuffer pRowBuffer = null;
            ICursor pCursor = null;
            IScratchWorkspaceFactory pWrkSpFact = null;
            IFeatureWorkspace pWorkSpace = null;
            UID pCLSID = null;
            IFieldEdit pFieldEdit = null;
            IFieldsEdit pFieldsEdit = null;
            try
            {
                pTable = new StandaloneTableClass();
                //create scratch workspace to store table
                pWrkSpFact = new ScratchWorkspaceFactory();
                pWorkSpace = pWrkSpFact.CreateNewScratchWorkspace() as IFeatureWorkspace;
                pCLSID = new UID();
                pCLSID.Value = "esricore.Object";
                pFieldsEdit = new FieldsClass();
                pFieldsEdit.FieldCount_2 = tbl.Columns.Count;
                //add fields to table
                foreach (DataColumn dc in tbl.Columns)
                {
                    pFieldEdit = new FieldClass();
                    pFieldEdit.AliasName_2 = dc.ColumnName.Replace(" ", "_");
                    pFieldEdit.Name_2 = dc.ColumnName.Replace(" ", "_");
                    pFieldEdit.Type_2 = GetEsriFieldType(dc.DataType);
                    pFieldsEdit.set_Field(dc.Ordinal, pFieldEdit);
                }
                //create table
                pTable = pWorkSpace.CreateTable(tbl.TableName, pFieldsEdit, pCLSID, null, "");
                //add rows to table
                pRowBuffer = pTable.CreateRowBuffer();
                pCursor = pTable.Insert(true);
                foreach (DataRow dr in tbl.Rows)
                {
                    for (int x = 0; x < tbl.Columns.Count; x++) //foreach column set values
                    {
                        pRowBuffer.set_Value(x, dr[x]);
                    }
                    pCursor.InsertRow(pRowBuffer);
                }
                //update table
                pCursor.Flush();
                return pTable;
            }
            catch
            {
                return null;
            }
            finally
            {
                pTable = null;
                pRowBuffer = null;
                pCursor = null;
                pWrkSpFact = null;
                pWorkSpace = null;
                pCLSID = null;
                pFieldEdit = null;
                pFieldsEdit = null;
            }
        }



        public IGeoFeatureLayer DefineUniqueValueRendererWithUniqueColors(IFeatureLayer featureLayer, string fieldName)
        {

            //Create the renderer.
            IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();

            //ISimpleLineSymbol pDefaultSimpleLineSymbol = new SimpleLineSymbol();
            //pDefaultSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            //pDefaultSimpleLineSymbol.Width = 0.4;


            ISimpleMarkerSymbol pDefaultSimpleLineSymbol = new SimpleMarkerSymbol();
            pDefaultSimpleLineSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            pDefaultSimpleLineSymbol.Size = 5;

            //These properties should be set prior to adding values.

            pUniqueValueRenderer.FieldCount = 1;
            pUniqueValueRenderer.set_Field(0, fieldName);
            pUniqueValueRenderer.DefaultSymbol = pDefaultSimpleLineSymbol as ISymbol;
            pUniqueValueRenderer.UseDefaultSymbol = true;
            pUniqueValueRenderer.DefaultLabel = "Tất cả";

            IDisplayTable pDisplayTable = featureLayer as IDisplayTable;
            IFeatureCursor pFeatureCursor = pDisplayTable.SearchDisplayTable(null, false) as
                IFeatureCursor;
            IFeature pFeature = pFeatureCursor.NextFeature();


            bool ValFound;
            int fieldIndex;

            //IFields pFields = pFeatureCursor.Fields;
            //fieldIndex = pFields.FindField(fieldName);
            fieldIndex = pFeatureCursor.FindField(fieldName);
            while (pFeature != null)
            {
                //ISimpleLineSymbol pClassSymbol = new SimpleLineSymbol();
                //pClassSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                //pClassSymbol.Width = 0.4;

                ISimpleMarkerSymbol pClassSymbol = new SimpleMarkerSymbol();
                pClassSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                pClassSymbol.Size = 5;

                string classValue;
                classValue = pFeature.get_Value(fieldIndex) as string;

                //Test to see if this value was added to the renderer. If not, add it.
                ValFound = false;
                for (int i = 0; i <= pUniqueValueRenderer.ValueCount - 1; i++)
                {
                    if (pUniqueValueRenderer.get_Value(i) == classValue)
                    {
                        ValFound = true;
                        break; //Exit the loop if the value was found.
                    }
                }
                //If the value was not found, it's new and will be added.
                if (ValFound == false)
                {
                    pUniqueValueRenderer.AddValue(classValue, fieldName, pClassSymbol as
                        ISymbol);
                    pUniqueValueRenderer.set_Label(classValue, classValue);
                    pUniqueValueRenderer.set_Symbol(classValue, pClassSymbol as ISymbol);
                }
                pFeature = pFeatureCursor.NextFeature();
            }
            //Since the number of unique values is known, the color ramp can be sized and the colors assigned.

            int seed = 0;
            List<int> colorList = new List<int>();
            IColor pColor = new RgbColor();

            for (int j = 0; j <= pUniqueValueRenderer.ValueCount - 1; j++)
            {
                string xv;
                xv = pUniqueValueRenderer.get_Value(j);
                if (xv != "")
                {
                    //ISimpleLineSymbol pSimpleLineSymbol = pUniqueValueRenderer.get_Symbol(xv)
                    //    as ISimpleLineSymbol;
                    ISimpleMarkerSymbol pSimpleLineSymbol = pUniqueValueRenderer.get_Symbol(xv) as ISimpleMarkerSymbol;

                    //EDITED: Create unique color from random value.
                    int colorValue = 0;
                    while (true)
                    {

                        Random rnd = new Random(seed);
                        seed++;

                        colorValue = rnd.Next(0x00000000, 0x00FFFFFF);
                        if (colorList.Contains(colorValue) == false)
                        {

                            colorList.Add(colorValue);
                            break;
                        }
                        else
                        {

                            double a = 0;
                        }
                    }

                    pColor.RGB = colorValue;

                    pSimpleLineSymbol.Color = pColor;
                    pUniqueValueRenderer.set_Symbol(xv, pSimpleLineSymbol as ISymbol);

                }
            }

            //'** If you didn't use a predefined color ramp in a style, use "Custom" here. 
            //'** Otherwise, use the name of the color ramp you selected.
            pUniqueValueRenderer.ColorScheme = "Custom";
            ITable pTable = pDisplayTable as ITable;
            bool isString = pTable.Fields.get_Field(fieldIndex).Type ==
                esriFieldType.esriFieldTypeString;
            pUniqueValueRenderer.set_FieldType(0, isString);
            (featureLayer as IGeoFeatureLayer).Renderer = pUniqueValueRenderer as IFeatureRenderer;

            ////This makes the layer properties symbology tab show the correct interface.
            //IUID pUID = new UIDClass();
            //pUID.Value = "{683C994E-A17B-11D1-8816-080009EC732A}";
            //pGeoFeatureLayer.RendererPropertyPageClassID = pUID as UIDClass;
            //IFeatureLayer result = (IFeatureLayer)pGeoFeatureLayer;
            return featureLayer as IGeoFeatureLayer;
        }

        public void DefineUniqueValueRenderer(IGeoFeatureLayer pGeoFeatureLayer, string fieldName)
        {

            IRandomColorRamp pRandomColorRamp = new RandomColorRamp();
            //Create the color ramp for the symbols in the renderer.
            pRandomColorRamp.MinSaturation = 0;
            pRandomColorRamp.MaxSaturation = 40;
            pRandomColorRamp.MinValue = 0;
            pRandomColorRamp.MaxValue = 100;
            pRandomColorRamp.StartHue = 76;
            pRandomColorRamp.EndHue = 188;
            pRandomColorRamp.UseSeed = true;
            pRandomColorRamp.Seed = 43;

            //Create the renderer.
            IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();

            ISimpleMarkerSymbol pSimpleFillSymbol = new SimpleMarkerSymbol();
            pSimpleFillSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            pSimpleFillSymbol.Size = 1;

            //These properties should be set prior to adding values.
            pUniqueValueRenderer.FieldCount = 1;
            pUniqueValueRenderer.set_Field(0, fieldName);
            pUniqueValueRenderer.DefaultSymbol = pSimpleFillSymbol as ISymbol;
            pUniqueValueRenderer.UseDefaultSymbol = true;

            IDisplayTable pDisplayTable = pGeoFeatureLayer as IDisplayTable;
            IFeatureCursor pFeatureCursor = pDisplayTable.SearchDisplayTable(null, false) as
                IFeatureCursor;
            IFeature pFeature = pFeatureCursor.NextFeature();


            bool ValFound;
            int fieldIndex;

            //IFields pFields = pFeatureCursor.Fields;
            //fieldIndex = pFields.FindField(fieldName);
            fieldIndex = pFeatureCursor.FindField(fieldName);
            while (pFeature != null)
            {
                ISimpleMarkerSymbol pClassSymbol = new SimpleMarkerSymbol();
                pClassSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                pClassSymbol.Size = 1;

                string classValue;
                classValue = pFeature.get_Value(fieldIndex) as string;

                //Test to see if this value was added to the renderer. If not, add it.
                ValFound = false;
                for (int i = 0; i <= pUniqueValueRenderer.ValueCount - 1; i++)
                {
                    if (pUniqueValueRenderer.get_Value(i) == classValue)
                    {
                        ValFound = true;
                        break; //Exit the loop if the value was found.
                    }
                }
                //If the value was not found, it's new and will be added.
                if (ValFound == false)
                {
                    pUniqueValueRenderer.AddValue(classValue, fieldName, pClassSymbol as
                        ISymbol);
                    pUniqueValueRenderer.set_Label(classValue, classValue);
                    pUniqueValueRenderer.set_Symbol(classValue, pClassSymbol as ISymbol);
                }
                pFeature = pFeatureCursor.NextFeature();
            }
            //Since the number of unique values is known, the color ramp can be sized and the colors assigned.
            pRandomColorRamp.Size = pUniqueValueRenderer.ValueCount;
            bool bOK;
            pRandomColorRamp.CreateRamp(out bOK);

            IEnumColors pEnumColors = pRandomColorRamp.Colors;
            pEnumColors.Reset();
            for (int j = 0; j <= pUniqueValueRenderer.ValueCount - 1; j++)
            {
                string xv;
                xv = pUniqueValueRenderer.get_Value(j);
                if (xv != "")
                {
                    ISimpleMarkerSymbol pSimpleFillColor = pUniqueValueRenderer.get_Symbol(xv)
                        as ISimpleMarkerSymbol;
                    pSimpleFillColor.Color = pEnumColors.Next();
                    pUniqueValueRenderer.set_Symbol(xv, pSimpleFillColor as ISymbol);

                }
            }

            //'** If you didn't use a predefined color ramp in a style, use "Custom" here. 
            //'** Otherwise, use the name of the color ramp you selected.
            pUniqueValueRenderer.ColorScheme = "Custom";
            ITable pTable = pDisplayTable as ITable;
            bool isString = pTable.Fields.get_Field(fieldIndex).Type ==
                esriFieldType.esriFieldTypeString;
            pUniqueValueRenderer.set_FieldType(0, isString);
            pGeoFeatureLayer.Renderer = pUniqueValueRenderer as IFeatureRenderer;

            //This makes the layer properties symbology tab show the correct interface.
            IUID pUID = new UIDClass();
            pUID.Value = "{683C994E-A17B-11D1-8816-080009EC732A}";
            pGeoFeatureLayer.RendererPropertyPageClassID = pUID as UIDClass;

        }
    }
}
