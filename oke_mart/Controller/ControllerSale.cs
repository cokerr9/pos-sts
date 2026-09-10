using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Controller
{
    internal class ControllerSale : Model.ModelSale
    {
        // View Data
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        public SqlDataAdapter adapter = new SqlDataAdapter();

        public void ViewData()
        {
            ViewSale();
        }

        public void ViewSale()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            string sql = "SELECT * FROM v_ProductsForSale WHERE Status = 'Yes' OR Status IS NULL;";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                dt.Clear();
                da.Fill(dt);
            }
        }

        public DataTable SearchBarcode(string barcode)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            DataTable dtSearch = new DataTable();
            string sql = "SELECT * FROM v_ProductsForSale WHERE Barcode LIKE @Barcode AND (Status = 'Yes' OR Status IS NULL);";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Barcode", "%" + (barcode ?? "").Trim() + "%");
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtSearch);
                }
            }
            return dtSearch;
        }

        public DataTable SearchProduct(string productName)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            DataTable dtSearch = new DataTable();
            string sql = "SELECT * FROM v_ProductsForSale WHERE ProductName LIKE @ProductName AND (Status = 'Yes' OR Status IS NULL);";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProductName", "%" + (productName ?? "").Trim() + "%");
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtSearch);
                }
            }
            return dtSearch;
        }

        public int GetReceiptCount()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            string sql = @"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'tblReceipt')
                    SELECT COUNT(*) FROM tblReceipt;
                ELSE
                    SELECT ISNULL(MAX(ReceiptId), 0) FROM tblSale;";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                object result = cmd.ExecuteScalar();
                return (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
            }
        }

                public int InsertReceipt(string receiptNumber, decimal totalAmount, decimal payment, decimal change, DateTime saleDate, int userId)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertReceipt", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReceiptNumber", SqlDbType.NVarChar, 50).Value = receiptNumber ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = totalAmount;
                    cmd.Parameters.Add("@Payment", SqlDbType.Decimal).Value = payment;
                    cmd.Parameters.Add("@Change", SqlDbType.Decimal).Value = change;
                    cmd.Parameters.Add("@SaleDate", SqlDbType.DateTime).Value = saleDate;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = (userId > 0) ? (object)userId : (object)DBNull.Value;

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch
            {
                string sql = @"
                    IF (@UserId <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UserId))
                    BEGIN
                        SET @UserId = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
                    END

                    INSERT INTO tblReceipt (ReceiptNumber, TotalAmount, Payment, Change, SaleDate, UserId)
                    VALUES (@ReceiptNumber, @TotalAmount, @Payment, @Change, @SaleDate, @UserId);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ReceiptNumber", receiptNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@Payment", payment);
                    cmd.Parameters.AddWithValue("@Change", change);
                    cmd.Parameters.AddWithValue("@SaleDate", saleDate);
                    cmd.Parameters.AddWithValue("@UserId", userId > 0 ? userId : (object)DBNull.Value);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }

            return 0;
        }

        public void InsertSale()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (SqlCommand cmd = new SqlCommand("InsertSale", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ReceiptId", SqlDbType.Int).Value = ReceiptId;
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = ProductId;
                cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = Price;
                cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = Qty;
                cmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = TotalPrice;
                cmd.Parameters.Add("@SaleDate", SqlDbType.Date).Value = SaleDate;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = (UserId > 0) ? (object)UserId : (object)DBNull.Value;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
