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


    //public class MediaHelper
    //{
    //    public static string GetPath(Context context, Uri uri)
    //    {
    //        bool isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

    //        // DocumentProvider
    //        if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
    //        {
    //            // ExternalStorageProvider
    //            if (isExternalStorageDocument(uri))
    //            {
    //                var docId = DocumentsContract.GetDocumentId(uri);
    //                var split = docId.Split(':');
    //                var type = split[0];

    //                if ("primary".Equals(type, StringComparison.InvariantCultureIgnoreCase))
    //                {
    //                    return Environment.ExternalStorageDirectory + "/" + split[1];
    //                }
    //                // TODO handle non-primary volumes
    //            }
    //            // DownloadsProvider
    //            else if (IsDownloadsDocument(uri))
    //            {
    //                var id = DocumentsContract.GetDocumentId(uri);
    //                var contentUri = ContentUris.WithAppendedId(Uri.Parse("content://downloads/public_downloads"), long.Parse(id));
    //                return GetDataColumn(context, contentUri, null, null);
    //            }
    //            // MediaProvider
    //            else
    //            if (IsMediaDocument(uri))
    //            {
    //                var docId = DocumentsContract.GetDocumentId(uri);
    //                var split = docId.Split(':');
    //                var type= split[0];
    //                Uri contentUri = null;
    //                switch (type)
    //                {
    //                    case "image":
    //                    case "video":
    //                    case "audio":
    //                        contentUri = MediaStore.Images.Media.ExternalContentUri;
    //                        break;
    //                    default:
    //                        break;
    //                }

    //                var selection = "_id=?";
    //                var selectionArgs = new [] { split[1] };
    //                return GetDataColumn(context, contentUri, selection, selectionArgs);
    //            }
    //        }
    //        // MediaStore (and general)
    //        else if ("content".Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase))
    //        {
    //            // Return the remote address
    //            if (IsGooglePhotosUri(uri))
    //                return uri.LastPathSegment;
    //            return GetDataColumn(context, uri, null, null);
    //        }
    //        // File
    //        else if ("file".Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase))
    //        {
    //            return uri.Path;
    //        }
    //        return null;
    //    }

    //    public static string GetDataColumn(Context context, Uri uri, string selection, string[] selectionArgs)
    //    {
    //        ICursor cursor = null;
    //        var column = MediaStore.Images.Media.InterfaceConsts.Data;
    //        var projection = new[] { column };
    //        try
    //        {
    //            cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs, null);
    //            if (cursor != null && cursor.MoveToFirst())
    //            {
    //                var index = cursor.GetColumnIndexOrThrow(column);
    //                return cursor.GetString(index);
    //            }
    //        }
    //        finally
    //        {
    //            if (cursor != null)
    //                cursor.Close();
    //        }
    //        return null;
    //    }

    //    public static bool isExternalStorageDocument(Uri uri)
    //    {
    //        return "com.android.externalstorage.documents".Equals(uri.Authority, StringComparison.InvariantCultureIgnoreCase);
    //    }

    //    /**
    //     * @param uri The Uri to check.
    //     * @return Whether the Uri authority is DownloadsProvider.
    //     */
    //    public static bool IsDownloadsDocument(Uri uri)
    //    {
    //        return "com.android.providers.downloads.documents".Equals(uri.Authority, StringComparison.InvariantCultureIgnoreCase);
    //    }

    //    /**
    //     * @param uri The Uri to check.
    //     * @return Whether the Uri authority is MediaProvider.
    //     */
    //    public static bool IsMediaDocument(Uri uri)
    //    {
    //        return "com.android.providers.media.documents".Equals(uri.Authority, StringComparison.InvariantCultureIgnoreCase);
    //    }

    //    /**
    //     * @param uri The Uri to check.
    //     * @return Whether the Uri authority is Google Photos.
    //     */
    //    public static bool IsGooglePhotosUri(Uri uri)
    //    {
    //        return "com.google.android.apps.photos.content".Equals(uri.Authority, StringComparison.InvariantCultureIgnoreCase);
    //    }
    //}
}

