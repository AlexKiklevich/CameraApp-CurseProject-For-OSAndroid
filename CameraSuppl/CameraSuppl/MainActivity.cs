using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.IO;
using Android.Graphics;
using Android.Content.PM;
using System.Collections.Generic;
using Android.Provider;
using Android.Net;
using Android.Graphics.Drawables;
using Android.Media;
using System.Linq;
using static Android.Views.View;
using System.Collections;

namespace CameraSuppl
{ 
    public static class Core
    {
        public static File file;
        public static File dir;
        public static Bitmap bitmap;
    } 

    [Activity(Label = "CameraSuppl", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity , View.IOnTouchListener
    {
        private static int size = 3;
        static int matrix = size * size;
        
        private ImageView[] _cutIV = new ImageView[matrix];
        ImageView FotoSector;

        string path;

        Button StartButton;
        RadioButton rb2;
        RadioButton rb3;
        RadioButton rb4;
        RadioGroup rg;
        CheckBox cbTimer;
        int rbID = 2;
        

        protected override void OnCreate(Bundle bundle)
        {
            #region elements
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CameraMain);

            if (CanUseFotoIntent())
            {
                CreatePicturesDirectory();

                StartButton = FindViewById<Button>(Resource.Id.BStart);
                Button OpenCameraButton = FindViewById<Button>(Resource.Id.BOpenCamera);
                Button SelectPictureButton = FindViewById<Button>(Resource.Id.BSelectPicture);
                Button AboutGameButton = FindViewById<Button>(Resource.Id.AboutGameButton);
                cbTimer = FindViewById<CheckBox>(Resource.Id.checkBoxTimer);
                rb2 = FindViewById<RadioButton>(Resource.Id.rb2);
                rb3 = FindViewById<RadioButton>(Resource.Id.rb3);
                rb4 = FindViewById<RadioButton>(Resource.Id.rb4);
                rg = FindViewById<RadioGroup>(Resource.Id.radioGroupDificulty);
                FotoSector = FindViewById<ImageView>(Resource.Id.IVFotoSector);
                
                rg.Enabled = false;
                rb2.Click += rb2Click;
                rb3.Click += rb3Click;
                rb4.Click += rb4Click;

                OpenCameraButton.Click += CreatePicture;
                SelectPictureButton.Click += TakePictureFrom;
                StartButton.Click += StartGame;
                AboutGameButton.Click += AboutGame;
            }
            #endregion
        }

        private void AboutGame(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AboutGameActivity));
            StartActivity(intent);
        }

        private void rb4Click(object sender, EventArgs e)
        {
            rbID = Convert.ToInt32(rb4.Text);
        }

        private void rb3Click(object sender, EventArgs e)
        {
            rbID = Convert.ToInt32(rb3.Text);
        }

        private void rb2Click(object sender, EventArgs e)
        {
            rbID = Convert.ToInt32(rb2.Text);
        }

        private bool CanUseFotoIntent()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableactivities = 
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableactivities != null && availableactivities.Count > 0;
        }
         
        private void CreatePicturesDirectory()
        {
            Core.dir = new File
                (Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures)
                ,"CameraPictures");
            if (!Core.dir.Exists())
            {
                Core.dir.Mkdirs();
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            
            Intent intent = new Intent(this, typeof(GameActivity));
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = FotoSector.Height;
            intent.PutExtra("width", width);
            intent.PutExtra("height", height);
            if (Core.file == null)
            {
                intent.PutExtra("ImagePath", path);
            }
            else
               intent.PutExtra("ImagePath", Core.file.Path);
            int i = rbID;
            intent.PutExtra("Difficulty", i);
            intent.PutExtra("Timer", cbTimer.Checked);
            StartActivity(intent);
        }

        

        private void CreatePicture(object sender, EventArgs e)
        {
            Intent camera = new Intent(MediaStore.ActionImageCapture);
            Core.file = new File(Core.dir, String.Format("picture_{0}.jpg", Guid.NewGuid()));
            camera.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(Core.file));
            StartActivityForResult(camera, 0);
            
        }

        private void TakePictureFrom(object sender, EventArgs e)
        {
            File CameraPictures = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryPictures).ListFiles()[0];
            if (CameraPictures.ListFiles().Count() == 0)
            {
                Toast.MakeText(this, "Папка пуста", ToastLength.Short).Show();
                return;
            }
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = FotoSector.Height;
            Intent intent = new Intent(this, typeof(PictureBaseActivity));
 
            intent.PutExtra("width", width);
            intent.PutExtra("height", height);
            StartActivityForResult(intent,0);
            

        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = FotoSector.Height;
            if (data != null)
            {
                if (data.Extras != null)
                {
                    if (data.Extras.GetString("SelectedImage") != null)
                    {
                        path = data.Extras.GetString("SelectedImage");
                        Core.bitmap = path.LoadAndResizeBitmap(width, height);
                        if (Core.bitmap != null)
                        {
                            FotoSector.SetImageBitmap(Core.bitmap);
                            Core.bitmap = null;
                            rb2.Enabled = true;
                            rb3.Enabled = true;
                            rb4.Enabled = true;
                            StartButton.Enabled = true;
                        }
                    }
                }
                return;
            }
            
            
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(Core.file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);
            
            Core.bitmap = Core.file.Path.LoadAndResizeBitmap(width, height);
            if (Core.bitmap != null)
            {
                FotoSector.SetImageBitmap(Core.bitmap);
                Core.bitmap = null;
                rb2.Enabled = true;
                rb3.Enabled = true;
                rb4.Enabled = true;
                StartButton.Enabled = true;
            }
        }
        public override void OnBackPressed()
        {
            GC.Collect();
            System.Environment.Exit(0);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            throw new NotImplementedException();
        }
    }
}

