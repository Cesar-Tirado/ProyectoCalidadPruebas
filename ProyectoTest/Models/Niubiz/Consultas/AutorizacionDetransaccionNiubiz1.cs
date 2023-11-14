using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTest.Models.Niubiz.Consultas
{

    public class CardHolder
    {
        public string documentNumber { get; set; } = null;
        public string documentType { get; set; } = null;
    }

    public class Order
    {
        public decimal amount { get; set; }
        public string currency { get; set; }
        //public string productId { get; set; } = null;
        public string purchaseNumber { get; set; }
        public string tokenId { get; set; }
        //public string originalAmount { get; set; }
    }

    public class Recurrence
    {
        public string amount { get; set; } = null;
        public string beneficiaryId { get; set; } = null;
        public string frequency { get; set; } = null;
        public string maxAmount { get; set; } = null;
        public string type { get; set; } = null;
    }

    public class AutorizacionDetransaccionNiubiz1
    {
        public object antifraud { get; set; } = null;
        public string captureType { get; set; }
        //public CardHolder cardHolder { get; set; } = null;
        public string channel { get; set; }
        public bool countable { get; set; }
        public List<Order> order { get; set; }
        public Recurrence recurrence { get; set; } = null;
        public object sponsored { get; set; } = null;
    }
}
