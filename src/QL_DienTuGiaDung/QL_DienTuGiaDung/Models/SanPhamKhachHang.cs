namespace QL_DienTuGiaDung.Models
{
    public class SanPhamKhachHang
    {
        public int MaSP { get; set; }
        public string? TenSP { get; set; }
        public decimal? GiaGocSauThue { get; set; }
        public decimal? GiaSauGiamVaThue { get; set; }
        public decimal? MucGiamGiaSP { get; set; }
        public int? SoLuongSP { get; set; }
        public int? SoLuongDaBan { get; set; }
        public decimal? DiemTrungBinh { get; set; }
        public string? UrlAnh { get; set; }
        public string? TenLSP { get; set; }
    }
}