namespace QL_DienTuGiaDung.Models
{
    public class MayLocNuoc : SanPham
    {
        public int MaMLN { get; set; }
        public string? KieuLapMLN { get; set; } = string.Empty;
        public string? CongSuatLocMLN { get; set; } = string.Empty;
        public string? TiLeLocThaiMLN { get; set; } = string.Empty;
        public string? ChiSoNuocMLN { get; set; } = string.Empty;
        public string? DoPHThucTeMLN { get; set; } = string.Empty;
        public string? ApLucNuocYeuCauMLN { get; set; } = string.Empty;
        public int SoLoiLocMLN { get; set; }
        public string? BangDieuKhienMLN { get; set; } = string.Empty;
    }
}
