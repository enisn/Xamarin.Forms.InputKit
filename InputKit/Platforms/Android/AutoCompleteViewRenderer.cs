using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.TextField;
using Java.Lang;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Plugin.InputKit.Platforms.Droid;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using AColor = Android.Graphics.Color;
using Color = Microsoft.Maui.Graphics.Color;

[assembly: ExportRenderer(typeof(AutoCompleteView), typeof(AutoCompleteViewRenderer))]
namespace Plugin.InputKit.Platforms.Droid
{
    public class AutoCompleteViewRenderer : Microsoft.Maui.Controls.Compatibility.Platform.Android.AppCompat.ViewRenderer<AutoCompleteView, TextInputLayout>
    {
        public AutoCompleteViewRenderer(Context context) : base(context)
        {
        }
        
        private AppCompatAutoCompleteTextView NativeControl => Control?.EditText as AppCompatAutoCompleteTextView;

        protected override TextInputLayout CreateNativeControl()
        {
            var textInputLayout = new TextInputLayout(Context);
            var autoComplete = new AppCompatAutoCompleteTextView(Context)
            {
                BackgroundTintList = ColorStateList.ValueOf(GetPlaceholderColor()),
                Text = Element?.Text,
                Hint = Element?.Placeholder,
            };

            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(global::Android.Graphics.Color.Transparent);
            autoComplete.SetBackground(gd);
            if (Element != null)
            {
                autoComplete.SetHintTextColor(Element.PlaceholderColor.ToAndroid());
                autoComplete.SetTextColor(Element.TextColor.ToAndroid());
            }
            textInputLayout.AddView(autoComplete);
            return textInputLayout;
        }

        protected override void OnElementChanged(Microsoft.Maui.Controls.Compatibility.Platform.Android.ElementChangedEventArgs<AutoCompleteView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                // unsubscribe
                NativeControl.ItemClick -= AutoCompleteOnItemSelected;
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
                NativeControl.TextChanged += (s, args) => Element.RaiseTextChanged(NativeControl.Text);
                NativeControl.ItemClick += AutoCompleteOnItemSelected;

                var elm = e.NewElement;
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

        private void AutoCompleteOnItemSelected(object sender, AdapterView.ItemClickEventArgs args)
        {
            var view = (AutoCompleteTextView)sender;
            var selectedItemArgs = new SelectedItemChangedEventArgs(view.Text, 0);
            var element = (AutoCompleteView)Element;
            element.OnItemSelectedInternal(Element, selectedItemArgs);
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            var element = (AutoCompleteView)Element;
            ResetAdapter(element);
        }

        private void KillPassword()
        {
            if (Element.IsPassword)
                throw new NotImplementedException("Cannot set IsPassword on a AutoComplete");
        }

        private void ResetAdapter(AutoCompleteView element)
        {
            var adapter = new BoxArrayAdapter(Context,
                Android.Resource.Layout.SimpleDropDownItem1Line,
                element.ItemsSource.ToList(),
                element.SortingAlgorithm);
            NativeControl.Adapter = adapter;

            adapter.NotifyDataSetChanged();
        }

        private void SetItemsSource()
        {
            var element = (AutoCompleteView)Element;
            if (element.ItemsSource == null) return;

            ResetAdapter(element);
        }

        private void SetThreshold()
        {
            var element = (AutoCompleteView)Element;
            NativeControl.Threshold = element.Threshold;
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
