using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOISE_APP
{

    public class _NoiseMesageBox
    {
        private static Stopwatch _stopWatch;

        public static int ShowErrorMessage(string msg)
        {
            try
            {
                if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null && DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
            catch
            {
                throw;
            }

            XtraMessageBox.Show(msg, "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            return 1;
        }
        public static int ShowInfoMessage(string msg)
        {
            try
            {
                if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null && DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                XtraMessageBox.Show(msg, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
            }

            return 1;
        }
        public static int ShowInfoMessage(string msg, string info)
        {
            try
            {
                if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null && DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
            catch
            {
            }
            XtraMessageBox.Show(msg, info,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return 1;
        }
        public static System.Windows.Forms.DialogResult ShowYesNoMessage(string msg)
        {
            try
            {
                if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null && DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
            catch
            {
                throw;
            }
            return XtraMessageBox.Show(msg, "Thông báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static System.Windows.Forms.DialogResult ShowYesNoCancelMessage(string msg)
        {
            try
            {
                if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null && DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
            catch
            {
                throw;
            }
            return XtraMessageBox.Show(msg, "CEMS",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        public static int ShowErrorMessage(string p, string p_2, object p_3, object p_4)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public static void SendSplashCommand(Enum cmd, object arg)
        {
            if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null)
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SendCommand(cmd, arg);
        }
        public static void ShowSplash(Form form, string caption = "", string description = "", bool cancellable = false, DevExpress.XtraSplashScreen.ParentFormState parentState = DevExpress.XtraSplashScreen.ParentFormState.Locked)
        {
            HideSplash();
            //
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(form, cancellable ? typeof(_ProgressCancellable) : typeof(_Progress), true, true, parentState);
            if (!string.IsNullOrWhiteSpace(caption))
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption(caption);
            if (!string.IsNullOrWhiteSpace(description))
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription(description);
        }

        public static void SetSplashDescription(string description)
        {
            if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null
                && DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription(description);
        }

        public static void SetSplashCaption(string caption)
        {
            if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null
                && DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption(caption);
        }
        public static void HideSplash()
        {
            try
            {
                if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null
                    && DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
            catch
            {
                throw;
            }
        }
        public static void SendCommand(Enum e, object arg = null)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SendCommand(e, arg);
        }
        public static void StartCounting()
        {
            _stopWatch = System.Diagnostics.Stopwatch.StartNew();
        }

        public static long StopCounting()
        {
            if (_stopWatch != null && _stopWatch.IsRunning)
            {
                _stopWatch.Stop();
                return _stopWatch.ElapsedMilliseconds;
            }
            else
                return 0;
        }
    }
}

