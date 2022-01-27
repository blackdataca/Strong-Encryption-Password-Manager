namespace MyId
{
    partial class Welcome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Welcome));
            this.label1 = new System.Windows.Forms.Label();
            this.uxMasterPin = new System.Windows.Forms.TextBox();
            this.uxVerify = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uxOk = new System.Windows.Forms.Button();
            this.uxCancel = new System.Windows.Forms.Button();
            this.uxCreateNewIn = new System.Windows.Forms.Label();
            this.uxOther = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.uxPrivateKeyPath = new System.Windows.Forms.TextBox();
            this.uxBrowsePrivateKey = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.uxSavePrivateKeyThisComputer = new System.Windows.Forms.RadioButton();
            this.uxSavePrivateKeyTo = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uxBrowseDataFile = new System.Windows.Forms.Button();
            this.uxDataFilePath = new System.Windows.Forms.TextBox();
            this.uxDataFileFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.uxPrivateFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&New master PIN";
            // 
            // uxMasterPassword
            // 
            this.uxMasterPin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxMasterPin.Location = new System.Drawing.Point(152, 238);
            this.uxMasterPin.Name = "uxMasterPassword";
            this.uxMasterPin.PasswordChar = '*';
            this.uxMasterPin.Size = new System.Drawing.Size(293, 20);
            this.uxMasterPin.TabIndex = 1;
            this.uxMasterPin.UseSystemPasswordChar = true;
            // 
            // uxVerify
            // 
            this.uxVerify.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxVerify.Location = new System.Drawing.Point(152, 273);
            this.uxVerify.Name = "uxVerify";
            this.uxVerify.PasswordChar = '*';
            this.uxVerify.Size = new System.Drawing.Size(293, 20);
            this.uxVerify.TabIndex = 3;
            this.uxVerify.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 276);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Verify new PIN";
            // 
            // uxOk
            // 
            this.uxOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxOk.Location = new System.Drawing.Point(261, 392);
            this.uxOk.Name = "uxOk";
            this.uxOk.Size = new System.Drawing.Size(75, 23);
            this.uxOk.TabIndex = 4;
            this.uxOk.Text = "&OK";
            this.uxOk.UseVisualStyleBackColor = true;
            this.uxOk.Click += new System.EventHandler(this.UxOk_Click);
            // 
            // uxCancel
            // 
            this.uxCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uxCancel.Location = new System.Drawing.Point(370, 392);
            this.uxCancel.Name = "uxCancel";
            this.uxCancel.Size = new System.Drawing.Size(75, 23);
            this.uxCancel.TabIndex = 5;
            this.uxCancel.Text = "&Cancel";
            this.uxCancel.UseVisualStyleBackColor = true;
            // 
            // uxCreateNewIn
            // 
            this.uxCreateNewIn.AutoSize = true;
            this.uxCreateNewIn.Location = new System.Drawing.Point(26, 363);
            this.uxCreateNewIn.Name = "uxCreateNewIn";
            this.uxCreateNewIn.Size = new System.Drawing.Size(117, 13);
            this.uxCreateNewIn.TabIndex = 6;
            this.uxCreateNewIn.Text = "Create new data files in";
            // 
            // uxOther
            // 
            this.uxOther.Location = new System.Drawing.Point(29, 392);
            this.uxOther.Name = "uxOther";
            this.uxOther.Size = new System.Drawing.Size(183, 23);
            this.uxOther.TabIndex = 7;
            this.uxOther.Text = "Open existing data file...";
            this.uxOther.UseVisualStyleBackColor = true;
            this.uxOther.Click += new System.EventHandler(this.uxOther_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(26, 197);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(419, 33);
            this.label3.TabIndex = 8;
            this.label3.Text = "Create private key encrypted data files. Only accessible by Personal Identificati" +
    "on Number (PIN)";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // uxPrivateKeyPath
            // 
            this.uxPrivateKeyPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxPrivateKeyPath.Location = new System.Drawing.Point(152, 331);
            this.uxPrivateKeyPath.Name = "uxPrivateKeyPath";
            this.uxPrivateKeyPath.Size = new System.Drawing.Size(252, 20);
            this.uxPrivateKeyPath.TabIndex = 10;
            // 
            // uxBrowsePrivateKey
            // 
            this.uxBrowsePrivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxBrowsePrivateKey.Location = new System.Drawing.Point(410, 330);
            this.uxBrowsePrivateKey.Name = "uxBrowsePrivateKey";
            this.uxBrowsePrivateKey.Size = new System.Drawing.Size(35, 23);
            this.uxBrowsePrivateKey.TabIndex = 11;
            this.uxBrowsePrivateKey.Text = "...";
            this.uxBrowsePrivateKey.UseVisualStyleBackColor = true;
            this.uxBrowsePrivateKey.Click += new System.EventHandler(this.uxBrowsePrivateKey_Click);
            // 
            // uxSavePrivateKeyThisComputer
            // 
            this.uxSavePrivateKeyThisComputer.AutoSize = true;
            this.uxSavePrivateKeyThisComputer.Checked = true;
            this.uxSavePrivateKeyThisComputer.Location = new System.Drawing.Point(29, 306);
            this.uxSavePrivateKeyThisComputer.Name = "uxSavePrivateKeyThisComputer";
            this.uxSavePrivateKeyThisComputer.Size = new System.Drawing.Size(186, 17);
            this.uxSavePrivateKeyThisComputer.TabIndex = 14;
            this.uxSavePrivateKeyThisComputer.TabStop = true;
            this.uxSavePrivateKeyThisComputer.Text = "Save private key on this computer";
            this.uxSavePrivateKeyThisComputer.UseVisualStyleBackColor = true;
            // 
            // uxSavePrivateKeyTo
            // 
            this.uxSavePrivateKeyTo.AutoSize = true;
            this.uxSavePrivateKeyTo.Location = new System.Drawing.Point(29, 333);
            this.uxSavePrivateKeyTo.Name = "uxSavePrivateKeyTo";
            this.uxSavePrivateKeyTo.Size = new System.Drawing.Size(117, 17);
            this.uxSavePrivateKeyTo.TabIndex = 15;
            this.uxSavePrivateKeyTo.Text = "Save private key to";
            this.uxSavePrivateKeyTo.UseVisualStyleBackColor = true;
            this.uxSavePrivateKeyTo.CheckedChanged += new System.EventHandler(this.uxSavePrivateKeyTo_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = global::MyId.Properties.Resources.MyId_Infograph_drawio__1_;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(29, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(416, 178);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // uxBrowseDataFile
            // 
            this.uxBrowseDataFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxBrowseDataFile.Location = new System.Drawing.Point(410, 359);
            this.uxBrowseDataFile.Name = "uxBrowseDataFile";
            this.uxBrowseDataFile.Size = new System.Drawing.Size(35, 23);
            this.uxBrowseDataFile.TabIndex = 17;
            this.uxBrowseDataFile.Text = "...";
            this.uxBrowseDataFile.UseVisualStyleBackColor = true;
            this.uxBrowseDataFile.Click += new System.EventHandler(this.uxBrowseDataFile_Click);
            // 
            // uxDataFilePath
            // 
            this.uxDataFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxDataFilePath.Location = new System.Drawing.Point(152, 360);
            this.uxDataFilePath.Name = "uxDataFilePath";
            this.uxDataFilePath.Size = new System.Drawing.Size(252, 20);
            this.uxDataFilePath.TabIndex = 16;
            // 
            // Welcome
            // 
            this.AcceptButton = this.uxOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.uxCancel;
            this.ClientSize = new System.Drawing.Size(468, 430);
            this.Controls.Add(this.uxBrowseDataFile);
            this.Controls.Add(this.uxDataFilePath);
            this.Controls.Add(this.uxSavePrivateKeyTo);
            this.Controls.Add(this.uxSavePrivateKeyThisComputer);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.uxBrowsePrivateKey);
            this.Controls.Add(this.uxPrivateKeyPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uxOther);
            this.Controls.Add(this.uxCreateNewIn);
            this.Controls.Add(this.uxCancel);
            this.Controls.Add(this.uxOk);
            this.Controls.Add(this.uxVerify);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.uxMasterPin);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Welcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Welcome to MyId ";
            this.Load += new System.EventHandler(this.CreateNewMaster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox uxMasterPin;
        private System.Windows.Forms.TextBox uxVerify;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button uxOk;
        private System.Windows.Forms.Button uxCancel;
        private System.Windows.Forms.Label uxCreateNewIn;
        private System.Windows.Forms.Button uxOther;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox uxPrivateKeyPath;
        private System.Windows.Forms.Button uxBrowsePrivateKey;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton uxSavePrivateKeyThisComputer;
        private System.Windows.Forms.RadioButton uxSavePrivateKeyTo;
        private System.Windows.Forms.Button uxBrowseDataFile;
        private System.Windows.Forms.TextBox uxDataFilePath;
        private System.Windows.Forms.FolderBrowserDialog uxDataFileFolderBrowser;
        private System.Windows.Forms.FolderBrowserDialog uxPrivateFolderBrowser;
    }
}