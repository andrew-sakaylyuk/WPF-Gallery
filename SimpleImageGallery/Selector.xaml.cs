using System;
using System.Windows;

namespace SimpleImageGallery {

    public partial class Selector : Window {

        public static int InputNumber { get; private set; }
        public static string InputString { get; private set; }

        public Selector() {
            InitializeComponent();
            if (MainWindow.needAngle) {
                label1.Content = "Введіть кут:";
                inputData.Text = "90";
                label2.Content = "°";
            } else
            if (MainWindow.needSigma) {
                label1.Content = "Введіть σ:";
                inputData.Text = "3";
                label2.Content = "";
            } else
            if (MainWindow.needMatrixSize) {
                label1.Content = "Введіть розмірність матриці:";
                inputData.Text = "3";
                label2.Content = "D";
            } else
            if (MainWindow.needThreshold) {
                label1.Content = "Введіть поріг (0-200):";
                inputData.Text = "0";
                label2.Content = "";
            } else
            if (MainWindow.needDataForGaborFilter) {
                label1.Content = "Введіть дані:";
                inputData.Text = "0.5;  5.0;  2.0;  1.0;  3;  1.5";
                label2.Content = " Gamma\n Lambda\n Psi\n Sigma\n Size\n Theta\n";
            } else
            if (MainWindow.needBrushSize) {
                label1.Content = "Введіть розмір кисті (3-21):";
                inputData.Text = "5";
                label2.Content = "";
            } else
            if (MainWindow.needRadius) {
                label1.Content = "Введіть радіус (1-10):";
                inputData.Text = "1";
                label2.Content = "";
            }
        }

        private void Check(object sender, RoutedEventArgs e) {
            if (MainWindow.needDataForGaborFilter) {
                InputString = inputData.Text;
                MainWindow.needDataForGaborFilter = false;
                this.Close();
            } else {
                int inputVal = Int32.Parse(inputData.Text);
                if (MainWindow.needAngle && inputVal >= -360 && inputVal <= 360) {
                    InputNumber = inputVal;
                    MainWindow.needAngle = false;
                    this.Close();
                } else
                if ((MainWindow.needSigma || MainWindow.needMatrixSize) && inputVal >= 0) {
                    InputNumber = inputVal;
                    MainWindow.needSigma = false; MainWindow.needMatrixSize = false;
                    this.Close();
                } else
                if (MainWindow.needThreshold && inputVal >= 0 && inputVal <= 200) {
                    InputNumber = inputVal;
                    MainWindow.needThreshold = false;
                    this.Close();
                } else
                if (MainWindow.needBrushSize && inputVal >= 3 && inputVal <= 21) {
                    InputNumber = inputVal;
                    MainWindow.needBrushSize = false;
                    this.Close();
                } else
                if (MainWindow.needRadius && inputVal >= 1 && inputVal <= 10) {
                    InputNumber = inputVal;
                    MainWindow.needRadius = false;
                    this.Close();
                }
            }
        }

    }
    
}
