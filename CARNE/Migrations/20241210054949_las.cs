using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARNE.Migrations
{
    public partial class las : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pruebaa",
                table: "RolePermisos",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pruebaa",
                table: "RolePermisos"
            );
        }
    }
}