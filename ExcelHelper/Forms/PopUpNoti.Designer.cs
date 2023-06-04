namespace PopUpNoti
{
    partial class PopUpNoti
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
            this.NotiTextLabel = new System.Windows.Forms.Label();
            this.ConfirmationButton = new System.Windows.Forms.Button();
            this.WorkCancelButton = new System.Windows.Forms.Button();
            this.WorkProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // NotiTextLabel
            // 
            this.NotiTextLabel.AutoSize = true;
            this.NotiTextLabel.Location = new System.Drawing.Point(67, 31);
            this.NotiTextLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NotiTextLabel.Name = "NotiTextLabel";
            this.NotiTextLabel.Size = new System.Drawing.Size(52, 12);
            this.NotiTextLabel.TabIndex = 0;
            this.NotiTextLabel.Text = "NotiText";
            this.NotiTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConfirmationButton
            // 
            this.ConfirmationButton.AutoSize = true;
            this.ConfirmationButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ConfirmationButton.Location = new System.Drawing.Point(44, 71);
            this.ConfirmationButton.Name = "ConfirmationButton";
            this.ConfirmationButton.Size = new System.Drawing.Size(75, 23);
            this.ConfirmationButton.TabIndex = 1;
            this.ConfirmationButton.Text = "확인";
            this.ConfirmationButton.UseVisualStyleBackColor = true;
            this.ConfirmationButton.Click += new System.EventHandler(this.ConfirmationButton_Click);
            // 
            // WorkCancelButton
            // 
            this.WorkCancelButton.AutoSize = true;
            this.WorkCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.WorkCancelButton.Location = new System.Drawing.Point(126, 71);
            this.WorkCancelButton.Name = "WorkCancelButton";
            this.WorkCancelButton.Size = new System.Drawing.Size(75, 23);
            this.WorkCancelButton.TabIndex = 2;
            this.WorkCancelButton.Text = "취소";
            this.WorkCancelButton.UseVisualStyleBackColor = true;
            this.WorkCancelButton.Click += new System.EventHandler(this.WorkCancelButton_Click);
            // 
            // WorkProgressBar
            // 
            this.WorkProgressBar.Location = new System.Drawing.Point(12, 46);
            this.WorkProgressBar.Name = "WorkProgressBar";
            this.WorkProgressBar.Size = new System.Drawing.Size(171, 23);
            this.WorkProgressBar.TabIndex = 3;
            // 
            // PopUpNoti
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(188, 116);
            this.Controls.Add(this.WorkProgressBar);
            this.Controls.Add(this.WorkCancelButton);
            this.Controls.Add(this.ConfirmationButton);
            this.Controls.Add(this.NotiTextLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopUpNoti";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PopUpNoti_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NotiTextLabel;
        private System.Windows.Forms.Button ConfirmationButton;
        private System.Windows.Forms.Button WorkCancelButton;
        private System.Windows.Forms.ProgressBar WorkProgressBar;
    }
}