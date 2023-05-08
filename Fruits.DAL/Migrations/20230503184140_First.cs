using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fruits.DAL.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FruitsCatalog",
                columns: table => new
                {
                    IdFruitsCatalog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Class = table.Column<string>(type: "ntext", nullable: false),
                    Sort = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FruitsCatalog", x => x.IdFruitsCatalog);
                });

            migrationBuilder.CreateTable(
                name: "ProvidersCatalog",
                columns: table => new
                {
                    IdProviderCatalog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameProvider = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvidersCatalog", x => x.IdProviderCatalog);
                });

            migrationBuilder.CreateTable(
                name: "PriceCatalog",
                columns: table => new
                {
                    IdPriceCatalog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IdFruitsCatalog = table.Column<int>(type: "int", nullable: false),
                    IdProviderCatalog = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceCatalog", x => x.IdPriceCatalog);
                    table.ForeignKey(
                        name: "FK_PriceCatalog_FruitsCatalog_IdFruitsCatalog",
                        column: x => x.IdFruitsCatalog,
                        principalTable: "FruitsCatalog",
                        principalColumn: "IdFruitsCatalog",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceCatalog_ProvidersCatalog_IdProviderCatalog",
                        column: x => x.IdProviderCatalog,
                        principalTable: "ProvidersCatalog",
                        principalColumn: "IdProviderCatalog",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    IdStock = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdProviderCatalog = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.IdStock);
                    table.ForeignKey(
                        name: "FK_Stock_ProvidersCatalog_IdProviderCatalog",
                        column: x => x.IdProviderCatalog,
                        principalTable: "ProvidersCatalog",
                        principalColumn: "IdProviderCatalog",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockFruits",
                columns: table => new
                {
                    IdStockFruits = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Mass = table.Column<int>(type: "int", nullable: false),
                    IdFruitsCatalog = table.Column<int>(type: "int", nullable: false),
                    IdStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockFruits", x => x.IdStockFruits);
                    table.ForeignKey(
                        name: "FK_StockFruits_FruitsCatalog_IdFruitsCatalog",
                        column: x => x.IdFruitsCatalog,
                        principalTable: "FruitsCatalog",
                        principalColumn: "IdFruitsCatalog",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockFruits_Stock_IdStock",
                        column: x => x.IdStock,
                        principalTable: "Stock",
                        principalColumn: "IdStock",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatalog_IdFruitsCatalog",
                table: "PriceCatalog",
                column: "IdFruitsCatalog");

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatalog_IdProviderCatalog",
                table: "PriceCatalog",
                column: "IdProviderCatalog");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IdProviderCatalog",
                table: "Stock",
                column: "IdProviderCatalog");

            migrationBuilder.CreateIndex(
                name: "IX_StockFruits_IdFruitsCatalog",
                table: "StockFruits",
                column: "IdFruitsCatalog");

            migrationBuilder.CreateIndex(
                name: "IX_StockFruits_IdStock",
                table: "StockFruits",
                column: "IdStock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceCatalog");

            migrationBuilder.DropTable(
                name: "StockFruits");

            migrationBuilder.DropTable(
                name: "FruitsCatalog");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "ProvidersCatalog");
        }
    }
}
