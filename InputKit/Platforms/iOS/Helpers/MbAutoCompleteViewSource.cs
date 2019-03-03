using Foundation;
using Plugin.InputKit.Platforms.iOS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

namespace Plugin.InputKit.Platforms.iOS.Helpers
{

    public abstract class MbAutoCompleteViewSource : UITableViewSource
    {
        public ICollection<string> Suggestions { get; set; } = new List<string>();

        public MbAutoCompleteTextField AutoCompleteTextField { get; set; }

        public abstract void UpdateSuggestions(ICollection<string> suggestions);

        public abstract override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath);

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Suggestions.Count;
        }

        public event EventHandler<SelectedItemChangedEventArgs> Selected;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            AutoCompleteTextField.Text = Suggestions.ElementAt(indexPath.Row);
            AutoCompleteTextField.AutoCompleteTableView.Hidden = true;
            AutoCompleteTextField.ResignFirstResponder();
            var item = Suggestions.ToList()[(int)indexPath.Item];
            Selected?.Invoke(tableView, new SelectedItemChangedEventArgs(item));
            // don't call base.RowSelected
        }
    }
}
