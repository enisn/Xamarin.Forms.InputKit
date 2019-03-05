using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Abstraction
{
    public class DefaultAnimator<T> : IAnimator<T> where T : View
    {
        public void Animate(T view)
        {
            try
            {
                if (actions.TryGetValue(view.GetType(), out Action<View> _action))
                    _action(view);
                else
                    actions[typeof(View)](view);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex?.ToString());
            }
        }

        //DECLERATION
        static readonly Dictionary<Type, Action<View>> actions = new Dictionary<Type, Action<View>>
        {
            {
                typeof(View), //default
                async (v) =>
                {
                    await v.ScaleTo(.9,100);
                    await v.ScaleTo(1,100);
                }
            },
            {
                typeof(SelectionView), //default
                async (v) =>
                {
                    await v.FadeTo(.9,50);
                    await v.FadeTo(1,50);
                    //nothing.
                }
            },
            {
                typeof(CheckBox),
                (v) =>
                {
                    var chk = v as CheckBox;
                    if (chk.Type == CheckBox.CheckType.Material)
                        chk.boxBackground.BackgroundColor = chk.IsChecked ? chk.Color : Color.Transparent;
                    else
                        chk.boxBackground.BorderColor = chk.IsChecked ? chk.Color : chk.BorderColor;
                }
            },
            {
                typeof(RadioButton),
                (v) =>
                {
                    var rb = v as RadioButton;
                    rb.iconCircle.FillColor = rb.IsChecked ? rb.Color : rb.CircleColor;
                }
            }
        };
    }
}
