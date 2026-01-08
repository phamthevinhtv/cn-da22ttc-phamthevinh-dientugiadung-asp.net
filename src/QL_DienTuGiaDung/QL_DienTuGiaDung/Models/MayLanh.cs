namespace QL_DienTuGiaDung.Models
{
    public class MayLanh : SanPham
    {
        public int MaML { get; set; }
        public string? CongSuatLamLanhML { get; set; } = string.Empty;
        public string? PhamViLamLanhML { get; set; } = string.Empty;
        public string? DoOnML { get; set; } = string.Empty;
        public string? LoaiGasML { get; set; } = string.Empty;
        public string? CheDoGioML { get; set; } = string.Empty;
    }
}
