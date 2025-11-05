using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class SuaBangTaiKhoan_BaoMatMatKhau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatKhau",
                table: "TaiKhoan");

            migrationBuilder.AddColumn<byte[]>(
                name: "MatKhauHash",
                table: "TaiKhoan",
                type: "varbinary(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "MatKhauSalt",
                table: "TaiKhoan",
                type: "varbinary(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Cccd",
                table: "NhanVien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "NgaySinh",
                table: "NhanVien",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatKhauHash",
                table: "TaiKhoan");

            migrationBuilder.DropColumn(
                name: "MatKhauSalt",
                table: "TaiKhoan");

            migrationBuilder.DropColumn(
                name: "Cccd",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "NhanVien");

            migrationBuilder.AddColumn<string>(
                name: "MatKhau",
                table: "TaiKhoan",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
