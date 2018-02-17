using Java.Lang;

namespace Xamarin.ExifLocation
{
    public class GPS
    {
        private static StringBuilder sb = new StringBuilder(20);
        /**
        * returns ref for latitude which is S or N.
        *
        * @param latitude
        * @return S or N
*/
        public static string LatitudeRef(double latitude)
        {
            return latitude < 0.0d ? "S" : "N";
        }
        /**
        * returns ref for latitude which is S or N.
        *
        * @param latitude
        * @return S or N
*/
        public static string LongitudeRef(double longitude)
        {
            return longitude < 0.0d ? "W" : "E";
        }
        /**
        * convert latitude into DMS (degree minute second) format. For instance<br/>
        * -79.948862 becomes<br/>
        * 79/1,56/1,55903/1000<br/>
        * It works for latitude and longitude<br/>
        *
        * @param latitude could be longitude.
        * @return
*/
        public static string Convert(double value)
        {
            value = Math.Abs(value);
            int degree = (int)value;
            value *= 60;
            value -= degree * 60.0d;
            int minute = (int)value;
            value *= 60;
            value -= minute * 60.0d;
            int second = (int)(value * 1000.0d);
            sb.SetLength(0);
            sb.Append(degree);
            sb.Append("/1,");
            sb.Append(minute);
            sb.Append("/1,");
            sb.Append(second);
            sb.Append("/1000,");
            return sb.ToString();
        }
    }
}

