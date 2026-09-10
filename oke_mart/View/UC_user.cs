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
    public partial class UC_user : UserControl
    {
        public UC_user()
        {
            InitializeComponent();
        }

        // Call from Class Functions
        Functions myfunction = new Functions();
        Controller.ControllerUsers myUser = new Controller.ControllerUsers();

        private void UC_user_Load(object sender, EventArgs e)
        {
            Color customColor = ColorTranslator.FromHtml("#e7e9f2");
            this.BackColor = customColor;
            myfunction.DisableAllTextAndCombobox(this);
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            checkShowPass.Enabled = false;
            ViewData();
            dgViewUsers.DefaultCellStyle.ForeColor = Color.Black;
        }

        // Check if Username already exists
        private bool IsUserTypeTaken(string username)
        {
            string sql = "SELECT count(*) FROM tblUsers where Username = @UserName;";
            using (SqlCommand cmd = new SqlCommand(sql, myUser.conn))
            {
                cmd.Parameters.AddWithValue("@UserName", username);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        // Dynamically populate role options based on current user privileges
        private void PopulateUserTypeCombo()
        {
            comboUserType.Items.Clear();

            if (UserDetail.UserType == "Super_Admin")
            {
                // Super_Admin: Full Control
                comboUserType.Items.Add("Super_Admin");
                comboUserType.Items.Add("Admin");
                comboUserType.Items.Add("User");
            }
            else if (UserDetail.UserType == "Admin")
            {
                // Admin: CANNOT create Super_Admin or Admin
                comboUserType.Items.Add("User");
            }
            else
            {
                comboUserType.Items.Add("User");
            }

            if (comboUserType.Items.Count > 0)
            {
                comboUserType.SelectedIndex = 0;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (btnInsert.Text == "Add New")
            {
                myfunction.EnableAllTextAndCombobox(this);
                PopulateUserTypeCombo(); // Populate allowed dropdown items

                textUserId.Enabled = false;
                btnInsert.Text = "Insert Data";
                btnUpdate.Text = "Clear";
                btnUpdate.Enabled = true;
                checkShowPass.Enabled = true;
            }
            else if (btnInsert.Text == "Insert Data")
            {
                // 1. Validation for empty fields
                if (string.IsNullOrWhiteSpace(textUserName.Text) ||
                    string.IsNullOrWhiteSpace(textPassword.Text) ||
                    comboUserType.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Strict Role Restriction: Admin CANNOT insert Super_Admin or Admin
                if (UserDetail.UserType == "Admin" && comboUserType.Text != "User")
                {
                    MessageBox.Show("Admins can only insert regular 'User' accounts.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // 3. Username uniqueness check
                if (IsUserTypeTaken(textUserName.Text))
                {
                    MessageBox.Show("UserName Already exists", "Pls Choose a Different User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4. Perform Insert
                myUser.UserName = textUserName.Text;
                myUser.Password = textPassword.Text;
                myUser.UserType = comboUserType.Text;
                myUser.InsertUser();

                MessageBox.Show("User Inserted", "Insert User", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 5. Reset UI State cleanly
                myfunction.ClearField(this);
                myfunction.DisableAllTextAndCombobox(this);

                btnInsert.Text = "Add New";
                btnInsert.Enabled = true;

                btnUpdate.Text = "Update";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                checkShowPass.Enabled = false;
                checkShowPass.Checked = false;
                textPassword.BackColor = Color.White;

                ViewData();
            }
            else if (btnInsert.Text == "Clear")
            {
                myfunction.ClearField(this);
                myfunction.DisableAllTextAndCombobox(this);
                btnInsert.Text = "Add New";
                btnUpdate.Text = "Update";
                btnUpdate.Enabled = false;
                checkShowPass.Checked = false;
                textPassword.BackColor = Color.White;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Clear")
            {
                myfunction.ClearField(this);
                myfunction.DisableAllTextAndCombobox(this);
                btnInsert.Text = "Add New";
                btnUpdate.Text = "Update";
                btnUpdate.Enabled = false;
                btnInsert.Enabled = true;
                checkShowPass.Checked = false;
                textUserId.Enabled = true;
                textPassword.BackColor = Color.White;
            }
            else if (btnUpdate.Text == "Update")
            {
                if (string.IsNullOrWhiteSpace(textUserId.Text))
                {
                    MessageBox.Show("Please select a user to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedUserId = int.Parse(textUserId.Text);

                // Find original UserType of selected record from DataGridView
                string targetOriginalUserType = "";
                foreach (DataGridViewRow row in dgViewUsers.Rows)
                {
                    if (row.Cells[0].Value != null && int.Parse(row.Cells[0].Value.ToString()) == selectedUserId)
                    {
                        targetOriginalUserType = row.Cells[3].Value?.ToString();
                        break;
                    }
                }

                // Guard 1: Normal User can only update their own profile
                if (UserDetail.UserType == "User" && UserDetail.UserId != selectedUserId)
                {
                    MessageBox.Show("Users can only update their own account profile.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Guard 2: Admin CANNOT update a Super_Admin user account
                if (UserDetail.UserType == "Admin" && targetOriginalUserType == "Super_Admin")
                {
                    MessageBox.Show("Admins do not have permission to modify Super Admin accounts.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Guard 3: Admin CANNOT change user role to Admin or Super_Admin
                if (UserDetail.UserType == "Admin" && comboUserType.Text != "User")
                {
                    MessageBox.Show("Admins cannot elevate account roles to Admin or Super Admin.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                myUser.UserId = selectedUserId;
                myUser.UserName = textUserName.Text;
                myUser.Password = textPassword.Text;
                myUser.UserType = comboUserType.Text;

                myUser.UpdateUsers();
                MessageBox.Show("User Updated Successfully", "Update User", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset Form State
                myfunction.ClearField(this);
                myfunction.DisableAllTextAndCombobox(this);
                btnInsert.Text = "Add New";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                comboUserType.Enabled = false;
                textPassword.BackColor = Color.White;

                ViewData();
            }
        }

        private void checkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (textPassword.PasswordChar == '*')
            {
                textPassword.PasswordChar = '\0';
            }
            else if (textPassword.PasswordChar == '\0')
            {
                textPassword.PasswordChar = '*';
            }
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            if (!textPassword.Enabled) return;

            if (textPassword.Text.Length < 8)
            {
                textPassword.BackColor = Color.Red;
                if (btnInsert.Text == "Insert Data")
                {
                    btnInsert.Enabled = false;
                }
            }
            else
            {
                textPassword.BackColor = Color.LightGreen;
                if (btnInsert.Text == "Insert Data")
                {
                    btnInsert.Enabled = true;
                }
            }
        }

        public void ViewData()
        {
            myUser.ShowData();
            dgViewUsers.DataSource = myUser.dt;
        }

        private void dgViewUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgViewUsers.Columns[e.ColumnIndex].Index == 2 && e.Value != null)
            {
                dgViewUsers.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new string('*', e.Value.ToString().Length);
            }
        }

        private void dgViewUsers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // Prevent header click errors

                // Detach handler temporarily to avoid false triggers while clicking rows
                comboUserType.SelectedIndexChanged -= comboUserType_SelectedIndexChanged;

                if (dgViewUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    textUserId.Text = dgViewUsers.Rows[e.RowIndex].Cells[0].Value?.ToString();
                    textUserName.Text = dgViewUsers.Rows[e.RowIndex].Cells[1].Value?.ToString();
                    textPassword.Text = dgViewUsers.Rows[e.RowIndex].Cells[2].Value?.ToString();
                    comboUserType.Text = dgViewUsers.Rows[e.RowIndex].Cells[3].Value?.ToString();
                }

                myfunction.EnableAllTextAndCombobox(this);
                textUserId.Enabled = false;
                btnInsert.Text = "Clear";

                if (btnUpdate.Text == "Clear")
                {
                    btnUpdate.Text = "Update";
                }

                int selectedUserId = int.Parse(textUserId.Text);
                string selectedUserType = comboUserType.Text;

                // ==========================================
                //  PERMISSION LOGIC FOR UI BUTTON CONTROLS
                // ==========================================

                // 1. UPDATE RULE
                if (UserDetail.UserType == "User")
                {
                    btnUpdate.Enabled = (UserDetail.UserId == selectedUserId);
                }
                else if (UserDetail.UserType == "Admin")
                {
                    // Admin CANNOT update Super_Admin users
                    btnUpdate.Enabled = (selectedUserType != "Super_Admin");
                }
                else
                {
                    btnUpdate.Enabled = true; // Super_Admin
                }

                // 2. DELETE RULES
                if (UserDetail.UserId == selectedUserId)
                {
                    btnDelete.Enabled = false; // Cannot delete logged-in account
                }
                else if (UserDetail.UserType == "Super_Admin")
                {
                    btnDelete.Enabled = true;
                }
                else if (UserDetail.UserType == "Admin")
                {
                    // Admin cannot delete User ID 1 or higher-tier users (Admin / Super_Admin)
                    if (selectedUserId == 1)
                    {
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        btnDelete.Enabled = (selectedUserType == "User");
                    }
                }
                else if (UserDetail.UserType == "User")
                {
                    btnDelete.Enabled = false;
                }
            }
            catch
            {
                // Handle exceptions
            }
            finally
            {
                // Re-attach event handler safely
                comboUserType.SelectedIndexChanged += comboUserType_SelectedIndexChanged;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textUserId.Text))
            {
                MessageBox.Show("Please select a user to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedUserId = int.Parse(textUserId.Text);
            string selectedUserType = comboUserType.Text;

            // Extra security checks
            if (UserDetail.UserType == "Admin" && selectedUserId == 1)
            {
                MessageBox.Show("Admins are not allowed to delete User ID 1.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (UserDetail.UserType == "Admin" && selectedUserType != "User")
            {
                MessageBox.Show("Admins do not have permission to delete Admin or Super Admin accounts.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (UserDetail.UserType == "User")
            {
                MessageBox.Show("Users do not have permission to delete accounts.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Do you want to Delete this user?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                myUser.UserId = selectedUserId;
                myUser.DeleteUsers();

                MessageBox.Show("User Has Been Deleted", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Information);

                myfunction.ClearField(this);
                myfunction.DisableAllTextAndCombobox(this);

                ViewData();

                btnInsert.Text = "Add New";
                btnInsert.Enabled = true;

                btnUpdate.Text = "Update";
                btnUpdate.Enabled = false;

                btnDelete.Enabled = false;
                checkShowPass.Enabled = false;
                checkShowPass.Checked = false;
                textPassword.BackColor = Color.White;
            }
        }

        private void comboUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textUserName.Text) || string.IsNullOrWhiteSpace(textPassword.Text))
            {
                if (btnInsert.Text == "Insert Data")
                {
                    btnInsert.Enabled = false;
                }
            }

            // Interactive Guard: Block Admin from selecting non-User roles when adding data
            if (UserDetail.UserType == "Admin" &&
                btnInsert.Text == "Insert Data" &&
                comboUserType.Focused &&
                !string.IsNullOrWhiteSpace(comboUserType.Text) &&
                comboUserType.Text != "User")
            {
                MessageBox.Show("Admins can only assign the 'User' role.", "Permission Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboUserType.Text = "User";
            }
        }
    }
}