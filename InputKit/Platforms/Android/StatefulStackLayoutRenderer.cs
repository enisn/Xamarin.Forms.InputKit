using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Plugin.InputKit.Platforms.Droid;
using Plugin.InputKit.Shared.Controls;
using Plugin.InputKit.Shared.Layouts;

[assembly:ExportRenderer(typeof(StatefulStackLayout),typeof(StatefulStackLayoutRenderer))]
namespace Plugin.InputKit.Platforms.Droid
{
    public class StatefulStackLayoutRenderer : VisualElementRenderer<StackLayout>, Android.Views.View.IOnTouchListener
    {
        public StatefulStackLayoutRenderer(Context context) : base(context)
        {

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            //System.Diagnostics.Debug.WriteLine("[OnTouchEvent] - " + e.Action);
            if (e.Action == MotionEventActions.Down)
            {
                VisualStateManager.GoToState(Element, "Pressed");
            }
            else if (e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Cancel)
            {
                VisualStateManager.GoToState(Element, "Normal");
            }
            return base.OnTouchEvent(e);
        }

        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
            return base.OnTouchEvent(e);
        }
    }
}
