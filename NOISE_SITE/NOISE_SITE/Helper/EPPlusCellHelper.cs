using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOISE_SITE.Helpers
{
    public class EPPlusCellHelper
    {
        public static string GetCellStringValue(object cell)
        {
            if (cell != null)
            {
                return cell.ToString();
            }
            return string.Empty;
        }

        public static int GetCellIntValue(object cell)
        {
            int cellValue = 0;
            if (cell != null)
            {
                if (int.TryParse(cell.ToString(), out cellValue))
                {
                    return cellValue;
                }
            }
            return cellValue;
        }

        public static double GetCellDoubleValue(object cell)
        {
            double cellValue = 0;
            if (cell != null)
            {
                if (double.TryParse(cell.ToString(), out cellValue))
                {
                    return cellValue;
                }
            }
            return cellValue;
        }
    }
}
