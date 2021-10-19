using System;
using FrameWork.Core.MapInterfaces;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
namespace FrameWork.Map.Utilities {

    
    public class ToolbarUtilitesPageControl {

        #region Fields
        private AxMapControl _axMap;
        private AxPageLayoutControl _axLayout;
        private IMap _map;
        private IPageLayout _PageLayout;
        private IActiveView _activeView;
        private IActiveView _activeView_layout;
        private IPoint _point = new PointClass();
        private INewEnvelopeFeedback _feedbackEnv;
        private BaseMapAction _baseAct;
        private bool _isMouseDown;
        #endregion

        #region Constructor
        public ToolbarUtilitesPageControl() { }
        public ToolbarUtilitesPageControl(AxMapControl axMap, AxPageLayoutControl _axLayout)
        {
            _axMap = axMap;
            _map = _axMap.Map;
            this._axLayout = _axLayout;
            _PageLayout = this._axLayout.PageLayout;
            _activeView = _map as IActiveView;
            _activeView_layout = _PageLayout as IActiveView;
            //_measurement = new MeasureTool(_map);
        }

        public BaseMapAction MapAction
        {
            set
            {
                _baseAct = value;
            }
            get
            {
                return _baseAct;
            }
        }
        #endregion

        #region IToolbarAction Members

        public void MouseDownAction(MouseEventInfo me)
        {
            int iBaseAct = (int)_baseAct;
            switch (iBaseAct)
            {
                case (int)BaseMapAction.Select:
                    break;
                case (int)BaseMapAction.SelectByLocation:
                    break;
                case (int)BaseMapAction.ZoomIn:
                    _point = _activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(me.x, me.y);
                    _isMouseDown = true;
                    break;
                case (int)BaseMapAction.ZoomOut:
                    _point = _activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(me.x, me.y);
                    _isMouseDown = true;
                    break;
                case (int)BaseMapAction.Pan:
                    ESRI.ArcGIS.Display.IScreenDisplay m_pScreenDisplay = _activeView.ScreenDisplay;
                    IPoint pStartPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(me.x, me.y);
                    IMap hitMap = _activeView.HitTestMap(pStartPoint);
                    if (hitMap == null)
                        return;
                    _activeView.ScreenDisplay.PanStart(pStartPoint);
                    break;
                case (int)BaseMapAction.Identify:
                    break;
                case (int)BaseMapAction.Distance:
                    break;
                default:
                    break;
            }
        }

        public void MouseMoveAction(MouseEventInfo me)
        {
            int iBaseAct = (int)_baseAct;
            switch (iBaseAct)
            {
                case (int)BaseMapAction.Select:
                    break;
                case (int)BaseMapAction.ZoomIn:
                    if (!_isMouseDown) return;
                    DrawFeedBack(me.x, me.y);
                    break;
                case (int)BaseMapAction.ZoomOut:
                    if (!_isMouseDown) return;
                    DrawFeedBack(me.x, me.y);
                    break;
                case (int)BaseMapAction.Pan:
                    ESRI.ArcGIS.Display.IScreenDisplay m_pScreenDisplay = _activeView.ScreenDisplay;
                    IPoint pMoveToPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(me.x, me.y);
                    m_pScreenDisplay.PanMoveTo(pMoveToPoint);
                    break;
                case (int)BaseMapAction.Identify:
                    break;
                case (int)BaseMapAction.Distance:
                    break;
            }
        }
        public void MouseMoveAction(MouseEventInfo me, ref double ms, ref double total, ref string unit)
        {
            int iBaseAct = (int)_baseAct;
            switch (iBaseAct)
            {
                case (int)BaseMapAction.ZoomIn:
                    if (!_isMouseDown) return;
                    DrawFeedBack(me.x, me.y);
                    break;
                case (int)BaseMapAction.ZoomOut:
                    if (!_isMouseDown) return;
                    DrawFeedBack(me.x, me.y);
                    break;
                case (int)BaseMapAction.Pan:
                    ESRI.ArcGIS.Display.IScreenDisplay m_pScreenDisplay = _activeView.ScreenDisplay;
                    IPoint pMoveToPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(me.x, me.y);
                    m_pScreenDisplay.PanMoveTo(pMoveToPoint);
                    break;
                case (int)BaseMapAction.Distance:
                    ms = 0.0;
                    total = 0;
                    
                    break;
            }
        }
        public void MouseUpAction(MouseEventInfo me)
        {
            int iBaseAct = (int)_baseAct;
            switch (iBaseAct)
            {
                case (int)BaseMapAction.Select:
                    break;
                case (int)BaseMapAction.ZoomIn:
                    IEnvelope pEnvIn = GetInEnvelope(_feedbackEnv);

                    // Implement "can't in out beyond extent" functionality
                    if ((pEnvIn.Width > 0) && (pEnvIn.Height > 0))
                    {
                        if (!_activeView.FullExtent.IsEmpty)
                        {
                            // apply ZoomIn only within 1 millionth of the full extent
                            if (((_activeView.FullExtent.Width / 1000000) < pEnvIn.Width) && ((_activeView.FullExtent.Height / 1000000) < pEnvIn.Height))
                            {
                                // zoom in
                                _activeView.Extent = pEnvIn;
                                _activeView.Refresh();
                            }
                        }
                    }

                    // reset rubberband and mousedown state
                    _feedbackEnv = null;
                    _isMouseDown = false;
                    break;
                case (int)BaseMapAction.ZoomOut:
                    IEnvelope pEnvOut = GetOutEnvelope(_feedbackEnv);
                    if (pEnvOut == null) return;
                    _activeView.Extent = pEnvOut;
                    _activeView.Refresh();
                    // reset rubberband and mousedown state
                    _feedbackEnv = null;
                    _isMouseDown = false;
                    break;
                case (int)BaseMapAction.Pan:
                    IScreenDisplay m_pScreenDisplay = _activeView.ScreenDisplay;
                    IEnvelope pEnvelope = m_pScreenDisplay.PanStop();
                    //					if(pEnvelope!=null)
                    //						_activeView.Extent = pEnvelope;
                    if (pEnvelope != null)
                    {
                        _activeView.ScreenDisplay.DisplayTransformation.VisibleBounds = pEnvelope;
                        _activeView.ScreenDisplay.Invalidate(null, true, (short)esriScreenCache.esriAllScreenCaches);
                    }
                    break;
                case (int)BaseMapAction.Identify:
                    //Identify(m_axMap, me.x, me.y);
                    break;
                case (int)BaseMapAction.Distance:
                    break;
                default:
                    break;
            }
        }

        public void DoubleClickAction(MouseEventInfo me)
        {
            try
            {
                int iBaseAct = (int)_baseAct;

                switch (iBaseAct)
                {
                    case (int)BaseMapAction.Distance:
                        break;
                    default:
                        break;
                }
            }
            catch { }
            finally { }
        }

        public void ZoomIn(double x, double y) {
            IRubberBand rubber = new RubberRectangularPolygonClass();
            IEnvelope env = rubber.TrackNew(_activeView.ScreenDisplay, null).Envelope;
            if (!env.IsEmpty) {
                _activeView.Extent = env;
                _activeView_layout.Extent = env;
            } else {
                env = _activeView.Extent;
                
                env.Expand(0.5, 0.5, true);
                IPoint p = new PointClass();
                p.PutCoords(x, y);
                env.CenterAt(p);
                _activeView.Extent = env;
                _activeView_layout.Extent = env;
            }
        }

        public void ZoomOut(double x, double y) {
            IRubberBand rubber = new RubberRectangularPolygonClass();
            IEnvelope env = rubber.TrackNew(_activeView.ScreenDisplay, null).Envelope;
            if (!env.IsEmpty) {
                double scale = 2 - ((env.Height * env.Width) / (_activeView.Extent.Width * _activeView.Extent.Height));
                env = _activeView.Extent;
                env.Expand(scale, scale, true);
                _activeView.Extent = env;
                _activeView_layout.Extent = env;
            } else {
                env = _activeView.Extent;
                env.Expand(1.2, 1.2, true);
                IPoint p = new PointClass();
                p.PutCoords(x, y);
                env.CenterAt(p);
                _activeView.Extent = env;
                _activeView_layout.Extent = env;
            }
        }

        public void FixZoomIn() {
            IEnvelope env = _activeView.Extent;
            env.Expand(0.5, 0.5, true);
            _activeView.Extent = env;
            _activeView.Refresh();
        }

        public void FixZoomOut() {
            IEnvelope env = _activeView.Extent;
            env.Expand(1.2, 1.2, true);
            _activeView.Extent = env;
            _activeView.Refresh();
        }

        public void Pan() {
            _activeView.ScreenDisplay.TrackPan();
        }

        public void Select() {
            throw new NotImplementedException();
        }

        public void FullExtent() {
            _activeView.Extent = _activeView.FullExtent;
        }

        public void NextAction() {
            IExtentStack extentStack = _activeView.ExtentStack;
            if (extentStack.CanRedo()) {
                extentStack.Redo();
            }
        }

        public void BackAction() {
            IExtentStack extentStack = _activeView.ExtentStack;
            if (extentStack.CanUndo()) {
                extentStack.Undo();
            }
        }



       
        #endregion

        #region Private




        private IEnvelope GetInEnvelope(INewEnvelopeFeedback pFeedbackEnv)
        {
            IEnvelope pEnvIn;
            // If user dragged an envelope, use it to calculate new extent
            if (pFeedbackEnv != null)
                pEnvIn = pFeedbackEnv.Stop();
            else
            {
                pEnvIn = _activeView.Extent;
                pEnvIn.Expand(0.5, 0.5, true);
            }
            return pEnvIn;
        }

        private IEnvelope GetOutEnvelope(INewEnvelopeFeedback pFeedbackEnv)
        {
            Double newWidth, newheight;
            IEnvelope extentEnv = null;

            // If user dragged an envelope, use it to calculate new extent
            if (pFeedbackEnv != null)
            {
                IEnvelope pEnvOut = pFeedbackEnv.Stop();
                extentEnv = new EnvelopeClass() as IEnvelope;
                if ((pEnvOut.Width > 0) && (pEnvOut.Height > 0))
                {
                    // Calculate the new width and height
                    newWidth = _activeView.Extent.Width * (_activeView.Extent.Width / pEnvOut.Width);
                    newheight = _activeView.Extent.Height * (_activeView.Extent.Height / pEnvOut.Height);

                    // Construct a new envelope that has the new extents
                    extentEnv.XMin = _activeView.Extent.XMin - ((pEnvOut.XMin - _activeView.Extent.XMin) * (_activeView.Extent.Width / pEnvOut.Width));
                    extentEnv.YMin = _activeView.Extent.YMin - ((pEnvOut.YMin - _activeView.Extent.YMin) * (_activeView.Extent.Height / pEnvOut.Height));
                    extentEnv.Width = newWidth;
                    extentEnv.Height = newheight;
                }
            }
            //						// Else, zoom out from mouse click
            else
            {
                extentEnv = _activeView.Extent;
                extentEnv.Expand(2.0, 2.0, true);
                extentEnv.CenterAt(_point);
            }
            return extentEnv;
        }

        private void DrawFeedBack(int x, int y)
        {
            // Create a rubber banding box, if it hasn't been created already
            if (_feedbackEnv == null)
            {
                _feedbackEnv = new NewEnvelopeFeedbackClass();
                _feedbackEnv.Display = _activeView.ScreenDisplay;
                _feedbackEnv.Start(_point);
            }

            // Store current point, and use to move rubberband
            _point = _activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            _feedbackEnv.MoveTo(_point);
        }

        private string ReturnValue(string oldS, string s, int index)
        {
            string newS = "";
            try { newS = string.Format("1:{0}", Convert.ToDouble(s.Substring(index)).ToString()); }
            catch { newS = oldS; }
            return newS;
        }
        #endregion

        #region IDisposable Members

        public void Dispose() {
            _map = null;
            _activeView = null;
        }

        #endregion
    }
}
