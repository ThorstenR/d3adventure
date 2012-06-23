using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using Utilities.ScreenShot;

namespace Utilities.PixelPower
{
    public class PixelTools
    {
        public static Bitmap FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(0, 0, destWidth, destHeight),
                new Rectangle(0, 0, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        // If I tried I should be able to pass a bitmap to bitblt
        // it would ideally be faster but idk how much worth it?
        // http://www.nerdydork.com/crop-an-image-bitmap-in-c-or-vbnet.html
        //   This site has the same method as well as a resizing method :D
        // http://www.switchonthecode.com/tutorials/csharp-tutorial-image-editing-saving-cropping-and-resizing
        public static Bitmap CropBitmap(Bitmap bitmap, int cropX, int cropY, int cropWidth, int cropHeight)
        {
            Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }

        public static Bitmap CropBitmap(Bitmap bitmap, Rectangle rect)
        {
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }

        /// Thanks to http://www.vcskicks.com/image-invert.php
        #region Inverting
        /// <summary>
        /// This is the slowest method of inverting an image
        /// </summary>
        public static Image InvertImage(Image originalImg)
        {
            Bitmap invertedBmp = null;

            using (Bitmap originalBmp = new Bitmap(originalImg))
            {
                invertedBmp = new Bitmap(originalBmp.Width, originalBmp.Height);

                for (int x = 0; x < originalBmp.Width; x++)
                {
                    for (int y = 0; y < originalBmp.Height; y++)
                    {
                        //Get the color
                        Color clr = originalBmp.GetPixel(x, y);

                        //Invert the clr
                        clr = Color.FromArgb(255 - clr.R, 255 - clr.G, 255 - clr.B);

                        //Update the color
                        invertedBmp.SetPixel(x, y, clr);
                    }
                }
            }

            return (Image)invertedBmp;
        }

        /// <summary>
        /// This is the medium speed method of inverting an image
        /// </summary>
        public static Image InvertImageColorMatrix(Image originalImg)
        {
            Bitmap invertedBmp = new Bitmap(originalImg.Width, originalImg.Height);

            //Setup color matrix
            ColorMatrix clrMatrix = new ColorMatrix(new float[][]
                                                    {
                                                    new float[] {-1, 0, 0, 0, 0},
                                                    new float[] {0, -1, 0, 0, 0},
                                                    new float[] {0, 0, -1, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {1, 1, 1, 0, 1}
                                                    });

            using (ImageAttributes attr = new ImageAttributes())
            {
                //Attach matrix to image attributes
                attr.SetColorMatrix(clrMatrix);

                using (Graphics g = Graphics.FromImage(invertedBmp))
                {
                    g.DrawImage(originalImg, new Rectangle(0, 0, originalImg.Width, originalImg.Height),
                                0, 0, originalImg.Width, originalImg.Height, GraphicsUnit.Pixel, attr);
                }
            }

            return invertedBmp;
        }

        /// <summary>
        /// This is the fastest method of inverting an image
        /// </summary>
        public static Image InvertImageUnsafe(Image originalImg)
        {
            Bitmap bmp = new Bitmap(originalImg);

            //Format is BGRA, NOT ARGB.
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                           ImageLockMode.ReadWrite,
                                           PixelFormat.Format32bppArgb);

            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - bmp.Width * 4;
                int nWidth = bmp.Width;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        p[0] = (byte)(255 - p[0]); //red
                        p[1] = (byte)(255 - p[1]); //green
                        p[2] = (byte)(255 - p[2]); //blue
                        //p[3] is alpha, don't want to invert alpha

                        p += 4;
                    }
                    p += nOffset;
                }
            }

            bmp.UnlockBits(bmData);

            return (Image)bmp;
        }
        #endregion        

        /// <summary>
        /// Replaces the target color with the selected color
        /// Taken from http://www.vcskicks.com/fast-image-processing.php
        /// </summary>
        public static Bitmap ReplaceColorInImage(Bitmap bmp, Color target, Color replace, int tolerance)
        {
            //Create a new blank image
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);

            //Edit/Read the images with our FastBitmap class
            FastBitmap original = new FastBitmap(bmp);
            FastBitmap output = new FastBitmap(newBmp);

            //Lock both images to make them editable/readable
            original.LockImage();
            output.LockImage();

            //Look through the original image
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    //Application.DoEvents(); //Keep application from crashing

                    Color originalColor = original.GetPixel(x, y);
                    //Match the current bitmap color with the target bitmap
                    if (isColorMatch(originalColor, target, tolerance))
                    {
                        //Write the new color instead
                        output.SetPixel(x, y, replace);
                    }
                    else
                    {
                        //Write the original color
                        output.SetPixel(x, y, originalColor);
                    }
                }
            }

            //Unlock the images
            original.UnlockImage();
            output.UnlockImage();

            //Return the final bitmap (not the FastBitmap object)
            return newBmp;
        }

        /// <summary>
        ///  Used by the ReplaceColorInImage Function
        ///  But this allows you to test if two colors
        ///  are equivelent to eachother due to the tolerance between them.
        /// </summary>
        public static bool isColorMatch(Color color1, Color color2, int tolerance)
        {
            //The difference in values of RGB should be within the tolerance level
            return Math.Abs(color1.R - color2.R) <= tolerance &&
                   Math.Abs(color1.G - color2.G) <= tolerance &&
                   Math.Abs(color1.B - color2.B) <= tolerance;
        }

        /// <summary>
        /// My modified pixel search function
        /// Takes an image and finds all the pixels that match
        /// the PixelColorOld paramater along with the shade variation
        /// and replaces them with the PixelColorNew parameter
        /// </summary>
        public static Bitmap SwitchColors(Bitmap img, Color PixelColorOld, int Shade_Variation, Color PixelColorNew)
        {
            Bitmap RegionIn_Bitmap = img;
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { PixelColorOld.B, PixelColorOld.G, PixelColorOld.R }; //bgr
            int[] Formatted_Color_New = new int[3] { PixelColorNew.B, PixelColorNew.G, PixelColorNew.R };

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {

                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                        {
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                            {
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                {
                                    row[x * 3] = Convert.ToByte(Formatted_Color_New[0]);
                                    row[x * 3 + 1] = Convert.ToByte(Formatted_Color_New[1]);
                                    row[x * 3 + 2] = Convert.ToByte(Formatted_Color_New[2]);
                                }
                            }
                        }


                    }
                }
            }

            RegionIn_Bitmap.UnlockBits(RegionIn_BitmapData);
            return RegionIn_Bitmap;
        }

        /// <summary>
        /// Replaces all colors in an image besides the specified color and changes to the PixelColorNew parameter
        /// </summary>
        public static Bitmap KeepAndReplace(Bitmap img, Color PixelColorToKeep, Color PixelColorNew)
        {
            Bitmap RegionIn_Bitmap = img;
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { PixelColorToKeep.B, PixelColorToKeep.G, PixelColorToKeep.R }; //bgr
            int[] Formatted_Color_New = new int[3] { PixelColorNew.B, PixelColorNew.G, PixelColorNew.R };

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (!(row[x * 3] == Formatted_Color[0] && //green
                            row[(x * 3) + 1] == Formatted_Color[1] && //red
                            row[(x * 3) + 2] == Formatted_Color[2])) //blue
                        {

                            row[x * 3] = Convert.ToByte(Formatted_Color_New[0]);
                            row[x * 3 + 1] = Convert.ToByte(Formatted_Color_New[1]);
                            row[x * 3 + 2] = Convert.ToByte(Formatted_Color_New[2]);
                        }
                    }
                }
            }

            RegionIn_Bitmap.UnlockBits(RegionIn_BitmapData);
            return RegionIn_Bitmap;
        }

        /// <summary>
        /// Replaces all colors in an image besides the specified color and changes to the PixelColorNew parameter
        /// also takes a parameter for shade variation
        /// </summary>
        public static Bitmap KeepAndReplace(Bitmap img, Color PixelColorToKeep, Color PixelColorNew, int Shade_Variation)
        {
            Bitmap RegionIn_Bitmap = img;
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { PixelColorToKeep.B, PixelColorToKeep.G, PixelColorToKeep.R }; //bgr
            int[] Formatted_Color_New = new int[3] { PixelColorNew.B, PixelColorNew.G, PixelColorNew.R };

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (!(row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation) && //red
                            row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation) && //green
                            row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation))) //blue
                        {
                            row[x * 3] = Convert.ToByte(Formatted_Color_New[0]);
                            row[x * 3 + 1] = Convert.ToByte(Formatted_Color_New[1]);
                            row[x * 3 + 2] = Convert.ToByte(Formatted_Color_New[2]);
                        }
                    }
                }
            }

            RegionIn_Bitmap.UnlockBits(RegionIn_BitmapData);
            return RegionIn_Bitmap;
        }

    }

    public class PixelSearching
    {
        // http://www.elitepvpers.de/forum/gamehacking-coding/247732-c-pixelsearch-search-screen-pixel.html#post2202857
        /// modified it to use my method of getting the desktop, which is supposed to be much faster, maybe the method autoit uses?
        public static Point PixelSearch(int px, int py, int pwidth, int pheight, Color PixelColor, int Shade_Variation)
        {
            Point Pixel_Coords = new Point(-1, -1);
            Bitmap RegionIn_Bitmap = CaptureScreen.GetDesktopImage(px, py, pwidth, pheight);
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { PixelColor.B, PixelColor.G, PixelColor.R }; //bgr

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                        {
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                            {
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                {
                                    Pixel_Coords = new Point(x + px, y + py);
                                    goto end;
                                }
                            }
                        }
                    }
                }
            }

        end:
            RegionIn_Bitmap.UnlockBits(RegionIn_BitmapData);
            return Pixel_Coords;
        }
        
        /// searches whole desktop, was going to reuse the above method, but it would require either
        /// finding a method to get the desktop size, or it would require to grab the desktop
        /// image twice, which would be a horrid waste.
        public static Point PixelSearch(Color PixelColor, int Shade_Variation)
        {
            Point Pixel_Coords = new Point(-1, -1);
            Bitmap RegionIn_Bitmap = CaptureScreen.GetDesktopImage();
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { PixelColor.B, PixelColor.G, PixelColor.R }; //bgr

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                        {
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                            {
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                {
                                    Pixel_Coords = new Point(x, y);
                                    goto end;
                                }
                            }
                        }
                    }
                }
            }

        end:
            RegionIn_Bitmap.UnlockBits(RegionIn_BitmapData);
            return Pixel_Coords;
        }

        public static Point PixelSearch(Bitmap bmp, Color PixelColor, int Shade_Variation)
        {
            Point Pixel_Coords = new Point(-1, -1);
            BitmapData RegionIn_BitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { PixelColor.B, PixelColor.G, PixelColor.R }; //bgr

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                        {
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                            {
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                {
                                    Pixel_Coords = new Point(x, y);
                                    goto end;
                                }
                            }
                        }
                    }
                }
            }

        end:
            bmp.UnlockBits(RegionIn_BitmapData);
            return Pixel_Coords;
        }

        public static Point PixelSearch(Bitmap bmp, Color PixelColor)
        {
            return PixelSearch(bmp, PixelColor, 0);
        }

        public static Point PixelSearch(Bitmap bmp, Color PixelColor, int notIncludedX, int notIncludedY)
        {
            Point Pixel_Coords = new Point(-1, -1);
            BitmapData RegionIn_BitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int[] Formatted_Color = new int[3] { PixelColor.B, PixelColor.G, PixelColor.R }; //bgr

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);

                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0]) & row[x * 3] <= (Formatted_Color[0])) //blue
                        {
                            if (row[(x * 3) + 1] >= (Formatted_Color[1]) & row[(x * 3) + 1] <= (Formatted_Color[1])) //green
                            {
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] ) & row[(x * 3) + 2] <= (Formatted_Color[2])) //red
                                {
                                    Pixel_Coords = new Point(x, y);
                                    if (Pixel_Coords != new Point(1,1) && //center
                                        Pixel_Coords != new Point(0,0) && //topleft
                                        Pixel_Coords != new Point(2,0) && //topright
                                        Pixel_Coords != new Point(0,2) && //bottomleft
                                        Pixel_Coords != new Point(2,2))   //bottomright
                                        goto end;
                                }
                            }
                        }
                    }
                }
            }

        end:
            bmp.UnlockBits(RegionIn_BitmapData);
            return Pixel_Coords;
        }

        public static Point PixelSearch(Bitmap bmp, Color PixelColor, Point point)
        {
            return PixelSearch(bmp, PixelColor, point.X, point.Y);
        }

    }

    unsafe public class FastBitmap
    {
        private struct PixelData
        {
            public byte blue;
            public byte green;
            public byte red;
            public byte alpha;

            public override string ToString()
            {
                return "(" + alpha.ToString() + ", " + red.ToString() + ", " + green.ToString() + ", " + blue.ToString() + ")";
            }
        }

        private Bitmap workingBitmap = null;
        private int width = 0;
        private BitmapData bitmapData = null;
        private Byte* pBase = null;

        public FastBitmap(Bitmap inputBitmap)
        {
            workingBitmap = inputBitmap;
        }

        public void LockImage()
        {
            Rectangle bounds = new Rectangle(Point.Empty, workingBitmap.Size);

            width = (int)(bounds.Width * sizeof(PixelData));
            if (width % 4 != 0) width = 4 * (width / 4 + 1);

            //Lock Image
            bitmapData = workingBitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            pBase = (Byte*)bitmapData.Scan0.ToPointer();
        }

        private PixelData* pixelData = null;

        public Color GetPixel(int x, int y)
        {
            pixelData = (PixelData*)(pBase + y * width + x * sizeof(PixelData));
            return Color.FromArgb(pixelData->alpha, pixelData->red, pixelData->green, pixelData->blue);
        }

        public Color GetPixelNext()
        {
            pixelData++;
            return Color.FromArgb(pixelData->alpha, pixelData->red, pixelData->green, pixelData->blue);
        }

        public void SetPixel(int x, int y, Color color)
        {
            PixelData* data = (PixelData*)(pBase + y * width + x * sizeof(PixelData));
            data->alpha = color.A;
            data->red = color.R;
            data->green = color.G;
            data->blue = color.B;
        }

        public void UnlockImage()
        {
            workingBitmap.UnlockBits(bitmapData);
            bitmapData = null;
            pBase = null;
        }
    }

}
