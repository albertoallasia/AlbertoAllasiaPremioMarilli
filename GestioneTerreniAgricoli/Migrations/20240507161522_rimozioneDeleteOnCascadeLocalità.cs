using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneTerreniAgricoli.Migrations
{
    /// <inheritdoc />
    public partial class rimozioneDeleteOnCascadeLocalità : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terreno_Localita_LocalitaId",
                table: "Terreno");

            migrationBuilder.AddForeignKey(
                name: "FK_Terreno_Localita_LocalitaId",
                table: "Terreno",
                column: "LocalitaId",
                principalTable: "Localita",
                principalColumn: "IdLocalita",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terreno_Localita_LocalitaId",
                table: "Terreno");

            migrationBuilder.AddForeignKey(
                name: "FK_Terreno_Localita_LocalitaId",
                table: "Terreno",
                column: "LocalitaId",
                principalTable: "Localita",
                principalColumn: "IdLocalita",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
