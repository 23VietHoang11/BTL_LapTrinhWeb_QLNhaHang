using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagement.Models.Entities;

public partial class QLNhaHangContext : DbContext
{
    public QLNhaHangContext()
    {
    }

    public QLNhaHangContext(DbContextOptions<QLNhaHangContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BanAn> BanAns { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<DatBan> DatBans { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<Kho> Khos { get; set; }

    public virtual DbSet<MonAn> MonAns { get; set; }

    public virtual DbSet<MonAnNguyenLieu> MonAnNguyenLieus { get; set; }

    public virtual DbSet<NguyenLieu> NguyenLieus { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhieuNhapKho> PhieuNhapKhos { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    public virtual DbSet<TonKho> TonKhos { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BanAn>(entity =>
        {
            entity.HasKey(e => e.IdbanAn).HasName("PK__BanAn__39899B20481D5716");

            entity.ToTable("BanAn");

            entity.Property(e => e.IdbanAn).HasColumnName("IDBanAn");
            entity.Property(e => e.LoaiBan).HasMaxLength(50);
            entity.Property(e => e.SucChua).HasDefaultValue(0);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.IdhoaDon, e.IdmonAn }).HasName("PK__ChiTietH__7208CE0FEB303B17");

            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.IdhoaDon).HasColumnName("IDHoaDon");
            entity.Property(e => e.IdmonAn).HasColumnName("IDMonAn");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GhiChu).HasMaxLength(255);

            entity.HasOne(d => d.IdhoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.IdhoaDon)
                .HasConstraintName("FK__ChiTietHo__IDHoa__09A971A2");

            entity.HasOne(d => d.IdmonAnNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.IdmonAn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__IDMon__0A9D95DB");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => new { e.IdphieuNhapKho, e.IdnguyenLieu }).HasName("PK__ChiTietP__7EDD060DA488CBFA");

            entity.ToTable("ChiTietPhieuNhap");

            entity.Property(e => e.IdphieuNhapKho).HasColumnName("IDPhieuNhapKho");
            entity.Property(e => e.IdnguyenLieu).HasColumnName("IDNguyenLieu");
            entity.Property(e => e.DonGiaNhap).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdnguyenLieuNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.IdnguyenLieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__IDNgu__7C4F7684");

            entity.HasOne(d => d.IdphieuNhapKhoNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.IdphieuNhapKho)
                .HasConstraintName("FK__ChiTietPh__IDPhi__7B5B524B");
        });

        modelBuilder.Entity<DatBan>(entity =>
        {
            entity.HasKey(e => e.IddatBan).HasName("PK__DatBan__9CFC96CA0B7FC3DA");

            entity.ToTable("DatBan");

            entity.Property(e => e.IddatBan).HasColumnName("IDDatBan");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.IdbanAn).HasColumnName("IDBanAn");
            entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");
            entity.Property(e => e.SoLuongKh).HasColumnName("SoLuongKH");
            entity.Property(e => e.ThoiGian).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.IdbanAnNavigation).WithMany(p => p.DatBans)
                .HasForeignKey(d => d.IdbanAn)
                .HasConstraintName("FK__DatBan__IDBanAn__5165187F");

            entity.HasOne(d => d.IdkhachHangNavigation).WithMany(p => p.DatBans)
                .HasForeignKey(d => d.IdkhachHang)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__DatBan__IDKhachH__5070F446");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.IdhoaDon).HasName("PK__HoaDon__5B896F49A6180061");

            entity.ToTable("HoaDon");

            entity.Property(e => e.IdhoaDon).HasColumnName("IDHoaDon");
            entity.Property(e => e.IdbanAn).HasColumnName("IDBanAn");
            entity.Property(e => e.IddatBan).HasColumnName("IDDatBan");
            entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");
            entity.Property(e => e.IdnhanVien).HasColumnName("IDNhanVien");
            entity.Property(e => e.NgayLap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThoiGianGiao).HasColumnType("datetime");
            entity.Property(e => e.TongTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.IdbanAnNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IdbanAn)
                .HasConstraintName("FK__HoaDon__IDBanAn__05D8E0BE");

            entity.HasOne(d => d.IddatBanNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IddatBan)
                .HasConstraintName("FK__HoaDon__IDDatBan__06CD04F7");

            entity.HasOne(d => d.IdkhachHangNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IdkhachHang)
                .HasConstraintName("FK__HoaDon__IDKhachH__03F0984C");

            entity.HasOne(d => d.IdnhanVienNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IdnhanVien)
                .HasConstraintName("FK__HoaDon__IDNhanVi__04E4BC85");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.IdkhachHang).HasName("PK__KhachHan__5A7167B54581CC1A");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.Email, "UQ__KhachHan__A9D10534B2B15251").IsUnique();

            entity.HasIndex(e => e.Sdt, "UQ__KhachHan__CA1930A5AD35A601").IsUnique();

            entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTenKh)
                .HasMaxLength(100)
                .HasColumnName("HoTenKH");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");
        });

        modelBuilder.Entity<Kho>(entity =>
        {
            entity.HasKey(e => e.Idkho).HasName("PK__Kho__939E101782446E63");

            entity.ToTable("Kho");

            entity.Property(e => e.Idkho).HasColumnName("IDKho");
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.TenKho).HasMaxLength(100);
        });

        modelBuilder.Entity<MonAn>(entity =>
        {
            entity.HasKey(e => e.IdmonAn).HasName("PK__MonAn__981A14636D5F2EB0");

            entity.ToTable("MonAn");

            entity.HasIndex(e => e.TenMon, "UQ__MonAn__332EF56534AB0ACB").IsUnique();

            entity.Property(e => e.IdmonAn).HasColumnName("IDMonAn");
            entity.Property(e => e.DonViTinh).HasMaxLength(20);
            entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Loai).HasMaxLength(50);
            entity.Property(e => e.TenMon).HasMaxLength(100);
        });

        modelBuilder.Entity<MonAnNguyenLieu>(entity =>
        {
            entity.HasKey(e => new { e.IdmonAn, e.IdnguyenLieu }).HasName("PK__MonAn_Ng__7A13E4EC21BB9103");

            entity.ToTable("MonAn_NguyenLieu");

            entity.Property(e => e.IdmonAn).HasColumnName("IDMonAn");
            entity.Property(e => e.IdnguyenLieu).HasColumnName("IDNguyenLieu");

            entity.HasOne(d => d.IdmonAnNavigation).WithMany(p => p.MonAnNguyenLieus)
                .HasForeignKey(d => d.IdmonAn)
                .HasConstraintName("FK__MonAn_Ngu__IDMon__123EB7A3");

            entity.HasOne(d => d.IdnguyenLieuNavigation).WithMany(p => p.MonAnNguyenLieus)
                .HasForeignKey(d => d.IdnguyenLieu)
                .HasConstraintName("FK__MonAn_Ngu__IDNgu__1332DBDC");
        });

        modelBuilder.Entity<NguyenLieu>(entity =>
        {
            entity.HasKey(e => e.IdnguyenLieu).HasName("PK__NguyenLi__209F08FFB06641A3");

            entity.ToTable("NguyenLieu");

            entity.HasIndex(e => e.TenNl, "UQ__NguyenLi__4CF9B492CE50D4D5").IsUnique();

            entity.Property(e => e.IdnguyenLieu).HasColumnName("IDNguyenLieu");
            entity.Property(e => e.DonVi).HasMaxLength(20);
            entity.Property(e => e.Loai).HasMaxLength(50);
            entity.Property(e => e.TenNl)
                .HasMaxLength(100)
                .HasColumnName("TenNL");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.IdnhaCungCap).HasName("PK__NhaCungC__47B484CD54A4413C");

            entity.ToTable("NhaCungCap");

            entity.HasIndex(e => e.Sdt, "UQ__NhaCungC__CA1930A516E7692B").IsUnique();

            entity.Property(e => e.IdnhaCungCap).HasColumnName("IDNhaCungCap");
            entity.Property(e => e.DiaChiNcc)
                .HasMaxLength(255)
                .HasColumnName("DiaChiNCC");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenNcc)
                .HasMaxLength(200)
                .HasColumnName("TenNCC");

            entity.HasMany(d => d.IdnguyenLieus).WithMany(p => p.IdnhaCungCaps)
                .UsingEntity<Dictionary<string, object>>(
                    "CungCap",
                    r => r.HasOne<NguyenLieu>().WithMany()
                        .HasForeignKey("IdnguyenLieu")
                        .HasConstraintName("FK__CungCap__IDNguye__6C190EBB"),
                    l => l.HasOne<NhaCungCap>().WithMany()
                        .HasForeignKey("IdnhaCungCap")
                        .HasConstraintName("FK__CungCap__IDNhaCu__6B24EA82"),
                    j =>
                    {
                        j.HasKey("IdnhaCungCap", "IdnguyenLieu").HasName("PK__CungCap__A5BD74429467ED85");
                        j.ToTable("CungCap");
                        j.IndexerProperty<int>("IdnhaCungCap").HasColumnName("IDNhaCungCap");
                        j.IndexerProperty<int>("IdnguyenLieu").HasColumnName("IDNguyenLieu");
                    });
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.IdnhanVien).HasName("PK__NhanVien__7AC2D9F704E1BEDB");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.Email, "UQ__NhanVien__A9D10534A86AA149").IsUnique();

            entity.HasIndex(e => e.Sdt, "UQ__NhanVien__CA1930A5C1CE5263").IsUnique();

            entity.Property(e => e.IdnhanVien).HasColumnName("IDNhanVien");
            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTenNv)
                .HasMaxLength(100)
                .HasColumnName("HoTenNV");
            entity.Property(e => e.Luong).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");
        });

        modelBuilder.Entity<PhieuNhapKho>(entity =>
        {
            entity.HasKey(e => e.IdphieuNhapKho).HasName("PK__PhieuNha__9CD4F6821276C3F2");

            entity.ToTable("PhieuNhapKho");

            entity.Property(e => e.IdphieuNhapKho).HasColumnName("IDPhieuNhapKho");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.IdnhaCungCap).HasColumnName("IDNhaCungCap");
            entity.Property(e => e.IdnhanVien).HasColumnName("IDNhanVien");
            entity.Property(e => e.NgayNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdnhaCungCapNavigation).WithMany(p => p.PhieuNhapKhos)
                .HasForeignKey(d => d.IdnhaCungCap)
                .HasConstraintName("FK__PhieuNhap__IDNha__787EE5A0");

            entity.HasOne(d => d.IdnhanVienNavigation).WithMany(p => p.PhieuNhapKhos)
                .HasForeignKey(d => d.IdnhanVien)
                .HasConstraintName("FK__PhieuNhap__IDNha__778AC167");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.IDQuyen).HasName("PK__Quyen__B3A2827E970DEFBE");

            entity.ToTable("Quyen");

            entity.HasIndex(e => e.TenQuyen, "UQ__Quyen__5637EE7992E70F48").IsUnique();

            entity.Property(e => e.IDQuyen).HasColumnName("IDQuyen");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.IDTaiKhoan).HasName("PK__TaiKhoan__BC5F907C1EF66573");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC007CE6683").IsUnique();

            entity.HasIndex(e => e.IDNhanVien, "UQ__TaiKhoan__7AC2D9F6D8F4C6B1").IsUnique();

            entity.Property(e => e.IDTaiKhoan).HasColumnName("IDTaiKhoan");
            entity.Property(e => e.IDNhanVien).HasColumnName("IDNhanVien");
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Hoạt động");

            entity.HasOne(d => d.IdnhanVienNavigation).WithOne(p => p.TaiKhoan)
                .HasForeignKey<TaiKhoan>(d => d.IDNhanVien)
                .HasConstraintName("FK__TaiKhoan__IDNhan__5EBF139D");

            entity.HasMany(d => d.Idquyens).WithMany(p => p.IdtaiKhoans)
                .UsingEntity<Dictionary<string, object>>(
                    "TaiKhoanQuyen",
                    r => r.HasOne<Quyen>().WithMany()
                        .HasForeignKey("Idquyen")
                        .HasConstraintName("FK__TaiKhoan___IDQuy__628FA481"),
                    l => l.HasOne<TaiKhoan>().WithMany()
                        .HasForeignKey("IdtaiKhoan")
                        .HasConstraintName("FK__TaiKhoan___IDTai__619B8048"),
                    j =>
                    {
                        j.HasKey("IdtaiKhoan", "Idquyen").HasName("PK__TaiKhoan__4765B85BE3A780AC");
                        j.ToTable("TaiKhoan_Quyen");
                        j.IndexerProperty<int>("IdtaiKhoan").HasColumnName("IDTaiKhoan");
                        j.IndexerProperty<int>("Idquyen").HasColumnName("IDQuyen");
                    });
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.IdthanhToan).HasName("PK__ThanhToa__DC57C3A1D3E43009");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.IdthanhToan).HasColumnName("IDThanhToan");
            entity.Property(e => e.IdhoaDon).HasColumnName("IDHoaDon");
            entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");
            entity.Property(e => e.NgayThanhToan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhuongThuc).HasMaxLength(50);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdhoaDonNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.IdhoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThanhToan__IDHoa__0E6E26BF");

            entity.HasOne(d => d.IdkhachHangNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.IdkhachHang)
                .HasConstraintName("FK__ThanhToan__IDKha__0F624AF8");
        });

        modelBuilder.Entity<TonKho>(entity =>
        {
            entity.HasKey(e => new { e.Idkho, e.IdnguyenLieu }).HasName("PK__TonKho__7197E098D3AC3031");

            entity.ToTable("TonKho");

            entity.Property(e => e.Idkho).HasColumnName("IDKho");
            entity.Property(e => e.IdnguyenLieu).HasColumnName("IDNguyenLieu");

            entity.HasOne(d => d.IdkhoNavigation).WithMany(p => p.TonKhos)
                .HasForeignKey(d => d.Idkho)
                .HasConstraintName("FK__TonKho__IDKho__71D1E811");

            entity.HasOne(d => d.IdnguyenLieuNavigation).WithMany(p => p.TonKhos)
                .HasForeignKey(d => d.IdnguyenLieu)
                .HasConstraintName("FK__TonKho__IDNguyen__72C60C4A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
