using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneTerreniAgricoli.Migrations
{
    /// <inheritdoc />
    public partial class rimozioneDeleteOnCascadeSpesaLavoro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spesa_Lavoro_LavoroId",
                table: "Spesa");

            migrationBuilder.AddForeignKey(
                name: "FK_Spesa_Lavoro_LavoroId",
                table: "Spesa",
                column: "LavoroId",
                principalTable: "Lavoro",
                principalColumn: "IdLavoro",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spesa_Lavoro_LavoroId",
                table: "Spesa");

            migrationBuilder.AddForeignKey(
                name: "FK_Spesa_Lavoro_LavoroId",
                table: "Spesa",
                column: "LavoroId",
                principalTable: "Lavoro",
                principalColumn: "IdLavoro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
