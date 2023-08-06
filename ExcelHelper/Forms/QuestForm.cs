using QuestManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestForm
{
    public partial class QuestForm : Form
    {
        static QuestManager.QuestManager questManager;

        public QuestForm()
        {
            InitializeComponent();

            questManager = new QuestManager.QuestManager();

            // ListView 세팅
            ReacceptableWarningQuestListView.Columns.Add("퀘스트 CUID", 150, HorizontalAlignment.Left);
            ReacceptableWarningQuestListView.Columns.Add("경고 항목", 300, HorizontalAlignment.Left);

            ReacceptableWarningQuestListView.Update();
        }

        private void CheckReacceptableButton_Click(object sender, EventArgs e)
        {
            ReacceptableWarningQuestListView.Clear();
            
            Thread mainWork = new Thread(() => questManager.FindQuestReacceptable());
            mainWork.IsBackground = true;
            mainWork.Start();

            // 작업 안내
            PopUpNoti.PopUpNoti workingNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.working, "작업 중입니다. '응답 없음'이 떠도 종료하지 말아주세요!", "작업 중입니다... 멈춘 게 아니니 종료하지 말아주세요!");
            workingNoti.Show();
            workingNoti.ThreadStart();

            // 검증 대기
            mainWork.Join();

            // 작업 안내 종료
            workingNoti.ThreadEnd();
            workingNoti.Close();

            // ListView 업데이트
            ReacceptableWarningQuestListView.Columns.Add("퀘스트 CUID", 150, HorizontalAlignment.Left);
            ReacceptableWarningQuestListView.Columns.Add("경고 항목", 300, HorizontalAlignment.Left);

            foreach (var warningQuest in QuestManager.QuestManager.reacceptableWarningQuestDataList)
            {
                ListViewItem listViewItem = new ListViewItem(warningQuest.questID);

                switch (warningQuest.code)
                {
                    case QuestManager.QuestManager.ReacceptableWarningCode.NotNpcAccpetTypeInGeneral:
                        listViewItem.SubItems.Add("Npc 수락이 아닙니다.");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.NotHaveAcceptNpc:
                        listViewItem.SubItems.Add("수락 Npc가 없습니다.");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.DesyncNpcQuestList:
                        listViewItem.SubItems.Add("수락 Npc의 QuestList에 없습니다.");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.FalseInShowHeadInfo:
                        listViewItem.SubItems.Add("수락 아이콘이 출력되지 않습니다.");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcCanDeath:
                        listViewItem.SubItems.Add("수락 Npc가 죽을 수 있습니다.");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcCanCombatWithFieldMonster:
                        listViewItem.SubItems.Add("수락 Npc가 몬스터와 교전할 수 있습니다.");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcCanShowInFieldPermanently:
                        listViewItem.SubItems.Add("수락 Npc가 필드에 상시 보입니다.");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.NotExistAcceptNpcSpawnData:
                        listViewItem.SubItems.Add("수락 Npc의 스포너가 없습니다.");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcNotSpawnInPermanentField:
                        listViewItem.SubItems.Add("수락 Npc가 스폰되지 않습니다(스포너).");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcNotSpawnInPermanentField_SpawnLayer:
                        listViewItem.SubItems.Add("수락 Npc가 스폰되지 않습니다(스폰레이어).");
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.LongDistanceBetweenAcceptNpcInInstanceAndPermanentField:
                        listViewItem.SubItems.Add("인스턴스 <-> 필드 Npc 사이가 너무 떨어져 있습니다.");
                        break;
                }

                ReacceptableWarningQuestListView.Items.Add(listViewItem);
            }
            
            ReacceptableWarningQuestListView.Update();

            // 완료 안내
            PopUpNoti.PopUpNoti completeNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Complete!", "재수락 세팅 확인이 완료되었습니다.");
            completeNoti.ShowDialog();
        }

        private void QuestForm_Load(object sender, EventArgs e)
        {

        }

        private void ReacceptableWarningQuestListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 아이템 선택하면 우측 '세부사항'에 해당 내용들 반영
            // Table 정보도 저장하고 있어야 함.
            // 열어주는 건 X. 체크아웃을 지원 안 해줄 예정이라서.

            if(ReacceptableWarningQuestListView.SelectedItems.Count != 0)
            {
                int selectedIdx = ReacceptableWarningQuestListView.SelectedItems[0].Index;

                QuestManager.QuestManager.ReacceptableWarningQuestData warningQuest = QuestManager.QuestManager.reacceptableWarningQuestDataList.Find(q => q.questID == ReacceptableWarningQuestListView.Items[selectedIdx].SubItems[0].Text);

                switch (warningQuest.code)
                {
                    case QuestManager.QuestManager.ReacceptableWarningCode.NotNpcAccpetTypeInGeneral:
                        QuestDetailSection_WarningLabel.Text = "Npc 수락이 아닙니다.\nAcceptObjectType을 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.NotHaveAcceptNpc:
                        QuestDetailSection_WarningLabel.Text = "수락 Npc가 없습니다.\nAcceptObjectCuid를 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.DesyncNpcQuestList:
                        QuestDetailSection_WarningLabel.Text = "수락 Npc의 QuestList에 없습니다.\nNpcTable의 QuestList를 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.FalseInShowHeadInfo:
                        QuestDetailSection_WarningLabel.Text = "수락 아이콘이 출력되지 않습니다.\nNpcTable의 ShowHeadInfo를 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcCanDeath:
                        QuestDetailSection_WarningLabel.Text = "수락 Npc가 죽을 수 있습니다.\nNpcTable의 IsCapturable을 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcCanCombatWithFieldMonster:
                        QuestDetailSection_WarningLabel.Text = "수락 Npc가 몬스터와 교전할 수 있습니다.\nNpcTable의 Faction을 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcCanShowInFieldPermanently:
                        QuestDetailSection_WarningLabel.Text = "수락 Npc가 필드에 상시 보입니다.\nNpcTable의 HipeNpc를 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.NotExistAcceptNpcSpawnData:
                        QuestDetailSection_WarningLabel.Text = "수락 Npc의 스포너가 없습니다.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcNotSpawnInPermanentField:
                        QuestDetailSection_WarningLabel.Text = "수락 Npc가 스폰되지 않습니다(스포너).\n스포너의 IsSpawnOnStartup을 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.AcceptNpcNotSpawnInPermanentField_SpawnLayer:
                        QuestDetailSection_WarningLabel.Text = "수락 Npc가 스폰되지 않습니다(스폰레이어).\n스폰레이어의 IsActivateonStartup을 확인해주세요.";
                        break;
                    case QuestManager.QuestManager.ReacceptableWarningCode.LongDistanceBetweenAcceptNpcInInstanceAndPermanentField:
                        QuestDetailSection_WarningLabel.Text = "인스턴스 <-> 필드 Npc 사이가 너무 떨어져 있습니다.";
                        break;
                }
                
                QuestDetailSection_QuestIdTextbox.Text = warningQuest.questID;
                QuestDetailSection_NpcIdTextbox.Text = warningQuest.npcID;
                QuestDetailSection_SpawnerIdTextbox.Text = warningQuest.spawnID;

                QuestDetailSection_QuestTableTextbox.Text = warningQuest.questTableName;
                QuestDetailSection_NpcTableTextbox.Text = warningQuest.npcTableName;
                QuestDetailSection_SpawnerTableTextbox.Text = warningQuest.spawnTableName;
            }

        }
    }
}
