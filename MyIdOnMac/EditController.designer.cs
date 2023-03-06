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
	[Register ("EditController")]
	partial class EditController
	{
		[Outlet]
		AppKit.NSButton buttonView { get; set; }

		[Outlet]
		AppKit.NSTableView imageTableView { get; set; }

		[Outlet]
		AppKit.NSTextField uxMemo { get; set; }

		[Outlet]
		AppKit.NSSecureTextField uxPassword { get; set; }

		[Outlet]
		AppKit.NSTextField uxSite { get; set; }

		[Outlet]
		AppKit.NSTextField uxUser { get; set; }

		[Action ("addImageAction:")]
		partial void addImageAction (Foundation.NSObject sender);

		[Action ("buttonViewAction:")]
		partial void buttonViewAction (Foundation.NSObject sender);

		[Action ("CancelDialog:")]
		partial void CancelDialog (Foundation.NSObject sender);

		[Action ("copyPassword:")]
		partial void copyPassword (Foundation.NSObject sender);

		[Action ("copyUser:")]
		partial void copyUser (Foundation.NSObject sender);

		[Action ("deleteImageAction:")]
		partial void deleteImageAction (Foundation.NSObject sender);

		[Action ("OkDialog:")]
		partial void OkDialog (Foundation.NSObject sender);

		[Action ("viewImageAction:")]
		partial void viewImageAction (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (buttonView != null) {
				buttonView.Dispose ();
				buttonView = null;
			}

			if (uxMemo != null) {
				uxMemo.Dispose ();
				uxMemo = null;
			}

			if (uxPassword != null) {
				uxPassword.Dispose ();
				uxPassword = null;
			}

			if (uxSite != null) {
				uxSite.Dispose ();
				uxSite = null;
			}

			if (uxUser != null) {
				uxUser.Dispose ();
				uxUser = null;
			}

			if (imageTableView != null) {
				imageTableView.Dispose ();
				imageTableView = null;
			}
		}
	}
}
