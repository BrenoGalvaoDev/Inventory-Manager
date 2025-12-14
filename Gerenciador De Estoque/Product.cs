using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador_De_Estoque
{
    /// <summary>
    /// Represents a product or item managed in the inventory system.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the unique identification code of the product (Barcode).
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Unit of Measure (e.g., "UN" for Unit, "KG" for Kilogram).
        /// </summary>
        public string UF { get; set; }

        /// <summary>
        /// Gets or sets the unit price or value of the product.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the product.
        /// </summary>
        public DateTime Validate { get; set; }

        /// <summary>
        /// Gets or sets the minimum required stock level for the product (critical stock threshold).
        /// </summary>
        public decimal minStock { get; set; }

        /// <summary>
        /// Gets or sets the current quantity/amount of the product in stock.
        /// </summary>
        public decimal Amount { get; set; }

    }
}