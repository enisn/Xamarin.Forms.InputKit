using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Plugin.InputKit.Platforms.iOS
{
    public class CenteredPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.TextAlignment = UITextAlignment.Center;
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "TextAlign")
            {
                switch ((this.Element as CenteredPicker).TextAlingn)
                {
                    case TextAlignment.Start:
                        this.Control.TextAlignment = UITextAlignment.Left;
                        break;
                    case TextAlignment.Center:
                        this.Control.TextAlignment = UITextAlignment.Center;
                        break;
                    case TextAlignment.End:
                        this.Control.TextAlignment = UITextAlignment.Right;
                        break;
                }
            }
        }
    }
}
