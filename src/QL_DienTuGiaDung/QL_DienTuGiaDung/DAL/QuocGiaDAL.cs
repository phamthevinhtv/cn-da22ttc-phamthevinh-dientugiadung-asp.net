using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using Microsoft.Data.SqlClient;

namespace QL_DienTuGiaDung.DAL
{
    public class QuocGiaDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public QuocGiaDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        
        public List<QuocGia> GetAllQuocGia()
        {
            string sql = "SELECT MaQG, TenQG FROM QuocGia";

            var datas = _databaseHelper.ExecuteQuery(sql);

            var quocGias = DataHelper.MapToList<QuocGia>(datas);

            return quocGias;
        }

        public List<QuocGia> GetAllQuocGiaForAdmin()
        {
            string sql = "SELECT MaQG, TenQG FROM QuocGia ORDER BY TenQG";
            var datas = _databaseHelper.ExecuteQuery(sql);
            return DataHelper.MapToList<QuocGia>(datas);
        }

        public QuocGia? GetQuocGiaById(int id)
        {
            string sql = "SELECT MaQG, TenQG FROM QuocGia WHERE MaQG = @MaQG";
            var parameters = new[] { new SqlParameter("@MaQG", id) };
            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);
            return DataHelper.MapToList<QuocGia>(datas).FirstOrDefault();
        }

        public void CreateQuocGia(QuocGia quocGia)
        {
            string sql = "INSERT INTO QuocGia (TenQG) VALUES (@TenQG)";
            var parameters = new[] { new SqlParameter("@TenQG", quocGia.TenQG) };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }

        public void UpdateQuocGia(QuocGia quocGia)
        {
            string sql = "UPDATE QuocGia SET TenQG = @TenQG WHERE MaQG = @MaQG";
            var parameters = new[]
            {
                new SqlParameter("@TenQG", quocGia.TenQG),
                new SqlParameter("@MaQG", quocGia.MaQG)
            };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }

        public bool CanDeleteQuocGia(int id)
        {
            string sql = @"
                SELECT COUNT(*) FROM (
                    SELECT MaQG FROM ThuongHieu WHERE MaQG = @MaQG
                    UNION ALL
                    SELECT MaQG FROM SanPham WHERE MaQG = @MaQG
                ) AS UsedCountries";
            var parameters = new[] { new SqlParameter("@MaQG", id) };
            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters);
            return Convert.ToInt32(result) == 0;
        }

        public void DeleteQuocGia(int id)
        {
            string sql = "DELETE FROM QuocGia WHERE MaQG = @MaQG";
            var parameters = new[] { new SqlParameter("@MaQG", id) };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }
    }
}