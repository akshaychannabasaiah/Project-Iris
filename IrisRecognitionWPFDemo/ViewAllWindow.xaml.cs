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
    /// Interaction logic for ViewAll.xaml
    /// </summary>
    public partial class ViewAllWindow : MetroWindow
    {
        IrisImage iris;
        List<string> descriptionlist = new List<string>(6);
        int currentDisplay = 0;
        public ViewAllWindow()
        {
            InitializeComponent();
        }

        public ViewAllWindow(IrisImage iris)
        {
            this.iris = iris;
            InitializeComponent();
            descriptionlist.Add("The first step in segmenting the iris is locating the pupil boundary. This can be done easily due to the uniform dark color of the human pupil. We start by removing noise from our input image by \"Smoothening\" it using Smooth Gaussian function. Then we mask the image to search for the darkest region, and finally we apply contour detection to locate the pupil boundary.");
            descriptionlist.Add("Once we detect the pupil boundary, we need to locate the outer boundary of the iris. For this we all Histogram Equilization to increase the contrast of the image, and then we apply hough circles to detect the outer boundary.");
            descriptionlist.Add("Using the iris boundarys detected in the previous steps, we now segment the iris from the input image. This is done by Subtracting the area of the iris from the input image using a mask that is created by drawing a black circle with the same center and radius of the iris region.");
            descriptionlist.Add("We now normalise the Segmented image using LogPolar function. This function converts the cartesian coordiantes to polar coordinates.");
            descriptionlist.Add("In order to reduce the size of the image so that we focus only on the part which is of use to us, we now proceed to cut the Image from the previous step so that only the Normalised iris is left.\nThe Left Most Point obtained is : ");
            descriptionlist.Add("We apply 3 gabor filters ( real and imaginary ) to the normalised image to get 6 filtered images. These images are used to obtian the iris code.");
            buttonPupilDetection_Click(new object(), new RoutedEventArgs());
        }

        private void buttonHome_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void MakeAllButtonNormalColors()
        {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#FFF7F7F7");
            buttonCutNormal.Background = brush;
            buttonFeatureExtraction.Background = brush;
            buttonIrisOuterBoudary.Background = brush;
            buttonNormalization.Background = brush;
            buttonPupilDetection.Background = brush;
            buttonSegmentation.Background = brush;
            gridGaborFiltersImage1.Visibility = System.Windows.Visibility.Collapsed;
            WindowsHost1.Visibility = System.Windows.Visibility.Visible;
            WindowsHost2.Visibility = System.Windows.Visibility.Visible;
            buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
            buttonRight.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonPupilDetection_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonPupilDetection.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            labelDescription.Text = descriptionlist[0];
            labelForImage1.Text = "Input Image";
            imageBoxImage1.Image = iris.InputImage;
            labelForImage2.Text = "Smoothened Image";
            imageBoxImage2.Image = iris.SmoothImage;
            currentDisplay = 0;
            buttonRight.Visibility = System.Windows.Visibility.Visible;
            buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void buttonIrisOuterBoudary_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonIrisOuterBoudary.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            labelDescription.Text = descriptionlist[1];
            labelForImage1.Text = "Contrast Image";
            imageBoxImage1.Image = iris.IncreaseContrastImage;
            labelForImage2.Text = "Optimized Iris Boundary";
            imageBoxImage2.Image = iris.OptimisedIrisBoundaries;
            currentDisplay = 1;
            buttonRight.Visibility = System.Windows.Visibility.Collapsed;
            buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonSegmentation_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonSegmentation.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            labelDescription.Text = descriptionlist[2];
            labelForImage1.Text = "Filled Contour Image";
            imageBoxImage1.Image = iris.FilledContourForSegmentation;
            labelForImage2.Text = "Optimized Iris Boundary";
            imageBoxImage2.Image = iris.OptimisedIrisBoundaries;
            currentDisplay = 2;
            buttonRight.Visibility = System.Windows.Visibility.Visible;
            buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonNormalization_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonNormalization.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            labelDescription.Text = descriptionlist[3];
            labelForImage1.Text = "Segmenated Iris";
            imageBoxImage1.Image = iris.SegmentedIrisImage;
            labelForImage2.Text = "Normalized Iris";
            imageBoxImage2.Image = iris.Iris.FullNormalImage;
            currentDisplay = 3;
            buttonRight.Visibility = System.Windows.Visibility.Collapsed;
            buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonCutNormal_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonCutNormal.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            labelDescription.Text = descriptionlist[4] + iris.Iris.LeftMostPoint;
            labelForImage1.Text = "Normalization Mask";
            imageBoxImage1.Image = iris.Iris.normalizationMask;
            labelForImage2.Text = "Normalized Iris";
            imageBoxImage2.Image = iris.Iris.NormalisedIris;
            currentDisplay = 4;
            buttonRight.Visibility = System.Windows.Visibility.Collapsed;
            buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonFeatureExtraction_Click(object sender, RoutedEventArgs e)
        {
            MakeAllButtonNormalColors();
            BrushConverter bc = new BrushConverter();
            buttonFeatureExtraction.Background = (Brush)bc.ConvertFrom("#FF8BFF69");
            labelDescription.Text = descriptionlist[5];
            labelForImage1.Text = "Normalized Iris";
            imageBoxImage1.Image = iris.Iris.NormalisedIris;
            labelForImage1.Text = "Extracted Features";
            WindowsHost2.Visibility = System.Windows.Visibility.Collapsed;
            gridGaborFiltersImage1.Visibility = System.Windows.Visibility.Visible;
            imageBoxImage1GB1.Image = iris.Iris.ExtractedFeatures[0];
            imageBoxImage1GB2.Image = iris.Iris.ExtractedFeatures[1];
            imageBoxImage1GB3.Image = iris.Iris.ExtractedFeatures[2];
            imageBoxImage1GB4.Image = iris.Iris.ExtractedFeatures[3];
            imageBoxImage1GB5.Image = iris.Iris.ExtractedFeatures[4];
            imageBoxImage1GB6.Image = iris.Iris.ExtractedFeatures[5];

            currentDisplay = 5;
            buttonRight.Visibility = System.Windows.Visibility.Collapsed;
            buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonRight_Click(object sender, RoutedEventArgs e)
        {
            switch (currentDisplay)
            {
                case 0:
                    labelForImage1.Text = "Masked Image";
                    imageBoxImage1.Image = iris.MaskedImage;
                    labelForImage2.Text = "Countour Detected Pupil Image";
                    imageBoxImage2.Image = iris.ContourDetectedPupilImageColor;
                    buttonRight.Visibility = System.Windows.Visibility.Collapsed;
                    buttonLeft.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 2:
                    labelForImage1.Text = "Segmentation Mask";
                    imageBoxImage1.Image = iris.mask;
                    labelForImage2.Text = "Segmanted Iris";
                    imageBoxImage2.Image = iris.SegmentedIrisImage;
                    buttonRight.Visibility = System.Windows.Visibility.Collapsed;
                    buttonLeft.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            switch (currentDisplay)
            {
                case 0:
                    labelForImage1.Text = "Input Image";
                    imageBoxImage1.Image = iris.InputImage;
                    labelForImage2.Text = "Smoothened Image";
                    imageBoxImage2.Image = iris.SmoothImage;
                    buttonRight.Visibility = System.Windows.Visibility.Visible;
                    buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case 2:
                    labelForImage1.Text = "Filled Contour Image";
                    imageBoxImage1.Image = iris.FilledContourForSegmentation;
                    labelForImage2.Text = "Optimized Iris Boundary";
                    imageBoxImage2.Image = iris.OptimisedIrisBoundaries;
                    buttonRight.Visibility = System.Windows.Visibility.Visible;
                    buttonLeft.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }
    }
}
