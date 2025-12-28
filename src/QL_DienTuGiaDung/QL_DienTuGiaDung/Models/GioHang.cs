namespace QL_DienTuGiaDung.Models
{
    public class GioHang
    {
        public List<SanPhamTrongGio> ListSanPhamTrongGio { get; set; } = new();

        public decimal? SoTien => ListSanPhamTrongGio?.Sum(sp => sp.ThanhTien);

        public decimal? PhiVanChuyen => SoTien >= 5000000 ? 0 : 30000;

        public decimal? TongTien => SoTien + PhiVanChuyen;
    }
}
