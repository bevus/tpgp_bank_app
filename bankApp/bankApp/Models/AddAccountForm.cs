using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class AddAccountForm
    {
        [Required(ErrorMessage = "champ obligatoire")]
        public string BIC { get; set; }
        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}", ErrorMessage = "IBAN Invalide")]
        public string IBAN { get; set; }
    }
}