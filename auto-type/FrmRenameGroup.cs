using System;
using System.Linq;
using System.Windows.Forms;

namespace auto_type
{
    public partial class FrmRenameGroup : Form
    {
        private FrmMain frmMain;
        private string oldGroup;
        public FrmRenameGroup(FrmMain frmMain, string curGroup)
        {
            InitializeComponent();
            this.frmMain = frmMain;
            txtGroup.Text = curGroup; oldGroup = curGroup;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGroup.Text) && !Data.Groups.Contains(txtGroup.Text))
            {
                var index = Data.Groups.IndexOf(oldGroup);
                Data.Groups[index] = txtGroup.Text;

                foreach (var button in Data.Buttons.Where(b => b.Group == oldGroup))
                {
                    button.Group = txtGroup.Text;
                }

                Data.SaveData();
                frmMain.UpdateGroupComboBox(txtGroup.Text);
            }
            else
            {
                MessageBox.Show(frmMain, "Category name must be unique and non-empty.", "Notice");
            }
        }
    }
}
