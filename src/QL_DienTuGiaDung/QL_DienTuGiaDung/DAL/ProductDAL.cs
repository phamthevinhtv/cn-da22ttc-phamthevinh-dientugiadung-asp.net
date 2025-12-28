using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class ProductDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public ProductDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<SanPhamKhachHang> GetAllProductsForCustomer(int maLSP)
        {
            string sql = "SELECT * FROM View_LayTatCaSanPhamKhachHang";
            SqlParameter[]? parameters = null;

            if (maLSP > 0)
            {
                sql += " WHERE MaLSP = @MaLSP";
                parameters = new[] { new SqlParameter("@MaLSP", maLSP) };
            }

            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);

            var products = DataHelper.MapToList<SanPhamKhachHang>(datas);

            return products;
        }

        public List<SanPham> GetAllProductsOrigin(int maLSP)
        {
            string sql = @"
            SELECT 
            sp.MaSP, sp.MaQG, sp.MaTH, sp.MaLSP, sp.TenSP,
            sp.SoLuongSP, sp.GiaNhapSP, sp.GiaGocSP, sp.PhanLoaiSP,
            sp.NamSanXuatSP, sp.BaoHanhSP, sp.KichThuocSP, sp.KhoiLuongSP,
            sp.CongSuatTieuThu, sp.ChatLieuSP, sp.TienIchSP, sp.CongNgheSP,
            sp.MucGiamGiaSP, sp.NgayHetGiamGiaSP, sp.NgayTaoSP, sp.NgayCapNhatSP, sp.TrangThaiSP,
            lsp.TenLSP,
            asp.UrlAnh 
            FROM SanPham sp 
            JOIN LoaiSanPham lsp ON sp.MaLSP = lsp.MaLSP
            LEFT JOIN Anh asp ON sp.MaSP = asp.MaSP
            AND asp.MacDinhAnh = 1";
            
            SqlParameter[]? parameters = null;

            if (maLSP > 0)
            {
                sql += " WHERE sp.MaLSP = @MaLSP";
                parameters = new[] { new SqlParameter("@MaLSP", maLSP) };
            }

            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);

            var products = DataHelper.MapToList<SanPham>(datas);

            return products;
        }

        public List<SanPhamKhachHang> GetAllProductsByNameForCustomer(string? tenSP)
        {
            string sql = @"
                SELECT *
                FROM View_LayTatCaSanPhamKhachHang
                WHERE TenSP COLLATE Vietnamese_CI_AI LIKE '%' + @TenSP + '%'
            ";

            SqlParameter[] parameters =
            {
                new SqlParameter("@TenSP", tenSP ?? string.Empty)
            };

            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);

            return DataHelper.MapToList<SanPhamKhachHang>(datas);
        }

        public List<SanPhamKhachHang> GetByProductIdsForCustomer(List<int> ids)
        {
            if (!ids.Any()) return new();

            string sql = $"SELECT * FROM View_LayTatCaSanPhamKhachHang WHERE MaSP IN ({string.Join(",", ids)}) AND SoLuongSP > 0";
            
            SqlParameter[]? parameters = null;

            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);

            var products = DataHelper.MapToList<SanPhamKhachHang>(datas);

            return products;
        }

        public SanPhamKhachHang? GetByProductIdForCustomer(int? id)
        {
            string sql = $"SELECT * FROM View_LayTatCaSanPhamKhachHang WHERE MaSP = @productId";
            
            SqlParameter[]? parameters = new[] { new SqlParameter("@productId", id) };

            var data = _databaseHelper.ExecuteQuery(sql, parameters: parameters);

            var product = DataHelper.MapToList<SanPhamKhachHang>(data).FirstOrDefault();

            return product;
        }

        public List<LoaiSanPham> GetAllProductTypesForCustomer()
        {
            string sql = "SELECT MaLSP, TenLSP FROM LoaiSanPham WHERE ThueGTGTLSP > 0 AND TrangThaiLSP = 1";

            var datas = _databaseHelper.ExecuteQuery(sql);

            var productTypes = DataHelper.MapToList<LoaiSanPham>(datas);

            return productTypes;
        }

        public ChiTietSanPhamKhachHang? GetProductDetailForCustomer(int productId)
        {
            string sqlGetProduct = "SELECT * FROM View_LayChiTietChungSanPhamKhachHang WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sqlGetProduct, parameters: new[] { new SqlParameter("@productId", productId) });

            var product = DataHelper.MapToList<ChiTietSanPhamKhachHang>(productData).FirstOrDefault();

            return product;
        }

        public SanPham? GetByProductIdOrigin(int? id)
        {
            string sql = @"
                SELECT 
                MaSP, MaQG, MaTH, MaLSP, TenSP, 
                SoLuongSP, GiaNhapSP, GiaGocSP, PhanLoaiSP,
                NamSanXuatSP, BaoHanhSP, KichThuocSP, KhoiLuongSP,
                CongSuatTieuThu, ChatLieuSP, TienIchSP, CongNgheSP,
                MucGiamGiaSP, NgayHetGiamGiaSP, NgayTaoSP, NgayCapNhatSP, TrangThaiSP
                FROM SanPham 
                WHERE MaSP = @productId";
            
            SqlParameter[]? parameters = new[] { new SqlParameter("@productId", id) };

            var data = _databaseHelper.ExecuteQuery(sql, parameters: parameters);

            var product = DataHelper.MapToList<SanPham>(data).FirstOrDefault();

            return product;
        }

        public List<Anh>? GetImagesProduct(int productId)
        {
            string sqlGetImage = @"
                SELECT MaAnh, UrlAnh 
                FROM Anh 
                WHERE TrangThaiAnh = 1 AND MaSP = @productId
                ORDER BY MacDinhAnh DESC
            ";

            var imageDatas = _databaseHelper.ExecuteQuery(sqlGetImage, parameters: new[] { new SqlParameter("@productId", productId) });

            var images = DataHelper.MapToList<Anh>(imageDatas);

            return images;
        }

        public MayLanh? GetMayLanhDetailForCustomer(int productId)
        {
            string sql = "SELECT * FROM MayLanh WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@productId", productId) });

            var mayLanh = DataHelper.MapToList<MayLanh>(productData).FirstOrDefault();

            return mayLanh;;
        }

        public TuLanh? GetTuLanhDetailForCustomer(int productId)
        {
            string sql = "SELECT * FROM TuLanh WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@productId", productId) });

            var tuLanh = DataHelper.MapToList<TuLanh>(productData).FirstOrDefault();

            return tuLanh;;
        }

        public MayLocKhongKhi? GetMayLocKhongKhiDetailForCustomer(int productId)
        {
            string sql = "SELECT * FROM MayLocKhongKhi WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@productId", productId) });

            var mayLocKhongKhi = DataHelper.MapToList<MayLocKhongKhi>(productData).FirstOrDefault();

            return mayLocKhongKhi;;
        }

        public MayLocNuoc? GetMayLocNuocDetailForCustomer(int productId)
        {
            string sql = "SELECT * FROM MayLocNuoc WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@productId", productId) });

            var mayLocNuoc = DataHelper.MapToList<MayLocNuoc>(productData).FirstOrDefault();

            return mayLocNuoc;;
        }

        public MayRuaChen? GetMayRuaChenDetailForCustomer(int productId)
        {
            string sql = "SELECT * FROM MayRuaChen WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@productId", productId) });

            var mayRuaChen = DataHelper.MapToList<MayRuaChen>(productData).FirstOrDefault();

            return mayRuaChen;;
        }

        public NoiComDien? GetNoiComDienDetailForCustomer(int productId)
        {
            string sql = "SELECT * FROM NoiComDien WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@productId", productId) });

            var noiComDien = DataHelper.MapToList<NoiComDien>(productData).FirstOrDefault();

            return noiComDien;;
        }

        public NoiChien? GetNoiChienDetailForCustomer(int productId)
        {
            string sql = "SELECT * FROM NoiChien WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@productId", productId) });

            var noiChien = DataHelper.MapToList<NoiChien>(productData).FirstOrDefault();

            return noiChien;;
        }

        public TiVi? GetTiViDetailForCustomer(int productId)
        {
            string sql = "SELECT * FROM TiVi WHERE MaSP = @productId";

            var productData = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@productId", productId) });

            var tiVi = DataHelper.MapToList<TiVi>(productData).FirstOrDefault();

            return tiVi;;
        }

        public void CreateUpdateRating(DanhGia danhGia)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaSP", SqlDbType.Int)
                {
                    Value = danhGia.MaSP
                },
                 new SqlParameter("@SoDienThoaiKH", SqlDbType.VarChar, 10)
                {
                    Value = danhGia.SoDienThoaiKH
                },
                new SqlParameter("@DiemDG", SqlDbType.Int)
                {
                    Value = danhGia.DiemDG
                },
                new SqlParameter("@NhanXetDG", SqlDbType.NVarChar, 255)
                {
                    Value = danhGia.NhanXetDG
                },
            };

            _databaseHelper.ExecuteNonQuery(
                "Proc_TaoSuaDanhGia",
                CommandType.StoredProcedure,
                parameters.ToArray()
            );
        }

        public List<DanhGia>? GetRatingProduct(int productId)
        {
            string sqlGetImage = @"
                SELECT
                dg.MaKH,
                kh.TenKH,
                kh.SoDienThoaiKH,
                dg.MaSP,
                dg.DiemDG,
                dg.NhanXetDG,
                dg.NgayTaoDG,
                dg.NgayCapNhatDG
                FROM DanhGia dg
                JOIN KhachHang kh
                ON dg.MaKH = kh.MaKH
                WHERE dg.MaSP = @productId
            ";

            var datas = _databaseHelper.ExecuteQuery(sqlGetImage, parameters: new[] { new SqlParameter("@productId", productId) });

            var ratings = DataHelper.MapToList<DanhGia>(datas);

            return ratings;
        }

        public int CreateProduct(SanPham sanPham)
        {
            string sql = @"
                INSERT INTO SanPham (MaQG, MaTH, MaLSP, TenSP, SoLuongSP, GiaNhapSP, GiaGocSP, 
                                   PhanLoaiSP, NamSanXuatSP, BaoHanhSP, KichThuocSP, KhoiLuongSP, 
                                   CongSuatTieuThu, ChatLieuSP, TienIchSP, CongNgheSP, MucGiamGiaSP, 
                                   NgayHetGiamGiaSP, NgayTaoSP, NgayCapNhatSP, TrangThaiSP)
                OUTPUT INSERTED.MaSP
                VALUES (@MaQG, @MaTH, @MaLSP, @TenSP, @SoLuongSP, @GiaNhapSP, @GiaGocSP, 
                        @PhanLoaiSP, @NamSanXuatSP, @BaoHanhSP, @KichThuocSP, @KhoiLuongSP, 
                        @CongSuatTieuThu, @ChatLieuSP, @TienIchSP, @CongNgheSP, @MucGiamGiaSP, 
                        @NgayHetGiamGiaSP, GETDATE(), GETDATE(), @TrangThaiSP)";

            var parameters = new[]
            {
                new SqlParameter("@MaQG", sanPham.MaQG ?? (object)DBNull.Value),
                new SqlParameter("@MaTH", sanPham.MaTH ?? (object)DBNull.Value),
                new SqlParameter("@MaLSP", sanPham.MaLSP ?? (object)DBNull.Value),
                new SqlParameter("@TenSP", sanPham.TenSP ?? (object)DBNull.Value),
                new SqlParameter("@SoLuongSP", sanPham.SoLuongSP),
                new SqlParameter("@GiaNhapSP", sanPham.GiaNhapSP),
                new SqlParameter("@GiaGocSP", sanPham.GiaGocSP),
                new SqlParameter("@PhanLoaiSP", sanPham.PhanLoaiSP ?? (object)DBNull.Value),
                new SqlParameter("@NamSanXuatSP", sanPham.NamSanXuatSP ?? (object)DBNull.Value),
                new SqlParameter("@BaoHanhSP", sanPham.BaoHanhSP ?? (object)DBNull.Value),
                new SqlParameter("@KichThuocSP", sanPham.KichThuocSP ?? (object)DBNull.Value),
                new SqlParameter("@KhoiLuongSP", sanPham.KhoiLuongSP ?? (object)DBNull.Value),
                new SqlParameter("@CongSuatTieuThu", sanPham.CongSuatTieuThu ?? (object)DBNull.Value),
                new SqlParameter("@ChatLieuSP", sanPham.ChatLieuSP ?? (object)DBNull.Value),
                new SqlParameter("@TienIchSP", sanPham.TienIchSP ?? (object)DBNull.Value),
                new SqlParameter("@CongNgheSP", sanPham.CongNgheSP ?? (object)DBNull.Value),
                new SqlParameter("@MucGiamGiaSP", sanPham.MucGiamGiaSP),
                new SqlParameter("@NgayHetGiamGiaSP", sanPham.NgayHetGiamGiaSP ?? (object)DBNull.Value),
                new SqlParameter("@TrangThaiSP", sanPham.TrangThaiSP)
            };

            return (int)(_databaseHelper.ExecuteScalar(sql, parameters: parameters) ?? 0);
        }

        public void UpdateProduct(SanPham sanPham)
        {
            string sql = @"
                UPDATE SanPham SET 
                    MaQG = @MaQG, MaTH = @MaTH, MaLSP = @MaLSP, TenSP = @TenSP,
                    SoLuongSP = @SoLuongSP, GiaNhapSP = @GiaNhapSP, GiaGocSP = @GiaGocSP,
                    PhanLoaiSP = @PhanLoaiSP, NamSanXuatSP = @NamSanXuatSP, BaoHanhSP = @BaoHanhSP,
                    KichThuocSP = @KichThuocSP, KhoiLuongSP = @KhoiLuongSP, CongSuatTieuThu = @CongSuatTieuThu,
                    ChatLieuSP = @ChatLieuSP, TienIchSP = @TienIchSP, CongNgheSP = @CongNgheSP,
                    MucGiamGiaSP = @MucGiamGiaSP, NgayHetGiamGiaSP = @NgayHetGiamGiaSP,
                    NgayCapNhatSP = GETDATE(), TrangThaiSP = @TrangThaiSP
                WHERE MaSP = @MaSP";

            var parameters = new[]
            {
                new SqlParameter("@MaSP", sanPham.MaSP),
                new SqlParameter("@MaQG", sanPham.MaQG ?? (object)DBNull.Value),
                new SqlParameter("@MaTH", sanPham.MaTH ?? (object)DBNull.Value),
                new SqlParameter("@MaLSP", sanPham.MaLSP ?? (object)DBNull.Value),
                new SqlParameter("@TenSP", sanPham.TenSP ?? (object)DBNull.Value),
                new SqlParameter("@SoLuongSP", sanPham.SoLuongSP),
                new SqlParameter("@GiaNhapSP", sanPham.GiaNhapSP),
                new SqlParameter("@GiaGocSP", sanPham.GiaGocSP),
                new SqlParameter("@PhanLoaiSP", sanPham.PhanLoaiSP ?? (object)DBNull.Value),
                new SqlParameter("@NamSanXuatSP", sanPham.NamSanXuatSP ?? (object)DBNull.Value),
                new SqlParameter("@BaoHanhSP", sanPham.BaoHanhSP ?? (object)DBNull.Value),
                new SqlParameter("@KichThuocSP", sanPham.KichThuocSP ?? (object)DBNull.Value),
                new SqlParameter("@KhoiLuongSP", sanPham.KhoiLuongSP ?? (object)DBNull.Value),
                new SqlParameter("@CongSuatTieuThu", sanPham.CongSuatTieuThu ?? (object)DBNull.Value),
                new SqlParameter("@ChatLieuSP", sanPham.ChatLieuSP ?? (object)DBNull.Value),
                new SqlParameter("@TienIchSP", sanPham.TienIchSP ?? (object)DBNull.Value),
                new SqlParameter("@CongNgheSP", sanPham.CongNgheSP ?? (object)DBNull.Value),
                new SqlParameter("@MucGiamGiaSP", sanPham.MucGiamGiaSP),
                new SqlParameter("@NgayHetGiamGiaSP", sanPham.NgayHetGiamGiaSP ?? (object)DBNull.Value),
                new SqlParameter("@TrangThaiSP", sanPham.TrangThaiSP)
            };

            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }

        public void CreateTiVi(TiVi tiVi)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaTV", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = tiVi.MaSP },
                new SqlParameter("@CoManHinhTV", SqlDbType.NVarChar, 10) { Value = tiVi.CoManHinhTV ?? (object)DBNull.Value },
                new SqlParameter("@DoPhanGiaiTV", SqlDbType.NVarChar, 10) { Value = tiVi.DoPhanGiaiTV ?? (object)DBNull.Value },
                new SqlParameter("@LoaiManHinhTV", SqlDbType.NVarChar, 100) { Value = tiVi.LoaiManHinhTV ?? (object)DBNull.Value },
                new SqlParameter("@TanSoQuetTV", SqlDbType.NVarChar, 10) { Value = tiVi.TanSoQuetTV ?? (object)DBNull.Value },
                new SqlParameter("@DieuKhienTV", SqlDbType.NVarChar, 100) { Value = tiVi.DieuKhienTV ?? (object)DBNull.Value },
                new SqlParameter("@CongKetNoiTV", SqlDbType.NVarChar, 100) { Value = tiVi.CongKetNoiTV ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaTiVi", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void UpdateTiVi(TiVi tiVi)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaTV", SqlDbType.Int) { Value = tiVi.MaTV },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = tiVi.MaSP },
                new SqlParameter("@CoManHinhTV", SqlDbType.NVarChar, 10) { Value = tiVi.CoManHinhTV ?? (object)DBNull.Value },
                new SqlParameter("@DoPhanGiaiTV", SqlDbType.NVarChar, 10) { Value = tiVi.DoPhanGiaiTV ?? (object)DBNull.Value },
                new SqlParameter("@LoaiManHinhTV", SqlDbType.NVarChar, 100) { Value = tiVi.LoaiManHinhTV ?? (object)DBNull.Value },
                new SqlParameter("@TanSoQuetTV", SqlDbType.NVarChar, 10) { Value = tiVi.TanSoQuetTV ?? (object)DBNull.Value },
                new SqlParameter("@DieuKhienTV", SqlDbType.NVarChar, 100) { Value = tiVi.DieuKhienTV ?? (object)DBNull.Value },
                new SqlParameter("@CongKetNoiTV", SqlDbType.NVarChar, 100) { Value = tiVi.CongKetNoiTV ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaTiVi", CommandType.StoredProcedure, parameters.ToArray());
        }

        public int CreateProductWithStoredProc(SanPham sanPham)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaQG", SqlDbType.Int) { Value = sanPham.MaQG ?? (object)DBNull.Value },
                new SqlParameter("@MaTH", SqlDbType.Int) { Value = sanPham.MaTH ?? (object)DBNull.Value },
                new SqlParameter("@MaLSP", SqlDbType.Int) { Value = sanPham.MaLSP ?? (object)DBNull.Value },
                new SqlParameter("@TenSP", SqlDbType.NVarChar, 100) { Value = sanPham.TenSP ?? (object)DBNull.Value },
                new SqlParameter("@SoLuongSP", SqlDbType.Int) { Value = sanPham.SoLuongSP },
                new SqlParameter("@GiaNhapSP", SqlDbType.Decimal) { Value = sanPham.GiaNhapSP },
                new SqlParameter("@GiaGocSP", SqlDbType.Decimal) { Value = sanPham.GiaGocSP },
                new SqlParameter("@PhanLoaiSP", SqlDbType.NVarChar, 50) { Value = sanPham.PhanLoaiSP ?? (object)DBNull.Value },
                new SqlParameter("@NamSanXuatSP", SqlDbType.Int) { Value = sanPham.NamSanXuatSP ?? (object)DBNull.Value },
                new SqlParameter("@BaoHanhSP", SqlDbType.NVarChar, 255) { Value = sanPham.BaoHanhSP ?? (object)DBNull.Value },
                new SqlParameter("@KichThuocSP", SqlDbType.NVarChar, 255) { Value = sanPham.KichThuocSP ?? (object)DBNull.Value },
                new SqlParameter("@KhoiLuongSP", SqlDbType.NVarChar, 100) { Value = sanPham.KhoiLuongSP ?? (object)DBNull.Value },
                new SqlParameter("@CongSuatTieuThu", SqlDbType.NVarChar, 10) { Value = sanPham.CongSuatTieuThu ?? (object)DBNull.Value },
                new SqlParameter("@ChatLieuSP", SqlDbType.NVarChar, 255) { Value = sanPham.ChatLieuSP ?? (object)DBNull.Value },
                new SqlParameter("@TienIchSP", SqlDbType.NVarChar, 500) { Value = sanPham.TienIchSP ?? (object)DBNull.Value },
                new SqlParameter("@CongNgheSP", SqlDbType.NVarChar, 500) { Value = sanPham.CongNgheSP ?? (object)DBNull.Value },
                new SqlParameter("@MucGiamGiaSP", SqlDbType.Decimal) { Value = sanPham.MucGiamGiaSP },
                new SqlParameter("@NgayHetGiamGiaSP", SqlDbType.DateTime2) { Value = sanPham.NgayHetGiamGiaSP ?? (object)DBNull.Value },
                new SqlParameter("@TrangThaiSP", SqlDbType.Int) { Value = sanPham.TrangThaiSP },
                new SqlParameter("@MaSP_Output", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaSanPham", CommandType.StoredProcedure, parameters.ToArray());
            
            return (int)parameters.Last().Value;
        }

        public void UpdateProductWithStoredProc(SanPham sanPham)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = sanPham.MaSP },
                new SqlParameter("@MaQG", SqlDbType.Int) { Value = sanPham.MaQG ?? (object)DBNull.Value },
                new SqlParameter("@MaTH", SqlDbType.Int) { Value = sanPham.MaTH ?? (object)DBNull.Value },
                new SqlParameter("@MaLSP", SqlDbType.Int) { Value = sanPham.MaLSP ?? (object)DBNull.Value },
                new SqlParameter("@TenSP", SqlDbType.NVarChar, 100) { Value = sanPham.TenSP ?? (object)DBNull.Value },
                new SqlParameter("@SoLuongSP", SqlDbType.Int) { Value = sanPham.SoLuongSP },
                new SqlParameter("@GiaNhapSP", SqlDbType.Decimal) { Value = sanPham.GiaNhapSP },
                new SqlParameter("@GiaGocSP", SqlDbType.Decimal) { Value = sanPham.GiaGocSP },
                new SqlParameter("@PhanLoaiSP", SqlDbType.NVarChar, 50) { Value = sanPham.PhanLoaiSP ?? (object)DBNull.Value },
                new SqlParameter("@NamSanXuatSP", SqlDbType.Int) { Value = sanPham.NamSanXuatSP ?? (object)DBNull.Value },
                new SqlParameter("@BaoHanhSP", SqlDbType.NVarChar, 255) { Value = sanPham.BaoHanhSP ?? (object)DBNull.Value },
                new SqlParameter("@KichThuocSP", SqlDbType.NVarChar, 255) { Value = sanPham.KichThuocSP ?? (object)DBNull.Value },
                new SqlParameter("@KhoiLuongSP", SqlDbType.NVarChar, 100) { Value = sanPham.KhoiLuongSP ?? (object)DBNull.Value },
                new SqlParameter("@CongSuatTieuThu", SqlDbType.NVarChar, 10) { Value = sanPham.CongSuatTieuThu ?? (object)DBNull.Value },
                new SqlParameter("@ChatLieuSP", SqlDbType.NVarChar, 255) { Value = sanPham.ChatLieuSP ?? (object)DBNull.Value },
                new SqlParameter("@TienIchSP", SqlDbType.NVarChar, 500) { Value = sanPham.TienIchSP ?? (object)DBNull.Value },
                new SqlParameter("@CongNgheSP", SqlDbType.NVarChar, 500) { Value = sanPham.CongNgheSP ?? (object)DBNull.Value },
                new SqlParameter("@MucGiamGiaSP", SqlDbType.Decimal) { Value = sanPham.MucGiamGiaSP },
                new SqlParameter("@NgayHetGiamGiaSP", SqlDbType.DateTime2) { Value = sanPham.NgayHetGiamGiaSP ?? (object)DBNull.Value },
                new SqlParameter("@TrangThaiSP", SqlDbType.Int) { Value = sanPham.TrangThaiSP },
                new SqlParameter("@MaSP_Output", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaSanPham", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void CreateMayLanh(MayLanh mayLanh)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaML", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = mayLanh.MaSP },
                new SqlParameter("@CongSuatLamLanhML", SqlDbType.NVarChar, 30) { Value = mayLanh.CongSuatLamLanhML ?? (object)DBNull.Value },
                new SqlParameter("@PhamViLamLanhML", SqlDbType.NVarChar, 40) { Value = mayLanh.PhamViLamLanhML ?? (object)DBNull.Value },
                new SqlParameter("@DoOnML", SqlDbType.NVarChar, 50) { Value = mayLanh.DoOnML ?? (object)DBNull.Value },
                new SqlParameter("@LoaiGasML", SqlDbType.NVarChar, 10) { Value = mayLanh.LoaiGasML ?? (object)DBNull.Value },
                new SqlParameter("@CheDoGioML", SqlDbType.NVarChar, 60) { Value = mayLanh.CheDoGioML ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaMayLanh", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void UpdateMayLanh(MayLanh mayLanh)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaML", SqlDbType.Int) { Value = mayLanh.MaML },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = mayLanh.MaSP },
                new SqlParameter("@CongSuatLamLanhML", SqlDbType.NVarChar, 30) { Value = mayLanh.CongSuatLamLanhML ?? (object)DBNull.Value },
                new SqlParameter("@PhamViLamLanhML", SqlDbType.NVarChar, 40) { Value = mayLanh.PhamViLamLanhML ?? (object)DBNull.Value },
                new SqlParameter("@DoOnML", SqlDbType.NVarChar, 50) { Value = mayLanh.DoOnML ?? (object)DBNull.Value },
                new SqlParameter("@LoaiGasML", SqlDbType.NVarChar, 10) { Value = mayLanh.LoaiGasML ?? (object)DBNull.Value },
                new SqlParameter("@CheDoGioML", SqlDbType.NVarChar, 60) { Value = mayLanh.CheDoGioML ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaMayLanh", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void CreateTuLanh(TuLanh tuLanh)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaTL", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = tuLanh.MaSP },
                new SqlParameter("@DungTichNganDaTL", SqlDbType.NVarChar, 10) { Value = tuLanh.DungTichNganDaTL ?? (object)DBNull.Value },
                new SqlParameter("@DungTichNganLanhTL", SqlDbType.NVarChar, 10) { Value = tuLanh.DungTichNganLanhTL ?? (object)DBNull.Value },
                new SqlParameter("@LayNuocNgoaiTL", SqlDbType.NVarChar, 10) { Value = tuLanh.LayNuocNgoaiTL ?? (object)DBNull.Value },
                new SqlParameter("@LayDaTuDongTL", SqlDbType.NVarChar, 10) { Value = tuLanh.LayDaTuDongTL ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaTuLanh", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void UpdateTuLanh(TuLanh tuLanh)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaTL", SqlDbType.Int) { Value = tuLanh.MaTL },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = tuLanh.MaSP },
                new SqlParameter("@DungTichNganDaTL", SqlDbType.NVarChar, 10) { Value = tuLanh.DungTichNganDaTL ?? (object)DBNull.Value },
                new SqlParameter("@DungTichNganLanhTL", SqlDbType.NVarChar, 10) { Value = tuLanh.DungTichNganLanhTL ?? (object)DBNull.Value },
                new SqlParameter("@LayNuocNgoaiTL", SqlDbType.NVarChar, 10) { Value = tuLanh.LayNuocNgoaiTL ?? (object)DBNull.Value },
                new SqlParameter("@LayDaTuDongTL", SqlDbType.NVarChar, 10) { Value = tuLanh.LayDaTuDongTL ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaTuLanh", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void CreateNoiChien(NoiChien noiChien)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaNC", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = noiChien.MaSP },
                new SqlParameter("@DungTichTongNC", SqlDbType.NVarChar, 10) { Value = noiChien.DungTichTongNC ?? (object)DBNull.Value },
                new SqlParameter("@DungTichSuDungNC", SqlDbType.NVarChar, 10) { Value = noiChien.DungTichSuDungNC ?? (object)DBNull.Value },
                new SqlParameter("@NhietDoNC", SqlDbType.NVarChar, 20) { Value = noiChien.NhietDoNC ?? (object)DBNull.Value },
                new SqlParameter("@HenGioNC", SqlDbType.NVarChar, 20) { Value = noiChien.HenGioNC ?? (object)DBNull.Value },
                new SqlParameter("@BangDieuKhienNC", SqlDbType.NVarChar, 50) { Value = noiChien.BangDieuKhienNC ?? (object)DBNull.Value },
                new SqlParameter("@ChieuDaiDayDienNC", SqlDbType.NVarChar, 10) { Value = noiChien.ChieuDaiDayDienNC ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaNoiChien", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void UpdateNoiChien(NoiChien noiChien)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaNC", SqlDbType.Int) { Value = noiChien.MaNC },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = noiChien.MaSP },
                new SqlParameter("@DungTichTongNC", SqlDbType.NVarChar, 10) { Value = noiChien.DungTichTongNC ?? (object)DBNull.Value },
                new SqlParameter("@DungTichSuDungNC", SqlDbType.NVarChar, 10) { Value = noiChien.DungTichSuDungNC ?? (object)DBNull.Value },
                new SqlParameter("@NhietDoNC", SqlDbType.NVarChar, 20) { Value = noiChien.NhietDoNC ?? (object)DBNull.Value },
                new SqlParameter("@HenGioNC", SqlDbType.NVarChar, 20) { Value = noiChien.HenGioNC ?? (object)DBNull.Value },
                new SqlParameter("@BangDieuKhienNC", SqlDbType.NVarChar, 50) { Value = noiChien.BangDieuKhienNC ?? (object)DBNull.Value },
                new SqlParameter("@ChieuDaiDayDienNC", SqlDbType.NVarChar, 10) { Value = noiChien.ChieuDaiDayDienNC ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaNoiChien", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void CreateMayLocNuoc(MayLocNuoc mayLocNuoc)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaMLN", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = mayLocNuoc.MaSP },
                new SqlParameter("@KieuLapMLN", SqlDbType.NVarChar, 30) { Value = mayLocNuoc.KieuLapMLN ?? (object)DBNull.Value },
                new SqlParameter("@CongSuatLocMLN", SqlDbType.NVarChar, 30) { Value = mayLocNuoc.CongSuatLocMLN ?? (object)DBNull.Value },
                new SqlParameter("@TiLeLocThaiMLN", SqlDbType.NVarChar, 30) { Value = mayLocNuoc.TiLeLocThaiMLN ?? (object)DBNull.Value },
                new SqlParameter("@ChiSoNuocMLN", SqlDbType.NVarChar, 100) { Value = mayLocNuoc.ChiSoNuocMLN ?? (object)DBNull.Value },
                new SqlParameter("@DoPHThucTeMLN", SqlDbType.NVarChar, 100) { Value = mayLocNuoc.DoPHThucTeMLN ?? (object)DBNull.Value },
                new SqlParameter("@ApLucNuocYeuCauMLN", SqlDbType.NVarChar, 20) { Value = mayLocNuoc.ApLucNuocYeuCauMLN ?? (object)DBNull.Value },
                new SqlParameter("@SoLoiLocMLN", SqlDbType.Int) { Value = mayLocNuoc.SoLoiLocMLN },
                new SqlParameter("@BangDieuKhienMLN", SqlDbType.NVarChar, 40) { Value = mayLocNuoc.BangDieuKhienMLN ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaMayLocNuoc", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void UpdateMayLocNuoc(MayLocNuoc mayLocNuoc)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaMLN", SqlDbType.Int) { Value = mayLocNuoc.MaMLN },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = mayLocNuoc.MaSP },
                new SqlParameter("@KieuLapMLN", SqlDbType.NVarChar, 30) { Value = mayLocNuoc.KieuLapMLN ?? (object)DBNull.Value },
                new SqlParameter("@CongSuatLocMLN", SqlDbType.NVarChar, 30) { Value = mayLocNuoc.CongSuatLocMLN ?? (object)DBNull.Value },
                new SqlParameter("@TiLeLocThaiMLN", SqlDbType.NVarChar, 30) { Value = mayLocNuoc.TiLeLocThaiMLN ?? (object)DBNull.Value },
                new SqlParameter("@ChiSoNuocMLN", SqlDbType.NVarChar, 100) { Value = mayLocNuoc.ChiSoNuocMLN ?? (object)DBNull.Value },
                new SqlParameter("@DoPHThucTeMLN", SqlDbType.NVarChar, 100) { Value = mayLocNuoc.DoPHThucTeMLN ?? (object)DBNull.Value },
                new SqlParameter("@ApLucNuocYeuCauMLN", SqlDbType.NVarChar, 20) { Value = mayLocNuoc.ApLucNuocYeuCauMLN ?? (object)DBNull.Value },
                new SqlParameter("@SoLoiLocMLN", SqlDbType.Int) { Value = mayLocNuoc.SoLoiLocMLN },
                new SqlParameter("@BangDieuKhienMLN", SqlDbType.NVarChar, 40) { Value = mayLocNuoc.BangDieuKhienMLN ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaMayLocNuoc", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void CreateNoiComDien(NoiComDien noiComDien)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaNCD", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = noiComDien.MaSP },
                new SqlParameter("@DungTichNCD", SqlDbType.NVarChar, 10) { Value = noiComDien.DungTichNCD ?? (object)DBNull.Value },
                new SqlParameter("@ChucNangNCD", SqlDbType.NVarChar, 255) { Value = noiComDien.ChucNangNCD ?? (object)DBNull.Value },
                new SqlParameter("@DoDayNCD", SqlDbType.NVarChar, 50) { Value = noiComDien.DoDayNCD ?? (object)DBNull.Value },
                new SqlParameter("@DieuKhienNCD", SqlDbType.NVarChar, 50) { Value = noiComDien.DieuKhienNCD ?? (object)DBNull.Value },
                new SqlParameter("@ChieuDaiDayDienNCD", SqlDbType.NVarChar, 10) { Value = noiComDien.ChieuDaiDayDienNCD ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaNoiComDien", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void UpdateNoiComDien(NoiComDien noiComDien)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaNCD", SqlDbType.Int) { Value = noiComDien.MaNCD },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = noiComDien.MaSP },
                new SqlParameter("@DungTichNCD", SqlDbType.NVarChar, 10) { Value = noiComDien.DungTichNCD ?? (object)DBNull.Value },
                new SqlParameter("@ChucNangNCD", SqlDbType.NVarChar, 255) { Value = noiComDien.ChucNangNCD ?? (object)DBNull.Value },
                new SqlParameter("@DoDayNCD", SqlDbType.NVarChar, 50) { Value = noiComDien.DoDayNCD ?? (object)DBNull.Value },
                new SqlParameter("@DieuKhienNCD", SqlDbType.NVarChar, 50) { Value = noiComDien.DieuKhienNCD ?? (object)DBNull.Value },
                new SqlParameter("@ChieuDaiDayDienNCD", SqlDbType.NVarChar, 10) { Value = noiComDien.ChieuDaiDayDienNCD ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaNoiComDien", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void CreateMayLocKhongKhi(MayLocKhongKhi mayLocKhongKhi)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaMLKK", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = mayLocKhongKhi.MaSP },
                new SqlParameter("@LoaiBuiLocDuocMLKK", SqlDbType.NVarChar, 100) { Value = mayLocKhongKhi.LoaiBuiLocDuocMLKK ?? (object)DBNull.Value },
                new SqlParameter("@PhamViLocMLKK", SqlDbType.NVarChar, 10) { Value = mayLocKhongKhi.PhamViLocMLKK ?? (object)DBNull.Value },
                new SqlParameter("@LuongGioMLKK", SqlDbType.NVarChar, 10) { Value = mayLocKhongKhi.LuongGioMLKK ?? (object)DBNull.Value },
                new SqlParameter("@MangLocMLKK", SqlDbType.NVarChar, 100) { Value = mayLocKhongKhi.MangLocMLKK ?? (object)DBNull.Value },
                new SqlParameter("@BangDieuKhienMLKK", SqlDbType.NVarChar, 50) { Value = mayLocKhongKhi.BangDieuKhienMLKK ?? (object)DBNull.Value },
                new SqlParameter("@DoOnMLKK", SqlDbType.NVarChar, 10) { Value = mayLocKhongKhi.DoOnMLKK ?? (object)DBNull.Value },
                new SqlParameter("@CamBienMLKK", SqlDbType.NVarChar, 100) { Value = mayLocKhongKhi.CamBienMLKK ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaMayLocKhongKhi", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void UpdateMayLocKhongKhi(MayLocKhongKhi mayLocKhongKhi)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaMLKK", SqlDbType.Int) { Value = mayLocKhongKhi.MaMLKK },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = mayLocKhongKhi.MaSP },
                new SqlParameter("@LoaiBuiLocDuocMLKK", SqlDbType.NVarChar, 100) { Value = mayLocKhongKhi.LoaiBuiLocDuocMLKK ?? (object)DBNull.Value },
                new SqlParameter("@PhamViLocMLKK", SqlDbType.NVarChar, 10) { Value = mayLocKhongKhi.PhamViLocMLKK ?? (object)DBNull.Value },
                new SqlParameter("@LuongGioMLKK", SqlDbType.NVarChar, 10) { Value = mayLocKhongKhi.LuongGioMLKK ?? (object)DBNull.Value },
                new SqlParameter("@MangLocMLKK", SqlDbType.NVarChar, 100) { Value = mayLocKhongKhi.MangLocMLKK ?? (object)DBNull.Value },
                new SqlParameter("@BangDieuKhienMLKK", SqlDbType.NVarChar, 50) { Value = mayLocKhongKhi.BangDieuKhienMLKK ?? (object)DBNull.Value },
                new SqlParameter("@DoOnMLKK", SqlDbType.NVarChar, 10) { Value = mayLocKhongKhi.DoOnMLKK ?? (object)DBNull.Value },
                new SqlParameter("@CamBienMLKK", SqlDbType.NVarChar, 100) { Value = mayLocKhongKhi.CamBienMLKK ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaMayLocKhongKhi", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void CreateMayRuaChen(MayRuaChen mayRuaChen)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaMRC", SqlDbType.Int) { Value = DBNull.Value },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = mayRuaChen.MaSP },
                new SqlParameter("@NuocTieuThuMRC", SqlDbType.NVarChar, 30) { Value = mayRuaChen.NuocTieuThuMRC ?? (object)DBNull.Value },
                new SqlParameter("@SoChenRuaDuocMRC", SqlDbType.NVarChar, 30) { Value = mayRuaChen.SoChenRuaDuocMRC ?? (object)DBNull.Value },
                new SqlParameter("@DoOnMRC", SqlDbType.NVarChar, 10) { Value = mayRuaChen.DoOnMRC ?? (object)DBNull.Value },
                new SqlParameter("@BangDieuKhienMRC", SqlDbType.NVarChar, 50) { Value = mayRuaChen.BangDieuKhienMRC ?? (object)DBNull.Value },
                new SqlParameter("@ChieuDaiOngCapNuocMRC", SqlDbType.NVarChar, 10) { Value = mayRuaChen.ChieuDaiOngCapNuocMRC ?? (object)DBNull.Value },
                new SqlParameter("@ChieuDaiOngThoatNuocMRC", SqlDbType.NVarChar, 10) { Value = mayRuaChen.ChieuDaiOngThoatNuocMRC ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaMayRuaChen", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void UpdateMayRuaChen(MayRuaChen mayRuaChen)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaMRC", SqlDbType.Int) { Value = mayRuaChen.MaMRC },
                new SqlParameter("@MaSP", SqlDbType.Int) { Value = mayRuaChen.MaSP },
                new SqlParameter("@NuocTieuThuMRC", SqlDbType.NVarChar, 30) { Value = mayRuaChen.NuocTieuThuMRC ?? (object)DBNull.Value },
                new SqlParameter("@SoChenRuaDuocMRC", SqlDbType.NVarChar, 30) { Value = mayRuaChen.SoChenRuaDuocMRC ?? (object)DBNull.Value },
                new SqlParameter("@DoOnMRC", SqlDbType.NVarChar, 10) { Value = mayRuaChen.DoOnMRC ?? (object)DBNull.Value },
                new SqlParameter("@BangDieuKhienMRC", SqlDbType.NVarChar, 50) { Value = mayRuaChen.BangDieuKhienMRC ?? (object)DBNull.Value },
                new SqlParameter("@ChieuDaiOngCapNuocMRC", SqlDbType.NVarChar, 10) { Value = mayRuaChen.ChieuDaiOngCapNuocMRC ?? (object)DBNull.Value },
                new SqlParameter("@ChieuDaiOngThoatNuocMRC", SqlDbType.NVarChar, 10) { Value = mayRuaChen.ChieuDaiOngThoatNuocMRC ?? (object)DBNull.Value }
            };

            _databaseHelper.ExecuteNonQuery("Proc_TaoSuaMayRuaChen", CommandType.StoredProcedure, parameters.ToArray());
        }

        public List<SanPham> GetAllProductsByNameForAdmin(string? tenSP, int productTypeId = 0)
        {
            string sql = @"
            SELECT 
            sp.MaSP, sp.MaQG, sp.MaTH, sp.MaLSP, sp.TenSP,
            sp.SoLuongSP, sp.GiaNhapSP, sp.GiaGocSP, sp.PhanLoaiSP,
            sp.NamSanXuatSP, sp.BaoHanhSP, sp.KichThuocSP, sp.KhoiLuongSP,
            sp.CongSuatTieuThu, sp.ChatLieuSP, sp.TienIchSP, sp.CongNgheSP,
            sp.MucGiamGiaSP, sp.NgayHetGiamGiaSP, sp.NgayTaoSP, sp.NgayCapNhatSP, sp.TrangThaiSP,
            lsp.TenLSP,
            asp.UrlAnh 
            FROM SanPham sp 
            JOIN LoaiSanPham lsp ON sp.MaLSP = lsp.MaLSP
            LEFT JOIN Anh asp ON sp.MaSP = asp.MaSP AND asp.MacDinhAnh = 1
            WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(tenSP))
            {
                sql += " AND sp.TenSP COLLATE Vietnamese_CI_AI LIKE '%' + @TenSP + '%'";
                parameters.Add(new SqlParameter("@TenSP", tenSP));
            }

            if (productTypeId > 0)
            {
                sql += " AND sp.MaLSP = @MaLSP";
                parameters.Add(new SqlParameter("@MaLSP", productTypeId));
            }

            sql += " ORDER BY sp.NgayTaoSP DESC";

            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters.ToArray());
            var products = DataHelper.MapToList<SanPham>(datas);

            return products;
        }

        // Quản lý ảnh sản phẩm
        public List<Anh> GetProductImages(int productId)
        {
            string sql = "SELECT MaAnh, MaSP, UrlAnh, MacDinhAnh, TrangThaiAnh FROM Anh WHERE MaSP = @MaSP AND TrangThaiAnh = 1 ORDER BY MacDinhAnh DESC, MaAnh ASC";
            var parameters = new[] { new SqlParameter("@MaSP", productId) };
            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);
            return DataHelper.MapToList<Anh>(datas);
        }

        public void AddProductImage(int productId, string imageUrl, bool isDefault = false)
        {
            // Nếu là ảnh mặc định, bỏ mặc định của các ảnh khác
            if (isDefault)
            {
                string updateSql = "UPDATE Anh SET MacDinhAnh = 0 WHERE MaSP = @MaSP";
                var updateParams = new[] { new SqlParameter("@MaSP", productId) };
                _databaseHelper.ExecuteNonQuery(updateSql, parameters: updateParams);
            }

            string sql = "INSERT INTO Anh (MaSP, UrlAnh, MacDinhAnh, TrangThaiAnh) VALUES (@MaSP, @UrlAnh, @MacDinhAnh, 1)";
            var parameters = new[]
            {
                new SqlParameter("@MaSP", productId),
                new SqlParameter("@UrlAnh", imageUrl),
                new SqlParameter("@MacDinhAnh", isDefault ? 1 : 0)
            };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }

        public void DeleteProductImage(int imageId)
        {
            string sql = "UPDATE Anh SET TrangThaiAnh = 0 WHERE MaAnh = @MaAnh";
            var parameters = new[] { new SqlParameter("@MaAnh", imageId) };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }

        public void SetDefaultImage(int imageId, int productId)
        {
            // Bỏ mặc định của tất cả ảnh
            string updateSql = "UPDATE Anh SET MacDinhAnh = 0 WHERE MaSP = @MaSP";
            var updateParams = new[] { new SqlParameter("@MaSP", productId) };
            _databaseHelper.ExecuteNonQuery(updateSql, parameters: updateParams);

            // Đặt ảnh được chọn làm mặc định
            string setSql = "UPDATE Anh SET MacDinhAnh = 1 WHERE MaAnh = @MaAnh";
            var setParams = new[] { new SqlParameter("@MaAnh", imageId) };
            _databaseHelper.ExecuteNonQuery(setSql, parameters: setParams);
        }

        // Xóa sản phẩm
        public bool CanDeleteProduct(int productId)
        {
            // Kiểm tra xem sản phẩm có trong đơn hàng nào không
            // Sử dụng bảng BaoGom theo schema database
            string sql = @"
                SELECT COUNT(*) 
                FROM BaoGom bg
                INNER JOIN DonHang dh ON bg.MaDH = dh.MaDH
                WHERE bg.MaSP = @MaSP AND dh.MaTTDH != 0"; // MaTTDH != 0 nghĩa là đơn hàng chưa bị hủy
            
            var parameters = new[] { new SqlParameter("@MaSP", productId) };
            
            try
            {
                var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters);
                return Convert.ToInt32(result) == 0; // Trả về true nếu không có trong đơn hàng nào
            }
            catch (Exception)
            {
                // Nếu có lỗi, cho phép xóa để tránh block chức năng
                return true;
            }
        }

        public void DeleteProduct(int productId)
        {
            // Xóa thật sản phẩm và tất cả dữ liệu liên quan
            // Do có CASCADE DELETE nên chỉ cần xóa sản phẩm chính
            string sql = "DELETE FROM SanPham WHERE MaSP = @MaSP";
            var parameters = new[] { new SqlParameter("@MaSP", productId) };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }
    }
}