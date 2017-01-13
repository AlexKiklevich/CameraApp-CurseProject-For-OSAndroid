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
    public class Cell
    {
        public int Id;
        public ImageView Picture;
    }

    public  class GameField
    {
        public Cell[] Cells { get; set; }
        public List<int> CorrectOrder { get; set; }
        public GameField(ImageView[] images)
        {
            Cells = new Cell[images.Count()];
            CorrectOrder = new List<int>();

            foreach (var i in images)
            {
                Cells[CorrectOrder.Count] = new Cell() { Picture = i, Id = Cells.Count() - 1 };
                CorrectOrder.Add(Cells.Count() - 1);
            }
        }
        public bool IsCorrectOrder (Cell [] cells)
        {
            int i = 0;
            foreach ( var c in cells)
            {
                if (!(i == c.Id))
                {
                    return false;
                }
                i++;
            }
            return true;
        }
        public  void Swap(ImageView iv1, ImageView iv2)
        {
            GameService.SwapPictures(iv1, iv2);

            //swap in array
            var buf = Cells[Cells.FirstOrDefault(x => x.Picture == iv1).Id];
            Cells[Cells.FirstOrDefault(x => x.Picture == iv1).Id] = Cells[Cells.FirstOrDefault(x => x.Picture == iv2).Id];
            Cells[Cells.FirstOrDefault(x => x.Picture == iv2).Id] = buf;
        }
    }

}