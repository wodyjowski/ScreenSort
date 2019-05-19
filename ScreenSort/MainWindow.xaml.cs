using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ScreenSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WriteableBitmap ImageBitmap { get; set; }
        public byte[] tempArray;
        public int[] intTempArray;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ScreenshotMaker screenshotMaker = new ScreenshotMaker();


            var bitmap = screenshotMaker.takeScreenshot().ToBitmapSource();
            ImageBitmap = new WriteableBitmap(bitmap);


            previewImage.Source = ImageBitmap;

            SortButton.IsEnabled = true;

        }

        private async void SortButton_Click(object sender, RoutedEventArgs e)
        {

            if (ImageBitmap == null)
            {
                return;
            }

            tempArray = ImageBitmap.ToByteArray();
            intTempArray = new int[tempArray.Length / 4];

            for (int i = 0; i < intTempArray.Length; i++)
            {
                intTempArray[i] = BitConverter.ToInt32(tempArray, i * 4);
            }



            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(15);
            dt.Tick += Dt_Tick;
            dt.Start();

            SortType srt;

            if (QuickChck.IsChecked ?? false)
            {
                srt = SortType.QuickSort;
            }
            else if (HeapSort.IsChecked ?? false)
            {
                srt = SortType.HeapSort;
            }
            else
            {
                srt = SortType.SomeSort;
            }



            await Task.Run(() =>
            {
                Sort(intTempArray, srt);
            });

            UpdateBitmap();
            dt.Stop();

            //ImageBitmap.FromByteArray(tempArray);

        }

        private void UpdateBitmap()
        {
            ImageBitmap.WritePixels(new Int32Rect(0, 0, ImageBitmap.PixelWidth, ImageBitmap.PixelHeight), intTempArray, ImageBitmap.PixelWidth * (ImageBitmap.Format.BitsPerPixel) / 8, 0);
        }


        private void Dt_Tick(object sender, EventArgs e)
        {
            UpdateBitmap();
        }

        private void Sort(int[] intTempArray, SortType sortType)
        {
            switch (sortType)
            {
                case SortType.QuickSort:
                    QuickSort(intTempArray, 0, intTempArray.Length - 1);
                    break;
                case SortType.SomeSort:
                    SomeSort(intTempArray);
                    break;
                case SortType.HeapSort:
                    heapSort(intTempArray, intTempArray.Length);
                    break;
            }

        }

        public void SomeSort(int[] array)
        {
            int temp = 0;

            for (int write = 0; write < intTempArray.Length; write++)
            {
                for (int sort = 0; sort < intTempArray.Length - 1; sort++)
                {
                    if (intTempArray[sort] > intTempArray[sort + 1])
                    {
                        temp = intTempArray[sort + 1];
                        intTempArray[sort + 1] = intTempArray[sort];
                        intTempArray[sort] = temp;
                    }
                }
            }

        }


        public void QuickSort(int[] array, int left, int right)
        {
            var i = left;
            var j = right;
            var pivot = array[(left + right) / 2];
            while (i < j)
            {
                while (array[i] < pivot) i++;
                while (array[j] > pivot) j--;
                if (i <= j)
                {
                    // swap
                    var tmp = array[i];
                    array[i++] = array[j];  // ++ and -- inside array braces for shorter code
                    array[j--] = tmp;
                }
            }
            if (left < j) QuickSort(array, left, j);
            if (i < right) QuickSort(array, i, right);
        }


        static void heapSort(int[] arr, int n)
        {
            for (int i = n / 2 - 1; i >= 0; i--)
                heapify(arr, n, i);
            for (int i = n - 1; i >= 0; i--)
            {
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;
                heapify(arr, i, 0);
            }
        }
        static void heapify(int[] arr, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            if (left < n && arr[left] > arr[largest])
                largest = left;
            if (right < n && arr[right] > arr[largest])
                largest = right;
            if (largest != i)
            {
                int swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;
                heapify(arr, n, largest);
            }
        }




        private int x2, y2;

        private void PreviewImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {

                double imageScale = ImageBitmap.PixelWidth / previewImage.ActualWidth;

                x2 = (int)(e.GetPosition(previewImage).X * imageScale);
                y2 = (int)(e.GetPosition(previewImage).Y * imageScale);
            }
        }

        private void PreviewImage_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {

                double imageScale = ImageBitmap.PixelWidth / previewImage.ActualWidth;

                int size = 5;

                int x = (int)(e.GetPosition(previewImage).X * imageScale);
                int y = (int)(e.GetPosition(previewImage).Y * imageScale);

                //ImageBitmap.BlitRender(x2, y2, x, y, x + size, y + size, x2 + size, y2 + size,  System.Windows.Media.Colors.Aquamarine);

                ImageBitmap.Lock();

                var b = new Bitmap(ImageBitmap.PixelWidth,
                                   ImageBitmap.PixelHeight,
                                   ImageBitmap.BackBufferStride,
                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                                   ImageBitmap.BackBuffer);

                using (var bitmapGraphics = System.Drawing.Graphics.FromImage(b))
                {
                    bitmapGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    bitmapGraphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    bitmapGraphics.CompositingQuality = CompositingQuality.HighQuality;

                    var pen = new Pen(Brushes.Aqua, 5);

                    bitmapGraphics.DrawLine(pen, x2, y2, x, y);
                }

                ImageBitmap.AddDirtyRect(new Int32Rect(0, 0, ImageBitmap.PixelWidth, ImageBitmap.PixelHeight));
                ImageBitmap.Unlock();


                x2 = x;
                y2 = y;
            }
        }





    }
}
