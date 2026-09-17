using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResidenciaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoTurno",
                table: "Turnos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoTurno",
                table: "Turnos");
        }
    }
}
