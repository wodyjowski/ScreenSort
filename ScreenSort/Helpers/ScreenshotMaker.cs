using System;
using System.Drawing;
using System.Windows;

namespace ScreenSort
{
    public class ScreenshotMaker
    {
        private double screenLeft;
        private double screenTop;
        private double screenWidth;
        private double screenHeight;

        public ScreenshotMaker()
        {
            screenLeft = SystemParameters.VirtualScreenLeft;
            screenTop = SystemParameters.VirtualScreenTop;
            screenWidth = SystemParameters.VirtualScreenWidth;
            screenHeight = SystemParameters.VirtualScreenHeight;
        }

        public Bitmap takeScreenshot(int x, int y)
        {
            Bitmap bmpRes = new Bitmap(x, y, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    using (Graphics g1 = Graphics.FromImage(bmpRes))
                    {
                        g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);

                        g1.DrawImage(bmp, new RectangleF(0,0, bmpRes.Width, bmpRes.Height), new RectangleF(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);

                        return bmpRes;
                    }
                }

            }
        }

    }
}
