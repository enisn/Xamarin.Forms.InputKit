using CoreGraphics;
using Plugin.InputKit.Platforms.iOS;
using Plugin.InputKit.Platforms.iOS.Controls;
using Plugin.InputKit.Platforms.iOS.Helpers;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AutoCompleteEntry), typeof(AutoCompleteEntryRenderer))]
namespace Plugin.InputKit.Platforms.iOS
{
    public class AutoCompleteEntryRenderer : ViewRenderer<AutoCompleteEntry, FloatLabeledTextField>
    {
        private bool _hasError;
        private bool _hasFocus;
        private UIColor _defaultTextColor;

        protected override void OnElementChanged(ElementChangedEventArgs<AutoCompleteEntry> e)
        {
            base.OnElementChanged(e);

            // unsubscribe
            if (e.OldElement != null)
            {
                Control.EditingDidBegin -= OnEditingDidBegin;
                Control.EditingDidEnd -= OnEditingDidEnd;
                Control.EditingChanged -= ViewOnEditingChanged;
            }

            if (e.NewElement != null)
            {
                SetNativeControl(CreateNativeControl());

                if (!string.IsNullOrWhiteSpace(Element.AutomationId))
                    SetAutomationId(Element.AutomationId);

                _defaultTextColor = Control.FloatingLabelTextColor;

                SetIsPassword();
                SetText();
                SetHintText();
                SetTextColor();
                SetBackgroundColor();
                SetPlaceholderColor();
                SetKeyboard();
                SetHorizontalTextAlignment();
                SetFont();
                SetUnfocusedColor();
                
                Control.EditingDidBegin += OnEditingDidBegin;
                Control.EditingDidEnd += OnEditingDidEnd;
                Control.EditingChanged += ViewOnEditingChanged;
            }
        }

        protected virtual FloatLabeledTextField CreateNativeControl()
        {
            return new FloatLabeledTextField();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
            if (e.PropertyName == Entry.TextColorProperty.PropertyName)
                SetTextColor();
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                SetBackgroundColor();
            else if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
                SetIsPassword();
            else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            {
                SetIsPassword();
                SetTextColor();
            }
            else if (e.PropertyName == Entry.TextProperty.PropertyName)
                SetText();
            else if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
            {
                SetUnfocusedColor();
                SetPlaceholderColor();
            }
            else if (e.PropertyName == Xamarin.Forms.InputView.KeyboardProperty.PropertyName)
                SetKeyboard();
            else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
                SetHorizontalTextAlignment();
            else if ((e.PropertyName == Entry.FontAttributesProperty.PropertyName) ||
                     (e.PropertyName == Entry.FontFamilyProperty.PropertyName) ||
                     (e.PropertyName == Entry.FontSizeProperty.PropertyName))
                SetFont();
        }

        private void OnEditingDidEnd(object sender, EventArgs eventArgs)
        {
            _hasFocus = false;
            Control.UnderlineColor = GetUnderlineColorForState();
        }

        private void OnEditingDidBegin(object sender, EventArgs eventArgs)
        {
            _hasFocus = true;
            Control.UnderlineColor = GetUnderlineColorForState();
        }

        private void ViewOnEditingChanged(object sender, EventArgs eventArgs)
        {
            Element?.SetValueFromRenderer(Entry.TextProperty, Control.Text);
        }

        private void SetUnfocusedColor()
        {
            Control.FloatingLabelTextColor = Element.PlaceholderColor == Color.Default
                ? Control.TextColor
                : Element.PlaceholderColor.ToUIColor();
        }

        private CGColor GetUnderlineColorForState()
        {
            if (_hasError) return UIColor.Red.CGColor;
            return 
            (Element.PlaceholderColor == Color.Default
                    ? Control.TextColor.CGColor
                    : Element.PlaceholderColor.ToCGColor());
        }

        private void SetBackgroundColor()
        {
            NativeView.BackgroundColor = Element.BackgroundColor.ToUIColor();
            Control.UnderlineColor = Element.PlaceholderColor.ToCGColor();
        }

        private void SetText()
        {
            if (Control.Text != Element.Text)
                Control.Text = Element.Text;
        }

        private void SetIsPassword()
        {
            if (Element.IsPassword && Control.IsFirstResponder)
            {
                Control.Enabled = false;
                Control.SecureTextEntry = true;
                Control.Enabled = Element.IsEnabled;
                Control.BecomeFirstResponder();
            }
            else
            {
                Control.SecureTextEntry = Element.IsPassword;
            }
        }

        private void SetHintText()
        {
            Control.Placeholder = Element.Placeholder;
        }

        // taken from Xamarin.Forms codebase
        private void SetPlaceholderColor()
        {
            var formatted = (FormattedString)Element.Placeholder;

            if (formatted == null)
                return;

            var targetColor = Element.PlaceholderColor;

            // Placeholder default color is 70% gray
            // https://developer.apple.com/library/prerelease/ios/documentation/UIKit/Reference/UITextField_Class/index.html#//apple_ref/occ/instp/UITextField/placeholder

            var color = Element.IsEnabled && !targetColor.IsDefault ? targetColor : Color.Gray;
        }

        private void SetTextColor()
        {
            if ((Element.TextColor == Color.Default) || !Element.IsEnabled)
                Control.TextColor = _defaultTextColor;
            else
                Control.TextColor = Element.TextColor.ToUIColor();
        }

        private void SetFont()
        {
            Control.Font = Element.ToUIFont();
        }

        private void SetHorizontalTextAlignment()
        {
            switch (Element.HorizontalTextAlignment)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                default:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

        private void SetKeyboard()
        {
            var kbd = Element.Keyboard.ToNative();
            Control.KeyboardType = kbd;
            Control.InputAccessoryView = kbd == UIKeyboardType.NumberPad ? NumberpadAccessoryView() : null;
            Control.ShouldReturn = InvokeCompleted;
        }

        private UIToolbar NumberpadAccessoryView()
        {
            return new UIToolbar(new RectangleF(0.0f, 0.0f, (float)Control.Frame.Size.Width, 44.0f))
            {
                Items = new[]
                {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { InvokeCompleted(null); })
                }
            };
        }

        private bool InvokeCompleted(UITextField textField)
        {
            Control.ResignFirstResponder();
            ((IEntryController)Element).SendCompleted();
            return true;
        }
    }
}
