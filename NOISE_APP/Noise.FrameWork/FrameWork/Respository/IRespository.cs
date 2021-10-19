using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using FrameWork.Data;

namespace FrameWork.Respository {
    public interface IRespository<T>  {
        T CreateNew();
        T GetByID(int id);
        IList<T> GetList(IQueryFilter qry);
        BindingTable GetBindingTable(IQueryFilter qry);
    }
}
