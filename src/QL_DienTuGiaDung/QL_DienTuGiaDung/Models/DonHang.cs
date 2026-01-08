namespace QL_DienTuGiaDung.Models
{
    public class DonHang
    {
        public int MaDH { get; set; }
        public int MaPTTT { get; set; }
        public decimal PhiVanChuyenDH { get; set; }
        public decimal TongTienDH { get; set; }
        public int MaTTTT { get; set; }
        public int MaTTDH { get; set; }
        public DateTime NgayTaoDH { get; set; }
        public DateTime NgayCapNhatDH { get; set; }
        public DateTime NgayCapNhatTT { get; set; }
        public string TenKH { get; set; } = string.Empty;
        public string SoDienThoaiKH { get; set; } = string.Empty;
        public string TenDCCT { get; set; } = string.Empty;
        public string TenXP { get; set; } = string.Empty;
        public string TenTTP { get; set; } = string.Empty;
        public KhachHang KhachHang { get; set; } = new KhachHang();
        public TrangThaiDonHang TrangThaiDonHang { get; set; } = new TrangThaiDonHang();
        public TrangThaiThanhToan TrangThaiThanhToan { get; set; } = new TrangThaiThanhToan();
        public PhuongThucThanhToan PhuongThucThanhToan { get; set; } = new PhuongThucThanhToan();
        public DiaChiNhanHang DiaChiNhanHang { get; set; } = new DiaChiNhanHang();
        public List<SanPhamDatHang> ListSanPhamDatHang { get; set; } = new();
    }
}