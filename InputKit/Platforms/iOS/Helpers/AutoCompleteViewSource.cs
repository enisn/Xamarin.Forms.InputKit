using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Microsoft.Maui.Controls;
using Plugin.InputKit.Platforms.iOS.Controls;
using UIKit;

namespace Plugin.InputKit.Platforms.iOS.Helpers
{
    public abstract class AutoCompleteViewSource : UITableViewSource
    {
        public ICollection<string> Suggestions { get; set; } = new List<string>();

        public AutoCompleteTextField AutoCompleteTextField { get; set; }

        public abstract void UpdateSuggestions(ICollection<string> suggestions);

        public abstract override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath);

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Suggestions.Count;
        }

        public event EventHandler<SelectedItemChangedEventArgs> Selected;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            AutoCompleteTextField.AutoCompleteTableView.Hidden = true;
            if (indexPath.Row < Suggestions.Count)
                AutoCompleteTextField.Text = Suggestions.ElementAt(indexPath.Row);
            AutoCompleteTextField.ResignFirstResponder();
            var index = (int)indexPath.Item;
            var item = Suggestions.ToList()[index];
            Selected?.Invoke(tableView, new SelectedItemChangedEventArgs(item, index));
            // don't call base.RowSelected
        }
    }
}
