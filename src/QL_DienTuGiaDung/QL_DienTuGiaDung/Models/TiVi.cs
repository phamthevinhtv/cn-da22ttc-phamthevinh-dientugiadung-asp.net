namespace QL_DienTuGiaDung.Models
{
    public class TiVi : SanPham
    {
        public int MaTV { get; set; }
        public string? CoManHinhTV { get; set; } = string.Empty;
        public string? DoPhanGiaiTV { get; set; } = string.Empty;
        public string? LoaiManHinhTV { get; set; } = string.Empty;
        public string? TanSoQuetTV { get; set; } = string.Empty;
        public string? DieuKhienTV { get; set; } = string.Empty;
        public string? CongKetNoiTV { get; set; } = string.Empty;
    }
}
