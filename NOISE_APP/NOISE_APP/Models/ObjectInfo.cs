using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOISE_APP.Models
{
    public class ObjectInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Name4 { get; set; }
        public string Name5 { get; set; }

        public ObjectInfo(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        public ObjectInfo(string id, string name1, string name2)
        {
            this.ID = id;
            this.Name = name1;
            this.Name2 = name2;
        }
        public ObjectInfo(string id, string name1, string name2, string name3)
        {
            this.ID = id;
            this.Name = name1;
            this.Name2 = name2;
            this.Name3 = name3;
        }
        public ObjectInfo(string id, string name1, string name2, string name3, string name4)
        {
            this.ID = id;
            this.Name = name1;
            this.Name2 = name2;
            this.Name3 = name3;
            this.Name4 = name4;
        }
        public ObjectInfo(string id, string name1, string name2, string name3, string name4, string name5)
        {
            this.ID = id;
            this.Name = name1;
            this.Name2 = name2;
            this.Name3 = name3;
            this.Name4 = name4;
            this.Name5 = name5;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
