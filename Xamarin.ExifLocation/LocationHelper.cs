using Android.Locations;
using Android.Media;
using Java.Text;
using Java.Util;
using Android.Content;

namespace Xamarin.ExifLocation
{
    static class LocationHelper
    {
        private static readonly SimpleDateFormat ExifFormat = new SimpleDateFormat("yyyy:MM:dd HH:mm:ss");

        public static Location ReadLocation(this ExifInterface exif)
        {
            var pos = new float[2];
            if (exif.GetLatLong(pos))
            {
                var date = exif.GetAttribute(ExifInterface.TagDatetime);
                var time = ExifFormat.Parse(date).Time;
                return new Location("exif")
                {
                    Latitude = pos[0],
                    Longitude = pos[1],
                    Time = time
                };
            }
            return null;
        }

        public static void WriteLocation(this ExifInterface exif, Location location)
        {
            exif.SetAttribute(ExifInterface.TagGpsLatitude, GPS.Convert(location.Latitude));
            exif.SetAttribute(ExifInterface.TagGpsLatitudeRef, GPS.LatitudeRef(location.Latitude));
            exif.SetAttribute(ExifInterface.TagGpsLongitude, GPS.Convert(location.Longitude));
            exif.SetAttribute(ExifInterface.TagGpsLongitudeRef, GPS.LongitudeRef(location.Longitude));
            
            exif.SetAttribute(ExifInterface.TagDatetime, ExifFormat.Format(new Date(location.Time)));
            exif.SaveAttributes();
        }

        public static string ToFormattedString(this Location location, Context context)
        {
            return location == null
                ? context.Resources.GetString(Resource.String.no_location)
                : $"{location.Latitude} {GPS.LatitudeRef(location.Latitude)}, {location.Longitude} {GPS.LongitudeRef(location.Longitude)}";
        }
    }
}

