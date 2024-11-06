using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicoRMP.Migrations;

/// <inheritdoc />
public partial class renamePropertyRepairServiceTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PropertyRepairs_Users_CustomerId",
            table: "PropertyRepairs");

        migrationBuilder.RenameColumn(
            name: "CustomerId",
            table: "PropertyRepairs",
            newName: "RepairerId");

        migrationBuilder.RenameIndex(
            name: "IX_PropertyRepairs_CustomerId",
            table: "PropertyRepairs",
            newName: "IX_PropertyRepairs_RepairerId");

        migrationBuilder.AddForeignKey(
            name: "FK_PropertyRepairs_Users_RepairerId",
            table: "PropertyRepairs",
            column: "RepairerId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PropertyRepairs_Users_RepairerId",
            table: "PropertyRepairs");

        migrationBuilder.RenameColumn(
            name: "RepairerId",
            table: "PropertyRepairs",
            newName: "CustomerId");

        migrationBuilder.RenameIndex(
            name: "IX_PropertyRepairs_RepairerId",
            table: "PropertyRepairs",
            newName: "IX_PropertyRepairs_CustomerId");

        migrationBuilder.AddForeignKey(
            name: "FK_PropertyRepairs_Users_CustomerId",
            table: "PropertyRepairs",
            column: "CustomerId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
