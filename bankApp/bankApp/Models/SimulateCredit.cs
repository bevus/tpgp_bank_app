using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class SimulateCredit
    {
        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"\d+", ErrorMessage = "Montant invalide")]
        public int RequestedAmount { get; set; }

        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"\d+", ErrorMessage = "Montant invalide")]
        public int Contribution { get; set; }

        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"\d+", ErrorMessage = "Montant invalide")]
        public int HouseholdIncomes { get; set; }

        [Required(ErrorMessage = "champ obligatoire")]
        [RegularExpression(@"\d+", ErrorMessage = "Specified the number of years")]
        public int Duration { get; set; }
    }
}