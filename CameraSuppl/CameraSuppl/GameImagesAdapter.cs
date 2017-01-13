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
    public class GameImagesAdapter : BaseAdapter
    {
        private Context context;
        private ImageView[] images; 

        public GameImagesAdapter (Context c, ImageView[] images)
        {
            this.context = c;
            this.images = new ImageView[images.Count()];
            for (int i = 0; i < images.Count(); i++)
            {
                this.images[i] = images[i];
            }
        }

        public override int Count
        {
            get
            {
                return images.Count();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return images[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;

            if (convertView == null)
            {
                // if it's not recycled, initialize some attributes
                imageView = new ImageView(context);
                imageView.LayoutParameters = new AbsListView.LayoutParams(90, 90);
                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imageView.SetPadding(8, 8, 8, 8);
            }
            else
            {
                imageView = (ImageView)convertView;
            }
            imageView = (images[position]);

            return imageView;
        }
    }
}