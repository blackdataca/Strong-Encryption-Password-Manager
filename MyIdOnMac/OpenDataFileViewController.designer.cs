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
	[Register ("OpenDataFileViewController")]
	partial class OpenDataFileViewController
	{
		[Outlet]
		AppKit.NSButton uxBrowsePrivateKeyFile { get; set; }

		[Outlet]
		AppKit.NSTextField uxDataFile { get; set; }

		[Outlet]
		AppKit.NSTextField uxPrivateKeyFile { get; set; }

		[Outlet]
		AppKit.NSButton uxPrivateKeyThisComputer { get; set; }

		[Outlet]
		AppKit.NSButton uxPriviateKeyOn { get; set; }

		[Action ("browseDataFile:")]
		partial void browseDataFile (Foundation.NSObject sender);

		[Action ("browsePrivateKey:")]
		partial void browsePrivateKey (Foundation.NSObject sender);

		[Action ("cancelDialog:")]
		partial void cancelDialog (Foundation.NSObject sender);

		[Action ("okDialog:")]
		partial void okDialog (Foundation.NSObject sender);

		[Action ("privateKeySelectionChanged:")]
		partial void privateKeySelectionChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (uxDataFile != null) {
				uxDataFile.Dispose ();
				uxDataFile = null;
			}

			if (uxPrivateKeyFile != null) {
				uxPrivateKeyFile.Dispose ();
				uxPrivateKeyFile = null;
			}

			if (uxPrivateKeyThisComputer != null) {
				uxPrivateKeyThisComputer.Dispose ();
				uxPrivateKeyThisComputer = null;
			}

			if (uxPriviateKeyOn != null) {
				uxPriviateKeyOn.Dispose ();
				uxPriviateKeyOn = null;
			}

			if (uxBrowsePrivateKeyFile != null) {
				uxBrowsePrivateKeyFile.Dispose ();
				uxBrowsePrivateKeyFile = null;
			}
		}
	}
}
