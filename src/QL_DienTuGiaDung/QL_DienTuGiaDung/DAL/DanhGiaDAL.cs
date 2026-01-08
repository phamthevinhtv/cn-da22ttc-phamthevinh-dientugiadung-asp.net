using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class DanhGiaDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public DanhGiaDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public DataTable LayDanhSachDanhGia(int masp)
        {
            string sql = @"
                SELECT 
                dg.MaSP,
                dg.MaKH,
                dg.DiemDG,
                dg.NhanXetDG,
                dg.NgayTaoDG,
                dg.NgayCapNhatDG,
                kh.TenKH
                FROM DanhGia dg 
                JOIN KhachHang kh ON dg.MaKH = kh.MaKH
                WHERE MaSP = @masp";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayDanhGia(int masp, int maKH)
        {
            string sql = @"
                SELECT 
                dg.MaSP,
                dg.MaKH,
                dg.DiemDG,
                dg.NhanXetDG,
                dg.NgayTaoDG,
                dg.NgayCapNhatDG,
                kh.TenKH
                FROM DanhGia dg 
                JOIN KhachHang kh ON dg.MaKH = kh.MaKH
                WHERE dg.MaSP = @masp AND dg.MaKH = @maKH";

            var parameters = new[]
            {
                new SqlParameter("@masp", masp),
                new SqlParameter("@maKH", maKH),
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public int TaoDanhGia(DanhGia danhGia)
        {
            var parameters = new[]
            {
                new SqlParameter("@MaSP", danhGia.MaSP),
                new SqlParameter("@MaKH", danhGia.MaKH),
                new SqlParameter("@DiemDG", danhGia.DiemDG),
                new SqlParameter("@NhanXetDG", (object?)danhGia.NhanXetDG ?? DBNull.Value)
            };

            string sql = @"
                INSERT INTO DanhGia (MaSP, MaKH, DiemDG, NhanXetDG)
                VALUES (@MaSP, @MaKH, @DiemDG, @NhanXetDG)
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

        public int SuaDanhGia(DanhGia danhGia)
        {
            var parameters = new[]
            {
                new SqlParameter("@MaSP", danhGia.MaSP),
                new SqlParameter("@MaKH", danhGia.MaKH),
                new SqlParameter("@DiemDG", danhGia.DiemDG),
                new SqlParameter("@NhanXetDG", (object?)danhGia.NhanXetDG ?? DBNull.Value)
            };

            string sql = @"
                UPDATE DanhGia SET DiemDG = @DiemDG, NhanXetDG = @NhanXetDG, NgayCapNhatDG = GETDATE()
                WHERE MaSP = @MaSP AND MaKH = @MaKH 
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
    }
}
