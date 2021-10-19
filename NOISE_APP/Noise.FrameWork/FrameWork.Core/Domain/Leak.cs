using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.Attributes;
using FrameWork.Core.Base;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("Leaks", ObjectTypeEnums.Row)]
    public class Leak{

        #region Fields
        private Node _node;
        private int _objectID;
        private string _leakID;
        private string _leakName;
        private string _leakPosRelative;
        private double _x;
        private double _y;
        private string _pipeID;
        private DateTime _dateDiscovered;
        private DateTime _dateRepaired;
        private string _status;
        private string _preRepaired;
        private string _postRepaired;
        private double _expense;
        private int _time;
        private string _teamID;
        private string _imgPreRepaired;
        private string _imgPostRepaired;
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
        [PrimaryKey("Leak_ID")]
        public string LeakID {
            get { return _leakID; }
            set { _leakID = value; }
        }
        [Property("Leak_Name", true)]
        public string LeakName {
            get { return _leakName; }
            set { _leakName = value; }
        }
        [Property("Leak_Pos_Relative", true)]
        public string LeakPosRelative {
            get { return _leakPosRelative; }
            set { _leakPosRelative = value; }
        }
        [Property("Pipe_ID", true)]
        public string PipeID {
            get { return _pipeID; }
            set { _pipeID = value; }
        }
        [Property("Leak_Date_Discover", true)]
        public DateTime DateDiscovered {
            get { return _dateDiscovered; }
            set { _dateDiscovered = value; }
        }
        [Property("Leak_Date_Repaired", true)]
        public DateTime DateRepaired {
            get { return _dateRepaired; }
            set { _dateRepaired = value; }
        }
        [Property("Leak_Status", true)]
        public string Status {
            get { return _status; }
            set { _status = value; }
        }
        [Property("Leak_Pre_Repair", true)]
        public string PreRepaired {
            get { return _preRepaired; }
            set { _preRepaired = value; }
        }
        [Property("Leak_Post_Repair", true)]
        public string PostRepaired {
            get { return _postRepaired; }
            set { _postRepaired = value; }
        }
        [Property("Leak_Expense", true)]
        public double Expense {
            get { return _expense; }
            set { _expense = value; }
        }
        [Property("Leak_Time", true)]
        public int Time {
            get { return _time; }
            set { _time = value; }
        }
        [Property("Team_ID", true)]
        public string TeamID {
            get { return _teamID; }
            set { _teamID = value; }
        }
        [Property("Leak_Img_Pre", true)]
        public string ImagePreRepaired {
            get { return _imgPreRepaired; }
            set { _imgPreRepaired = value; }
        }
        [Property("Leak_Img_Post", true)]
        public string ImagePostRepaired {
            get { return _imgPostRepaired; }
            set { _imgPostRepaired = value; }
        }
        [Property("Leak_X", true)]
        public double X {
            get { return _x; }
            set { _x = value; }
        }
        [Property("Leak_Y", true)]
        public double Y {
            get { return _y; }
            set { _y = value; }
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
        public Leak() { }
        #endregion

        #region Methods
        #endregion
    }
}
