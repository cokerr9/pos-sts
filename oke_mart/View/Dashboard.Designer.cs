namespace oke_mart.View
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.NavBar = new System.Windows.Forms.MenuStrip();
            this.MenuDashboard = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSale = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuProduct = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSupplier = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuPurchase = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCompany = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUser = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.pnShow = new System.Windows.Forms.Panel();
            this.pnImage = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.NavBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.NavBar);
            this.panel1.Location = new System.Drawing.Point(2, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(249, 698);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(55, 639);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "\r\n";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // NavBar
            // 
            this.NavBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.NavBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.NavBar.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.NavBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuDashboard,
            this.MenuSale,
            this.MenuProduct,
            this.MenuCategory,
            this.MenuSupplier,
            this.MenuPurchase,
            this.MenuCompany,
            this.MenuUser,
            this.MenuSetting,
            this.MenuExit});
            this.NavBar.Location = new System.Drawing.Point(0, 0);
            this.NavBar.Name = "NavBar";
            this.NavBar.Size = new System.Drawing.Size(248, 696);
            this.NavBar.TabIndex = 0;
            this.NavBar.Text = "menu";
            // 
            // MenuDashboard
            // 
            this.MenuDashboard.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuDashboard.ForeColor = System.Drawing.Color.White;
            this.MenuDashboard.Name = "MenuDashboard";
            this.MenuDashboard.Size = new System.Drawing.Size(235, 57);
            this.MenuDashboard.Text = "Dashboard";
            this.MenuDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuDashboard.Click += new System.EventHandler(this.MenuDashboard_Click);
            // 
            // MenuSale
            // 
            this.MenuSale.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuSale.ForeColor = System.Drawing.Color.White;
            this.MenuSale.Name = "MenuSale";
            this.MenuSale.Size = new System.Drawing.Size(235, 57);
            this.MenuSale.Text = "Sale";
            this.MenuSale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuSale.Click += new System.EventHandler(this.MenuSale_Click);
            // 
            // MenuProduct
            // 
            this.MenuProduct.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuProduct.ForeColor = System.Drawing.Color.White;
            this.MenuProduct.Name = "MenuProduct";
            this.MenuProduct.Size = new System.Drawing.Size(235, 57);
            this.MenuProduct.Text = "Product";
            this.MenuProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuProduct.Click += new System.EventHandler(this.MenuProduct_Click);
            // 
            // MenuCategory
            // 
            this.MenuCategory.ForeColor = System.Drawing.Color.White;
            this.MenuCategory.Name = "MenuCategory";
            this.MenuCategory.Size = new System.Drawing.Size(235, 57);
            this.MenuCategory.Text = "Category";
            this.MenuCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuCategory.Click += new System.EventHandler(this.MenuCategory_Click_1);
            // 
            // MenuSupplier
            // 
            this.MenuSupplier.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuSupplier.ForeColor = System.Drawing.Color.White;
            this.MenuSupplier.Name = "MenuSupplier";
            this.MenuSupplier.Size = new System.Drawing.Size(235, 57);
            this.MenuSupplier.Text = "Supplier";
            this.MenuSupplier.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuSupplier.Click += new System.EventHandler(this.MenuCategory_Click);
            // 
            // MenuPurchase
            // 
            this.MenuPurchase.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuPurchase.ForeColor = System.Drawing.Color.White;
            this.MenuPurchase.Name = "MenuPurchase";
            this.MenuPurchase.Size = new System.Drawing.Size(235, 57);
            this.MenuPurchase.Text = "Purchase";
            this.MenuPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuPurchase.Click += new System.EventHandler(this.MenuPurchase_Click);
            // 
            // MenuCompany
            // 
            this.MenuCompany.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuCompany.ForeColor = System.Drawing.Color.White;
            this.MenuCompany.Name = "MenuCompany";
            this.MenuCompany.Size = new System.Drawing.Size(235, 57);
            this.MenuCompany.Text = "Company";
            this.MenuCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuCompany.Click += new System.EventHandler(this.MenuCompany_Click);
            // 
            // MenuUser
            // 
            this.MenuUser.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuUser.ForeColor = System.Drawing.Color.White;
            this.MenuUser.Name = "MenuUser";
            this.MenuUser.Size = new System.Drawing.Size(235, 57);
            this.MenuUser.Text = "User";
            this.MenuUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuUser.Click += new System.EventHandler(this.MenuUser_Click);
            // 
            // MenuSetting
            // 
            this.MenuSetting.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuSetting.ForeColor = System.Drawing.Color.White;
            this.MenuSetting.Name = "MenuSetting";
            this.MenuSetting.Size = new System.Drawing.Size(235, 57);
            this.MenuSetting.Text = "Setting";
            this.MenuSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuSetting.Click += new System.EventHandler(this.MenuSetting_Click);
            // 
            // MenuExit
            // 
            this.MenuExit.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuExit.ForeColor = System.Drawing.Color.White;
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(235, 57);
            this.MenuExit.Text = "Exit";
            this.MenuExit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // pnShow
            // 
            this.pnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnShow.BackColor = System.Drawing.Color.White;
            this.pnShow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnShow.ForeColor = System.Drawing.Color.White;
            this.pnShow.Location = new System.Drawing.Point(268, 6);
            this.pnShow.Name = "pnShow";
            this.pnShow.Size = new System.Drawing.Size(990, 784);
            this.pnShow.TabIndex = 2;
            // 
            // pnImage
            // 
            this.pnImage.BackColor = System.Drawing.Color.Gray;
            this.pnImage.Location = new System.Drawing.Point(2, 6);
            this.pnImage.Name = "pnImage";
            this.pnImage.Size = new System.Drawing.Size(249, 84);
            this.pnImage.TabIndex = 0;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 802);
            this.Controls.Add(this.pnImage);
            this.Controls.Add(this.pnShow);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.NavBar;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dashboard_FormClosing);
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.NavBar.ResumeLayout(false);
            this.NavBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnShow;
        private System.Windows.Forms.MenuStrip NavBar;
        private System.Windows.Forms.ToolStripMenuItem MenuDashboard;
        private System.Windows.Forms.ToolStripMenuItem MenuSale;
        private System.Windows.Forms.ToolStripMenuItem MenuProduct;
        private System.Windows.Forms.ToolStripMenuItem MenuSupplier;
        private System.Windows.Forms.ToolStripMenuItem MenuPurchase;
        private System.Windows.Forms.ToolStripMenuItem MenuCompany;
        private System.Windows.Forms.ToolStripMenuItem MenuUser;
        private System.Windows.Forms.ToolStripMenuItem MenuSetting;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.Panel pnImage;
        private System.Windows.Forms.ToolStripMenuItem MenuCategory;
        private System.Windows.Forms.Label label1;
    }
}