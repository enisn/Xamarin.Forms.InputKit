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
                typeof(CheckBox),
                async (v)=>
                {
                    var chk = v as CheckBox;
                    await chk.boxBackground.ScaleTo(0.9, 100, Easing.BounceIn);
                    if (chk.Type == CheckBox.CheckType.Material)
                        chk.boxBackground.BackgroundColor = chk.IsChecked ? chk.Color : Color.Transparent;
                    else
                        chk.boxBackground.BorderColor = chk.IsChecked ? chk.Color : chk.BorderColor;
                    await chk.boxBackground.ScaleTo(1, 100, Easing.BounceIn);
                }
            },
            {
                typeof(RadioButton),
                async (v) =>
                {
                    var rb = v as RadioButton;
                    if (rb.IsChecked)
                    {
                        await rb.lblEmpty.ScaleTo(.5,100, Easing.BounceIn);
                        rb.lblEmpty.TextColor = rb.IsChecked ? rb.Color : rb.CircleColor;
                        await rb.lblEmpty.ScaleTo(1,100, Easing.BounceIn);
                    }
                    else
                        rb.lblEmpty.TextColor = rb.IsChecked ? rb.Color : rb.CircleColor;

                }
            }
        };
    }
}
