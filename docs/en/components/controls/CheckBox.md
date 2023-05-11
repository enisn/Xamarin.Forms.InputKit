# InputKit: CheckBox

A checkbox control that is useful, customizable, full-featured, fully-bindable and easy to use.



| Dark - Desktop | Light - Mobile |
| --- | --- |
| <img src="../../images/checkbox-dark-windows.gif" height="480" /> | <img src="../../images/checkbox-light-android.gif" height="480"/> |


> _You can visit entire code of this sample from [here](https://github.com/enisn/Xamarin.Forms.InputKit/tree/develop/sandbox/SandboxMAUI/Pages/CheckBoxPage.xaml)_


## Supported Platforms

| - | MAUI | Xamarin Forms |
| :--- | :---: | :---: |
| Windows | ✅ | v4.0+ |
| macOS | ✅ | ✅ |
| Linux | ❌ | ❌ |
| Android | ✅ | ✅ |
| iOS | ✅ | ✅ |



## Usage

Before starting to use. Make sure you're not using MAUI/Xamarin Forms checkbox in your pages.

Make sure you defined InputKit namespace in your XAML file.

| | |
| --- | --- |
| MAUI | `xmlns:input="clr-namespace:InputKit.Shared.Controls;assembly=InputKit.Maui"` |
| Xamarin Forms | `xmlns:input="clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit"` |


Now you're ready to use it in your XAML page.

```xml
<input:CheckBox Text="Hello World">
```

![maui checkbox](../../images/checkbox-usage-01.gif)

### Data Binding

All of the properties of the CheckBox control can be bound to any property of any object that supports data binding.


- You can bind the `IsChecked` property to any property of ViewModel to handle the checkbox state.
```xml
<input:CheckBox Text="Hello World" IsChecked="{Binding IsChecked}">
```

- You can bind the `CheckChangedCommand` property to any command of ViewModel to handle the checkbox changes. This command is executed right after the checkbox is checked or unchecked.
```xml
<input:CheckBox Text="Hello World" CheckChangedCommand="{Binding OnCheckChangedCommand}">
```

> If you're looking for dynamic CheckBox list and want to get only selected item/items after selection. Check the [SelectionView](SelectionView.md)

- Rest of the visual properties are bindable as well.

## Customization

CheckBox allows you to customize its appearance in different ways.

### Type
CheckBox supports three types of appearance:
    - Regular - default type, looks like a checkbox.
    - Filled - looks like filled when selected with Border Color of the control.
    - Material - looks like filled when selected with `Color` property color.

#### Regular Type
This is default value. If you don't specify any type, it will be regular type.

```xml
<input:CheckBox Text="Hello World!" />
<!-- Or you can specify the Type like below -->
<input:CheckBox Text="Hello World!" Type="Regular" />
```

![inputkit maui checkbox regular](../../images/checkbox-customization-types-regular.gif)


#### Filled Type
This type is used when you want to fill the control with Border Color of the control when it's selected.

```xml
<input:CheckBox Text="Hello World!" Type="Filled" />
```

![inputkit maui checkbox regular](../../images/checkbox-customization-types-filled.gif)

#### Material Type
This type is used when you want to fill the control with `Color` property color when it's selected.


```xml
<input:CheckBox Text="Hello World!" Type="Material" />
```

![inputkit maui checkbox regular](../../images/checkbox-customization-types-material.gif)


### Icons

CheckBox icon can be customized in two different ways. You can use predefined shapes or you can use custom shape.
InputKit provides a collection of predefined shapes that can be used as an icon.

> Check all [Predefined Shapes](../../PredefinedShapes.md) for more info.

- Predefined shapes can be used as parameter of `IconGeometry` property.

```xml
<input:CheckBox Text="Option 2 with Square Shape" IconGeometry="{x:Static input:PredefinedShapes.Square}" />
```

![inputkit maui checkbox square](../../images/checkbox-customization-icons-square.gif)


- Custom shape can be used as parameter of `IconGeometry` property. A plain SVG path can be used as an icon.

```xml
<input:CheckBox 
    Text="Option 5 with Material Custom Type (X)" 
    Type="Material" 
    IconGeometry="M17.705 7.705l-1.41-1.41L12 10.59 7.705 6.295l-1.41 1.41L10.59 12l-4.295 4.295 1.41 1.41L12 13.41l4.295 4.295 1.41-1.41L13.41 12l4.295-4.295z"/>
```

![inputkit maui checkbox material custom](../../images/checkbox-customization-icons-custom-x.gif)

### Label Position

CheckBox supports two label positions:
    - Before - label is positioned before the control.
    - After - label is positioned after the control. (default)

```xml
<input:CheckBox Text="Hello World! After" LabelPosition="After" />
<input:CheckBox Text="Hello World! Before" LabelPosition="Before" />
```

![](../../images/checkbox-customization-labelposition.gif)

### Colors

You can customize CheckBox colors by setting `Color`, `BackgroundColor`, `BorderColor`, `BoxBackgroundColor`, `TextColor` and `IconColor` properties.

```xml
    <input:CheckBox 
        Text="Hello World!"
        Color="Blue"
        BorderColor="LightBlue"
        IconColor="DarkBlue"
        BoxBackgroundColor="LightGray"
        TextColor="Blue"
        BackgroundColor="AliceBlue"
        />
```

![inputkit checkbox color customization](../../images/checkbox-customization-colors.gif)

Color property will be applied when you set `Type` to `Material`.

```xml
<input:CheckBox 
    Text="Hello World!"
    Type="Material"
    Color="Blue"
    BorderColor="LightBlue"
    IconColor="DarkBlue"
    BoxBackgroundColor="LightGray"
    TextColor="Blue"
    BackgroundColor="AliceBlue"
    />
```

![inputkit checkbox color customization](../../images/checkbox-customization-colors-material.gif)

## Custom Content
You can use your custom content as CheckBox content. You can use any `View` as content of CheckBox.

```xml
<input:CheckBox>
    <Image Source="xamarin.png" HeightRequest="50" WidthRequest="50" />
</input:CheckBox>

<input:CheckBox>
    <Image Source="dotnet_bot.png" WidthRequest="50" HeightRequest="50" />
</input:CheckBox>
```

![inputkit checkbox custom content](../../images/checkbox-custom-content.gif)

## Behaviors

** *Work in progess...* **:
-  _Actions to override_ 
-  _VisualStates_
-  _Make your own animation (?)_