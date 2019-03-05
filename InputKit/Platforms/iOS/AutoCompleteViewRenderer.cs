using CoreGraphics;
using Plugin.InputKit.Platforms.iOS;
using Plugin.InputKit.Platforms.iOS.Controls;
using Plugin.InputKit.Platforms.iOS.Helpers;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AutoCompleteView), typeof(AutoCompleteViewRenderer))]
namespace Plugin.InputKit.Platforms.iOS
{
    public class AutoCompleteViewRenderer : ViewRenderer<AutoCompleteView, UITextField>
    {
        private MbAutoCompleteTextField NativeControl => (MbAutoCompleteTextField)Control;
        private AutoCompleteView AutoCompleteEntry => (AutoCompleteView)Element;

        public AutoCompleteViewRenderer()
        {
            // ReSharper disable once VirtualMemberCallInContructor
            Frame = new RectangleF(0, 20, 320, 40);
        }

        protected override UITextField CreateNativeControl()
        {
            var element = (AutoCompleteView)Element;
            var view = new MbAutoCompleteTextField
            {
                AutoCompleteViewSource = new MbAutoCompleteDefaultDataSource(),
                SortingAlgorithm = element.SortingAlgorithm
            };
            view.AutoCompleteViewSource.Selected += AutoCompleteViewSourceOnSelected;
            return view;
        }
        public override void Draw(CGRect rect)
        {
            //base.Draw(rect);
            Debug.WriteLine($"*****************************");
            //Debug.WriteLine($"rect: X: {rect.X} | Y: {rect.Y}");
            //Debug.WriteLine($"Control.Frame: X: {Control.Frame.X} | Y: {Control.Frame.Y} | Bottom: {Control.Frame.Bottom}");
            //Debug.WriteLine($"Control.Bounds: X: {Control.Bounds.X} | Y: {Control.Bounds.Y} | Bottom: {Control.Bounds.Bottom}");
            //Debug.WriteLine($"Control.Superview.Frame: X: {Control.Superview.Frame.X} | Y: {Control.Superview.Frame.Y} | Bottom: {Control.Superview.Frame.Bottom}");
            //Debug.WriteLine($"Control.Superview.Bounds: X: {Control.Superview.Bounds.X} | Y: {Control.Superview.Bounds.Y} | Bottom: {Control.Superview.Bounds.Bottom}");
            //Debug.WriteLine($"Control.Superview.Superview.Frame: X: {Control.Superview.Superview?.Frame.X} | Y: {Control.Superview.Superview?.Frame.Y} | Bottom: {Control.Superview.Superview?.Frame.Bottom} | Top: {Control.Superview.Superview?.Frame.Bottom}");
            //Debug.WriteLine($"this.Frame: X: {this.Frame.X} | Y: {this.Frame.Y} | Bottom: {this.Frame.Bottom}");
            Debug.WriteLine($"NativeView.Frame: X: {NativeView.Frame.X} | Y: {this.NativeView.Frame.Y} | Bottom: {this.NativeView.Frame.Bottom}");
            Debug.WriteLine($"NativeView.Frame.Location: X: {NativeView.Frame.Location.X} | Y: {this.NativeView.Frame.Location.Y}");
            Debug.WriteLine($"NativeView.Bounds: X: {NativeView.Bounds.X} | Y: {this.NativeView.Bounds.Y} | Bottom: {this.NativeView.Bounds.Bottom}");
            Debug.WriteLine($"NativeView.Bounds.Location: X: {NativeView.Bounds.Location.X} | Y: {this.NativeView.Bounds.Location.Y}");
            //var gPoint = NativeView.Superview.ConvertRectFromCoordinateSpace(NativeView.Frame.Location,);
            //Debug.WriteLine($"gPoint.Bounds: X: {gPoint.X} | Y: {gPoint.Y} | Bottom: {gPoint.Bottom}");
            

            var scrollView = GetParentScrollView(Control);
            var ctrl = UIApplication.SharedApplication.GetTopViewController();


            NativeControl.Draw(ctrl, Layer, scrollView, Frame.X + Frame.Bottom);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AutoCompleteView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                // unsubscribe
                NativeControl.AutoCompleteViewSource.Selected -= AutoCompleteViewSourceOnSelected;
                var elm = (AutoCompleteView)e.OldElement;
                elm.CollectionChanged -= ItemsSourceCollectionChanged;
            }

            if (e.NewElement != null)
            {
                SetNativeControl(CreateNativeControl());
                SetItemsSource();
                SetThreshold();
                KillPassword();
                NativeControl.EditingChanged += (s, args) => Element.RaiseTextChanged(NativeControl.Text);
                var elm = (AutoCompleteView)e.NewElement;
                elm.CollectionChanged += ItemsSourceCollectionChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
                KillPassword();
            if (e.PropertyName == AutoCompleteView.ItemsSourceProperty.PropertyName)
                SetItemsSource();
            else if (e.PropertyName == AutoCompleteView.ThresholdProperty.PropertyName)
                SetThreshold();
        }

        private void SetThreshold()
        {
            NativeControl.Threshold = AutoCompleteEntry.Threshold;
        }

        private void SetItemsSource()
        {
            if (AutoCompleteEntry.ItemsSource != null)
            {
                var items = AutoCompleteEntry.ItemsSource.ToList();
                NativeControl.UpdateItems(items);
            }
        }

        private void KillPassword()
        {
            if (Element.IsPassword)
                throw new NotImplementedException("Cannot set IsPassword on a AutoCompleteEntry");
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            SetItemsSource();
        }

        private static UIScrollView GetParentScrollView(UIView element)
        {
            if (element.Superview == null) return null;
            var scrollView = element.Superview as UIScrollView;
            return scrollView ?? GetParentScrollView(element.Superview);
        }

        private void AutoCompleteViewSourceOnSelected(object sender, SelectedItemChangedEventArgs args)
        {
            AutoCompleteEntry.OnItemSelectedInternal(Element, args);
        }
    }
}
