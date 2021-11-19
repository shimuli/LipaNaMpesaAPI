using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LipaNaMpesaAPI.Dto
{
    public class CreateSettingsDto
    {
        [Required]public string BusinessName { get; set; }

        [JsonIgnore] public string BusinessId { get; set; }
        [Required] public string ConsumerKey { get; set; }
        [Required] public string ConsumerSecret { get; set; }
        [Required] public string BusinessCode { get; set; }
        [Required] public string PassKey { get; set; }
        [Required] public string TransactionDesc { get; set; }
    }
}
