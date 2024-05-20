using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace GestioneTerreniAgricoli.Models
{
    public class Lavoratore
    {
        [Key]
        public int IdLavoratore { get; set; }

        [Display(Name = "Codice Fiscale")]
        public string CodiceFiscale { get; set; }
        public string Ruolo { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data di Nascita")]
        public DateTime DataNascita { get; set; }

        [NotMapped]
        [Display(Name = "Nome e Cognome")]
        public string NomeCognome
        {
            get { return $"{Nome} {Cognome}"; }
        }

        //Proprietà di Navigazione
        public ICollection<TabellaLavoroLavoratore> Lavori { get; set; }
    }
}
