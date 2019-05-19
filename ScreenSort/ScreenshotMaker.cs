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

        public Bitmap takeScreenshot()
        {
            Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                //bmp.Save(filename);
                return bmp;
            }
        }

    }
}
