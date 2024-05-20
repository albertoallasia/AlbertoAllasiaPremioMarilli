using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneTerreniAgricoli.Models
{
    public class Coltura
    {
        [Key]
        public int IdColtura { get; set; }

        [Display(Name = "Nome Coltura")]
        public string NomeColtura { get; set; }
        public string Descrizione { get; set; }

        //Proprietà di Navigazione

        [NotMapped]
        public ICollection<Coltivazione> Coltivazioni { get; set; }
    }
}
