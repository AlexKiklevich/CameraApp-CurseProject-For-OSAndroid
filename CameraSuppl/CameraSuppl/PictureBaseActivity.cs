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
using static Android.Provider.SyncStateContract;
using System.Linq;

namespace CameraSuppl
{
    [Activity(Label = "Pictures")]
    public class PictureBaseActivity : Activity
    {
        public ImageView[] pictures;
        public File[] picturesfiles;
        Bitmap bitmap;
        private int height;
        private int width;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            #region elements
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PictureBase);

            
            GridView picbase = FindViewById<GridView>(Resource.Id.gridview);

            height = Intent.Extras.GetInt("height");
            width = Intent.Extras.GetInt("width");

            #endregion

            picturesfiles = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryPictures).ListFiles();
            File CameraPictures = picturesfiles[0];
            picturesfiles = new File[CameraPictures.ListFiles().Count()];


            LoadPictures(CameraPictures, picbase);
            

        }
        
        private void LoadPictures (File CameraPictures, GridView picbase)
        {
            for (int i = 0; i < CameraPictures.ListFiles().Count(); i++)
            {
                picturesfiles[i] = CameraPictures.ListFiles()[i];
            }
            pictures = new ImageView[picturesfiles.Count()];
            for (int i = 0; i < picturesfiles.Count(); i++)
            {
                bitmap = picturesfiles[i].Path.LoadAndResizeBitmap(width, height);
                pictures[i] = new ImageView(this);
                pictures[i].SetImageBitmap(bitmap);
                bitmap = null;
            }
            picbase.Adapter = new PicturesBaseAdapter(this, pictures);
            picbase.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) 
            {
                Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
            };
            picbase.ItemLongClick += picbaseLongClick;
        }

        private void picbaseLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            
            intent.PutExtra("SelectedImage", picturesfiles[e.Position].Path);
            SetResult(Result.Ok, intent);
            Finish();
            
            
            return;
        }
        public override void OnBackPressed()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            SetResult(Result.Canceled, intent);
            GC.Collect();
            Finish();
        }
        //Bundle extras = new Bundle();
        //extras.PutParcelable("SelectedImage", image);
        
    }
}