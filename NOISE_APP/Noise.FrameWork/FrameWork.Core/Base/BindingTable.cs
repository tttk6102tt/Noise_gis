using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ADF;

namespace FrameWork.Core.Base {
    [Guid("5a239147-b06a-49e5-aa1c-e47f81adc10e")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Framework.Data.BindingTable")]
    public class BindingTable : BindingList<IRow>, ITypedList
    {
        #region Private Members

        private ITable wrappedTable;
        private List<PropertyDescriptor> fakePropertiesList = new List<PropertyDescriptor>();
        private IWorkspaceEdit wkspcEdit;

        #endregion Private Members

        #region Construction/Destruction

        public BindingTable(ITable tableToWrap, IQueryFilter qry)
        {
            if (qry == null)
            {
                qry = new QueryFilterClass();
                qry.WhereClause = "1=1";
            }
            wrappedTable = tableToWrap;
            GenerateFakeProperties();
            AddData(qry);
            wkspcEdit = ((IDataset)wrappedTable).Workspace as IWorkspaceEdit;
            AllowNew = true;
            AllowRemove = true;
        }

        public BindingTable(ITable tableToWrap, ICursor cur)
        {
            wrappedTable = tableToWrap;
            GenerateFakeProperties();
            AddData(cur);
            wkspcEdit = ((IDataset)wrappedTable).Workspace as IWorkspaceEdit;
            AllowNew = true;
            AllowRemove = true;
        }

        #endregion Construction/Destruction

        #region ITypedList Members
        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            PropertyDescriptorCollection propCollection = null;
            if (null == listAccessors)
            {
                propCollection = new PropertyDescriptorCollection(fakePropertiesList.ToArray());
            }
            else
            {
                List<PropertyDescriptor> tempList = new List<PropertyDescriptor>();
                foreach (PropertyDescriptor curPropDesc in listAccessors)
                {
                    if (fakePropertiesList.Contains(curPropDesc))
                    {
                        tempList.Add(curPropDesc);
                    }
                }
                propCollection = new PropertyDescriptorCollection(tempList.ToArray());
            }
            return propCollection;
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return ((IDataset)wrappedTable).Name;
        }

        #endregion ITypedList Members

        public bool UseCVDomains
        {
            set
            {
                foreach (FieldPropertyDescriptor curPropDesc in fakePropertiesList)
                {
                    if (curPropDesc.HasCVDomain)
                    {
                        curPropDesc.UseCVDomain = value;
                    }
                }
            }
        }

        public IRow GetRow(int index)
        {
            return Items[index];
        }

        #region Protected Overrides
        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            if (AllowNew)
            {
                StartEditOp();
                IRow newRow = wrappedTable.CreateRow();
                e.NewObject = newRow;
                for (int fieldCount = 0; fieldCount < newRow.Fields.FieldCount; fieldCount++)
                {
                    IField curField = newRow.Fields.get_Field(fieldCount);
                    if (curField.Editable)
                    {
                        newRow.set_Value(fieldCount, curField.DefaultValue);
                    }
                }
                newRow.Store();
                StopEditOp();
                base.OnAddingNew(e);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (AllowRemove)
            {
                StartEditOp();
                IRow itemToRemove = Items[index];
                itemToRemove.Delete();
                StopEditOp();
                base.RemoveItem(index);
            }
        }
        #endregion Protected Overrides

        #region Private Methods
        private void GenerateFakeProperties()
        {
            for (int fieldCount = 0; fieldCount < wrappedTable.Fields.FieldCount; fieldCount++)
            {
                FieldPropertyDescriptor newPropertyDesc = new FieldPropertyDescriptor(
                  wrappedTable, GetStandarFieldName(wrappedTable.Fields.get_Field(fieldCount).Name), fieldCount);
                if (!CheckExisPropertyDesc(newPropertyDesc))
                    fakePropertiesList.Add(newPropertyDesc);
            }
        }
        private bool CheckExisPropertyDesc(FieldPropertyDescriptor propertyDescriptor)
        {
            foreach (PropertyDescriptor propertyDes in fakePropertiesList)
            {
                if (propertyDes.DisplayName == propertyDescriptor.DisplayName)
                    return true;
            }
            return false;
        }
        private string GetStandarFieldName(string fieldName)
        {
            int index = fieldName.LastIndexOf(".") + 1;
            if (index >= 0)
            {
                fieldName = fieldName.Substring(index, fieldName.Length - index);
            }
            return fieldName;
        }
        private void AddData(IQueryFilter qry)
        {
            using (ComReleaser cr = new ComReleaser())
            {
                ICursor cur = wrappedTable.Search(qry, false);
                IRow curRow = cur.NextRow();
                while (null != curRow)
                {
                    Add(curRow);
                    curRow = cur.NextRow();
                }
                cr.ManageLifetime(cur);
            }
        }

        private void AddData(ICursor cur)
        {
            using (ComReleaser cr = new ComReleaser())
            {
                IRow curRow = cur.NextRow();
                while (null != curRow)
                {
                    Add(curRow);
                    curRow = cur.NextRow();
                }
                cr.ManageLifetime(cur);
            }
        }

        private void StartEditOp()
        {
            wkspcEdit.StartEditOperation();
        }

        private void StopEditOp()
        {
            wkspcEdit.StopEditOperation();
        }
        #endregion Private Methods
    }
}

