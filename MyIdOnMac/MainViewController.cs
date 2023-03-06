// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyIdOnMac
{
	public partial class MainViewController : NSViewController
	{

        private UxListDataSource _dataSource = new UxListDataSource();

        public MainViewController (IntPtr handle) : base (handle)
		{
		}

        private string IdFile
        {
            get
            {
                if (KnownFolders.DataFile != "")
                    return KnownFolders.DataFile;
                else
                    return Path.Combine(KnownFolders.DataDir, "myid_secret.data");
            }
            set
            {
                KnownFolders.DataFile = value;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.

           

        }
        public override void ViewDidAppear()
        {
            base.ViewDidAppear();

            if (uxList.Delegate != null)
                return;
            // Create the Product Table Data Source and populate it
            _dataSource.Add(new IdItem("Xamarin.iOS", "User", "Password", "Allows you to develop native iOS Applications in C#"));
            _dataSource.Add(new IdItem("Xamarin.Android", "User", "Password", "Allows you to develop native Android Applications in C#"));
            _dataSource.Add(new IdItem("Xamarin.Mac", "User", "Password", "Allows you to develop Mac native Applications in C#"));

            // Populate the Product Table
            uxList.DataSource = _dataSource;
            uxList.Delegate = new UxListDelegate(_dataSource);

            if (System.IO.File.Exists(IdFile))
            {

                //TODO load file
                // ...
            }
            else
            {
                if (!CreateNewFile())
                    System.Environment.Exit(1);  //First time app run
            }

        }

        private bool CreateNewFile()
        {
            if (KnownFolders.DataFile != "")
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Warning,
                    InformativeText = "You will lose access to existing data file if private key is not backed up. Only click Yes if you are 100% sure private key has been backed up or you no longer need existing data file. Otherwise click No.",
                    MessageText = "Backup private key",
                };
                alert.AddButton("Yes");
                alert.AddButton("No");
                
                var result = alert.RunModal();
                if (result != 1000)
                    return false;
            }


            PerformSegue("WelcomeSegue", this);

            return true;

            
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            

        }

        [Action("add:")]
        public void Add(NSObject sender)
        {
            // Preform some action when the menu is selected
            Console.WriteLine("Request to add");
            PerformSegue("EditSegue", this);

        }

        public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            // Take action based on the segue name
            switch (segue.Identifier)
            {
                case "EditSegue":
                    {
                        var dialog = segue.DestinationController as EditController;
                        //dialog.DialogTitle = "MacDialog";
                        //dialog.DialogDescription = "This is a sample dialog.";
                        dialog.DialogAccepted += (s, e) =>
                        {
                            Console.WriteLine("Dialog accepted");
                            _dataSource.Add(dialog.AIdItem);
                            uxList.ReloadData();
                            _dataSource.SaveToDisk();
                        };
                        dialog.Presentor = this;
                        break;
                    }
                case "WelcomeSegue":
                    {
                        var dialog = segue.DestinationController as WelcomeController;

                        dialog.Presentor = this;
                        
                        dialog.DialogOk += (s, e) =>
                        {
                            
                            IdFile = dialog.DataFile;
                            byte[] masterPin = Encoding.Unicode.GetBytes(dialog.MasterPin);
                            

                            _dataSource.SaveToDisk(masterPin);
                        };

                        dialog.DialogOpen += (s, e) =>
                        {
                            //if (OpenDataFile())
                            //{
                            //    return true;
                            //}
                        };

                        dialog.DialogCanceled += (s, e) =>
                        {
                            System.Environment.Exit(1);
                        };
                        break;                        
                    }
            }
        }

        

        public void DeleteItem()
        {
            var rows = uxList.SelectedRows;
            uxList.RemoveRows(rows, NSTableViewAnimation.Fade);
        }

        public nint ItemCount
        {
            get
            {
                return uxList.RowCount;
            }
        }


    }
}
