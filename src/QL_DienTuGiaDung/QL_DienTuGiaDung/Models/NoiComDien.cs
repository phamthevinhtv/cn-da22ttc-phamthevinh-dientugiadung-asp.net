namespace QL_DienTuGiaDung.Models
{
    public class NoiComDien : SanPham
    {
        public int MaNCD { get; set; }
        public string? DungTichNCD { get; set; } = string.Empty;
        public string? ChucNangNCD { get; set; } = string.Empty;
        public string? DoDayNCD { get; set; } = string.Empty;
        public string? DieuKhienNCD { get; set; } = string.Empty;
        public string? ChieuDaiDayDienNCD { get; set; } = string.Empty;
    }
}
