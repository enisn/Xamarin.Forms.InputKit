using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.InputKit.Shared.Utils
{
    public class SelectedItemChangedArgs : EventArgs
    {
        public SelectedItemChangedArgs(object newItem, int newItemIndex)
        {
            NewItem = newItem;
            NewItemIndex = newItemIndex;
        }

        public object NewItem { get; set; }
        public int NewItemIndex { get; set; }
    }
}
