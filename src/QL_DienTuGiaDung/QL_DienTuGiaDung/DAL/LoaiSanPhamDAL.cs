using QL_DienTuGiaDung.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class LoaiSanPhamDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public LoaiSanPhamDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public DataTable LayDanhSachLoaiSanPham(int quyen)
        {
            string sql = "SELECT MaLSP, TenLSP, ThueGTGTLSP, TrangThaiLSP FROM LoaiSanPham";

            if (quyen == 0)
            {
                sql += " WHERE TrangThaiLSP = 1";
            }

            sql += " ORDER BY MaLSP";

            return _databaseHelper.ExecuteDataTable(sql);
        }

        public DataTable LayLoaiSanPham(int maLSP)
        {
            string sql = "SELECT MaLSP, TenLSP, ThueGTGTLSP, TrangThaiLSP FROM LoaiSanPham WHERE MaLSP = @MaLSP";
            var parameters = new[] { new SqlParameter("@MaLSP", maLSP) };
            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public int ThemLoaiSanPham(string tenLSP, decimal thueGTGTLSP)
        {
            string sql = "INSERT INTO LoaiSanPham (TenLSP, ThueGTGTLSP, TrangThaiLSP) VALUES (@TenLSP, @ThueGTGTLSP, 1)";
            var parameters = new[]
            {
                new SqlParameter("@TenLSP", tenLSP),
                new SqlParameter("@ThueGTGTLSP", thueGTGTLSP)
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

        public int CapNhatLoaiSanPham(int maLSP, string tenLSP, decimal thueGTGTLSP, int trangThaiLSP)
        {
            string sql = "UPDATE LoaiSanPham SET TenLSP = @TenLSP, ThueGTGTLSP = @ThueGTGTLSP, TrangThaiLSP = @TrangThaiLSP WHERE MaLSP = @MaLSP";
            var parameters = new[]
            {
                new SqlParameter("@TenLSP", tenLSP),
                new SqlParameter("@ThueGTGTLSP", thueGTGTLSP),
                new SqlParameter("@TrangThaiLSP", trangThaiLSP),
                new SqlParameter("@MaLSP", maLSP)
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

        public bool KiemTraTenLoaiSanPhamTonTai(string tenLSP, int maLSP = 0)
        {
            string sql = "SELECT COUNT(*) FROM LoaiSanPham WHERE TenLSP = @TenLSP";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TenLSP", tenLSP)
            };

            if (maLSP > 0)
            {
                sql += " AND MaLSP != @MaLSP";
                parameters.Add(new SqlParameter("@MaLSP", maLSP));
            }

            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters.ToArray());
            return Convert.ToInt32(result) > 0;
        }

        public bool KiemTraCoTheSuaXoa(int maLSP)
        {
            string sql = "SELECT COUNT(*) FROM SanPham WHERE MaLSP = @MaLSP";
            var parameters = new[] { new SqlParameter("@MaLSP", maLSP) };
            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters);
            return Convert.ToInt32(result) == 0;
        }

        public int XoaLoaiSanPham(int maLSP)
        {
            string sql = "DELETE FROM LoaiSanPham WHERE MaLSP = @MaLSP";
            var parameters = new[] { new SqlParameter("@MaLSP", maLSP) };
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
