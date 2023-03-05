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
	[Register ("WelcomeController")]
	partial class WelcomeController
	{
		[Outlet]
		AppKit.NSTextField uxDataFile { get; set; }

		[Outlet]
		AppKit.NSSecureTextField uxMasterPin { get; set; }

		[Outlet]
		AppKit.NSSecureTextField uxVerifyPin { get; set; }

		[Action ("BrowseNewDataFile:")]
		partial void BrowseNewDataFile (Foundation.NSObject sender);

		[Action ("CancelDialog:")]
		partial void CancelDialog (Foundation.NSObject sender);

		[Action ("OkDialog:")]
		partial void OkDialog (Foundation.NSObject sender);

		[Action ("OpenDialog:")]
		partial void OpenDialog (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (uxDataFile != null) {
				uxDataFile.Dispose ();
				uxDataFile = null;
			}

			if (uxMasterPin != null) {
				uxMasterPin.Dispose ();
				uxMasterPin = null;
			}

			if (uxVerifyPin != null) {
				uxVerifyPin.Dispose ();
				uxVerifyPin = null;
			}
		}
	}
}
