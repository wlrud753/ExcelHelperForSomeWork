using DocumentFormat.OpenXml.Wordprocessing;
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

            this.ActiveControl = AddStreamDataButton;
        }

        private void StreamManagingForm_Load(object sender, EventArgs e)
        {

        }

        private void AddStreamDataButton_Click(object sender, EventArgs e)
        {
            // Stream Name Validation
            if (AddStreamTextBox.Text == null || AddStreamTextBox.Text == "" || AddStreamPathTextBox.Text == "StreamName")
            {
                PopUpNoti.PopUpNoti noStreamName = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Add Error!", "'스트림 이름'을 입력해주세요!");
                noStreamName.ShowDialog();
                return;
            }
            if (StreamNameList.Items.Contains(AddStreamTextBox.Text))
            {
                PopUpNoti.PopUpNoti existStreamName = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Add Error!", "이미 존재하는 스트림 이름입니다!");
                existStreamName.ShowDialog();
                return;
            }

            // Stream Path Validation
            if (AddStreamPathTextBox.Text == null || AddStreamPathTextBox.Text == "" || AddStreamPathTextBox.Text == "TablePath")
            {
                PopUpNoti.PopUpNoti noStreamPath = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Add Error!", "'스트림 경로'를 입력해주세요!");
                noStreamPath.ShowDialog();
                return;
            }
            if (StreamPathList.Items.Contains(AddStreamPathTextBox.Text))
            {
                PopUpNoti.PopUpNoti existStreamPath = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Add Error!", "이미 존재하는 스트림 경로입니다!");
                existStreamPath.ShowDialog();
                return;
            }
            if (!AddStreamPathTextBox.Text.Contains("GameDesign") || !AddStreamPathTextBox.Text.Contains("Table"))
            {
                PopUpNoti.PopUpNoti pathError = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Path Error!", "테이블 경로가 이상한 것 같슴다?!\n스트림의 \"..\\GameDesign\\Table\" 경로가 들어 있어야 합니다.");
                pathError.ShowDialog();
                return;
            }

            // 동일한 Stream 이름인 경우, 동일한 Path인 경우

            StreamNameList.Items.Add(AddStreamTextBox.Text);
            StreamPathList.Items.Add(AddStreamPathTextBox.Text);

            StreamNameList.Show();
            StreamPathList.Show();

            AddStreamTextBox.Text = null;
            AddStreamPathTextBox.Text = null;
        }

        private void DeleteStreamDataButton_Click(object sender, EventArgs e)
        {
            if (StreamNameList.SelectedItem == null)
                return;

            int selectedIdx = StreamNameList.SelectedIndex;

            StreamNameList.Items.RemoveAt(selectedIdx);
            StreamPathList.Items.RemoveAt(selectedIdx);

            StreamNameList.Show();
            StreamPathList.Show();
        }

        public delegate void saveDataSendDelegate();
        public event saveDataSendDelegate saveDataSend;
        private void SaveStreamDataButton_Click(object sender, EventArgs e)
        {
            if(StreamNameList.Items.Count == 0)
            {
                PopUpNoti.PopUpNoti noDataToSaveWarning = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "No Data!", "저장할 스트림 내역이 없습니다.\n스트림을 추가하고 다시 저장해주세요.");
                noDataToSaveWarning.ShowDialog();
                return;
            }

            List<string[]> tmpStreamData = new List<string[]>();
            
            for(int streamIdx = 0; streamIdx < StreamNameList.Items.Count; streamIdx++)
            {
                string[] tmpStrArr = new string[2];
                tmpStrArr[0] = StreamNameList.Items[streamIdx].ToString();
                tmpStrArr[1] = StreamPathList.Items[streamIdx].ToString();

                tmpStreamData.Add(tmpStrArr);
            }

            streamManager.SaveStreamData(tmpStreamData);

            PopUpNoti.PopUpNoti saveComplete = new PopUpNoti.PopUpNoti(PopUpNoti.PopUpNoti.popupType.noti, "Save Complete!", "저장이 완료되었습니다!");
            saveComplete.ShowDialog();

            saveDataSend();
        }

        #region Util
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
        #endregion
        #region Util for TextBox
        System.Drawing.Font inputFont = new System.Drawing.Font("굴림", 9, FontStyle.Regular);
        System.Drawing.Font nonInputFont = new System.Drawing.Font("굴림", 9, FontStyle.Italic);

        private void AddStreamTextBox_GotFocus(object sender, EventArgs e)
        {
            if(AddStreamTextBox.Text == "StreamName")
            {
                AddStreamTextBox.Text = string.Empty;
                AddStreamTextBox.Font = inputFont;
            }
        }
        private void AddStreamTextBox_LostFocus(object sender, EventArgs e)
        {
            if (AddStreamTextBox.Text == string.Empty)
            {
                AddStreamTextBox.Text = "StreamName";
                AddStreamTextBox.Font = nonInputFont;
                
            }
        }

        private void AddStreamPathTextBox_GotFocus(object sender, EventArgs e)
        {
            if (AddStreamPathTextBox.Text == "TablePath")
            {
                AddStreamPathTextBox.Text = string.Empty;
                AddStreamPathTextBox.Font = inputFont;
            }
        }
        private void AddStreamPathTextBox_LostFocus(object sender, EventArgs e)
        {
            if (AddStreamPathTextBox.Text == string.Empty)
            {
                AddStreamPathTextBox.Text = "TablePath";
                AddStreamPathTextBox.Font= nonInputFont;
            }
        }
        #endregion
    }
}
