namespace QL_DienTuGiaDung.Models
{
    public class SanPhamGioHang
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; } = string.Empty;
        public decimal GiaBanSP { get; set; }
        public int SoLuongSP { get; set; }
        public string UrlAnh { get; set; } = string.Empty;
        public decimal ThanhTien => GiaBanSP * SoLuongSP;
    }
}