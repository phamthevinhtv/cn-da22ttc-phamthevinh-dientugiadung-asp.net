namespace QL_DienTuGiaDung.Models
{
    public class DonHang
    {
        public string? SoDienThoaiKH { get; set; }
        public int MaDH { get; set; }
        public int MaDCCT { get; set; }
        public int MaPTTT { get; set; }
        public decimal PhiVanChuyenDH { get; set; }
        public decimal TongTienDH { get; set; }
        public int? MaTTTT { get; set; }
        public int? MaTTDH { get; set; }
        public DateTime NgayTaoDH { get; set; }
        public DateTime NgayCapNhatDH { get; set; }
        public DateTime NgayCapNhatTT { get; set; }
        public string? TenDCCT { get; set; }
        public string? TenXP { get; set; }
        public string? TenTTP { get; set; }
        public string? TenPTTT { get; set; }
        public string? TenTTDH { get; set; }
        public string? TenTTTT { get; set; }
        public string? TenKH { get; set; }
        public List<SanPhamDatHang> ListSanPhamDatHang { get; set; } = new();
    }
}