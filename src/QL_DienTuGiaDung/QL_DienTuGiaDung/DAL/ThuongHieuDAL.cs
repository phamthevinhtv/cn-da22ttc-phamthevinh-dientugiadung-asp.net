using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using Microsoft.Data.SqlClient;

namespace QL_DienTuGiaDung.DAL
{
    public class ThuongHieuDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public ThuongHieuDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        
        public List<ThuongHieu> GetAllThuongHieu()
        {
            string sql = "SELECT MaTH, TenTH FROM ThuongHieu";

            var datas = _databaseHelper.ExecuteQuery(sql);

            var thuongHieus = DataHelper.MapToList<ThuongHieu>(datas);

            return thuongHieus;
        }

        public List<ThuongHieu> GetAllThuongHieuForAdmin()
        {
            string sql = @"
                SELECT th.MaTH, th.MaQG, th.TenTH, qg.TenQG 
                FROM ThuongHieu th
                JOIN QuocGia qg ON th.MaQG = qg.MaQG
                ORDER BY th.TenTH";
            var datas = _databaseHelper.ExecuteQuery(sql);
            return DataHelper.MapToList<ThuongHieu>(datas);
        }

        public ThuongHieu? GetThuongHieuById(int id)
        {
            string sql = @"
                SELECT th.MaTH, th.MaQG, th.TenTH, qg.TenQG 
                FROM ThuongHieu th
                JOIN QuocGia qg ON th.MaQG = qg.MaQG
                WHERE th.MaTH = @MaTH";
            var parameters = new[] { new SqlParameter("@MaTH", id) };
            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);
            return DataHelper.MapToList<ThuongHieu>(datas).FirstOrDefault();
        }

        public void CreateThuongHieu(ThuongHieu thuongHieu)
        {
            string sql = "INSERT INTO ThuongHieu (MaQG, TenTH) VALUES (@MaQG, @TenTH)";
            var parameters = new[]
            {
                new SqlParameter("@MaQG", thuongHieu.MaQG),
                new SqlParameter("@TenTH", thuongHieu.TenTH)
            };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }

        public void UpdateThuongHieu(ThuongHieu thuongHieu)
        {
            string sql = "UPDATE ThuongHieu SET MaQG = @MaQG, TenTH = @TenTH WHERE MaTH = @MaTH";
            var parameters = new[]
            {
                new SqlParameter("@MaQG", thuongHieu.MaQG),
                new SqlParameter("@TenTH", thuongHieu.TenTH),
                new SqlParameter("@MaTH", thuongHieu.MaTH)
            };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }

        public bool CanDeleteThuongHieu(int id)
        {
            string sql = "SELECT COUNT(*) FROM SanPham WHERE MaTH = @MaTH";
            var parameters = new[] { new SqlParameter("@MaTH", id) };
            var result = _databaseHelper.ExecuteScalar(sql, parameters: parameters);
            return Convert.ToInt32(result) == 0;
        }

        public void DeleteThuongHieu(int id)
        {
            string sql = "DELETE FROM ThuongHieu WHERE MaTH = @MaTH";
            var parameters = new[] { new SqlParameter("@MaTH", id) };
            _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
        }
    }
}