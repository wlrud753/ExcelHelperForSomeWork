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
            this.SuspendLayout();
            // 
            // AddStreamTextBox
            // 
            this.AddStreamTextBox.Location = new System.Drawing.Point(48, 44);
            this.AddStreamTextBox.Name = "AddStreamTextBox";
            this.AddStreamTextBox.Size = new System.Drawing.Size(100, 21);
            this.AddStreamTextBox.TabIndex = 0;
            this.AddStreamTextBox.Text = "Stream 이름";
            // 
            // AddStreamPathTextBox
            // 
            this.AddStreamPathTextBox.Location = new System.Drawing.Point(167, 44);
            this.AddStreamPathTextBox.Name = "AddStreamPathTextBox";
            this.AddStreamPathTextBox.Size = new System.Drawing.Size(353, 21);
            this.AddStreamPathTextBox.TabIndex = 1;
            this.AddStreamPathTextBox.Text = "Stream의 Table 경로";
            // 
            // AddStreamDataButton
            // 
            this.AddStreamDataButton.Location = new System.Drawing.Point(537, 42);
            this.AddStreamDataButton.Name = "AddStreamDataButton";
            this.AddStreamDataButton.Size = new System.Drawing.Size(75, 23);
            this.AddStreamDataButton.TabIndex = 2;
            this.AddStreamDataButton.Text = "추가";
            this.AddStreamDataButton.UseVisualStyleBackColor = true;
            this.AddStreamDataButton.Click += new System.EventHandler(this.AddStreamDataButton_Click);
            // 
            // SaveStreamDataButton
            // 
            this.SaveStreamDataButton.Location = new System.Drawing.Point(48, 381);
            this.SaveStreamDataButton.Name = "SaveStreamDataButton";
            this.SaveStreamDataButton.Size = new System.Drawing.Size(564, 41);
            this.SaveStreamDataButton.TabIndex = 3;
            this.SaveStreamDataButton.Text = "저장하기";
            this.SaveStreamDataButton.UseVisualStyleBackColor = true;
            // 
            // StreamNameList
            // 
            this.StreamNameList.FormattingEnabled = true;
            this.StreamNameList.ItemHeight = 12;
            this.StreamNameList.Location = new System.Drawing.Point(48, 95);
            this.StreamNameList.Name = "StreamNameList";
            this.StreamNameList.Size = new System.Drawing.Size(100, 256);
            this.StreamNameList.TabIndex = 4;
            // 
            // StreamPathList
            // 
            this.StreamPathList.FormattingEnabled = true;
            this.StreamPathList.ItemHeight = 12;
            this.StreamPathList.Location = new System.Drawing.Point(167, 95);
            this.StreamPathList.Name = "StreamPathList";
            this.StreamPathList.Size = new System.Drawing.Size(353, 256);
            this.StreamPathList.TabIndex = 5;
            // 
            // StreamManagingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 450);
            this.Controls.Add(this.StreamPathList);
            this.Controls.Add(this.StreamNameList);
            this.Controls.Add(this.SaveStreamDataButton);
            this.Controls.Add(this.AddStreamDataButton);
            this.Controls.Add(this.AddStreamPathTextBox);
            this.Controls.Add(this.AddStreamTextBox);
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
    }
}