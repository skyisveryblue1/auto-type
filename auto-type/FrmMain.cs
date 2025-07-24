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

        private GlobalMouseHook mouseHook = new GlobalMouseHook();

        private string curTextToType = "";

        public FrmMain()
        {
            InitializeComponent();

            Data.LoadData();
            UpdateGroupComboBox();

            cmsButton.Opening += ContextMenu_Button_Opening;

            mouseHook.MouseClick += MouseHook_MouseClick;

            btnStopAutoTyping.Enabled = false;
        }

        private void MouseHook_MouseClick(object sender, MouseEventArgs e)
        {
            lock (mouseHook) // Synchronize access to CurTextToType
            {
                string textToType = curTextToType;
                if (string.IsNullOrEmpty(textToType))
                    return;
            }

            // Marshal to UI thread to access form properties
            if (InvokeRequired)
            {
                Invoke(new Action<object, MouseEventArgs>(MouseHook_MouseClick), sender, e);
                return;
            }

            // Check if mouse is within form bounds
            bool isMouseInForm = Bounds.Contains(e.X, e.Y);
            Console.WriteLine($"Mouse {e.Button} clicked at ({e.X}, {e.Y}), In Form: {isMouseInForm}");

            if (isMouseInForm) return;

            System.Threading.Thread.Sleep(400); // Delay of 400ms
            lock (mouseHook) // Synchronize access to CurTextToType
            {
                string textToType = curTextToType;
                try
                {
                    Console.WriteLine($"Sending keys: {textToType}");
                    SendKeys.SendWait(textToType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending keys: {ex.Message}");
                }
            }
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
            if (Data.Buttons == null) return;

            var sortedButtons = Data.Buttons
                .Where(b => b.Group == CurGroup)
                .OrderBy(b => b.Index)
                .ThenBy(b => b.Name)
                .ToList();

            int columns = Math.Max(1, this.ClientSize.Width / 120);
            int buttonWidth = (this.ClientSize.Width - 20) / columns - 3;
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
            curTextToType = buttonInfo.TextToType;
            btnStopAutoTyping.Enabled = !string.IsNullOrEmpty(curTextToType);
        }

        private void miAddGroup_Click(object sender, EventArgs e)
        {
            btnStopAutoTyping.PerformClick();
            (new FrmAddGroup(this)).ShowDialog();
        }

        private void miRenameGroup_Click(object sender, EventArgs e)
        {
            btnStopAutoTyping.PerformClick();
            (new FrmRenameGroup(this, cmbGroups.SelectedItem.ToString())).ShowDialog();
        }

        private void miDeleteGroup_Click(object sender, EventArgs e)
        {
            btnStopAutoTyping.PerformClick();
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
            btnStopAutoTyping.PerformClick();
            (new FrmAddEditButton(this)).ShowDialog();
        }

        private void miEditButton_Click(object sender, EventArgs e)
        {
            btnStopAutoTyping.PerformClick();
            (new FrmAddEditButton(this, ctrlSourceByButtonMenu as Button)).ShowDialog();
        }

        private void miDeleteButton_Click(object sender, EventArgs e)
        {
            btnStopAutoTyping.PerformClick();
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
        private void btnStopAutoTyping_Click(object sender, EventArgs e)
        {
            curTextToType = "";
            btnStopAutoTyping.Enabled = false;
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            btnStopAutoTyping.PerformClick();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (pnlButtons != null)
            {
                pnlButtons.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 45);
                UpdateButtonLayout();
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mouseHook?.Dispose();
        }

    }
}
