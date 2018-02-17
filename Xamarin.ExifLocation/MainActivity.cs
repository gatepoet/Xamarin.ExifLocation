﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android;
using Android.Content.PM;
using Android.Util;
using Android.Gms.Location;
using System.Threading.Tasks;
using Android.Locations;
using Android.Provider;
using Java.IO;
using System.Linq;
using Android.Media;
using Android.Net;

namespace Xamarin.ExifLocation
{
    [Activity(Label = "Xamarin.ExifLocation", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private const int SelectImageRequest = 67676;
        private const int GetLocationRequest = 67677;
        private const int WriteExternalStorageRequest = 67678;

        private Location deviceLocation;
        private Uri imageUri;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.get_picture).Click += delegate { SelectImage(); };
            FindViewById<Button>(Resource.Id.set_location).Click += async delegate { await SetLocation(); };

            await GetLocation();
        }

        #region Get location

        private async Task GetLocation()
        {
            if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted)
            {
                var locationClient = LocationServices.GetFusedLocationProviderClient(this);
                deviceLocation = await locationClient.GetLastLocationAsync();
                if (deviceLocation != null)
                {
                    FindViewById<TextView>(Resource.Id.deviceLocationText).Text = deviceLocation.ToFormattedString(this);
                }
                else
                {
                    ShowError("Could not get location");
                }
            }
            else
            {
                RequestPermissions(new[] { Manifest.Permission.AccessFineLocation }, GetLocationRequest);
            }
        }

        public async override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case GetLocationRequest:
                    if (grantResults.All(result => result == Permission.Granted))
                    {
                        await GetLocation();
                    }
                    else
                    {
                        ShowError("Get GPS location permission not granted");
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Get image

        private void SelectImage()
        {
            FindViewById<Button>(Resource.Id.set_location).Visibility = ViewStates.Invisible;
            var intent = new Intent(Intent.ActionGetContent, MediaStore.Images.Media.ExternalContentUri);
            intent.SetType("image/*");
            intent.AddCategory(Intent.CategoryOpenable);

            StartActivityForResult(intent, SelectImageRequest);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            switch (requestCode)
            {
                case SelectImageRequest:
                    if (resultCode == Result.Ok)
                    {
                        ShowImage(data.Data);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ShowImage(Uri data)
        {
            imageUri = data;

            var exif = new ExifInterface(ContentResolver.OpenInputStream(imageUri));
            var location = exif.ReadLocation();
            FindViewById<TextView>(Resource.Id.imageLocationText).Text = location.ToFormattedString(this);

            FindViewById<ImageView>(Resource.Id.selected_image).SetImageURI(imageUri);
            FindViewById<Button>(Resource.Id.set_location).Visibility = ViewStates.Visible;
        }

        #endregion

        #region Set location

        private async Task SetLocation()
        {
            var outfile = new File(GetPicturesDirectory(), $"{System.Guid.NewGuid()}.jpg");
            var uri = Uri.FromFile(outfile);

            await CreateCopy(imageUri, uri);
            var exif = new ExifInterface(outfile.AbsolutePath);
            exif.WriteLocation(deviceLocation);
            MakeAvailableInGallery(uri);

            FindViewById<TextView>(Resource.Id.imageLocationText).Text = $"{deviceLocation.Latitude}, {deviceLocation.Longitude}";
        }

        private async Task CreateCopy(Uri sourceUri, Uri destinationUri)
        {
            using (var source = new FileInputStream(ContentResolver.OpenFileDescriptor(sourceUri, "r").FileDescriptor))
            {
                using (var destination = new FileOutputStream(ContentResolver.OpenFileDescriptor(destinationUri, "w").FileDescriptor))
                {
                    byte[] content = new byte[source.Available()];
                    await source.ReadAsync(content);
                    await destination.WriteAsync(content);
                    destination.Close();
                }
            }
        }

        private void MakeAvailableInGallery(Uri uri)
        {
            var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(uri);

            SendBroadcast(mediaScanIntent);
        }

        private File GetPicturesDirectory()
        {
            var dir = new File(
                Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures),
                PackageName);
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }
            return dir;
        }

        private void ShowError(string message)
        {
            Log.Error(PackageName, message);
            new AlertDialog.Builder(this)
                .SetMessage(message)
                .SetNeutralButton("Ok", delegate { })
                .Show();
        }

        #endregion
    }
}

