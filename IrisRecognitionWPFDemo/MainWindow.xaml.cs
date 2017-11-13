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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.GPU;
using MahApps.Metro.Controls;
using Iris_Recognition;

namespace IrisRecognitionWPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainIrisWindow : MetroWindow
    {
        public MainIrisWindow()
        {
            InitializeComponent();
            ToggleMatch();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void buttonMainRegister_Click(object sender, RoutedEventArgs e)
        {
            EnrollWindow enroll = new EnrollWindow();
            enroll.ShowDialog();
        }

        private void buttonMainMatchDouble_Click(object sender, RoutedEventArgs e)
        {
            DoubleMatchWindow doubleMatch = new DoubleMatchWindow();
            doubleMatch.ShowDialog();
        }

        private void ToggleMatch()
        {
            if(buttonMainMatchSingle.IsVisible)
            {
                buttonMainMatchSingle.Visibility = System.Windows.Visibility.Collapsed;
                buttonMainMatchDouble.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                buttonMainMatchSingle.Visibility = System.Windows.Visibility.Visible;
                buttonMainMatchDouble.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void buttonMainMatchDoNothing_Click(object sender, RoutedEventArgs e)
        {
            ToggleMatch();
        }

        private void buttonMainMatchSingle_Click(object sender, RoutedEventArgs e)
        {
            SingleMatchWindow window = new SingleMatchWindow();
            window.ShowDialog();
        }
    }
}
