using System;
using System.Configuration;
using System.Text;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

namespace FrameWork.Respository {
    public class ESRIDBHelper {
        private ESRIDBHelper() {
            Init();
        }

        public static ESRIDBHelper Instance {
            get {
                return Nested.ESRIDBHelper;
            }
        }
        private GeoDB _geoDB;
        public GeoDB GeoDatabase {
            get { return _geoDB; }
        }
        private class Nested{
            static Nested() { }
            internal static readonly ESRIDBHelper ESRIDBHelper = new ESRIDBHelper();
        }

        private void Init() {
            if (_geoDB == null) {
                string DBType = ConfigurationSettings.AppSettings["ESRIDB"];
                IPropertySet propertySet = new PropertySetClass();
                IWorkspace ws;
                IWorkspaceFactory wsf;
                IFeatureWorkspace featureWS = null;
                if (DBType == "SDE") {
                    propertySet.SetProperty("SERVER", ConfigurationSettings.AppSettings["SERVER"]);
                    propertySet.SetProperty("INSTANCE", ConfigurationSettings.AppSettings["INSTANCE"]);
                    propertySet.SetProperty("DATABASE", ConfigurationSettings.AppSettings["DATABASE"]);
                    propertySet.SetProperty("USER", ConfigurationSettings.AppSettings["USER"]);
                    propertySet.SetProperty("PASSWORD", ConfigurationSettings.AppSettings["PASSWORD"]);
                    propertySet.SetProperty("VERSION", ConfigurationSettings.AppSettings["VERSION"]);
                    wsf = new SdeWorkspaceFactoryClass();
                    ws = wsf.Open(propertySet, 0);
                    featureWS = ws as IFeatureWorkspace;
                } else if (DBType == "MDB") {
                    propertySet.SetProperty("DATABASE", ConfigurationSettings.AppSettings["DATABASE"]);
                    wsf = new AccessWorkspaceFactoryClass();
                    ws = wsf.Open(propertySet, 0);
                    featureWS = ws as IFeatureWorkspace;
                } else if (DBType == "SHP") {
                    propertySet.SetProperty("DATABASE", ConfigurationSettings.AppSettings["DATABASE"]);
                    wsf = new ShapefileWorkspaceFactoryClass();
                    ws = wsf.Open(propertySet, 0);
                    featureWS = ws as IFeatureWorkspace;
                } else if (DBType == "FGDB") {
                }
                _geoDB = new GeoDB(featureWS);
            }
            
        }
    }
}
