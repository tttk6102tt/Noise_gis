using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FrameWork.Core.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class ObjectAttribute : Attribute{
        private string _name;
        private ObjectTypeEnums _objectType;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public ObjectTypeEnums ObjectType {
            get { return _objectType; }
            set { _objectType = value; }
        }

        public ObjectAttribute(string name,ObjectTypeEnums objectType) {
            _name = name;
            _objectType = objectType;
        }
    }

    public class PropertyAttribute : Attribute {
        private string _name;
        private bool _editAble;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        public bool EditAble {
            get { return _editAble; }
            set { _editAble = value; }
        }
        public PropertyAttribute() {}
        public PropertyAttribute(string columnName,bool editAble) {
            _name = columnName;
            _editAble = editAble;
        }
    }

    public class ShapeAttribute : Attribute {
        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        public ShapeAttribute() {}
        public ShapeAttribute(string columnName) {
            _name = columnName;
        }
    }

    public class ObjectIDAttribute : Attribute {
        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        public ObjectIDAttribute() { }
        public ObjectIDAttribute(string name) {
            _name = name;
        }
    }

    public class PrimaryKeyAttribute : Attribute {
        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        public PrimaryKeyAttribute(){}
        public PrimaryKeyAttribute(string name) {
            _name = name;
        }
    }

    public class LinkToAttribute : Attribute {
        private string _className;
        private string _fieldName;
        private string _assemblyName;
        ObjectTypeEnums _objectType;
        public string ClassName {
            get { return _className; }
            set { _className = value; }
        }
        public string FieldName {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        public string AssemblyName {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }
        public ObjectTypeEnums ObjectType {
            get { return _objectType; }
            set { _objectType = value; }
        }
        public LinkToAttribute(){}
        public LinkToAttribute(string name,string field,string assemblyName,ObjectTypeEnums objectType) {
            _className = name;
            _fieldName = field;
            _assemblyName = assemblyName;
            _objectType = objectType;
        }
    }

}
