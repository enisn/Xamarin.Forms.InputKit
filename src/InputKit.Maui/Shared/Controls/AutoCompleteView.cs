﻿using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace InputKit.Shared.Controls;

internal class AutoCompleteView : Entry
{
    private static readonly Func<string, ICollection<string>, ICollection<string>> _defaultSortingAlgorithm = (t, d) => d;

    public static readonly BindableProperty SortingAlgorithmProperty = BindableProperty.Create(nameof(SortingAlgorithm),
        typeof(Func<string, ICollection<string>, ICollection<string>>),
        typeof(AutoCompleteView),
        _defaultSortingAlgorithm);

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
        typeof(IEnumerable<string>),
        typeof(AutoCompleteView),
        default(IEnumerable<string>), propertyChanged: OnItemsSourcePropertyChangedInternal);

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
        typeof(object),
        typeof(AutoCompleteView),
        default,
        BindingMode.OneWayToSource);

    public static readonly BindableProperty ThresholdProperty = BindableProperty.Create(nameof(Threshold),
        typeof(int),
        typeof(AutoCompleteView),
        2);

    internal AutoCompleteView()
    {
        // Keep the ctor for linker.
    }

    /// <summary>
    ///     Sorting Algorithm for the drop down list. This is a bindable property.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// SortingAlgorithm = (text, values) => values
    ///     .Where(t => t.ToLower().StartsWith(text.ToLower()))
    ///     .OrderBy(x => x)
    ///     .ToList();
    /// ]]>
    /// </example>
    public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm
    {
        get => (Func<string, ICollection<string>, ICollection<string>>)GetValue(SortingAlgorithmProperty);
        set => SetValue(SortingAlgorithmProperty, value);
    }

    /// <summary>
    ///     The number of characters the user must type before the dropdown is shown. This is a bindable property.
    /// </summary>
    public int Threshold
    {
        get => (int)GetValue(ThresholdProperty);
        set => SetValue(ThresholdProperty, value);
    }

    /// <summary>
    ///     Item selected from the DropDown List. This is a bindable property.
    /// </summary>
    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    ///     Drop Down List Items Source. This is a bindable property.
    /// </summary>
    public IEnumerable<string> ItemsSource
    {
        get => (IEnumerable<string>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

    internal void OnItemSelectedInternal(object sender, SelectedItemChangedEventArgs args)
    {
        SelectedItem = args.SelectedItem;
        ItemSelected?.Invoke(sender, args);
        OnItemSelected(args);
    }

    private static void OnItemsSourcePropertyChangedInternal(BindableObject bindable, object oldvalue, object newvalue)
    {
        var combo = (AutoCompleteView)bindable;
        var observableOld = oldvalue as INotifyCollectionChanged;
        var observableNew = newvalue as INotifyCollectionChanged;
        combo.OnItemsSourcePropertyChanged(combo, oldvalue, newvalue);

        if (observableOld != null)
        {
            observableOld.CollectionChanged -= combo.OnCollectionChangedInternal;
        }

        if (observableNew != null)
        {
            observableNew.CollectionChanged += combo.OnCollectionChangedInternal;
        }
    }

    public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanged;

    private void OnCollectionChangedInternal(object sender, NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(sender, args);
    }
    public virtual void RaiseTextChanged(string text)
    {
        Text = text;
    }
    protected virtual void OnItemsSourcePropertyChanged(AutoCompleteView bindable, object oldvalue, object newvalue) { }
    protected virtual void OnItemSelected(SelectedItemChangedEventArgs args) { }
}
