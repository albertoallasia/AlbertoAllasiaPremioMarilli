using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneTerreniAgricoli.Models
{
    public class Localita
    {
        [Key]
        public int IdLocalita { get; set; }
        public string CAP { get; set; }

        [Display(Name = "Nome Comune")]
        public string NomeComune { get; set; }

        [Display(Name = "Indirizzo")]
        public string Address { get; set; }

        [Display(Name = "Numero Appezzamento")]
        public int NumeroAppezzamento { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        [Display(Name = "Latitudine")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        [Display(Name = "Longitudine")]
        public decimal Longitude { get; set; }

        [NotMapped]
        public string FullAddress
        {
            get { return $"{Address} - Numero Appezzamento: {NumeroAppezzamento}"; }
        }


        //Proprietà di Navigazione
        public Terreno Terreno { get; set; }
    }
}
