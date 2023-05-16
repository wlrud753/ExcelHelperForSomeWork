using ExcelHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergeForm
{
    public partial class MergeForm : Form
    {
        ExcelManager.ExcelManager excelManager;

        public MergeForm(ExcelManager.ExcelManager _excelManager)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.excelManager = _excelManager;

            foreach (string item in this.excelManager.GetAllDividedData())
                DataListBox.Items.Add(item);
        }


        private void DataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DataListBox.SelectedItem != null)
            {
                FileListBox.Items.Clear();
                foreach (string item in this.excelManager.GetFilesInSpecificData(DataListBox.SelectedItem.ToString()))
                    FileListBox.Items.Add(Path.GetFileName(item)); // 경로 제거하고, File 이름만 보이도록 설정
            }
        }

        private void MergeButton_Click(object sender, EventArgs e)
        {
            if (DataListBox.SelectedItem != null)
            {
                string savePath = Application.StartupPath + "\\MergedTable";
                if(!Directory.Exists(savePath)) { Directory.CreateDirectory(savePath); }

                excelManager.Merge(savePath, DataListBox.SelectedItem.ToString());

                PopUpNoti.PopUpNoti completeNoti = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Merge Complete!", DataListBox.SelectedItem + " Data의 머지가 완료되었습니다.\n(통합 테이블이 자동으로 열립니다)");
                completeNoti.ShowDialog();

                System.Diagnostics.Process.Start(excelManager.GetMergedTableName(savePath, DataListBox.SelectedItem.ToString()));
            }
        }
    }
}
