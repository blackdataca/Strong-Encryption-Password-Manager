// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using GameKit;

namespace MyIdOnMac
{
	public partial class WelcomeController : NSViewController
	{
        public string DataFile { get { return this.uxDataFile.StringValue; } }

        public string MasterPin { get { return this.uxMasterPin.StringValue; } }

        public WelcomeController (IntPtr handle) : base (handle)
		{
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            string file = System.IO.Path.Combine(KnownFolders.DataDir, "myid_secret.data");
            uxDataFile.StringValue = file;
        }

     

        public NSViewController Presentor;

        private void CloseDialog()
        {
            Presentor.DismissViewController(this);
        }

        partial void OkDialog(NSObject sender)
        {
            if (System.IO.File.Exists(uxDataFile.StringValue))
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Warning,
                    InformativeText = $"Data file already exists: {uxDataFile.StringValue}",
                    MessageText = "Create new data file",
                };
                alert.RunModal();
                return;
            }
            RaiseDialogOk();
            CloseDialog();
        }

        

        public EventHandler DialogOk;
        internal void RaiseDialogOk()
        {
            if (this.DialogOk != null)
            {
                this.DialogOk(this, EventArgs.Empty);
            }
        }

        partial void CancelDialog(NSObject sender)
        {
            RaiseDialogCancelled();
            CloseDialog();
        }
        public EventHandler DialogCanceled;
        internal void RaiseDialogCancelled()
        {
            if (this.DialogCanceled != null)
                this.DialogCanceled(this, EventArgs.Empty);
        }

        partial void OpenDialog(NSObject sender)
        {
            RaiseDialogOpen();
            CloseDialog();
        }
        public EventHandler DialogOpen;
        internal void RaiseDialogOpen()
        {
            if (this.DialogOpen != null)
                this.DialogOpen(this, EventArgs.Empty);
        }

        partial void BrowseNewDataFile(NSObject sender)
        {
            var panel = new NSSavePanel();
            //panel.FloatingPanel = true;
            panel.CanCreateDirectories = true;
            panel.ReleasedWhenClosed = true;

            panel.Title = "Create new MyId secret file";
            //panel.NameFieldLabel = "File name:";
            panel.NameFieldStringValue = System.IO.Path.GetFileName(uxDataFile.StringValue);
           // panel.Prompt = "Create";

            panel.Directory = KnownFolders.DataDir;
            panel.AllowedFileTypes = new string[] { "data" };
            if (!string.IsNullOrWhiteSpace(uxDataFile.StringValue))
            {
                string dir = System.IO.Path.GetDirectoryName(uxDataFile.StringValue);
                if (System.IO.Directory.Exists(dir))
                    panel.Directory = dir;
            }
            panel.BeginSheet(this.View.Window, ret =>
            {
                if (ret == 1 && !string.IsNullOrWhiteSpace(panel.Filename))
                {
                    uxDataFile.StringValue = panel.Filename;

                }
            });
        
            
        }
    }
}