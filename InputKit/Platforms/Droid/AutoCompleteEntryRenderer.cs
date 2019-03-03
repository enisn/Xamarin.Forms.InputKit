using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;
using Color = Xamarin.Forms.Color;
using AColor = Android.Graphics.Color;
using FormsAppCompat = Xamarin.Forms.Platform.Android.AppCompat;
using Plugin.InputKit.Shared.Controls;
using Plugin.InputKit.Platforms.Droid;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

[assembly: ExportRenderer(typeof(AutoCompleteEntry), typeof(AutoCompleteEntryRenderer))]
namespace Plugin.InputKit.Platforms.Droid
{
    public class AutoCompleteEntryRenderer : FormsAppCompat.ViewRenderer<AutoCompleteEntry, TextInputLayout>
    {
        public AutoCompleteEntryRenderer(Context context) : base(context)
        {
        }

        private AppCompatAutoCompleteTextView AutoComplete => Control?.EditText as AppCompatAutoCompleteTextView;

        protected override TextInputLayout CreateNativeControl()
        {
            var textInputLayout = new TextInputLayout(Context);
            var autoComplete = new AppCompatAutoCompleteTextView(Context)
            {
                BackgroundTintList = ColorStateList.ValueOf(GetPlaceholderColor())
            };
            textInputLayout.AddView(autoComplete);
            return textInputLayout;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AutoCompleteEntry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                // unsubscribe
                AutoComplete.ItemClick -= AutoCompleteOnItemSelected;
                var elm = e.OldElement;
                elm.CollectionChanged -= ItemsSourceCollectionChanged;
            }

            if (e.NewElement != null)
            {
                SetNativeControl(CreateNativeControl());
                // subscribe
                SetItemsSource();
                SetThreshold();
                KillPassword();
                AutoComplete.ItemClick += AutoCompleteOnItemSelected;
                var elm = e.NewElement;
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

        private void AutoCompleteOnItemSelected(object sender, AdapterView.ItemClickEventArgs args)
        {
            var view = (AutoCompleteTextView)sender;
            var selectedItemArgs = new SelectedItemChangedEventArgs(view.Text);
            var element = (AutoCompleteEntry)Element;
            element.OnItemSelectedInternal(Element, selectedItemArgs);
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            var element = (AutoCompleteEntry)Element;
            ResetAdapter(element);
        }

        private void KillPassword()
        {
            if (Element.IsPassword)
                throw new NotImplementedException("Cannot set IsPassword on a AutoComplete");
        }

        private void ResetAdapter(AutoCompleteEntry element)
        {
            var adapter = new BoxArrayAdapter(Context,
                Android.Resource.Layout.SimpleDropDownItem1Line,
                element.ItemsSource.ToList(),
                element.SortingAlgorithm);
            AutoComplete.Adapter = adapter;
            adapter.NotifyDataSetChanged();
        }

        private void SetItemsSource()
        {
            var element = (AutoCompleteEntry)Element;
            if (element.ItemsSource == null) return;

            ResetAdapter(element);
        }

        private void SetThreshold()
        {
            var element = (AutoCompleteEntry)Element;
            AutoComplete.Threshold = element.Threshold;
        }

        #region Section 2
        protected AColor GetPlaceholderColor() => Element.PlaceholderColor.ToAndroid(Color.FromHex("#80000000"));
        #endregion
    }

    internal class BoxArrayAdapter : ArrayAdapter
    {
        private readonly IList<string> _objects;
        private readonly Func<string, ICollection<string>, ICollection<string>> _sortingAlgorithm;

        public BoxArrayAdapter(
            Context context,
            int textViewResourceId,
            List<string> objects,
            Func<string, ICollection<string>, ICollection<string>> sortingAlgorithm) : base(context, textViewResourceId, objects)
        {
            _objects = objects;
            _sortingAlgorithm = sortingAlgorithm;
        }

        public override Filter Filter
        {
            get
            {
                return new CustomFilter(_sortingAlgorithm) { Adapter = this, Originals = _objects };
            }
        }
    }

    internal class CustomFilter : Filter
    {
        private readonly Func<string, ICollection<string>, ICollection<string>> _sortingAlgorithm;

        public CustomFilter(Func<string, ICollection<string>, ICollection<string>> sortingAlgorithm)
        {
            _sortingAlgorithm = sortingAlgorithm;
        }

        public BoxArrayAdapter Adapter { private get; set; }
        public IList<string> Originals { get; set; }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            var results = new FilterResults();
            if (constraint == null || constraint.Length() == 0)
            {
                results.Values = new Java.Util.ArrayList(Originals.ToList());
                results.Count = Originals.Count;
            }
            else
            {
                var values = new Java.Util.ArrayList();
                var sorted = _sortingAlgorithm(constraint.ToString(), Originals).ToList();

                for (var index = 0; index < sorted.Count; index++)
                {
                    var item = sorted[index];
                    values.Add(item);
                }

                results.Values = values;
                results.Count = sorted.Count;
            }

            return results;
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            if (results.Count == 0)
                Adapter.NotifyDataSetInvalidated();
            else
            {
                Adapter.Clear();
                var vals = (Java.Util.ArrayList)results.Values;
                foreach (var val in vals.ToArray())
                {
                    Adapter.Add(val);
                }
                Adapter.NotifyDataSetChanged();
            }
        }
    }
}
