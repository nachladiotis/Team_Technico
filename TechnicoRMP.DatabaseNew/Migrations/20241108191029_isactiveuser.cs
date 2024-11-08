using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicoRMP.Migrations
{
    /// <inheritdoc />
    public partial class isactiveuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
            name: "IsActive",
            table: "Users",
            type: "bit",
            nullable: false,
            defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
            name: "IsActive",
            table: "Users",
            type: "bit",
            nullable: false,
            defaultValue: false);
        }
    }
}
