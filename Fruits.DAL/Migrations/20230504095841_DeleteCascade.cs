using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fruits.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceCatalog_FruitsCatalog_IdFruitsCatalog",
                table: "PriceCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceCatalog_ProvidersCatalog_IdProviderCatalog",
                table: "PriceCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_ProvidersCatalog_IdProviderCatalog",
                table: "Stock");

            migrationBuilder.DropForeignKey(
                name: "FK_StockFruits_FruitsCatalog_IdFruitsCatalog",
                table: "StockFruits");

            migrationBuilder.DropForeignKey(
                name: "FK_StockFruits_Stock_IdStock",
                table: "StockFruits");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceCatalog_FruitsCatalog_IdFruitsCatalog",
                table: "PriceCatalog",
                column: "IdFruitsCatalog",
                principalTable: "FruitsCatalog",
                principalColumn: "IdFruitsCatalog",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceCatalog_ProvidersCatalog_IdProviderCatalog",
                table: "PriceCatalog",
                column: "IdProviderCatalog",
                principalTable: "ProvidersCatalog",
                principalColumn: "IdProviderCatalog",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_ProvidersCatalog_IdProviderCatalog",
                table: "Stock",
                column: "IdProviderCatalog",
                principalTable: "ProvidersCatalog",
                principalColumn: "IdProviderCatalog",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockFruits_FruitsCatalog_IdFruitsCatalog",
                table: "StockFruits",
                column: "IdFruitsCatalog",
                principalTable: "FruitsCatalog",
                principalColumn: "IdFruitsCatalog",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockFruits_Stock_IdStock",
                table: "StockFruits",
                column: "IdStock",
                principalTable: "Stock",
                principalColumn: "IdStock",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceCatalog_FruitsCatalog_IdFruitsCatalog",
                table: "PriceCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceCatalog_ProvidersCatalog_IdProviderCatalog",
                table: "PriceCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_ProvidersCatalog_IdProviderCatalog",
                table: "Stock");

            migrationBuilder.DropForeignKey(
                name: "FK_StockFruits_FruitsCatalog_IdFruitsCatalog",
                table: "StockFruits");

            migrationBuilder.DropForeignKey(
                name: "FK_StockFruits_Stock_IdStock",
                table: "StockFruits");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceCatalog_FruitsCatalog_IdFruitsCatalog",
                table: "PriceCatalog",
                column: "IdFruitsCatalog",
                principalTable: "FruitsCatalog",
                principalColumn: "IdFruitsCatalog",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceCatalog_ProvidersCatalog_IdProviderCatalog",
                table: "PriceCatalog",
                column: "IdProviderCatalog",
                principalTable: "ProvidersCatalog",
                principalColumn: "IdProviderCatalog",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_ProvidersCatalog_IdProviderCatalog",
                table: "Stock",
                column: "IdProviderCatalog",
                principalTable: "ProvidersCatalog",
                principalColumn: "IdProviderCatalog",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockFruits_FruitsCatalog_IdFruitsCatalog",
                table: "StockFruits",
                column: "IdFruitsCatalog",
                principalTable: "FruitsCatalog",
                principalColumn: "IdFruitsCatalog",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockFruits_Stock_IdStock",
                table: "StockFruits",
                column: "IdStock",
                principalTable: "Stock",
                principalColumn: "IdStock",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
