using System.ComponentModel.DataAnnotations;

namespace EpiScarpe_Co.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Cognome è obbligatorio.")]
        public string Cognome { get; set; }

        public string Indirizzo { get; set; }
        public string Città { get; set; }
        public string Nazione { get; set; }

        [Display(Name = "Data di Nascita")]
        [DataType(DataType.Date)]
        public DateTime? DataNascita { get; set; }

        [Display(Name = "Numero di Cellulare")]
        public string NumeroCellulare { get; set; }

        [Required(ErrorMessage = "Il campo Email è obbligatorio.")]
        [EmailAddress(ErrorMessage = "Il formato dell'indirizzo email non è valido.")]
        public string IndirizzoEmail { get; set; }

        [Required(ErrorMessage = "Il campo Password è obbligatorio.")]
        [DataType(DataType.Password)]
        public string PasswordCliente { get; set; }       
    }
}
