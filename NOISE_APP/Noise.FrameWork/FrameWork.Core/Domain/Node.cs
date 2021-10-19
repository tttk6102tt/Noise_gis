using System;
using ESRI.ArcGIS.Geodatabase;
using FrameWork.Core.Base;
using FrameWork.Core.Attributes;
using ESRI.ArcGIS.Geometry;

namespace FrameWork.Core.Domain {
    [ObjectAttribute("Nodes", ObjectTypeEnums.Feature)]
    public class Node {
        #region Fields
        private int _objectID;
        private IGeometry _point;
        private string _nodeID;
        private string _nodeTypeID;
        #endregion

        #region Properties
        [ObjectID("ObjectID")]
        public int ObjectID {
            get { return _objectID; }
            set { _objectID = value; }
        }
        [PrimaryKey("Node_ID")]
        public string ID {
            get { return _nodeID; }
            set { _nodeID = value; }
        }
        [Property("NodeType_ID",true)]
        public string NodeTypeID {
            get { return _nodeTypeID; }
            set { _nodeTypeID = value; }
        }
        [Shape("Shape")]
        public IGeometry Shape {
            get { return _point; }
            set { _point = value; }
        }
        [Property("", false)]
        public double X {
            get {
                IPoint p = _point as IPoint;
                return p.X;    
            }
        }
        [Property("", false)]
        public double Y {
            get {
                IPoint p = _point as IPoint;
                return p.Y;
            }
        }
        #endregion

        #region Constructors
        public Node() { }
        #endregion

        #region Methods
        #endregion
    }
}