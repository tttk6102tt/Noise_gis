using NOISE_SITE.Enums;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOISE_SITE.Helpers
{
    public static class OfficeHelper
    {
        public static void setStyle(ref ExcelRange cell, EnumFormat format)
        {
            cell.Style.Font.Bold = format.HasFlag(EnumFormat.BOLD);
            cell.Style.Font.Italic = format.HasFlag(EnumFormat.ITALIC);
            cell.Style.Font.UnderLine = format.HasFlag(EnumFormat.UNDERLINE);
            if (format.HasFlag(EnumFormat.UNDERLINE))
                cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
            if (format.HasFlag(EnumFormat.BORDER))
            {
                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                    border.Left.Style =
                    border.Right.Style = ExcelBorderStyle.Thin;
            }
            else
            {
                var border = cell.Style.Border;
                if (format.HasFlag(EnumFormat.BORDER_TOP))
                    border.Top.Style = ExcelBorderStyle.Thin;
                if (format.HasFlag(EnumFormat.BORDER_BOTTOM))
                    border.Bottom.Style = ExcelBorderStyle.Thin;
                if (format.HasFlag(EnumFormat.BORDER_LEFT))
                    border.Left.Style = ExcelBorderStyle.Thin;
                if (format.HasFlag(EnumFormat.BORDER_RIGHT))
                    border.Right.Style = ExcelBorderStyle.Thin;
            }
            //
            if (format.HasFlag(EnumFormat.CENTER))
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            else if (format.HasFlag(EnumFormat.LEFT))
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            else if (format.HasFlag(EnumFormat.RIGHT))
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //
            if (format.HasFlag(EnumFormat.TOP))
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            else if (format.HasFlag(EnumFormat.BOTTOM))
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
            else if (format.HasFlag(EnumFormat.MIDDLE))
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
    }
}
