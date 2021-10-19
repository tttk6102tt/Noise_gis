using System;
using ESRI.ArcGIS.Geodatabase;
using FrameWork.Core.Base;
using FrameWork.Core.Attributes;
using ESRI.ArcGIS.Geometry;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("Pipes",ObjectTypeEnums.Feature)]
    public class Pipe {

        #region Fields
        private int _objectID;
        private string _pipeID;
        private string _pipeName;
        private string _pipePosRelative;
        private string _nodeSID;
        private string _nodeSType;
        private string _nodeEID;
        private string _nodeEType;
        private string _pipeTypeID;
        private string _materialID;
        private double _length;
        private double _diameter;
        private double _thick;
        private double _thickIn;
        private double _sDeep;
        private double _eDeep;
        private string _img1;
        private string _img2;
        private string _standard;
        private string _statusID;
        private string _manFacID;
        private string _orginID;
        private string _joinTypeID;
        private string _joinTypeStd;
        private double _old;
        private double _pipeValue;
        private DateTime _dateCreated;
        private DateTime _dateUse;
        private string _provinceID;
        private string _districtID;
        private string _communeID;
        private string _streetID;
        private string _zoneID;
        private string _wardID;
        private IGeometry _shape;
        #endregion

        #region Properties
        [ObjectID("ObjectID")]
        public int ObjectID {
            get { return _objectID; }
            set { _objectID = value; }
        }
        [PrimaryKey("Pipe_ID")]
        public string ID {
            get { return _pipeID; }
            set { _pipeID = value; }
        }
        [Shape("Shape")]
        public IGeometry Shape {
            get { return _shape; }
            set { _shape = value; }
        }
        [Property("Pipe_Name", true)]
        public string Name {
            get { return _pipeName; }
            set { _pipeName = value; }
        }
        [Property("Pipe_Pos_Relative", true)]
        public string PositionRelative {
            get { return _pipePosRelative; }
            set { _pipePosRelative = value; }
        }
        [Property("Node_SP_ID", true)]
        public string NodeStartID {
            get { return _nodeSID; }
            set { _nodeSID = value; }
        }
        [Property("TypeNode_SP", true)]
        public string NodeStartType {
            get { return _nodeSType; }
            set { _nodeSType = value; }
        }
        [Property("Node_EP_ID", true)]
        public string NodeEndID {
            get { return _nodeEID; }
            set { _nodeEID = value; }
        }
        [Property("TypeNode_EP", true)]
        public string NodeEndType {
            get { return _nodeEType; }
            set { _nodeEType = value; }
        }
        [Property("Pipe_Type_ID", true)]
        public string PipeTypeID {
            get { return _pipeTypeID; }
            set { _pipeTypeID = value; }
        }
        [Property("Material_ID", true)]
        public string MaterialID {
            get { return _materialID; }
            set { _materialID = value; }
        }
        [Property("Pipe_Length", true)]
        public double Length {
            get { return _length; }
            set { _length = value; }
        }
        [Property("Pipe_Diameter", true)]
        public double Diameter {
            get { return _diameter; }
            set { _diameter = value; }
        }
        [Property("Pipe_Thick", true)]
        public double Thick {
            get { return _thick; }
            set { _thick = value; }
        }
        [Property("Pipe_Thick_In", true)]
        public double ThickIn {
            get { return _thickIn; }
            set { _thickIn = value; }
        }
        [Property("Pipe_SP_Depth", true)]
        public double StartPointDeep {
            get { return _sDeep; }
            set { _sDeep = value; }
        }
        [Property("Pipe_EP_Depth", true)]
        public double EndPointDeep {
            get { return _eDeep; }
            set { _eDeep = value; }
        }
        [Property("Pipe_Img1", true)]
        public string Image1 {
            get { return _img1; }
            set { _img1 = value; }
        }
        [Property("Pipe_Img2", true)]
        public string Image2 {
            get { return _img2; }
            set { _img2 = value; }
        }
        [Property("Pipe_Standard", true)]
        public string Standard {
            get { return _standard; }
            set { _standard = value; }
        }
        [Property("Status_ID", true)]
        public string StatusID {
            get { return _statusID; }
            set { _statusID = value; }
        }
        [Property("ManFac_ID", true)]
        public string ManFacID {
            get { return _manFacID; }
            set { _manFacID = value; }
        }
        [Property("Origin_ID", true)]
        public string OrginID {
            get { return _orginID; }
            set { _orginID = value; }
        }
        [Property("JointType_ID", true)]
        public string JoinTypeID {
            get { return _joinTypeID; }
            set { _joinTypeID = value; }
        }
        [Property("JointType_Std", true)]
        public string JoinTypeStandard {
            get { return _joinTypeStd; }
            set { _joinTypeStd = value; }
        }
        [Property("Pipe_Old", true)]
        public double Old {
            get { return _old; }
            set { _old = value; }
        }
        [Property("Pipe_Value", true)]
        public double Value {
            get { return _pipeValue; }
            set { _pipeValue = value; }
        }
        [Property("Pipe_DateCreate", true)]
        public DateTime DateCreated {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        [Property("Pipe_DateOfUse", true)]
        public DateTime DateUse {
            get { return _dateUse; }
            set { _dateUse = value; }
        }
        [Property("Province_ID", true)]
        public string ProvinceID {
            get { return _provinceID; }
            set { _provinceID = value; }
        }
        [Property("District_ID", true)]
        public string DistrictID {
            get { return _districtID; }
            set { _districtID = value; }
        }
        [Property("Commune_ID", true)]
        public string CommuneID {
            get { return _communeID; }
            set { _communeID = value; }
        }
        [Property("Street_ID", true)]
        public string StreetID {
            get { return _streetID; }
            set { _streetID = value; }
        }
        [Property("Zone_ID", true)]
        public string ZoneID {
            get { return _zoneID; }
            set { _zoneID = value; }
        }
        [Property("Ward_ID", true)]
        public string WardID {
            get { return _wardID; }
            set { _wardID = value; }
        }
        #endregion

        #region Constructors
        public Pipe() { }
        #endregion

        #region Methods
        #endregion

    }
}
