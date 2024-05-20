using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneTerreniAgricoli.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coltura",
                columns: table => new
                {
                    IdColtura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeColtura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coltura", x => x.IdColtura);
                });

            migrationBuilder.CreateTable(
                name: "Lavoratore",
                columns: table => new
                {
                    IdLavoratore = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodiceFiscale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ruolo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascita = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lavoratore", x => x.IdLavoratore);
                });

            migrationBuilder.CreateTable(
                name: "Lavoro",
                columns: table => new
                {
                    IdLavoro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInizioLavoro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFineLavoro = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lavoro", x => x.IdLavoro);
                });

            migrationBuilder.CreateTable(
                name: "Localita",
                columns: table => new
                {
                    IdLocalita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CAP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeComune = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroAppezzamento = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localita", x => x.IdLocalita);
                });

            migrationBuilder.CreateTable(
                name: "Spesa",
                columns: table => new
                {
                    IdSpesa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Importo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataAquisto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LavoroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spesa", x => x.IdSpesa);
                    table.ForeignKey(
                        name: "FK_Spesa_Lavoro_LavoroId",
                        column: x => x.LavoroId,
                        principalTable: "Lavoro",
                        principalColumn: "IdLavoro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabellaLavoroLavoratore",
                columns: table => new
                {
                    IdTabellaLavoroLavoratore = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LavoratoreId = table.Column<int>(type: "int", nullable: false),
                    LavoroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabellaLavoroLavoratore", x => x.IdTabellaLavoroLavoratore);
                    table.ForeignKey(
                        name: "FK_TabellaLavoroLavoratore_Lavoratore_LavoratoreId",
                        column: x => x.LavoratoreId,
                        principalTable: "Lavoratore",
                        principalColumn: "IdLavoratore",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TabellaLavoroLavoratore_Lavoro_LavoroId",
                        column: x => x.LavoroId,
                        principalTable: "Lavoro",
                        principalColumn: "IdLavoro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Terreno",
                columns: table => new
                {
                    IdTerreno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTerreno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipologiaTerreno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Metratura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LocalitaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terreno", x => x.IdTerreno);
                    table.ForeignKey(
                        name: "FK_Terreno_Localita_LocalitaId",
                        column: x => x.LocalitaId,
                        principalTable: "Localita",
                        principalColumn: "IdLocalita",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coltivazione",
                columns: table => new
                {
                    IdColtivazione = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeColtivazione = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantitaProdotta = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ColtivazioneTerminata = table.Column<bool>(type: "bit", nullable: false),
                    ColturaId = table.Column<int>(type: "int", nullable: false),
                    TerrenoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coltivazione", x => x.IdColtivazione);
                    table.ForeignKey(
                        name: "FK_Coltivazione_Coltura_ColturaId",
                        column: x => x.ColturaId,
                        principalTable: "Coltura",
                        principalColumn: "IdColtura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Coltivazione_Terreno_TerrenoId",
                        column: x => x.TerrenoId,
                        principalTable: "Terreno",
                        principalColumn: "IdTerreno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabellaColtivazioneLavoro",
                columns: table => new
                {
                    IdTabellaColtivazioneLavoro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LavoroId = table.Column<int>(type: "int", nullable: false),
                    ColtivazioneId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabellaColtivazioneLavoro", x => x.IdTabellaColtivazioneLavoro);
                    table.ForeignKey(
                        name: "FK_TabellaColtivazioneLavoro_Coltivazione_ColtivazioneId",
                        column: x => x.ColtivazioneId,
                        principalTable: "Coltivazione",
                        principalColumn: "IdColtivazione",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TabellaColtivazioneLavoro_Lavoro_LavoroId",
                        column: x => x.LavoroId,
                        principalTable: "Lavoro",
                        principalColumn: "IdLavoro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coltivazione_ColturaId",
                table: "Coltivazione",
                column: "ColturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Coltivazione_TerrenoId",
                table: "Coltivazione",
                column: "TerrenoId");

            migrationBuilder.CreateIndex(
                name: "IX_Spesa_LavoroId",
                table: "Spesa",
                column: "LavoroId");

            migrationBuilder.CreateIndex(
                name: "IX_TabellaColtivazioneLavoro_ColtivazioneId",
                table: "TabellaColtivazioneLavoro",
                column: "ColtivazioneId");

            migrationBuilder.CreateIndex(
                name: "IX_TabellaColtivazioneLavoro_LavoroId",
                table: "TabellaColtivazioneLavoro",
                column: "LavoroId");

            migrationBuilder.CreateIndex(
                name: "IX_TabellaLavoroLavoratore_LavoratoreId",
                table: "TabellaLavoroLavoratore",
                column: "LavoratoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TabellaLavoroLavoratore_LavoroId",
                table: "TabellaLavoroLavoratore",
                column: "LavoroId");

            migrationBuilder.CreateIndex(
                name: "IX_Terreno_LocalitaId",
                table: "Terreno",
                column: "LocalitaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spesa");

            migrationBuilder.DropTable(
                name: "TabellaColtivazioneLavoro");

            migrationBuilder.DropTable(
                name: "TabellaLavoroLavoratore");

            migrationBuilder.DropTable(
                name: "Coltivazione");

            migrationBuilder.DropTable(
                name: "Lavoratore");

            migrationBuilder.DropTable(
                name: "Lavoro");

            migrationBuilder.DropTable(
                name: "Coltura");

            migrationBuilder.DropTable(
                name: "Terreno");

            migrationBuilder.DropTable(
                name: "Localita");
        }
    }
}
