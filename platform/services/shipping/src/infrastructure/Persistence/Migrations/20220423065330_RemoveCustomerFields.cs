using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendCoffee.Shipping.Infrastructure.Persistence.Migrations
{
    public partial class RemoveCustomerFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ShippingOrders");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "ShippingOrders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ShippingOrders");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "ShippingOrders");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ShippingOrders");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "ShippingOrders");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "ShippingOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ShippingOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "ShippingOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ShippingOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "ShippingOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ShippingOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "ShippingOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "ShippingOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
