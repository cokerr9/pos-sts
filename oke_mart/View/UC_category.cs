using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace oke_mart.View
{
    public partial class UC_category : UserControl
    {
        private readonly Functions myFunction = new Functions();
        private readonly Controller.ControllerCategory myCategory = new Controller.ControllerCategory();

        public UC_category()
        {
            InitializeComponent();

            this.dgCategory.CellClick += new DataGridViewCellEventHandler(this.dgCategory_CellClick);
            this.dgCategory.CellContentClick += new DataGridViewCellEventHandler(this.dgCategory_CellContentClick);
            this.dgCategory.SelectionChanged += new EventHandler(this.dgCategory_SelectionChanged);

            this.btnInsert.Click += new EventHandler(this.btnInsert_Click);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
        }

        private void UC_category_Load(object sender, EventArgs e)
        {
            lblUsername.Text = "Log in as: " + UserDetail.UserName;
            View_Data();
            ResetFormState();
        }

        #region Helper Methods

        public void View_Data()
        {
            // ដក Event ចេញជាបណ្ដោះអាសន្ន ការពារវា Auto-Select ពេល Bind ទិន្នន័យ
            dgCategory.SelectionChanged -= dgCategory_SelectionChanged;

            myCategory.ViewData();

            dgCategory.AutoGenerateColumns = true;
            dgCategory.DataSource = null;
            dgCategory.DataSource = myCategory.dt;
            dgCategory.ForeColor = Color.Black;

            // លាក់ Columns ដែលមិនចង់បង្ហាញ
            if (dgCategory.Columns.Contains("UserId"))
                dgCategory.Columns["UserId"].Visible = false;

            if (dgCategory.Columns.Contains("Create_At"))
                dgCategory.Columns["Create_At"].Visible = false;

            if (dgCategory.Columns.Contains("Update_At"))
                dgCategory.Columns["Update_At"].Visible = false;

            // Clear Selection ដើម្បីកុំឱ្យវា Auto-Select Row ទី ១
            dgCategory.ClearSelection();

            // ភ្ជាប់ Event ចូលវិញ បន្ទាប់ពី Bind រួចរាល់
            dgCategory.SelectionChanged += dgCategory_SelectionChanged;
        }

        private bool IsCategoryToken(string categoryName)
        {
            string sql = "SELECT COUNT(*) FROM tblCategory WHERE CategoryName = @CategoryName";

            using (SqlCommand cmd = new SqlCommand(sql, myCategory.conn))
            {
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                if (myCategory.conn.State == ConnectionState.Closed)
                    myCategory.conn.Open();

                int count = (int)cmd.ExecuteScalar();

                if (myCategory.conn.State == ConnectionState.Open)
                    myCategory.conn.Close();

                return count > 0;
            }
        }

        // កំណត់ UI ឱ្យត្រឡប់ទៅស្ថានភាព Form Load (btnInsert = Add New)
        private void ResetFormState()
        {
            myFunction.ClearField(this);
            myFunction.DisableAllTextAndCombobox(this);

            textCategoryId.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            btnInsert.Text = "Add New";
            btnUpdate.Text = "Update";

            dgCategory.ClearSelection();
        }

        private void EnableInputFields()
        {
            textCategoryName.Enabled = true;
            textDescription.Enabled = true;
            comboBoxStatus.Enabled = true;
            textCategoryId.Enabled = false;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(textCategoryName.Text) || comboBoxStatus.SelectedIndex == -1)
            {
                textCategoryName.Focus();
                return false;
            }
            return true;
        }

        // ទាញទិន្នន័យពី Row ចូល Textbox ហើយដូរប៊ូតុងទៅជា Clear
        private void PopulateFormFromGrid()
        {
            if (dgCategory.CurrentRow == null || dgCategory.CurrentRow.Index < 0 || dgCategory.SelectedRows.Count == 0) return;

            if (dgCategory.CurrentRow.DataBoundItem is DataRowView drv)
            {
                textCategoryId.Text = drv.Row[0]?.ToString() ?? "";
                textCategoryName.Text = drv.Row[1]?.ToString() ?? "";
                textDescription.Text = drv.Row[2]?.ToString() ?? "";
                comboBoxStatus.Text = drv.Row[3]?.ToString() ?? "";
            }
            else
            {
                DataGridViewRow row = dgCategory.CurrentRow;
                textCategoryId.Text = row.Cells[0].Value?.ToString() ?? "";
                textCategoryName.Text = row.Cells[1].Value?.ToString() ?? "";
                textDescription.Text = row.Cells[2].Value?.ToString() ?? "";
                comboBoxStatus.Text = row.Cells[3].Value?.ToString() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(textCategoryId.Text))
            {
                EnableInputFields();
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;

                btnInsert.Text = "Clear";  // ពេល Select Row ដូរទៅជា Clear
                btnUpdate.Text = "Update";
            }
        }

        #endregion

        #region Event Handlers

        private void dgCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                PopulateFormFromGrid();
            }
        }

        private void dgCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                PopulateFormFromGrid();
            }
        }

        private void dgCategory_SelectionChanged(object sender, EventArgs e)
        {
            if (btnInsert.Text == "Insert Data") return;
            PopulateFormFromGrid();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (btnInsert.Text == "Clear")
            {
                // ចុច Clear វានឹង Reset ត្រឡប់មក "Add New" វិញ
                ResetFormState();
            }
            else if (btnInsert.Text == "Add New")
            {
                EnableInputFields();
                myFunction.ClearField(this);

                btnInsert.Text = "Insert Data";
                btnUpdate.Text = "Clear";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = false;

                textCategoryName.Focus();
            }
            else if (btnInsert.Text == "Insert Data")
            {
                if (!ValidateInputs()) return;

                if (IsCategoryToken(textCategoryName.Text.Trim()))
                {
                    MessageBox.Show("Duplicate Category Name! Please check again.", "Duplicate Value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                myCategory.CategoryName = textCategoryName.Text.Trim();
                myCategory.Description = textDescription.Text.Trim();
                myCategory.Status = comboBoxStatus.Text;
                myCategory.UserId = UserDetail.UserId;
                myCategory.Create_At = DateTime.Now;

                myCategory.InsertCategory();

                MessageBox.Show("Category Has Been Inserted Successfully!", "Insert Category", MessageBoxButtons.OK, MessageBoxIcon.Information);

                View_Data();
                ResetFormState();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Clear")
            {
                ResetFormState();
            }
            else if (btnUpdate.Text == "Update")
            {
                if (string.IsNullOrWhiteSpace(textCategoryId.Text))
                {
                    //MessageBox.Show("Please select a Category from the list to update!", "Select Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInputs()) return;

                myCategory.CategoryId = int.Parse(textCategoryId.Text);
                myCategory.CategoryName = textCategoryName.Text.Trim();
                myCategory.Description = textDescription.Text.Trim();
                myCategory.Status = comboBoxStatus.Text;

                myCategory.UpdateCategory();

                MessageBox.Show("Category Has Been Updated Successfully!", "Update Category", MessageBoxButtons.OK, MessageBoxIcon.Information);

                View_Data();
                ResetFormState();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textCategoryId.Text))
            {
                return;
            }

            DialogResult dialog = MessageBox.Show("Are you sure you want to delete this Category?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                myCategory.CategoryId = int.Parse(textCategoryId.Text);
                myCategory.DeleteCategory();

                MessageBox.Show("Category Has Been Deleted Successfully!", "Delete Category", MessageBoxButtons.OK, MessageBoxIcon.Information);

                View_Data();
                ResetFormState();
            }
        }

        #endregion
    }
}