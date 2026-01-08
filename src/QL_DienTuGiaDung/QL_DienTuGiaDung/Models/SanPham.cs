using System.ComponentModel.DataAnnotations;

namespace QL_DienTuGiaDung.Models
{
    public class SanPham
    {
        public int MaSP { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn quốc gia")]
        public int MaQG { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int MaTH { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
        public int MaLSP { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        public string TenSP { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int SoLuongSP { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập giá nhập")]
        [Range(0, 9999999999, ErrorMessage = "Giá nhập phải lớn hơn hoặc bằng 0 và tối đa 10 ký tự")]
        public decimal GiaNhapSP { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập giá niêm yết")]
        [Range(0, 9999999999, ErrorMessage = "Giá niêm yết phải lớn hơn hoặc bằng 0 và tối đa 10 ký tự")]
        public decimal GiaGocSP { get; set; }
        
        [StringLength(50, ErrorMessage = "Phân loại không được vượt quá 50 ký tự")]
        public string? PhanLoaiSP { get; set; } = string.Empty;
        
        [Range(2000, 2026, ErrorMessage = "Năm sản xuất phải trước hoặc bằng năm hiện tại")]
        public int NamSanXuatSP { get; set; } = DateTime.Now.Year;
        
        [StringLength(50, ErrorMessage = "Thời gian bảo hành không được vượt quá 50 ký tự")]
        public string? BaoHanhSP { get; set; } = string.Empty;
        
        [StringLength(255, ErrorMessage = "Kích thước không được vượt quá 255 ký tự")]
        public string? KichThuocSP { get; set; } = string.Empty;
        
        [StringLength(100, ErrorMessage = "Khối lượng không được vượt quá 100 ký tự")]
        public string? KhoiLuongSP { get; set; } = string.Empty;
        
        [StringLength(10, ErrorMessage = "Công suất tiêu thụ không được vượt quá 10 ký tự")]
        public string? CongSuatTieuThuSP { get; set; } = string.Empty;
        
        [StringLength(255, ErrorMessage = "Chất liệu không được vượt quá 255 ký tự")]
        public string? ChatLieuSP { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Tiện ích không được vượt quá 500 ký tự")]
        public string? TienIchSP { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Công nghệ không được vượt quá 500 ký tự")]
        public string? CongNgheSP { get; set; } = string.Empty;
        
        [Range(0, 100, ErrorMessage = "Mức giảm giá phải ở trong khoảng từ 0 đến 100%")]
        public decimal? MucGiamGiaSP { get; set; } = 0;
        
        public DateTime? NgayHetGiamGiaSP { get; set; }
        
        public DateTime NgayTaoSP { get; set; }
        public DateTime NgayCapNhatSP { get; set; }
        public int TrangThaiSP { get; set; }
        public decimal GiaGocSauThueSP { get; set; }
        public decimal GiaSauGiamVaThueSP { get; set; }
        public int SoLuongDaBanSP { get; set; }
        public decimal DiemTrungBinhSP { get; set; }
        public int SoLuotDGSP { get; set; }
        public string UrlAnh { get; set; } = string.Empty;
        public int TrangThaiLSP { get; set; }
        public decimal ThueGTGTLSP { get; set; }
        public string TenLSP { get; set; } = string.Empty;
        public string TenQG { get; set; } = string.Empty;
        public string TenTH { get; set; } = string.Empty;
        public List<Anh> ListAnh { get; set; } = new List<Anh>();
        public List<DanhGia> ListDanhGia { get; set; } = new List<DanhGia>();
    }
}
