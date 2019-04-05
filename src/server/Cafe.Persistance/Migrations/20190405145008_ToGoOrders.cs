using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafe.Persistance.Migrations
{
    public partial class ToGoOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ToGoOrderId",
                table: "MenuItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cashiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShortName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cashiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToGoOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToGoOrders", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_ToGoOrders_ToGoOrderId",
                table: "MenuItems");

            migrationBuilder.DropTable(
                name: "Cashiers");

            migrationBuilder.DropTable(
                name: "ToGoOrders");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ToGoOrderId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ToGoOrderId",
                table: "MenuItems");
        }
    }
}
