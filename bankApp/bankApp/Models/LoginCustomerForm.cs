using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class LoginCustomerForm
    {
        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "numèro de compte invalide: doit contenir uniquement 4 chiffres")]
        public string CustomerNumber { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "mote de passe invalide: doit contenir 6 chiffres uniquement")]
        public string Password { get; set; }
    }
}