using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace invoice_web_api.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_company_CompanyId",
                table: "Invoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice");

            migrationBuilder.RenameTable(
                name: "Invoice",
                newName: "Invoices");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Invoices",
                newName: "company_id");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_CompanyId",
                table: "Invoices",
                newName: "IX_Invoices_company_id");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "company",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Tax",
                table: "company",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<Guid>(
                name: "company_id",
                table: "Invoices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "client_id",
                table: "Invoices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Invoices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "directory",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "invoice_number",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Invoices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "invoice_id");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_name = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    address_1 = table.Column<string>(type: "text", nullable: true),
                    address_2 = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.client_id);
                    table.ForeignKey(
                        name: "FK_Clients_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_client_id",
                table: "Invoices",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_user_id",
                table: "Clients",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Clients_client_id",
                table: "Invoices",
                column: "client_id",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_company_company_id",
                table: "Invoices",
                column: "company_id",
                principalTable: "company",
                principalColumn: "company_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Clients_client_id",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_company_company_id",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_client_id",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "company");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "company");

            migrationBuilder.DropColumn(
                name: "client_id",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "directory",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "invoice_number",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Invoices");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoice");

            migrationBuilder.RenameColumn(
                name: "company_id",
                table: "Invoice",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_company_id",
                table: "Invoice",
                newName: "IX_Invoice_CompanyId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Invoice",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice",
                column: "invoice_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_company_CompanyId",
                table: "Invoice",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "company_id");
        }
    }
}
