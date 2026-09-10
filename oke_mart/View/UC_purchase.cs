using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace oke_mart.View
{
    public partial class UC_purchase : UserControl
    {
        public UC_purchase()
        {
            InitializeComponent();
        }

        // Call Controller and Functions
        Controller.ControllerPurchase myPurchase = new Controller.ControllerPurchase();
        Functions myFunction = new Functions();

        private void UC_purchase_Load(object sender, EventArgs e)
        {
            Color customColor = ColorTranslator.FromHtml("#e7e9f2");
            this.BackColor = customColor;

            dgPurchase.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgPurchase.DefaultCellStyle.ForeColor = Color.Black;
            dgPurchase.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgPurchase.AllowUserToAddRows = false;

            myFunction.DisableAllTextAndCombobox(this);
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            View_Product();
            View_Data();
        }

        // Helper: Reset form state
        private void ResetFormState()
        {
            myFunction.ClearField(this);
            myFunction.DisableAllTextAndCombobox(this);
            btnInsert.Text = "Add New";
            btnInsert.Enabled = true;
            btnUpdate.Text = "Update";
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            if (dgPurchase.Rows.Count > 0)
            {
                dgPurchase.ClearSelection();
            }
        }

        // Helper: Calculate Total Price = Unit Price * Quantity
        private void CalculateTotal()
        {
            if (int.TryParse(txtQty.Text.Trim(), out int qty) && qty >= 0 &&
                (int.TryParse(txtUnitPrice.Text.Trim(), out int unitPrice) ||
                 (decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal decPrice) && (unitPrice = (int)Math.Round(decPrice)) >= 0)))
            {
                txtTotalPrice.Text = (unitPrice * qty).ToString();
            }
            else
            {
                txtTotalPrice.Text = "0";
            }
        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void cboxProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxProductName.SelectedIndex != -1 && cboxProductName.SelectedValue != null && myPurchase.dtPro != null)
            {
                if (int.TryParse(cboxProductName.SelectedValue.ToString(), out int productId))
                {
                    foreach (DataRow row in myPurchase.dtPro.Rows)
                    {
                        if (Convert.ToInt32(row["ProductId"]) == productId)
                        {
                            if (row["UnitPrice"] != DBNull.Value)
                            {
                                txtUnitPrice.Text = Convert.ToInt32(Convert.ToDouble(row["UnitPrice"])).ToString();
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (btnInsert.Text == "Add New")
            {
                myFunction.EnableAllTextAndCombobox(this);
                txtPurchaseId.Enabled = false;
                txtTotalPrice.Enabled = false;
                txtQty.Enabled = true; // Enabled so user can input quantity
                txtUnitPrice.Enabled = true;

                btnUpdate.Text = "Clear";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = false;
                btnInsert.Text = "Insert";

                txtPurchaseId.Clear();
                txtQty.Clear();
                txtUnitPrice.Clear();
                txtTotalPrice.Clear();

                // If a product is already selected, auto-populate its unit price
                cboxProductName_SelectedIndexChanged(null, null);
                txtQty.Focus();
            }
            else if (btnInsert.Text == "Insert")
            {
                // Validation 1: Product Selection
                if (cboxProductName.SelectedIndex == -1 || cboxProductName.SelectedValue == null ||
                    !int.TryParse(cboxProductName.SelectedValue.ToString(), out int productId))
                {
                    MessageBox.Show("Please select a valid product.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboxProductName.Focus();
                    return;
                }

                // Validation 2: Unit Price
                int unitPrice = 0;
                if (string.IsNullOrWhiteSpace(txtUnitPrice.Text) ||
                    (!int.TryParse(txtUnitPrice.Text.Trim(), out unitPrice) &&
                     (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal decPrice) || (unitPrice = (int)Math.Round(decPrice)) < 0)))
                {
                    MessageBox.Show("Please enter a valid Unit Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUnitPrice.Focus();
                    return;
                }

                // Validation 3: Quantity
                if (string.IsNullOrWhiteSpace(txtQty.Text) || !int.TryParse(txtQty.Text.Trim(), out int qty) || qty <= 0)
                {
                    MessageBox.Show("Please enter a valid Quantity (must be greater than 0).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQty.Focus();
                    return;
                }

                // Calculate Total Price
                int totalPrice = unitPrice * qty;
                txtTotalPrice.Text = totalPrice.ToString();

                int userId = UserDetail.UserId > 0 ? UserDetail.UserId : 16;

                myPurchase.ProductId = productId;
                myPurchase.UnitPrice = unitPrice;
                myPurchase.Quantity = qty;
                myPurchase.TotalPrice = totalPrice;
                myPurchase.PurchaseDate = DateTime.Now;
                myPurchase.UserId = userId;

                try
                {
                    myPurchase.InsertPurchase();
                    MessageBox.Show("Success For Insert Purchase", "Inserted Purchase",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetFormState();
                    View_Data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting purchase: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (btnInsert.Text == "Clear")
            {
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
                if (string.IsNullOrWhiteSpace(txtPurchaseId.Text) || !int.TryParse(txtPurchaseId.Text.Trim(), out int purchaseId))
                {
                    MessageBox.Show("Please select a purchase from the list to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboxProductName.SelectedIndex == -1 || cboxProductName.SelectedValue == null ||
                    !int.TryParse(cboxProductName.SelectedValue.ToString(), out int productId))
                {
                    MessageBox.Show("Please select a valid product.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboxProductName.Focus();
                    return;
                }

                int unitPrice = 0;
                if (string.IsNullOrWhiteSpace(txtUnitPrice.Text) ||
                    (!int.TryParse(txtUnitPrice.Text.Trim(), out unitPrice) &&
                     (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal decPrice) || (unitPrice = (int)Math.Round(decPrice)) < 0)))
                {
                    MessageBox.Show("Please enter a valid Unit Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUnitPrice.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtQty.Text) || !int.TryParse(txtQty.Text.Trim(), out int qty) || qty <= 0)
                {
                    MessageBox.Show("Please enter a valid Quantity (must be greater than 0).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQty.Focus();
                    return;
                }

                int totalPrice = unitPrice * qty;
                txtTotalPrice.Text = totalPrice.ToString();

                int userId = UserDetail.UserId > 0 ? UserDetail.UserId : 16;

                myPurchase.PurchaseId = purchaseId;
                myPurchase.ProductId = productId;
                myPurchase.UnitPrice = unitPrice;
                myPurchase.Quantity = qty;
                myPurchase.TotalPrice = totalPrice;
                myPurchase.PurchaseDate = DateTime.Now;
                myPurchase.UserId = userId;

                try
                {
                    myPurchase.UpdatePurchase();
                    MessageBox.Show("Purchase updated successfully!", "Update Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetFormState();
                    View_Data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating purchase: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPurchaseId.Text) || !int.TryParse(txtPurchaseId.Text.Trim(), out int purchaseId))
            {
                MessageBox.Show("Please select a purchase to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this purchase?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    myPurchase.PurchaseId = purchaseId;
                    myPurchase.DeletePurchase();
                    MessageBox.Show("Purchase has been deleted.", "Delete Purchase",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetFormState();
                    View_Data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting purchase: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgPurchase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgPurchase.Rows[e.RowIndex].DataBoundItem is DataRowView row)
            {
                txtPurchaseId.Text = row["PurchaseId"].ToString();
                cboxProductName.SelectedValue = row["ProductId"];
                txtUnitPrice.Text = row["UnitPrice"].ToString();
                txtQty.Text = row["Quantity"].ToString();
                txtTotalPrice.Text = (row["TotalPrice"] ?? row["TotalPirce"]).ToString();

                myFunction.EnableAllTextAndCombobox(this);
                txtPurchaseId.Enabled = false;
                txtTotalPrice.Enabled = false;
                txtQty.Enabled = true;
                txtUnitPrice.Enabled = true;

                btnInsert.Text = "Clear";
                btnInsert.Enabled = true;
                btnUpdate.Text = "Update";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        public void View_Product()
        {
            cboxProductName.DataSource = null;
            myPurchase.ViewProduct();
            cboxProductName.DataSource = myPurchase.dtPro;
            cboxProductName.DisplayMember = "ProductName";
            cboxProductName.ValueMember = "ProductId";
        }

        public void View_Data()
        {
            myPurchase.ViewPurchase();
            dgPurchase.AutoGenerateColumns = false;
            dgPurchase.DataSource = null;
            dgPurchase.DataSource = myPurchase.dt;
        }
    }
}
