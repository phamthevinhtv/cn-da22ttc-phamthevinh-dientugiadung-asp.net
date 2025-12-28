using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.DAL
{
    public class DeliveryAddressDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public DeliveryAddressDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<DiaChiDayDu>? GetAddressCustomer(string phone)
        {
            string sql = 
                @"SELECT 
                dc.MaDCCT,
                dc.TenDCCT,
                dc.MacDinhDCCT,
                xp.MaXP,
                xp.TenXP,
                ttp.MaTTP,
                ttp.TenTTP,
                CASE 
                    WHEN EXISTS (
                        SELECT 1 
                        FROM DonHang dh 
                        WHERE dh.MaDCCT = dc.MaDCCT
                    ) 
                    THEN 0 
                    ELSE 1 
                END AS DuocSuaXoa
                FROM DiaChiCuThe dc
                JOIN XaPhuong xp ON dc.MaXP = xp.MaXP
                JOIN TinhThanhPho ttp ON xp.MaTTP = ttp.MaTTP
                JOIN KhachHang kh ON kh.MaKH = dc.MaKH
                WHERE kh.SoDienThoaiKH = @phone
                ORDER BY dc.MacDinhDCCT DESC;";

            var data = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@phone", phone) });

            var address = DataHelper.MapToList<DiaChiDayDu>(data);

            return address;
        }

        public void DeleteAddress(int MaDCCT)
        {
            var parameters = new[]
            {
                new SqlParameter("@madcct", MaDCCT)
            };

            _databaseHelper.ExecuteNonQuery("DELETE FROM DiaChiCuThe WHERE MaDCCT = @madcct", parameters:parameters);
        }
    }
}