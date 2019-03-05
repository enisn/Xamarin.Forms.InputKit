using UIKit;

namespace Plugin.InputKit.Platforms.iOS.Helpers
{
    internal class MbAutoCompleteTableView : UITableView
    {
        private readonly UIScrollView _parentScrollView;

        public MbAutoCompleteTableView(UIScrollView parentScrollView)
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
