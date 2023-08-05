using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        }

        private void CheckReacceptableButton_Click(object sender, EventArgs e)
        {
            questManager.FindQuestReacceptable();
        }

        private void QuestForm_Load(object sender, EventArgs e)
        {

        }
    }
}
