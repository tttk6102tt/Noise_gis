using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.Attributes;
using FrameWork.Core.Base;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("Valves",ObjectTypeEnums.Row)]
    public class Valve {

        #region Fields
        private Node _node;
        private int _objectID;
        private string _valveID;
        private string _valveName;
        private string _valveTypeID;
        private string _valvePosRelative;
        private double _x;
        private double _y;
        private double _diameter;
        private double _deep;
        private double _dimesion;
        private bool _isLeft;
        private double _rNumber;
        private string _materialID;
        private string _manFacID;
        private string _orginID;
        private string _joinTypeID;
        private string _joinTypeStd;
        private string _standard;
        private string _bulongTypeID;
        private string _bulongStandard;
        private string _typeSetupID;
        private DateTime _dateCreated;
        private DateTime _dateUse;
        private string _status;
        private string _img1;
        private string _img2;
        private string _provinceID;
        private string _districtID;
        private string _communeID;
        private string _streetID;
        private string _zoneID;
        private string _wardID;
        private string _nodeID;
        #endregion

        #region Properties
        [LinkTo("Nodes", "Node_ID", "FrameWork.Core.Domain", ObjectTypeEnums.Feature)]
        public Node Node {
            get { return _node; }
            set { _node = value; }
        }
        [ObjectID("ObjectID")]
        public int ObjectID {
            get { return _objectID; }
            set { _objectID = value; }
        }
        [PrimaryKey("Valve_ID")]
        public string ValveID { 
            get { return _valveID; } 
            set { _valveID = value; } 
        }
        [Property("Valve_Name", true)]
        public string ValveName {
            get { return _valveName; }
            set { _valveName = value; }
        }
        [Property("TypeValve_ID", true)]
        public string ValveTypeID {
            get { return _valveTypeID; }
            set { _valveTypeID = value; }
        }
        [Property("Valve_Pos_Relative", true)]
        public string ValvePosRelative {
            get { return _valvePosRelative; }
            set { _valvePosRelative = value; }
        }
        [Property("Valve_X", true)]
        public double X {
            get { return _x; }
            set { _x = value; }
        }
        [Property("Valve_Y", true)]
        public double Y {
            get { return _y; }
            set { _y = value; }
        }
        [Property("Valve_Diameter", true)]
        public double Diameter {
            get { return _diameter; }
            set { _diameter = value; }
        }
        [Property("Vale_Depth", true)]
        public double Deep {
            get { return _deep; }
            set { _deep = value; }
        }
        [Property("Valve_Dimension", true)]
        public double Dimesion {
            get { return _dimesion; }
            set { _dimesion = value; }
        }
        [Property("Valve_Left", true)]
        public bool IsLeft {
            get { return _isLeft; }
            set { _isLeft = value; }
        }
        [Property("R_Num", true)]
        public double RNumber {
            get { return _rNumber; }
            set { _rNumber = value; }
        }
        [Property("Material_ID", true)]
        public string MaterialID {
            get { return _materialID; }
            set { _materialID = value; }
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
        public string JoinTypeStd {
            get { return _joinTypeStd; }
            set { _joinTypeStd = value; }
        }
        [Property("Valve_Standard", true)]
        public string Standard {
            get { return _standard; }
            set { _standard = value; }
        }
        [Property("TypeBulong_ID", true)]
        public string BulongTypeID {
            get { return _bulongTypeID; }
            set { _bulongTypeID = value; }
        }
        [Property("Std_Bulong", true)]
        public string BulongStandard {
            get { return _bulongStandard; }
            set { _bulongStandard = value; }
        }
        [Property("Type_Setup_ID", true)]
        public string TypeSetupID {
            get { return _typeSetupID; }
            set { _typeSetupID = value; }
        }
        [Property("Valve_DateCreate", true)]
        public DateTime DateCreated {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        [Property("Valve_DateOfUse", true)]
        public DateTime DateUse {
            get { return _dateUse; }
            set { _dateUse = value; }
        }
        [Property("Status_ID", true)]
        public string Status {
            get { return _status; }
            set { _status = value; }
        }
        [Property("Valve_Img1", true)]
        public string Image1 {
            get { return _img1; }
            set { _img1 = value; }
        }
        [Property("Valve_Img2", true)]
        public string Image2 {
            get { return _img2; }
            set { _img2 = value; }
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
        [Property("Node_ID",true)]
        public string NodeID {
            get { return _nodeID; }
            set { _nodeID = value; }
        }
        #endregion

        #region Constructor
        public Valve() {}
        #endregion

        #region Methods
        #endregion
    }
}
