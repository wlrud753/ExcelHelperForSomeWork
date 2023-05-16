using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopUpNoti
{
    public partial class PopUpNoti : Form
    {
        public enum popupType { ask, noti, working };
        public PopUpNoti(popupType _popupType, string _popupReason, string _NotiText)
        {
            InitializeComponent();

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
                    break;
                case popupType.noti:
                    WorkCancelButton.Visible = false;
                    ConfirmationButton.Left = (this.Width - ConfirmationButton.Width) / 2;
                    break;
                case popupType.working:
                    WorkCancelButton.Visible = false;
                    ConfirmationButton.Left = (this.Width - ConfirmationButton.Width) / 2;

                    this.TopMost = false;
                    break;
                default:
                    break;
            }
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
