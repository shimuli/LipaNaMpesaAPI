using System;
using System.Collections.Generic;

#nullable disable

namespace LipaNaMpesaAPI.Models
{
    public partial class Setting
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string BusinessCode { get; set; }
        public string PassKey { get; set; }
        public string TransactionDesc { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
