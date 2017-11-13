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
    /// Interaction logic for EnrollWindow.xaml
    /// </summary>
    public partial class EnrollWindow : MetroWindow
    {
        IrisRecognizer irisRecog = new IrisRecognizer();
        IrisImage iris = new IrisImage();        
        public EnrollWindow()
        {
            InitializeComponent();
        }

        private void buttonEnrollHome_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonEnrollChooseImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog window = new Microsoft.Win32.OpenFileDialog();
            window.Title = "Choose an iris image";
            window.Filter = "Bitmap Iris Images|*.bmp";
            if (window.ShowDialog() == true)
            {
                iris.Load(window.FileName);
                labelEnrollInitialDisplayMessage.Visibility = System.Windows.Visibility.Collapsed;
                labelName.Visibility = System.Windows.Visibility.Visible;
                textBoxName.Visibility = System.Windows.Visibility.Visible;
                WindowsHost.Visibility = System.Windows.Visibility.Visible;
                buttonConfirm.Visibility = System.Windows.Visibility.Visible;
                buttonConfirm.IsEnabled = false;
                imageBox.Image = iris.InputImage;
            }            
        }

        private void ThreadProcess(string name)
        {
            irisRecog.enroll(iris, name);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                gridEnroll.Visibility = System.Windows.Visibility.Visible;
                gridPopUp.Visibility = System.Windows.Visibility.Collapsed;
                buttonEnrollExtractFeatures.IsEnabled = true;
                buttonEnrollInputImage.IsEnabled = true;
                buttonEnrollIrisCode.IsEnabled = true;
                buttonEnrollNormalized.IsEnabled = true;
                buttonEnrollOuterBoundary.IsEnabled = true;
                buttonEnrollPupilDetected.IsEnabled = true;
                buttonEnrollSegmentedIris.IsEnabled = true;
                buttonEnrollViewAll.IsEnabled = true;
            }));
        }
        private void MakeAllButtonNormalColors()
        {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonEnrollChooseImage.Background = brush;
            buttonEnrollExtractFeatures.Background = brush;
            buttonEnrollInputImage.Background = brush;
            buttonEnrollIrisCode.Background = brush;
            buttonEnrollNormalized.Background = brush;
            buttonEnrollOuterBoundary.Background = brush;
            buttonEnrollPupilDetected.Background = brush;
            buttonEnrollSegmentedIris.Background = brush;
            buttonEnrollViewAll.Background = brush;
            gridGaborFilters.Visibility = System.Windows.Visibility.Collapsed;
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
        }
        
        private void MakeAllButtonNormalColorsAndDisableLastOnes()
        {
            BrushConverter bc = new BrushConverter();
            buttonEnrollChooseImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            Brush brush = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonEnrollExtractFeatures.Background = brush;
            buttonEnrollInputImage.Background = brush;
            buttonEnrollIrisCode.Background = brush;
            buttonEnrollNormalized.Background = brush;
            buttonEnrollOuterBoundary.Background = brush;
            buttonEnrollPupilDetected.Background = brush;
            buttonEnrollSegmentedIris.Background = brush;
            buttonEnrollViewAll.Background = brush;
            buttonEnrollExtractFeatures.IsEnabled = false;
            buttonEnrollInputImage.IsEnabled = false;
            buttonEnrollIrisCode.IsEnabled = false;
            buttonEnrollNormalized.IsEnabled = false;
            buttonEnrollOuterBoundary.IsEnabled = false;
            buttonEnrollPupilDetected.IsEnabled = false;
            buttonEnrollSegmentedIris.IsEnabled = false;
            buttonEnrollViewAll.IsEnabled = false;
            buttonConfirm.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFilters.Visibility = System.Windows.Visibility.Collapsed;
            WindowsHost.Visibility = System.Windows.Visibility.Collapsed;
            buttonConfirm.IsEnabled = false;
        }
        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            gridEnroll.Visibility = System.Windows.Visibility.Collapsed;
            gridPopUp.Visibility = System.Windows.Visibility.Visible;
            buttonConfirm.Visibility = System.Windows.Visibility.Collapsed;            
            buttonEnrollChooseImage.IsEnabled = false;
            labelName.Visibility = System.Windows.Visibility.Collapsed;
            textBoxName.Visibility = System.Windows.Visibility.Collapsed;
            string name = textBoxName.Text;
            Thread Iristhread = new Thread(() => ThreadProcess(name));
            Iristhread.Start();
            BrushConverter bc = new BrushConverter();
            buttonEnrollChooseImage.Background = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonEnrollInputImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
        }

        private void buttonEnrollInputImage_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonEnrollInputImage.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
            imageBox.Image = iris.InputImage;
        }

        private void buttonEnrollPupilDetected_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonEnrollPupilDetected.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
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

        private void buttonEnrollOuterBoundary_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonEnrollOuterBoundary.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
            imageBox.Image = iris.OptimisedIrisBoundaries;
        }

        private void buttonEnrollSegmentedIris_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonEnrollSegmentedIris.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
            imageBox.Image = iris.SegmentedIrisImage;
        }

        private void buttonEnrollNormalized_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonEnrollNormalized.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Visible;
            imageBox.Image = iris.Iris.FullNormalImage;
        }

        private void buttonEnrollExtractFeatures_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonEnrollExtractFeatures.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            WindowsHost.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFilters.Visibility = System.Windows.Visibility.Visible;
            imageBoxGB1.Image = iris.Iris.ExtractedFeatures[0];
            imageBoxGB2.Image = iris.Iris.ExtractedFeatures[1];
            imageBoxGB3.Image = iris.Iris.ExtractedFeatures[2];
            imageBoxGB4.Image = iris.Iris.ExtractedFeatures[3];
            imageBoxGB5.Image = iris.Iris.ExtractedFeatures[4];
            imageBoxGB6.Image = iris.Iris.ExtractedFeatures[5];
        }

        private void buttonEnrollIrisCode_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(iris.Iris.IrisCodeOfFilter1);
        }

        private void buttonEnrollReset_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColorsAndDisableLastOnes();
            buttonEnrollChooseImage.IsEnabled = true;
            iris = new IrisImage();
        }

        private void buttonEnrollViewAll_Click(object sender, RoutedEventArgs e)
        {
            ViewAllWindow viewall = new ViewAllWindow(iris);
            viewall.ShowDialog();
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxName.Text.Length == 0)
            {
                buttonConfirm.IsEnabled = false;
            }
            else
            {
                buttonConfirm.IsEnabled = true;
            }
        }
    }
}
