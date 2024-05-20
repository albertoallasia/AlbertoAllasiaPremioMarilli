using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneTerreniAgricoli.Migrations
{
    /// <inheritdoc />
    public partial class rimozioneDeleteOnCascadeColtivazioneTerreno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coltivazione_Terreno_TerrenoId",
                table: "Coltivazione");

            migrationBuilder.AddForeignKey(
                name: "FK_Coltivazione_Terreno_TerrenoId",
                table: "Coltivazione",
                column: "TerrenoId",
                principalTable: "Terreno",
                principalColumn: "IdTerreno",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coltivazione_Terreno_TerrenoId",
                table: "Coltivazione");

            migrationBuilder.AddForeignKey(
                name: "FK_Coltivazione_Terreno_TerrenoId",
                table: "Coltivazione",
                column: "TerrenoId",
                principalTable: "Terreno",
                principalColumn: "IdTerreno",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
