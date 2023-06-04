using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

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
        static string saveDictDataPath;
        static string saveUtilPath;
        #endregion

        string workingStream;

        public ExcelHelperMain()
        {
            InitializeComponent();

            if (!Directory.Exists(saveDataPath)) { Directory.CreateDirectory(saveDataPath); }

            if (StreamManager.HasSavedStreamData())
            {
                List<string[]> tmpStreamData = StreamManager.GetStreamData();

                foreach (var stream in tmpStreamData)
                {
                    // Set Default Stream
                    if (stream == tmpStreamData.First())
                    {
                        workingStream = stream[0];
                    }

                    MakeStreamDir(stream[0]);

                    SelectStreamComboBox.Items.Add(stream[0]);
                }

                SelectStreamComboBox.SelectedItem = workingStream;
                GetStreamData(workingStream);
            }
            else
                workingStream = null;

            // Form 관련 초기화
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            MergedTablesFunctionTipLabel.Width = MergeTablesButton.Width;
            MergedTablesFunctionTipLabel.Left = MergeTablesButton.Left;
        }

        #region Event
        private void UpdateTableDataButton_Click(object sender, EventArgs e)
        {
            if (workingStream == null)
            {
                PopUpNoti.PopUpNoti cannotFindStream = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Stream Error!", "스트림을 설정해주세요.");
                cannotFindStream.ShowDialog();
                return;
            }

            string streamTablePath = StreamManager.GetStreamTablePath(workingStream);

            if (streamTablePath == null)
            {
                PopUpNoti.PopUpNoti cannotFindStreamPath = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Stream Error!", "스트림 경로를 다시 설정해주세요.");
                cannotFindStreamPath.ShowDialog();
                return;
            }

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

                utilities.SaveTablePath(saveUtilPath, streamTablePath);
                utilities.SaveTablecount(saveUtilPath, Directory.GetFiles(streamTablePath, "*.xlsx", SearchOption.AllDirectories).Length);
            }
        }
        void RunExcelManagerReadFiles()
        {
            ExcelManager.ReadFilesFromFullio(StreamManager.GetStreamTablePath(workingStream), saveDictDataPath);
        }


        private void MergeTablesButton_Click(object sender, EventArgs e)
        {
            if(workingStream == null)
            {
                PopUpNoti.PopUpNoti cannotFindStream = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Stream Error!", "스트림을 설정해주세요.");
                cannotFindStream.ShowDialog();
                return;
            }

            // workingStream 데이터가 변경돼도 → ExcelManager는 그대로 가서,, 빈 스트림의 경우 이전 스트림 데이터가 그대로 남아 있음.
            // DictData가 정상적으로 들어 있는지 확인 -> DictData 내부의 파일이 손상되는 경우는 체크하지 않음 (수동으로 파일을 수정하는 경우는... 업보를 맞이하게 됨)
            if(System.IO.Directory.GetFiles(saveDictDataPath) == null || System.IO.Directory.GetFiles(saveDictDataPath).Length < 2)
            {
                PopUpNoti.PopUpNoti noTableInfoNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Warning!!", "테이블 정보가 없어, Merge 할 수 없습니다.\n'전체 테이블 확인'을 실행해주세요.");
                noTableInfoNoti.ShowDialog();
                return;
            }

            if (ExcelManager.GetAllRepresentDataList() != null)
            {
                MergeForm.MergeForm mergeForm = new MergeForm.MergeForm(this.ExcelManager, this.workingStream);
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

        private void SelectStreamComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedIndex >= 0)
            {
                workingStream = cb.SelectedItem as string;

                GetStreamData(workingStream);
            }
        }
        
        private void SetStreamOptionButton_Click(object sender, EventArgs e)
        {
            StreamManagingForm.StreamManagingForm streamManagingForm = new StreamManagingForm.StreamManagingForm();
            streamManagingForm.saveDataSend += new StreamManagingForm.StreamManagingForm.saveDataSendDelegate(UpdateSelectStreamComboBox);
            streamManagingForm.ShowDialog();
        }
        void UpdateSelectStreamComboBox()
        {
            SelectStreamComboBox.Items.Clear();

            foreach(var streamName in StreamManager.GetStreamPathList())
            {
                SelectStreamComboBox.Items.Add(streamName[0]);
                MakeStreamDir(streamName[0]);
            }

            // MakeStreamDir에서 SavePath들이 전부 변경됐었기에, 지금 선택한 stream으로 SavePath 다시 변경
            SetStreamSavePath(workingStream);
        }

        #region Utils
        void SetStreamSavePath(string _stream)
        {
            saveDictDataPath = saveDataPath + "\\" + _stream + "\\dictData";
            saveUtilPath = saveDataPath + "\\" + _stream + "\\util";
        }

        void GetStreamData(string _stream)
        {
            SetStreamSavePath(_stream);

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

            if (System.IO.File.Exists(saveUtilPath + "\\TableCount.txt"))
            {
                string tmpStreamPath = StreamManager.GetStreamTablePath(workingStream);

                if (tmpStreamPath.Contains("GameDesign") && tmpStreamPath.Contains("Table"))
                {
                    // 저장돼 있던 테이블 개수와, 오픈 시점의 테이블 개수 비교
                    if (!utilities.CompareTableCount(saveUtilPath, Directory.GetFiles(tmpStreamPath, "*.xlsx", SearchOption.AllDirectories).Length))
                    {
                        PopUpNoti.PopUpNoti nonEqualBetweenSavedAndNowTableCount = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Warning!!", "마지막 \'테이블 확인\' 이후 테이블 개수에 변화가 생겼습니다.\n\'전체 테이블 확인\' 기능을 수행해주세요.");
                        nonEqualBetweenSavedAndNowTableCount.ShowDialog();

                        UpdateTableDataButton.Text = "전체 테이블 확인\n"
                        + "(수행 필요: 테이블 개수에 변동 발생)";
                    }
                    else // 저장된 테이블 개수 == 오픈 시점 테이블 개수
                    {
                        UpdateTableDataButton.Text = "전체 테이블 확인";
                    }
                }
                // 파일 망가지면 흠... 못 잡아내는데...
            }
        }
        void MakeStreamDir(string _stream)
        {
            SetStreamSavePath(_stream);
            if (!Directory.Exists(saveDictDataPath)) { Directory.CreateDirectory(saveDictDataPath); }
            if (!Directory.Exists(saveUtilPath)) { Directory.CreateDirectory(saveUtilPath); }
        }
        #endregion
    }
}
