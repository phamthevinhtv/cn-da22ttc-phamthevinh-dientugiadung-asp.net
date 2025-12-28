namespace QL_DienTuGiaDung.Models
{
    public class SanPham
    {
        public int MaSP { get; set; }
        public int? MaQG { get; set; }
        public int? MaTH { get; set; }
        public int? MaLSP { get; set; }
        public string? TenSP { get; set; }
        public int SoLuongSP { get; set; }
        public decimal GiaNhapSP { get; set; }
        public decimal GiaGocSP { get; set; }
        public string? PhanLoaiSP { get; set; }
        public int? NamSanXuatSP { get; set; }
        public string? BaoHanhSP { get; set; }
        public string? KichThuocSP { get; set; }
        public string? KhoiLuongSP { get; set; }
        public string? CongSuatTieuThu { get; set; }
        public string? ChatLieuSP { get; set; }
        public string? TienIchSP { get; set; }
        public string? CongNgheSP { get; set; }
        public decimal MucGiamGiaSP { get; set; }
        public DateTime? NgayHetGiamGiaSP { get; set; }
        public DateTime NgayTaoSP { get; set; }
        public DateTime NgayCapNhatSP { get; set; }
        public int TrangThaiSP { get; set; }
        public string? TenLSP { get; set; }
        public string? UrlAnh { get; set; }
        public MayLanh? MayLanh { get; set; }
        public TuLanh? TuLanh { get; set; }
        public MayLocKhongKhi? MayLocKhongKhi { get; set; }
        public MayLocNuoc? MayLocNuoc { get; set; }
        public MayRuaChen? MayRuaChen { get; set; }
        public NoiComDien? NoiComDien { get; set; }
        public NoiChien? NoiChien { get; set; }
        public TiVi? TiVi { get; set; }
    }
}
