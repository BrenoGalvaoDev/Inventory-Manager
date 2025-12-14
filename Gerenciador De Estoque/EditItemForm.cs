using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    /// <summary>
    /// Form dedicated to editing the details of an existing product in the inventory.
    /// It allows modification of name, price, stock levels, and expiration date.
    /// </summary>
    public partial class EditItemForm : Form
    {
        /// <summary>
        /// Instance of the ManageItems class to handle asynchronous database update operations.
        /// </summary>
        ManageItems manageItems = new ManageItems();

        /// <summary>
        /// Local Product object to hold the data of the item being edited.
        /// </summary>
        Product product = new Product();

        /// <summary>
        /// Constructor for the EditItemForm.
        /// </summary>
        public EditItemForm()
        {
            // Set the thread culture to Brazilian Portuguese for consistent UI formatting (dates, currency).
            CultureInfo culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            InitializeComponent();
        }

        private void EditItemForm_Load(object sender, EventArgs e)
        {
            // Form Load event handler (Currently empty)
        }

        /// <summary>
        /// Loads the data from a selected Product object into the form controls (textboxes, date pickers, numeric up/down).
        /// </summary>
        /// <param name="prod">The Product object containing the item's current details.</param>
        public void LoadProductData(Product prod)
        {
            // Store the received product object locally
            product = prod;

            // Populate controls with current product data
            idTextBox.Text = product.Barcode;
            nameTextBox.Text = product.Name;
            ufTextBox.Text = product.UF;
            priceNumericUpDown.Value = Convert.ToDecimal(product.Value);
            dateTimePicker.Value = product.Validate;
            minStockNumericUpDown.Value = Convert.ToDecimal(product.minStock);
            amountNumericUpDown.Value = Convert.ToDecimal(product.Amount);
        }

        /// <summary>
        /// Collects the potentially modified data from the form controls, updates the local Product object,
        /// and calls the asynchronous method to persist the changes to the database.
        /// </summary>
        public async void UpdateProductData()
        {
            // Update the local Product object with data from the controls
            product.Barcode = idTextBox.Text;
            product.Name = nameTextBox.Text;
            product.UF = ufTextBox.Text;
            product.Value = priceNumericUpDown.Value;
            product.Validate = dateTimePicker.Value;
            product.minStock = minStockNumericUpDown.Value;
            product.Amount = amountNumericUpDown.Value;

            try
            {
                // Call the asynchronous database update method
                await manageItems.UpdateItem(product);

                // Check the result of the update operation
                if (manageItems.UpdateItem(product).Result)
                {
                    MessageBox.Show("Produto atualizado com sucesso!");
                    // Reload the product list in the main inventory form to reflect changes immediately
                    EstoqueForm.instance.LoadProducts();
                    this.Close(); // Close the edit form upon success
                }
                else
                {
                    MessageBox.Show("Falha ao atualizar o produto.");
                }
            }
            catch (Exception ex)
            {
                // Catch and display any exception that occurred during the update process
                MessageBox.Show($"Erro ao atualizar o produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler for the Confirm button click.
        /// Initiates the data collection and update process.
        /// </summary>
        private void confirmBtn_Click(object sender, EventArgs e)
        {
            UpdateProductData();
        }
    }
}