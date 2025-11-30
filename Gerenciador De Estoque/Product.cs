using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador_De_Estoque
{
    public class Product
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string UF { get; set; }
        public decimal Value { get; set; }
        public DateTime Validate { get; set; }
        public decimal minStock { get; set; }
        public decimal Amount { get; set; }

    }
}
