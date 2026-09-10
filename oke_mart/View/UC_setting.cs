using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace oke_mart.View
{
    public partial class UC_setting : UserControl
    {
        public UC_setting()
        {
            InitializeComponent();
        }

        Controller.ControllerSetting mySetting = new Controller.ControllerSetting();

        private void LkAddLogo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Choose Your Logo (*jpg;*png;*gif;)|*jpg;*png;*gif;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureLogo.Image = System.Drawing.Image.FromFile(ofd.FileName);

            }
        }

        private void btnAddLogo_Click(object sender, EventArgs e)
        {
            if (textCompanyName.Text == "")
            {
                MessageBox.Show("Pls Type Your Company Name", "Valid Compnay Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (btnAddLogo.Text == "Update Logo")
            {
                System.Drawing.Image img = pictureLogo.Image;
                byte[] arr;
                ImageConverter converter = new ImageConverter();
                arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                mySetting.CompanyName = textCompanyName.Text;
                mySetting.CompanyLogo = arr;
                mySetting.UpdateSetting();
                MessageBox.Show("Success For Update Logo Company", "Updated Company", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                System.Drawing.Image img = pictureLogo.Image;
                byte[] arr;
                ImageConverter converter = new ImageConverter();
                arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                mySetting.CompanyName = textCompanyName.Text;
                mySetting.CompanyLogo = arr;
                mySetting.InsertSetting();
                MessageBox.Show("Success For Add Logo Company", "Added Company", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        public void LoadImage()
        {
            try
            {
                SqlCommand cmd = new SqlCommand(
                "SELECT * FROM tblSetting WHERE SettingId = @SettingId", mySetting.conn
            );
                cmd.Parameters.AddWithValue("@SettingId", 1);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lbeSettingId.Text = dr["SettingId"].ToString();
                    textCompanyName.Text = dr["CompanyName"].ToString();

                    if (dr["CompanyLogo"] != DBNull.Value)
                    {
                        byte[] img = (byte[])dr["CompanyLogo"];
                        using (MemoryStream ms = new MemoryStream(img))
                        {
                            pictureLogo.Image = Image.FromStream(ms);
                        }
                        pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        pictureLogo.Image = null;
                    }
                }

                dr.Close();
                btnAddLogo.Text = "Update Logo";
            }
            catch { 
            
            }
        }

        private void UC_setting_Load(object sender, EventArgs e)
        {
            LoadImage();
            lbeSettingId.Visible = false;
        }
    }
}
