using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestioneTerreniAgricoli.Models;

namespace GestioneTerreniAgricoli.Data
{
    public class GestioneTerreniAgricoliContext : DbContext
    {
        public GestioneTerreniAgricoliContext (DbContextOptions<GestioneTerreniAgricoliContext> options)
            : base(options)
        {
        }

        public DbSet<GestioneTerreniAgricoli.Models.Localita> Localita { get; set; } = default!;

        public DbSet<GestioneTerreniAgricoli.Models.Coltivazione> Coltivazione { get; set; } = default!;

        public DbSet<GestioneTerreniAgricoli.Models.Coltura> Coltura { get; set; } = default!;

        public DbSet<GestioneTerreniAgricoli.Models.Lavoratore> Lavoratore { get; set; } = default!;

        public DbSet<GestioneTerreniAgricoli.Models.Lavoro> Lavoro { get; set; } = default!;

        public DbSet<GestioneTerreniAgricoli.Models.TabellaLavoroLavoratore> TabellaLavoroLavoratore { get; set; } = default!;

        public DbSet<GestioneTerreniAgricoli.Models.Spesa> Spesa { get; set; } = default!;

        public DbSet<GestioneTerreniAgricoli.Models.TabellaColtivazioneLavoro> TabellaColtivazioneLavoro { get; set; } = default!;

        public DbSet<GestioneTerreniAgricoli.Models.Terreno> Terreno { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Localita>()
                .HasOne(t => t.Terreno)
                .WithOne(l => l.Localita)
                .HasForeignKey<Terreno>(l => l.LocalitaId)
                .OnDelete(DeleteBehavior.Restrict); // Imposta il comportamento di eliminazione a Restrict


            modelBuilder.Entity<Coltivazione>()
                .HasOne(c => c.Coltura)
                .WithMany(cu => cu.Coltivazioni)
                .HasForeignKey(c => c.ColturaId)
                .OnDelete(DeleteBehavior.Restrict); // Imposta il comportamento di eliminazione a Restrict


            modelBuilder.Entity<Coltivazione>()
                .HasOne(c => c.Terreno)
                .WithMany(t => t.Coltivazioni)      // "WithMany" perché è una relazione "one-to-many"
                .HasForeignKey(c => c.TerrenoId)   // Qui si specifica la chiave esterna
                .OnDelete(DeleteBehavior.Restrict); // Imposta il comportamento di eliminazione a Restrict


            modelBuilder.Entity<Spesa>()
                .HasOne(s => s.Lavoro)
                .WithMany(l => l.Spese)
                .HasForeignKey(s => s.LavoroId)
                .OnDelete(DeleteBehavior.Restrict); // Imposta il comportamento di eliminazione a Restrict

            modelBuilder.Entity<TabellaColtivazioneLavoro>()
                .HasOne(tcl => tcl.Lavoro)
                .WithMany(l => l.Coltivazioni)
                .HasForeignKey(tcl => tcl.LavoroId)
                .OnDelete(DeleteBehavior.Restrict); // Assicurati che sia a Restrict

            modelBuilder.Entity<TabellaColtivazioneLavoro>()
                .HasOne(tcl => tcl.Coltivazione)
                .WithMany(c => c.Lavori)
                .HasForeignKey(tcl => tcl.ColtivazioneId)
                .OnDelete(DeleteBehavior.Restrict); // Assicurati che sia a Restrict

            modelBuilder.Entity<TabellaLavoroLavoratore>()
                .HasOne(tll => tll.Lavoratore)
                .WithMany(l => l.Lavori)
                .HasForeignKey(tll => tll.LavoratoreId)
                .OnDelete(DeleteBehavior.Restrict); // Assicurati che sia a Restrict

            modelBuilder.Entity<TabellaLavoroLavoratore>()
                .HasOne(tll => tll.Lavoro)
                .WithMany(l => l.Lavoratori)
                .HasForeignKey(tll => tll.LavoroId)
                .OnDelete(DeleteBehavior.Restrict); // Assicurati che sia a Restrict
        }


    }
}
