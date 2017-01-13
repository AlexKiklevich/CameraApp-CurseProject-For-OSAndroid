using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace CameraSuppl
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity, View.IOnTouchListener, View.IOnDragListener
    {
        private static  int  count;
        int[] NewA, OldA;
        ImageView[] _cutIV;
        ImageView[] Buf_cutIV;
        private int height;
        private int width;
        Bitmap corefile;
        GridView gridview;
        Button btnResult;
        long id = -1;
        bool timerenabled;
        TextView texttimer;
        Timer  _timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Game);

            btnResult = FindViewById<Button>(Resource.Id.buttonResult);
            btnResult.Click += ClickStart;
            texttimer = FindViewById<TextView>(Resource.Id.textViewTimer);

            string path = Intent.Extras.GetString("ImagePath");
            height = Intent.Extras.GetInt("height");
            width = Intent.Extras.GetInt("width");
            count = Intent.Extras.GetInt("Difficulty");
            timerenabled = Intent.Extras.GetBoolean("Timer");
           
            _cutIV = new ImageView[count*count];
            Buf_cutIV = new ImageView[count * count];
            NewA = new int[count * count];
            OldA = new int[count * count];

            corefile = path.LoadAndResizeBitmap(width, height);

            var bitmaps = BitmapHelper.splitBitmap(corefile, count, count);

            for (int i = 0; i < bitmaps.Length; i++)
            {
                _cutIV[i] = new ImageView(this);
                _cutIV[i].SetImageBitmap(bitmaps[i]);
            }

            for (int i = 0; i < _cutIV.Count(); i++)
            {
                OldA[i] = i;
            }

            NewA = GameService.MixImages(OldA);
            
            for (int i = 0; i < _cutIV.Count(); i++)
            {
                Buf_cutIV[i] = _cutIV[NewA[i]];
            }
        }

        private void ClickStart(object sender, EventArgs e)
        {
            if (timerenabled)
            {
                _timer = new Timer(10000, 1000, texttimer, btnResult);
                _timer.Start();
            }
            else
                texttimer.Text = "";
            
            gridview = FindViewById<GridView>(Resource.Id.GAgridview);
            gridview.SetNumColumns(count);
            gridview.Adapter = new GameImagesAdapter(this, Buf_cutIV);
            gridview.ItemClick += ItemClick;
            gridview.SetOnTouchListener(this);
            btnResult.Text = "Вперед!";
        }

        private void ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            id = e.Position;
            Toast.MakeText(this, e.Position.ToString(), ToastLength.Short).Show();
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                GridView parent = (GridView)v;

                int x = (int)e.GetX();
                int y = (int)e.GetY();

                int position = parent.PointToPosition(x, y);
                if (position > AdapterView.InvalidPosition)
                {
                    int count = parent.ChildCount;
                    for (int i = 0; i < count; i++)
                    {
                        View currentview = parent.GetChildAt(i);
                        currentview.SetOnDragListener(this);
                        int relativeposition = position - parent.FirstVisiblePosition;

                        View target = (View)parent.GetChildAt(relativeposition);

                        ImageView holder = (ImageView)target;
                        ImageView currentimage = holder;
                        target.StartDrag(null, new View.DragShadowBuilder(target), target, 0);
                    }
                }
            }
            return false;
        }

        public bool OnDrag(View v, DragEvent e)
        {
            bool result = true;
            switch(e.Action)
            {
                case DragAction.Started:
                    break;
                case DragAction.Location:
                    break;
                case DragAction.Entered:
                    break;
                case DragAction.Exited:
                    break;
                case DragAction.Drop:
                    if (e.LocalState == v)
                    {
                        result = false;
                    }
                    else
                    {
                        
                        View droped = (View)e.LocalState;
                        ImageView dropedImage = (ImageView)droped;
                        
                        if ((GridView)droped.Parent != gridview)
                        {

                        }
                        View target = v;
                        ImageView targetitem = (ImageView)target;
                        int index = 0;
                        int firstpos = 0;
                        for (int i = 0; i < Buf_cutIV.Count(); i++)
                        {
                            if (dropedImage == Buf_cutIV[i])
                            {
                                firstpos = i;
                                continue;
                            }
                            if (targetitem == Buf_cutIV[i])
                            {
                                index = i;
                                continue;
                            }
                        }

                        ImageView buf = dropedImage;
                        Buf_cutIV[firstpos] = targetitem;
                        Buf_cutIV[index] = buf;

                        int bufIndex = NewA[firstpos];
                        NewA[firstpos] = NewA[index];
                        NewA[index] = bufIndex;

                        gridview.Adapter = new GameImagesAdapter(this, Buf_cutIV);

                        if (GameService.WinAnalize(NewA))
                        {
                            _timer.Cancel();
                            btnResult.SetTextColor(Color.Green);
                            btnResult.Clickable = false;
                            btnResult.Text = "YOU WIN!";
                        }
                        

                    }
                    break;
                case DragAction.Ended:
                    break;
                default:
                    result = false;
                    break;
               
            }
            return result;
        }

    }
}