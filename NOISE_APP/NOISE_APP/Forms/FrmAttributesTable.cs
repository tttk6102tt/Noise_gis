using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Carto;
using FrameWork.Core.Base;
using ESRI.ArcGIS.Geodatabase;

namespace NOISE_APP.Forms
{
    public partial class FrmAttributesTable : DevExpress.XtraEditors.XtraForm
    {
        #region Fields
        private IMap _mapObject;
        private ILayer _layerObject;
        private BindingTable _tableObject;
        private ITable _table;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public FrmAttributesTable(IMap map, ILayer layerSelect, ITable table = null)
        {
            _mapObject = map;

            if (table != null)
                _table = table;
            if (layerSelect != null)
                _layerObject = layerSelect;
            InitializeComponent();
            gvAttributes.SelectionChanged += gvAttribute_SelectionChanged;
            btnZoom.Click += cmdZoomto_Click;
            btnToExcel.Click += cmdExport_Click;
        }
        #endregion

        #region Form events
        private void FrmAttributesTable_Load(object sender, EventArgs e)
        {
            ITable pTable;
            if (_table == null)
            {
                IGeoFeatureLayer pGeoFLayer = (IGeoFeatureLayer)_layerObject;
                pTable = (ITable)pGeoFLayer;
            }
            else
            {
                pTable = _table;
            }

            ICursor pCur = pTable.Search(null, false);
            _tableObject = new BindingTable(pTable, pCur);
            gcAttributes.DataSource = _tableObject;
            gvAttributes.OptionsBehavior.Editable = false;
            ConfigTableView();
        }
        #endregion
        #region Control events
        private void gvAttribute_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            IActiveView pActiveView = _mapObject as IActiveView;
            if (_mapObject.SelectionCount > 0)
            {
                _mapObject.ClearSelection();
                // Refresher.RefreshCurrentView(pActiveView, FrameWork.Map.RefreshType.SelectionChanged);
            }
            if (gvAttributes.SelectedRowsCount > 0)
            {
                foreach (int i in gvAttributes.GetSelectedRows())
                {
                    IFeature pFeature = _tableObject.GetRow(i) as IFeature;
                    _mapObject.SelectFeature(_layerObject, pFeature);
                }
                // Refresher.RefreshCurrentView(pActiveView, FrameWork.Map.RefreshType.SelectionChanged);
                // cmdZoomto.Enabled = true;
            }
            else
            {
                // cmdZoomto.Enabled = false;
            }
        }

        private void cmdZoomto_Click(object sender, EventArgs e)
        {
            FrameWork.Map.Utilities.MapUtility.ZoomToSelected(_mapObject);
        }

        private void cmdExport_Click(object sender, EventArgs e)
        {
            //DevExpress.XtraEditors.SimpleButton cmd = sender as DevExpress.XtraEditors.SimpleButton;
            //if (cmd.Tag.Equals("EXCEL"))
            //{
            Cursor = Cursors.WaitCursor;
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.Filter = "Excel(97-2003) file|*.xls|Excel(2007-2010) file|*.xlsx";
            SaveFile.FilterIndex = 1;
            //SaveFile.Title = _gisMain.SetLanguageDisplay("Xuất ra tệp Excel", "Export to Excel file");
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                if (SaveFile.FilterIndex == 1)
                    gvAttributes.ExportToXls(SaveFile.FileName);
                else
                    gvAttributes.ExportToXlsx(SaveFile.FileName);
                //Common.Class.GISMessageBox.ShowInfoMessage(_gisMain.SetLanguageDisplay("Xuất ra tệp Excel thành công!", "Export to Excel successful"));
            }
            Cursor = Cursors.Default;
            //}
        }
        #endregion

        #region Private functions
        private System.Type GetType(IField field)
        {
            switch (field.Type)
            {
                case esriFieldType.esriFieldTypeDate:
                    return typeof(DateTime);
                case esriFieldType.esriFieldTypeDouble:
                case esriFieldType.esriFieldTypeSingle:
                    return typeof(double);
                case esriFieldType.esriFieldTypeOID:
                case esriFieldType.esriFieldTypeInteger:
                case esriFieldType.esriFieldTypeSmallInteger:
                    return typeof(int);
                default:
                    return typeof(string);
            }
        }
        private void ConfigTableView()
        {
            // string sLayerName = ((IFeatureLayer)_layerObject).FeatureClass.AliasName;
            IFields fields;//
            if (_table != null)
                fields = _table.Fields;
            else
                fields = ((IFeatureLayer)_layerObject).FeatureClass.Fields;
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in this.gvAttributes.Columns)
            {
                if (col.FieldName.ToUpper().Contains("SHAPE"))
                    col.Visible = false;
                else
                {
                    int fieldIndex = fields.FindField(col.FieldName);
                    if (fieldIndex >= 0)
                    {
                        IField field = fields.get_Field(fieldIndex);
                        System.Type type = GetType(field);
                        if (type == typeof(DateTime))
                        {
                            DevExpress.XtraEditors.Repository.RepositoryItemDateEdit ItemDatetime = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
                            ItemDatetime.Mask.EditMask = "dd/MM/yyyy";
                            ItemDatetime.Mask.UseMaskAsDisplayFormat = true;
                            // _gisMain.FormatControl.DateTimeControl(ref ItemDatetime);
                            col.ColumnEdit = ItemDatetime;

                        }
                        else if (type == typeof(int))
                        {
                            DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ItemNumber = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
                            // _gisMain.FormatControl.NumberControl(ref ItemNumber);
                            col.ColumnEdit = ItemNumber;
                        }
                        else if (type == typeof(double))
                        {
                            DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ItemDecimel = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
                            // _gisMain.FormatControl.NumberControlDimension(ref ItemDecimel);
                            col.ColumnEdit = ItemDecimel;
                        }
                    }
                    col.Caption = fields.Field[fieldIndex].AliasName;
                    col.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    col.AppearanceHeader.Options.UseFont = true;
                    col.AppearanceHeader.Options.UseTextOptions = true;
                    col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    //col.Caption = this._gisMain.FieldMan.GetDisplayName(sLayerName, col.FieldName, col.FieldName, col.Width, language);
                    //col.Width = this._gisMain.FieldMan.GetWidthColumn(sLayerName, col.FieldName);
                    //col.VisibleIndex = this._gisMain.FieldMan.GetIndexColumn(sLayerName, col.FieldName);
                    col.BestFit();
                    col.Width = col.Width + 30;
                }
            }
            //for (int i = 0; i < gvAttribute.VisibleColumns.Count; i++)
            //{
            //    gvAttribute.VisibleColumns[i].VisibleIndex = this._gisMain.FieldMan.GetIndexColumn(sLayerName, gvAttribute.VisibleColumns[i].FieldName);
            //}
        }
        private void UpdateConfig()
        {
            //foreach (DevExpress.XtraGrid.Columns.GridColumn col in this.gvAttribute.Columns)
            //{
            //    this._gisMain.FieldMan.UpdateWidthField(((IFeatureLayer)_layerObject).FeatureClass.AliasName, col.FieldName, col.Width, col.VisibleIndex);
            //}
        }
        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            UpdateConfig();
        }

        private void gvAttributes_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            IActiveView pActiveView = _mapObject as IActiveView;
            if (_mapObject.SelectionCount > 0)
            {
                _mapObject.ClearSelection();
                // Refresher.RefreshCurrentView(pActiveView, FrameWork.Map.RefreshType.SelectionChanged);
            }
            if (gvAttributes.SelectedRowsCount > 0)
            {
                foreach (int i in gvAttributes.GetSelectedRows())
                {
                    IFeature pFeature = _tableObject.GetRow(i) as IFeature;
                    _mapObject.SelectFeature(_layerObject, pFeature);
                }
                //Refresher.RefreshCurrentView(pActiveView, FrameWork.Map.RefreshType.SelectionChanged);
                //cmdZoomto.Enabled = true;
            }
            else
            {
                // cmdZoomto.Enabled = false;
            }
        }
    }
}