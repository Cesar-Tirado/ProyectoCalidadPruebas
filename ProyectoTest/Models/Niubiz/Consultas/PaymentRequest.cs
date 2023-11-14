using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTest.Models.Niubiz.Consultas
{

    public class Antifraud
    {
        public string clientIp { get; set; }
        public MerchantDefineData merchantDefineData { get; set; }
    }

    public class MerchantDefineData
    {
        public string MDD4 { get; set; }
        public string MDD21 { get; set; }
        public string MDD32 { get; set; }
        public string MDD75 { get; set; }
        public string MDD77 { get; set; }
    }

    public class PaymentRequest
    {
        public decimal amount { get; set; }
        public Antifraud antifraud { get; set; }
        public string channel { get; set; }
        public string recurrenceMaxAmount { get; set; } = null;
    }
    public class PaymentResponse
    {
        
public string sessionKey { get; set; }
        public string expirationTime{ get; set; }
    }
}
