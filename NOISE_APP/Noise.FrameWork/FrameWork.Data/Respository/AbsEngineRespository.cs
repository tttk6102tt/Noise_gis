using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.Domain;
using FrameWork.Core.DataInterfaces;
using FrameWork.Data.DB;
using FrameWork.Core.Base;
using FrameWork.Core.Attributes;

namespace FrameWork.Data.Respository {
    public class AbsEngineRespository<T> : IRespository<T> {
        #region Fields
        private Type persitentType = typeof(T);
        private GeoDB Geodatabase {
            get { return ESRIDBHelper.Instance.GeoDatabase; }
        }
        #endregion

        #region IRespository<T> Members

        public void Save(T obj) {
            ObjectAttribute oa = (ObjectAttribute)Attribute.GetCustomAttribute(persitentType, typeof(ObjectAttribute));
            if (oa.ObjectType == ObjectTypeEnums.Feature) {
                IFeatureClass fc = Geodatabase.GetClass(oa.Name);
                IFeature f = fc.CreateFeature();
                UpdateDataForFeatureFromObj(f, obj);
            } else {
                ITable tbl = Geodatabase.GetTable(oa.Name);
                IRow r = tbl.CreateRow();
                UpdateDataForRowFromObj(r, obj);
            }
        }

        public void Update(T obj) {
            int id = -1;
            ObjectAttribute oa = (ObjectAttribute)Attribute.GetCustomAttribute(persitentType, typeof(ObjectAttribute));
            if (oa.ObjectType == ObjectTypeEnums.Feature) {
                IFeatureClass fc = Geodatabase.GetClass(oa.Name);
                id = GetObjectIDFromObj(obj);
                IFeature f = fc.GetFeature(id);
                UpdateDataForFeatureFromObj(f, obj);
            } else {
                ITable tbl = Geodatabase.GetTable(oa.Name);
                id = GetObjectIDFromObj(obj);
                IRow r = tbl.GetRow(id);
                UpdateDataForRowFromObj(r, obj);
            }
        }

        public T GetByObjectID(T obj) {
            ObjectAttribute oa = (ObjectAttribute)Attribute.GetCustomAttribute(persitentType, typeof(ObjectAttribute));
            PropertyInfo[] propers = persitentType.GetProperties();
            int id = GetObjectIDFromObj(obj);
            if (oa.ObjectType == ObjectTypeEnums.Feature) {
                if (id > 0) {
                    IFeatureClass fc = Geodatabase.GetClass(oa.Name);
                    IFeature f = fc.GetFeature(id);
                    FillPropertyDataFromFeature(f, obj);
                }
                return obj;
            } else {
                if (id > 0) {
                    ITable tbl = Geodatabase.GetTable(oa.Name);
                    IRow r = tbl.GetRow(id);
                    FillPropertyDataFromRow(r, obj);
                }
                return obj;
            }
        }

        public T GetByCodeID(T obj) {
            ObjectAttribute oa = (ObjectAttribute)Attribute.GetCustomAttribute(persitentType, typeof(ObjectAttribute));
            PropertyInfo[] propers = persitentType.GetProperties();
            string fieldName = string.Empty;
            string code = GetCodeIDFromObj(obj,out fieldName);
            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = fieldName + "=" + "'" + code + "'";
            using (ComReleaser cr = new ComReleaser()) {
                if (oa.ObjectType == ObjectTypeEnums.Feature) {
                    IFeatureClass fc = Geodatabase.GetClass(oa.Name);
                    IFeatureCursor fcursor = fc.Search(qry, true);
                    IFeature f = fcursor.NextFeature();
                    cr.ManageLifetime(fcursor);
                    FillPropertyDataFromFeature(f, obj);
                    return obj;
                } else {
                    ITable tbl = Geodatabase.GetTable(oa.Name);
                    ICursor cursor = tbl.Search(qry,true);
                    IRow r = cursor.NextRow();
                    cr.ManageLifetime(cursor);
                    FillPropertyDataFromRow(r, obj);
                    return obj;
                }
            }
        }

        public IList<T> GetList(IQueryFilter qry) {
            IList<T> entityList=new List<T>();
            using (ComReleaser cr = new ComReleaser()) {
                ObjectAttribute oa = (ObjectAttribute)Attribute.GetCustomAttribute(persitentType, typeof(ObjectAttribute));
                if (oa.ObjectType == ObjectTypeEnums.Feature) {
                    IFeatureClass fc = Geodatabase.GetClass(oa.Name);
                    IFeatureCursor fcursor = fc.Search(qry, true);
                    IFeature f = fcursor.NextFeature();
                    while (f != null) {
                        T obj = (T)Activator.CreateInstance(persitentType);
                        FillPropertyDataFromFeature(f, obj);
                        entityList.Add(obj);
                        f = fcursor.NextFeature();
                    }
                    cr.ManageLifetime(fcursor);
                } else {
                    ITable tbl = Geodatabase.GetTable(oa.Name);
                    ICursor cursor = tbl.Search(qry, true);
                    IRow r = cursor.NextRow();
                    while (r != null) {
                        T obj = (T)Activator.CreateInstance(persitentType);
                        FillPropertyDataFromRow(r, obj);
                        entityList.Add(obj);
                        r = cursor.NextRow();
                    }
                    cr.ManageLifetime(cursor);
                }
            }
            return entityList;
        }

        public BindingTable GetBindingTable(IQueryFilter qry) {
            ITable tbl = null;
            ObjectAttribute oa = (ObjectAttribute)Attribute.GetCustomAttribute(persitentType, typeof(ObjectAttribute));
            if (oa.ObjectType == ObjectTypeEnums.Feature) {
                IFeatureClass fc = Geodatabase.GetClass(oa.Name);
                tbl = fc as ITable;
            } else {
                tbl = Geodatabase.GetTable(oa.Name);
            }
            return new BindingTable(tbl,qry);
        }

        #endregion

        #region Private functions
        private object GetLinkedObject(object lnkObj,string className, string fieldName, ObjectTypeEnums objectType,object value) {
            PropertyInfo[] propers = lnkObj.GetType().GetProperties();
            Type keyType = GetPrimaryKeyTypeFromObj(lnkObj);
            IQueryFilter qry = new QueryFilterClass();
            if (keyType == Type.GetType("System.String")) {
                qry.WhereClause = fieldName + "='" + value + "'";
            } else {
                qry.WhereClause = fieldName + "=" + value;
            }
            using (ComReleaser cr = new ComReleaser()) {
                if (objectType == ObjectTypeEnums.Feature) {
                    IFeatureClass fc = Geodatabase.GetClass(className);
                    IFeatureCursor fcursor = fc.Search(qry, true);
                    IFeature f = fcursor.NextFeature();
                    FillPropertyDataLnkFromF(f, lnkObj);
                    return lnkObj;
                } else {
                    ITable tbl = Geodatabase.GetTable(className);
                    ICursor cursor = tbl.Search(qry, true);
                    IRow r = cursor.NextRow();
                    FillPropertyDataLnkFromRow(r, lnkObj);
                    return lnkObj;
                }
            }
        }
        private Type GetPrimaryKeyTypeFromObj(object obj) {
            PropertyInfo[] propers = obj.GetType().GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PrimaryKeyAttribute) {
                        PrimaryKeyAttribute pk = attr as PrimaryKeyAttribute;
                        return properInfo.PropertyType;
                    }
                }
            }
            return Type.GetType("System.String");
        }
        private int GetObjectIDFromObj(T obj) {
            int id = -1;
            PropertyInfo[] propers = persitentType.GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is ObjectIDAttribute) {
                        id = Convert.ToInt32(properInfo.GetValue(obj, null));
                    }
                }
            }
            return id;
        }
        private string GetCodeIDFromObj(T obj,out string fieldName) {
            string code = string.Empty;
            fieldName = string.Empty;
            PropertyInfo[] propers = persitentType.GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PrimaryKeyAttribute) {
                        PrimaryKeyAttribute pk = attr as PrimaryKeyAttribute;
                        code = properInfo.GetValue(obj, null).ToString();
                        fieldName = pk.Name;
                    }
                }
            }
            return code;
        }
        private void UpdateDataForFeatureFromObj(IFeature f,T obj) {
            PropertyInfo[] propers = persitentType.GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PropertyAttribute) {
                        PropertyAttribute property = attr as PropertyAttribute;
                        if (property.EditAble) {
                            f.set_Value(f.Fields.FindField(property.Name), properInfo.GetValue(obj, null));
                        }
                    } else if (attr is ShapeAttribute) {
                        ShapeAttribute property = attr as ShapeAttribute;
                        f.Shape = properInfo.GetValue(obj, null) as IGeometry;
                    }
                }
            }
            f.Store();
        }
        private void UpdateDataForRowFromObj(IRow r,T obj) {
            PropertyInfo[] propers = persitentType.GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PropertyAttribute) {
                        PropertyAttribute property = attr as PropertyAttribute;
                        if (property.EditAble) {
                            r.set_Value(r.Fields.FindField(property.Name), properInfo.GetValue(obj, null));
                        }
                    }
                }
            }
            r.Store();
        }
        private T FillPropertyDataFromFeature(IFeature f, T obj) {
            PropertyInfo[] propers = persitentType.GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PropertyAttribute) {
                        PropertyAttribute property = attr as PropertyAttribute;
                        if (property.EditAble) {
                            properInfo.SetValue(obj, f.get_Value(f.Fields.FindField(property.Name)), null);
                        }
                    } else if (attr is ObjectIDAttribute) {
                        properInfo.SetValue(obj, f.OID, null);
                    } else if (attr is PrimaryKeyAttribute){
                        PrimaryKeyAttribute pk = attr as PrimaryKeyAttribute;
                        properInfo.SetValue(obj, f.get_Value(f.Fields.FindField(pk.Name)), null);
                    } else if (attr is ShapeAttribute) {
                        ShapeAttribute property = attr as ShapeAttribute;
                        properInfo.SetValue(obj, f.ShapeCopy, null);
                    }
                }
            }
            return obj;
        }
        private T FillPropertyDataFromRow(IRow r,T obj) {
            PropertyInfo[] propers = persitentType.GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PropertyAttribute) {
                        PropertyAttribute property = attr as PropertyAttribute;
                        if (property.EditAble) {
                            SetValueForProperty(properInfo,r.get_Value(r.Fields.FindField(property.Name)),obj);
                        }
                    } else if (attr is ObjectIDAttribute) {
                        properInfo.SetValue(obj, r.OID, null);
                    } else if (attr is PrimaryKeyAttribute) {
                        PrimaryKeyAttribute pk = attr as PrimaryKeyAttribute;
                        SetValueForProperty(properInfo, r.get_Value(r.Fields.FindField(pk.Name)), obj);
                    } else if (attr is LinkToAttribute) {
                        LinkToAttribute lnkTo = attr as LinkToAttribute;
                        object lknObj = properInfo.GetValue(obj, null);
                        if (lknObj == null) {
                            lknObj = Activator.CreateInstance(lnkTo.AssemblyName,"Node");
                        }
                        lknObj = GetLinkedObject(lknObj, lnkTo.ClassName, lnkTo.FieldName, lnkTo.ObjectType, r.get_Value(r.Fields.FindField(lnkTo.FieldName)));
                        properInfo.SetValue(obj, lknObj, null);
                    }
                }
            }
            return obj;
        }
        private object FillPropertyDataLnkFromF(IFeature f, object obj) {
            PropertyInfo[] propers = obj.GetType().GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PropertyAttribute) {
                        PropertyAttribute property = attr as PropertyAttribute;
                        if (property.EditAble) {
                            properInfo.SetValue(obj, f.get_Value(f.Fields.FindField(property.Name)), null);
                        }
                    } else if (attr is ObjectIDAttribute) {
                        properInfo.SetValue(obj, f.OID, null);
                    } else if (attr is PrimaryKeyAttribute) {
                        PrimaryKeyAttribute pk = attr as PrimaryKeyAttribute;
                        properInfo.SetValue(obj, f.get_Value(f.Fields.FindField(pk.Name)), null);
                    } else if (attr is ShapeAttribute) {
                        ShapeAttribute property = attr as ShapeAttribute;
                        properInfo.SetValue(obj, f.ShapeCopy, null);
                    }
                }
            }
            return obj;
        }
        private object FillPropertyDataLnkFromRow(IRow r, object obj) {
            PropertyInfo[] propers = obj.GetType().GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PropertyAttribute) {
                        PropertyAttribute property = attr as PropertyAttribute;
                        if (property.EditAble) {
                            properInfo.SetValue(obj, r.get_Value(r.Fields.FindField(property.Name)), null);
                        }
                    } else if (attr is PrimaryKeyAttribute) {
                        PrimaryKeyAttribute pk = attr as PrimaryKeyAttribute;
                        properInfo.SetValue(obj, r.get_Value(r.Fields.FindField(pk.Name)), null);
                    } else if (attr is ObjectIDAttribute) {
                        properInfo.SetValue(obj, r.OID, null);
                    } 
                }
            }
            return obj;
        }
        private string GetPropertyNameByAttrName(T obj, string attrName) {
            PropertyInfo[] propers = persitentType.GetProperties();
            foreach (PropertyInfo properInfo in propers) {
                foreach (Attribute attr in properInfo.GetCustomAttributes(true)) {
                    if (attr is PropertyAttribute) {
                        PropertyAttribute property = attr as PropertyAttribute;
                        return properInfo.Name;
                    } 
                }
            }
            return string.Empty;
        }
        private void SetValueForProperty(PropertyInfo properInfo,object value,T obj) {
            if (value != DBNull.Value) {
                if (properInfo.PropertyType == typeof(string)) {
                    properInfo.SetValue(obj, value.ToString(), null);
                } else if (properInfo.PropertyType == typeof(int)) {
                    properInfo.SetValue(obj, Convert.ToInt32(value), null);
                } else if (properInfo.PropertyType == typeof(double)) {
                    properInfo.SetValue(obj, Convert.ToDouble(value), null);
                } else if (properInfo.PropertyType == typeof(DateTime)) {
                    properInfo.SetValue(obj, Convert.ToDateTime(value), null);
                }
            }
        }
        #endregion
    }
}
