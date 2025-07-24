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

        private GlobalMouseHook mouseHook;

        private string curTextToType = "";

        public FrmMain()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(screen.Width - this.Width, screen.Height - this.Height - 150);

            Data.LoadData();
            UpdateGroupComboBox();

            cmsButton.Opening += ContextMenu_Button_Opening;
            miStopTyping.Enabled = false;

            // Mouse events for dragging
            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;

            dragRectangle = new Rectangle(0, 0, this.Width, pnlButtons.Top);
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
                    curTextToType = "";
                    miStopTyping.Enabled = false;
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


            int buttonWidth = this.ClientSize.Width - 25;
            int buttonHeight = 25;
            var toolTip = new ToolTip();
            foreach (var buttonInfo in sortedButtons)
            {
                var button = new Button
                {
                    Text = buttonInfo.Name,
                    Tag = buttonInfo,
                    Width = buttonWidth,
                    Height = buttonHeight,
                    Margin = new Padding(5, 0, 5, 0),
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
            miStopTyping.Enabled = !string.IsNullOrEmpty(curTextToType);
            if (mouseHook == null)
            {
                mouseHook = new GlobalMouseHook();
                mouseHook.MouseClick += MouseHook_MouseClick;
            }
        }

        private void miAddGroup_Click(object sender, EventArgs e)
        {
            miStopTyping.PerformClick();
            (new FrmAddGroup(this)).ShowDialog();
        }

        private void miRenameGroup_Click(object sender, EventArgs e)
        {
            miStopTyping.PerformClick();
            (new FrmRenameGroup(this, cmbGroups.SelectedItem.ToString())).ShowDialog();
        }

        private void miDeleteGroup_Click(object sender, EventArgs e)
        {
            miStopTyping.PerformClick();
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
            miStopTyping.PerformClick();
            (new FrmAddEditButton(this)).ShowDialog();
        }

        private void miEditButton_Click(object sender, EventArgs e)
        {
            miStopTyping.PerformClick();
            (new FrmAddEditButton(this, ctrlSourceByButtonMenu as Button)).ShowDialog();
        }

        private void miDeleteButton_Click(object sender, EventArgs e)
        {
            miStopTyping.PerformClick();
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

        private void btnTools_Click(object sender, EventArgs e)
        {
            miStopTyping.PerformClick();
            if (cmsTool.Visible)
                cmsTool.Close();
            else
                cmsTool.Show(btnTools, -cmsTool.Width + btnTools.Width, btnTools.Height);
        }

        private void miStopTyping_Click(object sender, EventArgs e)
        {
            curTextToType = "";
            miStopTyping.Enabled = false;
        }
        private void miToggleHeader_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = miToggleHeader.Text == "Hide Header" ? FormBorderStyle.None : FormBorderStyle.SizableToolWindow;
            miToggleHeader.Text = miToggleHeader.Text == "Hide Header" ? "Show Header" : "Hide header";
        }

        private bool isDragging;
        private Point lastCursorPosition;
        private Rectangle dragRectangle;
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (dragRectangle.Contains(e.Location) && miToggleHeader.Text != "Hide Header")
            {
                isDragging = true;
                lastCursorPosition = e.Location;
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                this.Location = new Point(
                    this.Location.X + (e.X - lastCursorPosition.X),
                    this.Location.Y + (e.Y - lastCursorPosition.Y));
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (pnlButtons != null)
            {
                pnlButtons.Size = new Size(this.ClientSize.Width - 10, this.ClientSize.Height - 45);
                UpdateButtonLayout();
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mouseHook?.Dispose();
        }      
    }
}
