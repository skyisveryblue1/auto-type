using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace auto_type
{
    public partial class FrmAddEditButton : Form
    {
        private FrmMain frmMain;
        private Button curButton;

        public FrmAddEditButton(FrmMain frmMain, Button curButton = null)
        {
            InitializeComponent();
            this.frmMain = frmMain;
            this.curButton = curButton;

            cmbGroups.Items.AddRange(Data.Groups.ToArray());
            cmbGroups.SelectedItem = frmMain.CurGroup;
            if (curButton != null)
            {
                Text = "Edit Button";
                var buttonInfo = curButton.Tag as ButtonInfo;
                cmbGroups.SelectedItem = buttonInfo.Group;
                txtName.Text = buttonInfo.Name;
                txtHint.Text = buttonInfo.Hint;
                txtTextToType.Text = buttonInfo.TextToType;
                btnExample.Font = buttonInfo.Font;
                btnExample.ForeColor = buttonInfo.TextColor;
                btnExample.BackColor = buttonInfo.BackColor;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtTextToType.Text)  &&
                !string.IsNullOrEmpty(cmbGroups.SelectedItem.ToString()))
            {
                if (curButton != null)
                {
                    var buttonInfo = curButton.Tag as ButtonInfo;
                    buttonInfo.Name = txtName.Text;
                    curButton.Text = buttonInfo.Name;
                    buttonInfo.TextToType = txtTextToType.Text;
                    buttonInfo.Hint = txtHint.Text;
                    buttonInfo.Group = cmbGroups.SelectedItem.ToString();
                    curButton.Font = buttonInfo.Font = btnExample.Font;
                    curButton.ForeColor = buttonInfo.TextColor = btnExample.ForeColor;
                    curButton.BackColor = buttonInfo.BackColor = btnExample.BackColor;
                    (new ToolTip()).SetToolTip(curButton, buttonInfo.Hint);
                }
                else
                {
                    var buttonInfo = new ButtonInfo
                    {
                        Name = txtName.Text,
                        TextToType = txtTextToType.Text,
                        Hint = txtHint.Text,
                        Group = cmbGroups.SelectedItem.ToString(),
                        Index = Data.Buttons.Count > 0 ? Data.Buttons.Max(b => b.Index) + 1 : 0,
                        Font = btnExample.Font,
                        TextColor = btnExample.ForeColor,
                        BackColor = btnExample.BackColor
                    };
                    Data.Buttons.Add(buttonInfo);
                    frmMain.UpdateButtonLayout();
                }
                Data.SaveData();
            }
            else
            {
                MessageBox.Show("Please fill in both name, text, and group fields.");
            }
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            using (var fontDialog = new FontDialog())
            {
                fontDialog.Font = btnExample.Font;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    btnExample.Font = fontDialog.Font;
                }
            }
        }

        private void btnTextColor_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = btnExample.ForeColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    btnExample.ForeColor = colorDialog.Color;
                }
            }
        }

        private void btnBackColor_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = btnExample.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    btnExample.BackColor = colorDialog.Color;
                }
            }
        }
    }
}
