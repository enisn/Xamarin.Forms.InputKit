# Migrating to v4.1
InputKit version 4.1 comes a couple of breaking-changes. Please check following list if you're upgrading to v4.1.

## Breaking changes

This document applies both to MAUI and Xamarin Forms.


### CheckBox

- Material Type no longer sets the border color to same color as the `Color` property. You should set `BorderColor` property to same color as `Color` property if you want to keep the same look.

    ```xml
    <!-- Old -->
    <input:CheckBox Type="Material" Color="{StaticResource Primary}">

    <!-- New -->
    <input:CheckBox Type="Material" Color="{StaticResource Primary}" BorderColor="{StaticResource Primary}">
    ```

- Now CheckBox.CheckChanged sends CheckChangedEventArgs class instead of blank EventArgs class.

    ```csharp
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            checkBox.CheckChanged += CheckBox_CheckChanged;
        }

        // Old
        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            // ...
        }

        // New
        private void CheckBox_CheckChanged(object sender, CheckChangedEventArgs e)
        {
            Console.WriteLine(e.Value); // Value represents the new value of the checkbox.
            // ...
        }

    }
    ```

### AdvancedEntry
- `IconColor` is removed from `AdvancedEntry`.
- `IconImage` property type is changed to `ImageSource` from `string`. _(Not FontImageSource can be passed as parameter.)_

> After this change, AdvancedEntry can be used on Windows platform with all features.

### IconView

- IconView is deprecated. Use `IconImage` property of `AdvancedEntry` instead.
- IconView won't be part of this package in next versions. Please consider to change it FontImage or something equivalent.

IconView one of the platform specific limitations in InputKit. With this removal, Windows platform support is became more stable.


> It's still working in `v4.1` but it will be removed in next versions.