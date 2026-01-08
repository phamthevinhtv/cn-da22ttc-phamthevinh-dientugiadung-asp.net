namespace QL_DienTuGiaDung.Models
{
    public class MayRuaChen : SanPham
    {
        public int MaMRC { get; set; }
        public string? NuocTieuThuMRC { get; set; } = string.Empty;
        public string? SoChenRuaDuocMRC { get; set; } = string.Empty;
        public string? DoOnMRC { get; set; } = string.Empty;
        public string? BangDieuKhienMRC { get; set; } = string.Empty;
        public string? ChieuDaiOngCapNuocMRC { get; set; } = string.Empty;
        public string? ChieuDaiOngThoatNuocMRC { get; set; } = string.Empty;
    }
}
