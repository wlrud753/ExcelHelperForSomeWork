namespace UpdateTableForm
{
    partial class UpdateTableForm
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
            this.UpdateTablesButton = new System.Windows.Forms.Button();
            this.TableListNoticeLable = new System.Windows.Forms.Label();
            this.NeedToUpdateTablesListView = new System.Windows.Forms.ListView();
            this.NeedToUpdateTableCountLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UpdateTablesButton
            // 
            this.UpdateTablesButton.Location = new System.Drawing.Point(30, 377);
            this.UpdateTablesButton.Name = "UpdateTablesButton";
            this.UpdateTablesButton.Size = new System.Drawing.Size(432, 61);
            this.UpdateTablesButton.TabIndex = 0;
            this.UpdateTablesButton.Text = "테이블 갱신";
            this.UpdateTablesButton.UseVisualStyleBackColor = true;
            this.UpdateTablesButton.Click += new System.EventHandler(this.UpdateTablesButton_Click);
            // 
            // TableListNoticeLable
            // 
            this.TableListNoticeLable.AutoSize = true;
            this.TableListNoticeLable.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TableListNoticeLable.Location = new System.Drawing.Point(27, 30);
            this.TableListNoticeLable.Name = "TableListNoticeLable";
            this.TableListNoticeLable.Size = new System.Drawing.Size(182, 16);
            this.TableListNoticeLable.TabIndex = 2;
            this.TableListNoticeLable.Text = "갱신 필요한 테이블 목록";
            // 
            // NeedToUpdateTablesListView
            // 
            this.NeedToUpdateTablesListView.HideSelection = false;
            this.NeedToUpdateTablesListView.Location = new System.Drawing.Point(30, 59);
            this.NeedToUpdateTablesListView.Name = "NeedToUpdateTablesListView";
            this.NeedToUpdateTablesListView.Size = new System.Drawing.Size(432, 312);
            this.NeedToUpdateTablesListView.TabIndex = 2;
            this.NeedToUpdateTablesListView.UseCompatibleStateImageBehavior = false;
            this.NeedToUpdateTablesListView.View = System.Windows.Forms.View.Details;
            // 
            // NeedToUpdateTableCountLabel
            // 
            this.NeedToUpdateTableCountLabel.AutoSize = true;
            this.NeedToUpdateTableCountLabel.Location = new System.Drawing.Point(424, 34);
            this.NeedToUpdateTableCountLabel.Name = "NeedToUpdateTableCountLabel";
            this.NeedToUpdateTableCountLabel.Size = new System.Drawing.Size(35, 12);
            this.NeedToUpdateTableCountLabel.TabIndex = 3;
            this.NeedToUpdateTableCountLabel.Text = "{0}개";
            // 
            // UpdateTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 456);
            this.Controls.Add(this.NeedToUpdateTableCountLabel);
            this.Controls.Add(this.NeedToUpdateTablesListView);
            this.Controls.Add(this.TableListNoticeLable);
            this.Controls.Add(this.UpdateTablesButton);
            this.Name = "UpdateTableForm";
            this.Text = "테이블 갱신";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpdateTablesButton;
        private System.Windows.Forms.Label TableListNoticeLable;
        private System.Windows.Forms.ListView NeedToUpdateTablesListView;
        private System.Windows.Forms.Label NeedToUpdateTableCountLabel;
    }
}