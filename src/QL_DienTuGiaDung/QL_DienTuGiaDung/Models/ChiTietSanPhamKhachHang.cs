namespace QL_DienTuGiaDung.Models
{
    public class ChiTietSanPhamKhachHang
    {        
        public int? MaSP { get; set; }
        public int? MaLSP { get; set; }
        public string? TenSP { get; set; }
        public string? TenLSP { get; set; }
        public string? TenTH { get; set; }
        public string? TenQG { get; set; }
        public decimal? GiaGocSauThue { get; set; }
        public decimal? MucGiamGiaSP { get; set; }
        public decimal? GiaSauGiamVaThue { get; set; }
        public int? SoLuongSP { get; set; }
        public int? SoLuongDaBan { get; set; }
        public int? SoLuotDG { get; set; }
        public decimal? DiemTrungBinh { get; set; }
        public string? PhanLoaiSP { get; set; }
        public int? NamSanXuatSP { get; set; }
        public string? BaoHanhSP { get; set; }
        public string? KichThuocSP { get; set; }
        public string? KhoiLuongSP { get; set; }
        public string? CongSuatTieuThu { get; set; }
        public string? ChatLieuSP { get; set; }
        public string? TienIchSP { get; set; }
        public string? CongNgheSP { get; set; }
        public List<Anh> ListAnh { get; set; } = new();
        public List<DanhGia> ListDanhGia { get; set; } = new();
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
