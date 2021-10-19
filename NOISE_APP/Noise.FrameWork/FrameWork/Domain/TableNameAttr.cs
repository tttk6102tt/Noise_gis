using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FrameWork.Domain {
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttr : Attribute{
        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        public TableNameAttr(string name) {
            _name = name;
        }
    }
}
