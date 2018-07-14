----------------After Installation----------------

Go your XAML page and add that xmlns to your <ContentPage tag

xmlns:input="clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit"


Then you will be able to use controls easily inside your page like:

            
            <input:CheckBox Text="Option 1" Type="Check" />

------------------------------------------------------------


-------------------Advanced-Entry-Alert-Icon----------------------------------
Your resources must contain alert.png to display validation warning on AdvancedEntry !
If doesn't exits alert icon won't be displayed.
------------------------------------------------------------------------------



--------------EASY--DESIGNING----------------

If you want to set all the controls in your app you can use GlobalSettings for this package.
Go your

App.cs 

to set default values of controls and set some properties in control which you need. Like below:

Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.BorderColor = Color.Red;
Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.Size = 36;
Plugin.InputKit.Shared.Controls.RadioButton.GlobalSetting.Color = Color.Red;


...and you'll see all the controls default values changed in entire project.


