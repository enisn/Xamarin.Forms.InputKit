# Xamarin.Forms.InputKit
CheckBox, Radio Button, Labeled Slider, Dropdows etc. 

<hr />

[![Build status](https://ci.appveyor.com/api/projects/status/st6lcbts9bkhxqub?svg=true)](https://ci.appveyor.com/project/enisn/xamarin-forms-inputkit) <a href="https://www.nuget.org/packages/Xamarin.Forms.InputKit/"><img src="https://img.shields.io/badge/NuGet-1.0.7-blue.svg" /></a>
<p>
<b>Nuget Package Available: </b> :  <a href="https://www.nuget.org/packages/Xamarin.Forms.InputKit/"><img source="http://enisnecipoglu.com/Plugins/inputkit.png" height="15" />Xamarin.Forms.InputKit on NuGet</a>
</p>
<p>
<b>Sample Project Available: </b> :  <a href="https://github.com/enisn/Xamarin.Forms.InputKit">Sample.InputKit on GitHub</a>
</p>
<p>
<b>Source Codes Available Too: </b> :  <a href="https://github.com/enisn/Xamarin.Forms.InputKit/tree/master/InputKit">Plugin.InputKit on GitHub</a>
</p>


<hr />


<h2>Checkbox</h2>
<p>As you know ther is no CheckBox in Xamarin Forms Library. You can use a custom renderer to use Native Checkbox in portable layer. This CheckBox is not a native one, It's created in Xamarin Forms Portable layer.</p>

<h4>SAMPLE:</h4>

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Sample.InputKit"
             xmlns:input="clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit"
             x:Class="Sample.InputKit.MainPage">
        <StackLayout Spacing="12" Padding="30,0">
            <input:CheckBox Text="Option 1" Type="Box" />
            <input:CheckBox Text="Hello World I'm Option 2" Type="Check"/>
            <input:CheckBox Text="Consetetur eum kasd eos dolore Option 3" Type="Cross"/>
            <input:CheckBox Text="Sea sed justo" Type="Star"/>
        </StackLayout>
</ContentPage>
```
<br />
<img src="http://enisnecipoglu.com/wp-content/uploads/2018/04/XamarinFormsCheckbox-169x300.png" alt="Xamarin Forms CheckBox Input Kit Enis Necipoglu" width="270" height="480" class="aligncenter size-medium wp-image-996" />
<h4>PROPERTIES:</h4>
<ul>
	<li><strong>CheckChanged:</strong> <em>(Event)</em> Invokes when check changed.</li>
        <li><strong>CheckChangedCommand:</strong> <em>(Command)</em> Bindable Command, executed when check changed.</li>
        <li><strong>Key:</strong> <em>(int)</em> A key you can set to define checkboxes as ID.</li>
 <li><strong>Text:</strong> <em>(string)</em> Text to display description</li>
<li><strong>IsChecked:</strong> <em>(bool)</em> Checkbox checked situation. Works TwoWay Binding as default.</li>
<li><strong>Color:</strong> <em>(Color)</em> Color of selected check.</li>
<li><strong>TextColor:</strong> <em>(Color)</em> Color of description text.</li>
<li><strong>Type:</strong> <em>(CheckType)</em> Type of checkbox checked style. (Check,Cross,Star,Box etc.)</li>
</ul>
<hr />


<h2>RadioButon</h2>
<p>Radio Buttons should use inside a <strong>RadioButtonGroupView</strong>. If you want this view will return you selected radio button. But you can handle it one by one too. </p>
<h4>SAMPLE:</h4>

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            xmlns:local="clr-namespace:Sample.InputKit"
            xmlns:input="clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit"
            x:Class="Sample.InputKit.MainPage">

        <StackLayout Spacing="12" Padding="30,0">

            <input:RadioButtonGroupView>
                <input:RadioButton Text="Option 1" />
                <input:RadioButton Text="Option 2" />
                <input:RadioButton Text="Option 3" />
                <input:RadioButton Text="Option 4" />
            </input:RadioButtonGroupView>

        </StackLayout>

</ContentPage>
```

<img src="http://enisnecipoglu.com/wp-content/uploads/2018/04/XamarinFormsRadioButton-169x300.png" alt="Xamarin Forms Radio Button Input Kit Enis Necipoğlu" width="270" height="480" class="aligncenter size-medium wp-image-1001" />
<h4>PROPERTIES:</h4>
<h5>RadioButtonGroupView</h5>
<ul>
<li><strong>SelectedIndex:</strong> <em>(int)</em> Gets or Sets selected radio button inside itself by index</li>
<li><strong>SelectedItem:</strong> <em>(object)</em> Gets or Sets selected radio button inside itself by Value</li>
</ul>
<h5>RadioButton</h5>
<ul>
<li><strong>Clicked:</strong> <em>(event)</em> Invokes when clikced</li>
<li><strong>ClickCommand:</strong> <em>(int)</em> Bindable Command, Executes when clicked</li>
<li><strong>Value:</strong> <em>(object)</em> A value keeps inside and groupview returns that value as SelectedItem</li>
<li><strong>IsChecked:</strong> <em>(bool)</em> Gets or Sets that radio button selected</li>
<li><strong>Text:</strong> <em>(string)</em> Text to display near of Radio Button</li>
<li><strong>FontSize:</strong> <em>(double)</em> Fontsize of Text</li>
<li><strong>Color:</strong> <em>(Color)</em> Color of selected radio button dot</li>
<li><strong>TextColor:</strong> <em>(Color)</em> Color of Text</li>
</ul>
<hr />

<h2>Advanced Slider</h2>
<p>Xamarin Forms Slider works a Sticky label on it. Wonderful experience for your users.</p>
<h4>SAMPLE:</h4>

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Sample.InputKit"
             xmlns:input="clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit"
             x:Class="Sample.InputKit.MainPage">

    <StackLayout Spacing="12" Padding="10,0" VerticalOptions="CenterAndExpand">

        <input:AdvancedSlider MaxValue="5000" MinValue="50" StepValue="50" ValuePrefix="Price:" ValueSuffix="€" Title="Choose Budget:"/>

    </StackLayout>

</ContentPage>
```
<a href="https://media.giphy.com/media/BoIPfRefA0Q9AtJ6mQ/giphy.gif"><img src="https://media.giphy.com/media/BoIPfRefA0Q9AtJ6mQ/giphy.gif" width="270" height="480" alt="Xamarin Forms Slider Sticky Label" class="aligncenter size-medium" /></a>

<h4>PROPERTIES:</h4>
<ul>
<li><strong>Value:</strong> <em>(double)</em> Current Selected Value, (this can be used TwoWayBinding)</li>
<li><strong>Title:</strong> <em>(string)</em> Title of slider</li>
<li><strong>ValueSuffix:</strong> <em>(string)</em> Suffix to be displayed near Value on Floating Label</li>
<li><strong>ValuePrefix:</strong> <em>(string)</em> Prefix to be displayed near Value on Floating Label</li>
<li><strong>MinValue:</strong> <em>(double)</em> Sliders' minimum value</li>
<li><strong>MaxValue:</strong> <em>(double)</em> Sliders' maximum value</li>
<li><strong>MaxValue:</strong> <em>(double)</em> Sliders' increment value</li>
<li><strong>TextColor:</strong> <em>(Color)</em> Color of Texts</li>
<li><strong>DisplayMinMaxValue:</strong> <em>(bool)</em> Visibility of Minimum and Maximum value</li>
</ul>
<hr />

<h2>SelectionView</h2>
<p>Presents options to user to choose. This view didn't created to static usage. You should Bind a model List as ItemSource, or if you don't use MVVM you can set in page's cs file like below. (You can override ToString method to fix display value or I'll add displayMember property soon.)</p>
<h4>SAMPLE:</h4>

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Sample.InputKit"
             xmlns:input="clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit"
             x:Class="Sample.InputKit.MainPage">

    <StackLayout Spacing="12" Padding="10,0" VerticalOptions="CenterAndExpand">

        <input:SelectionView x:Name="selectionView" />

    </StackLayout>
</ContentPage>
```

```csharp
public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            selectionView.ItemSource = new[]
            {
                "Option 1","Option 2","Option 3","Option 4","Option 5","Option 6","Option 7","Option 8"
            };
		}
	}
```
<a href="https://media.giphy.com/media/KXtC6oNnOgnJhvYecy/giphy.gif"><img src="https://media.giphy.com/media/KXtC6oNnOgnJhvYecy/giphy.gif" width="270" height="480" alt="Xamarin Forms SelectionView Enis Necipoglu" class="aligncenter size-medium" /></a>
<h4>PROPERTIES:</h4>
<ul>
<li><strong>ItemSource:</strong> <em>(IList)</em> List of options</li>
<li><strong>SelectedItem:</strong> <em>(object)</em> Selected Item from ItemSource</li>
<li><strong>ColumnNumber:</strong> <em>(int)</em> Number of columng of this view</li>
</ul>


<br />
<hr />
<p>
<b>Nuget Package Available: </b> :  <a href="https://www.nuget.org/packages/Xamarin.Forms.InputKit/"><img source="http://enisnecipoglu.com/Plugins/inputkit.png" height="15" />Xamarin.Forms.InputKit on NuGet</a>
</p>
<p>
<b>Sample Project Available: </b> :  <a href="https://github.com/enisn/Xamarin.Forms.InputKit">Sample.InputKit on GitHub</a>
</p>
<p>
<b>Source Codes Available Too: </b> :  <a href="https://github.com/enisn/Xamarin.Forms.InputKit/tree/master/InputKit">Plugin.InputKit on GitHub</a>
</p>
<hr />

