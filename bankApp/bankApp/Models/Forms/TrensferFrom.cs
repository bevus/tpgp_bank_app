using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BankApp.Models
{
    public class TrensferFrom
    {
        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "Montant invalide")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"[a-zA-Z0-9 ]+", ErrorMessage = ("intitulé invalide : les caractères specieaux ne sont pas autorisés"))]
        [MaxLength(50, ErrorMessage = "trop long pas plus de 50 caractères")]
        public string Label { get; set; }
        
        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}", ErrorMessage = "IBAN Invalide")]
        public string IBAN { get; set; }

        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"(\w+ \w+)+", ErrorMessage = ("nom benificiare invalide"))]
        [MaxLength(50, ErrorMessage = "trop long pas plus de 50 caractères")]
        public string DestinationFullName { get; set; }

        [Required(ErrorMessage = "champ obligatoire")]
        public int SourceAccount { get; set; }

        public IEnumerable<SelectListItem> AccountsId { get; set; }
    }
}