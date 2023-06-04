namespace MergeForm
{
    partial class MergeForm
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
            this.DataListBox = new System.Windows.Forms.ListBox();
            this.DataListLabel = new System.Windows.Forms.Label();
            this.FileListBox = new System.Windows.Forms.ListBox();
            this.FilleListLabel = new System.Windows.Forms.Label();
            this.MergeButton = new System.Windows.Forms.Button();
            this.SavePathNotiLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DataListBox
            // 
            this.DataListBox.FormattingEnabled = true;
            this.DataListBox.ItemHeight = 12;
            this.DataListBox.Location = new System.Drawing.Point(25, 32);
            this.DataListBox.Name = "DataListBox";
            this.DataListBox.Size = new System.Drawing.Size(152, 364);
            this.DataListBox.Sorted = true;
            this.DataListBox.TabIndex = 0;
            this.DataListBox.SelectedIndexChanged += new System.EventHandler(this.DataListBox_SelectedIndexChanged);
            // 
            // DataListLabel
            // 
            this.DataListLabel.AutoSize = true;
            this.DataListLabel.Location = new System.Drawing.Point(23, 17);
            this.DataListLabel.Name = "DataListLabel";
            this.DataListLabel.Size = new System.Drawing.Size(97, 12);
            this.DataListLabel.TabIndex = 1;
            this.DataListLabel.Text = "합칠 테이블 선택";
            // 
            // FileListBox
            // 
            this.FileListBox.FormattingEnabled = true;
            this.FileListBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.FileListBox.ItemHeight = 12;
            this.FileListBox.Location = new System.Drawing.Point(195, 32);
            this.FileListBox.Name = "FileListBox";
            this.FileListBox.Size = new System.Drawing.Size(372, 364);
            this.FileListBox.Sorted = true;
            this.FileListBox.TabIndex = 2;
            // 
            // FilleListLabel
            // 
            this.FilleListLabel.AutoSize = true;
            this.FilleListLabel.Location = new System.Drawing.Point(193, 17);
            this.FilleListLabel.Name = "FilleListLabel";
            this.FilleListLabel.Size = new System.Drawing.Size(149, 12);
            this.FilleListLabel.TabIndex = 3;
            this.FilleListLabel.Text = "분리돼 있는 테이블 리스트";
            // 
            // MergeButton
            // 
            this.MergeButton.Location = new System.Drawing.Point(25, 403);
            this.MergeButton.Name = "MergeButton";
            this.MergeButton.Size = new System.Drawing.Size(542, 35);
            this.MergeButton.TabIndex = 4;
            this.MergeButton.Text = "Start Merge";
            this.MergeButton.UseVisualStyleBackColor = true;
            this.MergeButton.Click += new System.EventHandler(this.MergeButton_Click);
            // 
            // SavePathNotiLabel
            // 
            this.SavePathNotiLabel.AutoSize = true;
            this.SavePathNotiLabel.Location = new System.Drawing.Point(94, 441);
            this.SavePathNotiLabel.Name = "SavePathNotiLabel";
            this.SavePathNotiLabel.Size = new System.Drawing.Size(408, 12);
            this.SavePathNotiLabel.TabIndex = 5;
            this.SavePathNotiLabel.Text = "합쳐진 테이블은 실행 프로그램 폴더의 \'MergedTable\' 폴더에 저장됩니다.";
            // 
            // MergeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 469);
            this.Controls.Add(this.SavePathNotiLabel);
            this.Controls.Add(this.MergeButton);
            this.Controls.Add(this.FilleListLabel);
            this.Controls.Add(this.FileListBox);
            this.Controls.Add(this.DataListLabel);
            this.Controls.Add(this.DataListBox);
            this.MaximizeBox = false;
            this.Name = "MergeForm";
            this.ShowIcon = false;
            this.Text = "Merge";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox DataListBox;
        private System.Windows.Forms.Label DataListLabel;
        private System.Windows.Forms.ListBox FileListBox;
        private System.Windows.Forms.Label FilleListLabel;
        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.Label SavePathNotiLabel;
    }
}