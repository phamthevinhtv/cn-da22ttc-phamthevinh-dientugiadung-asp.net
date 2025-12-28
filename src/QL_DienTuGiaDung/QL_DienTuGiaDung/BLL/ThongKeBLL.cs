using QL_DienTuGiaDung.Models;
using QL_DienTuGiaDung.DAL;

namespace QL_DienTuGiaDung.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL _thongKeDAL;

        public ThongKeBLL(ThongKeDAL thongKeDAL)
        {
            _thongKeDAL = thongKeDAL;
        }

        public List<ThongKeDoanhThu> GetThongKeDoanhThu(ThongKeFilter filter)
        {
            switch (filter.LoaiThongKe.ToLower())
            {
                case "quy":
                    if (filter.Nam.HasValue)
                        return _thongKeDAL.GetThongKeTheoQuy(filter.Nam.Value);
                    break;
                case "thang":
                    if (filter.Nam.HasValue && filter.Quy.HasValue)
                        return _thongKeDAL.GetThongKeTheoThang(filter.Nam.Value, filter.Quy.Value);
                    break;
                default:
                    return _thongKeDAL.GetThongKeTheoNam();
            }
            return new List<ThongKeDoanhThu>();
        }

        public List<int> GetAvailableYears()
        {
            return _thongKeDAL.GetAvailableYears();
        }

        public decimal GetTongDoanhThu(List<ThongKeDoanhThu> data)
        {
            return data.Sum(x => x.DoanhThu);
        }

        public int GetTongDonHang(List<ThongKeDoanhThu> data)
        {
            return data.Sum(x => x.SoDonHang);
        }

        public int GetTongSanPhamBan(List<ThongKeDoanhThu> data)
        {
            return data.Sum(x => x.SoSanPhamBan);
        }
    }
}