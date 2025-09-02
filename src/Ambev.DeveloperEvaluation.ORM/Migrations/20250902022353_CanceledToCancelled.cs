using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class CanceledToCancelled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCanceled",
                table: "Sales",
                newName: "IsCancelled");

            migrationBuilder.RenameColumn(
                name: "IsCanceled",
                table: "SaleItems",
                newName: "IsCancelled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCancelled",
                table: "Sales",
                newName: "IsCanceled");

            migrationBuilder.RenameColumn(
                name: "IsCancelled",
                table: "SaleItems",
                newName: "IsCanceled");
        }
    }
}
