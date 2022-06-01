using Microsoft.EntityFrameworkCore.Migrations;

namespace HousewareWebAPI.Migrations
{
    public partial class ModyfiAddressFollowGHN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "Stores",
                newName: "WardName");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Stores",
                newName: "ProvinceId");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "Stores",
                newName: "DistrictId");

            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "Addresses",
                newName: "WardName");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Addresses",
                newName: "WardId");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "Addresses",
                newName: "ProvinceName");

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WardId",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "WardName",
                table: "Stores",
                newName: "Ward");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Stores",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "DistrictId",
                table: "Stores",
                newName: "District");

            migrationBuilder.RenameColumn(
                name: "WardName",
                table: "Addresses",
                newName: "Ward");

            migrationBuilder.RenameColumn(
                name: "WardId",
                table: "Addresses",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "ProvinceName",
                table: "Addresses",
                newName: "District");
        }
    }
}
