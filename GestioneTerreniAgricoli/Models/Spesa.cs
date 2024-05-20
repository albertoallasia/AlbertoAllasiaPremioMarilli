using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneTerreniAgricoli.Models
{
    public class Spesa
    {
        [Key]
        public int IdSpesa { get; set; }
        public string Descrizione { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Importo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Acquisto")]
        public DateTime DataAquisto { get; set; }

        //Proprietà di Navigazione
        public int LavoroId { get; set; }
        public Lavoro Lavoro { get; set; }
    }
}
