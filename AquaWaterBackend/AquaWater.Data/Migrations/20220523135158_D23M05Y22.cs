using Microsoft.EntityFrameworkCore.Migrations;

namespace AquaWater.Data.Migrations
{
    public partial class D23M05Y22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessEmail",
                table: "CompanyManagers");

            migrationBuilder.DropColumn(
                name: "BusinessPhoneNumber",
                table: "CompanyManagers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Companies",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "BusinessEmail",
                table: "CompanyManagers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessPhoneNumber",
                table: "CompanyManagers",
                type: "text",
                nullable: true);
        }
    }
}
