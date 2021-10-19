using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace FrameWork.Data {
    public class DataCache {
        private Dictionary<string,IFeatureClass> _classCached;
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
            _classCached = new Dictionary<string, IFeatureClass>();
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
                    if (!_classCached.ContainsKey(fc.AliasName)) {
                        _classCached.Add(fc.AliasName, fc);
                    }
                } else if (layer is ICompositeLayer) {
                    ICompositeLayer cpLayer = layer as ICompositeLayer;
                    for (int i = 0; i < cpLayer.Count; i++) {
                        stkLayers.Push(cpLayer.get_Layer(i));
                    }
                }
            }
        }
        public void Add(IFeatureClass fc, string name) {
            _classCached.Add(name, fc);
        }
        public void Clear() {
            _classCached.Clear();
        }
        public IFeatureClass GetClass(string name) {
            if (_classCached.ContainsKey(name)) {
                return _classCached[name];
            }
            return null;
        }
    }
}
