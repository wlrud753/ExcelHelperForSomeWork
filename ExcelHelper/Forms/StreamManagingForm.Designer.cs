namespace StreamManagingForm
{
    partial class StreamManagingForm
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
            this.AddStreamTextBox = new System.Windows.Forms.TextBox();
            this.AddStreamPathTextBox = new System.Windows.Forms.TextBox();
            this.AddStreamDataButton = new System.Windows.Forms.Button();
            this.SaveStreamDataButton = new System.Windows.Forms.Button();
            this.StreamNameList = new System.Windows.Forms.ListBox();
            this.StreamPathList = new System.Windows.Forms.ListBox();
            this.DeleteStreamDataButton = new System.Windows.Forms.Button();
            this.AddStreamNameLabel = new System.Windows.Forms.Label();
            this.StreamTablePathLabel = new System.Windows.Forms.Label();
            this.StreamListLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AddStreamTextBox
            // 
            this.AddStreamTextBox.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AddStreamTextBox.Location = new System.Drawing.Point(27, 37);
            this.AddStreamTextBox.Name = "AddStreamTextBox";
            this.AddStreamTextBox.Size = new System.Drawing.Size(109, 21);
            this.AddStreamTextBox.TabIndex = 0;
            this.AddStreamTextBox.Text = "StreamName";
            this.AddStreamTextBox.GotFocus += new System.EventHandler(this.AddStreamTextBox_GotFocus);
            this.AddStreamTextBox.LostFocus += new System.EventHandler(this.AddStreamTextBox_LostFocus);
            // 
            // AddStreamPathTextBox
            // 
            this.AddStreamPathTextBox.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AddStreamPathTextBox.Location = new System.Drawing.Point(145, 37);
            this.AddStreamPathTextBox.Name = "AddStreamPathTextBox";
            this.AddStreamPathTextBox.Size = new System.Drawing.Size(353, 21);
            this.AddStreamPathTextBox.TabIndex = 1;
            this.AddStreamPathTextBox.Text = "TablePath";
            this.AddStreamPathTextBox.GotFocus += new System.EventHandler(this.AddStreamPathTextBox_GotFocus);
            this.AddStreamPathTextBox.LostFocus += new System.EventHandler(this.AddStreamPathTextBox_LostFocus);
            // 
            // AddStreamDataButton
            // 
            this.AddStreamDataButton.Location = new System.Drawing.Point(403, 64);
            this.AddStreamDataButton.Name = "AddStreamDataButton";
            this.AddStreamDataButton.Size = new System.Drawing.Size(95, 23);
            this.AddStreamDataButton.TabIndex = 2;
            this.AddStreamDataButton.TabStop = false;
            this.AddStreamDataButton.Text = "추가";
            this.AddStreamDataButton.UseVisualStyleBackColor = true;
            this.AddStreamDataButton.Click += new System.EventHandler(this.AddStreamDataButton_Click);
            // 
            // SaveStreamDataButton
            // 
            this.SaveStreamDataButton.Location = new System.Drawing.Point(27, 335);
            this.SaveStreamDataButton.Name = "SaveStreamDataButton";
            this.SaveStreamDataButton.Size = new System.Drawing.Size(472, 41);
            this.SaveStreamDataButton.TabIndex = 3;
            this.SaveStreamDataButton.TabStop = false;
            this.SaveStreamDataButton.Text = "저장하기";
            this.SaveStreamDataButton.UseVisualStyleBackColor = true;
            this.SaveStreamDataButton.Click += new System.EventHandler(this.SaveStreamDataButton_Click);
            // 
            // StreamNameList
            // 
            this.StreamNameList.FormattingEnabled = true;
            this.StreamNameList.ItemHeight = 12;
            this.StreamNameList.Location = new System.Drawing.Point(28, 104);
            this.StreamNameList.Name = "StreamNameList";
            this.StreamNameList.Size = new System.Drawing.Size(109, 196);
            this.StreamNameList.TabIndex = 4;
            this.StreamNameList.TabStop = false;
            // 
            // StreamPathList
            // 
            this.StreamPathList.FormattingEnabled = true;
            this.StreamPathList.ItemHeight = 12;
            this.StreamPathList.Location = new System.Drawing.Point(147, 104);
            this.StreamPathList.Name = "StreamPathList";
            this.StreamPathList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.StreamPathList.Size = new System.Drawing.Size(353, 196);
            this.StreamPathList.TabIndex = 5;
            this.StreamPathList.TabStop = false;
            // 
            // DeleteStreamDataButton
            // 
            this.DeleteStreamDataButton.Location = new System.Drawing.Point(406, 306);
            this.DeleteStreamDataButton.Name = "DeleteStreamDataButton";
            this.DeleteStreamDataButton.Size = new System.Drawing.Size(94, 23);
            this.DeleteStreamDataButton.TabIndex = 6;
            this.DeleteStreamDataButton.TabStop = false;
            this.DeleteStreamDataButton.Text = "스트림 삭제";
            this.DeleteStreamDataButton.UseVisualStyleBackColor = true;
            this.DeleteStreamDataButton.Click += new System.EventHandler(this.DeleteStreamDataButton_Click);
            // 
            // AddStreamNameLabel
            // 
            this.AddStreamNameLabel.AutoSize = true;
            this.AddStreamNameLabel.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AddStreamNameLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.AddStreamNameLabel.Location = new System.Drawing.Point(27, 22);
            this.AddStreamNameLabel.Name = "AddStreamNameLabel";
            this.AddStreamNameLabel.Size = new System.Drawing.Size(101, 11);
            this.AddStreamNameLabel.TabIndex = 7;
            this.AddStreamNameLabel.Text = "추가할 스트림 이름";
            // 
            // StreamTablePathLabel
            // 
            this.StreamTablePathLabel.AutoSize = true;
            this.StreamTablePathLabel.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.StreamTablePathLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.StreamTablePathLabel.Location = new System.Drawing.Point(146, 22);
            this.StreamTablePathLabel.Name = "StreamTablePathLabel";
            this.StreamTablePathLabel.Size = new System.Drawing.Size(311, 11);
            this.StreamTablePathLabel.TabIndex = 8;
            this.StreamTablePathLabel.Text = "스트림의 Table 경로 (GameDesign\\DesignData\\Table)";
            // 
            // StreamListLabel
            // 
            this.StreamListLabel.AutoSize = true;
            this.StreamListLabel.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.StreamListLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.StreamListLabel.Location = new System.Drawing.Point(30, 86);
            this.StreamListLabel.Name = "StreamListLabel";
            this.StreamListLabel.Size = new System.Drawing.Size(64, 11);
            this.StreamListLabel.TabIndex = 9;
            this.StreamListLabel.Text = "스트림 목록";
            // 
            // StreamManagingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 398);
            this.Controls.Add(this.StreamListLabel);
            this.Controls.Add(this.StreamTablePathLabel);
            this.Controls.Add(this.AddStreamNameLabel);
            this.Controls.Add(this.DeleteStreamDataButton);
            this.Controls.Add(this.StreamPathList);
            this.Controls.Add(this.StreamNameList);
            this.Controls.Add(this.SaveStreamDataButton);
            this.Controls.Add(this.AddStreamDataButton);
            this.Controls.Add(this.AddStreamPathTextBox);
            this.Controls.Add(this.AddStreamTextBox);
            this.MaximizeBox = false;
            this.Name = "StreamManagingForm";
            this.Text = "StreamManager";
            this.Load += new System.EventHandler(this.StreamManagingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AddStreamTextBox;
        private System.Windows.Forms.TextBox AddStreamPathTextBox;
        private System.Windows.Forms.Button AddStreamDataButton;
        private System.Windows.Forms.Button SaveStreamDataButton;
        private System.Windows.Forms.ListBox StreamNameList;
        private System.Windows.Forms.ListBox StreamPathList;
        private System.Windows.Forms.Button DeleteStreamDataButton;
        private System.Windows.Forms.Label AddStreamNameLabel;
        private System.Windows.Forms.Label StreamTablePathLabel;
        private System.Windows.Forms.Label StreamListLabel;
    }
}