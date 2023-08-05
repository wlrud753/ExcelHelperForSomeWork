namespace QuestForm
{
    partial class QuestForm
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
            this.CheckReacceptableButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CheckReacceptableButton
            // 
            this.CheckReacceptableButton.Location = new System.Drawing.Point(174, 127);
            this.CheckReacceptableButton.Name = "CheckReacceptableButton";
            this.CheckReacceptableButton.Size = new System.Drawing.Size(111, 39);
            this.CheckReacceptableButton.TabIndex = 0;
            this.CheckReacceptableButton.Text = "재수락 확인";
            this.CheckReacceptableButton.UseVisualStyleBackColor = true;
            this.CheckReacceptableButton.Click += new System.EventHandler(this.CheckReacceptableButton_Click);
            // 
            // QuestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CheckReacceptableButton);
            this.MaximizeBox = false;
            this.Name = "QuestForm";
            this.ShowIcon = false;
            this.Text = "Quest";
            this.Load += new System.EventHandler(this.QuestForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CheckReacceptableButton;
    }
}