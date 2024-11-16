using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicoRMP.Database.Migrations
{
    /// <inheritdoc />
    public partial class E9Number_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "E9Number",
                table: "PropertyItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyItems_E9Number",
                table: "PropertyItems",
                column: "E9Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyItems_E9Number",
                table: "PropertyItems");

            migrationBuilder.AlterColumn<string>(
                name: "E9Number",
                table: "PropertyItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
