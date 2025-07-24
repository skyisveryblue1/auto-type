using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace auto_type
{
    public partial class FrmCategories : Form
    {
        private FrmMain frmMain;
        public FrmCategories(FrmMain frmMain)
        {
            InitializeComponent();
            this.frmMain = frmMain;

            lstCategories.Items.AddRange(Data.Categories.ToArray());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtCategory.Text) && !Data.Categories.Contains(txtCategory.Text))
            {
                Data.Categories.Add(txtCategory.Text);
                lstCategories.Items.Add(txtCategory.Text);
                Data.SaveData();
                frmMain.UpdateCategoryComboBox();
            }
            else
            {
                MessageBox.Show("Category name must be unique and non-empty.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItem != null && lstCategories.SelectedItem.ToString() != "Default")
            {
                if (!string.IsNullOrEmpty(txtCategory.Text) && !Data.Categories.Contains(txtCategory.Text))
                {
                    var oldCategory = lstCategories.SelectedItem.ToString();
                    var index = Data.Categories.IndexOf(oldCategory);
                    Data.Categories[index] = txtCategory.Text;

                    foreach (var button in Data.Buttons.Where(b => b.Category == oldCategory))
                    {
                        button.Category = txtCategory.Text;
                    }

                    lstCategories.Items[lstCategories.SelectedIndex] = txtCategory.Text;
                    Data.SaveData();
                    frmMain.UpdateCategoryComboBox();
                }
                else
                {
                    MessageBox.Show("Category name must be unique and non-empty.");
                }
            }
            else
            {
                MessageBox.Show("Select a non-Default category to edit.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItem != null && lstCategories.SelectedItem.ToString() != "Default")
            {
                var category = lstCategories.SelectedItem.ToString();
                if (MessageBox.Show($"Delete category '{category}' and move its buttons to Default?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (var button in Data.Buttons.Where(b => b.Category == category))
                    {
                        button.Category = "Default";
                    }
                    Data.Categories.Remove(category);
                    lstCategories.Items.Remove(lstCategories.SelectedItem);
                    Data.SaveData();
                    frmMain.UpdateCategoryComboBox();
                    frmMain.UpdateButtonLayout();
                }
            }
            else
            {
                MessageBox.Show("Select a non-Default category to delete.");
            }
        }
    }
}
