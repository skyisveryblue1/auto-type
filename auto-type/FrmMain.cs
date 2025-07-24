using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace auto_type
{
    public partial class FrmMain : Form
    {
        public string CurGroup { get; set; }

        public FrmMain()
        {
            InitializeComponent();

            Data.LoadData();
            UpdateGroupComboBox();

            cmsButton.Opening += ContextMenu_Button_Opening;
        }

        private void cmbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurGroup = cmbGroups.SelectedItem?.ToString() ?? "";
            UpdateButtonLayout();
        }

        internal void UpdateGroupComboBox(string activeGroup = "")
        {
            cmbGroups.Items.Clear();
            cmbGroups.Items.AddRange(Data.Groups.OrderBy(c => c).ToArray());
            cmbGroups.SelectedItem = activeGroup == "" ? (String.IsNullOrEmpty(CurGroup) ? cmbGroups.Items[0] : CurGroup) : activeGroup;
            CurGroup = cmbGroups.SelectedItem?.ToString() ?? "";
        }

        internal void UpdateButtonLayout()
        {
            pnlButtons.Controls.Clear();
            var sortedButtons = Data.Buttons
                .Where(b => b.Group == CurGroup)
                .OrderBy(b => b.Index)
                .ThenBy(b => b.Name)
                .ToList();

            int columns = Math.Max(1, this.ClientSize.Width / 120);
            int buttonWidth = (this.ClientSize.Width - 20) / columns - 10;
            int buttonHeight = 40;
            var toolTip = new ToolTip();
            foreach (var buttonInfo in sortedButtons)
            {
                var button = new Button
                {
                    Text = $"{buttonInfo.Index}: {buttonInfo.Name}",
                    Tag = buttonInfo,
                    Width = buttonWidth,
                    Height = buttonHeight,
                    ContextMenuStrip = cmsButton,
                    Font = buttonInfo.Font,
                    ForeColor = buttonInfo.TextColor,
                    BackColor = buttonInfo.BackColor,
                };
                button.Click += TypeButton_Click;
                toolTip.SetToolTip(button, buttonInfo.Hint);
                pnlButtons.Controls.Add(button);
            }
        }

        private void TypeButton_Click(object sender, EventArgs e)
        {
            var buttonInfo = (ButtonInfo)((Button)sender).Tag;
            System.Threading.Thread.Sleep(100);
            SendKeys.SendWait(buttonInfo.TextToType);
        }

        private void miAddGroup_Click(object sender, EventArgs e)
        {
            (new FrmAddGroup(this)).ShowDialog();
        }

        private void miRenameGroup_Click(object sender, EventArgs e)
        {
            (new FrmRenameGroup(this, cmbGroups.SelectedItem.ToString())).ShowDialog();
        }

        private void miDeleteGroup_Click(object sender, EventArgs e)
        {
            if (cmbGroups.SelectedItem != null)
            {
                if (cmbGroups.Items.Count < 2)
                {
                    MessageBox.Show(this, "You can't delete the last group.", "Notice");
                    return;
                }
                var group = cmbGroups.SelectedItem.ToString();
                if (MessageBox.Show(this, $"Are you sure you want to delete [{group}] group and its buttons?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Data.Buttons.RemoveAll(b => b.Group == group);
                    Data.Groups.Remove(group);
                    cmbGroups.Items.Remove(cmbGroups.SelectedItem);
                    Data.SaveData();
                    UpdateGroupComboBox();
                    UpdateButtonLayout();
                }
            }
            else
            {
                MessageBox.Show(this, "Select a group to delete.", "Notice");
            }
        }

        private Control ctrlSourceByButtonMenu;

        private void ContextMenu_Button_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            if (menu != null)
                ctrlSourceByButtonMenu = menu.SourceControl;
        }
        private void miAddButton_Click(object sender, EventArgs e)
        {
            (new FrmAddEditButton(this)).ShowDialog();
        }

        private void miEditButton_Click(object sender, EventArgs e)
        {
            (new FrmAddEditButton(this, ctrlSourceByButtonMenu as Button)).ShowDialog();
        }

        private void miDeleteButton_Click(object sender, EventArgs e)
        {
            Button button = ctrlSourceByButtonMenu as Button;
            if (button != null)
            {
                var buttonInfo = button.Tag as ButtonInfo;
                if (MessageBox.Show(this, $"Are you sure you want to delete [{buttonInfo.Name}] button?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Data.Buttons.RemoveAll(b => b.Index == buttonInfo.Index);
                    Data.SaveData();
                    UpdateButtonLayout();
                }
            }
        }

    }
}
