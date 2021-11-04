using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraSplashScreen;
using Noise.Common.GIS.Classes;
using NOISE_APP.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOISE_APP
{
    class _NoiseProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.XtraEditors.WindowsFormsSettings.DefaultFont = new Font("Tahoma", 8);
            DevExpress.XtraEditors.WindowsFormsSettings.DefaultMenuFont = new Font("Tahoma", 8);
            DevExpress.XtraEditors.WindowsFormsSettings.DefaultPrintFont = new Font("Tahoma", 8);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            //
            new _NoiseProgram();
        }

        private _NoiseProgram()
        {
            Application.Run(new ImportExcel());
            /*
            try
            {
                SplashScreenManager.ShowForm(typeof(_Splash));
                //
                NoiseArcLicense ArcLicense = new NoiseArcLicense();
                if (ArcLicense.CheckLicence())
                {
                    SplashScreenManager.CloseForm();

                    //Application.Run(new frmExportGDB());
                   
                    //Application.Run(new XtraForm2());
                    //Application.Run(new NOISE_APP.Forms.FormMain());
                    //Application.Run(new FormNoise());
                    //if (new FrmLogin().ShowDialog() == DialogResult.OK)
                    //{
                    //    Application.Run(new FrmMain());
                    //    //Application.Run(new FormNoise());
                    //}

                    //Application.Exit();
                }
                else
                {
                    _NoiseMesageBox.ShowErrorMessage($"Không thể khởi tạo licence ArcGIS");
                }
                
            }
            catch (System.Runtime.InteropServices.COMException COMEx)
            {
                MessageBox.Show(COMEx.Message, "COM Error: " + COMEx.ErrorCode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            */
        }
    }
}
