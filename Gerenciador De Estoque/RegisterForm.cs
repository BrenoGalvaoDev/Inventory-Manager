using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    public partial class RegisterForm : Form
    {
        public static RegisterForm instance;

        public bool isNewProduct = true;

        RegisterNewProduct registerNewProduct = new RegisterNewProduct();

        public RegisterForm()
        {
            InitializeComponent();
            instance = this;
        }

        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            string id = idTextBox.Text;
            registerNewProduct.ChangeID(id, nameTextBox, priceNumericUpDown, minStockNumericUpDown, dateTimePicker);
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            registerNewProduct.ChangeName(name);
        }

        private void priceNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            decimal value = priceNumericUpDown.Value;
            registerNewProduct.ChangeValue(value);
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = dateTimePicker.Value.Date;
            registerNewProduct.ChangeValidate(dt);
        }
        private void minStockNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal value = minStockNumericUpDown.Value;
            registerNewProduct.ChangeStockMin(value);
        }

        private void amountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal value = amountNumericUpDown.Value;
            registerNewProduct.ChangeAmount(value);
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            decimal amount = amountNumericUpDown.Value;
            if (isNewProduct) registerNewProduct.AddNewProduct();
            else registerNewProduct.UpdateProduct(idTextBox.Text, amount);
            isNewProduct = true;

            idTextBox.Clear();
            nameTextBox.Clear();
            priceNumericUpDown.Value = 0;
            dateTimePicker.Value = DateTime.Now;
            amountNumericUpDown.Value = 0;
            minStockNumericUpDown.Value = 0;
            idTextBox.Focus();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            new Main().Show();
            this.Hide();
        }

        private void priceNumericUpDown_Enter(object sender, EventArgs e)
        {
            priceNumericUpDown.Text = "";
        }

        private void minStockNumericUpDown_Enter(object sender, EventArgs e)
        {
            minStockNumericUpDown.Text = "";
        }

        private void amountNumericUpDown_Enter(object sender, EventArgs e)
        {
            amountNumericUpDown.Text = "";
        }
    }
}
