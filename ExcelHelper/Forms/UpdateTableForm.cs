using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace UpdateTableForm
{
    public partial class UpdateTableForm : Form
    {
        Thread updateTable;

        ExcelManager.ExcelManager excelManager;
        StreamManager.StreamManager streamManager;
        Utilities.Utilities utilities;

        string saveDataPath;
        string saveDictDataPath;
        string saveUtilPath;
        string streamTablePath;
        string workingStream;

        List<string> addedFiles = new List<string>();
        List<string> deletedFiles = new List<string>();

        public UpdateTableForm(ExcelManager.ExcelManager _excelManager, StreamManager.StreamManager _streamManager, Utilities.Utilities _utilities, string _saveDataPath, string _saveDictDataPath, string _saveUtilPath, string _streamTablePath, string _workingStream)
        {
            InitializeComponent();

            excelManager = _excelManager;
            streamManager = _streamManager;
            utilities = _utilities;

            saveDataPath = _saveDataPath;
            saveDictDataPath = _saveDictDataPath;
            saveUtilPath = _saveUtilPath;
            streamTablePath = _streamTablePath;
            workingStream = _workingStream;

            addedFiles.Clear();
            deletedFiles.Clear();

            InitUpdateTablesForm();

            int tableCount = 0;

            NeedToUpdateTablesListView.Clear();

            NeedToUpdateTablesListView.Columns.Add("파일 이름", 250, HorizontalAlignment.Left);
            NeedToUpdateTablesListView.Columns.Add("변경 내역", 150, HorizontalAlignment.Left);

            if (addedFiles != null || deletedFiles != null)
            {
                if(addedFiles != null)
                {
                    foreach (var file in addedFiles)
                    {
                        ListViewItem listViewItem = new ListViewItem(Path.GetFileName(file));

                        listViewItem.SubItems.Add("추가");

                        listViewItem.UseItemStyleForSubItems = false;
                        listViewItem.SubItems[1].BackColor = Color.Aqua;

                        NeedToUpdateTablesListView.Items.Add(listViewItem);
                    }
                    tableCount += addedFiles.Count;
                }

                if(deletedFiles != null)
                {
                    foreach (var file in deletedFiles)
                    {
                        ListViewItem listViewItem = new ListViewItem(Path.GetFileName(file));

                        listViewItem.SubItems.Add("삭제");

                        listViewItem.UseItemStyleForSubItems = false;
                        listViewItem.SubItems[1].BackColor = Color.DarkGreen;

                        NeedToUpdateTablesListView.Items.Add(listViewItem);
                    }

                    tableCount += deletedFiles.Count;
                }
            }
            else
            {
                TableListNoticeLable.Text = "추가/삭제된 테이블이 없습니다.";

                ListViewItem listViewItem = new ListViewItem("비었어요!");

                NeedToUpdateTablesListView.Items.Add(listViewItem);
            }

            NeedToUpdateTablesListView.Update();

            NeedToUpdateTableCountLabel.Text = tableCount + "개";
        }

        void InitUpdateTablesForm()
        {
            string tmpStreamPath = streamManager.GetStreamTablePath(workingStream);

            if (tmpStreamPath.Contains("GameDesign") && tmpStreamPath.Contains("Table"))
            {
                if (!utilities.CompareTableCount(saveUtilPath, Directory.GetFiles(tmpStreamPath, "*.xlsx", SearchOption.AllDirectories).ToList().FindAll(f => !f.Contains("~$")).Count))
                {
                    if (System.IO.File.Exists(saveDictDataPath + "\\" + "FileDict.txt"))
                    {
                        List<string> savedFiles = new List<string>();
                        string[] currentFiles = Directory.GetFiles(tmpStreamPath, "*.xlsx", SearchOption.AllDirectories);

                        foreach (var line in System.IO.File.ReadLines(saveDictDataPath + "\\" + "FileDict.txt", Encoding.UTF8))
                        {
                            if (line.Contains("Next...") || line.Contains("Sheet>>") || line.Contains("ColumnList>>"))
                            {
                                continue;
                            }

                            if (line.Contains("File>>"))
                            {
                                savedFiles.Add(line.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries)[1]);
                            }
                        }

                        foreach (var currentFile in currentFiles)
                        {
                            const string exceptionFilePrefix = "Merged_";
                            const string exceptionFileName_Replace = "Replace";
                            const string exceptionFileName_Override = "Override";
                            const string exceptionDir = "BotSetting";

                            if (currentFile.Contains(exceptionFilePrefix))
                                continue;
                            if (currentFile.Contains(exceptionFileName_Replace))
                                continue;
                            if (currentFile.Contains(exceptionFileName_Override))
                                continue;
                            if (currentFile.Contains(exceptionDir))
                                continue;

                            if (!savedFiles.Contains(currentFile))
                                addedFiles.Add(currentFile);

                        }
                        foreach (var savedFile in savedFiles)
                        {
                            if (!currentFiles.Contains(savedFile))
                                deletedFiles.Add(savedFile);
                        }
                    }
                    else
                    {
                        addedFiles = Directory.GetFiles(tmpStreamPath, "*.xlsx", SearchOption.AllDirectories).ToList();
                    }
                }
                else
                {
                    addedFiles = null;
                    deletedFiles = null;
                }
            }
        }

        private void UpdateTablesButton_Click(object sender, EventArgs e)
        {
            string askComment = "추가/삭제된 파일 내역을 반영합니다.";

            if (addedFiles == null && deletedFiles == null)
            {
                askComment = "전체 테이블 정보를 재구성합니다.\n시간이 많이 걸리는데 진행하시겠습니까?\n(Validator 완료만큼 소요)";
            }

            // 진짜 하시겠습니까? popup noti
            PopUpNoti.PopUpNoti askUpdate = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.ask, "UpdateTable", askComment);
            if (askUpdate.ShowDialog() == DialogResult.OK)
            {
                // Read 실행
                updateTable = new Thread(RunExcelManagerReadFiles);
                updateTable.IsBackground = true;
                updateTable.Start();

                // 작업 안내
                PopUpNoti.PopUpNoti workingNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.working, "작업 중입니다. '응답 없음'이 떠도 종료하지 말아주세요!", "작업 중입니다... 멈춘 게 아니니 종료하지 말아주세요!");
                workingNoti.Show();
                workingNoti.ThreadStart();

                // Read End 대기
                updateTable.Join();

                // 작업 안내 종료
                workingNoti.ThreadEnd();
                workingNoti.Close();

                // Update 완료 노티
                PopUpNoti.PopUpNoti completeNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Update Complete!", "전체 테이블 확인이 종료되었습니다.\n이제 다른 기능들을 사용할 수 있습니다.");
                completeNoti.ShowDialog();

                utilities.SaveTablePath(saveUtilPath, streamTablePath);
                utilities.SaveTablecount(saveUtilPath, Directory.GetFiles(streamTablePath, "*.xlsx", SearchOption.AllDirectories).Length);
            }

            this.Close();
        }

        void RunExcelManagerReadFiles()
        {
            if(addedFiles != null || deletedFiles != null)
            {
                if (addedFiles != null)
                {
                    excelManager.ReadFilesFromSpecificIO(streamManager.GetStreamTablePath(workingStream), saveDictDataPath, addedFiles);
                }

                if (deletedFiles != null)
                {
                    excelManager.DeleteFiles(saveDictDataPath, deletedFiles);
                }
            }
            else
            {
                excelManager.ReadFilesFromFullio(streamManager.GetStreamTablePath(workingStream), saveDictDataPath);
            }
        }
    }
}
