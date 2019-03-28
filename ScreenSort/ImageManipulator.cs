using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenSort
{
    class ImageManipulator
    {
        enum RasterOperation : uint { SRC_COPY = 0x00CC0020 }

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
        [DllImport("Gdi32.dll")]
        static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, RasterOperation rasterOperation);

        private readonly Rectangle bounds;
        private readonly IntPtr desktopPtr;


        public ImageManipulator()
        {
            desktopPtr = GetDC(IntPtr.Zero);
            bounds = Screen.PrimaryScreen.Bounds;
        }

        public void CreateScreenshot()
        {
            Bitmap bitmap = getScreenBitmap();

            bitmap.Save("image.png", ImageFormat.Png);
            bitmap.Dispose();
        }

        public void ShowPreview(PictureBox pb)
        {
            pb.Image?.Dispose();
            pb.Image = getScreenBitmap();
        }

        private Bitmap getScreenBitmap()
        {
            var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format24bppRgb);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                IntPtr dstHdc = graphics.GetHdc();
                BitBlt(dstHdc, 0, 0, bounds.Width, bounds.Height, desktopPtr, 0, 0,
                RasterOperation.SRC_COPY);
                graphics.ReleaseHdc(dstHdc);
            }

            return bitmap;
        }
    }
}
