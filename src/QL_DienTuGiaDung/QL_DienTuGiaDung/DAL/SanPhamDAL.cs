using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class SanPhamDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public SanPhamDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public DataTable LayDanhSachSanPham(int quyen, int maloai, string? tukhoa = null)
        {
            string sql = "SELECT * FROM View_LayDanhSachSanPham WHERE 1 = 1";
            var parameters = new List<SqlParameter>();

            if (quyen == 0)
            {
                sql += " AND TrangThaiSP = 1 AND TrangThaiLSP = 1 AND ThueGTGTLSP > 0";
            }

            if (maloai > 0)
            {
                sql += " AND MaLSP = @maloai";
                parameters.Add(new SqlParameter("@maloai", maloai));
            }

            if (!string.IsNullOrEmpty(tukhoa))
            {
                sql += " AND TenSP COLLATE Vietnamese_CI_AI LIKE '%' + @tukhoa + '%'";
                parameters.Add(new SqlParameter("@tukhoa", tukhoa));
            }

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters.ToArray());
        }

        public DataTable LayDanhSachSanPhamBangListMa(List<int> listMaSP)
        {
            if (!listMaSP.Any()) return new DataTable();

            string sql = @$"SELECT * FROM View_LayDanhSachSanPham WHERE MaSP IN ({string.Join(",", listMaSP)}) 
            AND TrangThaiSP = 1 AND TrangThaiLSP = 1 AND ThueGTGTLSP > 0 AND SoLuongSP > 0";
            
            return _databaseHelper.ExecuteDataTable(sql);
        }

        public DataTable LaySanPham(int quyen, int masp)
        {
            string sql = "SELECT * FROM View_LayDanhSachSanPham WHERE MaSP = @masp";

            if (quyen == 0)
            {
                sql += " AND TrangThaiSP = 1 AND TrangThaiLSP = 1 AND ThueGTGTLSP > 0";
            }

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayDanhSachSanPhamTrongDonHang(int maDH)
        {
            string sql = 
                @"SELECT
                bg.MaSP,
                bg.SoLuongDat,
                sp.TenSP,
                bg.GiaDat * (1 - bg.MucGiamGiaDat / 100) * (1 + bg.ThueGTGTDat / 100) AS GiaDatSauGiamVaThue,
                asp.UrlAnh
                FROM BaoGom bg
                JOIN SanPham sp ON sp.MaSP = bg.MaSP
                LEFT JOIN Anh asp 
                ON asp.MaSP = sp.MaSP AND asp.MacDinhAnh = 1
                WHERE bg.MaDH = @maDH";

            var parameters = new[]
            {
                new SqlParameter("@maDH", maDH)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayMayLanh(int masp)
        {
            string sql = "SELECT * FROM MayLanh WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayTuLanh(int masp)
        {
            string sql = "SELECT * FROM TuLanh WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayMayLocKhongKhi(int masp)
        {
            string sql = "SELECT * FROM MayLocKhongKhi WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayMayLocNuoc(int masp)
        {
            string sql = "SELECT * FROM MayLocNuoc WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayMayRuaChen(int masp)
        {
            string sql = "SELECT * FROM MayRuaChen WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayNoiComDien(int masp)
        {
            string sql = "SELECT * FROM NoiComDien WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayNoiChien(int masp)
        {
            string sql = "SELECT * FROM NoiChien WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayTiVi(int masp)
        {
            string sql = "SELECT * FROM TiVi WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public bool KiemTraTenSanPhamTonTai(string tenSP, int maSP = 0)
        {
            string sql = "SELECT COUNT(*) FROM SanPham WHERE TenSP = @TenSP";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TenSP", tenSP)
            };

            if (maSP > 0)
            {
                sql += " AND MaSP != @MaSP";
                parameters.Add(new SqlParameter("@MaSP", maSP));
            }

            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters.ToArray());
            return Convert.ToInt32(result) > 0;
        }

        public int ThemSanPham(SanPham sanPham)
        {
            string sql = @"INSERT INTO SanPham (MaQG, MaTH, MaLSP, TenSP, SoLuongSP, GiaNhapSP, GiaGocSP, 
                          PhanLoaiSP, NamSanXuatSP, BaoHanhSP, KichThuocSP, KhoiLuongSP, CongSuatTieuThuSP, 
                          ChatLieuSP, TienIchSP, CongNgheSP, MucGiamGiaSP, NgayHetGiamGiaSP, TrangThaiSP, NgayTaoSP, NgayCapNhatSP)
                          VALUES (@MaQG, @MaTH, @MaLSP, @TenSP, @SoLuongSP, @GiaNhapSP, @GiaGocSP, 
                          @PhanLoaiSP, @NamSanXuatSP, @BaoHanhSP, @KichThuocSP, @KhoiLuongSP, @CongSuatTieuThuSP, 
                          @ChatLieuSP, @TienIchSP, @CongNgheSP, @MucGiamGiaSP, @NgayHetGiamGiaSP, @TrangThaiSP, GETDATE(), GETDATE());
                          SELECT SCOPE_IDENTITY();";

            var parameters = new[]
            {
                new SqlParameter("@MaQG", sanPham.MaQG),
                new SqlParameter("@MaTH", sanPham.MaTH),
                new SqlParameter("@MaLSP", sanPham.MaLSP),
                new SqlParameter("@TenSP", sanPham.TenSP),
                new SqlParameter("@SoLuongSP", sanPham.SoLuongSP),
                new SqlParameter("@GiaNhapSP", sanPham.GiaNhapSP),
                new SqlParameter("@GiaGocSP", sanPham.GiaGocSP),
                new SqlParameter("@PhanLoaiSP", sanPham.PhanLoaiSP ?? (object)DBNull.Value),
                new SqlParameter("@NamSanXuatSP", sanPham.NamSanXuatSP),
                new SqlParameter("@BaoHanhSP", sanPham.BaoHanhSP ?? (object)DBNull.Value),
                new SqlParameter("@KichThuocSP", sanPham.KichThuocSP ?? (object)DBNull.Value),
                new SqlParameter("@KhoiLuongSP", sanPham.KhoiLuongSP ?? (object)DBNull.Value),
                new SqlParameter("@CongSuatTieuThuSP", sanPham.CongSuatTieuThuSP ?? (object)DBNull.Value),
                new SqlParameter("@ChatLieuSP", sanPham.ChatLieuSP ?? (object)DBNull.Value),
                new SqlParameter("@TienIchSP", sanPham.TienIchSP ?? (object)DBNull.Value),
                new SqlParameter("@CongNgheSP", sanPham.CongNgheSP ?? (object)DBNull.Value),
                new SqlParameter("@MucGiamGiaSP", sanPham.MucGiamGiaSP ?? 0),
                new SqlParameter("@NgayHetGiamGiaSP", sanPham.NgayHetGiamGiaSP ?? (object)DBNull.Value),
                new SqlParameter("@TrangThaiSP", sanPham.MaSP == 0 ? 1 : sanPham.TrangThaiSP)
            };

            try
            {
                var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int CapNhatSanPham(SanPham sanPham)
        {
            string sql = @"UPDATE SanPham SET MaQG = @MaQG, MaTH = @MaTH, MaLSP = @MaLSP, TenSP = @TenSP, 
                          SoLuongSP = @SoLuongSP, GiaNhapSP = @GiaNhapSP, GiaGocSP = @GiaGocSP, 
                          PhanLoaiSP = @PhanLoaiSP, NamSanXuatSP = @NamSanXuatSP, BaoHanhSP = @BaoHanhSP, 
                          KichThuocSP = @KichThuocSP, KhoiLuongSP = @KhoiLuongSP, CongSuatTieuThuSP = @CongSuatTieuThuSP, 
                          ChatLieuSP = @ChatLieuSP, TienIchSP = @TienIchSP, CongNgheSP = @CongNgheSP, 
                          MucGiamGiaSP = @MucGiamGiaSP, NgayHetGiamGiaSP = @NgayHetGiamGiaSP, 
                          TrangThaiSP = @TrangThaiSP, NgayCapNhatSP = GETDATE()
                          WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaQG", sanPham.MaQG),
                new SqlParameter("@MaTH", sanPham.MaTH),
                new SqlParameter("@MaLSP", sanPham.MaLSP),
                new SqlParameter("@TenSP", sanPham.TenSP),
                new SqlParameter("@SoLuongSP", sanPham.SoLuongSP),
                new SqlParameter("@GiaNhapSP", sanPham.GiaNhapSP),
                new SqlParameter("@GiaGocSP", sanPham.GiaGocSP),
                new SqlParameter("@PhanLoaiSP", sanPham.PhanLoaiSP),
                new SqlParameter("@NamSanXuatSP", sanPham.NamSanXuatSP),
                new SqlParameter("@BaoHanhSP", sanPham.BaoHanhSP),
                new SqlParameter("@KichThuocSP", sanPham.KichThuocSP),
                new SqlParameter("@KhoiLuongSP", sanPham.KhoiLuongSP),
                new SqlParameter("@CongSuatTieuThuSP", sanPham.CongSuatTieuThuSP),
                new SqlParameter("@ChatLieuSP", sanPham.ChatLieuSP),
                new SqlParameter("@TienIchSP", sanPham.TienIchSP),
                new SqlParameter("@CongNgheSP", sanPham.CongNgheSP),
                new SqlParameter("@MucGiamGiaSP", sanPham.MucGiamGiaSP ?? 0),
                new SqlParameter("@NgayHetGiamGiaSP", sanPham.NgayHetGiamGiaSP),
                new SqlParameter("@TrangThaiSP", sanPham.TrangThaiSP),
                new SqlParameter("@MaSP", sanPham.MaSP)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public bool KiemTraCoTheSuaXoaSanPham(int maSP)
        {
            string sql = "SELECT COUNT(*) FROM BaoGom WHERE MaSP = @MaSP";
            var parameters = new[] { new SqlParameter("@MaSP", maSP) };
            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters);
            return Convert.ToInt32(result) == 0;
        }

        public int XoaSanPham(int maSP)
        {
            string sql = "DELETE FROM SanPham WHERE MaSP = @MaSP";
            var parameters = new[] { new SqlParameter("@MaSP", maSP) };
            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemMayLanh(MayLanh mayLanh)
        {
            string sql = @"INSERT INTO MayLanh (MaSP, CongSuatLamLanhML, PhamViLamLanhML, DoOnML, LoaiGasML, CheDoGioML)
                          VALUES (@MaSP, @CongSuatLamLanhML, @PhamViLamLanhML, @DoOnML, @LoaiGasML, @CheDoGioML)";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", mayLanh.MaSP),
                new SqlParameter("@CongSuatLamLanhML", mayLanh.CongSuatLamLanhML),
                new SqlParameter("@PhamViLamLanhML", mayLanh.PhamViLamLanhML),
                new SqlParameter("@DoOnML", mayLanh.DoOnML),
                new SqlParameter("@LoaiGasML", mayLanh.LoaiGasML),
                new SqlParameter("@CheDoGioML", mayLanh.CheDoGioML)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatMayLanh(MayLanh mayLanh)
        {
            string sql = @"UPDATE MayLanh SET CongSuatLamLanhML = @CongSuatLamLanhML, PhamViLamLanhML = @PhamViLamLanhML, 
                          DoOnML = @DoOnML, LoaiGasML = @LoaiGasML, CheDoGioML = @CheDoGioML
                          WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", mayLanh.MaSP),
                new SqlParameter("@CongSuatLamLanhML", mayLanh.CongSuatLamLanhML),
                new SqlParameter("@PhamViLamLanhML", mayLanh.PhamViLamLanhML),
                new SqlParameter("@DoOnML", mayLanh.DoOnML),
                new SqlParameter("@LoaiGasML", mayLanh.LoaiGasML),
                new SqlParameter("@CheDoGioML", mayLanh.CheDoGioML)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemTuLanh(TuLanh tuLanh)
        {
            string sql = @"INSERT INTO TuLanh (MaSP, DungTichNganDaTL, DungTichNganLanhTL, LayNuocNgoaiTL, LayDaTuDongTL)
                          VALUES (@MaSP, @DungTichNganDaTL, @DungTichNganLanhTL, @LayNuocNgoaiTL, @LayDaTuDongTL)";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", tuLanh.MaSP),
                new SqlParameter("@DungTichNganDaTL", tuLanh.DungTichNganDaTL),
                new SqlParameter("@DungTichNganLanhTL", tuLanh.DungTichNganLanhTL),
                new SqlParameter("@LayNuocNgoaiTL", tuLanh.LayNuocNgoaiTL),
                new SqlParameter("@LayDaTuDongTL", tuLanh.LayDaTuDongTL)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatTuLanh(TuLanh tuLanh)
        {
            string sql = @"UPDATE TuLanh SET DungTichNganDaTL = @DungTichNganDaTL, DungTichNganLanhTL = @DungTichNganLanhTL, 
                          LayNuocNgoaiTL = @LayNuocNgoaiTL, LayDaTuDongTL = @LayDaTuDongTL
                          WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", tuLanh.MaSP),
                new SqlParameter("@DungTichNganDaTL", tuLanh.DungTichNganDaTL),
                new SqlParameter("@DungTichNganLanhTL", tuLanh.DungTichNganLanhTL),
                new SqlParameter("@LayNuocNgoaiTL", tuLanh.LayNuocNgoaiTL),
                new SqlParameter("@LayDaTuDongTL", tuLanh.LayDaTuDongTL)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemTiVi(TiVi tiVi)
        {
            string sql = @"INSERT INTO TiVi (MaSP, CoManHinhTV, DoPhanGiaiTV, LoaiManHinhTV, TanSoQuetTV, DieuKhienTV, CongKetNoiTV)
                          VALUES (@MaSP, @CoManHinhTV, @DoPhanGiaiTV, @LoaiManHinhTV, @TanSoQuetTV, @DieuKhienTV, @CongKetNoiTV)";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", tiVi.MaSP),
                new SqlParameter("@CoManHinhTV", tiVi.CoManHinhTV),
                new SqlParameter("@DoPhanGiaiTV", tiVi.DoPhanGiaiTV),
                new SqlParameter("@LoaiManHinhTV", tiVi.LoaiManHinhTV),
                new SqlParameter("@TanSoQuetTV", tiVi.TanSoQuetTV),
                new SqlParameter("@DieuKhienTV", tiVi.DieuKhienTV),
                new SqlParameter("@CongKetNoiTV", tiVi.CongKetNoiTV)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatTiVi(TiVi tiVi)
        {
            string sql = @"UPDATE TiVi SET CoManHinhTV = @CoManHinhTV, DoPhanGiaiTV = @DoPhanGiaiTV, 
                          LoaiManHinhTV = @LoaiManHinhTV, TanSoQuetTV = @TanSoQuetTV, DieuKhienTV = @DieuKhienTV, 
                          CongKetNoiTV = @CongKetNoiTV WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", tiVi.MaSP),
                new SqlParameter("@CoManHinhTV", tiVi.CoManHinhTV),
                new SqlParameter("@DoPhanGiaiTV", tiVi.DoPhanGiaiTV),
                new SqlParameter("@LoaiManHinhTV", tiVi.LoaiManHinhTV),
                new SqlParameter("@TanSoQuetTV", tiVi.TanSoQuetTV),
                new SqlParameter("@DieuKhienTV", tiVi.DieuKhienTV),
                new SqlParameter("@CongKetNoiTV", tiVi.CongKetNoiTV)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemMayLocKhongKhi(MayLocKhongKhi mayLocKhongKhi)
        {
            string sql = @"INSERT INTO MayLocKhongKhi (MaSP, LoaiBuiLocDuocMLKK, PhamViLocMLKK, LuongGioMLKK, MangLocMLKK, BangDieuKhienMLKK, DoOnMLKK, CamBienMLKK)
                          VALUES (@MaSP, @LoaiBuiLocDuocMLKK, @PhamViLocMLKK, @LuongGioMLKK, @MangLocMLKK, @BangDieuKhienMLKK, @DoOnMLKK, @CamBienMLKK)";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", mayLocKhongKhi.MaSP),
                new SqlParameter("@LoaiBuiLocDuocMLKK", mayLocKhongKhi.LoaiBuiLocDuocMLKK),
                new SqlParameter("@PhamViLocMLKK", mayLocKhongKhi.PhamViLocMLKK),
                new SqlParameter("@LuongGioMLKK", mayLocKhongKhi.LuongGioMLKK),
                new SqlParameter("@MangLocMLKK", mayLocKhongKhi.MangLocMLKK),
                new SqlParameter("@BangDieuKhienMLKK", mayLocKhongKhi.BangDieuKhienMLKK),
                new SqlParameter("@DoOnMLKK", mayLocKhongKhi.DoOnMLKK),
                new SqlParameter("@CamBienMLKK", mayLocKhongKhi.CamBienMLKK)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatMayLocKhongKhi(MayLocKhongKhi mayLocKhongKhi)
        {
            string sql = @"UPDATE MayLocKhongKhi SET LoaiBuiLocDuocMLKK = @LoaiBuiLocDuocMLKK, PhamViLocMLKK = @PhamViLocMLKK, 
                          LuongGioMLKK = @LuongGioMLKK, MangLocMLKK = @MangLocMLKK, BangDieuKhienMLKK = @BangDieuKhienMLKK, 
                          DoOnMLKK = @DoOnMLKK, CamBienMLKK = @CamBienMLKK
                          WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", mayLocKhongKhi.MaSP),
                new SqlParameter("@LoaiBuiLocDuocMLKK", mayLocKhongKhi.LoaiBuiLocDuocMLKK),
                new SqlParameter("@PhamViLocMLKK", mayLocKhongKhi.PhamViLocMLKK),
                new SqlParameter("@LuongGioMLKK", mayLocKhongKhi.LuongGioMLKK),
                new SqlParameter("@MangLocMLKK", mayLocKhongKhi.MangLocMLKK),
                new SqlParameter("@BangDieuKhienMLKK", mayLocKhongKhi.BangDieuKhienMLKK),
                new SqlParameter("@DoOnMLKK", mayLocKhongKhi.DoOnMLKK),
                new SqlParameter("@CamBienMLKK", mayLocKhongKhi.CamBienMLKK)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemMayLocNuoc(MayLocNuoc mayLocNuoc)
        {
            string sql = @"INSERT INTO MayLocNuoc (MaSP, KieuLapMLN, CongSuatLocMLN, TiLeLocThaiMLN, ChiSoNuocMLN, DoPHThucTeMLN, ApLucNuocYeuCauMLN, SoLoiLocMLN, BangDieuKhienMLN)
                          VALUES (@MaSP, @KieuLapMLN, @CongSuatLocMLN, @TiLeLocThaiMLN, @ChiSoNuocMLN, @DoPHThucTeMLN, @ApLucNuocYeuCauMLN, @SoLoiLocMLN, @BangDieuKhienMLN)";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", mayLocNuoc.MaSP),
                new SqlParameter("@KieuLapMLN", mayLocNuoc.KieuLapMLN),
                new SqlParameter("@CongSuatLocMLN", mayLocNuoc.CongSuatLocMLN),
                new SqlParameter("@TiLeLocThaiMLN", mayLocNuoc.TiLeLocThaiMLN),
                new SqlParameter("@ChiSoNuocMLN", mayLocNuoc.ChiSoNuocMLN),
                new SqlParameter("@DoPHThucTeMLN", mayLocNuoc.DoPHThucTeMLN),
                new SqlParameter("@ApLucNuocYeuCauMLN", mayLocNuoc.ApLucNuocYeuCauMLN),
                new SqlParameter("@SoLoiLocMLN", mayLocNuoc.SoLoiLocMLN),
                new SqlParameter("@BangDieuKhienMLN", mayLocNuoc.BangDieuKhienMLN)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatMayLocNuoc(MayLocNuoc mayLocNuoc)
        {
            string sql = @"UPDATE MayLocNuoc SET KieuLapMLN = @KieuLapMLN, CongSuatLocMLN = @CongSuatLocMLN, 
                          TiLeLocThaiMLN = @TiLeLocThaiMLN, ChiSoNuocMLN = @ChiSoNuocMLN, DoPHThucTeMLN = @DoPHThucTeMLN, 
                          ApLucNuocYeuCauMLN = @ApLucNuocYeuCauMLN, SoLoiLocMLN = @SoLoiLocMLN, BangDieuKhienMLN = @BangDieuKhienMLN
                          WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", mayLocNuoc.MaSP),
                new SqlParameter("@KieuLapMLN", mayLocNuoc.KieuLapMLN),
                new SqlParameter("@CongSuatLocMLN", mayLocNuoc.CongSuatLocMLN),
                new SqlParameter("@TiLeLocThaiMLN", mayLocNuoc.TiLeLocThaiMLN),
                new SqlParameter("@ChiSoNuocMLN", mayLocNuoc.ChiSoNuocMLN),
                new SqlParameter("@DoPHThucTeMLN", mayLocNuoc.DoPHThucTeMLN),
                new SqlParameter("@ApLucNuocYeuCauMLN", mayLocNuoc.ApLucNuocYeuCauMLN),
                new SqlParameter("@SoLoiLocMLN", mayLocNuoc.SoLoiLocMLN),
                new SqlParameter("@BangDieuKhienMLN", mayLocNuoc.BangDieuKhienMLN)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemMayRuaChen(MayRuaChen mayRuaChen)
        {
            string sql = @"INSERT INTO MayRuaChen (MaSP, NuocTieuThuMRC, SoChenRuaDuocMRC, DoOnMRC, BangDieuKhienMRC, ChieuDaiOngCapNuocMRC, ChieuDaiOngThoatNuocMRC)
                          VALUES (@MaSP, @NuocTieuThuMRC, @SoChenRuaDuocMRC, @DoOnMRC, @BangDieuKhienMRC, @ChieuDaiOngCapNuocMRC, @ChieuDaiOngThoatNuocMRC)";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", mayRuaChen.MaSP),
                new SqlParameter("@NuocTieuThuMRC", mayRuaChen.NuocTieuThuMRC),
                new SqlParameter("@SoChenRuaDuocMRC", mayRuaChen.SoChenRuaDuocMRC),
                new SqlParameter("@DoOnMRC", mayRuaChen.DoOnMRC),
                new SqlParameter("@BangDieuKhienMRC", mayRuaChen.BangDieuKhienMRC),
                new SqlParameter("@ChieuDaiOngCapNuocMRC", mayRuaChen.ChieuDaiOngCapNuocMRC),
                new SqlParameter("@ChieuDaiOngThoatNuocMRC", mayRuaChen.ChieuDaiOngThoatNuocMRC)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatMayRuaChen(MayRuaChen mayRuaChen)
        {
            string sql = @"UPDATE MayRuaChen SET NuocTieuThuMRC = @NuocTieuThuMRC, SoChenRuaDuocMRC = @SoChenRuaDuocMRC, 
                          DoOnMRC = @DoOnMRC, BangDieuKhienMRC = @BangDieuKhienMRC, ChieuDaiOngCapNuocMRC = @ChieuDaiOngCapNuocMRC, 
                          ChieuDaiOngThoatNuocMRC = @ChieuDaiOngThoatNuocMRC
                          WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", mayRuaChen.MaSP),
                new SqlParameter("@NuocTieuThuMRC", mayRuaChen.NuocTieuThuMRC),
                new SqlParameter("@SoChenRuaDuocMRC", mayRuaChen.SoChenRuaDuocMRC),
                new SqlParameter("@DoOnMRC", mayRuaChen.DoOnMRC),
                new SqlParameter("@BangDieuKhienMRC", mayRuaChen.BangDieuKhienMRC),
                new SqlParameter("@ChieuDaiOngCapNuocMRC", mayRuaChen.ChieuDaiOngCapNuocMRC),
                new SqlParameter("@ChieuDaiOngThoatNuocMRC", mayRuaChen.ChieuDaiOngThoatNuocMRC)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemNoiComDien(NoiComDien noiComDien)
        {
            string sql = @"INSERT INTO NoiComDien (MaSP, DungTichNCD, ChucNangNCD, DoDayNCD, DieuKhienNCD, ChieuDaiDayDienNCD)
                          VALUES (@MaSP, @DungTichNCD, @ChucNangNCD, @DoDayNCD, @DieuKhienNCD, @ChieuDaiDayDienNCD)";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", noiComDien.MaSP),
                new SqlParameter("@DungTichNCD", noiComDien.DungTichNCD),
                new SqlParameter("@ChucNangNCD", noiComDien.ChucNangNCD),
                new SqlParameter("@DoDayNCD", noiComDien.DoDayNCD),
                new SqlParameter("@DieuKhienNCD", noiComDien.DieuKhienNCD),
                new SqlParameter("@ChieuDaiDayDienNCD", noiComDien.ChieuDaiDayDienNCD)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatNoiComDien(NoiComDien noiComDien)
        {
            string sql = @"UPDATE NoiComDien SET DungTichNCD = @DungTichNCD, ChucNangNCD = @ChucNangNCD, 
                          DoDayNCD = @DoDayNCD, DieuKhienNCD = @DieuKhienNCD, ChieuDaiDayDienNCD = @ChieuDaiDayDienNCD
                          WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", noiComDien.MaSP),
                new SqlParameter("@DungTichNCD", noiComDien.DungTichNCD),
                new SqlParameter("@ChucNangNCD", noiComDien.ChucNangNCD),
                new SqlParameter("@DoDayNCD", noiComDien.DoDayNCD),
                new SqlParameter("@DieuKhienNCD", noiComDien.DieuKhienNCD),
                new SqlParameter("@ChieuDaiDayDienNCD", noiComDien.ChieuDaiDayDienNCD)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemNoiChien(NoiChien noiChien)
        {
            string sql = @"INSERT INTO NoiChien (MaSP, DungTichTongNC, DungTichSuDungNC, NhietDoNC, HenGioNC, BangDieuKhienNC, ChieuDaiDayDienNC)
                          VALUES (@MaSP, @DungTichTongNC, @DungTichSuDungNC, @NhietDoNC, @HenGioNC, @BangDieuKhienNC, @ChieuDaiDayDienNC)";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", noiChien.MaSP),
                new SqlParameter("@DungTichTongNC", noiChien.DungTichTongNC),
                new SqlParameter("@DungTichSuDungNC", noiChien.DungTichSuDungNC),
                new SqlParameter("@NhietDoNC", noiChien.NhietDoNC),
                new SqlParameter("@HenGioNC", noiChien.HenGioNC),
                new SqlParameter("@BangDieuKhienNC", noiChien.BangDieuKhienNC),
                new SqlParameter("@ChieuDaiDayDienNC", noiChien.ChieuDaiDayDienNC)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatNoiChien(NoiChien noiChien)
        {
            string sql = @"UPDATE NoiChien SET DungTichTongNC = @DungTichTongNC, DungTichSuDungNC = @DungTichSuDungNC, 
                          NhietDoNC = @NhietDoNC, HenGioNC = @HenGioNC, BangDieuKhienNC = @BangDieuKhienNC, 
                          ChieuDaiDayDienNC = @ChieuDaiDayDienNC
                          WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", noiChien.MaSP),
                new SqlParameter("@DungTichTongNC", noiChien.DungTichTongNC),
                new SqlParameter("@DungTichSuDungNC", noiChien.DungTichSuDungNC),
                new SqlParameter("@NhietDoNC", noiChien.NhietDoNC),
                new SqlParameter("@HenGioNC", noiChien.HenGioNC),
                new SqlParameter("@BangDieuKhienNC", noiChien.BangDieuKhienNC),
                new SqlParameter("@ChieuDaiDayDienNC", noiChien.ChieuDaiDayDienNC)
            };

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}