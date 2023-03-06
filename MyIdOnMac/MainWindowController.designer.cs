// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MyIdOnMac
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MyIdOnMac.ActivatableItem uxDeleteItem { get; set; }

		[Outlet]
		MyIdOnMac.ActivatableItem uxEditItem { get; set; }

		[Action ("addItem:")]
		partial void addItem (Foundation.NSObject sender);

		[Action ("deleteItem:")]
		partial void deleteItem (Foundation.NSObject sender);

		[Action ("editItem:")]
		partial void editItem (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (uxDeleteItem != null) {
				uxDeleteItem.Dispose ();
				uxDeleteItem = null;
			}

			if (uxEditItem != null) {
				uxEditItem.Dispose ();
				uxEditItem = null;
			}
		}
	}
}
