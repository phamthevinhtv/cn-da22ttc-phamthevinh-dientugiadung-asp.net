using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class TaiKhoanDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public TaiKhoanDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public DataTable LayTaiKhoanBangTenTK(string tentk, int quyen)
        {
            string sql = "";
            var parameters = new[]
            {
                new SqlParameter("@tentk", tentk)
            };

            if (quyen == 0)
            {
                sql = @"
                    SELECT 
                    kh.MaKH,
                    kh.TenKH,
                    kh.GioiTinhKH,
                    kh.SoDienThoaiKH,
                    kh.EmailKH,
                    tk.MaTK,
                    tk.TenTK,
                    tk.MatKhauTK,
                    tk.QuyenTK,
                    tk.TrangThaiTK
                    FROM KhachHang kh
                    LEFT JOIN TaiKhoan tk ON kh.MaKH = tk.MaKH
                    WHERE tk.TenTK = @tentk";
            }
            else if (quyen == 1)
            {
                sql = @"
                    SELECT 
                    MaTK,
                    TenTK,
                    QuyenTK,
                    MatKhauTK,
                    TrangThaiTK
                    FROM TaiKhoan
                    WHERE TenTK = @tentk";
            }

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public int TaoTaiKhoanKhachHang(TaiKhoanKhachHang taiKhoanKhachHangModel)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TenKH", taiKhoanKhachHangModel.KhachHang.TenKH),
                new SqlParameter("@GioiTinhKH", taiKhoanKhachHangModel.KhachHang.GioiTinhKH),
                new SqlParameter("@SoDienThoaiKH", taiKhoanKhachHangModel.KhachHang.SoDienThoaiKH),
                new SqlParameter("@EmailKH",
                    string.IsNullOrWhiteSpace(taiKhoanKhachHangModel.KhachHang.EmailKH)
                    ? DBNull.Value
                    : taiKhoanKhachHangModel.KhachHang.EmailKH),
                new SqlParameter("@MaTTP", taiKhoanKhachHangModel.DiaChiNhanHang.TinhThanhPho.MaTTP),
                new SqlParameter("@TenTTP", taiKhoanKhachHangModel.DiaChiNhanHang.TinhThanhPho.TenTTP),
                new SqlParameter("@MaXP", taiKhoanKhachHangModel.DiaChiNhanHang.XaPhuong.MaXP),
                new SqlParameter("@TenXP", taiKhoanKhachHangModel.DiaChiNhanHang.XaPhuong.TenXP),
                new SqlParameter("@TenDCCT", taiKhoanKhachHangModel.DiaChiNhanHang.DiaChiCuThe.TenDCCT)
            };

            return  _databaseHelper.ExecuteNonQuery(
                "SP_TaoTaiKhoanKhachHang",
                CommandType.StoredProcedure,
                parameters
            );
        }

        public int SuaThongTinKhachHang(TaiKhoanKhachHang taiKhoanKhachHang)
        {
            var parameters = new[]
            {
                new SqlParameter("@MaKH", taiKhoanKhachHang.KhachHang.MaKH),
                new SqlParameter("@TenKH", taiKhoanKhachHang.KhachHang.TenKH),
                new SqlParameter("@EmailKH", taiKhoanKhachHang.KhachHang.EmailKH),
                new SqlParameter("@GioiTinhKH", taiKhoanKhachHang.KhachHang.GioiTinhKH)
            };

            string sql = @"
                UPDATE KhachHang
                SET 
                    TenKH = @TenKH,
                    EmailKH = @EmailKH,
                    GioiTinhKH = @GioiTinhKH
                WHERE MaKH = @MaKH
            ";

            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
            } catch
            {
                return 0;
            }

            return 1;
        }

        public DataTable LayTaiKhoanBangMa(int maTK)
        {
            string sql = @"
                SELECT 
                MaTK,
                TenTK,
                QuyenTK,
                TrangThaiTK
                FROM TaiKhoan
                WHERE MaTK = @maTK";

            var parameters = new[]
            {
                new SqlParameter("@maTK", maTK)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public bool KiemTraTenDangNhapTonTai(string tenTK, int maTK = 0)
        {
            string sql = "SELECT COUNT(*) FROM TaiKhoan WHERE TenTK = @TenTK";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TenTK", tenTK)
            };

            if (maTK > 0)
            {
                sql += " AND MaTK != @MaTK";
                parameters.Add(new SqlParameter("@MaTK", maTK));
            }

            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters.ToArray());
            return Convert.ToInt32(result) > 0;
        }

        public int CapNhatTenDangNhap(int maTK, string tenTK)
        {
            string sql = "UPDATE TaiKhoan SET TenTK = @TenTK WHERE MaTK = @MaTK";
            var parameters = new[]
            {
                new SqlParameter("@MaTK", maTK),
                new SqlParameter("@TenTK", tenTK)
            };

            try
            {
                int rowsAffected = _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                return rowsAffected > 0 ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }

        public int CapNhatMatKhau(int maTK, string matKhau)
        {
            string sql = "UPDATE TaiKhoan SET MatKhauTK = @MatKhau WHERE MaTK = @MaTK";
            var parameters = new[]
            {
                new SqlParameter("@MaTK", maTK),
                new SqlParameter("@MatKhau", matKhau)
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
