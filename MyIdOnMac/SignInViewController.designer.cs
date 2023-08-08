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
	[Register ("SignInViewController")]
	partial class SignInViewController
	{
		[Outlet]
		AppKit.NSSecureTextField uxPassword { get; set; }

		[Action ("CancelDialog:")]
		partial void CancelDialog (Foundation.NSObject sender);

		[Action ("OkDialog:")]
		partial void OkDialog (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (uxPassword != null) {
				uxPassword.Dispose ();
				uxPassword = null;
			}
		}
	}
}
