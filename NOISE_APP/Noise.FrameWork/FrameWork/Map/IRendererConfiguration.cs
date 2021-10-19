using System;
using System.Text;

using ESRI.ArcGIS.Display;

namespace FrameWork.Map {
    public interface IRendererConfiguration {
        double Value { get; set; }
        ISymbol Symbol { get; set; }
        string Text { get; set; }
        double Size { get; set; }
    }

    public class RendererConfiguration : IRendererConfiguration {
        private double _value;
        private ISymbol _symbol;
        private string _text;
        private double _size;

        public RendererConfiguration(double value, ISymbol symbol, string text,double size) {
            _value = value;
            _symbol = symbol;
            _text = text;
            _size = size;
        }
        public RendererConfiguration() { }

        public double Value {
            get { return _value; }
            set { _value = value; }
        }

        public ISymbol Symbol {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public string Text {
            get { return _text; }
            set { _text = value; }
        }

        public double Size {
            get { return _size; }
            set { _size = value; }
        }

    }
}
