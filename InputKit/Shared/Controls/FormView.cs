using Plugin.InputKit.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    public class FormView : StackLayout
    {
        /// <summary>
        /// Gets a result from elements inside itself which inherits from IValidatable
        /// </summary>
        public bool IsValidated
        {
            get
            {
                foreach (var item in this.Children)
                {
                    if (item is IValidatable && (item as IValidatable).IsRequired && !(item as IValidatable).IsValidated)
                        return false;
                }
                return true;
            }
        }
        public void ShowValidation()
        {
            foreach (var item in this.Children)
            {
                if (item is IValidatable)
                    (item as IValidatable).DisplayValidation();
            }
        }
    }
}
