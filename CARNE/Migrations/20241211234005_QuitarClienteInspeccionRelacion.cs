using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARNE.Migrations
{
    /// <inheritdoc />
    public partial class QuitarClienteInspeccionRelacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM sys.indexes 
                    WHERE name = 'IX_Inspecciones_idUsuarioInspector' 
                    AND object_id = OBJECT_ID('Inspecciones')
                )
                BEGIN
                    DROP INDEX IX_Inspecciones_idUsuarioInspector ON Inspecciones;
                END
            ");

            migrationBuilder.DropForeignKey(
                name: "FK__Inspeccio__idUsu__2A164134",
                table: "Inspecciones");

            migrationBuilder.RenameColumn(
                name: "idUsuarioInspector",
                table: "Inspecciones",
                newName: "IdAdminInspector");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdAdminInspector",
                table: "Inspecciones",
                newName: "idUsuarioInspector");

            migrationBuilder.CreateIndex(
                name: "IX_Inspecciones_idUsuarioInspector",
                table: "Inspecciones",
                column: "idUsuarioInspector");

            migrationBuilder.AddForeignKey(
                name: "FK__Inspeccio__idUsu__2A164134",
                table: "Inspecciones",
                column: "idUsuarioInspector",
                principalTable: "Usuario",
                principalColumn: "idUsuario");
        }
    }
}