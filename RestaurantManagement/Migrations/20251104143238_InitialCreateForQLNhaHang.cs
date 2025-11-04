using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateForQLNhaHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BanAn",
                columns: table => new
                {
                    IDBanAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SucChua = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoaiBan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BanAn__39899B20481D5716", x => x.IDBanAn);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    IDKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTenKH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhachHan__5A7167B54581CC1A", x => x.IDKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "Kho",
                columns: table => new
                {
                    IDKho = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Kho__939E101782446E63", x => x.IDKho);
                });

            migrationBuilder.CreateTable(
                name: "MonAn",
                columns: table => new
                {
                    IDMonAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MonAn__981A14636D5F2EB0", x => x.IDMonAn);
                });

            migrationBuilder.CreateTable(
                name: "NguyenLieu",
                columns: table => new
                {
                    IDNguyenLieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DonVi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NguyenLi__209F08FFB06641A3", x => x.IDNguyenLieu);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    IDNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNCC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiaChiNCC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SDT = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhaCungC__47B484CD54A4413C", x => x.IDNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    IDNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTenNV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ChucVu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayVaoLam = table.Column<DateOnly>(type: "date", nullable: true),
                    Luong = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanVien__7AC2D9F704E1BEDB", x => x.IDNhanVien);
                });

            migrationBuilder.CreateTable(
                name: "Quyen",
                columns: table => new
                {
                    IDQuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Quyen__B3A2827E970DEFBE", x => x.IDQuyen);
                });

            migrationBuilder.CreateTable(
                name: "DatBan",
                columns: table => new
                {
                    IDDatBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDKhachHang = table.Column<int>(type: "int", nullable: true),
                    IDBanAn = table.Column<int>(type: "int", nullable: true),
                    ThoiGian = table.Column<DateTime>(type: "datetime", nullable: false),
                    SoLuongKH = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DatBan__9CFC96CA0B7FC3DA", x => x.IDDatBan);
                    table.ForeignKey(
                        name: "FK__DatBan__IDBanAn__5165187F",
                        column: x => x.IDBanAn,
                        principalTable: "BanAn",
                        principalColumn: "IDBanAn");
                    table.ForeignKey(
                        name: "FK__DatBan__IDKhachH__5070F446",
                        column: x => x.IDKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "IDKhachHang",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MonAn_NguyenLieu",
                columns: table => new
                {
                    IDMonAn = table.Column<int>(type: "int", nullable: false),
                    IDNguyenLieu = table.Column<int>(type: "int", nullable: false),
                    SoLuongCan = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MonAn_Ng__7A13E4EC21BB9103", x => new { x.IDMonAn, x.IDNguyenLieu });
                    table.ForeignKey(
                        name: "FK__MonAn_Ngu__IDMon__123EB7A3",
                        column: x => x.IDMonAn,
                        principalTable: "MonAn",
                        principalColumn: "IDMonAn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__MonAn_Ngu__IDNgu__1332DBDC",
                        column: x => x.IDNguyenLieu,
                        principalTable: "NguyenLieu",
                        principalColumn: "IDNguyenLieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TonKho",
                columns: table => new
                {
                    IDKho = table.Column<int>(type: "int", nullable: false),
                    IDNguyenLieu = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TonKho__7197E098D3AC3031", x => new { x.IDKho, x.IDNguyenLieu });
                    table.ForeignKey(
                        name: "FK__TonKho__IDKho__71D1E811",
                        column: x => x.IDKho,
                        principalTable: "Kho",
                        principalColumn: "IDKho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__TonKho__IDNguyen__72C60C4A",
                        column: x => x.IDNguyenLieu,
                        principalTable: "NguyenLieu",
                        principalColumn: "IDNguyenLieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CungCap",
                columns: table => new
                {
                    IDNhaCungCap = table.Column<int>(type: "int", nullable: false),
                    IDNguyenLieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CungCap__A5BD74429467ED85", x => new { x.IDNhaCungCap, x.IDNguyenLieu });
                    table.ForeignKey(
                        name: "FK__CungCap__IDNguye__6C190EBB",
                        column: x => x.IDNguyenLieu,
                        principalTable: "NguyenLieu",
                        principalColumn: "IDNguyenLieu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__CungCap__IDNhaCu__6B24EA82",
                        column: x => x.IDNhaCungCap,
                        principalTable: "NhaCungCap",
                        principalColumn: "IDNhaCungCap",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhapKho",
                columns: table => new
                {
                    IDPhieuNhapKho = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNhanVien = table.Column<int>(type: "int", nullable: true),
                    IDNhaCungCap = table.Column<int>(type: "int", nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuNha__9CD4F6821276C3F2", x => x.IDPhieuNhapKho);
                    table.ForeignKey(
                        name: "FK__PhieuNhap__IDNha__778AC167",
                        column: x => x.IDNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "IDNhanVien");
                    table.ForeignKey(
                        name: "FK__PhieuNhap__IDNha__787EE5A0",
                        column: x => x.IDNhaCungCap,
                        principalTable: "NhaCungCap",
                        principalColumn: "IDNhaCungCap");
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    IDTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNhanVien = table.Column<int>(type: "int", nullable: false),
                    TenDangNhap = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Hoạt động"),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TaiKhoan__BC5F907C1EF66573", x => x.IDTaiKhoan);
                    table.ForeignKey(
                        name: "FK__TaiKhoan__IDNhan__5EBF139D",
                        column: x => x.IDNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "IDNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    IDHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDKhachHang = table.Column<int>(type: "int", nullable: true),
                    IDNhanVien = table.Column<int>(type: "int", nullable: true),
                    IDBanAn = table.Column<int>(type: "int", nullable: true),
                    IDDatBan = table.Column<int>(type: "int", nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThoiGianGiao = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HoaDon__5B896F49A6180061", x => x.IDHoaDon);
                    table.ForeignKey(
                        name: "FK__HoaDon__IDBanAn__05D8E0BE",
                        column: x => x.IDBanAn,
                        principalTable: "BanAn",
                        principalColumn: "IDBanAn");
                    table.ForeignKey(
                        name: "FK__HoaDon__IDDatBan__06CD04F7",
                        column: x => x.IDDatBan,
                        principalTable: "DatBan",
                        principalColumn: "IDDatBan");
                    table.ForeignKey(
                        name: "FK__HoaDon__IDKhachH__03F0984C",
                        column: x => x.IDKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "IDKhachHang");
                    table.ForeignKey(
                        name: "FK__HoaDon__IDNhanVi__04E4BC85",
                        column: x => x.IDNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "IDNhanVien");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuNhap",
                columns: table => new
                {
                    IDPhieuNhapKho = table.Column<int>(type: "int", nullable: false),
                    IDNguyenLieu = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<double>(type: "float", nullable: false),
                    DonGiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietP__7EDD060DA488CBFA", x => new { x.IDPhieuNhapKho, x.IDNguyenLieu });
                    table.ForeignKey(
                        name: "FK__ChiTietPh__IDNgu__7C4F7684",
                        column: x => x.IDNguyenLieu,
                        principalTable: "NguyenLieu",
                        principalColumn: "IDNguyenLieu");
                    table.ForeignKey(
                        name: "FK__ChiTietPh__IDPhi__7B5B524B",
                        column: x => x.IDPhieuNhapKho,
                        principalTable: "PhieuNhapKho",
                        principalColumn: "IDPhieuNhapKho",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan_Quyen",
                columns: table => new
                {
                    IDTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    IDQuyen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TaiKhoan__4765B85BE3A780AC", x => new { x.IDTaiKhoan, x.IDQuyen });
                    table.ForeignKey(
                        name: "FK__TaiKhoan___IDQuy__628FA481",
                        column: x => x.IDQuyen,
                        principalTable: "Quyen",
                        principalColumn: "IDQuyen",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__TaiKhoan___IDTai__619B8048",
                        column: x => x.IDTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "IDTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                columns: table => new
                {
                    IDHoaDon = table.Column<int>(type: "int", nullable: false),
                    IDMonAn = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietH__7208CE0FEB303B17", x => new { x.IDHoaDon, x.IDMonAn });
                    table.ForeignKey(
                        name: "FK__ChiTietHo__IDHoa__09A971A2",
                        column: x => x.IDHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "IDHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ChiTietHo__IDMon__0A9D95DB",
                        column: x => x.IDMonAn,
                        principalTable: "MonAn",
                        principalColumn: "IDMonAn");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    IDThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDHoaDon = table.Column<int>(type: "int", nullable: false),
                    IDKhachHang = table.Column<int>(type: "int", nullable: true),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhuongThuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThanhToa__DC57C3A1D3E43009", x => x.IDThanhToan);
                    table.ForeignKey(
                        name: "FK__ThanhToan__IDHoa__0E6E26BF",
                        column: x => x.IDHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "IDHoaDon");
                    table.ForeignKey(
                        name: "FK__ThanhToan__IDKha__0F624AF8",
                        column: x => x.IDKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "IDKhachHang");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_IDMonAn",
                table: "ChiTietHoaDon",
                column: "IDMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhap_IDNguyenLieu",
                table: "ChiTietPhieuNhap",
                column: "IDNguyenLieu");

            migrationBuilder.CreateIndex(
                name: "IX_CungCap_IDNguyenLieu",
                table: "CungCap",
                column: "IDNguyenLieu");

            migrationBuilder.CreateIndex(
                name: "IX_DatBan_IDBanAn",
                table: "DatBan",
                column: "IDBanAn");

            migrationBuilder.CreateIndex(
                name: "IX_DatBan_IDKhachHang",
                table: "DatBan",
                column: "IDKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDBanAn",
                table: "HoaDon",
                column: "IDBanAn");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDDatBan",
                table: "HoaDon",
                column: "IDDatBan");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDKhachHang",
                table: "HoaDon",
                column: "IDKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDNhanVien",
                table: "HoaDon",
                column: "IDNhanVien");

            migrationBuilder.CreateIndex(
                name: "UQ__KhachHan__A9D10534B2B15251",
                table: "KhachHang",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__KhachHan__CA1930A5AD35A601",
                table: "KhachHang",
                column: "SDT",
                unique: true,
                filter: "[SDT] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__MonAn__332EF56534AB0ACB",
                table: "MonAn",
                column: "TenMon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonAn_NguyenLieu_IDNguyenLieu",
                table: "MonAn_NguyenLieu",
                column: "IDNguyenLieu");

            migrationBuilder.CreateIndex(
                name: "UQ__NguyenLi__4CF9B492CE50D4D5",
                table: "NguyenLieu",
                column: "TenNL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__NhaCungC__CA1930A516E7692B",
                table: "NhaCungCap",
                column: "SDT",
                unique: true,
                filter: "[SDT] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__NhanVien__A9D10534A86AA149",
                table: "NhanVien",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__NhanVien__CA1930A5C1CE5263",
                table: "NhanVien",
                column: "SDT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapKho_IDNhaCungCap",
                table: "PhieuNhapKho",
                column: "IDNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapKho_IDNhanVien",
                table: "PhieuNhapKho",
                column: "IDNhanVien");

            migrationBuilder.CreateIndex(
                name: "UQ__Quyen__5637EE7992E70F48",
                table: "Quyen",
                column: "TenQuyen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__TaiKhoan__55F68FC007CE6683",
                table: "TaiKhoan",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__TaiKhoan__7AC2D9F6D8F4C6B1",
                table: "TaiKhoan",
                column: "IDNhanVien",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_Quyen_IDQuyen",
                table: "TaiKhoan_Quyen",
                column: "IDQuyen");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_IDHoaDon",
                table: "ThanhToan",
                column: "IDHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_IDKhachHang",
                table: "ThanhToan",
                column: "IDKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_TonKho_IDNguyenLieu",
                table: "TonKho",
                column: "IDNguyenLieu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHoaDon");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuNhap");

            migrationBuilder.DropTable(
                name: "CungCap");

            migrationBuilder.DropTable(
                name: "MonAn_NguyenLieu");

            migrationBuilder.DropTable(
                name: "TaiKhoan_Quyen");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "TonKho");

            migrationBuilder.DropTable(
                name: "PhieuNhapKho");

            migrationBuilder.DropTable(
                name: "MonAn");

            migrationBuilder.DropTable(
                name: "Quyen");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "Kho");

            migrationBuilder.DropTable(
                name: "NguyenLieu");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "DatBan");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "BanAn");

            migrationBuilder.DropTable(
                name: "KhachHang");
        }
    }
}
