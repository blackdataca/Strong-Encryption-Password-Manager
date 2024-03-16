using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;   // Add reference to Microsoft.VisualBasic

namespace MyId;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program : WindowsFormsApplicationBase
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        var app = new Program();
        app.Run(args);

        
    }
    public Program()
    {
        this.IsSingleInstance = true;
        this.EnableVisualStyles = true;
        Application.EnableVisualStyles();
        //Application.SetCompatibleTextRenderingDefault(false);
        this.MainForm = new MainForm();
        
        //Application.Run(new MainForm());
    }


    protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
    {

        ((MainForm)this.MainForm).RestoreFromTray();
    }
   
}
