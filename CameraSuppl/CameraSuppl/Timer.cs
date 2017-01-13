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
using Android.Graphics;

namespace CameraSuppl
{
    public delegate void TickEvent(long millisUntilFinished);
    public delegate void FinishEvent();
    public class Timer : CountDownTimer
    {
        public event TickEvent Tick;
        public event FinishEvent Finish;
        private long totaltime { get; set; }
        private long interval { get; set; }

        TextView textview;
        Button result;

        public Timer(long totaltime, long interval, TextView textview, Button result) : base(totaltime, interval)
        {
            this.textview = textview;
            this.totaltime = totaltime;
            this.interval = interval;
            this.result = result;
        }
        public override void OnFinish()
        {
            textview.Text += "00:00";
            result.SetTextColor(Color.Red);
            result.Text = "YOU LOSE!";
        }

        public override void OnTick(long millisUntilFinished)
        {
            if (millisUntilFinished == 1000)
            {
                result.Text = "YOU LOSE!";
                Finish();
            }
            textview.Text = GameService.TimeParse(millisUntilFinished / interval);
            if (Tick != null)
                Tick(millisUntilFinished);
        }
    }
}