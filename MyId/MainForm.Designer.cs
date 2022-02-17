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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.uxList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uxContextNew = new System.Windows.Forms.ToolStripMenuItem();
            this.uxEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.uxDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxNewData = new System.Windows.Forms.ToolStripMenuItem();
            this.uxOpenData = new System.Windows.Forms.ToolStripMenuItem();
            this.uxExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.uxImport = new System.Windows.Forms.ToolStripMenuItem();
            this.uxExport = new System.Windows.Forms.ToolStripMenuItem();
            this.uxPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.uxExportPrivateKey = new System.Windows.Forms.ToolStripMenuItem();
            this.importPrivateKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxChangeMasterPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.uxExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.uxNew = new System.Windows.Forms.ToolStripButton();
            this.uxToolEdit = new System.Windows.Forms.ToolStripButton();
            this.uxToolDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.uxSearchBox = new System.Windows.Forms.ToolStripTextBox();
            this.uxClear = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.uxItemCountStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.uxVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.uxTimeout = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxList
            // 
            this.uxList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.uxList.ContextMenuStrip = this.contextMenuStrip1;
            this.uxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxList.FullRowSelect = true;
            this.uxList.HideSelection = false;
            this.uxList.Location = new System.Drawing.Point(0, 63);
            this.uxList.Name = "uxList";
            this.uxList.ShowItemToolTips = true;
            this.uxList.Size = new System.Drawing.Size(719, 365);
            this.uxList.TabIndex = 0;
            this.uxList.UseCompatibleStateImageBehavior = false;
            this.uxList.View = System.Windows.Forms.View.Details;
            this.uxList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.UxList_ColumnClick);
            this.uxList.DoubleClick += new System.EventHandler(this.UxList_DoubleClick);
            this.uxList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UxList_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Site";
            this.columnHeader1.Width = 175;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "User";
            this.columnHeader2.Width = 103;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Password";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Last Update";
            this.columnHeader4.Width = 113;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Memo";
            this.columnHeader5.Width = 227;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxContextNew,
            this.uxEdit,
            this.uxDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(142, 70);
            // 
            // uxContextNew
            // 
            this.uxContextNew.Image = global::MyId.Properties.Resources.tab_new_32x32;
            this.uxContextNew.Name = "uxContextNew";
            this.uxContextNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.uxContextNew.Size = new System.Drawing.Size(141, 22);
            this.uxContextNew.Text = "New";
            this.uxContextNew.Click += new System.EventHandler(this.UxContextNew_Click);
            // 
            // uxEdit
            // 
            this.uxEdit.Image = global::MyId.Properties.Resources.pencil_32x32;
            this.uxEdit.Name = "uxEdit";
            this.uxEdit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.uxEdit.Size = new System.Drawing.Size(141, 22);
            this.uxEdit.Text = "Edit";
            this.uxEdit.Click += new System.EventHandler(this.UxEdit_Click);
            // 
            // uxDelete
            // 
            this.uxDelete.Image = global::MyId.Properties.Resources.delete_32x32;
            this.uxDelete.Name = "uxDelete";
            this.uxDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.uxDelete.Size = new System.Drawing.Size(141, 22);
            this.uxDelete.Text = "Delete";
            this.uxDelete.Click += new System.EventHandler(this.UxDelete_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(719, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxNewData,
            this.uxOpenData,
            this.uxExplorer,
            this.uxImport,
            this.uxExport,
            this.uxPrint,
            this.toolStripSeparator2,
            this.uxExportPrivateKey,
            this.importPrivateKeyToolStripMenuItem,
            this.uxChangeMasterPasswordToolStripMenuItem,
            this.toolStripMenuItem1,
            this.uxExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // uxNewData
            // 
            this.uxNewData.Name = "uxNewData";
            this.uxNewData.Size = new System.Drawing.Size(246, 22);
            this.uxNewData.Text = "&New Data File";
            this.uxNewData.Click += new System.EventHandler(this.uxNewData_Click);
            // 
            // uxOpenData
            // 
            this.uxOpenData.Name = "uxOpenData";
            this.uxOpenData.Size = new System.Drawing.Size(246, 22);
            this.uxOpenData.Text = "&Open Data File...";
            this.uxOpenData.Click += new System.EventHandler(this.UxOpenData_Click);
            // 
            // uxExplorer
            // 
            this.uxExplorer.Name = "uxExplorer";
            this.uxExplorer.Size = new System.Drawing.Size(246, 22);
            this.uxExplorer.Text = "Open Data Folder in File Explorer";
            this.uxExplorer.Click += new System.EventHandler(this.UxExplorer_Click);
            // 
            // uxImport
            // 
            this.uxImport.Name = "uxImport";
            this.uxImport.Size = new System.Drawing.Size(246, 22);
            this.uxImport.Text = "&Import...";
            this.uxImport.Click += new System.EventHandler(this.ImportToolStripMenuItem_Click);
            // 
            // uxExport
            // 
            this.uxExport.Name = "uxExport";
            this.uxExport.Size = new System.Drawing.Size(246, 22);
            this.uxExport.Text = "&Export...";
            this.uxExport.Visible = false;
            this.uxExport.Click += new System.EventHandler(this.UxExport_Click);
            // 
            // uxPrint
            // 
            this.uxPrint.Name = "uxPrint";
            this.uxPrint.Size = new System.Drawing.Size(246, 22);
            this.uxPrint.Text = "&Print";
            this.uxPrint.Click += new System.EventHandler(this.uxPrint_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(243, 6);
            // 
            // uxExportPrivateKey
            // 
            this.uxExportPrivateKey.Name = "uxExportPrivateKey";
            this.uxExportPrivateKey.Size = new System.Drawing.Size(246, 22);
            this.uxExportPrivateKey.Text = "Backup &Private Key...";
            this.uxExportPrivateKey.Click += new System.EventHandler(this.UxExportPrivateKey_Click);
            // 
            // importPrivateKeyToolStripMenuItem
            // 
            this.importPrivateKeyToolStripMenuItem.Name = "importPrivateKeyToolStripMenuItem";
            this.importPrivateKeyToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.importPrivateKeyToolStripMenuItem.Text = "Restore Private &Key...";
            this.importPrivateKeyToolStripMenuItem.Click += new System.EventHandler(this.ImportPrivateKeyToolStripMenuItem_Click);
            // 
            // uxChangeMasterPasswordToolStripMenuItem
            // 
            this.uxChangeMasterPasswordToolStripMenuItem.Name = "uxChangeMasterPasswordToolStripMenuItem";
            this.uxChangeMasterPasswordToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.uxChangeMasterPasswordToolStripMenuItem.Text = "C&hange Master Password...";
            this.uxChangeMasterPasswordToolStripMenuItem.Click += new System.EventHandler(this.UxChangeMasterPasswordToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(243, 6);
            // 
            // uxExit
            // 
            this.uxExit.Name = "uxExit";
            this.uxExit.Size = new System.Drawing.Size(246, 22);
            this.uxExit.Text = "E&xit";
            this.uxExit.Click += new System.EventHandler(this.UxExit_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.mnuEdit,
            this.mnuDelete});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::MyId.Properties.Resources.small_new_32x32;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItem.Text = "&New...";
            // 
            // mnuEdit
            // 
            this.mnuEdit.Image = global::MyId.Properties.Resources.small_pencil_32x32;
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(107, 22);
            this.mnuEdit.Text = "&Edit";
            this.mnuEdit.Click += new System.EventHandler(this.MnuEdit_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Image = global::MyId.Properties.Resources.small_delete_32x32;
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(107, 22);
            this.mnuDelete.Text = "&Delete";
            this.mnuDelete.Click += new System.EventHandler(this.MnuDelete_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // uxAbout
            // 
            this.uxAbout.Name = "uxAbout";
            this.uxAbout.Size = new System.Drawing.Size(107, 22);
            this.uxAbout.Text = "&About";
            this.uxAbout.Click += new System.EventHandler(this.UxAbout_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxNew,
            this.uxToolEdit,
            this.uxToolDelete,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.uxSearchBox,
            this.uxClear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(719, 39);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // uxNew
            // 
            this.uxNew.Image = global::MyId.Properties.Resources.small_new_32x32;
            this.uxNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.uxNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxNew.Name = "uxNew";
            this.uxNew.Size = new System.Drawing.Size(67, 36);
            this.uxNew.Text = "&New";
            this.uxNew.ToolTipText = "New item";
            this.uxNew.Click += new System.EventHandler(this.UxNew_Click);
            // 
            // uxToolEdit
            // 
            this.uxToolEdit.Image = global::MyId.Properties.Resources.small_pencil_32x32;
            this.uxToolEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.uxToolEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxToolEdit.Name = "uxToolEdit";
            this.uxToolEdit.Size = new System.Drawing.Size(63, 36);
            this.uxToolEdit.Text = "&Edit";
            this.uxToolEdit.Click += new System.EventHandler(this.UxToolEdit_Click);
            // 
            // uxToolDelete
            // 
            this.uxToolDelete.Image = global::MyId.Properties.Resources.small_delete_32x32;
            this.uxToolDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.uxToolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxToolDelete.Name = "uxToolDelete";
            this.uxToolDelete.Size = new System.Drawing.Size(76, 36);
            this.uxToolDelete.Text = "&Delete";
            this.uxToolDelete.Click += new System.EventHandler(this.UxToolDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(78, 36);
            this.toolStripLabel1.Text = "&Quick search:";
            // 
            // uxSearchBox
            // 
            this.uxSearchBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uxSearchBox.Name = "uxSearchBox";
            this.uxSearchBox.Size = new System.Drawing.Size(100, 39);
            this.uxSearchBox.ToolTipText = "Search";
            this.uxSearchBox.Click += new System.EventHandler(this.UxSearchBox_Click);
            this.uxSearchBox.TextChanged += new System.EventHandler(this.UxSearchBox_TextChanged);
            // 
            // uxClear
            // 
            this.uxClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uxClear.Image = ((System.Drawing.Image)(resources.GetObject("uxClear.Image")));
            this.uxClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxClear.Name = "uxClear";
            this.uxClear.Size = new System.Drawing.Size(38, 36);
            this.uxClear.Text = "&Clear";
            this.uxClear.ToolTipText = "Clear search";
            this.uxClear.Click += new System.EventHandler(this.UxClear_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxItemCountStatus,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.uxVersion,
            this.uxTimeout});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(719, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // uxItemCountStatus
            // 
            this.uxItemCountStatus.Image = global::MyId.Properties.Resources._lock;
            this.uxItemCountStatus.Name = "uxItemCountStatus";
            this.uxItemCountStatus.Size = new System.Drawing.Size(56, 17);
            this.uxItemCountStatus.Text = "0 item";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(456, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(108, 17);
            this.toolStripStatusLabel2.Text = "(C) 2019 Black Data";
            // 
            // uxVersion
            // 
            this.uxVersion.Name = "uxVersion";
            this.uxVersion.Size = new System.Drawing.Size(13, 17);
            this.uxVersion.Text = "v";
            // 
            // uxTimeout
            // 
            this.uxTimeout.Name = "uxTimeout";
            this.uxTimeout.Size = new System.Drawing.Size(71, 17);
            this.uxTimeout.Text = "Idle timeout";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // printDocument1
            // 
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 450);
            this.Controls.Add(this.uxList);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MyID Secured";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

