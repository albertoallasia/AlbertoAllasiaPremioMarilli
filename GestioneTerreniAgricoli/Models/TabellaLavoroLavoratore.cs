using System.ComponentModel.DataAnnotations;

namespace GestioneTerreniAgricoli.Models
{
    public class TabellaLavoroLavoratore
    {
        [Key]
        public int IdTabellaLavoroLavoratore { get; set; }

        //Proprietà di Navigazione
        public int LavoratoreId { get; set; }
        public Lavoratore Lavoratore { get; set; }

        public int LavoroId { get; set; }
        public Lavoro Lavoro { get; set; }
    }
}
