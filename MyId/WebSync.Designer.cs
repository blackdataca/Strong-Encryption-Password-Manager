namespace MyId
{
    partial class WebSync
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
            label1 = new System.Windows.Forms.Label();
            uxEmail = new System.Windows.Forms.TextBox();
            uxPassword = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            uxCancel = new System.Windows.Forms.Button();
            uxOk = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 113);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Email:";
            // 
            // uxEmail
            // 
            uxEmail.Location = new System.Drawing.Point(86, 110);
            uxEmail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uxEmail.Name = "uxEmail";
            uxEmail.Size = new System.Drawing.Size(305, 23);
            uxEmail.TabIndex = 1;
            // 
            // uxPassword
            // 
            uxPassword.Location = new System.Drawing.Point(86, 156);
            uxPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uxPassword.Name = "uxPassword";
            uxPassword.PasswordChar = '●';
            uxPassword.Size = new System.Drawing.Size(305, 23);
            uxPassword.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 164);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 15);
            label2.TabIndex = 2;
            label2.Text = "Password:";
            // 
            // uxCancel
            // 
            uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            uxCancel.Location = new System.Drawing.Point(18, 322);
            uxCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uxCancel.Name = "uxCancel";
            uxCancel.Size = new System.Drawing.Size(88, 27);
            uxCancel.TabIndex = 4;
            uxCancel.Text = "Cancel";
            uxCancel.UseVisualStyleBackColor = true;
            // 
            // uxOk
            // 
            uxOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            uxOk.Location = new System.Drawing.Point(303, 322);
            uxOk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uxOk.Name = "uxOk";
            uxOk.Size = new System.Drawing.Size(88, 27);
            uxOk.TabIndex = 5;
            uxOk.Text = "Sync";
            uxOk.UseVisualStyleBackColor = true;
            uxOk.Click += uxOk_Click;
            // 
            // WebSync
            // 
            AcceptButton = uxOk;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = uxCancel;
            ClientSize = new System.Drawing.Size(419, 370);
            Controls.Add(uxOk);
            Controls.Add(uxCancel);
            Controls.Add(uxPassword);
            Controls.Add(label2);
            Controls.Add(uxEmail);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "WebSync";
            Text = "MyID Cloud";
            Load += WebSync_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox uxEmail;
        private System.Windows.Forms.TextBox uxPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button uxCancel;
        private System.Windows.Forms.Button uxOk;
    }
}