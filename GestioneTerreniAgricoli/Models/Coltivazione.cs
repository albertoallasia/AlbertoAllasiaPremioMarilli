using System.ComponentModel.DataAnnotations;

namespace GestioneTerreniAgricoli.Models
{
    public class Coltivazione
    {
        [Key]
        public int IdColtivazione { get; set; }

        [Display(Name = "Nome Coltivazione")]
        public string NomeColtivazione { get; set; }

        [Display(Name = "Quantità Prodotta")]
        public decimal? QuantitaProdotta { get; set; }

        [Display(Name = "Coltivazione Terminata")]
        public bool ColtivazioneTerminata {  get; set; } 

        //Proprietà di Navigazione
        public int ColturaId { get; set; }
        public Coltura Coltura { get; set; }

        public int TerrenoId { get; set; }
        public Terreno Terreno { get; set; }

        public ICollection<TabellaColtivazioneLavoro> Lavori { get; set; }
    }
}
