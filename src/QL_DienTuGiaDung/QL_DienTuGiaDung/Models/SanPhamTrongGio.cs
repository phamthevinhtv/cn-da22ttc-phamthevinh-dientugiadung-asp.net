namespace QL_DienTuGiaDung.Models
{
    public class SanPhamTrongGio
    {
        public int MaSP { get; set; }
        public string? TenSP { get; set; }
        public decimal? GiaBanSP { get; set; }
        public int SoLuongSP { get; set; }
        public string? UrlAnh { get; set; }
        public decimal? ThanhTien => GiaBanSP * SoLuongSP;
    }
}