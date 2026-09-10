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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            UC_Dashboard uc_user = new UC_Dashboard();
            AddUserControl(uc_user);
        }

        // call user control to show on sale pn
        private void AddUserControl(UserControl userControl)
        {
            pnShow.Controls.Clear();
            userControl.Location = new Point(
             (pnShow.Width - userControl.Width) / 2
            );
            pnShow.Controls.Add(userControl);
            userControl.BringToFront();
        }

        // Set active color to Menu Item in Nabar
        private void SetActiveMenu(ToolStripMenuItem selected)
        {
            foreach (ToolStripMenuItem item in NavBar.Items)
            {
                Color customColor = ColorTranslator.FromHtml("#97a1c3");
                this.BackColor = customColor;
                item.BackColor = customColor;
            }
            selected.BackColor = Color.SkyBlue;
        }

        private void MenuSale_Click(object sender, EventArgs e)
        {
            UC_sale uc_sale = new UC_sale();
            AddUserControl(uc_sale);
            SetActiveMenu(MenuSale);
        }

        private void MenuProduct_Click(object sender, EventArgs e)
        {
            UC_product uc_product = new UC_product();
            AddUserControl(uc_product);
            SetActiveMenu(MenuProduct);
        }

        private void MenuCategory_Click(object sender, EventArgs e)
        {
            UC_supplier uc_supplier = new UC_supplier();
            AddUserControl(uc_supplier);
            SetActiveMenu(MenuSupplier);
        }

        private void MenuPurchase_Click(object sender, EventArgs e)
        {
            UC_purchase uc_purchase = new UC_purchase();
            AddUserControl(uc_purchase);
            SetActiveMenu(MenuPurchase);
        }

        private void MenuCompany_Click(object sender, EventArgs e)
        {
            UC_company uc_company = new UC_company();       
            AddUserControl(uc_company);
            SetActiveMenu(MenuCompany);
        }

        private void MenuUser_Click(object sender, EventArgs e)
        {
            UC_user uc_user = new UC_user();
            AddUserControl(uc_user);
            SetActiveMenu(MenuUser);

        }

        private void MenuSetting_Click(object sender, EventArgs e)
        {
            UC_setting uc_setting = new UC_setting();
            AddUserControl(uc_setting);
            SetActiveMenu(MenuSetting);
        }

        private void MenuCategory_Click_1(object sender, EventArgs e)
        {
            UC_category uc_category = new UC_category();
            AddUserControl(uc_category);
            SetActiveMenu(MenuCategory);
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            View.frmLogin myLogin = new View.frmLogin();
            myLogin.Show();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            MainForm uc_mainform = new MainForm();
            label1.Text = Environment.UserName;
            AddUserControl (uc_mainform);
            SetActiveMenu(MenuDashboard);
            NavBar.Cursor = Cursors.Hand;
            

            if (UserDetail.UserType == "Admin")
            {
                MenuSale.Visible = false;
                MenuSetting.Visible = false;
            }
            else if (UserDetail.UserType == "User")
            {
                MenuCategory.Visible = false;
                MenuSetting.Visible = false;
                MenuCompany.Visible = false;
                MenuProduct.Visible = false;
                MenuPurchase.Visible = false;
                MenuUser.Visible = false;
                MenuSupplier.Visible = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void MenuDashboard_Click(object sender, EventArgs e)
        {
            UC_Dashboard uc_user = new UC_Dashboard();
            AddUserControl(uc_user);
            SetActiveMenu(MenuDashboard);
        }
    }
}
