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
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		AppKit.NSTableColumn lastUpdateColumn { get; set; }

		[Outlet]
		AppKit.NSTableColumn memoColumn { get; set; }

		[Outlet]
		AppKit.NSTableColumn passwordColumn { get; set; }

		[Outlet]
		AppKit.NSTableColumn siteColumn { get; set; }

		[Outlet]
		AppKit.NSTableColumn userColumn { get; set; }

		[Outlet]
		AppKit.NSTableView uxList { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lastUpdateColumn != null) {
				lastUpdateColumn.Dispose ();
				lastUpdateColumn = null;
			}

			if (memoColumn != null) {
				memoColumn.Dispose ();
				memoColumn = null;
			}

			if (passwordColumn != null) {
				passwordColumn.Dispose ();
				passwordColumn = null;
			}

			if (siteColumn != null) {
				siteColumn.Dispose ();
				siteColumn = null;
			}

			if (userColumn != null) {
				userColumn.Dispose ();
				userColumn = null;
			}

			if (uxList != null) {
				uxList.Dispose ();
				uxList = null;
			}
		}
	}
}
