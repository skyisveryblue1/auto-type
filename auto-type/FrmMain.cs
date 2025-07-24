using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace auto_type
{
    public partial class FrmMain : Form
    {
        private string currentCategory = "Default";

        public FrmMain()
        {
            InitializeComponent();

            Data.LoadData();
            UpdateCategoryComboBox();
        }
      
        // Manage the categories
        private void btnManageCategory_Click(object sender, EventArgs e)
        {
            FrmCategories frmCategories = new FrmCategories(this);
            frmCategories.ShowDialog();
        }


        internal void UpdateCategoryComboBox()
        {
            cmbCategories.Items.Clear();
            cmbCategories.Items.AddRange(Data.Categories.OrderBy(c => c).ToArray());
            cmbCategories.SelectedItem = Data.Categories.Contains(currentCategory) ? currentCategory : "Default";
            currentCategory = cmbCategories.SelectedItem?.ToString() ?? "Default";
        }

        internal void UpdateButtonLayout()
        {
            pnlButtons.Controls.Clear();
            var sortedButtons = Data.Buttons
                .Where(b => b.Category == currentCategory)
                .OrderBy(b => b.Index)
                .ThenBy(b => b.Name)
                .ToList();

            int columns = Math.Max(1, this.ClientSize.Width / 120);
            int buttonWidth = (this.ClientSize.Width - 20) / columns - 10;
            int buttonHeight = 40;

            foreach (var buttonInfo in sortedButtons)
            {
                var button = new Button
                {
                    Text = $"{buttonInfo.Index}: {buttonInfo.Name}",
                    Tag = buttonInfo,
                    Width = buttonWidth,
                    Height = buttonHeight,
                    //Font = buttonFont,
                    //ForeColor = buttonTextColor
                };
                button.Click += TypeButton_Click;
                pnlButtons.Controls.Add(button);
            }
        }

        private void TypeButton_Click(object sender, EventArgs e)
        {
            var buttonInfo = (ButtonInfo)((Button)sender).Tag;
            System.Threading.Thread.Sleep(100);
            SendKeys.SendWait(buttonInfo.TextToType);
        }

    }
}
