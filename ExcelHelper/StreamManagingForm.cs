using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamManagingForm
{
    public partial class StreamManagingForm : Form
    {
        StreamManager.StreamManager streamManager;

        public StreamManagingForm()
        {
            InitializeComponent();

            streamManager = new StreamManager.StreamManager();

            if(streamManager.HasSavedStreamData())
                SetStreamDataList();

        }

        private void StreamManagingForm_Load(object sender, EventArgs e)
        {

        }

        private void AddStreamDataButton_Click(object sender, EventArgs e)
        {
            if (!AddStreamPathTextBox.Text.Contains("GameDesign") || !AddStreamPathTextBox.Text.Contains("Table"))
            {
                PopUpNoti.PopUpNoti pathError = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Path Error!", "테이블 경로가 이상한 것 같슴다?!\n스트림의 \"..\\GameDesign\\Table\"이 들어 있어야 합니다.");
                pathError.ShowDialog();
            }
            else
            {

            }
        }

        void SetStreamDataList()
        {
            List<string[]> streamData = streamManager.GetStreamData();

            foreach (string[] data in streamData)
            {
                StreamNameList.Items.Add(data[0]);
                StreamPathList.Items.Add(data[1]);

                StreamNameList.Show();
                StreamPathList.Show();
            }
        }
    }
}
