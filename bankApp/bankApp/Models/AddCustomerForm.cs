using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankApp.Models
{
    public class AddCustomerForm
    {
        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\w+$", ErrorMessage = "Seule les lettres alphabetiques sont autorisées")]
        [MaxLength(50, ErrorMessage = "Trop long, pas plus de 50 caractères")]
        [MinLength(2, ErrorMessage = "Trop court, au moins 2 caractères")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\w+$", ErrorMessage = "Seule les lettres alphabetiques sont autorisées")]
        [MaxLength(50, ErrorMessage = "Trop long, pas plus de 50 caractères")]
        [MinLength(2, ErrorMessage = "Trop court, au moins 2 caractères")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "numèro de compte invalide: doit contenir 4 chiffres uniquement")]
        public string CustomerNumber { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "mot de passe invalide: doit contenir 6 chiffres uniquement")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [MaxLength(8, ErrorMessage = "BIC invalide")]
        public string BIC { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}", ErrorMessage = "IBAN Invalide")]
        public string IBAN { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solde invalide")]
        public int Solde { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        public int BankerID { get; set; }
        public IEnumerable<SelectListItem> Bankers { get; set; }
    } 
}