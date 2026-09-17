using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResidenciaApp.Migrations
{
    /// <inheritdoc />
    public partial class TurnoFreeTextFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Instituciones_IdInstitucion",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Profesionales_IdProfesional",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_IdInstitucion",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_IdProfesional",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "IdInstitucion",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "IdProfesional",
                table: "Turnos");

            migrationBuilder.AddColumn<int>(
                name: "InstitucionIdInstitucion",
                table: "Turnos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreInstitucion",
                table: "Turnos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreProfesional",
                table: "Turnos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProfesionalIdProfesional",
                table: "Turnos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_InstitucionIdInstitucion",
                table: "Turnos",
                column: "InstitucionIdInstitucion");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_ProfesionalIdProfesional",
                table: "Turnos",
                column: "ProfesionalIdProfesional");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Instituciones_InstitucionIdInstitucion",
                table: "Turnos",
                column: "InstitucionIdInstitucion",
                principalTable: "Instituciones",
                principalColumn: "IdInstitucion");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Profesionales_ProfesionalIdProfesional",
                table: "Turnos",
                column: "ProfesionalIdProfesional",
                principalTable: "Profesionales",
                principalColumn: "IdProfesional");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Instituciones_InstitucionIdInstitucion",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Profesionales_ProfesionalIdProfesional",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_InstitucionIdInstitucion",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_ProfesionalIdProfesional",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "InstitucionIdInstitucion",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "NombreInstitucion",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "NombreProfesional",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "ProfesionalIdProfesional",
                table: "Turnos");

            migrationBuilder.AddColumn<int>(
                name: "IdInstitucion",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdProfesional",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdInstitucion",
                table: "Turnos",
                column: "IdInstitucion");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdProfesional",
                table: "Turnos",
                column: "IdProfesional");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Instituciones_IdInstitucion",
                table: "Turnos",
                column: "IdInstitucion",
                principalTable: "Instituciones",
                principalColumn: "IdInstitucion",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Profesionales_IdProfesional",
                table: "Turnos",
                column: "IdProfesional",
                principalTable: "Profesionales",
                principalColumn: "IdProfesional",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
