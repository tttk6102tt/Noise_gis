using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NOISE_SITE.Helper
{
    public static class UtilitiesHelper
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff");
        }

        public static DateTime GetTimeFromTimeStampString(string timeStamp)
        {
            return DateTime.ParseExact(timeStamp, "yyyyMMddHHmmssfff", null);
        }

        public static string GetMd5Sum(string str)
        {
            System.Text.Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static byte[] getCaptcha(string sCaptchaText, int iHeight, int iWidth)
        {
            Random oRandom = new Random();

            int[] aBackgroundNoiseColor = new int[] { 150, 150, 150 };
            int[] aTextColor = new int[] { 0, 0, 0 };
            int[] aFontEmSizes = new int[] { 15, 20, 25, 30, 35 };

            string[] aFontNames = new string[]
            {
 "Comic Sans MS",
 "Arial",
 "Times New Roman",
 "Georgia",
 "Verdana",
 "Geneva"
            };
            FontStyle[] aFontStyles = new FontStyle[]
            {
 FontStyle.Bold,
 FontStyle.Italic,
 FontStyle.Regular,
 FontStyle.Strikeout,
 FontStyle.Underline
            };
            HatchStyle[] aHatchStyles = new HatchStyle[]
            {
 HatchStyle.BackwardDiagonal, HatchStyle.Cross,
    HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
 HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
    HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
 HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid,
    HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
 HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard,
    HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
 HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal,
    HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
 HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal,
    HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
 HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard,
    HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
 HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis,
    HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
 HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
            };

            //Creates an output Bitmap
            Bitmap oOutputBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
            Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Create a Drawing area
            RectangleF oRectangleF = new RectangleF(0, 0, iWidth, iHeight);
            Brush oBrush = default(Brush);

            //Draw background (Lighter colors RGB 100 to 255)
            oBrush = new HatchBrush(aHatchStyles[oRandom.Next
                (aHatchStyles.Length - 1)], Color.FromArgb((oRandom.Next(100, 255)),
                (oRandom.Next(100, 255)), (oRandom.Next(100, 255))), Color.White);
            oGraphics.FillRectangle(oBrush, oRectangleF);

            System.Drawing.Drawing2D.Matrix oMatrix = new System.Drawing.Drawing2D.Matrix();
            int i = 0;
            for (i = 0; i <= sCaptchaText.Length - 1; i++)
            {
                oMatrix.Reset();
                int iChars = sCaptchaText.Length;
                int x = iWidth / (iChars + 1) * i;
                int y = iHeight / 2;

                //Rotate text Random
                oMatrix.RotateAt(oRandom.Next(-40, 40), new PointF(x, y));
                oGraphics.Transform = oMatrix;

                //Draw the letters with Random Font Type, Size and Color
                oGraphics.DrawString
                (
                //Text
                sCaptchaText.Substring(i, 1),
                //Random Font Name and Style
                new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
                   aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                   aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
                //Random Color (Darker colors RGB 0 to 100)
                new SolidBrush(Color.FromArgb(oRandom.Next(0, 100),
                   oRandom.Next(0, 100), oRandom.Next(0, 100))),
                x,
                oRandom.Next(10, 40)
                );
                oGraphics.ResetTransform();
            }

            MemoryStream oMemoryStream = new MemoryStream();
            oOutputBitmap.Save(oMemoryStream, ImageFormat.Png);
            byte[] oBytes = oMemoryStream.GetBuffer();

            oOutputBitmap.Dispose();
            oMemoryStream.Close();

            return oBytes;
        }

        public static byte[] CaptchaImage(string prefix, int iHeight, int iWidth, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            ////generate new question 
            //int a = rand.Next(10, 99);
            //int b = rand.Next(0, 9);
            //var captcha = string.Format("{0} + {1} = ?", a, b);

            ////store answer 
            //Session["Captcha" + prefix] = a + b;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(iWidth, iHeight))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise 
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (iWidth / 3));
                        x = rand.Next(0, iWidth);
                        y = rand.Next(0, iHeight);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question 
                gfx.DrawString(prefix, new Font("monospace", iHeight / 2, FontStyle.Bold | FontStyle.Underline), Brushes.Gray, 3, 3);

                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);

                return mem.GetBuffer();
            }
        }

        public static string GetRandomName()
        {
            return Path.GetFileName(string.Format("{0}{1}", DateTime.Now.Ticks.GetHashCode().ToString("x"), ".xlsx"));
        }


    }
}