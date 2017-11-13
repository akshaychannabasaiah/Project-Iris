using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.GPU;
using Iris_Recognition;
using System.Threading;

namespace IrisRecognitionWPFDemo
{
    /// <summary>
    /// Interaction logic for DoubleMatchWindow.xaml
    /// </summary>
    public partial class DoubleMatchWindow : MetroWindow
    {
        IrisRecognizer irisRecog = new IrisRecognizer();
        IrisImage iris1 = new IrisImage();
        IrisImage iris2 = new IrisImage();
        bool isImage1Loaded = false;
        bool isImage2Loaded = false;
        public DoubleMatchWindow()
        {
            InitializeComponent();
            imageBoxImage1.Image = new Image<Bgr, Byte>("..\\..\\take-action-click-icon.jpg");
            imageBoxImage2.Image = new Image<Bgr, Byte>("..\\..\\take-action-click-icon.jpg");
            imageBoxImage1.Cursor = System.Windows.Forms.Cursors.Hand;
            imageBoxImage2.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void buttonDoubleMatchHome_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonDoubleMatchChooseImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog window = new Microsoft.Win32.OpenFileDialog();
            window.Filter = "Bitmap Iris Images|*.bmp";
            window.Title = "Choose the first iris image.";
            if (window.ShowDialog() == true)
            {
                buttonConfirm.Visibility = System.Windows.Visibility.Visible;
                iris1 = new IrisImage(window.FileName);
                labelDoubleMatchInitialDisplayMessage.Visibility = System.Windows.Visibility.Collapsed;
                gridImages.Visibility = System.Windows.Visibility.Visible;
                imageBoxImage1.Image = iris1.InputImage;
                isImage1Loaded = true;
                Microsoft.Win32.OpenFileDialog window2 = new Microsoft.Win32.OpenFileDialog();
                window2.Filter = "Bitmap Iris Images|*.bmp";
                window2.Title = "Choose the second iris image.";
                if (window2.ShowDialog() == true)
                {
                    iris2 = new IrisImage(window2.FileName);
                    imageBoxImage2.Image = iris2.InputImage;
                    isImage2Loaded = true;
                    if (isImage1Loaded && isImage2Loaded)
                        buttonConfirm.IsEnabled = true;
                }
            }
        }

        private void imageBoxImage1_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog window = new Microsoft.Win32.OpenFileDialog();
            window.Filter = "Bitmap Iris Images|*.bmp";
            window.Title = "Choose the first iris image.";
            if (window.ShowDialog() == true)
            {
                iris1 = new IrisImage(window.FileName);
                imageBoxImage1.Image = iris1.InputImage;
                isImage1Loaded = true;
                if (isImage1Loaded && isImage2Loaded)
                    buttonConfirm.IsEnabled = true;
            }
        }

        private void imageBoxImage2_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog window2 = new Microsoft.Win32.OpenFileDialog();
            window2.Filter = "Bitmap Iris Images|*.bmp";
            window2.Title = "Choose the second iris image.";
            if (window2.ShowDialog() == true)
            {
                iris2 = new IrisImage(window2.FileName);
                imageBoxImage2.Image = iris2.InputImage;
                isImage2Loaded = true;
                if (isImage1Loaded && isImage2Loaded)
                    buttonConfirm.IsEnabled = true;
            }
        }
        private void ThreadProcess()
        {
            bool result;
            result = irisRecog.Match(iris1, iris2);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                gridDoubleMatch.Visibility = System.Windows.Visibility.Visible;
                gridPopUp.Visibility = System.Windows.Visibility.Collapsed;
                buttonDoubleMatchExtractFeatures.IsEnabled = true;
                buttonDoubleMatchInputImage.IsEnabled = true;
                buttonDoubleMatchIrisCode.IsEnabled = true;
                buttonDoubleMatchNormalized.IsEnabled = true;
                buttonDoubleMatchOuterBoundary.IsEnabled = true;
                buttonDoubleMatchPupilDetected.IsEnabled = true;
                buttonDoubleMatchSegmentedIris.IsEnabled = true;
                buttonDoubleMatchViewAll.IsEnabled = true;
                buttonDoubleMatchHammingResult.IsEnabled = true;
                if (result)
                {
                    MessageBox.Show("They match");
                }
                else
                {
                    MessageBox.Show("They do not match");
                }
            }));
        }

        private void MakeAllButtonNormalColors()
        {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonDoubleMatchChooseImage.Background = brush;
            buttonDoubleMatchExtractFeatures.Background = brush;
            buttonDoubleMatchInputImage.Background = brush;
            buttonDoubleMatchIrisCode.Background = brush;
            buttonDoubleMatchNormalized.Background = brush;
            buttonDoubleMatchOuterBoundary.Background = brush;
            buttonDoubleMatchPupilDetected.Background = brush;
            buttonDoubleMatchSegmentedIris.Background = brush;
            buttonDoubleMatchViewAll.Background = brush;
            buttonDoubleMatchHammingResult.Background = brush;
            gridGaborFiltersImage1.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFiltersImage2.Visibility = System.Windows.Visibility.Collapsed;
            gridImages.Visibility = System.Windows.Visibility.Visible;
        }

        private void MakeAllButtonNormalColorsAndDisableLastOnes()
        {
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchChooseImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            Brush brush = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonDoubleMatchExtractFeatures.Background = brush;
            buttonDoubleMatchInputImage.Background = brush;
            buttonDoubleMatchIrisCode.Background = brush;
            buttonDoubleMatchNormalized.Background = brush;
            buttonDoubleMatchOuterBoundary.Background = brush;
            buttonDoubleMatchPupilDetected.Background = brush;
            buttonDoubleMatchSegmentedIris.Background = brush;
            buttonDoubleMatchViewAll.Background = brush;
            buttonDoubleMatchHammingResult.Background = brush;
            buttonDoubleMatchExtractFeatures.IsEnabled = false;
            buttonDoubleMatchInputImage.IsEnabled = false;
            buttonDoubleMatchIrisCode.IsEnabled = false;
            buttonDoubleMatchNormalized.IsEnabled = false;
            buttonDoubleMatchOuterBoundary.IsEnabled = false;
            buttonDoubleMatchPupilDetected.IsEnabled = false;
            buttonDoubleMatchSegmentedIris.IsEnabled = false;
            buttonDoubleMatchViewAll.IsEnabled = false;
            buttonDoubleMatchHammingResult.IsEnabled = false;
            buttonConfirm.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFiltersImage1.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFiltersImage2.Visibility = System.Windows.Visibility.Collapsed;
            gridHamming.Visibility = System.Windows.Visibility.Collapsed;
            gridImages.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            gridDoubleMatch.Visibility = System.Windows.Visibility.Collapsed;
            gridPopUp.Visibility = System.Windows.Visibility.Visible;
            buttonConfirm.Visibility = System.Windows.Visibility.Collapsed;
            buttonDoubleMatchChooseImage.IsEnabled = false;
            Thread Iristhread = new Thread(() => ThreadProcess());
            Iristhread.Start();
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchChooseImage.Background = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonDoubleMatchInputImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            imageBoxImage1.Cursor = System.Windows.Forms.Cursors.Cross;
            imageBoxImage2.Cursor = System.Windows.Forms.Cursors.Cross;

        }

        private void buttonDoubleMatchRestart_Click(object sender, RoutedEventArgs e)
        {
            isImage1Loaded = false;
            isImage2Loaded = false;
            imageBoxImage1.Image = new Image<Bgr, Byte>("..\\..\\take-action-click-icon.jpg");
            imageBoxImage2.Image = new Image<Bgr, Byte>("..\\..\\take-action-click-icon.jpg");
            WindowsHostDoubleMatch.Cursor = Cursors.Hand;
            WindowsHostDoubleMatch2.Cursor = Cursors.Hand;
            buttonDoubleMatchChooseImage.IsEnabled = true;
            MakeAllButtonNormalColorsAndDisableLastOnes();
            iris1 = new IrisImage();
            iris2 = new IrisImage();
        }

        private void buttonDoubleMatchInputImage_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchInputImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            gridImages.Visibility = System.Windows.Visibility.Visible;
            imageBoxImage1.Image = iris1.InputImage;
            imageBoxImage2.Image = iris2.InputImage;
        }

        private void buttonDoubleMatchPupilDetected_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchPupilDetected.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            if (iris1.IsContourDetectionSatisfactory)
            {
                gridImages.Visibility = System.Windows.Visibility.Visible;
                imageBoxImage1.Image = iris1.ContourDetectedPupilImageColor;
            }
            else
            {
                MessageBox.Show("Pupil detection using contours failed for Iris Image 1, hence Hough Tranform was applied to detect approximate pupil");
                gridImages.Visibility = System.Windows.Visibility.Visible;
                imageBoxImage1.Image = iris1.ApproximatedPupilImage;
            }
            if (iris2.IsContourDetectionSatisfactory)
            {
                imageBoxImage2.Image = iris2.ContourDetectedPupilImageColor;
            }
            else
            {
                MessageBox.Show("Pupil detection using contours failed for Iris Image 2, hence Hough Tranform was applied to detect approximate pupil");
                imageBoxImage2.Image = iris2.ApproximatedPupilImage;
            }
        }

        private void buttonDoubleMatchOuterBoundary_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchOuterBoundary.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            gridImages.Visibility = System.Windows.Visibility.Visible;
            imageBoxImage1.Image = iris1.OptimisedIrisBoundaries;
            imageBoxImage2.Image = iris2.OptimisedIrisBoundaries;
        }

        private void buttonDoubleMatchSegmentedIris_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchSegmentedIris.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            gridImages.Visibility = System.Windows.Visibility.Visible;
            imageBoxImage1.Image = iris1.SegmentedIrisImage;
            imageBoxImage2.Image = iris2.SegmentedIrisImage;
        }

        private void buttonDoubleMatchNormalized_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchNormalized.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            gridImages.Visibility = System.Windows.Visibility.Visible;
            imageBoxImage1.Image = iris1.Iris.FullNormalImage;
            imageBoxImage2.Image = iris2.Iris.FullNormalImage;
        }

        private void buttonDoubleMatchExtractFeatures_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchExtractFeatures.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            gridImages.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFiltersImage1.Visibility = System.Windows.Visibility.Visible;
            gridGaborFiltersImage2.Visibility = System.Windows.Visibility.Visible;
            imageBoxImage1GB1.Image = iris1.Iris.ExtractedFeatures[0];
            imageBoxImage1GB2.Image = iris1.Iris.ExtractedFeatures[1];
            imageBoxImage1GB3.Image = iris1.Iris.ExtractedFeatures[2];
            imageBoxImage1GB4.Image = iris1.Iris.ExtractedFeatures[3];
            imageBoxImage1GB5.Image = iris1.Iris.ExtractedFeatures[4];
            imageBoxImage1GB6.Image = iris1.Iris.ExtractedFeatures[5];

            imageBoxImage2GB1.Image = iris2.Iris.ExtractedFeatures[0];
            imageBoxImage2GB2.Image = iris2.Iris.ExtractedFeatures[1];
            imageBoxImage2GB3.Image = iris2.Iris.ExtractedFeatures[2];
            imageBoxImage2GB4.Image = iris2.Iris.ExtractedFeatures[3];
            imageBoxImage2GB5.Image = iris2.Iris.ExtractedFeatures[4];
            imageBoxImage2GB6.Image = iris2.Iris.ExtractedFeatures[5];
        }

        private void buttonDoubleMatchHammingResult_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonDoubleMatchHammingResult.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            gridImages.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFiltersImage1.Visibility = System.Windows.Visibility.Visible;
            gridGaborFiltersImage2.Visibility = System.Windows.Visibility.Visible;
            gridHamming.Visibility = System.Windows.Visibility.Visible;
            imageBoxImage1GB1.Image = iris1.Iris.ExtractedFeatures[0];
            imageBoxImage1GB2.Image = iris1.Iris.ExtractedFeatures[1];
            imageBoxImage1GB3.Image = iris1.Iris.ExtractedFeatures[2];
            imageBoxImage1GB4.Image = iris1.Iris.ExtractedFeatures[3];
            imageBoxImage1GB5.Image = iris1.Iris.ExtractedFeatures[4];
            imageBoxImage1GB6.Image = iris1.Iris.ExtractedFeatures[5];

            imageBoxImage2GB1.Image = iris2.Iris.ExtractedFeatures[0];
            imageBoxImage2GB2.Image = iris2.Iris.ExtractedFeatures[1];
            imageBoxImage2GB3.Image = iris2.Iris.ExtractedFeatures[2];
            imageBoxImage2GB4.Image = iris2.Iris.ExtractedFeatures[3];
            imageBoxImage2GB5.Image = iris2.Iris.ExtractedFeatures[4];
            imageBoxImage2GB6.Image = iris2.Iris.ExtractedFeatures[5];

            double average = Math.Round(irisRecog.hammingDistance, 6);
            labelHammingAvg.Text = "The overall Hamming Code = " + average.ToString();
        }

        private void buttonDoubleMatchViewAll_Click(object sender, RoutedEventArgs e)
        {
            ViewAllDualWindow window = new ViewAllDualWindow(iris1, iris2);
            window.ShowDialog();
        }
    }
}
