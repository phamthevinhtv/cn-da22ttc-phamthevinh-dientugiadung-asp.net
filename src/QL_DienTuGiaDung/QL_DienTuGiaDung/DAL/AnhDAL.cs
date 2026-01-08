using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class AnhDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public AnhDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public DataTable LayDanhSachAnhSanPham(int quyen, int masp)
        {
            string sql = "SELECT * FROM Anh WHERE MaSP = @masp";

            if (quyen == 1)
            {
                sql += " AND TrangThaiAnh = 1";
            }

            var parameters = new[]
            {
                new SqlParameter("@masp", masp)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public int XoaAnh(int maAnh)
        {
            string sql = "DELETE FROM Anh WHERE MaAnh = @MaAnh";
            var parameters = new[] { new SqlParameter("@MaAnh", maAnh) };
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

        public int ThemAnh(QL_DienTuGiaDung.Models.Anh anh)
        {
            string sql = "INSERT INTO Anh (MaSP, UrlAnh, MacDinhAnh, TrangThaiAnh) VALUES (@MaSP, @UrlAnh, @MacDinhAnh, @TrangThaiAnh)";
            var parameters = new[]
            {
                new SqlParameter("@MaSP", anh.MaSP),
                new SqlParameter("@UrlAnh", anh.UrlAnh),
                new SqlParameter("@MacDinhAnh", anh.MacDinhAnh),
                new SqlParameter("@TrangThaiAnh", 1)
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

        public int DatAnhMacDinh(int maAnh, int maSP)
        {
            try
            {
                // Bỏ mặc định của tất cả ảnh khác
                string sql1 = "UPDATE Anh SET MacDinhAnh = 0 WHERE MaSP = @MaSP";
                var parameters1 = new[] { new SqlParameter("@MaSP", maSP) };
                _databaseHelper.ExecuteNonQuery(sql1, parameters: parameters1);

                // Đặt ảnh được chọn làm mặc định
                string sql2 = "UPDATE Anh SET MacDinhAnh = 1 WHERE MaAnh = @MaAnh";
                var parameters2 = new[] { new SqlParameter("@MaAnh", maAnh) };
                _databaseHelper.ExecuteNonQuery(sql2, parameters: parameters2);

                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
