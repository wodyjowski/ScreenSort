using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ScreenSort.Algorithms;
using ScreenSort.Helpers;

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
        HSBColor[] hueArray;
        private CancellationTokenSource cTokenSource;
        private DispatcherTimer screenUpdateTimer;

        private bool wasCancelled = false;
        private Stopwatch stopwatch;

        private bool _isSorting = false;
        public bool IsSorting
        {
            get
            {
                return _isSorting;
            }
            set
            {
                _isSorting = value;
                screenshotButton.IsEnabled = !value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            comboBoxSortType.ItemsSource = Enum.GetValues(typeof(SortType)).Cast<SortType>();
            comboBoxSortType.SelectedIndex = 0;

            screenUpdateTimer = new DispatcherTimer();
            screenUpdateTimer.Interval = TimeSpan.FromMilliseconds(15);
            screenUpdateTimer.Tick += Dt_Tick;

            labelTime.Foreground = System.Windows.Media.Brushes.Orange;

            setResolutionText(textBoxResolution);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ScreenshotMaker screenshotMaker = new ScreenshotMaker();


            var tupleRes = GetResolution();

            var bitmap = screenshotMaker.takeScreenshot(tupleRes.Item1, tupleRes.Item2).ToBitmapSource();
            ImageBitmap = new WriteableBitmap(bitmap);

            SetImage();

        }


        private async void SortButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsSorting)
            {
                wasCancelled = true;
                IsSorting = false;
                cTokenSource.Cancel();
                return;
            }
            wasCancelled = false;

            if (ImageBitmap == null)
            {
                return;
            }

            tempArray = ImageBitmap.ToByteArray();
            intTempArray = new int[tempArray.Length / 4];
            hueArray = new HSBColor[intTempArray.Length];

            for (int i = 0; i < intTempArray.Length; i++)
            {
                intTempArray[i] = BitConverter.ToInt32(tempArray, i * 4);
                var color = Color.FromArgb(intTempArray[i]);
                hueArray[i] = new HSBColor();

                hueArray[i].Saturation = color.GetSaturation();
                hueArray[i].Hue = color.GetHue();
                hueArray[i].Brightness = color.GetBrightness();


                    
                    //=  (color.GetHue() * 10000) + (color.GetBrightness() * 100000) + color.GetSaturation();

            }

            //byte[] byt = new byte[4] { 0, 255, 234, 0 }; //bgra

            //var test = Color.FromArgb(BitConverter.ToInt32(byt, 0));
            //var hue = test.GetHue();

            //var x = Color.Red.GetHue();
            //var y = Color.Green.GetHue();
            //var z = Color.Blue.GetHue();

            screenUpdateTimer.Start();

            SortType srt = (SortType)comboBoxSortType.SelectedValue;


            cTokenSource = new CancellationTokenSource();

            bool hsb = checkBoxHSB.IsChecked ?? false;

            int delay = 0;
            int.TryParse(textBoxDelay.Text, out delay);

            Task sortTask = new Task(() =>
            {
                Sort(intTempArray, srt, cTokenSource.Token, hueArray, hsb, delay);
                ResetSort();
            }, cTokenSource.Token);

            IsSorting = true;
            SortButton.Content = "Stop";

            labelTime.Foreground = System.Windows.Media.Brushes.Orange;
            labelTime.Content = "Executing...";

            stopwatch = System.Diagnostics.Stopwatch.StartNew();
            sortTask.Start();
        }


        private void Sort(int[] intTempArray, SortType sortType, CancellationToken cToken, HSBColor[] floatSortArray, bool hsb, int delay)
        {
            ISortingAlgorithm sortingAlgorithm = null;

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
                case SortType.SelectionSort:
                    sortingAlgorithm = new SelectionSortAlgorithm();
                    break;
                case SortType.MergeSort:
                    sortingAlgorithm = new MergeSortAlgorithm();
                    break;
                    break;
                case SortType.ParalellMergeSort:
                    sortingAlgorithm = new ParalellMergeSortAlgorithm();
                    break;
            }

            sortingAlgorithm.Token = cToken;


            sortingAlgorithm.Delay = delay;

            if (hsb)
            {
                sortingAlgorithm.Sort(intTempArray, floatSortArray);
            }
            else
            {
                sortingAlgorithm.Sort(intTempArray);
            }

        }

        private void ResetSort()
        {
            //last bitmap update after sorting
            screenUpdateTimer.Dispatcher.Invoke(() =>
            {
                IsSorting = false;
                screenUpdateTimer.Stop();
                UpdateBitmap();
                SortButton.Content = "Sort";
                screenUpdateTimer.Stop();

                stopwatch.Stop();

                labelTime.Foreground = System.Windows.Media.Brushes.LightGreen;
                labelTime.Content = $"{stopwatch.Elapsed.TotalMilliseconds}ms";

            });
            //wasCancelled
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
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && !IsSorting)
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

        private void SetImage()
        {
            previewImage.Source = ImageBitmap;
            SortButton.IsEnabled = true;
        }

        private void TextBox_Validation(object sender, RoutedEventArgs e)
        {
            validateResolution((TextBox)sender);
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                validateResolution((TextBox)sender);
            }
        }

        private void validateResolution(TextBox sender)
        {
            Regex rg = new Regex(@"([0-9])+x([0-9])+");

            if (!rg.IsMatch(sender.Text))
            {
                setResolutionText(sender);
            }
        }

        private void setResolutionText(TextBox sender)
        {
            sender.Text = $"{SystemParameters.VirtualScreenWidth}x{SystemParameters.VirtualScreenHeight}";
        }

        private Tuple<int, int> GetResolution()
        {
            var res = textBoxResolution.Text.Split('x');

            return new Tuple<int, int>(int.Parse(res[0]),int.Parse(res[1]));    
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            var tupleRes = GetResolution();


            RandomBitmapGenerator rbg = new RandomBitmapGenerator(tupleRes.Item1, tupleRes.Item2);
            ImageBitmap = rbg.GenerateRandomWritableBitmap(checkBoxHSB.IsChecked ?? false);
            
            SetImage();

        }
    }
}
