﻿using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.InputKit
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            
            //selectionView.ItemsSource = new[]
            //{
            //    new SampleClass{ Name = "Option 1", Id = 1 },
            //    new SampleClass{ Name = "Option 2", Id = 2 },
            //    new SampleClass{ Name = "Option 3", Id = 3 },
            //    new SampleClass{ Name = "Option 4", Id = 4 },
            //    new SampleClass{ Name = "Option 5", Id = 5 },
            //    new SampleClass{ Name = "Option 6", Id = 6 },
            //    new SampleClass{ Name = "Option 7", Id = 7 },
            //    new SampleClass{ Name = "Option 8", Id = 8 },
            //};
        }

        private void AdvancedEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.NewTextValue);
        }
    }
}
