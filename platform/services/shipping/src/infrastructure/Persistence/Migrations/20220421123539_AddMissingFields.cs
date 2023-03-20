using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendCoffee.Shipping.Infrastructure.Persistence.Migrations
{
    public partial class AddMissingFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "ShippingOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "ShippingOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "ShippingOrders");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "ShippingOrders");
        }
    }
}
