namespace MyId
{
    partial class SignIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignIn));
            uxCancel = new System.Windows.Forms.Button();
            uxUnlock = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            uxPassword = new System.Windows.Forms.TextBox();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // uxCancel
            // 
            uxCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            uxCancel.Location = new System.Drawing.Point(341, 143);
            uxCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uxCancel.Name = "uxCancel";
            uxCancel.Size = new System.Drawing.Size(88, 27);
            uxCancel.TabIndex = 3;
            uxCancel.Text = "&Cancel";
            uxCancel.UseVisualStyleBackColor = true;
            uxCancel.Click += uxCancel_Click;
            // 
            // uxUnlock
            // 
            uxUnlock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            uxUnlock.DialogResult = System.Windows.Forms.DialogResult.OK;
            uxUnlock.Location = new System.Drawing.Point(214, 143);
            uxUnlock.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uxUnlock.Name = "uxUnlock";
            uxUnlock.Size = new System.Drawing.Size(88, 27);
            uxUnlock.TabIndex = 2;
            uxUnlock.Text = "&Unlock";
            uxUnlock.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(31, 47);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(65, 15);
            label1.TabIndex = 0;
            label1.Text = "&Master PIN";
            // 
            // uxPassword
            // 
            uxPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            uxPassword.Location = new System.Drawing.Point(35, 82);
            uxPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uxPassword.Name = "uxPassword";
            uxPassword.PasswordChar = '*';
            uxPassword.Size = new System.Drawing.Size(392, 23);
            uxPassword.TabIndex = 1;
            uxPassword.UseSystemPasswordChar = true;
            uxPassword.TextChanged += UxPassword_TextChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(486, 24);
            menuStrip1.TabIndex = 5;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newDataFileToolStripMenuItem, openDataFileToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newDataFileToolStripMenuItem
            // 
            newDataFileToolStripMenuItem.Name = "newDataFileToolStripMenuItem";
            newDataFileToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            newDataFileToolStripMenuItem.Text = "&New Data File";
            newDataFileToolStripMenuItem.Click += newDataFileToolStripMenuItem_Click;
            // 
            // openDataFileToolStripMenuItem
            // 
            openDataFileToolStripMenuItem.Name = "openDataFileToolStripMenuItem";
            openDataFileToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            openDataFileToolStripMenuItem.Text = "&Open Data File";
            openDataFileToolStripMenuItem.Click += openDataFileToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            aboutToolStripMenuItem.Text = "&About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // SignIn
            // 
            AcceptButton = uxUnlock;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = uxCancel;
            ClientSize = new System.Drawing.Size(486, 196);
            Controls.Add(uxPassword);
            Controls.Add(label1);
            Controls.Add(uxUnlock);
            Controls.Add(uxCancel);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "SignIn";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "SignIn";
            Load += SignIn_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button uxCancel;
        public System.Windows.Forms.TextBox uxPassword;
        public System.Windows.Forms.Button uxUnlock;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDataFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDataFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}