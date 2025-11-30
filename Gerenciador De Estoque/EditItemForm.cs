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
    public partial class EditItemForm : Form
    {
        ManageItems manageItems = new ManageItems();

        Product product = new Product();
        public EditItemForm()
        {

            CultureInfo culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            InitializeComponent();
        }

        private void EditItemForm_Load(object sender, EventArgs e)
        {
            
        }

        public void LoadProductData(Product prod)
        {
            product = prod;
            idTextBox.Text = product.Barcode;
            nameTextBox.Text = product.Name;
            ufTextBox.Text = product.UF;
            priceNumericUpDown.Value = Convert.ToDecimal(product.Value);
            dateTimePicker.Value = product.Validate;
            minStockNumericUpDown.Value = Convert.ToDecimal(product.minStock);
            amountNumericUpDown.Value = Convert.ToDecimal(product.Amount);
        }


        public async void UpdateProductData()
        {
            product.Barcode = idTextBox.Text;
            product.Name = nameTextBox.Text;
            product.UF = ufTextBox.Text;
            product.Value = priceNumericUpDown.Value;
            product.Validate = dateTimePicker.Value;
            product.minStock = minStockNumericUpDown.Value;
            product.Amount = amountNumericUpDown.Value;
            try
            {
                await manageItems.UpdateItem(product);
                if(manageItems.UpdateItem(product).Result)
                {
                    MessageBox.Show("Produto atualizado com sucesso!");
                    EstoqueForm.instance.LoadProducts();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Falha ao atualizar o produto.");
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar o produto: {ex.Message}");
            }
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            UpdateProductData();
        }
    }
}
