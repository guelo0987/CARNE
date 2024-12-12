using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARNE.Migrations
{
    /// <inheritdoc />
    public partial class arreglar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdAdminInspector",
                table: "Inspecciones",
                newName: "idAdminInspector");

            migrationBuilder.AddColumn<int>(
                name: "IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inspecciones_idAdminInspector",
                table: "Inspecciones",
                column: "idAdminInspector");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Inspecciones_Admin_idAdminInspector",
                table: "Inspecciones",
                column: "idAdminInspector",
                principalTable: "Admin",
                principalColumn: "idAdmin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspecciones_Admin_IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspecciones_Admin_idAdminInspector",
                table: "Inspecciones");

            migrationBuilder.DropIndex(
                name: "IX_Inspecciones_idAdminInspector",
                table: "Inspecciones");

            migrationBuilder.DropIndex(
                name: "IX_Inspecciones_IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones");

            migrationBuilder.DropColumn(
                name: "IdAdminInspectorNavigationIdAdmin",
                table: "Inspecciones");

            migrationBuilder.RenameColumn(
                name: "idAdminInspector",
                table: "Inspecciones",
                newName: "IdAdminInspector");
        }
    }
}
