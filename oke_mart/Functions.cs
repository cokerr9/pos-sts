using System;
using System.Windows.Forms;

namespace oke_mart
{
    internal class Functions
    {
        // Enable Text and ComboBox
        public void EnableAllTextAndCombobox(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox || control is ComboBox)
                {
                    control.Enabled = true;
                }
                if (control.HasChildren)
                {
                    EnableAllTextAndCombobox(control);
                }
            }
        }

        // Disable Text and ComboBox
        public void DisableAllTextAndCombobox(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox || control is ComboBox)
                {
                    control.Enabled = false;
                }
                if (control.HasChildren)
                {
                    DisableAllTextAndCombobox(control);
                }
            }
        }

        // Clear All Data
        public void ClearField(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox txtBox)
                {
                    txtBox.Clear();
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = -1;
                }
                if (control.HasChildren)
                {
                    ClearField(control);
                }
            }
        }
    }
}