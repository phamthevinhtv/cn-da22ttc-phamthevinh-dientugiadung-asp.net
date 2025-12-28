using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using Microsoft.Data.SqlClient;

namespace QL_DienTuGiaDung.DAL
{
    public class ThongKeDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public ThongKeDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<ThongKeDoanhThu> GetThongKeTheoNam()
        {
            string sql = @"
                SELECT 
                    YEAR(dh.NgayTaoDH) as Nam,
                    NULL as Quy,
                    NULL as Thang,
                    SUM(dh.TongTienDH) as DoanhThu,
                    COUNT(DISTINCT dh.MaDH) as SoDonHang,
                    SUM(bg.SoLuongDat) as SoSanPhamBan
                FROM DonHang dh
                INNER JOIN BaoGom bg ON dh.MaDH = bg.MaDH
                WHERE dh.MaTTDH = 5
                GROUP BY YEAR(dh.NgayTaoDH)
                ORDER BY YEAR(dh.NgayTaoDH) DESC";

            var datas = _databaseHelper.ExecuteQuery(sql);
            return DataHelper.MapToList<ThongKeDoanhThu>(datas);
        }

        public List<ThongKeDoanhThu> GetThongKeTheoQuy(int nam)
        {
            string sql = @"
                SELECT 
                    @Nam as Nam,
                    CASE 
                        WHEN MONTH(dh.NgayTaoDH) IN (1,2,3) THEN 1
                        WHEN MONTH(dh.NgayTaoDH) IN (4,5,6) THEN 2
                        WHEN MONTH(dh.NgayTaoDH) IN (7,8,9) THEN 3
                        WHEN MONTH(dh.NgayTaoDH) IN (10,11,12) THEN 4
                    END as Quy,
                    NULL as Thang,
                    SUM(dh.TongTienDH) as DoanhThu,
                    COUNT(DISTINCT dh.MaDH) as SoDonHang,
                    SUM(bg.SoLuongDat) as SoSanPhamBan
                FROM DonHang dh
                INNER JOIN BaoGom bg ON dh.MaDH = bg.MaDH
                WHERE dh.MaTTDH = 5 AND YEAR(dh.NgayTaoDH) = @Nam
                GROUP BY CASE 
                    WHEN MONTH(dh.NgayTaoDH) IN (1,2,3) THEN 1
                    WHEN MONTH(dh.NgayTaoDH) IN (4,5,6) THEN 2
                    WHEN MONTH(dh.NgayTaoDH) IN (7,8,9) THEN 3
                    WHEN MONTH(dh.NgayTaoDH) IN (10,11,12) THEN 4
                END
                ORDER BY Quy";

            var parameters = new[] { new SqlParameter("@Nam", nam) };
            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);
            return DataHelper.MapToList<ThongKeDoanhThu>(datas);
        }

        public List<ThongKeDoanhThu> GetThongKeTheoThang(int nam, int quy)
        {
            int thangBatDau = (quy - 1) * 3 + 1;
            int thangKetThuc = quy * 3;

            string sql = @"
                SELECT 
                    @Nam as Nam,
                    @Quy as Quy,
                    MONTH(dh.NgayTaoDH) as Thang,
                    SUM(dh.TongTienDH) as DoanhThu,
                    COUNT(DISTINCT dh.MaDH) as SoDonHang,
                    SUM(bg.SoLuongDat) as SoSanPhamBan
                FROM DonHang dh
                INNER JOIN BaoGom bg ON dh.MaDH = bg.MaDH
                WHERE dh.MaTTDH = 5 
                    AND YEAR(dh.NgayTaoDH) = @Nam
                    AND MONTH(dh.NgayTaoDH) BETWEEN @ThangBatDau AND @ThangKetThuc
                GROUP BY MONTH(dh.NgayTaoDH)
                ORDER BY MONTH(dh.NgayTaoDH)";

            var parameters = new[]
            {
                new SqlParameter("@Nam", nam),
                new SqlParameter("@Quy", quy),
                new SqlParameter("@ThangBatDau", thangBatDau),
                new SqlParameter("@ThangKetThuc", thangKetThuc)
            };

            var datas = _databaseHelper.ExecuteQuery(sql, parameters: parameters);
            return DataHelper.MapToList<ThongKeDoanhThu>(datas);
        }

        public List<int> GetAvailableYears()
        {
            string sql = @"
                SELECT DISTINCT YEAR(NgayTaoDH) as Nam
                FROM DonHang 
                WHERE MaTTDH = 5
                ORDER BY YEAR(NgayTaoDH) DESC";

            var datas = _databaseHelper.ExecuteQuery(sql);
            return datas.Select(row => Convert.ToInt32(row["Nam"])).ToList();
        }
    }
}