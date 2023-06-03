using DataHandler;
using ExcelManager;
using MergeForm;
using Utilities;
using PopUpNoti;
using Perforce.P4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.AccessControl;
using System.Threading;

namespace ExcelHelper
{
    public partial class ExcelHelperMain : Form
    {
        ExcelManager.ExcelManager ExcelManager = new ExcelManager.ExcelManager();
        StreamManager.StreamManager StreamManager = new StreamManager.StreamManager();
        Utilities.Utilities utilities = new Utilities.Utilities();

        Thread mainWork;

        #region Path
        public static string saveDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ExcelHelper";
        static string saveDictDataPath = saveDataPath + "\\dictdata";
        static string saveUtilPath = saveDataPath + "\\util";
        #endregion

        public ExcelHelperMain()
        {
            InitializeComponent();

            if (!Directory.Exists(saveDataPath)) { Directory.CreateDirectory(saveDataPath); }
            if (!Directory.Exists(saveDictDataPath)) { Directory.CreateDirectory(saveDictDataPath); }
            if (!Directory.Exists(saveUtilPath) ) { Directory.CreateDirectory(saveUtilPath); }

            // 기존 메모장 파일 있으면 -> 해당 파일 기반으로 Data 세팅
            if (System.IO.File.Exists(saveDictDataPath + "\\DataDict.txt") && System.IO.File.Exists(saveDictDataPath + "\\FileDict.txt"))
            {
                ExcelManager.ReadFilesFromDictData(saveDictDataPath);
            }
            else
            {
                // Label 에서 노티
                UpdateTableDataButton.Text = "전체 테이블 확인\n"
                    + "(최초 1회 수행 필요: 확인된 테이블 정보 없음)";
            }

            if(System.IO.File.Exists(saveUtilPath + "\\TablePath.txt"))
            {
                if(System.IO.File.ReadLines(saveUtilPath + "\\TablePath.txt").ToList().Count > 0) // 파일 망가지는 경우 대비 (값 날아가는 경우만 대비. 값이 이상해지는 건 영향 없음)
                {
                    TablePathInputTextBox.Text = System.IO.File.ReadLines(saveUtilPath + "\\TablePath.txt").ToList()[0];
                }
            }

            if(System.IO.File.Exists(saveUtilPath + "\\TableCount.txt"))
            {
                if (TablePathInputTextBox.Text.Contains("GameDesign") && TablePathInputTextBox.Text.Contains("Table"))
                {
                    // 저장돼 있던 테이블 개수와, 오픈 시점의 테이블 개수 비교
                    if (!utilities.CompareTableCount(saveUtilPath, Directory.GetFiles(TablePathInputTextBox.Text, "*.xlsx", SearchOption.AllDirectories).Length))
                    {
                        PopUpNoti.PopUpNoti nonEqualBetweenSavedAndNowTableCount = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Warning!!", "마지막 \'테이블 확인\' 이후 테이블 개수에 변화가 생겼습니다.\n\'전체 테이블 확인\' 기능을 수행해주세요.");
                        nonEqualBetweenSavedAndNowTableCount.ShowDialog();

                        UpdateTableDataButton.Text = "전체 테이블 확인\n"
                        + "(수행 필요: 테이블 개수에 변동 발생)";
                    }
                }
                // 파일 망가지면 흠... 못 잡아내는데...
            }

            // Form 관련 초기화
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            MergedTablesFunctionTipLabel.Width = MergeTablesButton.Width;
            MergedTablesFunctionTipLabel.Left = MergeTablesButton.Left;

            TablePathInputTextBox.GotFocus += TablePathInputTextBox_GotFocus;
            TablePathInputTextBox.LostFocus += TablePathInputTextBox_LostFocus;
        }

        #region Event
        private void UpdateTableDataButton_Click(object sender, EventArgs e)
        {
            if (!TablePathInputTextBox.Text.Contains("GameDesign") || !TablePathInputTextBox.Text.Contains("Table"))
            {
                PopUpNoti.PopUpNoti pathError = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Path Error!", "테이블 경로가 이상한 것 같슴다?!\n스트림의 \"..\\GameDesign\\Table\"이 들어 있어야 합니다.");
                pathError.ShowDialog();
            }
            else
            {
                // 진짜 하시겠습니까? popup noti
                PopUpNoti.PopUpNoti askUpdate = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.ask, "UpdateTable", "정말 전체 테이블 정보를 확인하시겠습니까?");
                if (askUpdate.ShowDialog() == DialogResult.OK)
                {
                    // Read 실행
                    mainWork = new Thread(RunExcelManagerReadFiles);
                    mainWork.IsBackground = true;
                    mainWork.Start();

                    // 작업 안내
                    PopUpNoti.PopUpNoti workingNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.working, "작업 중입니다. '응답 없음'이 떠도 종료하지 말아주세요!", "작업 중입니다... 멈춘 게 아니니 종료하지 말아주세요!");
                    workingNoti.Show();
                    workingNoti.ThreadStart();

                    // Read End 대기
                    mainWork.Join();

                    // 작업 안내 종료
                    workingNoti.ThreadEnd();
                    workingNoti.Close();

                    // Update 완료 노티
                    PopUpNoti.PopUpNoti completeNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Update Complete!", "전체 테이블 확인이 종료되었습니다.\n이제 다른 기능들을 사용할 수 있습니다.");
                    completeNoti.ShowDialog();

                    UpdateTableDataButton.Text = "전체 테이블 확인";

                    utilities.SaveTablePath(saveUtilPath, TablePathInputTextBox.Text);
                    utilities.SaveTablecount(saveUtilPath, Directory.GetFiles(TablePathInputTextBox.Text, "*.xlsx", SearchOption.AllDirectories).Length);
                }
            }
        }
        void RunExcelManagerReadFiles()
        {
            ExcelManager.ReadFilesFromFullio(TablePathInputTextBox.Text, saveDictDataPath);
        }

        private void TablePathInputTextBox_LostFocus(object sender, EventArgs e)
        {
            if (TablePathInputTextBox.Text == "")
            {
                TablePathInputTextBox.Text = "테이블 경로를 입력해주세요";
            }
        }

        private void TablePathInputTextBox_GotFocus(object sender, EventArgs e)
        {
            if (TablePathInputTextBox.Text == "테이블 경로를 입력해주세요")
            {
                TablePathInputTextBox.Text = "";
            }
        }


        private void MergeTablesButton_Click(object sender, EventArgs e)
        {
            if (ExcelManager.GetAllRepresentDataList() != null)
            {
                MergeForm.MergeForm mergeForm = new MergeForm.MergeForm(this.ExcelManager);
                mergeForm.ShowDialog();
            }
            else
            {
                // 테이블 정보가 없는 경우 -> 머지 불가
                PopUpNoti.PopUpNoti noTableInfoNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Warning!!", "테이블 정보가 없어, Merge 할 수 없습니다.\n'전체 테이블 확인'을 실행해주세요.");
                noTableInfoNoti.ShowDialog();
            }
        }

        private void ExcelHelperMain_Load(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
