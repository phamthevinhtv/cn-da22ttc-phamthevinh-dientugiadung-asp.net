using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class DiaChiDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public DiaChiDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public DataTable LayDanhSachDiaChi(int maKH)
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
                WHERE dc.MaKH = @maKH
                ORDER BY dc.MacDinhDCCT DESC;";

            var parameters = new[]
            {
                new SqlParameter("@maKH", maKH)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public DataTable LayDiaChi(int maDC, int maKH)
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
                WHERE dc.MaDCCT = @maDC
                AND dc.MaKH = @maKH";

            var parameters = new[]
            {
                new SqlParameter("@maDC", maDC),
                new SqlParameter("@maKH", maKH)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public int ThemDiaChiNhanHang(DiaChiNhanHang diaChiNhanHang)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaKH", diaChiNhanHang.DiaChiCuThe.MaKH),
                new SqlParameter("@MaTTP", diaChiNhanHang.TinhThanhPho.MaTTP),
                new SqlParameter("@TenTTP", diaChiNhanHang.TinhThanhPho.TenTTP),
                new SqlParameter("@MaXP", diaChiNhanHang.XaPhuong.MaXP),
                new SqlParameter("@TenXP", diaChiNhanHang.XaPhuong.TenXP),
                new SqlParameter("@TenDCCT", diaChiNhanHang.DiaChiCuThe.TenDCCT),
                new SqlParameter("@MacDinhDCCT", diaChiNhanHang.DiaChiCuThe.MacDinhDCCT),
            };

            return  _databaseHelper.ExecuteNonQuery(
                "SP_ThemDiaChiNhanHang",
                CommandType.StoredProcedure,
                parameters
            );
        }

        public int SuaDiaChiNhanHang(DiaChiNhanHang diaChiNhanHang)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDCCT", diaChiNhanHang.DiaChiCuThe.MaDCCT),
                new SqlParameter("@MaKH", diaChiNhanHang.DiaChiCuThe.MaKH),
                new SqlParameter("@MaTTP", diaChiNhanHang.TinhThanhPho.MaTTP),
                new SqlParameter("@TenTTP", diaChiNhanHang.TinhThanhPho.TenTTP),
                new SqlParameter("@MaXP", diaChiNhanHang.XaPhuong.MaXP),
                new SqlParameter("@TenXP", diaChiNhanHang.XaPhuong.TenXP),
                new SqlParameter("@TenDCCT", diaChiNhanHang.DiaChiCuThe.TenDCCT),
                new SqlParameter("@MacDinhDCCT", diaChiNhanHang.DiaChiCuThe.MacDinhDCCT),
            };
            
            try
            {
                _databaseHelper.ExecuteNonQuery(
                    "SP_SuaDiaChiNhanHang",
                    CommandType.StoredProcedure,
                    parameters
                );
            } catch
            {
                return 0;
            }

            return 1;
        }

        public int XoaDiaChi(int maDC)
        {
            var parameters = new[]
            {
                new SqlParameter("@maDC", maDC)
            };

            string sql = @"
                DELETE FROM DiaChiCuThe
                WHERE MaDCCT = @maDC
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

        public int DatDiaChiMacDinh(int MaKH, int MaDCCT)
        {
            var parameters = new[]
            {
                new SqlParameter("@MaKH", MaKH)
            };

            var parameters1 = new[]
            {
                new SqlParameter("@MaKH", MaKH),
                new SqlParameter("@MaDCCT", MaDCCT)
            };

            string sql = @"
                UPDATE DiaChiCuThe
                SET MacDinhDCCT = 0
                WHERE MaKH = @MaKH;
            ";

            string sql1 = @"
                UPDATE DiaChiCuThe
                SET MacDinhDCCT = 1
                WHERE MaDCCT = @MaDCCT
                AND MaKH = @MaKH;
            ";
            
            try
            {
                _databaseHelper.ExecuteNonQuery(sql, parameters: parameters);
                _databaseHelper.ExecuteNonQuery(sql1, parameters: parameters1);
            } catch
            {
                return 0;
            }

            return 1;
        }
    }
}
