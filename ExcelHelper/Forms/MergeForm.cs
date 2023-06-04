using ExcelHelper;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MergeForm
{
    public partial class MergeForm : Form
    {
        ExcelManager.ExcelManager excelManager;

        Thread mainWork;

        static string saveMergePath;

        public MergeForm(ExcelManager.ExcelManager _excelManager, string _workingStream)
        {
            InitializeComponent();

            this.excelManager = _excelManager;

            saveMergePath = ExcelHelperMain.saveDataPath + "\\" + _workingStream + "\\MergedTable";
            if (!Directory.Exists(saveMergePath)) { Directory.CreateDirectory(saveMergePath); }


            foreach (string item in this.excelManager.GetAllDividedData())
            {
                // 데이터 달라지면, 어차피 적용 안 되는 코드들이라... 그냥 하드코딩함 (99% 케이스만 고려해서 간단하게 짬)
                switch (item)
                {
                    case "Event":
                        const string InsEvent = "InstanceEvent";
                        DataListBox.Items.Add(InsEvent);
                        break;
                    case "QuestTitleText":
                        const string QuestText = "QuestText";
                        DataListBox.Items.Add(QuestText);
                        break;
                    case "SelectTalk":
                        const string Talk = "Talk";
                        DataListBox.Items.Add(Talk);
                        break;
                    default:
                        DataListBox.Items.Add(item);
                        break;
                }
            }

            // Form Setting
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }


        private void DataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DataListBox.SelectedItem != null)
            {
                FileListBox.Items.Clear();

                string data = DataListBox.SelectedItem.ToString();

                // 임의로 수정했던 Data 이름이면
                if (!excelManager.IsDataDictContain(data))
                {
                    switch (data)
                    {
                        case "InstanceEvent":
                            data = "Event";
                            break;
                        case "QuestText":
                            data = "QuestTitleText";
                            break;
                        case "Talk":
                            data = "SelectTalk";
                            break;
                        default:
                            data = null;
                            break;
                    }
                }

                if (data == null)
                {
                    PopUpNoti.PopUpNoti cannotFindDataNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "경고!", "해당하는 Data를 찾을 수 없습니다.\n제작자에게 문의해주세요!");
                    cannotFindDataNoti.ShowDialog();

                    return;
                }

                foreach (string item in this.excelManager.GetFilesInSpecificData(data))
                {
                    FileListBox.Items.Add(Path.GetFileName(item)); // 경로 제거하고, File 이름만 보이도록 설정
                }

            }
        }

        private void MergeButton_Click(object sender, EventArgs e)
        {
            if (DataListBox.SelectedItem != null)
            {
                if (!Directory.Exists(saveMergePath)) { Directory.CreateDirectory(saveMergePath); }

                // Merge 시작
                // 데이터 달라지면, 어차피 적용 안 되는 코드들이라... 그냥 하드코딩함 (99% 케이스만 고려해서 간단하게 짬)
                string data = DataListBox.SelectedItem.ToString();
                if (!excelManager.IsDataDictContain(data))
                {
                    switch (data)
                    {
                        case "InstanceEvent":
                            data = "Event";
                            break;
                        case "QuestText":
                            data = "QuestTitleText";
                            break;
                        case "Talk":
                            data = "SelectTalk";
                            break;
                        default:
                            data = null;
                            break;
                    }
                }

                if (data == null)
                {
                    PopUpNoti.PopUpNoti cannotFindDataNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "경고!", "해당하는 Data를 찾을 수 없습니다.\n제작자에게 문의해주세요!");
                    cannotFindDataNoti.ShowDialog();

                    return;
                }

                mainWork = new Thread(() => DoMerge(saveMergePath, data));
                mainWork.IsBackground = true;
                mainWork.Start();

                // 작업 안내
                PopUpNoti.PopUpNoti workingNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.working, "작업 중입니다. '응답 없음'이 떠도 종료하지 말아주세요!", "작업 중입니다... 멈춘 게 아니니 종료하지 말아주세요!");
                workingNoti.Show();
                workingNoti.ThreadStart();

                // Merge 대기
                mainWork.Join();

                // 작업 안내 종료
                workingNoti.ThreadEnd();
                workingNoti.Close();

                //PopUpNoti.PopUpNoti completeNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Merge Complete!", DataListBox.SelectedItem + " Data의 머지가 완료되었습니다.\n메모리 관리를 위해 프로그램이 재시작됩니다.\n※통합 테이블이 자동으로 열립니다.");
                PopUpNoti.PopUpNoti completeNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Merge Complete!", DataListBox.SelectedItem + " Data의 머지가 완료되었습니다.\n※통합 테이블이 자동으로 열립니다.");
                
                completeNoti.ShowDialog();

                System.Diagnostics.Process.Start(excelManager.GetMergedTableName(saveMergePath, data));

                // It doesnt work in Office enviroment
                //Application.Restart();
            }
        }
        void DoMerge(string _savePath, string _data)
        {
            excelManager.Merge(_savePath, _data);
        }
    }
}
