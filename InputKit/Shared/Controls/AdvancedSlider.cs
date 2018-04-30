using System;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    public class AdvancedSlider : StackLayout
    {
        Slider slider = new Slider();
        Label lblTitle = new Label { Margin = new Thickness(20, 0), InputTransparent = true, FontAttributes = FontAttributes.Bold, TextColor = (Color)Label.TextColorProperty.DefaultValue, };
        Label lblValue = new Label { InputTransparent = true, TextColor = (Color)Label.TextColorProperty.DefaultValue, };
        Label lblMinValue = new Label { TextColor = (Color) Label.TextColorProperty.DefaultValue, };
        Label lblMaxValue = new Label { TextColor = (Color) Label.TextColorProperty.DefaultValue, };
        private string _valueSuffix;
        private string _valuePrefix;
        private Color _textColor;
        private double _stepValue = 1;

        public AdvancedSlider()
        {
            this.Children.Add(lblTitle);
            this.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    lblMinValue,
                    new Grid
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(0,30,0,0),
                        Children =
                        {
                            slider,
                            lblValue,
                        }
                    },
                    lblMaxValue,
                }
            });

            slider.ValueChanged += Slider_ValueChanged;
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue % StepValue != 0)
            {
                slider.Value = Math.Round(e.NewValue / StepValue) * StepValue;
                return;
            }
            UpdateValueText();
            UpdateView();
            SetValue(ValueProperty, slider.Value);
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            UpdateValueText();
            UpdateView();
            UpdateMinMaxValueText();
        }
        ///---------------------------------------------------------------------
        /// <summary>
        /// Value of slider which user selected
        /// </summary>
        public double Value { get => slider.Value; set => slider.Value = value; }
        ///---------------------------------------------------------------------
        /// <summary>
        /// Title of slider, It'll be shown tp of slider
        /// </summary>
        public string Title { get => lblTitle.Text; set { lblTitle.Text = value; lblTitle.IsVisible = !String.IsNullOrEmpty(value); } }
        ///---------------------------------------------------------------------
        /// <summary>
        /// It will be displayed start of value 
        /// </summary>
        public string ValueSuffix { get => _valueSuffix; set { _valueSuffix = value; UpdateValueText(); } }
        ///---------------------------------------------------------------------
        /// <summary>
        /// It'll be displayed end of value
        /// </summary>
        public string ValuePrefix { get => _valuePrefix; set { _valuePrefix = value; UpdateValueText(); } }
        ///---------------------------------------------------------------------
        /// <summary>
        /// Minimum value, user can slide
        /// </summary>
        public double MinValue { get => slider.Minimum; set { slider.Minimum = value; UpdateMinMaxValueText(); } }
        ///---------------------------------------------------------------------
        /// <summary>
        /// Maximum value, user can slide
        /// </summary>
        public double MaxValue { get => slider.Maximum; set { slider.Maximum = value; UpdateMinMaxValueText(); } }
        ///---------------------------------------------------------------------
        /// <summary>
        /// Slider Increase number
        /// </summary>
        public double StepValue { get => _stepValue; set { _stepValue = value; UpdateValueText(); UpdateView(); } }
        ///---------------------------------------------------------------------
        /// <summary>
        /// Visibility of Min value and Max value at right and left
        /// </summary>
        public bool DisplayMinMaxValue
        {
            get => lblMinValue.IsVisible && lblMaxValue.IsVisible;

            set { lblMaxValue.IsVisible = value; lblMinValue.IsVisible = value; }
        }
        ///---------------------------------------------------------------------
        /// <summary>
        /// Text color of labels
        /// </summary>
        public Color TextColor
        {
            get => _textColor; set
            {
                _textColor = value;
                lblMaxValue.TextColor = value;
                lblMinValue.TextColor = value;
                lblTitle.TextColor = value;
                lblValue.TextColor = value;
            }
        }

        #region BindableProperties
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(AdvancedSlider), 0.0, BindingMode.OneWayToSource, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).Value = (double)nv);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AdvancedSlider), Color.Gray, BindingMode.OneWayToSource, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).TextColor = (Color)nv);
        #endregion

        void UpdateMinMaxValueText()
        {
            lblMinValue.Text = this.MinValue.ToString();
            lblMaxValue.Text = this.MaxValue.ToString() + "+ ";
        }
        void UpdateValueText()
        {
            lblValue.Text = $"{this.ValuePrefix} {this.Value} {this.ValueSuffix}";
        }
        void UpdateView()
        {

            lblValue.TranslateTo(
                this.Value * ((slider.Width - 40) / this.MaxValue), //pos X
                slider.TranslationY - lblValue.Height * 0.9, //pos Y
                40 //Latency
                );


            //lblValue.LayoutTo(new Rectangle(new Point(pos, slider.Y + lblValue.Height * 0.8), new Size(lblValue.Width, lblValue.Height)));
        }
    }
}
