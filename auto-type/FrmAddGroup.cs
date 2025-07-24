using System;
using System.Windows.Forms;

namespace auto_type
{
    public partial class FrmAddGroup : Form
    {
        private FrmMain frmMain;
        public FrmAddGroup(FrmMain frmMain)
        {
            InitializeComponent();
            this.frmMain = frmMain;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGroup.Text) && !Data.Groups.Contains(txtGroup.Text))
            {
                Data.Groups.Add(txtGroup.Text);
                Data.SaveData();
                frmMain.UpdateGroupComboBox();
            }
            else
            {
                MessageBox.Show(frmMain, "Group name must be unique and non-empty.", "Notice");
            }
        }
    }
}
