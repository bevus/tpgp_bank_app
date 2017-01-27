using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public class LoginBankerForm
    {
        [Required(ErrorMessage = "Champ obligatoire")]
        [EmailAddress(ErrorMessage = "adresse mail invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        public string Password { get; set; }
    }
}