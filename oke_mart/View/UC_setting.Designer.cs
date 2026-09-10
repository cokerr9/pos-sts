namespace oke_mart.View
{
    partial class UC_setting
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textCompanyName = new System.Windows.Forms.TextBox();
            this.pictureLogo = new System.Windows.Forms.PictureBox();
            this.btnAddLogo = new System.Windows.Forms.Button();
            this.LkAddLogo = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.lbeSettingId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(432, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Company Name";
            // 
            // textCompanyName
            // 
            this.textCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textCompanyName.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCompanyName.Location = new System.Drawing.Point(630, 230);
            this.textCompanyName.Name = "textCompanyName";
            this.textCompanyName.Size = new System.Drawing.Size(378, 39);
            this.textCompanyName.TabIndex = 1;
            // 
            // pictureLogo
            // 
            this.pictureLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureLogo.Image = global::oke_mart.Properties.Resources.b799a65b606a58cd4bfaf68893c5dbf5;
            this.pictureLogo.Location = new System.Drawing.Point(687, 314);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.Size = new System.Drawing.Size(245, 227);
            this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureLogo.TabIndex = 4;
            this.pictureLogo.TabStop = false;
            // 
            // btnAddLogo
            // 
            this.btnAddLogo.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddLogo.ForeColor = System.Drawing.Color.Black;
            this.btnAddLogo.Location = new System.Drawing.Point(710, 608);
            this.btnAddLogo.Name = "btnAddLogo";
            this.btnAddLogo.Size = new System.Drawing.Size(206, 58);
            this.btnAddLogo.TabIndex = 5;
            this.btnAddLogo.Text = "Add Logo";
            this.btnAddLogo.UseVisualStyleBackColor = true;
            this.btnAddLogo.Click += new System.EventHandler(this.btnAddLogo_Click);
            // 
            // LkAddLogo
            // 
            this.LkAddLogo.AutoSize = true;
            this.LkAddLogo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LkAddLogo.Location = new System.Drawing.Point(755, 565);
            this.LkAddLogo.Name = "LkAddLogo";
            this.LkAddLogo.Size = new System.Drawing.Size(89, 22);
            this.LkAddLogo.TabIndex = 6;
            this.LkAddLogo.TabStop = true;
            this.LkAddLogo.Text = "Add Logo";
            this.LkAddLogo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LkAddLogo_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(734, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 49);
            this.label3.TabIndex = 7;
            this.label3.Text = "Setting";
            // 
            // lbeSettingId
            // 
            this.lbeSettingId.AutoSize = true;
            this.lbeSettingId.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbeSettingId.ForeColor = System.Drawing.Color.Black;
            this.lbeSettingId.Location = new System.Drawing.Point(432, 182);
            this.lbeSettingId.Name = "lbeSettingId";
            this.lbeSettingId.Size = new System.Drawing.Size(192, 33);
            this.lbeSettingId.TabIndex = 8;
            this.lbeSettingId.Text = "Company Name";
            // 
            // UC_setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbeSettingId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LkAddLogo);
            this.Controls.Add(this.btnAddLogo);
            this.Controls.Add(this.pictureLogo);
            this.Controls.Add(this.textCompanyName);
            this.Controls.Add(this.label1);
            this.Name = "UC_setting";
            this.Size = new System.Drawing.Size(1550, 892);
            this.Load += new System.EventHandler(this.UC_setting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textCompanyName;
        private System.Windows.Forms.PictureBox pictureLogo;
        private System.Windows.Forms.Button btnAddLogo;
        private System.Windows.Forms.LinkLabel LkAddLogo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbeSettingId;
    }
}
