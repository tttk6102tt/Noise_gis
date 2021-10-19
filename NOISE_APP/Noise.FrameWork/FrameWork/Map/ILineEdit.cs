using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork.Map {
    public interface ILineEdit {
        void Move(double x, double y);
        void Stop();
        void Add(double x, double y);
    }
}
