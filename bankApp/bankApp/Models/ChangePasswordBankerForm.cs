using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class ChangePasswordBankerForm
    {
        [Required(ErrorMessage = "Champ obligatoire")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [Compare("NewPassword", ErrorMessage = "Les deux mots de passe ne correspondent pas")]
        public string ConfirmNewPassword { get; set; }
    }
}