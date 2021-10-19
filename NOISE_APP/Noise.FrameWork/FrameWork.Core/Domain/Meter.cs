using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.Attributes;
using FrameWork.Core.Base;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("Meters", ObjectTypeEnums.Row)]
    public class Meter {

        #region Fields
        private Node _node;
        private int _objectID;
        private string _meterID;
        private string _meterName;
        private string _meterTypeID;
        private string _meterPosRelative;
        private double _x;
        private double _y;
        private string _materialID;
        private string _manFacID;
        private string _joinTypeID;
        private string _joinTypeStd;
        private double _diameter;
        private string _status;
        private string _mode;
        private double _flow;
        private string _img1;
        private string _img2;
        private double _meterValue;
        private DateTime _dateCreated;
        private DateTime _dateUse;
        private string _statusID;
        private string _orginID;
        private string _provinceID;
        private string _districtID;
        private string _communeID;
        private string _streetID;
        private string _zoneID;
        private string _wardID;
        #endregion

        #region Properties
        [Property("", false)]
        public Node Node {
            get { return _node; }
            set { _node = value; }
        }
        [ObjectID("ObjectID")]
        public int ObjectID {
            get { return _objectID; }
            set { _objectID = value; }
        }
        [PrimaryKey("Meter_ID")]
        public string MeterID {
            get { return _meterID; }
            set { _meterID = value; }
        }
        [Property("Meter_Name", true)]
        public string MeterName {
            get { return _meterName; }
            set { _meterName = value; }
        }
        [Property("MeterType_ID", true)]
        public string MeterTypeID {
            get { return _meterTypeID; }
            set { _meterTypeID = value; }
        }
        [Property("Meter_Pos_Relative", true)]
        public string MeterPosRelative {
            get { return _meterPosRelative; }
            set { _meterPosRelative = value; }
        }
        [Property("Meter_X", true)]
        public double X {
            get { return _x; }
            set { _x = value; }
        }
        [Property("Meter_Y", true)]
        public double Y {
            get { return _y; }
            set { _y = value; }
        }
        [Property("Meter_Diameter", true)]
        public double Diameter {
            get { return _diameter; }
            set { _diameter = value; }
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
        [Property("Meter_DateCreate", true)]
        public DateTime DateCreated {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        [Property("Meter_DateOfUse", true)]
        public DateTime DateUse {
            get { return _dateUse; }
            set { _dateUse = value; }
        }
        [Property("Status_ID", true)]
        public string Status {
            get { return _status; }
            set { _status = value; }
        }
        [Property("Meter_Img1", true)]
        public string Image1 {
            get { return _img1; }
            set { _img1 = value; }
        }
        [Property("Meter_Img2", true)]
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
        public Meter() { }
        #endregion

        #region Methods
        #endregion
    }
}
