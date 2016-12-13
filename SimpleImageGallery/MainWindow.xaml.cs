using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SimpleImageGallery {

    public partial class MainWindow : Window  {
        public PhotoCollection Photos;
        public Photo _photo;
        public static bool needAngle = false;
        public static bool needSigma = false;
        public static bool needMatrixSize = false;
        public static bool needThreshold = false;
        public static bool needDataForGaborFilter = false;
        public static bool needBrushSize = false;
        public static bool needRadius = false;

        public MainWindow()  {
            InitializeComponent();
        }

        private void Refresh(){
            if (PhotosListBox.SelectedItem != null) {
                _photo = (Photo)PhotosListBox.SelectedItem;
                ViewedPhoto.Source = _photo.Image;
                SIG.Title = "S-Gallery: " + _photo.Source;
            }
        }

        private void OnPhotoMove(object sender, System.Windows.Input.MouseEventArgs e) {
            Refresh();
        }

        private void Update(string FilePath)  {
            Photos.Path = FilePath.Remove(FilePath.LastIndexOf('\\') + 1);
            for (int i = 0; i < Photos.Count; ++i) {
                if (Photos[i].Source.Equals(FilePath)) {
                    PhotosListBox.SelectedItem = Photos[i]; break;
                }
            }
            Refresh();
        }

        private void Clear(object sender, RoutedEventArgs e)  {
            Update(_photo.Source);
        }

        private void OpenFile(object sender, RoutedEventArgs e)  {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images|*.jpg; *.jpeg; *.png; *.bmp|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                Update(openFileDialog.FileName);
        }

        private void SaveToJpg(object sender, RoutedEventArgs e) {
            SaveUsingEncoder(new JpegBitmapEncoder(), ".jpg");
        }

        void SaveToPng(object sender, RoutedEventArgs e) {
            SaveUsingEncoder(new PngBitmapEncoder(), ".png");
        }

        void SaveToBmp(object sender, RoutedEventArgs e)  {
            SaveUsingEncoder(new BmpBitmapEncoder(), ".bmp");
        }

        void SaveUsingEncoder(BitmapEncoder encoder, string format) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            switch (format) {
                case ".jpg":
                    saveFileDialog.Filter = "JPG|*.jpg";
                    break;
                case ".png":
                    saveFileDialog.Filter = "PNG|*.png";
                    break;
                case ".bmp":
                    saveFileDialog.Filter = "BMP|*.bmp";
                    break;
            }
            if (saveFileDialog.ShowDialog() == true)  {
                encoder.Frames.Add(_photo.Image);
                using (var stream = File.Create(saveFileDialog.FileName)) {
                    encoder.Save(stream);
                }
            }
        }

        private void Grayscale(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Grayscale(_photo);
                /* Another way to make picture black and white
                  _photo.Image = BitmapFrame.Create(new FormatConvertedBitmap(
                    _photo.Image, PixelFormats.Gray8, BitmapPalettes.Gray256, 1.0));
                */
                Refresh();
            }
        }

        private void Negative(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Negative(_photo);
                Refresh();
            }
        }

        private void Transparency(object sender, RoutedEventArgs e)  {
            if (ViewedPhoto.Source != null) {
                Effects.Transparency(_photo);
                Refresh();
            }
        }

        private void SepiaTone(object sender, RoutedEventArgs e)  {
            if (ViewedPhoto.Source != null) {
                Effects.SepiaTone(_photo);
                Refresh();
            }
        }

        private void SelectMatrixSize(object sender, RoutedEventArgs e) {
            needMatrixSize = true;
            Selector matrixSizeSelector = new Selector();
            matrixSizeSelector.Show();
            matrixSizeSelector.Closed += new EventHandler(MedianFilter);
        }
        private void MedianFilter(object sender, EventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.MedianFilter(_photo, Selector.InputNumber);
                Refresh();
            }
        }

        private void SelectSigma(object sender, RoutedEventArgs e) {
            needSigma = true;
            Selector sigmaSelector = new Selector();
            sigmaSelector.Show();
            sigmaSelector.Closed += new EventHandler(GaussianBlur);
        }
        private void GaussianBlur(object sender, EventArgs e) { 
            if (ViewedPhoto.Source != null) {
                Effects.GaussianBlur(_photo, Selector.InputNumber);
                Refresh();
            }    
        }

        private void SelectThreshold(object sender, RoutedEventArgs e) {
            needThreshold  = true;
            Selector thresholdSelector = new Selector();
            thresholdSelector.Show();
            thresholdSelector.Closed += new EventHandler(GradientEdgeDetect);
        }
        private void GradientEdgeDetect(object sender, EventArgs e)  {
            if (ViewedPhoto.Source != null) {
                Effects.GradientEdgeDetect(_photo, Selector.InputNumber);
                Refresh();
            }
        }

        private void PrewittFilter(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.PrewittFilter(_photo);
                Refresh();
            }
        }

        private void PrewittFilterGrayscale(object sender, RoutedEventArgs e)  {
            if (ViewedPhoto.Source != null) {
                Effects.PrewittFilter(_photo, true);
                Refresh();
            }
        }

        private void SobelFilter(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.SobelFilter(_photo);
                Refresh();
            }
        }

        private void SobelFilterGrayscale(object sender, RoutedEventArgs e)  {
            if (ViewedPhoto.Source != null) {
                Effects.SobelFilter(_photo, true);
                Refresh();
            }
        }

        private void KirschFilter(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.KirschFilter(_photo);
                Refresh();
            }
        }

        private void KirschFilterGrayscale(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null)  {
                Effects.KirschFilter(_photo, true);
                Refresh();
            }
        }

        private void Laplacian3x3Filter(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Laplacian3x3Filter(_photo);
                Refresh();
            }
        }

        private void Laplacian3x3FilterGrayscale(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Laplacian3x3Filter(_photo, true);
                Refresh();
            }
        }

        private void Laplacian5x5Filter(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Laplacian5x5Filter(_photo);
                Refresh();
            }
        }

        private void Laplacian5x5FilterGrayscale(object sender, RoutedEventArgs e)  {
            if (ViewedPhoto.Source != null) {
                Effects.Laplacian5x5Filter(_photo, true);
                Refresh();
            }
        }

        private void FlipVertical(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.RotateFlip(_photo, RotateFlipType.RotateNoneFlipY);
                Refresh();
            }
        }

        private void FlipHorisontal(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.RotateFlip(_photo, RotateFlipType.RotateNoneFlipX);
                Refresh();
            }
        }

        private void Rotate90(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Rotate(_photo,90);
                Refresh();
            }
        }

        private void Rotate180(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Rotate(_photo, 180);
                Refresh();
            }
        }

        private void Rotate270(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Rotate(_photo, 270);
                Refresh();
            }
        }

        private void RotateMinus90(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Rotate(_photo, -90);
                Refresh();
            }
        }
        private void RotateMinus180(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Rotate(_photo, -180);
                Refresh();
            }
        }

        private void RotateMinus270(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Rotate(_photo, -270);
                Refresh();
            }
        }

        private void SelectAngle(object sender, RoutedEventArgs e) {
            needAngle = true;
            Selector angleSelector = new Selector();
            angleSelector.Show();
            angleSelector.Closed += new EventHandler(RotateBilinear);
        }
        private void RotateBilinear(object sender, EventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.RotateBilinear(_photo, Selector.InputNumber);
                Refresh();
            }   
        }

        private void SelectForGaborFilter(object sender, RoutedEventArgs e) {
            needDataForGaborFilter = true;
            Selector dataForGaborFilterSelector = new Selector();
            dataForGaborFilterSelector.Show();
            dataForGaborFilterSelector.Closed += new EventHandler(GaborFilter);
        }
        private void GaborFilter(object sender, EventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.GaborFilter(_photo, Selector.InputString);
                Refresh();
            }
        }

        private void SelectBrushSize(object sender, RoutedEventArgs e) {
            needBrushSize = true;
            Selector brushSizeSelector = new Selector();
            brushSizeSelector.Show();
            brushSizeSelector.Closed += new EventHandler(OilPainting);
        }
        private void OilPainting(object sender, EventArgs e) {
            if (ViewedPhoto.Source != null)  {
                Effects.OilPainting(_photo, Selector.InputNumber);
                Refresh();
            }
        }

        private void SelectRadius(object sender, RoutedEventArgs e) {
            needRadius = true;
            Selector radiusSelector = new Selector();
            radiusSelector.Show();
            radiusSelector.Closed += new EventHandler(Jitter);
        }
        private void Jitter(object sender, EventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Jitter(_photo, Selector.InputNumber);
                Refresh();
            }
        }

        private void Sharpen(object sender, RoutedEventArgs e) {
            if (ViewedPhoto.Source != null) {
                Effects.Sharpen(_photo);
                Refresh();
            }
        }

    }

}
