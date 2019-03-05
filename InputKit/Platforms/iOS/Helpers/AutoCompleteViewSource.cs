using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Plugin.InputKit.Platforms.iOS.Controls;
using UIKit;
using Xamarin.Forms;

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
            var item = Suggestions.ToList()[(int)indexPath.Item];
            Selected?.Invoke(tableView, new SelectedItemChangedEventArgs(item));
            // don't call base.RowSelected
        }
    }
}
