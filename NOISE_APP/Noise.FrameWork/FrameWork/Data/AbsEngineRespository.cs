using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using FrameWork.Respository;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ADF;
using FrameWork.Domain;

namespace FrameWork.Data {
    public class AbsEngineRespository<T> : IRespository<T> {
        #region Fields
        private Type persitentType = typeof(T);
        private GeoDB Geodatabase {
            get { return ESRIDBHelper.Instance.GeoDatabase; }
        }
        #endregion

        #region IRespository<T> Members

        public T CreateNew() {
            TableNameAttr tba = (TableNameAttr)Attribute.GetCustomAttribute(persitentType, typeof(TableNameAttr));
            IFeatureClass fc = Geodatabase.GetClass(tba.Name);
            IFeature f = fc.CreateFeature();
            return (T)Activator.CreateInstance(persitentType,f);
        }

        public T GetByID(int id) {
            TableNameAttr tba = (TableNameAttr)Attribute.GetCustomAttribute(persitentType, typeof(TableNameAttr));
            IFeatureClass fc = Geodatabase.GetClass(tba.Name);
            IFeature f = fc.GetFeature(id);
            return (T)Activator.CreateInstance(persitentType, f);
        }

        public IList<T> GetList(IQueryFilter qry) {
            IList<T> entityList=new List<T>();
            //IFeatureClass fc = Geodatabase.GetClass(persitentType.Name);
            //IFeatureClass fc = Geodatabase.GetClass(persitentType.GetProperty("TableName").GetValue(persitentType,null).ToString());
            using (ComReleaser cr = new ComReleaser()) {
                TableNameAttr tba = (TableNameAttr)Attribute.GetCustomAttribute(persitentType, typeof(TableNameAttr));
                IFeatureClass fc = Geodatabase.GetClass(tba.Name);
                IFeatureCursor fcursor = fc.Search(qry, false);
                IFeature f = fcursor.NextFeature();
                while (f != null) {
                    entityList.Add((T)Activator.CreateInstance(persitentType, f));
                    f = fcursor.NextFeature();
                }
                //Marshal.ReleaseComObject(fcursor);
                cr.ManageLifetime(fcursor);
            }
            return entityList;
        }

        public BindingTable GetBindingTable(IQueryFilter qry) {
            TableNameAttr tba = (TableNameAttr)Attribute.GetCustomAttribute(persitentType, typeof(TableNameAttr));
            IFeatureClass fc = Geodatabase.GetClass(tba.Name);
            ITable tbl = fc as ITable;
            return new BindingTable(tbl,qry);
        }

        #endregion
    }
}
