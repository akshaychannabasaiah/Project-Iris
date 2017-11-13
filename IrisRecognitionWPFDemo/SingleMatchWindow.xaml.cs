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
    /// Interaction logic for SingleMatchWindow.xaml
    /// </summary>
    public partial class SingleMatchWindow : MetroWindow
    {
        IrisRecognizer irisRecog = new IrisRecognizer();
        IrisImage iris = new IrisImage();        
        public SingleMatchWindow()
        {
            InitializeComponent();
        }

        private void buttonSingleMatchHome_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonSingleMatchChooseImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog window = new Microsoft.Win32.OpenFileDialog();
            window.Title = "Choose an iris image";
            window.Filter = "Bitmap Iris Images|*.bmp";
            if (window.ShowDialog() == true)
            {
                iris.Load(window.FileName);
                labelSingleMatchInitialDisplayMessage.Visibility = System.Windows.Visibility.Collapsed;                
                WindowsHost.Visibility = System.Windows.Visibility.Visible;
                buttonConfirm.Visibility = System.Windows.Visibility.Visible;
                imageBox.Image = iris.InputImage;
            }            
        }

        private void ThreadProcess()
        {           
            IrisDBEntry DBEntry = irisRecog.Match(iris);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                gridSingleMatch.Visibility = System.Windows.Visibility.Visible;
                gridPopUp.Visibility = System.Windows.Visibility.Collapsed;
                buttonSingleMatchExtractFeatures.IsEnabled = true;
                buttonSingleMatchInputImage.IsEnabled = true;
                buttonSingleMatchIrisCode.IsEnabled = true;
                buttonSingleMatchNormalized.IsEnabled = true;
                buttonSingleMatchOuterBoundary.IsEnabled = true;
                buttonSingleMatchPupilDetected.IsEnabled = true;
                buttonSingleMatchSegmentedIris.IsEnabled = true;
                buttonSingleMatchViewAll.IsEnabled = true;
                buttonSingleMatchHammingResult.IsEnabled = true;
                labelSingleMatchDisplayMessage.Visibility = System.Windows.Visibility.Visible;
                if (DBEntry != null)
                {
                    labelSingleMatchDisplayMessage.Text = "Iris Matched with : " + DBEntry.name;
                }
                else
                {
                    labelSingleMatchDisplayMessage.Text = "Iris Not Matched with any Iris in the database ";
                }
            }));
        }
        private void MakeAllButtonNormalColors()
        {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonSingleMatchChooseImage.Background = brush;
            buttonSingleMatchExtractFeatures.Background = brush;
            buttonSingleMatchInputImage.Background = brush;
            buttonSingleMatchIrisCode.Background = brush;
            buttonSingleMatchNormalized.Background = brush;
            buttonSingleMatchOuterBoundary.Background = brush;
            buttonSingleMatchPupilDetected.Background = brush;
            buttonSingleMatchSegmentedIris.Background = brush;
            buttonSingleMatchViewAll.Background = brush;
            buttonSingleMatchHammingResult.Background = brush;
            gridGaborFilters.Visibility = System.Windows.Visibility.Collapsed;
            gridHamming.Visibility = System.Windows.Visibility.Collapsed;
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
        }
        
        private void MakeAllButtonNormalColorsAndDisableLastOnes()
        {
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchChooseImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            Brush brush = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonSingleMatchExtractFeatures.Background = brush;
            buttonSingleMatchInputImage.Background = brush;
            buttonSingleMatchIrisCode.Background = brush;
            buttonSingleMatchNormalized.Background = brush;
            buttonSingleMatchOuterBoundary.Background = brush;
            buttonSingleMatchPupilDetected.Background = brush;
            buttonSingleMatchSegmentedIris.Background = brush;
            buttonSingleMatchViewAll.Background = brush;
            buttonSingleMatchHammingResult.Background = brush;
            buttonSingleMatchExtractFeatures.IsEnabled = false;
            buttonSingleMatchInputImage.IsEnabled = false;
            buttonSingleMatchIrisCode.IsEnabled = false;
            buttonSingleMatchNormalized.IsEnabled = false;
            buttonSingleMatchOuterBoundary.IsEnabled = false;
            buttonSingleMatchPupilDetected.IsEnabled = false;
            buttonSingleMatchSegmentedIris.IsEnabled = false;
            buttonSingleMatchViewAll.IsEnabled = false;
            buttonSingleMatchHammingResult.IsEnabled = false;
            buttonConfirm.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFilters.Visibility = System.Windows.Visibility.Collapsed;
            WindowsHost.Visibility = System.Windows.Visibility.Collapsed;
            labelSingleMatchDisplayMessage.Visibility = System.Windows.Visibility.Collapsed;
            gridHamming.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            gridSingleMatch.Visibility = System.Windows.Visibility.Collapsed;
            gridPopUp.Visibility = System.Windows.Visibility.Visible;
            buttonConfirm.Visibility = System.Windows.Visibility.Collapsed;            
            buttonSingleMatchChooseImage.IsEnabled = false;
            Thread Iristhread = new Thread(() => ThreadProcess());
            Iristhread.Start();
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchChooseImage.Background = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonSingleMatchInputImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
        }

        private void buttonSingleMatchInputImage_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchInputImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
            imageBox.Image = iris.InputImage;
        }

        private void buttonSingleMatchPupilDetected_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchPupilDetected.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            if (iris.IsContourDetectionSatisfactory)
            {
                WindowsHost.Visibility = System.Windows.Visibility.Visible;
                imageBox.Image = iris.ContourDetectedPupilImageColor;
            }
            else
            {
                MessageBox.Show("Pupil detection using contours failed, hence Hough Tranform was applied to detect approximate pupil");
                WindowsHost.Visibility = System.Windows.Visibility.Visible;
                imageBox.Image = iris.ApproximatedPupilImage;
            }
            
        }

        private void buttonSingleMatchOuterBoundary_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchOuterBoundary.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
            imageBox.Image = iris.OptimisedIrisBoundaries;
        }

        private void buttonSingleMatchSegmentedIris_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchSegmentedIris.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
            imageBox.Image = iris.SegmentedIrisImage;
        }

        private void buttonSingleMatchNormalized_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchNormalized.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
            imageBox.Image = iris.Iris.FullNormalImage;
        }

        private void buttonSingleMatchExtractFeatures_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchExtractFeatures.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFilters.Visibility = System.Windows.Visibility.Visible;
            imageBoxGB1.Image = iris.Iris.ExtractedFeatures[0];
            imageBoxGB2.Image = iris.Iris.ExtractedFeatures[1];
            imageBoxGB3.Image = iris.Iris.ExtractedFeatures[2];
            imageBoxGB4.Image = iris.Iris.ExtractedFeatures[3];
            imageBoxGB5.Image = iris.Iris.ExtractedFeatures[4];
            imageBoxGB6.Image = iris.Iris.ExtractedFeatures[5];
        }

        private void buttonSingleMatchIrisCode_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(iris.Iris.IrisCodeOfFilter1);
        }

        private void buttonSingleMatchRestart_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColorsAndDisableLastOnes();
            buttonSingleMatchChooseImage.IsEnabled = true;
            labelSingleMatchInitialDisplayMessage.Visibility = System.Windows.Visibility.Visible;
            iris = new IrisImage();
        }

        private void buttonSingleMatchViewAll_Click(object sender, RoutedEventArgs e)
        {
            ViewAllWindow viewall = new ViewAllWindow(iris);
            viewall.ShowDialog();
        }

        private void buttonSingleMatchHome_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonSingleMatchHammingResult_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonSingleMatchHammingResult.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFilters.Visibility = System.Windows.Visibility.Visible;
            imageBoxGB1.Image = iris.Iris.ExtractedFeatures[0];
            imageBoxGB2.Image = iris.Iris.ExtractedFeatures[1];
            imageBoxGB3.Image = iris.Iris.ExtractedFeatures[2];
            imageBoxGB4.Image = iris.Iris.ExtractedFeatures[3];
            imageBoxGB5.Image = iris.Iris.ExtractedFeatures[4];
            imageBoxGB6.Image = iris.Iris.ExtractedFeatures[5];
            double average = Math.Round(irisRecog.hammingDistance, 6);
            gridHamming.Visibility = System.Windows.Visibility.Visible;
            labelHammingAvg.Text = "The overall Hamming Code = " + average.ToString();
        }

    }
}
