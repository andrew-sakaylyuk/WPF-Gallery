using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System;
using System.Windows;
using Accord.Imaging.Filters;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace SimpleImageGallery {

    public static class Effects {

        //--------------- Effects using Color Matrix ---------------
        private static Bitmap ApplyColorMatrix(Bitmap source, ColorMatrix colorMatrix) {
            Bitmap dest = new Bitmap(source.Width, source.Height, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(dest)) {
                ImageAttributes bmpAttributes = new ImageAttributes();
                bmpAttributes.SetColorMatrix(colorMatrix);
                graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                    0, 0, source.Width, source.Height, GraphicsUnit.Pixel, bmpAttributes);
            }
            source.Dispose();
            return dest;
        }

        public static void Grayscale(Photo photo) {
            Bitmap img = GetBitmap(photo.Image);
            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                //           r     g     b     a     t
                new float[] {.3f,  .3f,  .3f,  0,    0}, // red
                new float[] {.59f, .59f, .59f, 0,    0}, // green
                new float[] {.11f, .11f, .11f, 0,    0}, // blue
                new float[] {0,    0,    0,    1,    0}, // alpha 
                new float[] {0,    0,    0,    0,    1}  // three translations
            });
            photo.Image = BitmapFrame.Create(GetBitmapSource(ApplyColorMatrix(img, colorMatrix)));
        }

        public static void Negative(Photo photo) {
            Bitmap img = GetBitmap(photo.Image);
            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                //           r    g    b    a    t
                new float[] {-1,  0,   0,   0,   0}, // red
                new float[] {0,  -1,   0,   0,   0}, // green
                new float[] {0,   0,  -1,   0,   0}, // blue
                new float[] {0,   0,   0,   1,   0}, // alpha 
                new float[] {1,   1,   1,   1,   1}  // three translations
            });
            photo.Image = BitmapFrame.Create(GetBitmapSource(ApplyColorMatrix(img, colorMatrix)));
        }

        public static void Transparency(Photo photo) {
            Bitmap img = GetBitmap(photo.Image);
            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                //           r     g     b     a     t
                new float[] {1,    0,    0,    0,    0}, // red
                new float[] {0,    1,    0,    0,    0}, // green
                new float[] {0,    0,    1,    0,    0}, // blue
                new float[] {0,    0,    0,    0.3f, 0}, // alpha 
                new float[] {0,    0,    0,    0,    1}  // three translations
            });
            photo.Image = BitmapFrame.Create(GetBitmapSource(ApplyColorMatrix(img, colorMatrix)));
        }

        public static void SepiaTone(Photo photo) {
            Bitmap img = GetBitmap(photo.Image);
            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                //           r      g      b      a      t
                new float[] {.393f, .349f, .272f, 0,     0}, // red
                new float[] {.769f, .686f, .534f, 0,     0}, // green
                new float[] {.189f, .168f, .131f, 0,     0}, // blue
                new float[] {0,     0,     0,     1,     0}, // alpha 
                new float[] {0,     0,     0,     0,     1}  // three translations
            });
            photo.Image = BitmapFrame.Create(GetBitmapSource(ApplyColorMatrix(img, colorMatrix)));
        }
        //------------ End of Effects using Color Matrix -----------

        //----------------- Edge Detection Filters -----------------
        public static void PrewittFilter(Photo photo, bool grayscale = false) {
            Bitmap source = GetBitmap(photo.Image);
            Bitmap resultBitmap = ConvolutionFilter(source,
                /*Horizontal*/ new double[,] { { -1,  0,  1 }, { -1,  0,  1 }, { -1,  0,  1 } },
                /*Vertical*/ new double[,] { {  1,  1,  1 }, {  0,  0,  0 }, { -1, -1, -1, } },
                1.0, 0, grayscale);
            photo.Image = BitmapFrame.Create(GetBitmapSource(resultBitmap));
        }

        public static void SobelFilter(Photo photo, bool grayscale = false) {
            Bitmap source = GetBitmap(photo.Image);
            Bitmap resultBitmap = ConvolutionFilter(source,
                /*Horizontal*/ new double[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } },
                /*Vertical*/ new double[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } },
                1.0, 0, grayscale);
            photo.Image = BitmapFrame.Create(GetBitmapSource(resultBitmap));
        }

        public static void KirschFilter(Photo photo, bool grayscale = false)  {
            Bitmap source = GetBitmap(photo.Image);
            Bitmap resultBitmap = ConvolutionFilter(source,
                /*Horizontal*/ new double[,]{ { 5, 5, 5 }, { -3, 0, -3 }, { -3, -3, -3 } },
                /*Vertical*/ new double[,] { {  5, -3, -3 }, {  5,  0, -3 }, { 5, -3, -3 } },
                1.0, 0, grayscale);
            photo.Image = BitmapFrame.Create(GetBitmapSource(resultBitmap));
        }

        public static void Laplacian3x3Filter(Photo photo, bool grayscale = false)  {
            Bitmap source = GetBitmap(photo.Image);
            Bitmap resultBitmap = ConvolutionFilter(source,
                new double[,] { { -1, -1, -1 },  { -1,  8, -1 }, { -1, -1, -1 } },
                1.0, 0, grayscale);
            photo.Image = BitmapFrame.Create(GetBitmapSource(resultBitmap));
        }

        public static void Laplacian5x5Filter(Photo photo, bool grayscale = false) {
            Bitmap source = GetBitmap(photo.Image);
            Bitmap resultBitmap = ConvolutionFilter(source,
                new double[,] { 
                    { -1, -1, -1, -1, -1 },  
                    { -1, -1, -1, -1, -1 }, 
                    { -1, -1, 24, -1, -1 }, 
                    { -1, -1, -1, -1, -1, }, 
                    { -1, -1, -1, -1, -1 } },
                1.0, 0, grayscale);
            photo.Image = BitmapFrame.Create(GetBitmapSource(resultBitmap));
        }

        public static Bitmap ConvolutionFilter 
        (
            this Bitmap sourceBitmap, 
            double[,] xFilterMatrix, 
            double[,] yFilterMatrix, 
            double factor = 1, 
            int bias = 0,
            bool grayscale = false
        ) {
            BitmapData sourceData = sourceBitmap.LockBits(
                new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), 
                ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            if (grayscale == true) {
                float rgb = 0;
                for (int k = 0; k < pixelBuffer.Length; k += 4) {
                    rgb = pixelBuffer[k] * 0.11f;
                    rgb += pixelBuffer[k + 1] * 0.59f;
                    rgb += pixelBuffer[k + 2] * 0.3f;
                    pixelBuffer[k] = (byte)rgb;
                    pixelBuffer[k + 1] = pixelBuffer[k];
                    pixelBuffer[k + 2] = pixelBuffer[k];
                    pixelBuffer[k + 3] = 255;
                }
            }
            double blueX = 0.0;
            double greenX = 0.0;
            double redX = 0.0;
            double blueY = 0.0;
            double greenY = 0.0;
            double redY = 0.0;
            double blueTotal = 0.0;
            double greenTotal = 0.0;
            double redTotal = 0.0;
            int filterOffset = 1;
            int calcOffset = 0;
            int byteOffset = 0;
            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++) {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++) {
                    blueX = greenX = redX = 0;
                    blueY = greenY = redY = 0;
                    blueTotal = greenTotal = redTotal = 0.0;
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++) {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++) {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                            blueX += (double)(pixelBuffer[calcOffset]) * 
                                xFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            greenX += (double)(pixelBuffer[calcOffset + 1]) * 
                                xFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            redX += (double)(pixelBuffer[calcOffset + 2]) * 
                                xFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            blueY += (double)(pixelBuffer[calcOffset]) * 
                                yFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            greenY += (double)(pixelBuffer[calcOffset + 1]) * 
                                yFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            redY += (double)(pixelBuffer[calcOffset + 2]) * 
                                yFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                        }
                    }
                    blueTotal = Math.Sqrt((blueX * blueX) + (blueY * blueY));
                    greenTotal = Math.Sqrt((greenX * greenX) + (greenY * greenY));
                    redTotal = Math.Sqrt((redX * redX) + (redY * redY));
                    if (blueTotal > 255) { blueTotal = 255; }
                    else if (blueTotal < 0) { blueTotal = 0; }
                    if (greenTotal > 255) { greenTotal = 255; }
                    else if (greenTotal < 0) { greenTotal = 0; }
                    if (redTotal > 255) { redTotal = 255; }
                    else if (redTotal < 0) { redTotal = 0; }
                    resultBuffer[byteOffset] = (byte)(blueTotal);
                    resultBuffer[byteOffset + 1] = (byte)(greenTotal);
                    resultBuffer[byteOffset + 2] = (byte)(redTotal);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), 
                ImageLockMode.WriteOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }

        private static Bitmap ConvolutionFilter
        (
            Bitmap sourceBitmap, 
            double[,] filterMatrix,
            double factor = 1, 
            int bias = 0, 
            bool grayscale = false
        ) {
            BitmapData sourceData = sourceBitmap.LockBits(
                new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), 
                ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            if (grayscale == true) {
                float rgb = 0;
                for (int k = 0; k < pixelBuffer.Length; k += 4) {
                    rgb = pixelBuffer[k] * 0.11f;
                    rgb += pixelBuffer[k + 1] * 0.59f;
                    rgb += pixelBuffer[k + 2] * 0.3f;
                    pixelBuffer[k] = (byte)rgb;
                    pixelBuffer[k + 1] = pixelBuffer[k];
                    pixelBuffer[k + 2] = pixelBuffer[k];
                    pixelBuffer[k + 3] = 255;
                }
            }
            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;
            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);
            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset = 0;
            int byteOffset = 0;
            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++) {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++) {
                    blue = 0; green = 0; red = 0;
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++) {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++) {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                            blue += (double)(pixelBuffer[calcOffset]) * 
                                filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            green += (double)(pixelBuffer[calcOffset + 1]) * 
                                filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            red += (double)(pixelBuffer[calcOffset + 2]) * 
                                filterMatrix[filterY + filterOffset, filterX + filterOffset];
                        }
                    }
                    blue = factor * blue + bias;
                    green = factor * green + bias;
                    red = factor * red + bias;
                    if (blue > 255) { blue = 255; }
                    else if (blue < 0) { blue = 0; }
                    if (green > 255) { green = 255; }
                    else if (green < 0) { green = 0; }
                    if (red > 255) { red = 255; }
                    else if (red < 0) { red = 0; }
                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), 
                ImageLockMode.WriteOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }

        public static void GradientEdgeDetect
        (
            Photo photo, 
            int threshold = 0, 
            float redFactor = 1.0f, 
            float greenFactor = 1.0f, 
            float blueFactor = 1.0f
        ) {
            Bitmap sourceBitmap = GetBitmap(photo.Image);
            BitmapData sourceData = sourceBitmap.LockBits(
                new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), 
                ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            int byteOffset = 0;
            int blueGradient, greenGradient, redGradient = 0;
            double blue = 0, green = 0, red = 0;
            bool exceedsThreshold = false;
            for (int offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++) {
                for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++) {
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                    blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - 
                        pixelBuffer[byteOffset + sourceData.Stride]);
                    byteOffset++;
                    greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - 
                        pixelBuffer[byteOffset + sourceData.Stride]);
                    byteOffset++;
                    redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    redGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] -
                        pixelBuffer[byteOffset + sourceData.Stride]);
                    if (blueGradient + greenGradient + redGradient > threshold) { exceedsThreshold = true; }
                    else {
                        byteOffset -= 2;
                        blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                        byteOffset++;
                        greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                        byteOffset++;
                        redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                        if (blueGradient + greenGradient + redGradient > threshold) { exceedsThreshold = true; }
                        else {
                            byteOffset -= 2;
                            blueGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] -
                            pixelBuffer[byteOffset + sourceData.Stride]);
                            byteOffset++;
                            greenGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - 
                                pixelBuffer[byteOffset + sourceData.Stride]);
                            byteOffset++;
                            redGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - 
                                pixelBuffer[byteOffset + sourceData.Stride]);
                            if (blueGradient + greenGradient + redGradient > threshold) {
                                exceedsThreshold = true;
                            } else {
                                byteOffset -= 2;
                                blueGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - 
                                    pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - 
                                    pixelBuffer[byteOffset + sourceData.Stride - 4]);
                                byteOffset++;
                                greenGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - 
                                    pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - 
                                    pixelBuffer[byteOffset + sourceData.Stride - 4]);
                                byteOffset++;
                                redGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - 
                                    pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                redGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] -  
                                    pixelBuffer[byteOffset + sourceData.Stride - 4]);
                                if (blueGradient + greenGradient + redGradient > threshold) {
                                    exceedsThreshold = true;
                                } else { exceedsThreshold = false; }
                            }
                        }
                    }
                    byteOffset -= 2;
                    if (exceedsThreshold) {
                            blue = blueGradient * blueFactor;
                            green = greenGradient * greenFactor;
                            red = redGradient * redFactor;
                    } else blue = green = red = 0;
                    blue = (blue > 255 ? 255 : (blue < 0 ? 0 : blue));
                    green = (green > 255 ? 255 : (green < 0 ? 0 : green));
                    red = (red > 255 ? 255 : (red < 0 ? 0 : red));
                    resultBuffer[byteOffset] = (byte)blue;
                    resultBuffer[byteOffset + 1] = (byte)green;
                    resultBuffer[byteOffset + 2] = (byte)red;
                    resultBuffer[byteOffset + 3] = 255;
                }
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), 
                ImageLockMode.WriteOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            photo.Image = BitmapFrame.Create(GetBitmapSource(resultBitmap));
        }
        //------------- End of Edge Detection Filters --------------

        //------------------------ Filters -------------------------
        public static void MedianFilter
        (
            Photo photo, 
            int matrixSize, 
            int bias = 0, 
            bool grayscale = false
        ) {
            Bitmap source = GetBitmap(photo.Image);
            BitmapData sourceData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height), 
                ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            source.UnlockBits(sourceData);
            if (grayscale == true) {
                float rgb = 0;
                for (int k = 0; k < pixelBuffer.Length; k += 4) {
                    rgb = pixelBuffer[k] * 0.11f;
                    rgb += pixelBuffer[k + 1] * 0.59f;
                    rgb += pixelBuffer[k + 2] * 0.3f;
                    pixelBuffer[k] = (byte)rgb;
                    pixelBuffer[k + 1] = pixelBuffer[k];
                    pixelBuffer[k + 2] = pixelBuffer[k];
                    pixelBuffer[k + 3] = 255;
                }
            }
            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;
            int byteOffset = 0;
            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;
            for (int offsetY = filterOffset; offsetY < source.Height - filterOffset; offsetY++) {
                for (int offsetX = filterOffset; offsetX < source.Width - filterOffset; offsetX++)  {
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                    neighbourPixels.Clear();
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++) {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++) {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                            neighbourPixels.Add(BitConverter.ToInt32(pixelBuffer, calcOffset));
                        }
                    }
                    neighbourPixels.Sort();
                    middlePixel = BitConverter.GetBytes(neighbourPixels[filterOffset]);
                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
                }
            }
            Bitmap resultBitmap = new Bitmap(source.Width, source.Height);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), 
                ImageLockMode.WriteOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            photo.Image = BitmapFrame.Create(GetBitmapSource(resultBitmap));
        }

        public static void GaussianBlur(Photo photo, int radial) {
            Bitmap img = GetBitmap(photo.Image);
            var gaussianBlur = new GaussianBlur(img);
            img = gaussianBlur.Process(radial);
            photo.Image = BitmapFrame.Create(GetBitmapSource(img));
        }
        //--------------------- End of Filters  --------------------
       
        //-------------------- Rotate and Flip ---------------------
        public static void RotateFlip(Photo photo, RotateFlipType type) {
            Bitmap img = GetBitmap(photo.Image);
            img.RotateFlip(type);
            photo.Image = BitmapFrame.Create(GetBitmapSource(img));
        }

        public static void Rotate(Photo photo, double angle) {
            BitmapSource img = photo.Image;
            CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            photo.Image = BitmapFrame.Create(new TransformedBitmap(cache, new RotateTransform(angle)));
        } 

        public static void RotateBilinear(Photo photo, double angle)  {
            Bitmap img = GetBitmap(photo.Image);
            if (angle > 180) angle -= 360;
            System.Drawing.Color bkColor = System.Drawing.Color.Transparent;
            System.Drawing.Imaging.PixelFormat pf = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
            float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
            float newImgWidth = sin * img.Height + cos * img.Width;
            float newImgHeight = sin * img.Width + cos * img.Height;
            float originX = 0f; float originY = 0f;
            if (angle > 0) {
                if (angle <= 90)
                    originX = sin * img.Height;
                else {
                    originX = newImgWidth;
                    originY = newImgHeight - sin * img.Width;
                }
            } else {
                if (angle >= -90)
                    originY = sin * img.Width;
                else {
                    originX = newImgWidth - sin * img.Height;
                    originY = newImgHeight;
                }
            }
            Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(bkColor);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform((float)angle); // set up rotate
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(img, 0, 0); // draw the image at 0, 0
            g.Dispose();
            photo.Image = BitmapFrame.Create(GetBitmapSource(newImg));
        }
        //----------------- End of Rotate and Flip -----------------

        //---------------- Filters from Accord.NET -----------------
        public static void GaborFilter(Photo photo, string parameters) {
            Bitmap img = GetBitmap(photo.Image);
            GaborFilter gaborFilter = new GaborFilter();
            try  {
                double[] gaborParams = Array.ConvertAll(
                    System.Text.RegularExpressions.Regex.Split(parameters, ";"), Double.Parse);
                gaborFilter.Gamma = gaborParams[0];
                gaborFilter.Lambda = gaborParams[1];
                gaborFilter.Psi = gaborParams[2];
                gaborFilter.Sigma = gaborParams[3];
                gaborFilter.Size = (int)gaborParams[4];
                gaborFilter.Theta = gaborParams[5];
            }
            catch { }
            img = gaborFilter.Apply(Get24bppRgb(img));
            photo.Image = BitmapFrame.Create(GetBitmapSource(Get32bppArgb(img)));
        }

        public static void OilPainting(Photo photo, int brushSize) {
            Bitmap img = GetBitmap(photo.Image);
            OilPainting oilPainting = new OilPainting(brushSize);
            img = oilPainting.Apply(Get24bppRgb(img));
            photo.Image = BitmapFrame.Create(GetBitmapSource(Get32bppArgb(img)));
        }

        public static void Jitter(Photo photo, int radius) {
            Bitmap img = GetBitmap(photo.Image);
            Jitter jitter = new Jitter(radius);
            img = jitter.Apply(Get24bppRgb(img));
            photo.Image = BitmapFrame.Create(GetBitmapSource(Get32bppArgb(img)));
        }

        public static void Sharpen(Photo photo) {
            Bitmap img = GetBitmap(photo.Image);
            Sharpen sharpen = new Sharpen();
            img = sharpen.Apply(Get24bppRgb(img));
            photo.Image = BitmapFrame.Create(GetBitmapSource(Get32bppArgb(img)));
        }
        //------------- End of filters from Accord.NET -------------

        //----------------------- Converters -----------------------
        public static Bitmap GetBitmap(BitmapSource source)  {
            Bitmap bitmap;
            using (var outStream = new MemoryStream()) {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(source));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        public static BitmapSource GetBitmapSource(Bitmap source) {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                  source.GetHbitmap(),
                  IntPtr.Zero,
                  Int32Rect.Empty,
                  BitmapSizeOptions.FromEmptyOptions());
        }

        private static Bitmap Get24bppRgb(Image image){
            var bitmap = new Bitmap(image);
            var bitmap24 = new Bitmap(bitmap.Width, bitmap.Height, 
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bitmap24)) {
                gr.DrawImage(bitmap, new Rectangle(0, 0, bitmap24.Width, bitmap24.Height));
            }
            return bitmap24;
        }

        private static Bitmap Get32bppArgb(Image image) {
            Bitmap bitmap32 = new Bitmap(image.Width, image.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(bitmap32)) {
                gr.DrawImage(image, new Rectangle(0, 0, bitmap32.Width, bitmap32.Height));
            }
            return bitmap32;
        }
        //------------------- End of Converters --------------------

    }

}
