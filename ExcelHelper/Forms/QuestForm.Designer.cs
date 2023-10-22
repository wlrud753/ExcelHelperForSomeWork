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
            this.ReacceptableWarningQuestListView = new System.Windows.Forms.ListView();
            this.ReacceptableCheckNotifyLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_TitleLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_QuestIdLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_QuestIdTextbox = new System.Windows.Forms.TextBox();
            this.QuestDetailSection_NpcIdLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_NpcIdTextbox = new System.Windows.Forms.TextBox();
            this.QuestDetailSection_SpawnerIdLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_SpawnerIdTextbox = new System.Windows.Forms.TextBox();
            this.QuestDetailSection_WarningLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_TableInfoTitleLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_QuestTableLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_QuestTableTextbox = new System.Windows.Forms.TextBox();
            this.QuestDetailSection_NpcTableLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_NpcTableTextbox = new System.Windows.Forms.TextBox();
            this.QuestDetailSection_SpawnerTableLabel = new System.Windows.Forms.Label();
            this.QuestDetailSection_SpawnerTableTextbox = new System.Windows.Forms.TextBox();
            this.QuestDetailSection_CanCopyNotiLabel02 = new System.Windows.Forms.Label();
            this.QuestDetailSection_CanCopyNotiLabel01 = new System.Windows.Forms.Label();
            this.QuestDetailSection_WarningNotiLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CheckReacceptableButton
            // 
            this.CheckReacceptableButton.Location = new System.Drawing.Point(22, 488);
            this.CheckReacceptableButton.Name = "CheckReacceptableButton";
            this.CheckReacceptableButton.Size = new System.Drawing.Size(459, 39);
            this.CheckReacceptableButton.TabIndex = 0;
            this.CheckReacceptableButton.Text = "재수락 확인";
            this.CheckReacceptableButton.UseVisualStyleBackColor = true;
            this.CheckReacceptableButton.Click += new System.EventHandler(this.CheckReacceptableButton_Click);
            // 
            // ReacceptableWarningQuestListView
            // 
            this.ReacceptableWarningQuestListView.HideSelection = false;
            this.ReacceptableWarningQuestListView.Location = new System.Drawing.Point(22, 23);
            this.ReacceptableWarningQuestListView.Name = "ReacceptableWarningQuestListView";
            this.ReacceptableWarningQuestListView.Size = new System.Drawing.Size(459, 459);
            this.ReacceptableWarningQuestListView.TabIndex = 3;
            this.ReacceptableWarningQuestListView.UseCompatibleStateImageBehavior = false;
            this.ReacceptableWarningQuestListView.View = System.Windows.Forms.View.Details;
            this.ReacceptableWarningQuestListView.SelectedIndexChanged += new System.EventHandler(this.ReacceptableWarningQuestListView_SelectedIndexChanged);
            // 
            // ReacceptableCheckNotifyLabel
            // 
            this.ReacceptableCheckNotifyLabel.AutoSize = true;
            this.ReacceptableCheckNotifyLabel.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ReacceptableCheckNotifyLabel.Location = new System.Drawing.Point(23, 533);
            this.ReacceptableCheckNotifyLabel.Name = "ReacceptableCheckNotifyLabel";
            this.ReacceptableCheckNotifyLabel.Size = new System.Drawing.Size(442, 55);
            this.ReacceptableCheckNotifyLabel.TabIndex = 2;
            this.ReacceptableCheckNotifyLabel.Text = "모든 퀘스트에 대하여 재수락 관련 문제가 없는지 체크합니다.\r\n※Quest, Npc 등에 변경점이 있는 경우, 엑셀 저장 후 \'재수락 확인\'을 누" +
    "르시면 됩니다.\r\n\r\n※※프로그램을 껐다 키거나 하지 않아도\r\n※※엑셀 저장하고 버튼만 눌러도 작업한 내역까지 다 체크해줌";
            // 
            // QuestDetailSection_TitleLabel
            // 
            this.QuestDetailSection_TitleLabel.AutoSize = true;
            this.QuestDetailSection_TitleLabel.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.QuestDetailSection_TitleLabel.Location = new System.Drawing.Point(625, 38);
            this.QuestDetailSection_TitleLabel.Name = "QuestDetailSection_TitleLabel";
            this.QuestDetailSection_TitleLabel.Size = new System.Drawing.Size(144, 18);
            this.QuestDetailSection_TitleLabel.TabIndex = 3;
            this.QuestDetailSection_TitleLabel.Text = "===세부사항===";
            // 
            // QuestDetailSection_QuestIdLabel
            // 
            this.QuestDetailSection_QuestIdLabel.AutoSize = true;
            this.QuestDetailSection_QuestIdLabel.Location = new System.Drawing.Point(507, 141);
            this.QuestDetailSection_QuestIdLabel.Name = "QuestDetailSection_QuestIdLabel";
            this.QuestDetailSection_QuestIdLabel.Size = new System.Drawing.Size(68, 12);
            this.QuestDetailSection_QuestIdLabel.TabIndex = 4;
            this.QuestDetailSection_QuestIdLabel.Text = "Quest Cuid";
            // 
            // QuestDetailSection_QuestIdTextbox
            // 
            this.QuestDetailSection_QuestIdTextbox.Location = new System.Drawing.Point(603, 136);
            this.QuestDetailSection_QuestIdTextbox.Name = "QuestDetailSection_QuestIdTextbox";
            this.QuestDetailSection_QuestIdTextbox.ReadOnly = true;
            this.QuestDetailSection_QuestIdTextbox.Size = new System.Drawing.Size(271, 21);
            this.QuestDetailSection_QuestIdTextbox.TabIndex = 5;
            // 
            // QuestDetailSection_NpcIdLabel
            // 
            this.QuestDetailSection_NpcIdLabel.AutoSize = true;
            this.QuestDetailSection_NpcIdLabel.Location = new System.Drawing.Point(507, 179);
            this.QuestDetailSection_NpcIdLabel.Name = "QuestDetailSection_NpcIdLabel";
            this.QuestDetailSection_NpcIdLabel.Size = new System.Drawing.Size(58, 12);
            this.QuestDetailSection_NpcIdLabel.TabIndex = 4;
            this.QuestDetailSection_NpcIdLabel.Text = "Npc Cuid";
            // 
            // QuestDetailSection_NpcIdTextbox
            // 
            this.QuestDetailSection_NpcIdTextbox.Location = new System.Drawing.Point(603, 174);
            this.QuestDetailSection_NpcIdTextbox.Name = "QuestDetailSection_NpcIdTextbox";
            this.QuestDetailSection_NpcIdTextbox.ReadOnly = true;
            this.QuestDetailSection_NpcIdTextbox.Size = new System.Drawing.Size(271, 21);
            this.QuestDetailSection_NpcIdTextbox.TabIndex = 5;
            // 
            // QuestDetailSection_SpawnerIdLabel
            // 
            this.QuestDetailSection_SpawnerIdLabel.AutoSize = true;
            this.QuestDetailSection_SpawnerIdLabel.Location = new System.Drawing.Point(507, 219);
            this.QuestDetailSection_SpawnerIdLabel.Name = "QuestDetailSection_SpawnerIdLabel";
            this.QuestDetailSection_SpawnerIdLabel.Size = new System.Drawing.Size(85, 12);
            this.QuestDetailSection_SpawnerIdLabel.TabIndex = 4;
            this.QuestDetailSection_SpawnerIdLabel.Text = "Spawner Cuid";
            // 
            // QuestDetailSection_SpawnerIdTextbox
            // 
            this.QuestDetailSection_SpawnerIdTextbox.Location = new System.Drawing.Point(603, 214);
            this.QuestDetailSection_SpawnerIdTextbox.Name = "QuestDetailSection_SpawnerIdTextbox";
            this.QuestDetailSection_SpawnerIdTextbox.ReadOnly = true;
            this.QuestDetailSection_SpawnerIdTextbox.Size = new System.Drawing.Size(271, 21);
            this.QuestDetailSection_SpawnerIdTextbox.TabIndex = 5;
            // 
            // QuestDetailSection_WarningLabel
            // 
            this.QuestDetailSection_WarningLabel.AutoSize = true;
            this.QuestDetailSection_WarningLabel.Font = new System.Drawing.Font("굴림", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.QuestDetailSection_WarningLabel.Location = new System.Drawing.Point(600, 87);
            this.QuestDetailSection_WarningLabel.Name = "QuestDetailSection_WarningLabel";
            this.QuestDetailSection_WarningLabel.Size = new System.Drawing.Size(188, 13);
            this.QuestDetailSection_WarningLabel.TabIndex = 6;
            this.QuestDetailSection_WarningLabel.Text = "확인할 퀘스트를 선택해주세요.";
            this.QuestDetailSection_WarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QuestDetailSection_TableInfoTitleLabel
            // 
            this.QuestDetailSection_TableInfoTitleLabel.AutoSize = true;
            this.QuestDetailSection_TableInfoTitleLabel.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.QuestDetailSection_TableInfoTitleLabel.Location = new System.Drawing.Point(634, 312);
            this.QuestDetailSection_TableInfoTitleLabel.Name = "QuestDetailSection_TableInfoTitleLabel";
            this.QuestDetailSection_TableInfoTitleLabel.Size = new System.Drawing.Size(135, 15);
            this.QuestDetailSection_TableInfoTitleLabel.TabIndex = 3;
            this.QuestDetailSection_TableInfoTitleLabel.Text = "===테이블 정보===";
            this.QuestDetailSection_TableInfoTitleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // QuestDetailSection_QuestTableLabel
            // 
            this.QuestDetailSection_QuestTableLabel.AutoSize = true;
            this.QuestDetailSection_QuestTableLabel.Location = new System.Drawing.Point(507, 351);
            this.QuestDetailSection_QuestTableLabel.Name = "QuestDetailSection_QuestTableLabel";
            this.QuestDetailSection_QuestTableLabel.Size = new System.Drawing.Size(74, 12);
            this.QuestDetailSection_QuestTableLabel.TabIndex = 4;
            this.QuestDetailSection_QuestTableLabel.Text = "Quest Table";
            // 
            // QuestDetailSection_QuestTableTextbox
            // 
            this.QuestDetailSection_QuestTableTextbox.Location = new System.Drawing.Point(603, 346);
            this.QuestDetailSection_QuestTableTextbox.Name = "QuestDetailSection_QuestTableTextbox";
            this.QuestDetailSection_QuestTableTextbox.ReadOnly = true;
            this.QuestDetailSection_QuestTableTextbox.Size = new System.Drawing.Size(271, 21);
            this.QuestDetailSection_QuestTableTextbox.TabIndex = 5;
            // 
            // QuestDetailSection_NpcTableLabel
            // 
            this.QuestDetailSection_NpcTableLabel.AutoSize = true;
            this.QuestDetailSection_NpcTableLabel.Location = new System.Drawing.Point(507, 389);
            this.QuestDetailSection_NpcTableLabel.Name = "QuestDetailSection_NpcTableLabel";
            this.QuestDetailSection_NpcTableLabel.Size = new System.Drawing.Size(64, 12);
            this.QuestDetailSection_NpcTableLabel.TabIndex = 4;
            this.QuestDetailSection_NpcTableLabel.Text = "Npc Table";
            // 
            // QuestDetailSection_NpcTableTextbox
            // 
            this.QuestDetailSection_NpcTableTextbox.Location = new System.Drawing.Point(603, 384);
            this.QuestDetailSection_NpcTableTextbox.Name = "QuestDetailSection_NpcTableTextbox";
            this.QuestDetailSection_NpcTableTextbox.ReadOnly = true;
            this.QuestDetailSection_NpcTableTextbox.Size = new System.Drawing.Size(271, 21);
            this.QuestDetailSection_NpcTableTextbox.TabIndex = 5;
            // 
            // QuestDetailSection_SpawnerTableLabel
            // 
            this.QuestDetailSection_SpawnerTableLabel.AutoSize = true;
            this.QuestDetailSection_SpawnerTableLabel.Location = new System.Drawing.Point(507, 429);
            this.QuestDetailSection_SpawnerTableLabel.Name = "QuestDetailSection_SpawnerTableLabel";
            this.QuestDetailSection_SpawnerTableLabel.Size = new System.Drawing.Size(91, 12);
            this.QuestDetailSection_SpawnerTableLabel.TabIndex = 4;
            this.QuestDetailSection_SpawnerTableLabel.Text = "Spawner Table";
            // 
            // QuestDetailSection_SpawnerTableTextbox
            // 
            this.QuestDetailSection_SpawnerTableTextbox.Location = new System.Drawing.Point(603, 424);
            this.QuestDetailSection_SpawnerTableTextbox.Name = "QuestDetailSection_SpawnerTableTextbox";
            this.QuestDetailSection_SpawnerTableTextbox.ReadOnly = true;
            this.QuestDetailSection_SpawnerTableTextbox.Size = new System.Drawing.Size(271, 21);
            this.QuestDetailSection_SpawnerTableTextbox.TabIndex = 5;
            // 
            // QuestDetailSection_CanCopyNotiLabel02
            // 
            this.QuestDetailSection_CanCopyNotiLabel02.AutoSize = true;
            this.QuestDetailSection_CanCopyNotiLabel02.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.QuestDetailSection_CanCopyNotiLabel02.Location = new System.Drawing.Point(653, 469);
            this.QuestDetailSection_CanCopyNotiLabel02.Name = "QuestDetailSection_CanCopyNotiLabel02";
            this.QuestDetailSection_CanCopyNotiLabel02.Size = new System.Drawing.Size(175, 11);
            this.QuestDetailSection_CanCopyNotiLabel02.TabIndex = 2;
            this.QuestDetailSection_CanCopyNotiLabel02.Text = "↑↑↑더블클릭 이후 복사 가능↑↑↑";
            // 
            // QuestDetailSection_CanCopyNotiLabel01
            // 
            this.QuestDetailSection_CanCopyNotiLabel01.AutoSize = true;
            this.QuestDetailSection_CanCopyNotiLabel01.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.QuestDetailSection_CanCopyNotiLabel01.Location = new System.Drawing.Point(653, 254);
            this.QuestDetailSection_CanCopyNotiLabel01.Name = "QuestDetailSection_CanCopyNotiLabel01";
            this.QuestDetailSection_CanCopyNotiLabel01.Size = new System.Drawing.Size(175, 11);
            this.QuestDetailSection_CanCopyNotiLabel01.TabIndex = 2;
            this.QuestDetailSection_CanCopyNotiLabel01.Text = "↑↑↑더블클릭 이후 복사 가능↑↑↑";
            // 
            // QuestDetailSection_WarningNotiLabel
            // 
            this.QuestDetailSection_WarningNotiLabel.AutoSize = true;
            this.QuestDetailSection_WarningNotiLabel.Location = new System.Drawing.Point(507, 87);
            this.QuestDetailSection_WarningNotiLabel.Name = "QuestDetailSection_WarningNotiLabel";
            this.QuestDetailSection_WarningNotiLabel.Size = new System.Drawing.Size(73, 12);
            this.QuestDetailSection_WarningNotiLabel.TabIndex = 4;
            this.QuestDetailSection_WarningNotiLabel.Text = "!!확인 필요!!";
            // 
            // QuestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 604);
            this.Controls.Add(this.QuestDetailSection_WarningLabel);
            this.Controls.Add(this.QuestDetailSection_SpawnerTableTextbox);
            this.Controls.Add(this.QuestDetailSection_SpawnerTableLabel);
            this.Controls.Add(this.QuestDetailSection_SpawnerIdTextbox);
            this.Controls.Add(this.QuestDetailSection_NpcTableTextbox);
            this.Controls.Add(this.QuestDetailSection_SpawnerIdLabel);
            this.Controls.Add(this.QuestDetailSection_NpcTableLabel);
            this.Controls.Add(this.QuestDetailSection_NpcIdTextbox);
            this.Controls.Add(this.QuestDetailSection_QuestTableTextbox);
            this.Controls.Add(this.QuestDetailSection_NpcIdLabel);
            this.Controls.Add(this.QuestDetailSection_QuestTableLabel);
            this.Controls.Add(this.QuestDetailSection_QuestIdTextbox);
            this.Controls.Add(this.QuestDetailSection_WarningNotiLabel);
            this.Controls.Add(this.QuestDetailSection_QuestIdLabel);
            this.Controls.Add(this.QuestDetailSection_TableInfoTitleLabel);
            this.Controls.Add(this.QuestDetailSection_TitleLabel);
            this.Controls.Add(this.QuestDetailSection_CanCopyNotiLabel01);
            this.Controls.Add(this.QuestDetailSection_CanCopyNotiLabel02);
            this.Controls.Add(this.ReacceptableCheckNotifyLabel);
            this.Controls.Add(this.ReacceptableWarningQuestListView);
            this.Controls.Add(this.CheckReacceptableButton);
            this.MaximizeBox = false;
            this.Name = "QuestForm";
            this.ShowIcon = false;
            this.Text = "Quest";
            this.Load += new System.EventHandler(this.QuestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CheckReacceptableButton;
        private System.Windows.Forms.ListView ReacceptableWarningQuestListView;
        private System.Windows.Forms.Label ReacceptableCheckNotifyLabel;
        private System.Windows.Forms.Label QuestDetailSection_TitleLabel;
        private System.Windows.Forms.Label QuestDetailSection_QuestIdLabel;
        private System.Windows.Forms.TextBox QuestDetailSection_QuestIdTextbox;
        private System.Windows.Forms.Label QuestDetailSection_NpcIdLabel;
        private System.Windows.Forms.TextBox QuestDetailSection_NpcIdTextbox;
        private System.Windows.Forms.Label QuestDetailSection_SpawnerIdLabel;
        private System.Windows.Forms.TextBox QuestDetailSection_SpawnerIdTextbox;
        private System.Windows.Forms.Label QuestDetailSection_WarningLabel;
        private System.Windows.Forms.Label QuestDetailSection_TableInfoTitleLabel;
        private System.Windows.Forms.Label QuestDetailSection_QuestTableLabel;
        private System.Windows.Forms.TextBox QuestDetailSection_QuestTableTextbox;
        private System.Windows.Forms.Label QuestDetailSection_NpcTableLabel;
        private System.Windows.Forms.TextBox QuestDetailSection_NpcTableTextbox;
        private System.Windows.Forms.Label QuestDetailSection_SpawnerTableLabel;
        private System.Windows.Forms.TextBox QuestDetailSection_SpawnerTableTextbox;
        private System.Windows.Forms.Label QuestDetailSection_CanCopyNotiLabel02;
        private System.Windows.Forms.Label QuestDetailSection_CanCopyNotiLabel01;
        private System.Windows.Forms.Label QuestDetailSection_WarningNotiLabel;
    }
}