using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noise.Common.GIS.Classes
{
   public class NoiseArcLicense
    {
        IAoInitialize mAoInitialize;

        public NoiseArcLicense()
        {
        }
        private static bool Initialize(ProductCode product, esriLicenseProductCode esriLicenseProduct)
        {
            if (RuntimeManager.Bind(product))
            {
                IAoInitialize aoInit = new AoInitializeClass();
                aoInit.Initialize(esriLicenseProduct);
                return true;
            }
            return false;
        }
        public bool CheckLicence()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            mAoInitialize = new AoInitialize();
            esriLicenseStatus licenseStatus = mAoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngine);
            esriLicenseStatus licenseStatusGeoData = mAoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            if (licenseStatus == esriLicenseStatus.esriLicenseAvailable && licenseStatusGeoData == esriLicenseStatus.esriLicenseAvailable)
            {
                licenseStatus = mAoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngine);
                licenseStatusGeoData = mAoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
                mAoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeArcServer);
                if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut || licenseStatusGeoData != esriLicenseStatus.esriLicenseCheckedOut)
                {
                    //throw new ApplicationException("Unable to bind to ArcGIS license Server nor to Engine. Please check your licenses.");
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        public string GetLicense()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);

            mAoInitialize = new AoInitialize();

            string license = string.Empty;
            //Determine if the product is available
            esriLicenseStatus licenseStatus = mAoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngine);
            esriLicenseStatus licenseStatusGeoData = mAoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            license += $";engine available ({licenseStatus == esriLicenseStatus.esriLicenseAvailable})";
            license += $";engineGeoDb available ({licenseStatusGeoData == esriLicenseStatus.esriLicenseAvailable})";
            if (licenseStatus == esriLicenseStatus.esriLicenseAvailable && licenseStatusGeoData == esriLicenseStatus.esriLicenseAvailable)
            {
                licenseStatus = mAoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngine);
                licenseStatusGeoData = mAoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);

                license += $";engine checkedin ({licenseStatus == esriLicenseStatus.esriLicenseCheckedIn})";
                license += $";engineGeoDb checkedin ({licenseStatusGeoData == esriLicenseStatus.esriLicenseCheckedIn})";
            }
            return license;
        }
    }
}
