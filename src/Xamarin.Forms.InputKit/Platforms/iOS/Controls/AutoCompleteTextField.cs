using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Plugin.InputKit.Platforms.iOS.Helpers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Plugin.InputKit.Platforms.iOS.Controls
{
    [Register("UIAutoCompleteTextField")]
    public class AutoCompleteTextField : UITextField, IUITextFieldDelegate
    {
        private AutoCompleteViewSource _autoCompleteViewSource;
        private UIView _background;
        private CGRect _drawnFrame;
        private List<string> _items;
        private UIViewController _parentViewController;
        private UIScrollView _scrollView;

        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; set; } = (t, d) => d;

        public AutoCompleteViewSource AutoCompleteViewSource
        {
            get { return _autoCompleteViewSource; }
            set
            {
                _autoCompleteViewSource = value;
                _autoCompleteViewSource.AutoCompleteTextField = this;
                if (AutoCompleteTableView != null)
                {
                    AutoCompleteTableView.Source = AutoCompleteViewSource;
                }
            }
        }

        public UITableView AutoCompleteTableView { get; private set; }

        public bool IsInitialized { get; private set; }

        public int Threshold { get; set; } = 2;

        public int AutocompleteTableViewHeight { get; set; } = 150;

        public void Draw(UIViewController viewController, CALayer layer, UIScrollView scrollView, nfloat y)
        {
            _scrollView = scrollView;
            _drawnFrame = layer.Frame;
            _parentViewController = viewController ?? throw new ArgumentNullException(nameof(viewController), "View cannot be null");


            //Make new tableview and do some settings
            AutoCompleteTableView = new AutoCompleteTableView(_scrollView)
            {
                DelaysContentTouches = true,
                ClipsToBounds = true,
                ScrollEnabled = true,
                AllowsSelection = true,
                Bounces = false,
                Hidden = true,
                ContentInset = UIEdgeInsets.Zero,
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                Source = AutoCompleteViewSource,
                TableFooterView = new UIView()
            };

            //Some textfield settings
            AutocorrectionType = UITextAutocorrectionType.No;
            ClearButtonMode = UITextFieldViewMode.Never;

            var scrollViewIsNull = _scrollView == null;

            CGRect frame;
            UIView view;
            if (scrollViewIsNull)
            {
                view = _parentViewController.View;
                frame = new CGRect(_drawnFrame.X, y + _drawnFrame.Height, _drawnFrame.Width, AutocompleteTableViewHeight);
            }
            else
            {
                var e = (ScrollView)((ScrollViewRenderer)_scrollView).Element;
                var p = e.Padding;
                var m = e.Margin;
                frame = new CGRect(_drawnFrame.X + p.Left + m.Left,
                    y + _drawnFrame.Height,
                    _drawnFrame.Width,
                    AutocompleteTableViewHeight);
                view = _scrollView;
            }

            AutoCompleteTableView.Layer.CornerRadius = 5;

            _background = new UIView(frame) { BackgroundColor = UIColor.White, Hidden = true };
            _background.Layer.CornerRadius = 5; //rounded corners
            _background.Layer.MasksToBounds = false;
            _background.Layer.ShadowColor = UIColor.Black.CGColor;
            _background.Layer.ShadowOffset = new CGSize(0.0f, 4.0f);
            _background.Layer.ShadowOpacity = 0.25f;
            _background.Layer.ShadowRadius = 8f;
            _background.Layer.BorderColor = UIColor.LightGray.CGColor;
            _background.Layer.BorderWidth = 0.1f;

            AutoCompleteTableView.Frame = frame;
            view.AddSubview(_background);
            view.AddSubview(AutoCompleteTableView);

            //listen to edit events
            EditingChanged += OnEditingChanged;
            EditingDidEnd += OnEditingDidEnd;

            UpdateTableViewData();
            IsInitialized = true;
        }

        private void OnEditingDidEnd(object sender, EventArgs eventArgs)
        {
            HideAutoCompleteView();
        }

        private void OnEditingChanged(object sender, EventArgs eventArgs)
        {
            if (Text.Length >= Threshold)
            {
                ShowAutoCompleteView();
                UpdateTableViewData();
            }
            else
            {
                HideAutoCompleteView();
            }
        }

        private void ShowAutoCompleteView()
        {
            _background.Hidden = false;
            AutoCompleteTableView.Hidden = false;
            if (_scrollView != null)
            {
                _scrollView.ScrollRectToVisible(AutoCompleteTableView.Frame, true);
            }
        }

        private void HideAutoCompleteView()
        {
            _background.Hidden = true;
            AutoCompleteTableView.Hidden = true;
        }

        public void UpdateTableViewData()
        {
            var sorted = SortingAlgorithm(Text, _items);
            if (!sorted.Any())
            {
                HideAutoCompleteView();
                return;
            }
            AutoCompleteViewSource.Suggestions = sorted;
            AutoCompleteTableView.ReloadData();

            var f = AutoCompleteTableView.Frame;
            var height = Math.Min(AutocompleteTableViewHeight, (int)AutoCompleteTableView.ContentSize.Height);
            var frame = new CGRect(f.X, f.Y, f.Width, height);
            AutoCompleteTableView.Frame = frame;
            _background.Frame = frame;
        }

        public void UpdateItems(List<string> items)
        {
            _items = items;
            AutoCompleteViewSource.UpdateSuggestions(items);
        }
    }
}
