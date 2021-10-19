using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using FrameWork.Core.Base;

namespace FrameWork.Core.DataInterfaces {
    public interface IRespository<T>  {
        void Save(T obj);
        void Update(T obj);
        T GetByObjectID(T obj);
        T GetByCodeID(T obj);
        IList<T> GetList(IQueryFilter qry);
        BindingTable GetBindingTable(IQueryFilter qry);
    }
}
