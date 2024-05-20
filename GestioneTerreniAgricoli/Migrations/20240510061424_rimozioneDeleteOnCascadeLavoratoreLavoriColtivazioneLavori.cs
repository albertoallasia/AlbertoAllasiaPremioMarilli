using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneTerreniAgricoli.Migrations
{
    /// <inheritdoc />
    public partial class rimozioneDeleteOnCascadeLavoratoreLavoriColtivazioneLavori : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TabellaColtivazioneLavoro_Coltivazione_ColtivazioneId",
                table: "TabellaColtivazioneLavoro");

            migrationBuilder.DropForeignKey(
                name: "FK_TabellaColtivazioneLavoro_Lavoro_LavoroId",
                table: "TabellaColtivazioneLavoro");

            migrationBuilder.DropForeignKey(
                name: "FK_TabellaLavoroLavoratore_Lavoratore_LavoratoreId",
                table: "TabellaLavoroLavoratore");

            migrationBuilder.DropForeignKey(
                name: "FK_TabellaLavoroLavoratore_Lavoro_LavoroId",
                table: "TabellaLavoroLavoratore");

            migrationBuilder.AddForeignKey(
                name: "FK_TabellaColtivazioneLavoro_Coltivazione_ColtivazioneId",
                table: "TabellaColtivazioneLavoro",
                column: "ColtivazioneId",
                principalTable: "Coltivazione",
                principalColumn: "IdColtivazione",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TabellaColtivazioneLavoro_Lavoro_LavoroId",
                table: "TabellaColtivazioneLavoro",
                column: "LavoroId",
                principalTable: "Lavoro",
                principalColumn: "IdLavoro",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TabellaLavoroLavoratore_Lavoratore_LavoratoreId",
                table: "TabellaLavoroLavoratore",
                column: "LavoratoreId",
                principalTable: "Lavoratore",
                principalColumn: "IdLavoratore",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TabellaLavoroLavoratore_Lavoro_LavoroId",
                table: "TabellaLavoroLavoratore",
                column: "LavoroId",
                principalTable: "Lavoro",
                principalColumn: "IdLavoro",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TabellaColtivazioneLavoro_Coltivazione_ColtivazioneId",
                table: "TabellaColtivazioneLavoro");

            migrationBuilder.DropForeignKey(
                name: "FK_TabellaColtivazioneLavoro_Lavoro_LavoroId",
                table: "TabellaColtivazioneLavoro");

            migrationBuilder.DropForeignKey(
                name: "FK_TabellaLavoroLavoratore_Lavoratore_LavoratoreId",
                table: "TabellaLavoroLavoratore");

            migrationBuilder.DropForeignKey(
                name: "FK_TabellaLavoroLavoratore_Lavoro_LavoroId",
                table: "TabellaLavoroLavoratore");

            migrationBuilder.AddForeignKey(
                name: "FK_TabellaColtivazioneLavoro_Coltivazione_ColtivazioneId",
                table: "TabellaColtivazioneLavoro",
                column: "ColtivazioneId",
                principalTable: "Coltivazione",
                principalColumn: "IdColtivazione",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TabellaColtivazioneLavoro_Lavoro_LavoroId",
                table: "TabellaColtivazioneLavoro",
                column: "LavoroId",
                principalTable: "Lavoro",
                principalColumn: "IdLavoro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TabellaLavoroLavoratore_Lavoratore_LavoratoreId",
                table: "TabellaLavoroLavoratore",
                column: "LavoratoreId",
                principalTable: "Lavoratore",
                principalColumn: "IdLavoratore",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TabellaLavoroLavoratore_Lavoro_LavoroId",
                table: "TabellaLavoroLavoratore",
                column: "LavoroId",
                principalTable: "Lavoro",
                principalColumn: "IdLavoro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
