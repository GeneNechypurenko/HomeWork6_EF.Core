using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeWork6.Migrations
{
    /// <inheritdoc />
    public partial class AddBooksPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pulisher",
                table: "Books",
                newName: "Publisher");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Books",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Publisher",
                table: "Books",
                newName: "Pulisher");
        }
    }
}
