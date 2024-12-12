using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARNE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInspeccione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coordenadas",
                table: "Inspecciones");

            migrationBuilder.DropColumn(
                name: "direccion",
                table: "Inspecciones");

            migrationBuilder.AlterColumn<int>(
                name: "idSolicitud",
                table: "Inspecciones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "idAdmin",
                table: "Inspecciones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "idSolicitud",
                table: "Inspecciones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idAdmin",
                table: "Inspecciones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "coordenadas",
                table: "Inspecciones",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "direccion",
                table: "Inspecciones",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true);
        }
    }
}
