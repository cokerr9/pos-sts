using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace oke_mart.View
{
    public partial class UC_supplier : UserControl
    {
        public UC_supplier()
        {
            InitializeComponent();
        }

        Functions myFunction = new Functions();
        Controller.ControllerSupplier mySupplier = new Controller.ControllerSupplier();

        private void UC_supplier_Load(object sender, EventArgs e)
        {
            dgSupplier.CellMouseClick += dgSupplier_CellMouseClick;
            this.BackColor = ColorTranslator.FromHtml("#F8FAFC");

            Color btnColor = ColorTranslator.FromHtml("#0EA5E9");

            btnInsert.BackColor = btnColor;
            btnInsert.ForeColor = Color.White;
            btnUpdate.BackColor = btnColor;
            btnUpdate.ForeColor = Color.White;
            btnDelete.BackColor = btnColor;
            btnDelete.ForeColor = Color.White;

            myFunction.DisableAllTextAndCombobox(this);
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            View_Data();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (btnInsert.Text == "Add New")
            {
                myFunction.EnableAllTextAndCombobox(this);
                textSupplierId.Enabled = false;
                btnUpdate.Text = "Clear";
                btnUpdate.Enabled = true;
                btnInsert.Text = "Insert Data";
            }
            else if (btnInsert.Text == "Insert Data")
            {
                if (string.IsNullOrWhiteSpace(textCompanyName.Text) || string.IsNullOrWhiteSpace(textSupplierName.Text))
                {
                    MessageBox.Show("Please fill in the Company Name and Supplier Name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                mySupplier.CompanyName = textCompanyName.Text;
                mySupplier.SupplierName = textSupplierName.Text;
                mySupplier.PhoneNumber = textPhoneNumber.Text;
                mySupplier.Address = textAddress.Text;
                mySupplier.UserId = UserDetail.UserId;

                mySupplier.InsertSupplier();
                View_Data();

                MessageBox.Show("Supplier has been Inserted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnInsert.Text = "Add New";
                btnUpdate.Enabled = false;
                btnUpdate.Text = "Update";
            }
            else if (btnInsert.Text == "Clear")
            {
                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnInsert.Text = "Add New";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnUpdate.Text = "Update";
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Clear")
            {
                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnInsert.Text = "Add New";
                btnUpdate.Enabled = false;
                btnUpdate.Text = "Update";
                btnDelete.Enabled = false;
            }
            else if (btnUpdate.Text == "Update")
            {
                // 🟢 ថែម Validation ការពារអ្នកប្រើប្រាស់វាយប្រអប់ទទេហើយចុច Update
                if (string.IsNullOrWhiteSpace(textCompanyName.Text) || string.IsNullOrWhiteSpace(textSupplierName.Text))
                {
                    MessageBox.Show("Please fill in the Company Name and Supplier Name before updating.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                mySupplier.SupplierId = Convert.ToInt32(textSupplierId.Text);
                mySupplier.CompanyName = textCompanyName.Text;
                mySupplier.SupplierName = textSupplierName.Text;
                mySupplier.PhoneNumber = textPhoneNumber.Text;
                mySupplier.Address = textAddress.Text;
                mySupplier.UserId = UserDetail.UserId;

                mySupplier.UpdateSupplier();

                MessageBox.Show("Supplier has been Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                View_Data();

                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnInsert.Text = "Add New";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        public void View_Data()
        {
            mySupplier.ViewData();
            dgSupplier.DataSource = mySupplier.dt;
        }

        private void dgSupplier_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgSupplier.Rows[e.RowIndex];

                textSupplierId.Text = row.Cells[0].Value?.ToString();
                textCompanyName.Text = row.Cells[1].Value?.ToString();
                textSupplierName.Text = row.Cells[2].Value?.ToString();
                textPhoneNumber.Text = row.Cells[3].Value?.ToString();
                textAddress.Text = row.Cells[4].Value?.ToString();

                myFunction.EnableAllTextAndCombobox(this);
                textSupplierId.Enabled = false;

                btnInsert.Text = "Clear";
                btnUpdate.Text = "Update";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textSupplierId.Text))
            {
                MessageBox.Show("Please select a supplier from the table first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this supplier?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    mySupplier.SupplierId = Convert.ToInt32(textSupplierId.Text);
                    mySupplier.DeleteSupplier();

                    MessageBox.Show("Supplier has been successfully deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    View_Data();

                    myFunction.ClearField(this);
                    myFunction.DisableAllTextAndCombobox(this);
                    btnInsert.Text = "Add New";
                    btnUpdate.Text = "Update";
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        MessageBox.Show("Cannot delete this Supplier because they have Products linked to them.\n\nPlease delete or reassign their products first.",
                                        "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("System Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}