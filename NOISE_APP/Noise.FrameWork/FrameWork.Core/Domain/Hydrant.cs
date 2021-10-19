using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.Attributes;
using FrameWork.Core.Base;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("Hydrants", ObjectTypeEnums.Row)]
    public class Hydrant {

        #region Fields
        private Node _node;
        private int _objectID;
        private string _hydrantID;
        private string _hydrantName;
        private string _hydrantPosRelative;
        private double _x;
        private double _y;
        private string _pipeID;
        private double _diameter;
        private double _pressure;
        private string _materialID;
        private string _manFacID;
        private string _orginID;
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
        [PrimaryKey("Hydrant_ID")]
        public string HydrantID {
            get { return _hydrantID; }
            set { _hydrantID = value; }
        }
        [Property("Hydrant_Name", true)]
        public string HydrantName {
            get { return _hydrantName; }
            set { _hydrantName = value; }
        }
        [Property("Hydrant_Pos_Relative", true)]
        public string HydrantPosRelative {
            get { return _hydrantPosRelative; }
            set { _hydrantPosRelative = value; }
        }
        [Property("Hydrant_X", true)]
        public double X {
            get { return _x; }
            set { _x = value; }
        }
        [Property("Hydrant_Y", true)]
        public double Y {
            get { return _y; }
            set { _y = value; }
        }
        [Property("PipeID", true)]
        public string PipeID {
            get { return _pipeID; }
            set { _pipeID = value; }
        }
        [Property("Hydrant_Diameter", true)]
        public double Diameter {
            get { return _diameter; }
            set { _diameter = value; }
        }
        [Property("Hydrant_Pressure", true)]
        public double Pressure {
            get { return _pressure; }
            set { _pressure = value; }
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
        [Property("Hydrant_Standard", true)]
        public string Standard {
            get { return _standard; }
            set { _standard = value; }
        }
        [Property("Hydrant_DateCreate", true)]
        public DateTime DateCreated {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        [Property("Hydrant_DateOfUse", true)]
        public DateTime DateUse {
            get { return _dateUse; }
            set { _dateUse = value; }
        }
        [Property("Status_ID", true)]
        public string Status {
            get { return _status; }
            set { _status = value; }
        }
        [Property("Hydrant_Img1", true)]
        public string Image1 {
            get { return _img1; }
            set { _img1 = value; }
        }
        [Property("Hydrant_Img2", true)]
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
        public Hydrant() { }
        #endregion

        #region Methods
        #endregion
    }
}
