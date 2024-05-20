using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneTerreniAgricoli.Migrations
{
    /// <inheritdoc />
    public partial class RimozioneDeleteOnCascadeColtura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coltivazione_Coltura_ColturaId",
                table: "Coltivazione");

            migrationBuilder.AddForeignKey(
                name: "FK_Coltivazione_Coltura_ColturaId",
                table: "Coltivazione",
                column: "ColturaId",
                principalTable: "Coltura",
                principalColumn: "IdColtura",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coltivazione_Coltura_ColturaId",
                table: "Coltivazione");

            migrationBuilder.AddForeignKey(
                name: "FK_Coltivazione_Coltura_ColturaId",
                table: "Coltivazione",
                column: "ColturaId",
                principalTable: "Coltura",
                principalColumn: "IdColtura",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
