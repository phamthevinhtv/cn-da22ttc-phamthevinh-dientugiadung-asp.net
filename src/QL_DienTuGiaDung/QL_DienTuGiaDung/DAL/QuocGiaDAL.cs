using QL_DienTuGiaDung.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class QuocGiaDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public QuocGiaDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        
        public DataTable LayDanhSachQuocGia(int quyen)
        {
            string sql = "SELECT MaQG, TenQG FROM QuocGia";
            
            if (quyen == 0)
            {
                sql += " WHERE TrangThaiQG = 1";
            }
            
            sql += " ORDER BY TenQG";
            
            return _databaseHelper.ExecuteDataTable(sql);
        }

        public DataTable LayQuocGia(int maQG)
        {
            string sql = "SELECT MaQG, TenQG FROM QuocGia WHERE MaQG = @MaQG";
            var parameters = new[] { new SqlParameter("@MaQG", maQG) };
            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public int ThemQuocGia(string tenQG)
        {
            string sql = "INSERT INTO QuocGia (TenQG) VALUES (@TenQG)";
            var parameters = new[] { new SqlParameter("@TenQG", tenQG) };
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

        public int CapNhatQuocGia(int maQG, string tenQG)
        {
            string sql = "UPDATE QuocGia SET TenQG = @TenQG WHERE MaQG = @MaQG";
            var parameters = new[]
            {
                new SqlParameter("@TenQG", tenQG),
                new SqlParameter("@MaQG", maQG)
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

        public bool KiemTraCoTheSuaXoa(int maQG)
        {
            string sql = @"
                SELECT COUNT(*) FROM (
                    SELECT MaQG FROM ThuongHieu WHERE MaQG = @MaQG
                    UNION ALL
                    SELECT MaQG FROM SanPham WHERE MaQG = @MaQG
                ) AS UsedCountries";
            var parameters = new[] { new SqlParameter("@MaQG", maQG) };
            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters);
            return Convert.ToInt32(result) == 0;
        }

        public int XoaQuocGia(int maQG)
        {
            string sql = "DELETE FROM QuocGia WHERE MaQG = @MaQG";
            var parameters = new[] { new SqlParameter("@MaQG", maQG) };
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