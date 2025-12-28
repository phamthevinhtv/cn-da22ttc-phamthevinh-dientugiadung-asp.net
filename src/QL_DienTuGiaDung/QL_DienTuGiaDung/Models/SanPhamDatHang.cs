namespace QL_DienTuGiaDung.Models
{
    public class SanPhamDatHang
    {
        public int MaSP { get; set; }
        public int SoLuongDat { get; set; }
        public string? TenSP { get; set; }
        public decimal? GiaDatSauGiamVaThue { get; set; }
        public string? UrlAnh { get; set; }
        public decimal? ThanhTien => GiaDatSauGiamVaThue * SoLuongDat;
    }
}