using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendCoffee.CustomerManagement.Infrastructure.Persistence.Migrations
{
    public partial class AddRowVersionProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Customers",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Customers");
        }
    }
}
