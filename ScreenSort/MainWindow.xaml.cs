using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WBAnimation.Algorithms;

namespace ScreenSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WriteableBitmap ImageBitmap { get; set; }
        private byte[] tempArray;
        private int[] intTempArray;
        private CancellationTokenSource cTokenSource;

        public MainWindow()
        {
            InitializeComponent();
            comboBoxSortType.ItemsSource = Enum.GetValues(typeof(SortType)).Cast<SortType>();
            comboBoxSortType.SelectedIndex = 0;

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
            if(cTokenSource != null)
            {
                ResetSort();
                cTokenSource.Cancel();
            }


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

            SortType srt = (SortType)comboBoxSortType.SelectedValue;


            cTokenSource = new CancellationTokenSource();


            Task sortTask = new Task(() =>
            {
                Sort(intTempArray, srt, dt, cTokenSource.Token);
            });

            SortButton.Content = "Stop";
            sortTask.Start();
        }


        private void Sort(int[] intTempArray, SortType sortType, DispatcherTimer dt, CancellationToken cToken)
        {
            ISortingAlgorithm sortingAlgorithm = null;

            cToken.Register(() =>
            {
                ResetSort();
            });

            switch (sortType)
            {
                case SortType.QuickSort:
                    sortingAlgorithm = new QuickSortAlgorithm();
                    break;
                case SortType.BubbleSort:
                    sortingAlgorithm = new BubbleSortAlgorithm();
                    break;
                case SortType.HeapSort:
                    sortingAlgorithm = new HeapSortAlgorithm();
                    break;
            }

            sortingAlgorithm.Token = cToken;
            sortingAlgorithm.Sort(intTempArray);

            //last bitmap update after sorting
            dt.Dispatcher.Invoke(() =>
            {
                ResetSort();
            });
            dt.Stop();
        }

        private void ResetSort()
        {
            UpdateBitmap();
            SortButton.Content = "Sort";
        }


        private void UpdateBitmap()
        {
            ImageBitmap.WritePixels(new Int32Rect(0, 0, ImageBitmap.PixelWidth, ImageBitmap.PixelHeight), intTempArray, ImageBitmap.PixelWidth * (ImageBitmap.Format.BitsPerPixel) / 8, 0);
        }


        private void Dt_Tick(object sender, EventArgs e)
        {
            UpdateBitmap();
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
