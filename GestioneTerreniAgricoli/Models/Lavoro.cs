using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneTerreniAgricoli.Models
{
    // Implementa la proprietà SelectedLavoratori e SelectedColtivazioni nel model Lavoro
    public class Lavoro
    {
        [Key]
        public int IdLavoro { get; set; }
        public string Descrizione { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Inizio Lavoro")]
        public DateTime DataInizioLavoro { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Fine Lavoro")]
        public DateTime? DataFineLavoro { get; set; }

        // Proprietà di navigazione
        public ICollection<TabellaLavoroLavoratore> Lavoratori { get; set; }

        public ICollection<TabellaColtivazioneLavoro> Coltivazioni { get; set; }
        public ICollection<Spesa> Spese { get; set; }
        
    }
}
