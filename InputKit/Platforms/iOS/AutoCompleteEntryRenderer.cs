using CoreGraphics;
using Plugin.InputKit.Platforms.iOS;
using Plugin.InputKit.Platforms.iOS.Controls;
using Plugin.InputKit.Platforms.iOS.Helpers;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AutoCompleteEntry), typeof(AutoCompleteEntryRenderer))]
namespace Plugin.InputKit.Platforms.iOS
{
    public class AutoCompleteEntryRenderer : ViewRenderer<AutoCompleteEntry, UITextField>
    {
        private MbAutoCompleteTextField NativeControl => (MbAutoCompleteTextField)Control;
        private AutoCompleteEntry AutoCompleteEntry => (AutoCompleteEntry)Element;

        public AutoCompleteEntryRenderer()
        {
            // ReSharper disable once VirtualMemberCallInContructor
            Frame = new RectangleF(0, 20, 320, 40);
        }

        protected override UITextField CreateNativeControl()
        {
            var element = (AutoCompleteEntry)Element;
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
            base.Draw(rect);
            var scrollView = GetParentScrollView(Control);
            var ctrl = UIApplication.SharedApplication.GetTopViewController();
            NativeControl.Draw(ctrl, Layer, scrollView);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AutoCompleteEntry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                // unsubscribe
                NativeControl.AutoCompleteViewSource.Selected -= AutoCompleteViewSourceOnSelected;
                var elm = (AutoCompleteEntry)e.OldElement;
                elm.CollectionChanged -= ItemsSourceCollectionChanged;
            }

            if (e.NewElement != null)
            {
                SetNativeControl(CreateNativeControl());
                SetItemsSource();
                SetThreshold();
                KillPassword();

                var elm = (AutoCompleteEntry)e.NewElement;
                elm.CollectionChanged += ItemsSourceCollectionChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
                KillPassword();
            if (e.PropertyName == AutoCompleteEntry.ItemsSourceProperty.PropertyName)
                SetItemsSource();
            else if (e.PropertyName == AutoCompleteEntry.ThresholdProperty.PropertyName)
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
            // TODO : Florell, Chase (Contractor) 02/15/17 SET FOCUS
        }
    }
}
