namespace MyId
{
    partial class OpenDataFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenDataFile));
            this.label1 = new System.Windows.Forms.Label();
            this.uxDataFileDir = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.uxPrivateKeyThisComputer = new System.Windows.Forms.RadioButton();
            this.uxPriviateKeyOn = new System.Windows.Forms.RadioButton();
            this.uxPrivateKeyPath = new System.Windows.Forms.TextBox();
            this.uxBrowsePrivateKey = new System.Windows.Forms.Button();
            this.uxOk = new System.Windows.Forms.Button();
            this.uxCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data file";
            // 
            // uxDataFileDir
            // 
            this.uxDataFileDir.Location = new System.Drawing.Point(115, 46);
            this.uxDataFileDir.Name = "uxDataFileDir";
            this.uxDataFileDir.Size = new System.Drawing.Size(342, 20);
            this.uxDataFileDir.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(463, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // uxPrivateKeyThisComputer
            // 
            this.uxPrivateKeyThisComputer.AutoSize = true;
            this.uxPrivateKeyThisComputer.Location = new System.Drawing.Point(27, 106);
            this.uxPrivateKeyThisComputer.Name = "uxPrivateKeyThisComputer";
            this.uxPrivateKeyThisComputer.Size = new System.Drawing.Size(180, 17);
            this.uxPrivateKeyThisComputer.TabIndex = 3;
            this.uxPrivateKeyThisComputer.Text = "Use private key on this computer";
            this.uxPrivateKeyThisComputer.UseVisualStyleBackColor = true;
            // 
            // uxPriviateKeyOn
            // 
            this.uxPriviateKeyOn.AutoSize = true;
            this.uxPriviateKeyOn.Checked = true;
            this.uxPriviateKeyOn.Location = new System.Drawing.Point(27, 156);
            this.uxPriviateKeyOn.Name = "uxPriviateKeyOn";
            this.uxPriviateKeyOn.Size = new System.Drawing.Size(94, 17);
            this.uxPriviateKeyOn.TabIndex = 4;
            this.uxPriviateKeyOn.TabStop = true;
            this.uxPriviateKeyOn.Text = "Private key file";
            this.uxPriviateKeyOn.UseVisualStyleBackColor = true;
            this.uxPriviateKeyOn.CheckedChanged += new System.EventHandler(this.uxPriviateKeyOn_CheckedChanged);
            // 
            // uxPrivateKeyPath
            // 
            this.uxPrivateKeyPath.Location = new System.Drawing.Point(160, 156);
            this.uxPrivateKeyPath.Name = "uxPrivateKeyPath";
            this.uxPrivateKeyPath.Size = new System.Drawing.Size(297, 20);
            this.uxPrivateKeyPath.TabIndex = 5;
            // 
            // uxBrowsePrivateKey
            // 
            this.uxBrowsePrivateKey.Location = new System.Drawing.Point(463, 156);
            this.uxBrowsePrivateKey.Name = "uxBrowsePrivateKey";
            this.uxBrowsePrivateKey.Size = new System.Drawing.Size(30, 23);
            this.uxBrowsePrivateKey.TabIndex = 6;
            this.uxBrowsePrivateKey.Text = "...";
            this.uxBrowsePrivateKey.UseVisualStyleBackColor = true;
            // 
            // uxOk
            // 
            this.uxOk.Location = new System.Drawing.Point(312, 236);
            this.uxOk.Name = "uxOk";
            this.uxOk.Size = new System.Drawing.Size(75, 23);
            this.uxOk.TabIndex = 9;
            this.uxOk.Text = "&OK";
            this.uxOk.UseVisualStyleBackColor = true;
            this.uxOk.Click += new System.EventHandler(this.uxOk_Click);
            // 
            // uxCancel
            // 
            this.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uxCancel.Location = new System.Drawing.Point(418, 236);
            this.uxCancel.Name = "uxCancel";
            this.uxCancel.Size = new System.Drawing.Size(75, 23);
            this.uxCancel.TabIndex = 10;
            this.uxCancel.Text = "&Cancel";
            this.uxCancel.UseVisualStyleBackColor = true;
            // 
            // OpenDataFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 289);
            this.Controls.Add(this.uxCancel);
            this.Controls.Add(this.uxOk);
            this.Controls.Add(this.uxBrowsePrivateKey);
            this.Controls.Add(this.uxPrivateKeyPath);
            this.Controls.Add(this.uxPriviateKeyOn);
            this.Controls.Add(this.uxPrivateKeyThisComputer);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.uxDataFileDir);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OpenDataFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open existing data file";
            this.Load += new System.EventHandler(this.OpenDataFile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton uxPrivateKeyThisComputer;
        private System.Windows.Forms.Button uxBrowsePrivateKey;
        private System.Windows.Forms.Button uxOk;
        private System.Windows.Forms.Button uxCancel;
        public System.Windows.Forms.TextBox uxDataFileDir;
        public System.Windows.Forms.RadioButton uxPriviateKeyOn;
        public System.Windows.Forms.TextBox uxPrivateKeyPath;
    }
}