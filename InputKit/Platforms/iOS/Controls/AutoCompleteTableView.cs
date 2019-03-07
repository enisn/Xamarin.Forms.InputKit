using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace Plugin.InputKit.Platforms.iOS.Controls
{
    internal class AutoCompleteTableView : UITableView
    {
        private readonly UIScrollView _parentScrollView;

        public AutoCompleteTableView(UIScrollView parentScrollView)
        {
            _parentScrollView = parentScrollView;
        }

        public override bool Hidden
        {
            get { return base.Hidden; }
            set
            {
                base.Hidden = value;
                if (_parentScrollView == null) return;
                _parentScrollView.DelaysContentTouches = !value;
            }
        }
    }
}
