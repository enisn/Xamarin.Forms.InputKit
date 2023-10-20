
# Getting Started with InputKit on MAUI
> MAUI doesn't implement exactly same features as Xamarin.Forms.InputKit.
> You can follow the progress with [this issue](https://github.com/enisn/Xamarin.Forms.InputKit/issues/221)

## Installation

- Install [InputKit.Maui](https://www.nuget.org/packages/InputKit.Maui) package from NuGet.

- Go to your **MauiProgram.cs** file and add following line:

    ```csharp
    builder
    .UseMauiApp<App>()
    .ConfigureMauiHandlers(handlers =>
    {
        // Add following line:
        handlers.AddInputKitHandlers(); // 👈
    })
                            
    ```
						
<hr />

## Usage

- Define following xml namespace to your Page tag as attribute:

    ```xml
    xmlns:input="clr-namespace:InputKit.Shared.Controls;assembly=InputKit.Maui"
    ```
    > [See how it's done at sample ↗️](https://github.com/enisn/Xamarin.Forms.InputKit/blob/develop/sandbox/SandboxMAUI/Pages/CheckBoxPage.xaml#L3)
- Then use it at wherever you want in your page with defined prefix `input` like following code:


    ```xml
    <input:CheckBox Text="Hello World">
    ```
