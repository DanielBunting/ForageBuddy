using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ForageBuddy
{
    public class LockedBitmap : IDisposable
    {
        public Bitmap Source { get; private set; } = null;
        private IntPtr _iptr = IntPtr.Zero;
        private BitmapData _bitmapData = null;

        private byte[] Pixels { get; set; }
        private int Depth { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private int RowSize { get; set; }

        private bool IsDisposed { get; set; }

        public LockedBitmap(Bitmap source)
        {
            this.Source = source;
            IsDisposed = false;
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(typeof(LockedBitmap).Name);

                // Get width and TopLeft of bitmap
                Width = Source.Width;
                Height = Source.Height;

                // Create rectangle to lock
                var rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = Image.GetPixelFormatSize(Source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                _bitmapData = Source.LockBits(rect, ImageLockMode.ReadWrite,
                                             Source.PixelFormat);

                // create byte array to copy pixel values
                RowSize = _bitmapData.Stride < 0 ? -_bitmapData.Stride : _bitmapData.Stride;
                Pixels = new byte[Height * RowSize];
                _iptr = _bitmapData.Scan0;

                // Copy data from pointer to array
                // Not working for negative Stride see. http://stackoverflow.com/a/10360753/1498252
                //Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
                // Solution for positive and negative Stride:
                for (int y = 0; y < Height; y++)
                {
                    Marshal.Copy(IntPtr.Add(_iptr, y * _bitmapData.Stride),
                        Pixels, y * RowSize,
                        RowSize);
                }

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(typeof(LockedBitmap).Name);
                if (_bitmapData == null)
                    throw new InvalidOperationException("Image is not locked.");

                // Copy data from byte array to pointer
                //Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);
                for (int y = 0; y < Height; y++)
                {
                    Marshal.Copy(Pixels, y * RowSize,
                        IntPtr.Add(_iptr, y * _bitmapData.Stride),
                        RowSize);
                }

                // Unlock bitmap data
                Source.UnlockBits(_bitmapData);
                _bitmapData = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(typeof(LockedBitmap).Name);

            Color clr = Color.Empty;

            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = (y * RowSize) + (x * cCount);

            if (i > Pixels.Length - cCount) return Color.Black;

            if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                byte a = Pixels[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            if (Depth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            if (Depth == 8)
            // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = Pixels[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(typeof(LockedBitmap).Name);

            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = (y * RowSize) + (x * cCount);
            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }
            if (Depth == 24) // For 24 bpp set Red, Green and Blue
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }
            if (Depth == 8)
            // For 8 bpp set color value (Red, Green and Blue values are the same)
            {
                Pixels[i] = color.B;
            }
        }

        #region IDisposable Members

        ~LockedBitmap()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_bitmapData != null)
            {
                try { Source.UnlockBits(_bitmapData); }
                catch { }
                _bitmapData = null;
            }
            Source = null;
            IsDisposed = true;
        }

        #endregion
    }
}
