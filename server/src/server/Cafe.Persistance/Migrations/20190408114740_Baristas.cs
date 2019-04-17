using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafe.Persistance.Migrations
{
    public partial class Baristas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BaristaId",
                table: "ToGoOrders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Baristas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShortName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baristas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToGoOrders_BaristaId",
                table: "ToGoOrders",
                column: "BaristaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToGoOrders_Baristas_BaristaId",
                table: "ToGoOrders",
                column: "BaristaId",
                principalTable: "Baristas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToGoOrders_Baristas_BaristaId",
                table: "ToGoOrders");

            migrationBuilder.DropTable(
                name: "Baristas");

            migrationBuilder.DropIndex(
                name: "IX_ToGoOrders_BaristaId",
                table: "ToGoOrders");

            migrationBuilder.DropColumn(
                name: "BaristaId",
                table: "ToGoOrders");
        }
    }
}
