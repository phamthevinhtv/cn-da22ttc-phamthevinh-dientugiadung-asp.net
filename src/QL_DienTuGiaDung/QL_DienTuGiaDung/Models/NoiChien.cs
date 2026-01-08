namespace QL_DienTuGiaDung.Models
{
    public class NoiChien : SanPham
    {
        public int MaNC { get; set; }
        public string? DungTichTongNC { get; set; } = string.Empty;
        public string? DungTichSuDungNC { get; set; } = string.Empty;
        public string? NhietDoNC { get; set; } = string.Empty;
        public string? HenGioNC { get; set; } = string.Empty;
        public string? BangDieuKhienNC { get; set; } = string.Empty;
        public string? ChieuDaiDayDienNC { get; set; } = string.Empty;
    }
}
