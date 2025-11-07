using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddHinhAnhToMonAn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "MonAn",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BanAnIdbanAn",
                table: "HoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DatBanIddatBan",
                table: "HoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KhachHangIdkhachHang",
                table: "HoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_BanAnIdbanAn",
                table: "HoaDon",
                column: "BanAnIdbanAn");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_DatBanIddatBan",
                table: "HoaDon",
                column: "DatBanIddatBan");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_KhachHangIdkhachHang",
                table: "HoaDon",
                column: "KhachHangIdkhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_BanAn_BanAnIdbanAn",
                table: "HoaDon",
                column: "BanAnIdbanAn",
                principalTable: "BanAn",
                principalColumn: "IDBanAn",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_DatBan_DatBanIddatBan",
                table: "HoaDon",
                column: "DatBanIddatBan",
                principalTable: "DatBan",
                principalColumn: "IDDatBan",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_KhachHang_KhachHangIdkhachHang",
                table: "HoaDon",
                column: "KhachHangIdkhachHang",
                principalTable: "KhachHang",
                principalColumn: "IDKhachHang",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_BanAn_BanAnIdbanAn",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_DatBan_DatBanIddatBan",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_KhachHang_KhachHangIdkhachHang",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_BanAnIdbanAn",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_DatBanIddatBan",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_KhachHangIdkhachHang",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "MonAn");

            migrationBuilder.DropColumn(
                name: "BanAnIdbanAn",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "DatBanIddatBan",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "KhachHangIdkhachHang",
                table: "HoaDon");
        }
    }
}
