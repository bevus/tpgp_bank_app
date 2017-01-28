using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class ChangePasswordCustomerForm
    {
        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "mot de passe invalide: doit contenir 6 chiffres uniquement")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "mot de passe invalide: doit contenir 6 chiffres uniquement")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "mot de passe invalide: doit contenir 6 chiffres uniquement")]
        [Compare("NewPassword", ErrorMessage = "Les deux mots de passe ne correspondent pas")]
        public string ConfirmNewPassword { get; set; }
    }
}