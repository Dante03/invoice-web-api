using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace invoice_web_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClientsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Clients_client_id",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_client_id",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "client_id",
                table: "Invoices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "client_id",
                table: "Invoices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_client_id",
                table: "Invoices",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Clients_client_id",
                table: "Invoices",
                column: "client_id",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
