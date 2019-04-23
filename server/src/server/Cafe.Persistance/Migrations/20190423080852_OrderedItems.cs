using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafe.Persistance.Migrations
{
    public partial class OrderedItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_ToGoOrders_ToGoOrderId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ToGoOrderId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ToGoOrderId",
                table: "MenuItems");

            migrationBuilder.CreateTable(
                name: "ToGoOrderMenuItem",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(nullable: false),
                    MenuItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToGoOrderMenuItem", x => new { x.MenuItemId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_ToGoOrderMenuItem_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToGoOrderMenuItem_ToGoOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ToGoOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToGoOrderMenuItem_OrderId",
                table: "ToGoOrderMenuItem",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToGoOrderMenuItem");

            migrationBuilder.AddColumn<Guid>(
                name: "ToGoOrderId",
                table: "MenuItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ToGoOrderId",
                table: "MenuItems",
                column: "ToGoOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_ToGoOrders_ToGoOrderId",
                table: "MenuItems",
                column: "ToGoOrderId",
                principalTable: "ToGoOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
