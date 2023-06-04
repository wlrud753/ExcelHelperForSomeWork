using System;
using System.Threading;
using System.Windows.Forms;

namespace PopUpNoti
{
    public partial class PopUpNoti : Form
    {
        public enum popupType { ask, noti, working };

        Thread thread = null;

        public PopUpNoti(popupType _popupType, string _popupReason, string _NotiText)
        {
            InitializeComponent();

            // Form 초기화
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            NotiTextLabel.Left = (this.Width - NotiTextLabel.Width) / 2;

            this.Text = _popupReason;
            NotiTextLabel.Text = _NotiText;
            NotiTextLabel.Left = (this.Width - NotiTextLabel.Width) / 2;

            ConfirmationButton.Width = WorkCancelButton.Width = 85;

            switch (_popupType)
            {
                case popupType.ask:
                    ConfirmationButton.Left = (this.Width - (ConfirmationButton.Width + WorkCancelButton.Width)) / 2;
                    WorkCancelButton.Left = (this.Width - (ConfirmationButton.Width - WorkCancelButton.Width)) / 2;

                    WorkProgressBar.Visible = false;

                    break;
                case popupType.noti:
                    WorkCancelButton.Visible = false;
                    ConfirmationButton.Left = (this.Width - ConfirmationButton.Width) / 2;

                    WorkProgressBar.Visible = false;

                    break;
                case popupType.working:
                    WorkCancelButton.Visible = false;
                    ConfirmationButton.Visible = false;

                    NotiTextLabel.Top = (this.Height - NotiTextLabel.Height) / 5;

                    WorkProgressBar.Visible = true;
                    WorkProgressBar.Left = (this.Width - WorkProgressBar.Width) / 2;
                    WorkProgressBar.Top = (this.Height - WorkProgressBar.Top) * 3 / 5;
                    WorkProgressBar.Enabled = true;

                    this.TopMost = false;
                    this.MinimizeBox = true;

                    break;
                default:
                    break;
            }
        }
        public void ThreadStart()
        {
            thread = new Thread(ChangeProgressBar);

            thread.Start();
        }
        private void ChangeProgressBar()
        {
            int progress = 0;
            while (true)
            {
                if (this.InvokeRequired)
                {
                    if (progress >= 100)
                        progress = 0;

                    //this.Invoke(new Action(() => { WorkProgressBar.Value = progress; }));
                    this.BeginInvoke(new Action(() => { WorkProgressBar.Value = progress; this.Refresh(); }));

                    progress += 5;
                    Thread.Sleep(200);
                }
            }
        }
        public void ThreadEnd()
        {
            thread.Abort();
        }

        private void PopUpNoti_Load(object sender, EventArgs e)
        {

        }

        private void ConfirmationButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WorkCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
