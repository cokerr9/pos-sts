using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace oke_mart.View
{
    public partial class UC_sale : UserControl
    {
        public UC_sale()
        {
            InitializeComponent();
            InitializePOSControls();
        }

        // Controller instance
        private readonly Controller.ControllerSale mysale = new Controller.ControllerSale();
        private int count = 0;

        // Convenient aliases matching your naming style
        public TextBox txtbarcode => textBox3;
        public TextBox txtproduct => textBox4;
        public DataGridView dgsale => dgFinalSale;
        public DataGridView dgcart => dataGridView1;
        public DataGridView dgfinalsale => dataGridView1;
        public TextBox txttotalpayment => textBox1;
        public TextBox txtpayment => textBox2;
        public Button btnpayment => button1;

        private void InitializePOSControls()
        {
            // Wire event handlers to Designer controls (no dynamic controls created)
            this.Load += UC_sale_Load;
            this.dgFinalSale.CellMouseClick += dgsale_CellMouseClick;
            this.dataGridView1.CellEndEdit += dgfinalsale_CellEndEdit;
            this.dataGridView1.RowsAdded += dgfinalsale_RowsAdded;
            this.dataGridView1.RowsRemoved += dgfinalsale_RowsRemoved;
            this.dataGridView1.CellValueChanged += dgfinalsale_CellValueChanged;
            
            // Allow deleting items from the cart using Delete key or double click
            this.dataGridView1.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete)
                {
                    btnremove_Click(s, e);
                }
            };
            this.dataGridView1.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    btnremove_Click(s, e);
                }
            };
        }

        private void SetupDataGridViews()
        {
            // Set DataPropertyName for each column in dgFinalSale so data actually shows up
            if (dgFinalSale.Columns.Contains("Column1")) dgFinalSale.Columns["Column1"].DataPropertyName = "ProductId";
            if (dgFinalSale.Columns.Contains("Column2")) dgFinalSale.Columns["Column2"].DataPropertyName = "CategoryName";
            if (dgFinalSale.Columns.Contains("Column3")) dgFinalSale.Columns["Column3"].DataPropertyName = "Barcode";
            if (dgFinalSale.Columns.Contains("Column4")) dgFinalSale.Columns["Column4"].DataPropertyName = "ProductName";
            if (dgFinalSale.Columns.Contains("Column5")) dgFinalSale.Columns["Column5"].DataPropertyName = "SalePrice";
            if (dgFinalSale.Columns.Contains("Column6")) dgFinalSale.Columns["Column6"].DataPropertyName = "QtyInStock";
            if (dgFinalSale.Columns.Contains("Column7")) dgFinalSale.Columns["Column7"].DataPropertyName = "Photo";

            if (dgFinalSale.Columns.Count >= 6)
            {
                dgFinalSale.Columns[0].DataPropertyName = "ProductId";
                dgFinalSale.Columns[1].DataPropertyName = "CategoryName";
                dgFinalSale.Columns[2].DataPropertyName = "Barcode";
                dgFinalSale.Columns[3].DataPropertyName = "ProductName";
                dgFinalSale.Columns[4].DataPropertyName = "SalePrice";
                dgFinalSale.Columns[5].DataPropertyName = "QtyInStock";
            }

            // Hide raw binary photo column in the text grid
            if (dgFinalSale.Columns.Contains("Column7"))
                dgFinalSale.Columns["Column7"].Visible = false;
            else if (dgFinalSale.Columns.Count > 6)
                dgFinalSale.Columns[6].Visible = false;

            // Clean white backgrounds and high-contrast text for both grids
            dgFinalSale.BackgroundColor = Color.White;
            dgFinalSale.GridColor = Color.FromArgb(226, 232, 240);
            dgFinalSale.DefaultCellStyle.BackColor = Color.White;
            dgFinalSale.DefaultCellStyle.ForeColor = Color.Black;
            dgFinalSale.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#0EA5E9");
            dgFinalSale.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.GridColor = Color.FromArgb(226, 232, 240);
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#0EA5E9");
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.ReadOnly = false;

            // Make only the Quantity column editable in the cart grid
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (i == 3 || dataGridView1.Columns[i].Name == "Column11")
                    dataGridView1.Columns[i].ReadOnly = false;
                else
                    dataGridView1.Columns[i].ReadOnly = true;
            }
        }

        private void UC_sale_Load(object sender, EventArgs e)
        {
            // Clean, clear UI colors
            this.BackColor = ColorTranslator.FromHtml("#F8FAFC");

            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;

            textBox1.BackColor = Color.White;
            textBox1.ForeColor = Color.Black;
            textBox1.ReadOnly = true;

            textBox2.BackColor = Color.White;
            textBox2.ForeColor = Color.Black;

            button1.BackColor = ColorTranslator.FromHtml("#0EA5E9");
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;

            View_Data();
            CountRecript();

            txttotalpayment.Enabled = false;
            btnpayment.Enabled = false;
        }

        public void View_Data()
        {
            try
            {
                SetupDataGridViews();
                mysale.ViewData();
                dgFinalSale.AutoGenerateColumns = false;
                dgFinalSale.DataSource = null;
                dgFinalSale.DataSource = mysale.dt;
                dgFinalSale.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CountRecript()
        {
            try
            {
                if (mysale.conn.State == ConnectionState.Closed)
                    mysale.conn.Open();

                string sql = @"
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'tblReceipt')
                        SELECT COUNT(*) FROM tblReceipt;
                    ELSE
                        SELECT ISNULL(MAX(ReceiptId), 0) FROM tblSale;";

                using (SqlCommand cmd = new SqlCommand(sql, mysale.conn))
                {
                    object result = cmd.ExecuteScalar();
                    count = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                    lbrecript.Text = "RM" + (count + 1).ToString();
                }
            }
            catch
            {
                count = 0;
                lbrecript.Text = "RM1";
            }
        }

        private void Search_Barcode()
        {
            try
            {
                if (mysale.conn.State == ConnectionState.Closed)
                    mysale.conn.Open();

                if (string.IsNullOrWhiteSpace(txtbarcode.Text))
                {
                    View_Data();
                    return;
                }

                using (SqlDataAdapter adapterba = new SqlDataAdapter("SELECT * FROM v_ProductsForSale WHERE Barcode LIKE @Barcode", mysale.conn))
                {
                    adapterba.SelectCommand.Parameters.AddWithValue("@Barcode", "%" + txtbarcode.Text.Trim() + "%");
                    DataTable dtba = new DataTable();
                    adapterba.Fill(dtba);
                    SetupDataGridViews();
                    dgFinalSale.AutoGenerateColumns = false;
                    dgFinalSale.DataSource = dtba;
                }
            }
            catch { }
        }

        private void Search_Product()
        {
            try
            {
                if (mysale.conn.State == ConnectionState.Closed)
                    mysale.conn.Open();

                if (string.IsNullOrWhiteSpace(txtproduct.Text))
                {
                    View_Data();
                    return;
                }

                using (SqlDataAdapter adapterpd = new SqlDataAdapter("SELECT * FROM v_ProductsForSale WHERE ProductName LIKE @ProductName", mysale.conn))
                {
                    adapterpd.SelectCommand.Parameters.AddWithValue("@ProductName", "%" + txtproduct.Text.Trim() + "%");
                    DataTable dtpd = new DataTable();
                    adapterpd.Fill(dtpd);
                    SetupDataGridViews();
                    dgFinalSale.AutoGenerateColumns = false;
                    dgFinalSale.DataSource = dtpd;
                }
            }
            catch { }
        }

        private void txtbarcode_TextChanged(object sender, EventArgs e)
        {
            Search_Barcode();
        }

        private void txtproduct_TextChanged(object sender, EventArgs e)
        {
            Search_Product();
        }

        private void dgsale_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                DataGridViewRow selectedrow = dgFinalSale.Rows[e.RowIndex];

                int productid = 0;
                string productname = "";
                decimal price = 0;
                int stock = 0;

                // Extract via DataRowView if bound, otherwise from row cells
                if (selectedrow.DataBoundItem is DataRowView rowView)
                {
                    productid = Convert.ToInt32(rowView["ProductId"]);
                    productname = rowView["ProductName"]?.ToString() ?? "";
                    price = Convert.ToDecimal(rowView["SalePrice"] ?? 0);
                    stock = Convert.ToInt32(rowView["QtyInStock"] ?? 0);
                }
                else
                {
                    productid = Convert.ToInt32(selectedrow.Cells[0].Value ?? 0);
                    productname = selectedrow.Cells[3].Value?.ToString() ?? "";
                    price = Convert.ToDecimal(selectedrow.Cells[4].Value ?? 0);
                    stock = Convert.ToInt32(selectedrow.Cells[5].Value ?? 0);
                }

                if (stock <= 0)
                {
                    MessageBox.Show("This product is currently out of stock!", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if product is already in the cart (dataGridView1)
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value != null && Convert.ToInt32(row.Cells[0].Value) == productid)
                    {
                        int oldqty = Convert.ToInt32(row.Cells[3].Value ?? 0);
                        if (oldqty >= stock)
                        {
                            MessageBox.Show("Cannot add more. Available stock is: " + stock.ToString(), "Stock Limit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        int newqty = oldqty + 1;
                        row.Cells[3].Value = newqty;
                        row.Cells[4].Value = (price * newqty).ToString("0.00");
                        UpdateTotal();
                        return;
                    }
                }

                // Add as new row in the cart
                int qty = 1;
                decimal amount = price * qty;
                dataGridView1.Rows.Add(productid, productname, price.ToString("0.00"), qty, amount.ToString("0.00"));
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgfinalsale_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                decimal price = Convert.ToDecimal(row.Cells[2].Value ?? 0);
                int qty = Convert.ToInt32(row.Cells[3].Value ?? 1);
                if (qty <= 0)
                {
                    qty = 1;
                    row.Cells[3].Value = 1;
                }

                decimal amount = price * qty;
                row.Cells[4].Value = amount.ToString("0.00");
                UpdateTotal();
            }
            catch { }
        }

        private void UpdateTotal()
        {
            try
            {
                decimal total = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    decimal totalPrice = Convert.ToDecimal(row.Cells[4].Value ?? 0);
                    total += totalPrice;
                }

                txttotalpayment.Text = total.ToString("0.00");
                ValidatePayment();
            }
            catch { }
        }

        private void ValidatePayment()
        {
            try
            {
                decimal total = 0;
                decimal payment = 0;
                decimal.TryParse(txttotalpayment.Text, out total);
                decimal.TryParse(txtpayment.Text, out payment);

                if (total > 0 && (txtpayment.Text.Trim() == txttotalpayment.Text.Trim() || payment >= total))
                {
                    btnpayment.Enabled = true;
                }
                else
                {
                    btnpayment.Enabled = false;
                }
            }
            catch
            {
                btnpayment.Enabled = false;
            }
        }

        private void dgfinalsale_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateTotal();
        }

        private void dgfinalsale_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateTotal();
        }

        private void dgfinalsale_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTotal();
        }

        private void txtpayment_TextChanged(object sender, EventArgs e)
        {
            ValidatePayment();
        }

        private void btnpayment_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Please select products before paying.", "Cart is Empty", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal total = Convert.ToDecimal(txttotalpayment.Text);
                decimal payment = Convert.ToDecimal(txtpayment.Text);
                decimal change = payment - total;
                int userId = (UserDetail.UserId > 0) ? UserDetail.UserId : 1;
                string receiptNum = lbrecript.Text.Trim();
                if (string.IsNullOrEmpty(receiptNum))
                {
                    receiptNum = "RM" + (count + 1);
                }

                // 1. Insert receipt header into tblReceipt
                int currentReceiptId = mysale.InsertReceipt(receiptNum, total, payment, change, DateTime.Now, userId);
                if (currentReceiptId <= 0)
                {
                    currentReceiptId = count + 1;
                }

                // 2. Insert line items into tblSale
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    mysale.ReceiptId = currentReceiptId;
                    mysale.ProductId = Convert.ToInt32(row.Cells[0].Value);
                    mysale.Price = Convert.ToDecimal(row.Cells[2].Value);
                    mysale.Qty = Convert.ToInt32(row.Cells[3].Value);
                    mysale.TotalPrice = Convert.ToDecimal(row.Cells[4].Value);
                    mysale.SaleDate = DateTime.Today;
                    mysale.UserId = userId;
                    mysale.InsertSale();
                }

                string msg = $"Thank You For Your Payment!\n\nReceipt: {receiptNum}\nTotal: ${total:F2}\nPaid: ${payment:F2}";
                if (change > 0)
                {
                    msg += $"\nChange: ${change:F2}";
                }

                MessageBox.Show(msg, "Payment Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.Rows.Clear();
                txttotalpayment.Clear();
                txtpayment.Clear();
                btnpayment.Enabled = false;

                CountRecript();
                View_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing payment: " + ex.Message, "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                {
                    if (!r.IsNewRow)
                    {
                        dataGridView1.Rows.Remove(r);
                    }
                }
                UpdateTotal();
            }
            else if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                UpdateTotal();
            }
            else
            {
                MessageBox.Show("Please select an item in the cart to remove.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnpayment_Click(sender, e);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
