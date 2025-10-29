CREATE DATABASE QLNhaHang
GO
USE QLNhaHang
GO

/* TẠO BẢNG */
-- =========== I. QUẢN LÝ KHÁCH HÀNG & ĐẶT CHỖ ===========

-- Bảng 1: KhachHang (Thực thể)
CREATE TABLE KhachHang (
    IDKhachHang INT PRIMARY KEY IDENTITY(1,1),
    HoTenKH NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) UNIQUE,
    Email VARCHAR(100) UNIQUE,
    DiaChi NVARCHAR(255),
    GioiTinh NVARCHAR(10)
);

-- Bảng 2: BanAn (Thực thể)
CREATE TABLE BanAn (
    IDBanAn INT PRIMARY KEY IDENTITY(1,1),
    SucChua INT DEFAULT 0,
    TrangThai NVARCHAR(50), -- Ví dụ: Trống, Đang phục vụ, Đã đặt
    LoaiBan NVARCHAR(50)
);

-- Bảng 3: DatBan (Mối quan hệ có thuộc tính)
CREATE TABLE DatBan (
    IDDatBan INT PRIMARY KEY IDENTITY(1,1),
    IDKhachHang INT,
    IDBanAn INT,
    ThoiGian DATETIME NOT NULL,
    SoLuongKH INT,
    TrangThai NVARCHAR(50), -- Ví dụ: Đã xác nhận, Chờ, Đã hủy
    GhiChu NVARCHAR(500),
    
    FOREIGN KEY (IDKhachHang) REFERENCES KhachHang(IDKhachHang) ON DELETE SET NULL,
    FOREIGN KEY (IDBanAn) REFERENCES BanAn(IDBanAn)
);

-- =========== II. QUẢN LÝ NHÂN VIÊN & PHÂN QUYỀN ===========

-- Bảng 4: NhanVien (Thực thể)
CREATE TABLE NhanVien (
    IDNhanVien INT PRIMARY KEY IDENTITY(1,1),
    HoTenNV NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) UNIQUE NOT NULL,
    Email VARCHAR(100) UNIQUE,
    DiaChi NVARCHAR(255),
    ChucVu NVARCHAR(50),
    NgayVaoLam DATE,
    Luong DECIMAL(18, 2)
);

-- Bảng 5: Quyen (Thực thể)
CREATE TABLE Quyen (
    IDQuyen INT PRIMARY KEY IDENTITY(1,1),
    TenQuyen NVARCHAR(50) NOT NULL UNIQUE, -- Ví dụ: Admin, Quản lý, Lễ tân
    MoTa NVARCHAR(255)
);

-- Bảng 6: TaiKhoan (Thực thể)
CREATE TABLE TaiKhoan (
    IDTaiKhoan INT PRIMARY KEY IDENTITY(1,1),
    IDNhanVien INT UNIQUE NOT NULL, -- Giả định mỗi nhân viên có 1 tài khoản (Quan hệ 1-1)
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhauHash NVARCHAR(255) NOT NULL, -- Không bao giờ lưu mật khẩu rõ
    TrangThai NVARCHAR(20) DEFAULT N'Hoạt động',
    NgayTao DATETIME DEFAULT GETDATE(),
    
    FOREIGN KEY (IDNhanVien) REFERENCES NhanVien(IDNhanVien) ON DELETE CASCADE
);

-- Bảng 7: TaiKhoan_Quyen (Mối quan hệ N-M "Là")
CREATE TABLE TaiKhoan_Quyen (
    IDTaiKhoan INT NOT NULL,
    IDQuyen INT NOT NULL,
    
    PRIMARY KEY (IDTaiKhoan, IDQuyen), -- Khóa chính tổ hợp
    FOREIGN KEY (IDTaiKhoan) REFERENCES TaiKhoan(IDTaiKhoan) ON DELETE CASCADE,
    FOREIGN KEY (IDQuyen) REFERENCES Quyen(IDQuyen) ON DELETE CASCADE
);

-- =========== III. QUẢN LÝ KHO & NGUYÊN LIỆU ===========

-- Bảng 8: NhaCungCap (Thực thể)
CREATE TABLE NhaCungCap (
    IDNhaCungCap INT PRIMARY KEY IDENTITY(1,1),
    TenNCC NVARCHAR(200) NOT NULL,
    DiaChiNCC NVARCHAR(255),
    SDT VARCHAR(15) UNIQUE,
    Email VARCHAR(100)
);

-- Bảng 9: NguyenLieu (Thực thể)
CREATE TABLE NguyenLieu (
    IDNguyenLieu INT PRIMARY KEY IDENTITY(1,1),
    TenNL NVARCHAR(100) NOT NULL UNIQUE,
    DonVi NVARCHAR(20), -- Ví dụ: kg, lít, cái
    Loai NVARCHAR(50)
);

-- Bảng 10: CungCap (Mối quan hệ N-M)
CREATE TABLE CungCap (
    IDNhaCungCap INT NOT NULL,
    IDNguyenLieu INT NOT NULL,
    
    PRIMARY KEY (IDNhaCungCap, IDNguyenLieu),
    FOREIGN KEY (IDNhaCungCap) REFERENCES NhaCungCap(IDNhaCungCap) ON DELETE CASCADE,
    FOREIGN KEY (IDNguyenLieu) REFERENCES NguyenLieu(IDNguyenLieu) ON DELETE CASCADE
);

-- Bảng 11: Kho (Thực thể)
CREATE TABLE Kho (
    IDKho INT PRIMARY KEY IDENTITY(1,1),
    TenKho NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(255)
);

-- Bảng 12: TonKho (Mối quan hệ N-M "NhapVao", đã đổi tên cho rõ nghĩa)
-- Lưu số lượng của từng nguyên liệu trong từng kho
CREATE TABLE TonKho (
    IDKho INT NOT NULL,
    IDNguyenLieu INT NOT NULL,
    SoLuong FLOAT NOT NULL DEFAULT 0,
    
    PRIMARY KEY (IDKho, IDNguyenLieu),
    FOREIGN KEY (IDKho) REFERENCES Kho(IDKho) ON DELETE CASCADE,
    FOREIGN KEY (IDNguyenLieu) REFERENCES NguyenLieu(IDNguyenLieu) ON DELETE CASCADE
);

-- Bảng 13: PhieuNhapKho (Thực thể)
CREATE TABLE PhieuNhapKho (
    IDPhieuNhapKho INT PRIMARY KEY IDENTITY(1,1),
    IDNhanVien INT, -- Nhân viên lập phiếu
    IDNhaCungCap INT, -- Nhập từ NCC nào
    NgayNhap DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2) DEFAULT 0,
    GhiChu NVARCHAR(500),
    
    FOREIGN KEY (IDNhanVien) REFERENCES NhanVien(IDNhanVien),
    FOREIGN KEY (IDNhaCungCap) REFERENCES NhaCungCap(IDNhaCungCap)
);

-- Bảng 14: ChiTietPhieuNhap (Thực thể yếu, phụ thuộc PhieuNhapKho)
CREATE TABLE ChiTietPhieuNhap (
    IDPhieuNhapKho INT NOT NULL,
    IDNguyenLieu INT NOT NULL,
    SoLuong FLOAT NOT NULL,
    DonGiaNhap DECIMAL(18, 2) NOT NULL,
    
    PRIMARY KEY (IDPhieuNhapKho, IDNguyenLieu),
    FOREIGN KEY (IDPhieuNhapKho) REFERENCES PhieuNhapKho(IDPhieuNhapKho) ON DELETE CASCADE,
    FOREIGN KEY (IDNguyenLieu) REFERENCES NguyenLieu(IDNguyenLieu)
);


-- =========== IV. QUẢN LÝ BÁN HÀNG & HÓA ĐƠN ===========

-- Bảng 15: MonAn (Thực thể)
CREATE TABLE MonAn (
    IDMonAn INT PRIMARY KEY IDENTITY(1,1),
    TenMon NVARCHAR(100) NOT NULL UNIQUE,
    Gia DECIMAL(18, 2) NOT NULL,
    DonViTinh NVARCHAR(20), -- Ví dụ: Bát, Đĩa, Suất
    Loai NVARCHAR(50) -- Ví dụ: Món chính, Món phụ, Đồ uống
);

-- Bảng 16: HoaDon (Thực thể - Hợp nhất từ "HoaDon" và "Chi tiet hoa don")
CREATE TABLE HoaDon (
    IDHoaDon INT PRIMARY KEY IDENTITY(1,1),
    IDKhachHang INT, -- Có thể NULL nếu là khách vãng lai
    IDNhanVien INT, -- Nhân viên tạo hóa đơn
    IDBanAn INT, -- Bàn của hóa đơn này
    IDDatBan INT, -- Liên kết đến đặt bàn (nếu có)
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2) DEFAULT 0,
    TrangThai NVARCHAR(50), -- Ví dụ: Chưa thanh toán, Đã thanh toán, Đã hủy
    ThoiGianGiao DATETIME, -- Dùng cho đơn giao hàng
    
    FOREIGN KEY (IDKhachHang) REFERENCES KhachHang(IDKhachHang),
    FOREIGN KEY (IDNhanVien) REFERENCES NhanVien(IDNhanVien),
    FOREIGN KEY (IDBanAn) REFERENCES BanAn(IDBanAn),
    FOREIGN KEY (IDDatBan) REFERENCES DatBan(IDDatBan)
);

-- Bảng 17: ChiTietHoaDon (Mối quan hệ N-M giữa HoaDon và MonAn)
CREATE TABLE ChiTietHoaDon (
    IDHoaDon INT NOT NULL,
    IDMonAn INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL, -- Giá tại thời điểm bán
    GhiChu NVARCHAR(255),
    
    PRIMARY KEY (IDHoaDon, IDMonAn),
    FOREIGN KEY (IDHoaDon) REFERENCES HoaDon(IDHoaDon) ON DELETE CASCADE,
    FOREIGN KEY (IDMonAn) REFERENCES MonAn(IDMonAn)
);

-- Bảng 18: ThanhToan (Mối quan hệ có thuộc tính)
-- Bảng này lưu lại các giao dịch thanh toán. Một hóa đơn có thể được thanh toán nhiều lần (đặt cọc, trả nợ).
CREATE TABLE ThanhToan (
    IDThanhToan INT PRIMARY KEY IDENTITY(1,1),
    IDHoaDon INT NOT NULL,
    IDKhachHang INT,
    NgayThanhToan DATETIME NOT NULL DEFAULT GETDATE(),
    SoTien DECIMAL(18, 2) NOT NULL,
    PhuongThuc NVARCHAR(50), -- Ví dụ: Tiền mặt, Chuyển khoản, Thẻ
    
    FOREIGN KEY (IDHoaDon) REFERENCES HoaDon(IDHoaDon),
    FOREIGN KEY (IDKhachHang) REFERENCES KhachHang(IDKhachHang)
);

-- Bảng 19: (Dự phòng) MonAn_NguyenLieu (Định mức nguyên liệu)
-- Bảng này không có rõ trong sơ đồ nhưng RẤT CẦN THIẾT cho logic nghiệp vụ
-- để biết 1 Món Ăn cần bao nhiêu Nguyên Liệu (để trừ kho).
CREATE TABLE MonAn_NguyenLieu (
    IDMonAn INT NOT NULL,
    IDNguyenLieu INT NOT NULL,
    SoLuongCan FLOAT NOT NULL, -- Số lượng nguyên liệu cần cho 1 đơn vị món ăn
    
    PRIMARY KEY (IDMonAn, IDNguyenLieu),
    FOREIGN KEY (IDMonAn) REFERENCES MonAn(IDMonAn) ON DELETE CASCADE,
    FOREIGN KEY (IDNguyenLieu) REFERENCES NguyenLieu(IDNguyenLieu) ON DELETE CASCADE
);