# Options
InputKit has options system to customize your controls from single point. Default values of controls can be set and it's much more performant against setting them in XAML page.


## InputKitOptions
This is a simple options that define Accent color of controls.

Following properties can be configured.

- GetAccentColor : Gets accent color of controls.

    ```csharp
    InputKitOptions.GetAccentColor = () => Color.FromArgb("#1CD6CE");
    ```

## GlobalSettings
This is a simple options that define default values of controls. Each control has its own GlobalSettings.

GlobalSettins is a static property o control and it can be accessed over a control. It should be configured while application initializing. _(MauiProgram.cs or App.xaml.cs)_


```csharp
InputKit.Shared.Controls.CheckBox.GlobalSetting.Color = Colors.Blue;
InputKit.Shared.Controls.CheckBox.GlobalSetting.FontFamily = "Roboto";
InputKit.Shared.Controls.CheckBox.GlobalSetting.LabelPosition = LabelPosition.Before;
```