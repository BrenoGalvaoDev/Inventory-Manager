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
    /// <summary>
    /// Form used for registering new products or updating stock/details of existing ones.
    /// </summary>
    public partial class RegisterForm : Form
    {
        /// <summary>
        /// Static instance of the form for easy access from other parts of the application.
        /// </summary>
        public static RegisterForm instance;

        /// <summary>
        /// Flag indicating whether the current operation is to register a new product (true) or update an existing one (false).
        /// </summary>
        public bool isNewProduct = true;

        /// <summary>
        /// Instance of the RegisterNewProduct class which handles the business logic and data persistence.
        /// </summary>
        RegisterNewProduct registerNewProduct = new RegisterNewProduct();

        /// <summary>
        /// Constructor for the RegisterForm.
        /// </summary>
        public RegisterForm()
        {
            InitializeComponent();
            instance = this;
        }

        /// <summary>
        /// Event handler for changes in the Barcode/ID text box.
        /// It calls the logic class to handle ID changes, which may involve checking for existing products.
        /// </summary>
        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            string id = idTextBox.Text;
            // Pass related controls to the logic class to potentially populate existing data
            registerNewProduct.ChangeID(id, nameTextBox, priceNumericUpDown, minStockNumericUpDown, dateTimePicker);
        }

        /// <summary>
        /// Event handler for changes in the Product Name text box.
        /// Updates the Name property in the logic class.
        /// </summary>
        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            registerNewProduct.ChangeName(name);
        }

        /// <summary>
        /// Event handler for changes in the Price Numeric Up/Down control.
        /// Updates the Value property in the logic class.
        /// </summary>
        private void priceNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            decimal value = priceNumericUpDown.Value;
            registerNewProduct.ChangeValue(value);
        }

        /// <summary>
        /// Event handler for changes in the Expiration Date Time Picker.
        /// Updates the Validate property in the logic class.
        /// </summary>
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = dateTimePicker.Value.Date;
            registerNewProduct.ChangeValidate(dt);
        }

        /// <summary>
        /// Event handler for changes in the Minimum Stock Numeric Up/Down control.
        /// Updates the minStock property in the logic class.
        /// </summary>
        private void minStockNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal value = minStockNumericUpDown.Value;
            registerNewProduct.ChangeStockMin(value);
        }

        /// <summary>
        /// Event handler for changes in the Amount Numeric Up/Down control (current stock quantity).
        /// Updates the Amount property in the logic class.
        /// </summary>
        private void amountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal value = amountNumericUpDown.Value;
            registerNewProduct.ChangeAmount(value);
        }

        /// <summary>
        /// Event handler for changes in the Unit of Measure (UF) text box.
        /// Updates the UF property in the logic class.
        /// </summary>
        private void ufTextBox_TextChanged(object sender, EventArgs e)
        {
            string value = ufTextBox.Text;
            registerNewProduct.ChangeUF(value);
        }

        /// <summary>
        /// Event handler for the Confirm button click.
        /// Executes either the new product registration or the product update based on the isNewProduct flag.
        /// Clears all input fields after the operation.
        /// </summary>
        private void confirmBtn_Click(object sender, EventArgs e)
        {
            decimal amount = amountNumericUpDown.Value;

            // Call the appropriate method based on the current operation mode
            if (isNewProduct)
            {
                registerNewProduct.AddNewProduct();
            }
            else
            {
                registerNewProduct.UpdateProduct(idTextBox.Text, amount);
            }

            // Reset the flag and clear the form fields
            isNewProduct = true;

            idTextBox.Clear();
            nameTextBox.Clear();
            ufTextBox.Clear();
            priceNumericUpDown.Value = 0;
            dateTimePicker.Value = DateTime.Now;
            amountNumericUpDown.Value = 0;
            minStockNumericUpDown.Value = 0;
            idTextBox.Focus(); // Set focus back to the ID field
        }

        /// <summary>
        /// Event handler for the Back button click.
        /// Opens the Main form and hides the current form.
        /// </summary>
        private void backBtn_Click(object sender, EventArgs e)
        {
            new Main().Show();
            this.Hide();
        }

        /// <summary>
        /// Event handler for the Price Numeric Up/Down control gaining focus.
        /// Clears the text for easier input (common pattern for NumericUpDown).
        /// </summary>
        private void priceNumericUpDown_Enter(object sender, EventArgs e)
        {
            priceNumericUpDown.Text = "";
        }

        /// <summary>
        /// Event handler for the Minimum Stock Numeric Up/Down control gaining focus.
        /// Clears the text for easier input.
        /// </summary>
        private void minStockNumericUpDown_Enter(object sender, EventArgs e)
        {
            minStockNumericUpDown.Text = "";
        }

        /// <summary>
        /// Event handler for the Amount Numeric Up/Down control gaining focus.
        /// Clears the text for easier input.
        /// </summary>
        private void amountNumericUpDown_Enter(object sender, EventArgs e)
        {
            amountNumericUpDown.Text = "";
        }
    }
}