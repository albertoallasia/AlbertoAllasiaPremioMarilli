using System.ComponentModel.DataAnnotations;

namespace GestioneTerreniAgricoli.Models
{
    public class TabellaColtivazioneLavoro
    {
        [Key]
        public int IdTabellaColtivazioneLavoro { get; set; }

        //Proprietà di Navigazione
        public int LavoroId { get; set; }
        public Lavoro Lavoro { get; set; }

        public int ColtivazioneId { get; set; }
        public Coltivazione Coltivazione { get; set; }
    }
}
