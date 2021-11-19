using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LipaNaMpesaAPI.Dto
{
    public class GetConfigDto
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string BusinessId { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string BusinessCode { get; set; }
        public string PassKey { get; set; }
        public string TransactionDesc { get; set; }
    }
}
