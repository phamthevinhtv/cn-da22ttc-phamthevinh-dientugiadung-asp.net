using QL_DienTuGiaDung.Models;
using QL_DienTuGiaDung.DAL;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL _thongKeDAL;

        public ThongKeBLL(ThongKeDAL thongKeDAL)
        {
            _thongKeDAL = thongKeDAL;
        }

        public List<ThongKeDoanhThu> LayThongKeDoanhThu(ThongKeFilter boLoc)
        {
            DataTable dataTable;
            
            switch (boLoc.LoaiThongKe.ToLower())
            {
                case "quy":
                    if (boLoc.Nam.HasValue)
                        dataTable = _thongKeDAL.LayThongKeTheoQuy(boLoc.Nam.Value);
                    else
                        return new List<ThongKeDoanhThu>();
                    break;
                case "thang":
                    if (boLoc.Nam.HasValue && boLoc.Quy.HasValue)
                        dataTable = _thongKeDAL.LayThongKeTheoThang(boLoc.Nam.Value, boLoc.Quy.Value);
                    else
                        return new List<ThongKeDoanhThu>();
                    break;
                default:
                    dataTable = _thongKeDAL.LayThongKeTheoNam();
                    break;
            }

            return MapDataTableToThongKeDoanhThu(dataTable);
        }

        public List<int> LayCacNamTonTai()
        {
            var dataTable = _thongKeDAL.LayCacNamTonTai();
            var result = new List<int>();

            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Nam"] != DBNull.Value)
                {
                    result.Add(Convert.ToInt32(row["Nam"]));
                }
            }

            return result;
        }

        private List<ThongKeDoanhThu> MapDataTableToThongKeDoanhThu(DataTable dataTable)
        {
            var result = new List<ThongKeDoanhThu>();

            foreach (DataRow row in dataTable.Rows)
            {
                var thongKe = new ThongKeDoanhThu
                {
                    Nam = row["Nam"] != DBNull.Value ? Convert.ToInt32(row["Nam"]) : 0,
                    Quy = row["Quy"] != DBNull.Value ? Convert.ToInt32(row["Quy"]) : (int?)null,
                    Thang = row["Thang"] != DBNull.Value ? Convert.ToInt32(row["Thang"]) : (int?)null,
                    DoanhThu = row["DoanhThu"] != DBNull.Value ? Convert.ToDecimal(row["DoanhThu"]) : 0m,
                    SoDonHang = row["SoDonHang"] != DBNull.Value ? Convert.ToInt32(row["SoDonHang"]) : 0,
                    SoSanPhamBan = row["SoSanPhamBan"] != DBNull.Value ? Convert.ToInt32(row["SoSanPhamBan"]) : 0
                };

                result.Add(thongKe);
            }

            return result;
        }

        public decimal LayTongDoanhThu(List<ThongKeDoanhThu> ListThongKeDoanhThu)
        {
            return ListThongKeDoanhThu.Sum(x => x.DoanhThu);
        }

        public int LayTongDonHang(List<ThongKeDoanhThu> ListThongKeDoanhThu)
        {
            return ListThongKeDoanhThu.Sum(x => x.SoDonHang);
        }

        public int LayTongSanPhamBan(List<ThongKeDoanhThu> ListThongKeDoanhThu)
        {
            return ListThongKeDoanhThu.Sum(x => x.SoSanPhamBan);
        }
    }
}