using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace CameraSuppl
{
    public class PicturesBaseAdapter : BaseAdapter
    {
        private ImageView[] Pictures;
        private readonly Context context;

        public PicturesBaseAdapter ( Context c, ImageView[] pictures)
        {
            context = c;
            Pictures = new ImageView[pictures.Length];
            for (int i = 0; i < pictures.Length; i++)
            {
                Pictures[i] = pictures[i];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;

            if (convertView == null)
            {
                // if it's not recycled, initialize some attributes
                imageView = new ImageView(context);
                imageView.LayoutParameters = new AbsListView.LayoutParams(20, 20);
                imageView.SetScaleType(ImageView.ScaleType.Center);
                imageView.SetPadding(8, 8, 8, 8);
            }
            else
            {
                imageView = (ImageView)convertView;
            }
            imageView = (Pictures[position]);

            return imageView;
        }

        #region Another
        public override int Count
        {
            get
            {
                return Pictures.Length;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return Pictures[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        #endregion


       
    }
}