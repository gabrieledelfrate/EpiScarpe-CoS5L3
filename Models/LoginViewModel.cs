using System.ComponentModel.DataAnnotations;

namespace EpiScarpe_Co.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Il campo Email è obbligatorio.")]
        [EmailAddress(ErrorMessage = "Il formato dell'indirizzo email non è valido.")]
        public string IndirizzoEmail { get; set; }

        [Required(ErrorMessage = "Il campo Password è obbligatorio.")]
        [DataType(DataType.Password)]
        public string PasswordCliente { get; set; }
    }
}
