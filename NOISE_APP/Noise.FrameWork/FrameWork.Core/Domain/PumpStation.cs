using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.Attributes;
using FrameWork.Core.Base;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("PumpStations", ObjectTypeEnums.Row)]
    public class PumpStation {

        #region Fields
        private Node _node;
        private int _objectID;
        private string _pumpID;
        private string _pumpName;
        private string _pumpTypeID;
        private string _pumpPosRelative;
        private double _x;
        private double _y;
        private int _pumpNumber;
        private int _pumpMotor;
        private int _pumpMeter;
        private int _pumpValve;
        private int _pumpTank;
        private double _flowOut;
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
        [PrimaryKey("PS_ID")]
        public string PumpID {
            get { return _pumpID; }
            set { _pumpID = value; }
        }
        [Property("PS_Name", true)]
        public string PumpName {
            get { return _pumpName; }
            set { _pumpName = value; }
        }
        [Property("PS_Type_ID", true)]
        public string PumpTypeID {
            get { return _pumpTypeID; }
            set { _pumpTypeID = value; }
        }
        [Property("PS_Pos_Relative", true)]
        public string PumpPosRelative {
            get { return _pumpPosRelative; }
            set { _pumpPosRelative = value; }
        }
        [Property("PS_X", true)]
        public double X {
            get { return _x; }
            set { _x = value; }
        }
        [Property("PS_Y", true)]
        public double Y {
            get { return _y; }
            set { _y = value; }
        }
        [Property("PS_NumPump", true)]
        public int NumberOfPump{
            get{return _pumpNumber;}
            set{_pumpNumber=value;}
        }
        [Property("PS_NumMotor", true)]
        public int NumberOfMotor {
            get { return _pumpMotor; }
            set { _pumpMotor = value; }
        }
        [Property("PS_NumMeter", true)]
        public int NumberOfMeter {
            get { return _pumpMeter; }
            set { _pumpMeter = value; }
        }
        [Property("PS_NumTank", true)]
        public int NumberOfTank {
            get { return _pumpTank; }
            set { _pumpTank = value; }
        }
        [Property("PS_Flow_out", true)]
        public double FlowOut {
            get { return _flowOut; }
            set { _flowOut = value; }
        }
        [Property("PS_NumValve", true)]
        public int NumberOfValve {
            get { return _pumpValve; }
            set { _pumpValve = value; }
        }
        [Property("PS_DateCreate", true)]
        public DateTime DateCreated {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        [Property("PS_DateOfUse", true)]
        public DateTime DateUse {
            get { return _dateUse; }
            set { _dateUse = value; }
        }
        [Property("Status_ID", true)]
        public string Status {
            get { return _status; }
            set { _status = value; }
        }
        [Property("PS_Img1", true)]
        public string Image1 {
            get { return _img1; }
            set { _img1 = value; }
        }
        [Property("PS_Img2", true)]
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
        public PumpStation() { }
        #endregion

        #region Methods
        #endregion
    }
}
