namespace MyId
{
    partial class Edit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Edit));
            this.label1 = new System.Windows.Forms.Label();
            this.uxSite = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uxUser = new System.Windows.Forms.TextBox();
            this.uxMemo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.uxPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.uxOK = new System.Windows.Forms.Button();
            this.uxCancel = new System.Windows.Forms.Button();
            this.uxGenerate = new System.Windows.Forms.Button();
            this.uxCopyPassword = new System.Windows.Forms.Button();
            this.uxCopyUser = new System.Windows.Forms.Button();
            this.uxHint = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.uxImageAdd = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.uxImages = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.uxDelete = new System.Windows.Forms.Button();
            this.uxView = new System.Windows.Forms.Button();
            this.uxViewPass = new System.Windows.Forms.Button();
            this.uxGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Site";
            // 
            // uxSite
            // 
            this.uxSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSite.Location = new System.Drawing.Point(66, 12);
            this.uxSite.Name = "uxSite";
            this.uxSite.Size = new System.Drawing.Size(280, 20);
            this.uxSite.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&User";
            // 
            // uxUser
            // 
            this.uxUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxUser.Location = new System.Drawing.Point(66, 45);
            this.uxUser.Name = "uxUser";
            this.uxUser.Size = new System.Drawing.Size(280, 20);
            this.uxUser.TabIndex = 3;
            // 
            // uxMemo
            // 
            this.uxMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxMemo.Location = new System.Drawing.Point(66, 159);
            this.uxMemo.Multiline = true;
            this.uxMemo.Name = "uxMemo";
            this.uxMemo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.uxMemo.Size = new System.Drawing.Size(361, 72);
            this.uxMemo.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "&Memo";
            // 
            // uxPassword
            // 
            this.uxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxPassword.Location = new System.Drawing.Point(66, 86);
            this.uxPassword.Name = "uxPassword";
            this.uxPassword.Size = new System.Drawing.Size(199, 20);
            this.uxPassword.TabIndex = 6;
            this.uxPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "&Password";
            // 
            // uxOK
            // 
            this.uxOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.uxOK.Location = new System.Drawing.Point(226, 342);
            this.uxOK.Name = "uxOK";
            this.uxOK.Size = new System.Drawing.Size(75, 23);
            this.uxOK.TabIndex = 18;
            this.uxOK.Text = "&OK";
            this.uxOK.UseVisualStyleBackColor = true;
            this.uxOK.Click += new System.EventHandler(this.UxOK_Click);
            // 
            // uxCancel
            // 
            this.uxCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uxCancel.Location = new System.Drawing.Point(352, 342);
            this.uxCancel.Name = "uxCancel";
            this.uxCancel.Size = new System.Drawing.Size(75, 23);
            this.uxCancel.TabIndex = 19;
            this.uxCancel.Text = "&Cancel";
            this.uxCancel.UseVisualStyleBackColor = true;
            // 
            // uxGenerate
            // 
            this.uxGenerate.Location = new System.Drawing.Point(66, 122);
            this.uxGenerate.Name = "uxGenerate";
            this.uxGenerate.Size = new System.Drawing.Size(159, 23);
            this.uxGenerate.TabIndex = 9;
            this.uxGenerate.Text = "Generate strong password";
            this.uxGenerate.UseVisualStyleBackColor = true;
            this.uxGenerate.Click += new System.EventHandler(this.UxGenerate_Click);
            // 
            // uxCopyPassword
            // 
            this.uxCopyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCopyPassword.Location = new System.Drawing.Point(352, 85);
            this.uxCopyPassword.Name = "uxCopyPassword";
            this.uxCopyPassword.Size = new System.Drawing.Size(75, 23);
            this.uxCopyPassword.TabIndex = 8;
            this.uxCopyPassword.Text = "Copy";
            this.uxCopyPassword.UseVisualStyleBackColor = true;
            this.uxCopyPassword.Click += new System.EventHandler(this.UxCopyPassword_Click);
            // 
            // uxCopyUser
            // 
            this.uxCopyUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCopyUser.Location = new System.Drawing.Point(352, 44);
            this.uxCopyUser.Name = "uxCopyUser";
            this.uxCopyUser.Size = new System.Drawing.Size(75, 23);
            this.uxCopyUser.TabIndex = 4;
            this.uxCopyUser.Text = "Copy";
            this.uxCopyUser.UseVisualStyleBackColor = true;
            this.uxCopyUser.Click += new System.EventHandler(this.UxCopyUser_Click);
            // 
            // uxHint
            // 
            this.uxHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxHint.Location = new System.Drawing.Point(232, 114);
            this.uxHint.Name = "uxHint";
            this.uxHint.Size = new System.Drawing.Size(195, 42);
            this.uxHint.TabIndex = 10;
            this.uxHint.Text = "uxHint";
            this.uxHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 252);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Files";
            // 
            // uxImageAdd
            // 
            this.uxImageAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxImageAdd.Location = new System.Drawing.Point(353, 252);
            this.uxImageAdd.Name = "uxImageAdd";
            this.uxImageAdd.Size = new System.Drawing.Size(75, 23);
            this.uxImageAdd.TabIndex = 15;
            this.uxImageAdd.Text = "Add...";
            this.uxImageAdd.UseVisualStyleBackColor = true;
            this.uxImageAdd.Click += new System.EventHandler(this.UxImageAdd_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Select files";
            this.openFileDialog1.Filter = "Any file (*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // uxImages
            // 
            this.uxImages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxImages.HideSelection = false;
            this.uxImages.LargeImageList = this.imageList1;
            this.uxImages.Location = new System.Drawing.Point(66, 252);
            this.uxImages.Name = "uxImages";
            this.uxImages.Size = new System.Drawing.Size(280, 84);
            this.uxImages.TabIndex = 14;
            this.uxImages.UseCompatibleStateImageBehavior = false;
            this.uxImages.SelectedIndexChanged += new System.EventHandler(this.UxImages_SelectedIndexChanged);
            this.uxImages.DoubleClick += new System.EventHandler(this.UxImages_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(64, 64);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // uxDelete
            // 
            this.uxDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxDelete.Enabled = false;
            this.uxDelete.Location = new System.Drawing.Point(353, 313);
            this.uxDelete.Name = "uxDelete";
            this.uxDelete.Size = new System.Drawing.Size(75, 23);
            this.uxDelete.TabIndex = 17;
            this.uxDelete.Text = "Delete";
            this.uxDelete.UseVisualStyleBackColor = true;
            this.uxDelete.Click += new System.EventHandler(this.UxDelete_Click);
            // 
            // uxView
            // 
            this.uxView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxView.Enabled = false;
            this.uxView.Location = new System.Drawing.Point(353, 282);
            this.uxView.Name = "uxView";
            this.uxView.Size = new System.Drawing.Size(75, 23);
            this.uxView.TabIndex = 16;
            this.uxView.Text = "View";
            this.uxView.UseVisualStyleBackColor = true;
            this.uxView.Click += new System.EventHandler(this.UxView_Click);
            // 
            // uxViewPass
            // 
            this.uxViewPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxViewPass.Location = new System.Drawing.Point(271, 85);
            this.uxViewPass.Name = "uxViewPass";
            this.uxViewPass.Size = new System.Drawing.Size(75, 23);
            this.uxViewPass.TabIndex = 7;
            this.uxViewPass.Text = "&View";
            this.uxViewPass.UseVisualStyleBackColor = true;
            this.uxViewPass.Click += new System.EventHandler(this.UxViewPass_Click);
            this.uxViewPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UxViewPass_KeyDown);
            this.uxViewPass.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UxViewPass_KeyUp);
            this.uxViewPass.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UxViewPass_MouseDown);
            this.uxViewPass.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UxViewPass_MouseUp);
            // 
            // uxGo
            // 
            this.uxGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxGo.Location = new System.Drawing.Point(353, 12);
            this.uxGo.Name = "uxGo";
            this.uxGo.Size = new System.Drawing.Size(75, 23);
            this.uxGo.TabIndex = 20;
            this.uxGo.Text = "&Go";
            this.uxGo.UseVisualStyleBackColor = true;
            this.uxGo.Click += new System.EventHandler(this.UxGo_Click);
            // 
            // Edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.uxCancel;
            this.ClientSize = new System.Drawing.Size(447, 377);
            this.Controls.Add(this.uxGo);
            this.Controls.Add(this.uxViewPass);
            this.Controls.Add(this.uxView);
            this.Controls.Add(this.uxDelete);
            this.Controls.Add(this.uxImages);
            this.Controls.Add(this.uxImageAdd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.uxHint);
            this.Controls.Add(this.uxCopyUser);
            this.Controls.Add(this.uxCopyPassword);
            this.Controls.Add(this.uxGenerate);
            this.Controls.Add(this.uxCancel);
            this.Controls.Add(this.uxOK);
            this.Controls.Add(this.uxMemo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uxPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.uxUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.uxSite);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Edit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Edit_FormClosing);
            this.Load += new System.EventHandler(this.Edit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox uxSite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox uxUser;
        private System.Windows.Forms.TextBox uxMemo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox uxPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button uxOK;
        private System.Windows.Forms.Button uxCancel;
        private System.Windows.Forms.Button uxGenerate;
        private System.Windows.Forms.Button uxCopyPassword;
        private System.Windows.Forms.Button uxCopyUser;
        private System.Windows.Forms.Label uxHint;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button uxImageAdd;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.ListView uxImages;
        private System.Windows.Forms.Button uxDelete;
        private System.Windows.Forms.Button uxView;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button uxViewPass;
        private System.Windows.Forms.Button uxGo;
    }
}