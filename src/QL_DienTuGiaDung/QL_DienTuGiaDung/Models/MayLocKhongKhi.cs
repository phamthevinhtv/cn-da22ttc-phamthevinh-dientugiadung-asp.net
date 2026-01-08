namespace QL_DienTuGiaDung.Models
{
    public class MayLocKhongKhi : SanPham
    {
        public int MaMLKK { get; set; }
        public string? LoaiBuiLocDuocMLKK { get; set; } = string.Empty;
        public string? PhamViLocMLKK { get; set; } = string.Empty;
        public string? LuongGioMLKK { get; set; } = string.Empty;
        public string? MangLocMLKK { get; set; } = string.Empty;
        public string? BangDieuKhienMLKK { get; set; } = string.Empty;
        public string? DoOnMLKK { get; set; } = string.Empty;
        public string? CamBienMLKK { get; set; } = string.Empty;
    }
}
