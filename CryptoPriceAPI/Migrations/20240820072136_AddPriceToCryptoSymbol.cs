using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoPriceAPI.Migrations
{
    public partial class AddPriceToCryptoSymbol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CryptoSymbols",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CryptoSymbols");
        }
    }
}
