// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using CoreAudioKit;
using System.Text;

namespace MyIdOnMac
{
	public partial class MainWindowController : NSWindowController
	{
		public MainWindowController (IntPtr handle) : base (handle)
		{
		}
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            var controller = ContentViewController as MainViewController;

            uxDeleteItem.Active = controller.ItemCount != 0;
            
        }

        public override void WindowDidLoad()
        {
            base.WindowDidLoad();


        }

        partial void deleteItem(NSObject sender)
        {

            var controller = ContentViewController as MainViewController;
            controller.DeleteItem();
            
            uxDeleteItem.Active = controller.ItemCount != 0;
        }

        partial void addItem(NSObject sender)
        {
            var controller = ContentViewController as MainViewController;
            controller.Add(sender);
        }

        public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            switch (segue.Identifier)
            {
                case "NewSegue":
                    var dialog = segue.DestinationController as WelcomeController;

                    dialog.Presentor = ContentViewController;

                    
                    break;
            }
        }

    }
}
