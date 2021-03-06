using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using SciChart.Examples.Demo.Application;

namespace Xamarin.Examples.Demo.iOS
{
    public partial class MainViewController : UITableViewController
    {
        private List<Example> _examples = ExampleManager.Instance.Examples;

        protected MainViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.DataSource = this;
            TableView.Delegate = this;
            TableView.BackgroundColor = new UIColor(red:0.14f, green:0.14f, blue:0.15f, alpha:1.0f); // #232426
            TableView.SeparatorColor = new UIColor(red: 0.11f, green: 0.11f, blue: 0.11f, alpha: 1.0f); // #1B1B1B
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
            TableView.RowHeight = 60;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _examples.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var example = _examples[indexPath.Row];

            var cell = tableView.DequeueReusableCell(ExampleTableViewCell.Key) as ExampleTableViewCell;
            if (cell == null)
            {
                cell = ExampleTableViewCell.Nib.Instantiate(tableView, null)[0] as ExampleTableViewCell;
            }

            cell.UpdateCell(example.Title, example.Description, example.Icon);

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var exampleType = _examples[indexPath.Row].ExampleType;

            var exampleViewController = (UIViewController)Activator.CreateInstance(exampleType);
            NavigationController.PushViewController(exampleViewController, true);
        }
    }
}