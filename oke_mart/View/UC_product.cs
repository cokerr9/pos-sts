using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace oke_mart.View
{
    public partial class UC_product : UserControl
    {
        public UC_product()
        {
            InitializeComponent();
        }

        // Call function 
        Functions myFunction = new Functions();
        Controller.ControllerProduct myProduct = new Controller.ControllerProduct();

        private void UC_product_Load(object sender, EventArgs e)
        {
            Color customColor = ColorTranslator.FromHtml("#e7e9f2");
            this.BackColor = customColor;
            myFunction.DisableAllTextAndCombobox(this);
            dgProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgProduct.DefaultCellStyle.ForeColor = Color.Black;
            dgProduct.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgProduct.AllowUserToAddRows = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            // 1. Load Category Data
            myProduct.ViewCategory();
            cboCategory.DataSource = myProduct.DtCa;
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryId";

            myProduct.ViewSupplier();
            cboSupplier.DataSource = myProduct.DtSu;
            cboSupplier.DisplayMember = "SupplierName";
            cboSupplier.ValueMember = "SupplierId";
            View_Data();
        }

        // Check Duplicate Data
        private bool IsProductNameToken(string barCode, string productName)
        {
            string sql = "SELECT COUNT(*) FROM tblProduct " +
                "WHERE BarCode = @Barcode OR ProductName = @productName";

            using (SqlCommand cmd = new SqlCommand(sql, myProduct.conn))
            {
                cmd.Parameters.AddWithValue("@Barcode", barCode);
                cmd.Parameters.AddWithValue("@productName", productName);

                bool openedByUs = false;

                try
                {
                    if (myProduct.conn.State == ConnectionState.Closed)
                    {
                        myProduct.conn.Open();
                        openedByUs = true;
                    }

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                finally
                {
                    if (openedByUs && myProduct.conn.State == ConnectionState.Open)
                    {
                        myProduct.conn.Close();
                    }
                }
            }
        }

        // Insert Product
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (btnInsert.Text == "Add New")
            {
                myFunction.EnableAllTextAndCombobox(this);
                textProductId.Enabled = false;
                textUnitPrice.Enabled = false;
                textQtyInStock.Enabled = false;
                linkAddPicture.Enabled = true;
                btnUpdate.Enabled = true;
                textUnitPrice.Text = 0.ToString();
                textQtyInStock.Text = 0.ToString();
                btnUpdate.Text = "Clear";
                btnInsert.Text = "Insert Data";
            }
            else if (btnInsert.Text == "Insert Data")
            {
                // 1. Check for Duplicate Product
                if (IsProductNameToken(textBarCode.Text, textProductName.Text))
                {
                    MessageBox.Show("Your Product already exists.",
                    "Please Check your Product again",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (proPhoto.Image == null)
                {
                    MessageBox.Show("Please check your values before Insert.", "Check Pls",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                byte[] arr = null;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    proPhoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    arr = ms.ToArray();
                }

                myProduct.CategoryId = Convert.ToInt16(cboCategory.SelectedValue);
                myProduct.Barcode = textBarCode.Text;
                myProduct.ProductName = textProductName.Text;
                myProduct.SupplierId = Convert.ToInt16(cboSupplier.SelectedValue);
                myProduct.UnitPrice = Convert.ToDouble(textUnitPrice.Text);
                myProduct.SalePrice = Convert.ToDouble(textSalePrice.Text);
                myProduct.QtyInStock = Convert.ToInt16(textQtyInStock.Text);
                myProduct.ExpireDate = DateTime.Parse(dataTime.Text);
                myProduct.Status = cboStatus.SelectedItem.ToString();

                myProduct.Photo = arr;
                myProduct.UserCreate = UserDetail.UserId;
                myProduct.Create_At = DateTime.Now;

                myProduct.InsertProduct();
                View_Data();

                MessageBox.Show("Your Product Has Been Inserted",
                "Insert Product", MessageBoxButtons.OK, MessageBoxIcon.Information);

                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnInsert.Text = "Add New";
                btnUpdate.Text = "Update";
                btnUpdate.Enabled = false;
                proPhoto.Image = null;
            }
            else if (btnInsert.Text == "Clear")
            {
                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnUpdate.Text = "Update"; 
                btnInsert.Text = "Add New";
                proPhoto.Image = null;
                textUnitPrice.Text = "0";
                textQtyInStock.Text = "0";
            }
        }

        // Call Method (V_Product) from ControllerProduct
        public void View_Data()
        {
            myProduct.ViewData();
            dgProduct.AutoGenerateColumns = false;
            dgProduct.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgProduct.DataSource = myProduct.dt;

            if (dgProduct.Columns["UnitPrice"] != null)
            {
                dgProduct.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
            }
            if (dgProduct.Columns["SalePrice"] != null)
            {
                dgProduct.Columns["SalePrice"].DefaultCellStyle.Format = "N2";
            }
        }

        // Update Product 
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Clear")
            {
                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnUpdate.Text = "Update";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnInsert.Text = "Add New";
                linkAddPicture.Enabled = false;
                proPhoto.Image = null;
            }
            else if (btnUpdate.Text == "Update")
            {
                if (proPhoto.Image == null)
                {
                    MessageBox.Show("Please check your photo before Update.", "Check Pls",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] arr = null;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    proPhoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    arr = ms.ToArray();
                }
                myProduct.ProductId = Convert.ToInt32(textProductId.Text);
                myProduct.CategoryId = Convert.ToInt16(cboCategory.SelectedValue);
                myProduct.Barcode = textBarCode.Text;
                myProduct.ProductName = textProductName.Text;
                myProduct.SupplierId = Convert.ToInt16(cboSupplier.SelectedValue);
                myProduct.UnitPrice = Convert.ToDouble(textUnitPrice.Text, CultureInfo.InvariantCulture);
                myProduct.SalePrice = Convert.ToDouble(textSalePrice.Text, CultureInfo.InvariantCulture);
                myProduct.QtyInStock = Convert.ToInt16(textQtyInStock.Text);
                myProduct.ExpireDate = DateTime.Parse(dataTime.Text);
                myProduct.Status = cboStatus.SelectedItem.ToString();

                myProduct.Photo = arr;
                myProduct.UserUpdate = UserDetail.UserId;
                myProduct.Update_At = DateTime.Now;

                myProduct.UpdateProduct();
                View_Data();

                MessageBox.Show("Your Product Has Been Updated!", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnInsert.Text = "Add New";
                proPhoto.Image = null;
            }
        }

        // Add Picture 
        private void linkAddPicture_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog oFt = new OpenFileDialog();
            oFt.Filter = "Choose Image(*.jpg;*.png;*.gif;)|*.jpg;*.png;*.gif;";
            if (oFt.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = new FileStream(oFt.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (Image img = Image.FromStream(stream))
                    {
                        proPhoto.Image = new Bitmap(img);
                    }
                }
                proPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        // Delete Button
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure to delete this Product?",
                "Confirm Data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                myProduct.ProductId = Convert.ToInt32(textProductId.Text);
                myProduct.DeleteProduct();
                View_Data();

                MessageBox.Show("Your Product Has Been Deleted", "Delete Product",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

                myFunction.ClearField(this);
                myFunction.DisableAllTextAndCombobox(this);
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                btnInsert.Text = "Add New";
                proPhoto.Image = null;
            }
        }

        private void dgProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
            {
                DataRowView row = (DataRowView)dgProduct.Rows[e.RowIndex].DataBoundItem;

                textProductId.Text = row["ProductId"].ToString();
                cboCategory.SelectedValue = row["CategoryId"];
                textBarCode.Text = row["Barcode"].ToString();
                textProductName.Text = row["ProductName"].ToString();
                cboSupplier.SelectedValue = row["SupplierId"];
                textUnitPrice.Text = Convert.ToDouble(row["UnitPrice"]).ToString("0.00");
                textSalePrice.Text = Convert.ToDouble(row["SalePrice"]).ToString("0.00");
                object qtyVal = row.Row.Table.Columns.Contains("QtyInStock") ? row["QtyInStock"] :
                               (row.Row.Table.Columns.Contains("QtyInStcok") ? row["QtyInStcok"] :
                               (row.Row.Table.Columns.Contains("QuantityInStcok") ? row["QuantityInStcok"] : 0));
                textQtyInStock.Text = qtyVal?.ToString() ?? "0";
                dataTime.Text = row["ExpireDate"].ToString();
                cboStatus.Text = row["Status"].ToString();

                if (row["Photo"] != DBNull.Value)
                {
                    byte[] imgData = (byte[])row["Photo"];
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        using (Image tempImg = Image.FromStream(ms))
                        {
                            proPhoto.Image = new Bitmap(tempImg);
                        }
                        proPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    proPhoto.Image = null;
                }

                myFunction.EnableAllTextAndCombobox(this);
                textProductId.Enabled = false;
                linkAddPicture.Enabled = true;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;

                btnUpdate.Text = "Update";
                btnInsert.Text = "Clear";
            }
        }
    }
}