using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryService.Context.Migrations
{
    /// <inheritdoc />
    public partial class M5GatheringAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GatheringAddress",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GatheringAddress",
                table: "Packages");
        }
    }
}
