using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicoRMP.Database.Migrations
{
    /// <inheritdoc />
    public partial class M1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRepairs_Users_RepairerId",
                table: "PropertyRepairs");

            migrationBuilder.RenameColumn(
                name: "RepairerId",
                table: "PropertyRepairs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRepairs_RepairerId",
                table: "PropertyRepairs",
                newName: "IX_PropertyRepairs_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "PropertyRepairs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRepairs_Users_UserId",
                table: "PropertyRepairs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRepairs_Users_UserId",
                table: "PropertyRepairs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PropertyRepairs",
                newName: "RepairerId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRepairs_UserId",
                table: "PropertyRepairs",
                newName: "IX_PropertyRepairs_RepairerId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "PropertyRepairs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRepairs_Users_RepairerId",
                table: "PropertyRepairs",
                column: "RepairerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
