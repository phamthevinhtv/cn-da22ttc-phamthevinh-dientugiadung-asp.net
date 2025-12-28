using System.Data;
using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.DAL
{
    public class OrderDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public OrderDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public void DatHang(DonHang donHang)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@SoDienThoaiKH", SqlDbType.VarChar, 10)
                {
                    Value = donHang.SoDienThoaiKH
                },
                new SqlParameter("@MaDCCT", SqlDbType.Int)
                {
                    Value = donHang.MaDCCT
                },
                new SqlParameter("@MaPTTT", SqlDbType.Int)
                {
                    Value = donHang.MaPTTT
                },
                new SqlParameter("@PhiVanChuyenDH", SqlDbType.Decimal)
                {
                    Precision = 10,
                    Scale = 0,
                    Value = donHang.PhiVanChuyenDH
                },
                new SqlParameter("@MaTTTT", SqlDbType.Int)
                {
                    Value = (object?)donHang.MaTTTT ?? DBNull.Value
                }
            };

            var tvpParam = new SqlParameter("@DanhSachSP", SqlDbType.Structured)
            {
                TypeName = "TVP_DanhSachDatHang",
                Value = DataHelper.MapToDataTable(donHang.ListSanPhamDatHang)
            };
            parameters.Add(tvpParam);

            _databaseHelper.ExecuteNonQuery(
                "Proc_DatHang",
                CommandType.StoredProcedure,
                parameters.ToArray()
            );
        }

        public List<DonHang> GetOrdersByPhone(string phone)
        {
            string sql = 
                @"SELECT 
                dh.MaDH,
                dh.NgayTaoDH,
                dh.NgayCapNhatDH,
                dh.PhiVanChuyenDH,
                dh.TongTienDH,
                dc.TenDCCT,
                xp.TenXP,
                ttp.TenTTP,
                pttt.TenPTTT,
                ttdh.TenTTDH,
                tttt.TenTTTT,
                pttt.MaPTTT,
                ttdh.MaTTDH,
                tttt.MaTTTT,
                tt.NgayCapNhatTT
                FROM DonHang dh
                JOIN KhachHang kh ON dh.MaKH = kh.MaKH
                JOIN DiaChiCuThe dc ON dh.MaDCCT = dc.MaDCCT
                JOIN XaPhuong xp ON dc.MaXP = xp.MaXP
                JOIN TinhThanhPho ttp ON xp.MaTTP = ttp.MaTTP
                JOIN PhuongThucThanhToan pttt ON dh.MaPTTT = pttt.MaPTTT
                JOIN TrangThaiDonHang ttdh ON dh.MaTTDH = ttdh.MaTTDH
                JOIN ThanhToan tt ON dh.MaDH = tt.MaDH
                JOIN TrangThaiThanhToan tttt ON tt.MaTTTT = tttt.MaTTTT
                WHERE kh.SoDienThoaiKH = @phone
                ORDER BY dh.NgayTaoDH DESC";

            var data = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@phone", phone) });

            var orders = DataHelper.MapToList<DonHang>(data);

            return orders;
        }

        public List<DonHang> GetOrders()
        {
            string sql = 
                @"SELECT 
                dh.MaDH,
                dh.NgayTaoDH,
                dh.NgayCapNhatDH,
                dh.PhiVanChuyenDH,
                dh.TongTienDH,
                dc.TenDCCT,
                xp.TenXP,
                ttp.TenTTP,
                pttt.TenPTTT,
                ttdh.TenTTDH,
                tttt.TenTTTT,
                pttt.MaPTTT,
                ttdh.MaTTDH,
                tttt.MaTTTT,
                tt.NgayCapNhatTT,
                kh.SoDienThoaiKH,
                kh.TenKH
                FROM DonHang dh
                JOIN KhachHang kh ON dh.MaKH = kh.MaKH
                JOIN DiaChiCuThe dc ON dh.MaDCCT = dc.MaDCCT
                JOIN XaPhuong xp ON dc.MaXP = xp.MaXP
                JOIN TinhThanhPho ttp ON xp.MaTTP = ttp.MaTTP
                JOIN PhuongThucThanhToan pttt ON dh.MaPTTT = pttt.MaPTTT
                JOIN TrangThaiDonHang ttdh ON dh.MaTTDH = ttdh.MaTTDH
                JOIN ThanhToan tt ON dh.MaDH = tt.MaDH
                JOIN TrangThaiThanhToan tttt ON tt.MaTTTT = tttt.MaTTTT
                ORDER BY dh.NgayTaoDH DESC";

            var data = _databaseHelper.ExecuteQuery(sql);

            var orders = DataHelper.MapToList<DonHang>(data);

            return orders;
        }

        public List<SanPhamDatHang> GetProductsInOrder(int orderId)
        {
             string sql = 
                @"SELECT
                bg.MaSP,
                bg.SoLuongDat,
                sp.TenSP,
                bg.GiaDat * (1 - bg.MucGiamGiaDat / 100) * (1 + bg.ThueGTGTDat / 100) AS GiaDatSauGiamVaThue,
                asp.UrlAnh
                FROM BaoGom bg
                JOIN SanPham sp ON sp.MaSP = bg.MaSP
                LEFT JOIN Anh asp 
                ON asp.MaSP = sp.MaSP AND asp.MacDinhAnh = 1
                WHERE bg.MaDH = @orderId";

            var data = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@orderId", orderId) });

            var products = DataHelper.MapToList<SanPhamDatHang>(data);

            return products;
        }

        public void CancelOrder(int orderId)
        {
            var parameters = new[]
            {
                new SqlParameter("@orderId", orderId)
            };

            _databaseHelper.ExecuteNonQuery("UPDATE DonHang SET MaTTDH = 6, NgayCapNhatDH = GETDATE() WHERE MaDH = @orderId", parameters:parameters);
        }

        public List<TrangThaiDonHang> GetAllTrangThaiDonHang()
        {
            string sql = "SELECT MaTTDH, TenTTDH FROM TrangThaiDonHang ORDER BY MaTTDH ASC";

            var datas = _databaseHelper.ExecuteQuery(sql);

            var trangThai = DataHelper.MapToList<TrangThaiDonHang>(datas);

            return trangThai;
        }

        public void UpdateTrangThaiDonHang(int MaDH, int MaTTDH)
        {
            var parameters = new[]
            {
                new SqlParameter("@MaDH", MaDH),
                new SqlParameter("@MaTTDH", MaTTDH)
            };
            
            _databaseHelper.ExecuteQuery("UPDATE DonHang SET MaTTDH = @MaTTDH, NgayCapNhatDH = GETDATE() WHERE MaDH = @MaDH", parameters: parameters);
        }

        public List<TrangThaiThanhToan> GetAllTrangThaiThanhToan()
        {
            string sql = "SELECT MaTTTT, TenTTTT FROM TrangThaiThanhToan ORDER BY MaTTTT ASC";

            var datas = _databaseHelper.ExecuteQuery(sql);

            var trangThai = DataHelper.MapToList<TrangThaiThanhToan>(datas);

            return trangThai;
        }

        public void UpdateTrangThaiThanhToan(int MaDH, int MaTTTT, string MaGiaoDichTT)
        {
            var parameters = new[]
            {
                new SqlParameter("@MaDH", MaDH),
                new SqlParameter("@MaTTTT", MaTTTT),
                new SqlParameter("@MaGiaoDichTT", MaGiaoDichTT)
            };
            
            _databaseHelper.ExecuteQuery("UPDATE ThanhToan SET MaTTTT = @MaTTTT, MaGiaoDichTT = @MaGiaoDichTT, NgayCapNhatTT = GETDATE() WHERE MaDH = @MaDH", parameters: parameters);
        }
    }
}