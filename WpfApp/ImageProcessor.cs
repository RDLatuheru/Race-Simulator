using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Media;

namespace WpfApp
{
    public static class ImageProcessor
    {
        private static Dictionary<string, Bitmap> cache = new Dictionary<string, Bitmap>();

        public static Bitmap GetBitmap(string url)
        {
            if (!cache.ContainsKey(url))
            {
                cache.Add(url, new Bitmap(url));
            }

            return cache[url];
        }

        public static void ClearCache()
        {
            cache.Clear();
        }

        public static Bitmap CreateBitmap(int x, int y)
        {
            string key = "empty";
            if (!cache.ContainsKey(key))
            {
                cache.Add(key, new Bitmap(x,y));
                Graphics graphics = Graphics.FromImage(cache[key]);
                graphics.FillRectangle(new SolidBrush(System.Drawing.Color.Black), 0,0,x,y);
            }

            return (Bitmap)cache[key].Clone();
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
