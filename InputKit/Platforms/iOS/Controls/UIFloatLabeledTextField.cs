using CoreAnimation;
using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace Plugin.InputKit.Platforms.iOS.Controls
{

    public class FloatLabeledTextField : UITextField
    {
        private readonly UILabel _errorLabel;
        private readonly UILabel _floatingLabel;
        private readonly CALayer _underline;
        private UIColor _floatingLabelTextColor;
        private UIColor _floatingLabelActiveTextColor;

        public FloatLabeledTextField()
        {
            _floatingLabel = new UILabel { Alpha = 0.0f };
            _errorLabel = new UILabel { Font = UIFont.SystemFontOfSize(11) };

            _underline = new CALayer
            {
                MasksToBounds = true,
                BorderColor = UIColor.Clear.CGColor,
                BackgroundColor = UIColor.Clear.CGColor,
                BorderWidth = 1f
            };

            AddSubview(_errorLabel);
            AddSubview(_floatingLabel);
            Layer.AddSublayer(_underline);

            BorderStyle = UITextBorderStyle.None;
            ErrorTextColor = UIColor.Red;
            UnderlineErrorTextIsVisible = false;
            FloatingLabelTextColor = UIColor.DarkGray;
            FloatingLabelFont = UIFont.BoldSystemFontOfSize(12);
        }

        public UIColor FloatingLabelTextColor
        {
            get { return _floatingLabelTextColor; }
            set
            {
                _floatingLabelTextColor = value;
                if (!IsFirstResponder)
                {
                    _floatingLabel.TextColor = value;
                }
            }
        }

        public UIColor FloatingLabelActiveTextColor
        {
            get { return _floatingLabelActiveTextColor; }
            set
            {
                _floatingLabelActiveTextColor = value;
                if (IsFirstResponder)
                {
                    _floatingLabel.TextColor = value;
                }
            }
        }

        public bool FloatingLabelEnabled { get; set; } = true;

        public bool UnderlineErrorSpaceEnabled { get; set; } = true;

        public float UnderlineSpace
        {
            get
            {
                return UnderlineErrorSpaceEnabled ? 22 : 4;
            }
        }
        public UIColor ErrorTextColor
        {
            get { return _errorLabel.TextColor; }
            set { _errorLabel.TextColor = value; }
        }

        public bool UnderlineErrorTextIsVisible
        {
            get { return !_errorLabel.Hidden; }
            set
            {
                _errorLabel.Hidden = !value;
            }
        }

        public CGColor UnderlineColor
        {
            get { return _underline.BackgroundColor; }
            set { _underline.BackgroundColor = value; }
        }

        public UIFont FloatingLabelFont
        {
            get { return _floatingLabel.Font; }
            set { _floatingLabel.Font = value; }
        }

        public string ErrorText
        {
            get { return _errorLabel.Text; }
            set
            {
                _errorLabel.Text = value;
                _errorLabel.SizeToFit();
                _errorLabel.Frame =
                    new CGRect(
                        0,
                        _errorLabel.Font.LineHeight + 30,
                        _errorLabel.Frame.Size.Width,
                        _errorLabel.Frame.Size.Height);
            }
        }

        public override string Placeholder
        {
            get { return base.Placeholder; }
            set
            {
                base.Placeholder = value;

                _floatingLabel.Text = value;
                _floatingLabel.SizeToFit();
                _floatingLabel.Frame =
                    new CGRect(
                        0,
                        _floatingLabel.Font.LineHeight,
                        _floatingLabel.Frame.Size.Width,
                        _floatingLabel.Frame.Size.Height);
            }
        }

        public override CGRect TextRect(CGRect forBounds)
        {
            if (_floatingLabel == null)
            {
                return base.TextRect(forBounds);
            }
            return InsetRect(base.TextRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, UnderlineSpace, 0));
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            if (_floatingLabel == null)
            {
                return base.EditingRect(forBounds);
            }

            return InsetRect(base.EditingRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, UnderlineSpace, 0));
        }

        public override CGRect ClearButtonRect(CGRect forBounds)
        {
            var rect = base.ClearButtonRect(forBounds);

            if (_floatingLabel == null)
            {
                return rect;
            }

            return new CGRect(
                rect.X,
                rect.Y + _floatingLabel.Font.LineHeight / 2.0f,
                rect.Size.Width,
                rect.Size.Height);
        }

        public override void LayoutSubviews()
        {

            _underline.Frame = new CGRect(0f, Frame.Height - UnderlineSpace + 2, Frame.Width, 1f);

            // local function
            void UpdateLabel()
            {
                if (!string.IsNullOrEmpty(Text) && FloatingLabelEnabled)
                {
                    _floatingLabel.Alpha = 1.0f;
                    _floatingLabel.Frame = new CGRect(_floatingLabel.Frame.Location.X, 2.0f, _floatingLabel.Frame.Size.Width, _floatingLabel.Frame.Size.Height);
                }
                else
                {
                    _floatingLabel.Alpha = 0.0f;
                    _floatingLabel.Frame = new CGRect(_floatingLabel.Frame.Location.X, _floatingLabel.Font.LineHeight, _floatingLabel.Frame.Size.Width, _floatingLabel.Frame.Size.Height);
                }
            }

            if (IsFirstResponder)
            {
                _floatingLabel.TextColor = FloatingLabelActiveTextColor;

                var shouldFloat = !string.IsNullOrEmpty(Text) && FloatingLabelEnabled;
                var isFloating = _floatingLabel.Alpha == 1f;

                if (shouldFloat == isFloating)
                {
                    UpdateLabel();
                }
                else
                {
                    Animate(
                        0.3f,
                        0.0f,
                        UIViewAnimationOptions.BeginFromCurrentState
                        | UIViewAnimationOptions.CurveEaseOut,
                        UpdateLabel,
                        () => { });
                }
            }
            else
            {
                _floatingLabel.TextColor = FloatingLabelTextColor;

                UpdateLabel();
            }
            base.LayoutSubviews();
        }

        private static CGRect InsetRect(CGRect rect, UIEdgeInsets insets)
        {
            return new CGRect(
                rect.X + insets.Left,
                rect.Y + insets.Top,
                rect.Width - insets.Left - insets.Right,
                rect.Height - insets.Top - insets.Bottom);
        }
    }
}
