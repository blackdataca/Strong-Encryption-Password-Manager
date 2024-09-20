namespace MyId
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            uxList = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            uxContextNew = new System.Windows.Forms.ToolStripMenuItem();
            uxEdit = new System.Windows.Forms.ToolStripMenuItem();
            uxDelete = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            uxNewData = new System.Windows.Forms.ToolStripMenuItem();
            uxOpenData = new System.Windows.Forms.ToolStripMenuItem();
            uxExplorer = new System.Windows.Forms.ToolStripMenuItem();
            uxImport = new System.Windows.Forms.ToolStripMenuItem();
            uxExport = new System.Windows.Forms.ToolStripMenuItem();
            uxPrint = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            uxExportPrivateKey = new System.Windows.Forms.ToolStripMenuItem();
            importPrivateKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            uxChangeMasterPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            uxExit = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            uxAbout = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            uxNew = new System.Windows.Forms.ToolStripButton();
            uxToolEdit = new System.Windows.Forms.ToolStripButton();
            uxToolDelete = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            uxSearchBox = new System.Windows.Forms.ToolStripTextBox();
            uxClear = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            uxWebSync = new System.Windows.Forms.ToolStripButton();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            uxItemCountStatus = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            uxVersion = new System.Windows.Forms.ToolStripStatusLabel();
            uxTimeout = new System.Windows.Forms.ToolStripStatusLabel();
            timer1 = new System.Windows.Forms.Timer(components);
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            printDialog1 = new System.Windows.Forms.PrintDialog();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // uxList
            // 
            uxList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            uxList.ContextMenuStrip = contextMenuStrip1;
            uxList.Dock = System.Windows.Forms.DockStyle.Fill;
            uxList.FullRowSelect = true;
            uxList.Location = new System.Drawing.Point(0, 63);
            uxList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uxList.Name = "uxList";
            uxList.ShowItemToolTips = true;
            uxList.Size = new System.Drawing.Size(839, 434);
            uxList.TabIndex = 0;
            uxList.UseCompatibleStateImageBehavior = false;
            uxList.View = System.Windows.Forms.View.Details;
            uxList.ColumnClick += UxList_ColumnClick;
            uxList.SelectedIndexChanged += uxList_SelectedIndexChanged;
            uxList.DoubleClick += UxList_DoubleClick;
            uxList.KeyDown += UxList_KeyDown;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Site";
            columnHeader1.Width = 175;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "User";
            columnHeader2.Width = 103;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Password";
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Last Update";
            columnHeader4.Width = 113;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Memo";
            columnHeader5.Width = 227;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { uxContextNew, uxEdit, uxDelete });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            // 
            // uxContextNew
            // 
            uxContextNew.Image = Properties.Resources.tab_new_32x32;
            uxContextNew.Name = "uxContextNew";
            uxContextNew.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            uxContextNew.Size = new System.Drawing.Size(180, 22);
            uxContextNew.Text = "New";
            uxContextNew.Click += UxContextNew_Click;
            // 
            // uxEdit
            // 
            uxEdit.Enabled = false;
            uxEdit.Image = Properties.Resources.pencil_32x32;
            uxEdit.Name = "uxEdit";
            uxEdit.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E;
            uxEdit.Size = new System.Drawing.Size(180, 22);
            uxEdit.Text = "Edit";
            uxEdit.Click += UxEdit_Click;
            // 
            // uxDelete
            // 
            uxDelete.Enabled = false;
            uxDelete.Image = Properties.Resources.delete_32x32;
            uxDelete.Name = "uxDelete";
            uxDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            uxDelete.Size = new System.Drawing.Size(180, 22);
            uxDelete.Text = "Delete";
            uxDelete.Click += UxDelete_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(839, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { uxNewData, uxOpenData, uxExplorer, uxImport, uxExport, uxPrint, toolStripSeparator2, uxExportPrivateKey, importPrivateKeyToolStripMenuItem, uxChangeMasterPasswordToolStripMenuItem, toolStripMenuItem1, uxExit });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // uxNewData
            // 
            uxNewData.Name = "uxNewData";
            uxNewData.Size = new System.Drawing.Size(246, 22);
            uxNewData.Text = "&New Data File";
            uxNewData.Click += uxNewData_Click;
            // 
            // uxOpenData
            // 
            uxOpenData.Name = "uxOpenData";
            uxOpenData.Size = new System.Drawing.Size(246, 22);
            uxOpenData.Text = "&Open Data File...";
            uxOpenData.Click += UxOpenData_Click;
            // 
            // uxExplorer
            // 
            uxExplorer.Name = "uxExplorer";
            uxExplorer.Size = new System.Drawing.Size(246, 22);
            uxExplorer.Text = "Open Data Folder in File Explorer";
            uxExplorer.Click += UxExplorer_Click;
            // 
            // uxImport
            // 
            uxImport.Name = "uxImport";
            uxImport.Size = new System.Drawing.Size(246, 22);
            uxImport.Text = "&Import Data...";
            uxImport.Click += ImportToolStripMenuItem_Click;
            // 
            // uxExport
            // 
            uxExport.Name = "uxExport";
            uxExport.Size = new System.Drawing.Size(246, 22);
            uxExport.Text = "&Export Data...";
            uxExport.Visible = false;
            uxExport.Click += UxExport_Click;
            // 
            // uxPrint
            // 
            uxPrint.Name = "uxPrint";
            uxPrint.Size = new System.Drawing.Size(246, 22);
            uxPrint.Text = "&Print";
            uxPrint.Click += uxPrint_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(243, 6);
            // 
            // uxExportPrivateKey
            // 
            uxExportPrivateKey.Name = "uxExportPrivateKey";
            uxExportPrivateKey.Size = new System.Drawing.Size(246, 22);
            uxExportPrivateKey.Text = "Export &Private Key...";
            uxExportPrivateKey.Click += UxExportPrivateKey_Click;
            // 
            // importPrivateKeyToolStripMenuItem
            // 
            importPrivateKeyToolStripMenuItem.Name = "importPrivateKeyToolStripMenuItem";
            importPrivateKeyToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            importPrivateKeyToolStripMenuItem.Text = "Import Private &Key...";
            importPrivateKeyToolStripMenuItem.Click += ImportPrivateKeyToolStripMenuItem_Click;
            // 
            // uxChangeMasterPasswordToolStripMenuItem
            // 
            uxChangeMasterPasswordToolStripMenuItem.Name = "uxChangeMasterPasswordToolStripMenuItem";
            uxChangeMasterPasswordToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            uxChangeMasterPasswordToolStripMenuItem.Text = "C&hange Master Password...";
            uxChangeMasterPasswordToolStripMenuItem.Click += UxChangeMasterPasswordToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(243, 6);
            // 
            // uxExit
            // 
            uxExit.Name = "uxExit";
            uxExit.Size = new System.Drawing.Size(246, 22);
            uxExit.Text = "E&xit";
            uxExit.Click += UxExit_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripMenuItem, mnuEdit, mnuDelete });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Image = Properties.Resources.small_new_32x32;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            newToolStripMenuItem.Text = "&New...";
            // 
            // mnuEdit
            // 
            mnuEdit.Enabled = false;
            mnuEdit.Image = Properties.Resources.small_pencil_32x32;
            mnuEdit.Name = "mnuEdit";
            mnuEdit.Size = new System.Drawing.Size(180, 22);
            mnuEdit.Text = "&Edit";
            mnuEdit.Click += MnuEdit_Click;
            // 
            // mnuDelete
            // 
            mnuDelete.Enabled = false;
            mnuDelete.Image = Properties.Resources.small_delete_32x32;
            mnuDelete.Name = "mnuDelete";
            mnuDelete.Size = new System.Drawing.Size(180, 22);
            mnuDelete.Text = "&Delete";
            mnuDelete.Click += MnuDelete_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { uxAbout });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // uxAbout
            // 
            uxAbout.Name = "uxAbout";
            uxAbout.Size = new System.Drawing.Size(107, 22);
            uxAbout.Text = "&About";
            uxAbout.Click += UxAbout_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { uxNew, uxToolEdit, uxToolDelete, toolStripSeparator1, toolStripLabel1, uxSearchBox, uxClear, toolStripSeparator3, uxWebSync });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(839, 39);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // uxNew
            // 
            uxNew.Image = Properties.Resources.small_new_32x32;
            uxNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            uxNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            uxNew.Name = "uxNew";
            uxNew.Size = new System.Drawing.Size(67, 36);
            uxNew.Text = "&New";
            uxNew.ToolTipText = "New item";
            uxNew.Click += UxNew_Click;
            // 
            // uxToolEdit
            // 
            uxToolEdit.Enabled = false;
            uxToolEdit.Image = Properties.Resources.small_pencil_32x32;
            uxToolEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            uxToolEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            uxToolEdit.Name = "uxToolEdit";
            uxToolEdit.Size = new System.Drawing.Size(63, 36);
            uxToolEdit.Text = "&Edit";
            uxToolEdit.Click += UxToolEdit_Click;
            // 
            // uxToolDelete
            // 
            uxToolDelete.Enabled = false;
            uxToolDelete.Image = Properties.Resources.small_delete_32x32;
            uxToolDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            uxToolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            uxToolDelete.Name = "uxToolDelete";
            uxToolDelete.Size = new System.Drawing.Size(76, 36);
            uxToolDelete.Text = "&Delete";
            uxToolDelete.Click += UxToolDelete_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(33, 36);
            toolStripLabel1.Text = "&Find:";
            // 
            // uxSearchBox
            // 
            uxSearchBox.Name = "uxSearchBox";
            uxSearchBox.Size = new System.Drawing.Size(116, 39);
            uxSearchBox.ToolTipText = "Search";
            uxSearchBox.Click += UxSearchBox_Click;
            uxSearchBox.TextChanged += UxSearchBox_TextChanged;
            // 
            // uxClear
            // 
            uxClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            uxClear.Image = (System.Drawing.Image)resources.GetObject("uxClear.Image");
            uxClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            uxClear.Name = "uxClear";
            uxClear.Size = new System.Drawing.Size(38, 36);
            uxClear.Text = "&Clear";
            uxClear.ToolTipText = "Clear search";
            uxClear.Click += UxClear_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // uxWebSync
            // 
            uxWebSync.Image = (System.Drawing.Image)resources.GetObject("uxWebSync.Image");
            uxWebSync.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            uxWebSync.ImageTransparentColor = System.Drawing.Color.Magenta;
            uxWebSync.Name = "uxWebSync";
            uxWebSync.Size = new System.Drawing.Size(75, 36);
            uxWebSync.Text = "&Cloud";
            uxWebSync.ToolTipText = "Sync  to Cloud";
            uxWebSync.Click += uxToolSync_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { uxItemCountStatus, toolStripStatusLabel1, toolStripStatusLabel2, uxVersion, uxTimeout });
            statusStrip1.Location = new System.Drawing.Point(0, 497);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(839, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // uxItemCountStatus
            // 
            uxItemCountStatus.Image = Properties.Resources._lock;
            uxItemCountStatus.Name = "uxItemCountStatus";
            uxItemCountStatus.Size = new System.Drawing.Size(56, 17);
            uxItemCountStatus.Text = "0 item";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(574, 17);
            toolStripStatusLabel1.Spring = true;
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(108, 17);
            toolStripStatusLabel2.Text = "(C) 2019 Black Data";
            // 
            // uxVersion
            // 
            uxVersion.Name = "uxVersion";
            uxVersion.Size = new System.Drawing.Size(13, 17);
            uxVersion.Text = "v";
            // 
            // uxTimeout
            // 
            uxTimeout.Name = "uxTimeout";
            uxTimeout.Size = new System.Drawing.Size(71, 17);
            uxTimeout.Text = "Idle timeout";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            // 
            // printDocument1
            // 
            printDocument1.BeginPrint += printDocument1_BeginPrint;
            printDocument1.PrintPage += printDocument1_PrintPage;
            // 
            // printDialog1
            // 
            printDialog1.AllowSomePages = true;
            printDialog1.Document = printDocument1;
            printDialog1.UseEXDialog = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(839, 519);
            Controls.Add(uxList);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainForm";
            Text = "MyID Secured";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView uxList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton uxNew;
        private System.Windows.Forms.ToolStripTextBox uxSearchBox;
        private System.Windows.Forms.ToolStripButton uxClear;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel uxItemCountStatus;
        private System.Windows.Forms.ToolStripMenuItem uxImport;
        private System.Windows.Forms.ToolStripMenuItem uxExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem uxExit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem uxEdit;
        private System.Windows.Forms.ToolStripMenuItem uxDelete;
        private System.Windows.Forms.ToolStripMenuItem uxContextNew;
        private System.Windows.Forms.ToolStripMenuItem uxExportPrivateKey;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton uxToolEdit;
        private System.Windows.Forms.ToolStripButton uxToolDelete;
        private System.Windows.Forms.ToolStripMenuItem uxAbout;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel uxTimeout;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel uxVersion;
        private System.Windows.Forms.ToolStripMenuItem uxNewData;
        private System.Windows.Forms.ToolStripMenuItem uxOpenData;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem uxExplorer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem uxChangeMasterPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPrivateKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripMenuItem uxPrint;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.ToolStripButton uxWebSync;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

