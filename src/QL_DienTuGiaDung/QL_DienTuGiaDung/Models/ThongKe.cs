namespace QL_DienTuGiaDung.Models
{
    public class ThongKeDoanhThu
    {
        public int Nam { get; set; }
        public int? Quy { get; set; }
        public int? Thang { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoDonHang { get; set; }
        public int SoSanPhamBan { get; set; }
    }

    public class ThongKeFilter
    {
        public int? Nam { get; set; }
        public int? Quy { get; set; }
        public string LoaiThongKe { get; set; } = "nam"; // "nam", "quy", "thang"
    }
}