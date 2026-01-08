using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class DonHangBLL
    {
        private readonly DonHangDAL _donHangDAL;
        private readonly SanPhamBLL _sanPhamBLL;

        public DonHangBLL(DonHangDAL donHangDAL, SanPhamBLL sanPhamBLL)
        {
            _donHangDAL = donHangDAL;
            _sanPhamBLL = sanPhamBLL;
        }

        private static void MapDonHang(DonHang donHang, DataRow row)
        {
            donHang.MaDH = row["MaDH"] != DBNull.Value ? Convert.ToInt32(row["MaDH"]) : 0;
            donHang.MaPTTT = row["MaPTTT"] != DBNull.Value ? Convert.ToInt32(row["MaPTTT"]) : 0;
            donHang.PhiVanChuyenDH = row["PhiVanChuyenDH"] != DBNull.Value ? Convert.ToDecimal(row["PhiVanChuyenDH"]) : 0;
            donHang.TongTienDH = row["TongTienDH"] != DBNull.Value ? Convert.ToDecimal(row["TongTienDH"]) : 0;
            donHang.MaTTTT = row["MaTTTT"] != DBNull.Value ? Convert.ToInt32(row["MaTTTT"]) : 0;
            donHang.MaTTDH = row["MaTTDH"] != DBNull.Value ? Convert.ToInt32(row["MaTTDH"]) : 0;
            donHang.NgayTaoDH = row["NgayTaoDH"] != DBNull.Value ? Convert.ToDateTime(row["NgayTaoDH"]) : DateTime.MinValue;
            donHang.NgayCapNhatDH = row["NgayCapNhatDH"] != DBNull.Value ? Convert.ToDateTime(row["NgayCapNhatDH"]) : DateTime.MinValue;
            donHang.NgayCapNhatTT = row["NgayCapNhatTT"] != DBNull.Value ? Convert.ToDateTime(row["NgayCapNhatTT"]) : DateTime.MinValue;
            donHang.PhuongThucThanhToan.MaPTTT = row["MaPTTT"] != DBNull.Value ? Convert.ToInt32(row["MaPTTT"]) : 0;
            donHang.PhuongThucThanhToan.TenPTTT = row["TenPTTT"] != DBNull.Value ? row["TenPTTT"].ToString()! : string.Empty;
            donHang.TrangThaiDonHang.MaTTDH = row["MaTTDH"] != DBNull.Value ? Convert.ToInt32(row["MaTTDH"]) : 0;
            donHang.TrangThaiDonHang.TenTTDH = row["TenTTDH"] != DBNull.Value ? row["TenTTDH"].ToString()! : string.Empty;
            donHang.TrangThaiThanhToan.MaTTTT = row["MaTTTT"] != DBNull.Value ? Convert.ToInt32(row["MaTTTT"]) : 0;
            donHang.TrangThaiThanhToan.TenTTTT = row["TenTTTT"] != DBNull.Value ? row["TenTTTT"].ToString()! : string.Empty;
            donHang.DiaChiNhanHang.DiaChiCuThe.TenDCCT = row["TenDCCT"] != DBNull.Value ? row["TenDCCT"].ToString()! : string.Empty;
            donHang.DiaChiNhanHang.XaPhuong.TenXP = row["TenXP"] != DBNull.Value ? row["TenXP"].ToString()! : string.Empty;
            donHang.DiaChiNhanHang.TinhThanhPho.TenTTP = row["TenTTP"] != DBNull.Value ? row["TenTTP"].ToString()! : string.Empty;
            
            donHang.TenDCCT = row["TenDCCT"] != DBNull.Value ? row["TenDCCT"].ToString()! : string.Empty;
            donHang.TenXP = row["TenXP"] != DBNull.Value ? row["TenXP"].ToString()! : string.Empty;
            donHang.TenTTP = row["TenTTP"] != DBNull.Value ? row["TenTTP"].ToString()! : string.Empty;
        }

        public int DatHang(DonHang donHang)
        {
            return _donHangDAL.DatHang(donHang);
        }

        public List<DonHang> LayDanhSachDonHangBangMaKH(int maKH)
        {
            List<DonHang> list = new();

            var table = _donHangDAL.LayDanhSachDonHangBangMaKH(maKH);

            foreach (DataRow row in table.Rows)
            {
                DonHang donHang = new DonHang();
                MapDonHang(donHang, row);
                donHang.ListSanPhamDatHang = _sanPhamBLL.LayDanhSachSanPhamTrongDonHang(donHang.MaDH);
                list.Add(donHang);
            }

            return list;
        }

        public int HuyDonHang(int maDH)
        {
            return _donHangDAL.HuyDonHang(maDH);
        }

        public List<DonHang> LayDanhSachDonHang(int quyen)
        {
            List<DonHang> list = new();
            var table = _donHangDAL.LayDanhSachDonHang(quyen);

            foreach (DataRow row in table.Rows)
            {
                DonHang donHang = new DonHang();
                MapDonHang(donHang, row);
                if (row.Table.Columns.Contains("SoDienThoaiKH"))
                {
                    donHang.SoDienThoaiKH = row["SoDienThoaiKH"] != DBNull.Value ? row["SoDienThoaiKH"].ToString()! : string.Empty;
                    donHang.KhachHang.SoDienThoaiKH = donHang.SoDienThoaiKH;
                }
                if (row.Table.Columns.Contains("TenKH"))
                {
                    donHang.TenKH = row["TenKH"] != DBNull.Value ? row["TenKH"].ToString()! : string.Empty;
                    donHang.KhachHang.TenKH = donHang.TenKH;
                }
                donHang.ListSanPhamDatHang = _sanPhamBLL.LayDanhSachSanPhamTrongDonHang(donHang.MaDH);
                list.Add(donHang);
            }

            return list;
        }

        public List<TrangThaiDonHang> LayDanhSachTrangThaiDonHang()
        {
            List<TrangThaiDonHang> list = new();
            var table = _donHangDAL.LayDanhSachTrangThaiDonHang();

            foreach (DataRow row in table.Rows)
            {
                TrangThaiDonHang trangThai = new TrangThaiDonHang
                {
                    MaTTDH = row["MaTTDH"] != DBNull.Value ? Convert.ToInt32(row["MaTTDH"]) : 0,
                    TenTTDH = row["TenTTDH"] != DBNull.Value ? row["TenTTDH"].ToString()! : string.Empty
                };
                list.Add(trangThai);
            }

            return list;
        }

        public int CapNhatTrangThaiDonHang(int maDH, int maTTDH)
        {
            return _donHangDAL.CapNhatTrangThaiDonHang(maDH, maTTDH);
        }

        public List<TrangThaiThanhToan> LayDanhSachTrangThaiThanhToan()
        {
            List<TrangThaiThanhToan> list = new();
            var table = _donHangDAL.LayDanhSachTrangThaiThanhToan();

            foreach (DataRow row in table.Rows)
            {
                TrangThaiThanhToan trangThai = new TrangThaiThanhToan
                {
                    MaTTTT = row["MaTTTT"] != DBNull.Value ? Convert.ToInt32(row["MaTTTT"]) : 0,
                    TenTTTT = row["TenTTTT"] != DBNull.Value ? row["TenTTTT"].ToString()! : string.Empty
                };
                list.Add(trangThai);
            }

            return list;
        }

        public int CapNhatTrangThaiThanhToan(int maDH, int maTTTT, string maGiaoDichTT)
        {
            return _donHangDAL.CapNhatTrangThaiThanhToan(maDH, maTTTT, maGiaoDichTT);
        }
    }
}
