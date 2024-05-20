using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneTerreniAgricoli.Models
{
    public class Terreno
    {
        [Key]
        public int IdTerreno { get; set; }
        public string NomeTerreno { get; set; }
        public string TipologiaTerreno { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        
        public decimal Metratura { get; set; }

        //Proprietà di Navigazione
        [Display(Name = "Località")]
        public int LocalitaId { get; set; }
        [Display(Name = "Località")]
        public Localita Localita { get; set; }

        //un terreno può avere più coltivazioni
        public ICollection<Coltivazione> Coltivazioni { get; set; }
    }
}
