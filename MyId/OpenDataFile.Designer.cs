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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.uxPrivateKeyThisComputer = new System.Windows.Forms.RadioButton();
            this.uxPriviateKeyOn = new System.Windows.Forms.RadioButton();
            this.uxPrivateKeyPath = new System.Windows.Forms.TextBox();
            this.uxBrowsePrivateKey = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.uxOk = new System.Windows.Forms.Button();
            this.uxCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data file";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(115, 46);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(342, 20);
            this.textBox1.TabIndex = 1;
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
            this.uxPrivateKeyThisComputer.Location = new System.Drawing.Point(47, 106);
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
            this.uxPriviateKeyOn.Location = new System.Drawing.Point(47, 156);
            this.uxPriviateKeyOn.Name = "uxPriviateKeyOn";
            this.uxPriviateKeyOn.Size = new System.Drawing.Size(114, 17);
            this.uxPriviateKeyOn.TabIndex = 4;
            this.uxPriviateKeyOn.TabStop = true;
            this.uxPriviateKeyOn.Text = "Use private key on";
            this.uxPriviateKeyOn.UseVisualStyleBackColor = true;
            this.uxPriviateKeyOn.CheckedChanged += new System.EventHandler(this.uxPriviateKeyOn_CheckedChanged);
            // 
            // uxPrivateKeyPath
            // 
            this.uxPrivateKeyPath.Location = new System.Drawing.Point(115, 194);
            this.uxPrivateKeyPath.Name = "uxPrivateKeyPath";
            this.uxPrivateKeyPath.Size = new System.Drawing.Size(342, 20);
            this.uxPrivateKeyPath.TabIndex = 5;
            // 
            // uxBrowsePrivateKey
            // 
            this.uxBrowsePrivateKey.Location = new System.Drawing.Point(463, 191);
            this.uxBrowsePrivateKey.Name = "uxBrowsePrivateKey";
            this.uxBrowsePrivateKey.Size = new System.Drawing.Size(30, 23);
            this.uxBrowsePrivateKey.TabIndex = 6;
            this.uxBrowsePrivateKey.Text = "...";
            this.uxBrowsePrivateKey.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 260);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "PIN";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(115, 253);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(342, 20);
            this.textBox3.TabIndex = 8;
            // 
            // uxOk
            // 
            this.uxOk.Location = new System.Drawing.Point(316, 316);
            this.uxOk.Name = "uxOk";
            this.uxOk.Size = new System.Drawing.Size(75, 23);
            this.uxOk.TabIndex = 9;
            this.uxOk.Text = "&OK";
            this.uxOk.UseVisualStyleBackColor = true;
            // 
            // uxCancel
            // 
            this.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uxCancel.Location = new System.Drawing.Point(422, 316);
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
            this.ClientSize = new System.Drawing.Size(535, 367);
            this.Controls.Add(this.uxCancel);
            this.Controls.Add(this.uxOk);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.uxBrowsePrivateKey);
            this.Controls.Add(this.uxPrivateKeyPath);
            this.Controls.Add(this.uxPriviateKeyOn);
            this.Controls.Add(this.uxPrivateKeyThisComputer);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton uxPrivateKeyThisComputer;
        private System.Windows.Forms.RadioButton uxPriviateKeyOn;
        private System.Windows.Forms.TextBox uxPrivateKeyPath;
        private System.Windows.Forms.Button uxBrowsePrivateKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button uxOk;
        private System.Windows.Forms.Button uxCancel;
    }
}