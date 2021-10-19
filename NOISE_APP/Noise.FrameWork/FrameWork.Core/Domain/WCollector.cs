using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.Attributes;
using FrameWork.Core.Base;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("WCollectors", ObjectTypeEnums.Row)]
    public class WCollector {

        #region Fields
        private Node _node;
        private int _objectID;
        private string _collectorID;
        private string _collectorName;
        private string _collectorPosRelative;
        private double _x;
        private double _y;
        private string _collectorTypeID;
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
        [PrimaryKey("WCollector_ID")]
        public string CollectorID {
            get { return _collectorID; }
            set { _collectorID = value; }
        }
        [Property("WCollector_Name", true)]
        public string CollectorName {
            get { return _collectorName; }
            set { _collectorName = value; }
        }
        [Property("WCollector_Pos_Relative", true)]
        public string CollectorPosRelative {
            get { return _collectorPosRelative; }
            set { _collectorPosRelative = value; }
        }
        [Property("WCollectorType_ID", true)]
        public string CollectorTypeID{
            get{return _collectorTypeID;}
            set{_collectorTypeID = value;}
        }
        [Property("WCollector_X", true)]
        public double X {
            get { return _x; }
            set { _x = value; }
        }
        [Property("WCollector_Y", true)]
        public double Y {
            get { return _y; }
            set { _y = value; }
        }
        [Property("WCollector_DateCreate", true)]
        public DateTime DateCreated {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        [Property("WCollector_DateOfUse", true)]
        public DateTime DateUse {
            get { return _dateUse; }
            set { _dateUse = value; }
        }
        [Property("Status_ID", true)]
        public string Status {
            get { return _status; }
            set { _status = value; }
        }
        [Property("WCollector_Img1", true)]
        public string Image1 {
            get { return _img1; }
            set { _img1 = value; }
        }
        [Property("WCollector_Img2", true)]
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
        public WCollector() { }
        #endregion

        #region Methods
        #endregion
    }
}
