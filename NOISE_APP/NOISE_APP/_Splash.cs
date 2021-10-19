using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace NOISE_APP
{
    public partial class _Splash : SplashScreen
    {
        public _Splash()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
            SplashScreenCommand command = (SplashScreenCommand)cmd;
            if (command == SplashScreenCommand.SetLabel)
            {
                lblStatus.Text = arg?.ToString();
            }
        }

        #endregion

        public enum SplashScreenCommand
        {
            SetLabel
        }
    }
}