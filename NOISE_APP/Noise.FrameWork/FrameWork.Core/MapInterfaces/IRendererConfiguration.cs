using System;
using System.Text;

using ESRI.ArcGIS.Display;

namespace FrameWork.Core.MapInterfaces {
    public interface IRendererConfiguration {
        double Value { get; set; }
        ISymbol Symbol { get; set; }
        string TextValue { get; set; }
        string Text { get; set; }
        double Size { get; set; }
    }

    public class RendererConfiguration : IRendererConfiguration {
        private double _value;
        private ISymbol _symbol;
        private string _text;
        private string _textValue;
        private double _size;

        public RendererConfiguration(double value, ISymbol symbol,string textValue, string text,double size) {
            _value = value;
            _symbol = symbol;
            _textValue = textValue;
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

        public string TextValue
        {
            get { return _textValue; }
            set { _textValue = value; }
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
