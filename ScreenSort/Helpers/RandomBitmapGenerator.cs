using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenSort.Helpers
{
    class RandomBitmapGenerator
    {
        private readonly int width;
        private readonly int height;

        public RandomBitmapGenerator(int width, int height)
        {
            this.width = width;
            this.height = height;
        }


        public WriteableBitmap GenerateRandomWritableBitmap(bool HSB)
        {
            Random r = new Random();
            WriteableBitmap bm = new WriteableBitmap(width, height, width, width, PixelFormats.Pbgra32, BitmapPalettes.WebPalette);

            var bArray = bm.ToByteArray();

            for (int i = 0; i < bArray.Length; i += 4)
            {
                if (HSB)
                {
                    bArray[i] = (byte)r.Next(255); //Blue
                    bArray[i + 1] = (byte)r.Next(255); //Green
                    bArray[i + 2] = (byte)r.Next(255); //Red
                    bArray[i + 3] = 255;
                }
                else
                {
                    bArray[i + 3] = 255;

                    switch (r.Next(3))
                    {
                        case 0:
                            bArray[i] = 255;
                            break;
                        case 1:
                            bArray[i + 1] = 255;
                            break;
                        case 2:
                            bArray[i + 2] = 255;
                            break;
                    }
                }


            }

            bm.WritePixels(new Int32Rect(0, 0, bm.PixelWidth, bm.PixelHeight), bArray, bm.PixelWidth * (bm.Format.BitsPerPixel) / 8, 0);


            return bm;
        }
    }
}
