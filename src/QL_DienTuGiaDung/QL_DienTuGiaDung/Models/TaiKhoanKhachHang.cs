
namespace QL_DienTuGiaDung.Models
{
    public class TaiKhoanKhachHang
    {
        public int? MaTK { get; set; }
        public string? TenTK { get; set; }
        public int? QuyenTK { get; set; }
        public int? TrangThaiTK { get; set; }
        public string? TenKH { get; set; }
        public int? GioiTinhKH { get; set; }
        public string? SoDienThoaiKH { get; set; }
        public string? EmailKH { get; set; }
        public List<DiaChiDayDu>? ListDiaChiDayDu { get; set; } = new();
        public TinhThanhPho? TinhThanhPho { get; set; }
        public XaPhuong? XaPhuong { get; set; }
        public DiaChiCuThe? DiaChiCuThe { get; set; }
    }
}