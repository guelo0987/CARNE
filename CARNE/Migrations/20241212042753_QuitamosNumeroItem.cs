using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARNE.Migrations
{
    /// <inheritdoc />
    public partial class QuitamosNumeroItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numeroItem",
                table: "ItemsVerificacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "numeroItem",
                table: "ItemsVerificacion",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
