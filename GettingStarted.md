# Getting Started with InputKit
Importing and using **InputKit** is easy as possible. It's plug & play logic. Just add to your project and use it. 
> No more Init(); methods for your each platform !

<hr />

# Adding to your project
This plugin published on [NuGet](https://www.nuget.org/) to much more quicker implementation.  This instructions uses nuget option.

- **Right Click** to your solution and go **Manage NuGet Packages**.
- Type to searchbar **Xamarin.Forms.InputKit** and find that package.
- Choose all platforms you want to use and click the **Install** button.

### Android
If you use it on Android platform there is just one more step.

- **Right Click** to your solution and go **Manage NuGet Packages**.
- Find and Install [Plugin.CurrentActivity](https://github.com/jamesmontemagno/CurrentActivityPlugin) from NuGet to your only Android Project!
- Add following code to **OnCreate** method of your **MainActivity** in Android Project.
```csharp
//...
base.OnCreate(savedInstanceState);
CrossCurrentActivity.Current.Init(this, savedInstanceState); // < ---- Add here
LoadApplication(new App());
//...
```

#### Why Do That on Android ?
Controls in InputKit uses current android activity to make some actions like open dropdown menu, changing color of icons,... etc. 

<hr />

### iOS
Nothing more. Just Plug & Play!

<hr />

### UWP
This platform is Experimental. Some of features may not work! 
It's not recommended to release your projects with InputKit on UWP platform.

But if want a try. No more code needed on UWP too.

<hr>

# Using at your Project
 - Go to your XAML page and add  following xml namespace to your **ContentPage** tag as like [this sample](https://github.com/enisn/Xamarin.Forms.InputKit/blob/f9aba6b7ce104c1466b521a59830d69956e5cc54/Sample.InputKit/Sample.InputKit/MainPage.xaml#L5)
```csharp
xmlns:input="clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit"
```
*[See how it should be done at sample](https://github.com/enisn/Xamarin.Forms.InputKit/blob/f9aba6b7ce104c1466b521a59830d69956e5cc54/Sample.InputKit/Sample.InputKit/MainPage.xaml#L5)*

- Then use it at wherever you want in your page with defined prefix `input` like following code:
```csharp
<input:CheckBox Text="Option 5" IsChecked="True" Type="Material"/>
```