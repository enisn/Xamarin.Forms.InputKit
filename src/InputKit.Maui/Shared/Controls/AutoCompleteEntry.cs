﻿using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InputKit.Shared.Controls;

internal class AutoCompleteEntry : AdvancedEntry
{
    private AutoCompleteView txtInput;

    internal AutoCompleteEntry()
    {
        // Keep the ctor for linker.
    }

    public IEnumerable<string> ItemsSource { get => txtInput.ItemsSource; set => txtInput.ItemsSource = value; }

    #region BindableProperties
    public static BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(AutoCompleteView.ItemsSource),
            typeof(IEnumerable<string>),
            typeof(AutoCompleteEntry),
            propertyChanged: (bo, nv, ov) => (bo as AutoCompleteEntry).txtInput.ItemsSource = nv as IEnumerable<string>);
    #endregion

    private protected override Entry GetInputEntry()
    {
        txtInput = new AutoCompleteView
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.CenterAndExpand,
        };
        txtInput.SortingAlgorithm = (text, options) =>
            options
            .Where(x => x.ToLowerInvariant().Contains(text.ToLowerInvariant()))
            .OrderBy(o => o.StartsWith(text, StringComparison.CurrentCultureIgnoreCase) ? 0 : 1)
            .ThenBy(t => t)
            .ToList();
        return txtInput;
    }
}
