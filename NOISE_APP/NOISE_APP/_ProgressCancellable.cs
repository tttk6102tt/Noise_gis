using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;

namespace NOISE_APP
{
    public partial class _ProgressCancellable : WaitForm
    {
        public interface ILocked
        {
            bool IsCanceled { get; set; }
        }

        public class Locker : ILocked
        {
            //// Fields...

            private bool _IsCanceled;

            public bool IsCanceled
            {
                get { return _IsCanceled; }
                set { _IsCanceled = value; }
            }
            public Locker()
            {
                _IsCanceled = false;
            }
        }
        
        public _ProgressCancellable()
        {
            InitializeComponent();
            //
            btnCancel.Click += (object sender, EventArgs e) =>
            {
                ((DevExpress.XtraEditors.SimpleButton)sender).Enabled = false;
                //
                if (locker != null)
                    locker.IsCanceled = true;
                lblStatus.Text = "Đang hủy bỏ thao tác cuối...";
            };
        }

        #region Overrides
        public static Locker locker;

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            lblStatus.Text = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
            switch ((Commands)cmd)
            {
                case Commands.AwaitDispose:
                    locker = arg as Locker;
                    break;
                case Commands.ShowProgress:
                    lciProgress.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case Commands.HideProgress:
                    lciProgress.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    break;
                case Commands.SetProgress:
                    double vl = 0.0;
                    if (double.TryParse(arg?.ToString(), out vl))
                        pbcMain.EditValue = vl * 100;
                    break;
            }
        }

        #endregion

        public enum Commands
        {
            AwaitDispose,
            SetProgress,
            ShowProgress,
            HideProgress
        }
    }
}