﻿using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using SandboxMAUI.Pages;

namespace SandboxMAUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        async void GoToCheckBoxPage(System.Object sender, System.EventArgs e)
        {
			await Navigation.PushAsync(new CheckBoxPage());
        }
    }
}