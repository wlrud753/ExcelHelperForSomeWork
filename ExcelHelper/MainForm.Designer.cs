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
            this.TablePathInputTextBox = new System.Windows.Forms.TextBox();
            this.TablePathLabel = new System.Windows.Forms.Label();
            this.MergedTablesFunctionTipLabel = new System.Windows.Forms.Label();
            this.AdditionalFunctionLineLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UpdateTableDataButton
            // 
            this.UpdateTableDataButton.Location = new System.Drawing.Point(28, 12);
            this.UpdateTableDataButton.Name = "UpdateTableDataButton";
            this.UpdateTableDataButton.Size = new System.Drawing.Size(453, 56);
            this.UpdateTableDataButton.TabIndex = 3;
            this.UpdateTableDataButton.Text = "전체 테이블 확인";
            this.UpdateTableDataButton.UseVisualStyleBackColor = true;
            this.UpdateTableDataButton.Click += new System.EventHandler(this.UpdateTableDataButton_Click);
            // 
            // MergeTablesButton
            // 
            this.MergeTablesButton.Location = new System.Drawing.Point(28, 165);
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
            this.UpdataTableDataLabel.Location = new System.Drawing.Point(204, 102);
            this.UpdataTableDataLabel.Name = "UpdataTableDataLabel";
            this.UpdataTableDataLabel.Size = new System.Drawing.Size(275, 22);
            this.UpdataTableDataLabel.TabIndex = 5;
            this.UpdataTableDataLabel.Text = "※ Table이나 시트, 컬럼 추가 시 1회 수행해야 합니다\r\n안 그러면 프로그램이 터져요...";
            this.UpdataTableDataLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TablePathInputTextBox
            // 
            this.TablePathInputTextBox.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TablePathInputTextBox.Location = new System.Drawing.Point(97, 78);
            this.TablePathInputTextBox.Name = "TablePathInputTextBox";
            this.TablePathInputTextBox.Size = new System.Drawing.Size(384, 21);
            this.TablePathInputTextBox.TabIndex = 6;
            this.TablePathInputTextBox.Text = "테이블 경로를 입력해주세요";
            // 
            // TablePathLabel
            // 
            this.TablePathLabel.AutoSize = true;
            this.TablePathLabel.Location = new System.Drawing.Point(26, 81);
            this.TablePathLabel.Name = "TablePathLabel";
            this.TablePathLabel.Size = new System.Drawing.Size(65, 12);
            this.TablePathLabel.TabIndex = 7;
            this.TablePathLabel.Text = "Table 경로";
            // 
            // MergedTablesFunctionTipLabel
            // 
            this.MergedTablesFunctionTipLabel.AutoSize = true;
            this.MergedTablesFunctionTipLabel.Location = new System.Drawing.Point(26, 227);
            this.MergedTablesFunctionTipLabel.Name = "MergedTablesFunctionTipLabel";
            this.MergedTablesFunctionTipLabel.Size = new System.Drawing.Size(173, 12);
            this.MergedTablesFunctionTipLabel.TabIndex = 8;
            this.MergedTablesFunctionTipLabel.Text = "※분리된 테이블을 합치는 기능";
            this.MergedTablesFunctionTipLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AdditionalFunctionLineLabel
            // 
            this.AdditionalFunctionLineLabel.AutoSize = true;
            this.AdditionalFunctionLineLabel.Location = new System.Drawing.Point(26, 138);
            this.AdditionalFunctionLineLabel.Name = "AdditionalFunctionLineLabel";
            this.AdditionalFunctionLineLabel.Size = new System.Drawing.Size(453, 12);
            this.AdditionalFunctionLineLabel.TabIndex = 9;
            this.AdditionalFunctionLineLabel.Text = "---------------------------------추가 기능---------------------------------";
            // 
            // ExcelHelperMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 386);
            this.Controls.Add(this.AdditionalFunctionLineLabel);
            this.Controls.Add(this.MergedTablesFunctionTipLabel);
            this.Controls.Add(this.TablePathLabel);
            this.Controls.Add(this.TablePathInputTextBox);
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
        private System.Windows.Forms.TextBox TablePathInputTextBox;
        private System.Windows.Forms.Label TablePathLabel;
        private System.Windows.Forms.Label MergedTablesFunctionTipLabel;
        private System.Windows.Forms.Label AdditionalFunctionLineLabel;
    }
}

