using Plugin.InputKit.Shared.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Plugin.InputKit.Shared.Controls
{
    public class SelectionView : Grid
    {
        private IList _itemSource;
        private SelectionType _selectionType = SelectionType.Button;
        private IList _disabledSource;
        private int _columnNumber = 2;

        public SelectionView()
        {
            this.RowSpacing = 0;
            this.ColumnSpacing = 0;
            //this.ChildAdded += SelectionView_ChildAdded;
            //this.ChildRemoved += SelectionView_ChildRemoved;
        }

        //private void SelectionView_ChildRemoved(object sender, ElementEventArgs e)
        //{
        //    UpdateView();
        //}

  

        /// <summary>
        /// Selection Type, More types will be added later
        /// </summary>
        public SelectionType SelectionType { get => _selectionType; set { _selectionType = value; UpdateView(); } }
        ///----------------------------------------------------------
        /// <summary>
        /// Added later
        /// </summary>
        public string IsDisabledPropertyName { get; set; }
        ///----------------------------------------------------------
        /// <summary>
        /// Column of this view
        /// </summary>
        public int ColumnNumber { get => _columnNumber; set { _columnNumber = value; UpdateView(); } }
        public IList DisabledSource { get => _disabledSource; set { _disabledSource = value; UpdateView(); } }
        public IList ItemSource
        {
            get => _itemSource;
            set
            {
                _itemSource = value;
                UpdateEvents(value);
                UpdateView();
            }
        }
        public object SelectedItem
        {
            get
            {

                foreach (var item in this.Children)
                    if (item is ISelection && (item as ISelection).IsSelected)
                        return (item as ISelection).Value;
                return null;
            }
            set
            {

                foreach (var item in this.Children)
                    if (item is ISelection && !(item as ISelection).IsDisabled)
                        (item as ISelection).IsSelected = (item as ISelection).Value == value;
            }
        }
        private void UpdateEvents(IList value)
        {
            if (value is INotifyCollectionChanged)
            {
                (value as INotifyCollectionChanged).CollectionChanged -= MultiSelectionView_CollectionChanged;
                (value as INotifyCollectionChanged).CollectionChanged += MultiSelectionView_CollectionChanged;
            }
        }
        private void MultiSelectionView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateView();
        }
        private void UpdateView()
        {
            try
            {
                this.Children.Clear();
                SetValue(SelectedItemProperty, null);
                foreach (var item in ItemSource)
                {
                    var _View = GetInstance(item);
                    (_View as ISelection).Clicked -= Btn_Clicked;
                    (_View as ISelection).Clicked += Btn_Clicked;

                    if (!String.IsNullOrEmpty(IsDisabledPropertyName)) //Sets if property Disabled
                        (_View as ISelection).IsDisabled = Convert.ToBoolean(item.GetType().GetProperty(IsDisabledPropertyName)?.GetValue(item) ?? false);
                    if (DisabledSource?.Contains(item) ?? false)
                        (_View as ISelection).IsDisabled = true;
                    

                    this.Children.Add(_View, this.Children.Count % ColumnNumber, this.Children.Count / ColumnNumber);


                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            SelectedItem = (sender as ISelection).Value;
            SetValue(SelectedItemProperty, SelectedItem);
        }

        private View GetInstance(object obj)
        {
            switch (SelectionType)
            {
                case SelectionType.Button:
                    return new SelectableButton(obj);
                case SelectionType.RadioButton:
                    return new SelectableRadioButton(obj);
     
            }
            return null;
        }


        #region BindableProperties
        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource), typeof(IList), typeof(SelectionView), null, propertyChanged: (bo, ov, nv) => (bo as SelectionView).ItemSource = (IList)nv);
        public static readonly BindableProperty DisabledSourceProperty = BindableProperty.Create(nameof(DisabledSource), typeof(IList), typeof(SelectionView), null, propertyChanged: (bo, ov, nv) => (bo as SelectionView).DisabledSource = (IList)nv);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SelectionView), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as SelectionView).SelectedItem = nv);
        #endregion
    }

    public enum SelectionType
    {
        Button,
        RadioButton
    }




    public class SelectableButton : Button, ISelection
    {
        private bool _isSelected = false;
        private object _value;

        public SelectableButton()
        {
            this.BorderRadius = 20;
            this.Margin = new Thickness(20, 5);
            UpdateColors();
        }
        public SelectableButton(object value) : this()
        {
            this.Value = value;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; UpdateColors(); }
        }

        private void UpdateColors()
        {
            if (IsSelected)
            {
                this.BackgroundColor = Color.Accent;
                this.TextColor = Color.WhiteSmoke;
            }
            else
            {
                this.BackgroundColor = (Color)Button.BackgroundColorProperty.DefaultValue;
                this.TextColor =(Color) Button.TextColorProperty.DefaultValue;
            }
        }
        public object Value { get => _value; set { _value = value; this.Text = value?.ToString(); } }
        public bool IsDisabled { get; set; } = false;
    }
    public class SelectableRadioButton : RadioButton, ISelection
    {
        private bool _isDisabled;
        public SelectableRadioButton()
        {

        }
        public SelectableRadioButton(object value)
        {
            this.Value = value;
            this.Text = value?.ToString();
        }
        public bool IsSelected { get => this.IsChecked; set => this.IsChecked = value; }
        public bool IsDisabled { get => _isDisabled; set { _isDisabled = value; this.Opacity = value ? 0.6 : 1; } }
    }
}
