﻿// <auto-generated />
using System;
using GestioneTerreniAgricoli.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestioneTerreniAgricoli.Migrations
{
    [DbContext(typeof(GestioneTerreniAgricoliContext))]
    [Migration("20240507161522_rimozioneDeleteOnCascadeLocalità")]
    partial class rimozioneDeleteOnCascadeLocalità
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Coltivazione", b =>
                {
                    b.Property<int>("IdColtivazione")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdColtivazione"));

                    b.Property<bool>("ColtivazioneTerminata")
                        .HasColumnType("bit");

                    b.Property<int>("ColturaId")
                        .HasColumnType("int");

                    b.Property<string>("NomeColtivazione")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("QuantitaProdotta")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TerrenoId")
                        .HasColumnType("int");

                    b.HasKey("IdColtivazione");

                    b.HasIndex("ColturaId");

                    b.HasIndex("TerrenoId");

                    b.ToTable("Coltivazione");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Coltura", b =>
                {
                    b.Property<int>("IdColtura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdColtura"));

                    b.Property<string>("Descrizione")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeColtura")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdColtura");

                    b.ToTable("Coltura");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Lavoratore", b =>
                {
                    b.Property<int>("IdLavoratore")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLavoratore"));

                    b.Property<string>("CodiceFiscale")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataNascita")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ruolo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdLavoratore");

                    b.ToTable("Lavoratore");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Lavoro", b =>
                {
                    b.Property<int>("IdLavoro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLavoro"));

                    b.Property<DateTime?>("DataFineLavoro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInizioLavoro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descrizione")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdLavoro");

                    b.ToTable("Lavoro");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Localita", b =>
                {
                    b.Property<int>("IdLocalita")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLocalita"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CAP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(18, 5)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(18, 5)");

                    b.Property<string>("NomeComune")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumeroAppezzamento")
                        .HasColumnType("int");

                    b.HasKey("IdLocalita");

                    b.ToTable("Localita");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Spesa", b =>
                {
                    b.Property<int>("IdSpesa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSpesa"));

                    b.Property<DateTime>("DataAquisto")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descrizione")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Importo")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("LavoroId")
                        .HasColumnType("int");

                    b.HasKey("IdSpesa");

                    b.HasIndex("LavoroId");

                    b.ToTable("Spesa");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.TabellaColtivazioneLavoro", b =>
                {
                    b.Property<int>("IdTabellaColtivazioneLavoro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTabellaColtivazioneLavoro"));

                    b.Property<int>("ColtivazioneId")
                        .HasColumnType("int");

                    b.Property<int>("LavoroId")
                        .HasColumnType("int");

                    b.HasKey("IdTabellaColtivazioneLavoro");

                    b.HasIndex("ColtivazioneId");

                    b.HasIndex("LavoroId");

                    b.ToTable("TabellaColtivazioneLavoro");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.TabellaLavoroLavoratore", b =>
                {
                    b.Property<int>("IdTabellaLavoroLavoratore")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTabellaLavoroLavoratore"));

                    b.Property<int>("LavoratoreId")
                        .HasColumnType("int");

                    b.Property<int>("LavoroId")
                        .HasColumnType("int");

                    b.HasKey("IdTabellaLavoroLavoratore");

                    b.HasIndex("LavoratoreId");

                    b.HasIndex("LavoroId");

                    b.ToTable("TabellaLavoroLavoratore");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Terreno", b =>
                {
                    b.Property<int>("IdTerreno")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTerreno"));

                    b.Property<int>("LocalitaId")
                        .HasColumnType("int");

                    b.Property<decimal>("Metratura")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("NomeTerreno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipologiaTerreno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdTerreno");

                    b.HasIndex("LocalitaId")
                        .IsUnique();

                    b.ToTable("Terreno");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Coltivazione", b =>
                {
                    b.HasOne("GestioneTerreniAgricoli.Models.Coltura", "Coltura")
                        .WithMany("Coltivazioni")
                        .HasForeignKey("ColturaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestioneTerreniAgricoli.Models.Terreno", "Terreno")
                        .WithMany("Coltivazioni")
                        .HasForeignKey("TerrenoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coltura");

                    b.Navigation("Terreno");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Spesa", b =>
                {
                    b.HasOne("GestioneTerreniAgricoli.Models.Lavoro", "Lavoro")
                        .WithMany("Spese")
                        .HasForeignKey("LavoroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lavoro");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.TabellaColtivazioneLavoro", b =>
                {
                    b.HasOne("GestioneTerreniAgricoli.Models.Coltivazione", "Coltivazione")
                        .WithMany("Lavori")
                        .HasForeignKey("ColtivazioneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestioneTerreniAgricoli.Models.Lavoro", "Lavoro")
                        .WithMany("Coltivazioni")
                        .HasForeignKey("LavoroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coltivazione");

                    b.Navigation("Lavoro");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.TabellaLavoroLavoratore", b =>
                {
                    b.HasOne("GestioneTerreniAgricoli.Models.Lavoratore", "Lavoratore")
                        .WithMany("Lavori")
                        .HasForeignKey("LavoratoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestioneTerreniAgricoli.Models.Lavoro", "Lavoro")
                        .WithMany("Lavoratori")
                        .HasForeignKey("LavoroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lavoratore");

                    b.Navigation("Lavoro");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Terreno", b =>
                {
                    b.HasOne("GestioneTerreniAgricoli.Models.Localita", "Localita")
                        .WithOne("Terreno")
                        .HasForeignKey("GestioneTerreniAgricoli.Models.Terreno", "LocalitaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Localita");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Coltivazione", b =>
                {
                    b.Navigation("Lavori");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Coltura", b =>
                {
                    b.Navigation("Coltivazioni");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Lavoratore", b =>
                {
                    b.Navigation("Lavori");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Lavoro", b =>
                {
                    b.Navigation("Coltivazioni");

                    b.Navigation("Lavoratori");

                    b.Navigation("Spese");
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Localita", b =>
                {
                    b.Navigation("Terreno")
                        .IsRequired();
                });

            modelBuilder.Entity("GestioneTerreniAgricoli.Models.Terreno", b =>
                {
                    b.Navigation("Coltivazioni");
                });
#pragma warning restore 612, 618
        }
    }
}
