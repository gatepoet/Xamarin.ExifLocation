using Android.Graphics;
using Android.Media;

namespace Xamarin.ExifLocation
{
    public static class BitmapLoader
    {
        public static Bitmap FixOrientation(this Bitmap bitmap, string path)
        {
            var exif = new ExifInterface(path);
            var orientation = (Orientation)exif.GetAttributeInt(
                ExifInterface.TagOrientation,
                (int) Orientation.Normal);

            int rotate = 0;

            switch (orientation)
            {
                case Orientation.Rotate90:
                    rotate = 90;
                    break;
                case Orientation.Rotate180:
                    rotate = 180;
                    break;
                case Orientation.Rotate270:
                    rotate = 270;
                    break;
                default:
                    break;
            }

            if (rotate != 0)
            {
                int w = bitmap.Width;
                int h = bitmap.Height;

                var matrix = new Matrix();
                matrix.PreRotate(rotate);

                bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, w, h, matrix, false);
                bitmap = bitmap.Copy(Bitmap.Config.Argb8888, true);
            }

            return bitmap;
        }

        public static Bitmap LoadImage(string path, int sampleSize = 1)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InSampleSize = sampleSize,
                InPreferredConfig = Bitmap.Config.Argb8888
            };
            var bitmap = BitmapFactory.DecodeFile(path, options);

            return bitmap.FixOrientation(path);
        }

        public static Bitmap LoadImage(string path, int width, int height)
        {
            var options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            BitmapFactory.DecodeFile(path, options);

            return LoadImage(path, GetSampleSize(options, width, height));
        }

        private static int GetSampleSize(BitmapFactory.Options options, int width, int height)
        {
            var outHeight = options.OutHeight;
            var outWidth = options.OutWidth;
            var inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                    : outWidth / width;
            }

            return inSampleSize;
        }
    }
}

