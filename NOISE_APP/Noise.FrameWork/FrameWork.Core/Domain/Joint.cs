using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.Attributes;
using FrameWork.Core.Base;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("Joints", ObjectTypeEnums.Row)]
    public class Joint{

        #region Fields
        private Node _node;
        private int _objectID;
        private string _jointID;
        private string _jointName;
        private string _jointPosRelative;
        private string _joinTypeID;
        private double _x;
        private double _y;
        private string _materialID;
        private string _manFacID;
        private string _orginID;
        private string _mode;
        private string _classID;
        private double _length;
        private string _bulongTypeID;
        private string _bulongStandard;
        private string _jjoinTypeID;
        private string _joinTypeStd;
        private string _standard;
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
        #endregion

        #region Properties
        [Property("",false)]
        public Node Node {
            get { return _node; }
            set { _node = value; }
        }
        [ObjectID("ObjectID")]
        public int ObjectID {
            get { return _objectID; }
            set { _objectID = value; }
        }
        [PrimaryKey("Joint_ID")]
        public string JointID {
            get { return _jointID; }
            set { _jointID = value; }
        }
        [Property("Joint_Name", true)]
        public string JointName {
            get { return _jointName; }
            set { _jointName = value; }
        }
        [Property("Joint_Pos_Relative", true)]
        public string JointPosRelative {
            get { return _jointPosRelative; }
            set { _jointPosRelative = value; }
        }
        [Property("Joint_X", true)]
        public double X {
            get { return _x; }
            set { _x = value; }
        }
        [Property("Joint_Y", true)]
        public double Y {
            get { return _y; }
            set { _y = value; }
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
        [Property("Node_Mode", true)]
        public string Mode {
            get { return _mode; }
            set { _mode = value; }
        }
        [Property("NodeClass_ID", true)]
        public string ClassID {
            get { return _classID; }
            set { _classID = value; }
        }
        [Property("Node_Length", true)]
        public double Length {
            get { return _length; }
            set { _length = value; }
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
        [Property("JointType_ID", true)]
        public string JoinTypeID {
            get { return _jjoinTypeID; }
            set { _jjoinTypeID = value; }
        }
        [Property("JointType_Std", true)]
        public string JoinTypeStd {
            get { return _joinTypeStd; }
            set { _joinTypeStd = value; }
        }
        [Property("Node_Standard", true)]
        public string Standard {
            get { return _standard; }
            set { _standard = value; }
        }
        [Property("Node_DateCreate", true)]
        public DateTime DateCreated {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        [Property("Node_DateOfUse", true)]
        public DateTime DateUse {
            get { return _dateUse; }
            set { _dateUse = value; }
        }
        [Property("Status_ID", true)]
        public string Status {
            get { return _status; }
            set { _status = value; }
        }
        [Property("Node_Img1", true)]
        public string Image1 {
            get { return _img1; }
            set { _img1 = value; }
        }
        [Property("Node_Img2", true)]
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
        #endregion

        #region Constructor
        public Joint() { }
        #endregion

        #region Methods
        
        #endregion
    }
}
