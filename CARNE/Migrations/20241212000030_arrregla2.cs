using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARNE.Migrations
{
    /// <inheritdoc />
    public partial class arrregla2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspecciones_Admin_IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones");

            migrationBuilder.DropIndex(
                name: "IX_Inspecciones_IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones");

            migrationBuilder.DropColumn(
                name: "IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inspecciones_IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones",
                column: "IdAdminInspectorNavigationIdAdmin");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspecciones_Admin_IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones",
                column: "IdAdminInspectorNavigationIdAdmin",
                principalTable: "Admin",
                principalColumn: "idAdmin",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
