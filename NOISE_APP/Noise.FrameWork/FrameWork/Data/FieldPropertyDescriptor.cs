using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace FrameWork.Data {
    internal class FieldPropertyDescriptor : PropertyDescriptor {

        #region Private Members
        private int wrappedFieldIndex;
        private Type netType;
        private Type actualType;
        private esriFieldType esriType;
        bool isEditable = true;
        private IWorkspaceEdit wkspcEdit;
        private ICodedValueDomain cvDomain;
        private bool useCVDomain;
        private TypeConverter actualValueConverter;
        private TypeConverter cvDomainValDescriptionConverter;
        #endregion Private Members

        #region Construction/Destruction
        public FieldPropertyDescriptor(ITable wrappedTable, string fieldName, int fieldIndex)
            : base(fieldName, null) {
            wrappedFieldIndex = fieldIndex;
            IField wrappedField = wrappedTable.Fields.get_Field(fieldIndex);
            esriType = wrappedField.Type;
            isEditable = wrappedField.Editable &&
              (esriType != esriFieldType.esriFieldTypeBlob) &&
              (esriType != esriFieldType.esriFieldTypeRaster) &&
              (esriType != esriFieldType.esriFieldTypeGeometry);
            netType = actualType = EsriFieldTypeToSystemType(wrappedField);
            wkspcEdit = ((IDataset)wrappedTable).Workspace as IWorkspaceEdit;
        }
        #endregion Construction/Destruction

        public bool HasCVDomain {
            get {
                return null != cvDomain;
            }
        }

        public bool UseCVDomain {
            set {
                useCVDomain = value;
                if (value) {
                    netType = typeof(string);
                } else {
                    netType = actualType;
                }
            }
        }

        #region Public Overrides
        public override TypeConverter Converter {
            get {
                TypeConverter retVal = null;

                if (null != cvDomain) {
                    if (useCVDomain) {
                        if (null == cvDomainValDescriptionConverter) {
                            cvDomainValDescriptionConverter = TypeDescriptor.GetConverter(typeof(string));
                        }

                        retVal = cvDomainValDescriptionConverter;
                    } else {
                        if (null == actualValueConverter) {
                            actualValueConverter = TypeDescriptor.GetConverter(actualType);
                        }

                        retVal = actualValueConverter;
                    }
                } else {
                    retVal = base.Converter;
                }

                return retVal;
            }
        }

        public override bool CanResetValue(object component) {
            return false;
        }

        public override Type ComponentType {
            get { return typeof(IRow); }
        }

        public override object GetValue(object component) {
            object retVal = null;

            IRow givenRow = (IRow)component;
            try {
                object value = givenRow.get_Value(wrappedFieldIndex);

                if ((null != cvDomain) && useCVDomain) {
                    value = cvDomain.get_Name(Convert.ToInt32(value));
                }
                switch (esriType) {
                    case esriFieldType.esriFieldTypeBlob:
                        retVal = "Blob";
                        break;

                    case esriFieldType.esriFieldTypeGeometry:
                        retVal = GetGeometryTypeAsString(value);
                        break;

                    case esriFieldType.esriFieldTypeRaster:
                        retVal = "Raster";
                        break;

                    default:
                        retVal = value;
                        break;
                }
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return retVal;
        }

        public override bool IsReadOnly {
            get { return !isEditable; }
        }

        public override Type PropertyType {
            get { return netType; }
        }

        public override void ResetValue(object component) {

        }

        public override void SetValue(object component, object value) {
            IRow givenRow = (IRow)component;

            if (null != cvDomain) {
                if (!useCVDomain) {
                    if (!((IDomain)cvDomain).MemberOf(value)) {
                        //System.Windows.Forms.MessageBox.Show(string.Format(
                        //  "Value {0} is not valid for coded value domain {1}", value.ToString(), ((IDomain)cvDomain).Name));
                        return;
                    }
                } else {
                    bool foundMatch = false;
                    for (int valueCount = 0; valueCount < cvDomain.CodeCount; valueCount++) {
                        if (value.ToString() == cvDomain.get_Name(valueCount)) {
                            foundMatch = true;
                            value = valueCount;
                            break;
                        }
                    }

                    if (!foundMatch) {
                        //System.Windows.Forms.MessageBox.Show(string.Format(
                        //  "Value {0} is not valid for coded value domain {1}", value.ToString(), ((IDomain)cvDomain).Name));
                        return;
                    }
                }
            }
            givenRow.set_Value(wrappedFieldIndex, value);

            bool weStartedEditing = false;
            if (!wkspcEdit.IsBeingEdited()) {
                wkspcEdit.StartEditing(false);
                weStartedEditing = true;
            }

            wkspcEdit.StartEditOperation();
            givenRow.Store();
            wkspcEdit.StopEditOperation();

            if (weStartedEditing) {
                wkspcEdit.StopEditing(true);
            }

        }

        public override bool ShouldSerializeValue(object component) {
            return false;
        }
        #endregion Public Overrides

        #region Private Methods
        private Type EsriFieldTypeToSystemType(IField field) {
            esriFieldType esriType = field.Type;

            cvDomain = field.Domain as ICodedValueDomain;
            if ((null != cvDomain) && useCVDomain) {
                return typeof(string);
            }

            try {
                switch (esriType) {
                    case esriFieldType.esriFieldTypeBlob:
                        return typeof(string);
                    case esriFieldType.esriFieldTypeDate:
                        return typeof(DateTime);
                    case esriFieldType.esriFieldTypeDouble:
                        return typeof(double);
                    case esriFieldType.esriFieldTypeGeometry:
                        return typeof(string);
                    case esriFieldType.esriFieldTypeGlobalID:
                        return typeof(string);
                    case esriFieldType.esriFieldTypeGUID:
                        return typeof(Guid);
                    case esriFieldType.esriFieldTypeInteger:
                        return typeof(Int32);
                    case esriFieldType.esriFieldTypeOID:
                        return typeof(Int32);
                    case esriFieldType.esriFieldTypeRaster:
                        return typeof(string);
                    case esriFieldType.esriFieldTypeSingle:
                        return typeof(Single);
                    case esriFieldType.esriFieldTypeSmallInteger:
                        return typeof(Int16);
                    case esriFieldType.esriFieldTypeString:
                        return typeof(string);
                    default:
                        return typeof(string);
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return typeof(string);
            }
        }

        private string GetGeometryTypeAsString(object value) {
            string retVal = "";
            IGeometry geometry = value as IGeometry;
            if (geometry != null) {
                retVal = geometry.GeometryType.ToString();
            }
            return retVal;
        }
        #endregion Private Methods
    }
}
