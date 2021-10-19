using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using FrameWork.Data;

namespace FrameWork.Respository {
    public class GeoDB {

        #region Fields
        private IFeatureWorkspace _featureWS;
        private bool _isEditing = false;
        private bool _isUndo = false;
        private bool _isBegined = false;
        #endregion

        #region Properties
        public bool IsUnDo {
            get { return _isUndo; }
            set { _isUndo = value; }
        }
        public bool IsEditing {
            get{
                IWorkspaceEdit wsEdit = _featureWS as IWorkspaceEdit;
                _isEditing = wsEdit.IsBeingEdited();
                return _isEditing;
            }
        }
        private DataCache Cache {
            get { return DataCache.Instance; }
        }
        #endregion

        #region Constructor
        public GeoDB(IFeatureWorkspace fws) {
            _featureWS = fws;
        }
        #endregion

        #region Methods
        public void StartEditing(bool isUnDo) {
            IWorkspaceEdit wsEdit = _featureWS as IWorkspaceEdit;
            if (!wsEdit.IsBeingEdited()) {
                _isUndo = isUnDo;
                wsEdit.StartEditing(_isUndo);
            }
        }
        public void StopEditing(bool isSave) {
            IWorkspaceEdit wsEdit = _featureWS as IWorkspaceEdit;
            if (wsEdit.IsBeingEdited()) {
                wsEdit.StopEditing(isSave);
            }
        }
        public bool BeginAction() {
            if (!_isUndo) {
                return false;
            }
            if (_isBegined) {
                return false;
            }
            IWorkspaceEdit wsEdit = _featureWS as IWorkspaceEdit;
            wsEdit.StartEditOperation();
            _isBegined = true;
            return true;
        }
        public bool EndAction() {
            if (_isBegined) {
                IWorkspaceEdit wsEdit = _featureWS as IWorkspaceEdit;
                wsEdit.StopEditOperation();
                _isBegined = false;
                return true;
            }
            return false;
        }
        public IFeatureClass GetClass(string name) {
            IFeatureClass fc = Cache.GetClass(name);
            if (fc == null) {
                fc = _featureWS.OpenFeatureClass(name);
            }
            return fc;
        }
        #endregion
    }
}
