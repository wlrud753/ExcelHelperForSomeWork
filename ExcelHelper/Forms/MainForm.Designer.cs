namespace ExcelHelper
{
    partial class ExcelHelperMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.UpdateTableDataButton = new System.Windows.Forms.Button();
            this.MergeTablesButton = new System.Windows.Forms.Button();
            this.UpdataTableDataLabel = new System.Windows.Forms.Label();
            this.StreamSelectLabel = new System.Windows.Forms.Label();
            this.MergedTablesFunctionTipLabel = new System.Windows.Forms.Label();
            this.AdditionalFunctionLineLabel = new System.Windows.Forms.Label();
            this.SaveNotiLabel = new System.Windows.Forms.Label();
            this.SelectStreamComboBox = new System.Windows.Forms.ComboBox();
            this.SetStreamOptionButton = new System.Windows.Forms.Button();
            this.QuestDataButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UpdateTableDataButton
            // 
            this.UpdateTableDataButton.Location = new System.Drawing.Point(26, 47);
            this.UpdateTableDataButton.Name = "UpdateTableDataButton";
            this.UpdateTableDataButton.Size = new System.Drawing.Size(453, 56);
            this.UpdateTableDataButton.TabIndex = 3;
            this.UpdateTableDataButton.Text = "전체 테이블 확인";
            this.UpdateTableDataButton.UseVisualStyleBackColor = true;
            this.UpdateTableDataButton.Click += new System.EventHandler(this.UpdateTableDataButton_Click);
            // 
            // MergeTablesButton
            // 
            this.MergeTablesButton.Location = new System.Drawing.Point(28, 176);
            this.MergeTablesButton.Name = "MergeTablesButton";
            this.MergeTablesButton.Size = new System.Drawing.Size(185, 59);
            this.MergeTablesButton.TabIndex = 4;
            this.MergeTablesButton.Text = "MergeTables";
            this.MergeTablesButton.UseVisualStyleBackColor = true;
            this.MergeTablesButton.Click += new System.EventHandler(this.MergeTablesButton_Click);
            // 
            // UpdataTableDataLabel
            // 
            this.UpdataTableDataLabel.AutoSize = true;
            this.UpdataTableDataLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.UpdataTableDataLabel.ForeColor = System.Drawing.Color.Red;
            this.UpdataTableDataLabel.Location = new System.Drawing.Point(204, 106);
            this.UpdataTableDataLabel.Name = "UpdataTableDataLabel";
            this.UpdataTableDataLabel.Size = new System.Drawing.Size(275, 33);
            this.UpdataTableDataLabel.TabIndex = 5;
            this.UpdataTableDataLabel.Text = "※ Table이나 시트, 컬럼 추가 시 1회 수행해야 합니다\r\n안 그러면 프로그램이 터져요...\r\n※ ErDesignDataValidator 정도" +
    " 시간이 소요됩니다.";
            this.UpdataTableDataLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // StreamSelectLabel
            // 
            this.StreamSelectLabel.AutoSize = true;
            this.StreamSelectLabel.Location = new System.Drawing.Point(28, 24);
            this.StreamSelectLabel.Name = "StreamSelectLabel";
            this.StreamSelectLabel.Size = new System.Drawing.Size(53, 12);
            this.StreamSelectLabel.TabIndex = 7;
            this.StreamSelectLabel.Text = "Stream: ";
            // 
            // MergedTablesFunctionTipLabel
            // 
            this.MergedTablesFunctionTipLabel.AutoSize = true;
            this.MergedTablesFunctionTipLabel.Location = new System.Drawing.Point(35, 238);
            this.MergedTablesFunctionTipLabel.Name = "MergedTablesFunctionTipLabel";
            this.MergedTablesFunctionTipLabel.Size = new System.Drawing.Size(173, 12);
            this.MergedTablesFunctionTipLabel.TabIndex = 8;
            this.MergedTablesFunctionTipLabel.Text = "※분리된 테이블을 합치는 기능";
            this.MergedTablesFunctionTipLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AdditionalFunctionLineLabel
            // 
            this.AdditionalFunctionLineLabel.AutoSize = true;
            this.AdditionalFunctionLineLabel.Location = new System.Drawing.Point(23, 152);
            this.AdditionalFunctionLineLabel.Name = "AdditionalFunctionLineLabel";
            this.AdditionalFunctionLineLabel.Size = new System.Drawing.Size(461, 12);
            this.AdditionalFunctionLineLabel.TabIndex = 9;
            this.AdditionalFunctionLineLabel.Text = "------------------------------------기능------------------------------------";
            // 
            // SaveNotiLabel
            // 
            this.SaveNotiLabel.AutoSize = true;
            this.SaveNotiLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SaveNotiLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SaveNotiLabel.Location = new System.Drawing.Point(247, 375);
            this.SaveNotiLabel.Name = "SaveNotiLabel";
            this.SaveNotiLabel.Size = new System.Drawing.Size(232, 11);
            this.SaveNotiLabel.TabIndex = 10;
            this.SaveNotiLabel.Text = "관련한 모든 데이터는 \'내 문서\'에 저장됩니다.";
            this.SaveNotiLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SelectStreamComboBox
            // 
            this.SelectStreamComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectStreamComboBox.FormattingEnabled = true;
            this.SelectStreamComboBox.Location = new System.Drawing.Point(87, 19);
            this.SelectStreamComboBox.Name = "SelectStreamComboBox";
            this.SelectStreamComboBox.Size = new System.Drawing.Size(126, 20);
            this.SelectStreamComboBox.TabIndex = 11;
            this.SelectStreamComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectStreamComboBox_SelectedIndexChanged);
            // 
            // SetStreamOptionButton
            // 
            this.SetStreamOptionButton.Location = new System.Drawing.Point(375, 19);
            this.SetStreamOptionButton.Name = "SetStreamOptionButton";
            this.SetStreamOptionButton.Size = new System.Drawing.Size(104, 23);
            this.SetStreamOptionButton.TabIndex = 12;
            this.SetStreamOptionButton.Text = "스트림 설정";
            this.SetStreamOptionButton.UseVisualStyleBackColor = true;
            this.SetStreamOptionButton.Click += new System.EventHandler(this.SetStreamOptionButton_Click);
            // 
            // QuestDataButton
            // 
            this.QuestDataButton.Location = new System.Drawing.Point(294, 176);
            this.QuestDataButton.Name = "QuestDataButton";
            this.QuestDataButton.Size = new System.Drawing.Size(185, 59);
            this.QuestDataButton.TabIndex = 13;
            this.QuestDataButton.Text = "Quest";
            this.QuestDataButton.UseVisualStyleBackColor = true;
            // 
            // ExcelHelperMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 395);
            this.Controls.Add(this.QuestDataButton);
            this.Controls.Add(this.SetStreamOptionButton);
            this.Controls.Add(this.SelectStreamComboBox);
            this.Controls.Add(this.SaveNotiLabel);
            this.Controls.Add(this.AdditionalFunctionLineLabel);
            this.Controls.Add(this.MergedTablesFunctionTipLabel);
            this.Controls.Add(this.StreamSelectLabel);
            this.Controls.Add(this.UpdataTableDataLabel);
            this.Controls.Add(this.MergeTablesButton);
            this.Controls.Add(this.UpdateTableDataButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ExcelHelperMain";
            this.Text = "ExcelHelper";
            this.Load += new System.EventHandler(this.ExcelHelperMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button UpdateTableDataButton;
        private System.Windows.Forms.Button MergeTablesButton;
        private System.Windows.Forms.Label UpdataTableDataLabel;
        private System.Windows.Forms.Label StreamSelectLabel;
        private System.Windows.Forms.Label MergedTablesFunctionTipLabel;
        private System.Windows.Forms.Label AdditionalFunctionLineLabel;
        private System.Windows.Forms.Label SaveNotiLabel;
        private System.Windows.Forms.ComboBox SelectStreamComboBox;
        private System.Windows.Forms.Button SetStreamOptionButton;
        private System.Windows.Forms.Button QuestDataButton;
    }
}

