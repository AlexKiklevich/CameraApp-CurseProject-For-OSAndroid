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

namespace CameraSuppl
{
    class GameService
    {
        public static void SwapPictures ( ImageView iv1, ImageView iv2)
        {
            var getX = iv1.GetX();
            var getY = iv1.GetY();

            iv2.Layout((int)getX, (int)getY, (int)getX + iv2.Width, (int)getY + iv2.Height);
            iv1.Layout((int)iv2.GetX(), (int)iv2.GetY(), (int)iv2.GetX() + iv1.Width, (int)iv2.GetY() + iv1.Height);
        }
        public static int[] MixImages(int[] indexes)
        {
            var random = new Random();
            int[] buf = new int[indexes.Count()];
            buf = indexes.OrderBy(x => random.Next()).ToArray();
            return buf;
        }

        public static bool WinAnalize(int[] i1)
        {
            for (int i = 0; i < i1.Count(); i++)
            {
                if (i1[i] != i)
                {
                    return false;
                }
            }
            return true;
        }
        public static string TimeParse(double time)
        {
            string result = "         ";
            if ((time / 60) / 10 >= 1)
            {
                result += (time / 60).ToString() + ":";
            }
            else
            {
                if ((time / 60) >= 1)
                {
                    result += "0" + (time / 60).ToString() + ":";
                }
                else result += "00" + ":";

            }
            if ((time % 60) / 10 >= 1)
            {
                result += (time % 60);
            }
            else
            {
                if ((time % 60) >= 1)
                {
                    result += "0" + (time % 60);
                }
                else result += "00";

            }

            return result;
        } 
    }
}