using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace FrameWork.Data.DB {
    public class DataCache {
        private IDictionary<string,object> _cachedList;
        public DataCache() {
            Init();
        }
        public static DataCache Instance {
            get {
                return Nested.DataCache;
            }
        }
        class Nested{
            static Nested() { }
            internal static readonly DataCache DataCache = new DataCache();
        }
        private void Init() {
            _cachedList = new Dictionary<string, object>();
        }
        public void FillFromMap(IMap map) {
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < map.LayerCount; i++) {
                stkLayers.Push(map.get_Layer(i));
            }
            while (stkLayers.Count > 0) {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer) {
                    IFeatureClass fc = ((IFeatureLayer)layer).FeatureClass;
                    if (!_cachedList.ContainsKey(fc.AliasName)) {
                        _cachedList.Add(fc.AliasName, fc);
                    }
                } else if (layer is ICompositeLayer) {
                    ICompositeLayer cpLayer = layer as ICompositeLayer;
                    for (int i = 0; i < cpLayer.Count; i++) {
                        stkLayers.Push(cpLayer.get_Layer(i));
                    }
                }
            }
        }
        public void Add(object fc, string name) {
            _cachedList.Add(name, fc);
        }
        public void Clear() {
            _cachedList.Clear();
        }
        public object GetData(string name) {
            if (_cachedList.ContainsKey(name)) {
                return _cachedList[name];
            }
            return null;
        }
    }
}
