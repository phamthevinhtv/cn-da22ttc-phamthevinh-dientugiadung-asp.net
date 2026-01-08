using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.DAL
{
    public class DonHangDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public DonHangDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public int DatHang(DonHang donHang)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaKH", SqlDbType.VarChar, 10)
                {
                    Value = donHang.KhachHang.MaKH
                },
                new SqlParameter("@MaDCCT", SqlDbType.Int)
                {
                    Value = donHang.DiaChiNhanHang.DiaChiCuThe.MaDCCT
                },
                new SqlParameter("@PhiVanChuyenDH", SqlDbType.Decimal)
                {
                    Precision = 10,
                    Scale = 0,
                    Value = donHang.PhiVanChuyenDH
                }
            };

            DataTable dtSanPhamDatHang = new DataTable();
            dtSanPhamDatHang.Columns.Add("MaSP", typeof(int));
            dtSanPhamDatHang.Columns.Add("SoLuongDat", typeof(int));

            foreach (var item in donHang.ListSanPhamDatHang)
            {
                dtSanPhamDatHang.Rows.Add(
                    item.MaSP,
                    item.SoLuongDat
                );
            }

            var tvpParam = new SqlParameter("@DanhSachSP", SqlDbType.Structured)
            {
                TypeName = "TVP_DanhSachDatHang",
                Value = dtSanPhamDatHang
            };
            parameters.Add(tvpParam);

            return _databaseHelper.ExecuteNonQuery(
                "Proc_DatHang",
                CommandType.StoredProcedure,
                parameters.ToArray()
            );
        }

        public DataTable LayDanhSachDonHangBangMaKH(int maKH)
        {
            string sql = @"
                SELECT 
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
                WHERE dh.MaKH = @maKH
                ORDER BY dh.NgayTaoDH DESC
            ";

            var parameters = new[]
            {
                new SqlParameter("@maKH", maKH)
            };

            return _databaseHelper.ExecuteDataTable(sql, parameters: parameters);
        }

        public int HuyDonHang(int maDH)
        {
            var parameters = new[]
            {
                new SqlParameter("@maDH", maDH)
            };

            var parameters1 = new[]
            {
                new SqlParameter("@maDH", maDH)
            };

            string sql = @"
                UPDATE sp
                SET sp.SoLuongSP = sp.SoLuongSP + bg.SoLuongDat,
                    sp.NgayCapNhatSP = GETDATE()
                FROM SanPham sp
                JOIN BaoGom bg ON bg.MaSP = sp.MaSP
                JOIN DonHang dh ON dh.MaDH = bg.MaDH 
                WHERE dh.MaDH = @maDH
                AND dh.MaTTDH <> 6;
            ";

            string sql1 = @"
                UPDATE DonHang
                SET MaTTDH = 6,
                NgayCapNhatDH = GETDATE()
                WHERE MaDH = @maDH
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

        /* Không động vào code trên dòng này trở lên trên đầu */
        /* Làm từ đây trở xuống */
        public DataTable LayDanhSachDonHang(int quyen)
        {
            string sql = @"
                SELECT 
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
                JOIN TrangThaiThanhToan tttt ON tt.MaTTTT = tttt.MaTTTT";
            
            if (quyen == 0)
            {
                sql += " WHERE dh.MaTTDH <> 6";
            }
            
            sql += " ORDER BY dh.NgayTaoDH DESC";

            return _databaseHelper.ExecuteDataTable(sql);
        }

        public DataTable LayDanhSachTrangThaiDonHang()
        {
            string sql = "SELECT MaTTDH, TenTTDH FROM TrangThaiDonHang ORDER BY MaTTDH ASC";
            return _databaseHelper.ExecuteDataTable(sql);
        }

        public int CapNhatTrangThaiDonHang(int maDH, int maTTDH)
        {
            string sql = "UPDATE DonHang SET MaTTDH = @MaTTDH, NgayCapNhatDH = GETDATE() WHERE MaDH = @MaDH";
            var parameters = new[]
            {
                new SqlParameter("@MaDH", maDH),
                new SqlParameter("@MaTTDH", maTTDH)
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

        public DataTable LayDanhSachTrangThaiThanhToan()
        {
            string sql = "SELECT MaTTTT, TenTTTT FROM TrangThaiThanhToan ORDER BY MaTTTT ASC";
            return _databaseHelper.ExecuteDataTable(sql);
        }

        public int CapNhatTrangThaiThanhToan(int maDH, int maTTTT, string maGiaoDichTT)
        {
            string sql = "UPDATE ThanhToan SET MaTTTT = @MaTTTT, MaGiaoDichTT = @MaGiaoDichTT, NgayCapNhatTT = GETDATE() WHERE MaDH = @MaDH";
            var parameters = new[]
            {
                new SqlParameter("@MaDH", maDH),
                new SqlParameter("@MaTTTT", maTTTT),
                new SqlParameter("@MaGiaoDichTT", maGiaoDichTT)
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
