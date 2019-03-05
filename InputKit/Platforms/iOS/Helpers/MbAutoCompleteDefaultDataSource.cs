using Foundation;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Plugin.InputKit.Platforms.iOS.Helpers
{
    public class MbAutoCompleteDefaultDataSource : MbAutoCompleteViewSource
    {
        private const string _cellIdentifier = "DefaultIdentifier";

        public override void UpdateSuggestions(ICollection<string> suggestions)
        {
            Suggestions = suggestions;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(_cellIdentifier);
            var item = Suggestions.ElementAt(indexPath.Row);

            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, _cellIdentifier);

            cell.BackgroundColor = UIColor.Clear;
            cell.TextLabel.Text = item;

            return cell;
        }
    }
}
