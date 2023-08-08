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
	partial class AppDelegate
	{
		[Outlet]
		AppKit.NSMenuItem uxMenuNew { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (uxMenuNew != null) {
				uxMenuNew.Dispose ();
				uxMenuNew = null;
			}
		}
	}
}
