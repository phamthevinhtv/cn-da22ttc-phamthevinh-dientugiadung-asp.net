namespace QL_DienTuGiaDung.Models
{
    public class GioHang
    {
        public List<SanPhamGioHang> ListSanPhamGioHang { get; set; } = new();
        public decimal SoTien => ListSanPhamGioHang.Sum(sp => sp.ThanhTien);
        public decimal PhiVanChuyen => SoTien >= 5000000 ? 0 : 30000;
        public decimal TongTien => SoTien + PhiVanChuyen;
        public List<DiaChiNhanHang> ListDiaChiNhanHang { get; set; } = new List<DiaChiNhanHang>();
        public List<PhuongThucThanhToan> ListPhuongThucThanhToan { get; set; } = new List<PhuongThucThanhToan>();
    }
}
