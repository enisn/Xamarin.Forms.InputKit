using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Abstraction
{
    public interface IAnimator<T> where T : View 
    {
        void Animate(T view);
    }
}
