CREATE DATABASE DienTuGiaDung2025;
GO
USE DienTuGiaDung2025;
GO

CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(50) NOT NULL,
    GioiTinhKH INT NULL,
    SoDienThoaiKH VARCHAR(10) NOT NULL UNIQUE,
    EmailKH VARCHAR(100) NULL UNIQUE
);
GO

CREATE TABLE TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT NULL UNIQUE,
    TenTK VARCHAR(20) NOT NULL UNIQUE,
    QuyenTK INT NOT NULL DEFAULT 0,
    MatKhauTK VARCHAR(255) NULL,
    NgayTaoTK DATETIME2 NOT NULL DEFAULT GETDATE(),
    NgayCapNhatTK DATETIME2 NOT NULL DEFAULT GETDATE(),
    TrangThaiTK INT NOT NULL DEFAULT 1,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH) ON DELETE CASCADE
);
GO

CREATE TABLE TinhThanhPho (
    MaTTP CHAR(2) PRIMARY KEY,
    TenTTP NVARCHAR(30) NOT NULL UNIQUE
);
GO

CREATE TABLE XaPhuong (
    MaXP CHAR(5) PRIMARY KEY,
    MaTTP CHAR(2) NOT NULL,
    TenXP NVARCHAR(40) NOT NULL,
    FOREIGN KEY (MaTTP) REFERENCES TinhThanhPho(MaTTP)
);
GO

CREATE TABLE DiaChiCuThe (
    MaDCCT INT IDENTITY(1,1) PRIMARY KEY,
    MaXP CHAR(5) NOT NULL,
    MaKH INT NOT NULL,
    TenDCCT NVARCHAR(255) NOT NULL,
    MacDinhDCCT INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaXP) REFERENCES XaPhuong(MaXP),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH) ON DELETE CASCADE
);
GO

CREATE TABLE QuocGia (
    MaQG INT IDENTITY(1,1) PRIMARY KEY,
    TenQG NVARCHAR(50) NOT NULL UNIQUE
);
GO

CREATE TABLE ThuongHieu (
    MaTH INT IDENTITY(1,1) PRIMARY KEY,
    MaQG INT NOT NULL,
    TenTH NVARCHAR(20) NOT NULL UNIQUE,
    FOREIGN KEY (MaQG) REFERENCES QuocGia(MaQG)
);
GO

CREATE TABLE LoaiSanPham (
    MaLSP INT IDENTITY(1,1) PRIMARY KEY,
    TenLSP NVARCHAR(40) NOT NULL UNIQUE,
    ThueGTGTLSP DECIMAL(4,2) NOT NULL DEFAULT 0,
    TrangThaiLSP INT NOT NULL DEFAULT 1
);
GO

CREATE TABLE SanPham (
    MaSP INT IDENTITY(1, 1) PRIMARY KEY,
    MaQG INT NULL,
    MaTH INT NULL,
    MaLSP INT NULL,
    TenSP NVARCHAR(100) NOT NULL,
    SoLuongSP INT NOT NULL DEFAULT 0,
    GiaNhapSP DECIMAL(10, 0) NOT NULL DEFAULT 0,
    GiaGocSP DECIMAL(10, 0) NOT NULL DEFAULT 0,
    PhanLoaiSP NVARCHAR(50) NULL,
    NamSanXuatSP INT NULL,
    BaoHanhSP NVARCHAR(255) NULL,
    KichThuocSP NVARCHAR(255) NULL,
    KhoiLuongSP NVARCHAR(100) NULL,
    CongSuatTieuThu NVARCHAR(10) NULL,
    ChatLieuSP NVARCHAR(255) NULL,
    TienIchSP NVARCHAR(500) NULL,
    CongNgheSP NVARCHAR(500) NULL,
    MucGiamGiaSP DECIMAL(4,2) NOT NULL DEFAULT 0,
    NgayHetGiamGiaSP DATETIME2 NULL,
    NgayTaoSP DATETIME2 NOT NULL DEFAULT GETDATE(),
    NgayCapNhatSP DATETIME2 NOT NULL DEFAULT GETDATE(),
    TrangThaiSP INT NOT NULL DEFAULT 1,
    FOREIGN KEY (MaQG) REFERENCES QuocGia(MaQG) ON DELETE SET NULL,
    FOREIGN KEY (MaTH) REFERENCES ThuongHieu(MaTH) ON DELETE SET NULL,
    FOREIGN KEY (MaLSP) REFERENCES LoaiSanPham(MaLSP) ON DELETE SET NULL
);
GO

CREATE TABLE Anh (
    MaAnh INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL,
    UrlAnh NVARCHAR(100) NOT NULL UNIQUE,
    MacDinhAnh INT NOT NULL DEFAULT 0,
    TrangThaiAnh INT NOT NULL DEFAULT 1,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE TrangThaiDonHang (
    MaTTDH INT PRIMARY KEY,
    TenTTDH NVARCHAR(50) NOT NULL UNIQUE
);
GO

CREATE TABLE PhuongThucThanhToan (
    MaPTTT INT PRIMARY KEY,                     
    TenPTTT NVARCHAR(50) NOT NULL UNIQUE
);
GO

CREATE TABLE DonHang (
    MaDH INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT NOT NULL,
    SoTienDH DECIMAL(10,0) NOT NULL,
    PhiVanChuyenDH DECIMAL(10,0) NOT NULL,
    TongTienDH DECIMAL(10,0) NOT NULL,
    MaPTTT INT NOT NULL,
    MaTTDH INT NOT NULL,
    MaDCCT INT NOT NULL,
    NgayTaoDH DATETIME2 NOT NULL DEFAULT GETDATE(),
    NgayCapNhatDH DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaPTTT) REFERENCES PhuongThucThanhToan(MaPTTT),
    FOREIGN KEY (MaTTDH) REFERENCES TrangThaiDonHang(MaTTDH)
    FOREIGN KEY (MaDCCT) REFERENCES DiaChiCuThe(MaDCCT),
);
GO

CREATE TABLE TrangThaiThanhToan (
    MaTTTT INT PRIMARY KEY,
    TenTTTT NVARCHAR(50) NOT NULL UNIQUE
);
GO

CREATE TABLE ThanhToan (
    MaTT INT IDENTITY(1,1) PRIMARY KEY,
    MaDH INT NOT NULL,
    MaTTTT INT NOT NULL,
    MaGiaoDichTT NVARCHAR(30) NULL,
    NgayTaoTT DATETIME2 NOT NULL DEFAULT GETDATE(),
    NgayCapNhatTT DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH) ON DELETE CASCADE,
    FOREIGN KEY (MaTTTT) REFERENCES TrangThaiThanhToan(MaTTTT)
);
GO

CREATE TABLE BaoGom (
    MaSP INT NOT NULL,
    MaDH INT NOT NULL,
    SoLuongDat INT NOT NULL,
    GiaDat DECIMAL(10,0) NOT NULL,
    ThueGTGTDat DECIMAL(4,2) NOT NULL,
    MucGiamGiaDat DECIMAL(4,2) NOT NULL,
    PRIMARY KEY (MaSP, MaDH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE,
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH) ON DELETE CASCADE
);
GO

CREATE TABLE MayLanh (
    MaML INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL UNIQUE,
    CongSuatLamLanhML NVARCHAR(30) NULL,
    PhamViLamLanhML NVARCHAR(40) NULL,
    DoOnML NVARCHAR(50) NULL,
    LoaiGasML NVARCHAR(10) NULL,
    CheDoGioML NVARCHAR(60) NULL,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE TuLanh (
    MaTL INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL UNIQUE,
    DungTichNganDaTL NVARCHAR(10) NULL,
    DungTichNganLanhTL NVARCHAR(10) NULL,
    LayNuocNgoaiTL NVARCHAR(10) NULL,
    LayDaTuDongTL NVARCHAR(10) NULL,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE NoiChien (
    MaNC INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL UNIQUE,
    DungTichTongNC NVARCHAR(10) NULL,
    DungTichSuDungNC NVARCHAR(10) NULL,
    NhietDoNC NVARCHAR(20) NULL,
    HenGioNC NVARCHAR(20) NULL,
    BangDieuKhienNC NVARCHAR(50) NULL,
    ChieuDaiDayDienNC NVARCHAR(10) NULL,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE MayLocNuoc (
    MaMLN INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL UNIQUE,
    KieuLapMLN NVARCHAR(30) NULL,
    CongSuatLocMLN NVARCHAR(30) NULL,
    TiLeLocThaiMLN NVARCHAR(30) NULL,
    ChiSoNuocMLN NVARCHAR(100) NULL,
    DoPHThucTeMLN NVARCHAR(100) NULL,
    ApLucNuocYeuCauMLN NVARCHAR(20) NULL,
    SoLoiLocMLN INT NULL,
    BangDieuKhienMLN NVARCHAR(40) NULL,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE TiVi (
    MaTV INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL UNIQUE,
    CoManHinhTV NVARCHAR(10) NULL, 
    DoPhanGiaiTV NVARCHAR(10) NULL, 
    LoaiManHinhTV NVARCHAR(100) NULL, 
    TanSoQuetTV NVARCHAR(10) NULL, 
    DieuKhienTV NVARCHAR(100) NULL, 
    CongKetNoiTV NVARCHAR(100) NULL, 
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE NoiComDien (
    MaNCD INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL UNIQUE,
    DungTichNCD NVARCHAR(10) NULL,
    ChucNangNCD NVARCHAR(255) NULL,
    DoDayNCD NVARCHAR(50) NULL,
    DieuKhienNCD NVARCHAR(50) NULL,
    ChieuDaiDayDienNCD NVARCHAR(10) NULL,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE MayLocKhongKhi (
    MaMLKK INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL UNIQUE,
    LoaiBuiLocDuocMLKK NVARCHAR(100) NULL,
    PhamViLocMLKK NVARCHAR(10) NULL,
    LuongGioMLKK NVARCHAR(10) NULL,
    MangLocMLKK NVARCHAR(100) NULL,
    BangDieuKhienMLKK NVARCHAR(50) NULL,
    DoOnMLKK NVARCHAR(10) NULL,
    CamBienMLKK NVARCHAR(100) NULL,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE MayRuaChen (
    MaMRC INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL UNIQUE,
    NuocTieuThuMRC NVARCHAR(30) NULL,
    SoChenRuaDuocMRC NVARCHAR(30) NULL,
    DoOnMRC NVARCHAR(10) NULL,
    BangDieuKhienMRC NVARCHAR(50) NULL,
    ChieuDaiOngCapNuocMRC NVARCHAR(10) NULL,
    ChieuDaiOngThoatNuocMRC NVARCHAR(10) NULL,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE
);
GO

CREATE TABLE DanhGia (
    MaSP INT NOT NULL,
    MaKH INT NOT NULL,
    DiemDG INT NOT NULL,
    NhanXetDG NVARCHAR(255) NULL,
    NgayTaoDG DATETIME2 NOT NULL DEFAULT GETDATE(),
    NgayCapNhatDG DATETIME2 NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (MaSP, MaKH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH) ON DELETE CASCADE
);
GO

-- View
CREATE VIEW View_LayDanhSachSanPham
AS
SELECT 
    sp.MaSP,
    sp.MaLSP,
    sp.MaQG,
    sp.MaTH,
    sp.TenSP,
    sp.SoLuongSP,
    sp.GiaNhapSP,
    sp.GiaGocSP,
    sp.PhanLoaiSP,
    sp.NamSanXuatSP,
    sp.BaoHanhSP,
    sp.KichThuocSP,
    sp.KhoiLuongSP,
    sp.CongSuatTieuThuSP,
    sp.ChatLieuSP,
    sp.TienIchSP,
    sp.CongNgheSP,
    sp.NgayTaoSP,
    sp.NgayCapNhatSP,
    sp.TrangThaiSP,
    lsp.TenLSP,
    lsp.TrangThaiLSP,
    lsp.ThueGTGTLSP,
    sp.GiaGocSP * (1 + lsp.ThueGTGTLSP / 100) AS GiaGocSauThueSP,
    CASE 
        WHEN sp.NgayHetGiamGiaSP IS NOT NULL AND sp.NgayHetGiamGiaSP >= GETDATE()
        THEN sp.MucGiamGiaSP
        ELSE 0
    END AS MucGiamGiaSP,
    CASE 
        WHEN sp.NgayHetGiamGiaSP IS NOT NULL AND sp.NgayHetGiamGiaSP >= GETDATE()
        THEN sp.GiaGocSP * (1 - sp.MucGiamGiaSP / 100) * (1 + lsp.ThueGTGTLSP / 100)
        ELSE sp.GiaGocSP * (1 + lsp.ThueGTGTLSP / 100)
    END AS GiaSauGiamVaThueSP,
    sp.NgayHetGiamGiaSP,
    bg.SoLuongDaBanSP,
    dg.DiemTrungBinhSP,
    dg.SoLuotDGSP,
    asp.UrlAnh,
    qg.TenQG,
    CONCAT(th.TenTH, ' (', qg.TenQG, ')') AS TenTH
FROM SanPham sp
JOIN LoaiSanPham lsp ON sp.MaLSP = lsp.MaLSP
LEFT JOIN Anh asp 
    ON sp.MaSP = asp.MaSP 
    AND asp.MacDinhAnh = 1
LEFT JOIN (
    SELECT bg.MaSP, SUM(bg.SoLuongDat) AS SoLuongDaBanSP
    FROM BaoGom bg
    JOIN DonHang dh ON bg.MaDH = dh.MaDH
    WHERE dh.MaTTDH = 5
    GROUP BY bg.MaSP
) bg ON sp.MaSP = bg.MaSP
LEFT JOIN (
    SELECT MaSP, AVG(CAST(DiemDG AS DECIMAL(3,2))) AS DiemTrungBinhSP, COUNT(*) AS SoLuotDGSP
    FROM DanhGia
    GROUP BY MaSP
) dg ON sp.MaSP = dg.MaSP
LEFT JOIN ThuongHieu th ON sp.MaTH = th.MaTH
LEFT JOIN QuocGia qg ON sp.MaQG = qg.MaQG;
GO

-- PROC
CREATE PROCEDURE SP_TaoTaiKhoanKhachHang
(
    @TenKH NVARCHAR(50),
    @GioiTinhKH INT = NULL,
    @SoDienThoaiKH VARCHAR(10),
    @EmailKH VARCHAR(100) = NULL,
    @MaTTP CHAR(2),
    @TenTTP NVARCHAR(30),
    @MaXP CHAR(5),
    @TenXP NVARCHAR(40),
    @TenDCCT NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM TinhThanhPho WHERE MaTTP = @MaTTP)
        BEGIN
            INSERT INTO TinhThanhPho (MaTTP, TenTTP)
            VALUES (@MaTTP, @TenTTP);
        END

        IF NOT EXISTS (SELECT 1 FROM XaPhuong WHERE MaXP = @MaXP)
        BEGIN
            INSERT INTO XaPhuong (MaXP, MaTTP, TenXP)
            VALUES (@MaXP, @MaTTP, @TenXP);
        END

        INSERT INTO KhachHang
        (
            TenKH,
            GioiTinhKH,
            SoDienThoaiKH,
            EmailKH
        )
        VALUES
        (
            @TenKH,
            @GioiTinhKH,
            @SoDienThoaiKH,
            @EmailKH
        );

        DECLARE @MaKH INT = SCOPE_IDENTITY();

        INSERT INTO TaiKhoan
        (
            MaKH,
            TenTK,
            QuyenTK,
            TrangThaiTK
        )
        VALUES
        (
            @MaKH,
            @SoDienThoaiKH,
            0,
            1
        );

        INSERT INTO DiaChiCuThe
        (
            MaXP,
            MaKH,
            TenDCCT,
            MacDinhDCCT
        )
        VALUES
        (
            @MaXP,
            @MaKH,
            @TenDCCT,
            1
        );

        COMMIT TRANSACTION;

        RETURN 1; 
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        RETURN 0;
    END CATCH
END
GO

CREATE PROCEDURE SP_ThemDiaChiNhanHang
(
    @MaKH INT,
    @MaTTP CHAR(2),
    @TenTTP NVARCHAR(30),
    @MaXP CHAR(5),
    @TenXP NVARCHAR(40),
    @TenDCCT NVARCHAR(255),
    @MacDinhDCCT INT 
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM TinhThanhPho WHERE MaTTP = @MaTTP)
        BEGIN
            INSERT INTO TinhThanhPho (MaTTP, TenTTP)
            VALUES (@MaTTP, @TenTTP);
        END

        IF NOT EXISTS (SELECT 1 FROM XaPhuong WHERE MaXP = @MaXP)
        BEGIN
            INSERT INTO XaPhuong (MaXP, MaTTP, TenXP)
            VALUES (@MaXP, @MaTTP, @TenXP);
        END

        IF (@MacDinhDCCT = 1)
        BEGIN
            UPDATE DiaChiCuThe
            SET MacDinhDCCT = 0
            WHERE MaKH = @MaKH;
        END

        INSERT INTO DiaChiCuThe
        (
            MaXP,
            MaKH,
            TenDCCT,
            MacDinhDCCT
        )
        VALUES
        (
            @MaXP,
            @MaKH,
            @TenDCCT,
            @MacDinhDCCT
        );

        COMMIT TRANSACTION;
        RETURN 1;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        RETURN 0;
    END CATCH
END
GO

CREATE PROCEDURE SP_SuaDiaChiNhanHang
(
    @MaDCCT INT,
    @MaKH INT,
    @MaTTP CHAR(2),
    @TenTTP NVARCHAR(30),
    @MaXP CHAR(5),
    @TenXP NVARCHAR(40),
    @TenDCCT NVARCHAR(255),
    @MacDinhDCCT INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM TinhThanhPho WHERE MaTTP = @MaTTP)
        BEGIN
            INSERT INTO TinhThanhPho (MaTTP, TenTTP)
            VALUES (@MaTTP, @TenTTP);
        END

        IF NOT EXISTS (SELECT 1 FROM XaPhuong WHERE MaXP = @MaXP)
        BEGIN
            INSERT INTO XaPhuong (MaXP, MaTTP, TenXP)
            VALUES (@MaXP, @MaTTP, @TenXP);
        END

        IF (@MacDinhDCCT = 1)
        BEGIN
            UPDATE DiaChiCuThe
            SET MacDinhDCCT = 0
            WHERE MaKH = @MaKH;
        END

        UPDATE DiaChiCuThe
        SET
            MaXP = @MaXP,
            TenDCCT = @TenDCCT,
            MacDinhDCCT = @MacDinhDCCT
        WHERE MaDCCT = @MaDCCT
          AND MaKH = @MaKH;

        COMMIT TRANSACTION;
        RETURN 1;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        RETURN 0;
    END CATCH
END
GO

CREATE TYPE TVP_DanhSachDatHang AS TABLE
(
    MaSP INT NOT NULL,
    SoLuongDat INT NOT NULL
);
GO

CREATE PROCEDURE Proc_DatHang
    @MaKH INT,
    @MaDCCT INT,
    @PhiVanChuyenDH DECIMAL(10,0) = 0,
    @DanhSachSP TVP_DanhSachDatHang READONLY,
    @MaTTTT INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @MaKH IS NULL
        RETURN 0;

    BEGIN TRAN;
    BEGIN TRY
        IF EXISTS (
            SELECT 1
            FROM SanPham sp WITH (UPDLOCK, HOLDLOCK)
            JOIN @DanhSachSP ds ON sp.MaSP = ds.MaSP
            WHERE sp.SoLuongSP < ds.SoLuongDat
        )
        BEGIN
            ROLLBACK;
            RETURN 0;
        END

        UPDATE sp
        SET sp.SoLuongSP -= ds.SoLuongDat,
            sp.NgayCapNhatSP = GETDATE()
        FROM SanPham sp
        JOIN @DanhSachSP ds ON sp.MaSP = ds.MaSP;

        DECLARE @MaDH INT;
        DECLARE @SoTienHang DECIMAL(10,0);

        SELECT @SoTienHang =
            SUM(
                ds.SoLuongDat
                * sp.GiaGocSP
                * (1 + ISNULL(lsp.ThueGTGTLSP,0)/100.0)
                * (1 - CASE 
                        WHEN sp.NgayHetGiamGiaSP IS NOT NULL 
                             AND sp.NgayHetGiamGiaSP >= GETDATE()
                        THEN ISNULL(sp.MucGiamGiaSP,0)/100.0
                        ELSE 0
                      END)
            )
        FROM @DanhSachSP ds
        JOIN SanPham sp ON ds.MaSP = sp.MaSP
        LEFT JOIN LoaiSanPham lsp ON sp.MaLSP = lsp.MaLSP;

        INSERT INTO DonHang
            (MaKH, SoTienDH, PhiVanChuyenDH, TongTienDH, MaPTTT, MaTTDH, MaDCCT)
        VALUES
            (@MaKH, @SoTienHang, ISNULL(@PhiVanChuyenDH,0), @SoTienHang + ISNULL(@PhiVanChuyenDH,0), 1, 1, @MaDCCT);

        SET @MaDH = SCOPE_IDENTITY();

        INSERT INTO BaoGom
            (MaSP, MaDH, SoLuongDat, GiaDat, ThueGTGTDat, MucGiamGiaDat)
        SELECT
            ds.MaSP,
            @MaDH,
            ds.SoLuongDat,
            sp.GiaGocSP,
            ISNULL(lsp.ThueGTGTLSP,0),
            CASE 
                WHEN sp.NgayHetGiamGiaSP IS NOT NULL 
                     AND sp.NgayHetGiamGiaSP >= GETDATE()
                THEN ISNULL(sp.MucGiamGiaSP,0)
                ELSE 0
            END
        FROM @DanhSachSP ds
        JOIN SanPham sp ON ds.MaSP = sp.MaSP
        LEFT JOIN LoaiSanPham lsp ON sp.MaLSP = lsp.MaLSP;

        INSERT INTO ThanhToan (MaDH, MaTTTT)
        VALUES (@MaDH, ISNULL(@MaTTTT,1));

        COMMIT;

        RETURN 1; 
    END TRY
    BEGIN CATCH
        ROLLBACK;
        RETURN 0;
    END CATCH
END;
GO