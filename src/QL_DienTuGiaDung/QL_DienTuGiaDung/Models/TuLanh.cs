namespace QL_DienTuGiaDung.Models
{
    public class TuLanh : SanPham
    {
        public int MaTL { get; set; }
        public string? DungTichNganDaTL { get; set; } = string.Empty;
        public string? DungTichNganLanhTL { get; set; } = string.Empty;
        public string? LayNuocNgoaiTL { get; set; } = string.Empty;
        public string? LayDaTuDongTL { get; set; } = string.Empty;
    }
}
