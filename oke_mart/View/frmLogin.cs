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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            Color customColor = ColorTranslator.FromHtml("#7b4325");
            this.BackColor = customColor;
            btnLogin.BackColor = customColor;
            btnLogin.Enabled = false;
            ForeColor = Color.White;
            textPassword.Enabled = false;
            btnLogin.ForeColor = Color.White;
        }

        private void textUsername_TextChanged(object sender, EventArgs e)
        {
            if (textUsername.Text != "")
            {
                textPassword.Enabled=true;
            }
            else
            {
                textPassword.Enabled=false;
            }
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            if (textPassword.Text != "")
            {
                btnLogin.Enabled=true;

            }
            else
            {
                btnLogin.Enabled=false;
            }
        }

        db_Connection myCon = new db_Connection();
        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = myCon.conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM tblUsers WHERE UserName = @UserName AND Password = @Password";
            cmd.Parameters.AddWithValue("@UserName", textUsername.Text.Trim());
            cmd.Parameters.AddWithValue("@Password", textPassword.Text.Trim());
            DataTable dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                UserDetail.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                UserDetail.UserName = dt.Rows[0]["UserName"].ToString();
                UserDetail.UserType = dt.Rows[0]["UserType"].ToString();
                this.Hide();
                View.Dashboard MyDshboard = new View.Dashboard();
                MyDshboard.Show();
            }
            else
            {
                MessageBox.Show("Incurrent User Or Password", "Invalid User Or Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textPassword.Clear();
                textPassword.Focus();
            }
        }
    }
}
