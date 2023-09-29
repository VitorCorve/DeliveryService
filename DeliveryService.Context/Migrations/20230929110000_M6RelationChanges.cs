using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryService.Context.Migrations
{
    /// <inheritdoc />
    public partial class M6RelationChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Couriers_CourierId",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Customers_CustomerId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_CourierId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_CustomerId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "CourierId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Packages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourierId",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_CourierId",
                table: "Packages",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_CustomerId",
                table: "Packages",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Couriers_CourierId",
                table: "Packages",
                column: "CourierId",
                principalTable: "Couriers",
                principalColumn: "CourierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Customers_CustomerId",
                table: "Packages",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
