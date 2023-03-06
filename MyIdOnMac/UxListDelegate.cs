using System;
using AppKit;
using StoreKit;

namespace MyIdOnMac
{
	public class UxListDelegate : NSTableViewDelegate
    {
		private const string CellIdentifier = "ItemCell";
		private UxListDataSource DataSource;
		public UxListDelegate(UxListDataSource dataSource)
		{
			this.DataSource = dataSource;
		}
        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTextField view = (NSTextField)tableView.MakeView(CellIdentifier, this);
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier = CellIdentifier;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            // Setup view based on the column selected
            switch (tableColumn.Title)
            {
                case "Site":
                    view.StringValue = DataSource.Get((int)row).Site;
                    break;
                case "User":
                    view.StringValue = DataSource.Get((int)row).User;
                    break;
                case "Password":
                    view.StringValue = DataSource.Get((int)row).Password;
                    break;
                case "Last Update":
                    view.StringValue = DataSource.Get((int)row).ChangedHuman;
                    break;
                case "Memo":
                    view.StringValue = DataSource.Get((int)row).Memo;
                    break;
            }

            return view;
        }


        /*
        public override bool ShouldSelectRow(NSTableView tableView, nint row)
        {
            return true;
        }*/

        public override nint GetNextTypeSelectMatch(NSTableView tableView, nint startRow, nint endRow, string searchString)
        {
            return DataSource.Search(searchString);
            
        }
    }
}

