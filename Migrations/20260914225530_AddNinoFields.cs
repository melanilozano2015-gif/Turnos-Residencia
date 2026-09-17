using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResidenciaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNinoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "NinosAdolescentes");

            migrationBuilder.AddColumn<string>(
                name: "Domicilio",
                table: "NinosAdolescentes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "NinosAdolescentes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Escolaridad",
                table: "NinosAdolescentes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstadoActual",
                table: "NinosAdolescentes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "NinosAdolescentes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LugarNacimiento",
                table: "NinosAdolescentes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nacionalidad",
                table: "NinosAdolescentes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroExpediente",
                table: "NinosAdolescentes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObraSocial",
                table: "NinosAdolescentes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "NinosAdolescentes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Domicilio",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "Escolaridad",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "EstadoActual",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "Genero",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "LugarNacimiento",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "Nacionalidad",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "NumeroExpediente",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "ObraSocial",
                table: "NinosAdolescentes");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "NinosAdolescentes");

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "NinosAdolescentes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
