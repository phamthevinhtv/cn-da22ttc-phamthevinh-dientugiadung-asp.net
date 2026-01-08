using QL_DienTuGiaDung.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class ThuongHieuDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public ThuongHieuDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public DataTable LayDanhSachThuongHieu(int quyen)
        {
            string sql = @"
                SELECT th.MaTH, th.MaQG, th.TenTH, qg.TenQG 
                FROM ThuongHieu th
                JOIN QuocGia qg ON th.MaQG = qg.MaQG";
            
            if (quyen == 0)
            {
                sql += " WHERE th.TrangThaiTH = 1";
            }
            
            sql += " ORDER BY th.TenTH";
            
            return _databaseHelper.ExecuteDataTable(sql);
        }

        public DataTable LayThuongHieu(int maTH)
        {
            string sql = @"
                SELECT th.MaTH, th.MaQG, th.TenTH, qg.TenQG 
                FROM ThuongHieu th
                JOIN QuocGia qg ON th.MaQG = qg.MaQG
                WHERE th.MaTH = @MaTH";
            var parameters = new[] { new SqlParameter("@MaTH", maTH) };
            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public int ThemThuongHieu(int maQG, string tenTH)
        {
            string sql = "INSERT INTO ThuongHieu (MaQG, TenTH) VALUES (@MaQG, @TenTH)";
            var parameters = new[]
            {
                new SqlParameter("@MaQG", maQG),
                new SqlParameter("@TenTH", tenTH)
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

        public int CapNhatThuongHieu(int maTH, int maQG, string tenTH)
        {
            string sql = "UPDATE ThuongHieu SET MaQG = @MaQG, TenTH = @TenTH WHERE MaTH = @MaTH";
            var parameters = new[]
            {
                new SqlParameter("@MaQG", maQG),
                new SqlParameter("@TenTH", tenTH),
                new SqlParameter("@MaTH", maTH)
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

        public bool KiemTraCoTheSuaXoa(int maTH)
        {
            string sql = "SELECT COUNT(*) FROM SanPham WHERE MaTH = @MaTH";
            var parameters = new[] { new SqlParameter("@MaTH", maTH) };
            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters);
            return Convert.ToInt32(result) == 0;
        }

        public int XoaThuongHieu(int maTH)
        {
            string sql = "DELETE FROM ThuongHieu WHERE MaTH = @MaTH";
            var parameters = new[] { new SqlParameter("@MaTH", maTH) };
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