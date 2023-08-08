using System;
using System.Security.Policy;
using AppKit;
using Foundation;

namespace MyIdOnMac
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : NSApplicationDelegate
	{
		public AppDelegate ()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			// Insert code here to initialize your application
            
		}

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }

        [Export("openDocument:")]
        void OpenDialog(NSObject sender)
        {
            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = true;
            dlg.CanChooseDirectories = false;

            if (dlg.RunModal() == 1)
            {
                // Add document to the Open Recent menu
                                NSDocumentController.SharedDocumentController.NoteNewRecentDocumentURL(dlg.Url);
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = "At this point we should do something with the folder that the user just selected in the Open File Dialog box...",
                    MessageText = "Folder Selected"

                };
                alert.RunModal();
            }
        }

        public override bool OpenFile(NSApplication sender, string filename)
        {
            // Trap all errors
            try
            {
                filename = filename.Replace(" ", "%20");
                var url = new NSUrl("file://" + filename);
                //return OpenFile(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        

        public override void WillTerminate (NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}

