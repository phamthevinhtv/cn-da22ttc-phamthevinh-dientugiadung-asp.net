using System.ComponentModel.DataAnnotations;

namespace QL_DienTuGiaDung.Models
{
    public class TaiKhoan
    {
        public int MaTK { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Tên đăng nhập phải từ 6 đến 20 ký tự")]
        public string TenTK { get; set; } = string.Empty;

        public int QuyenTK { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 30 ký tự")]
        public string MatKhauTK { get; set; } = string.Empty;

        public DateTime NgayTaoTK { get; set; }
        public DateTime NgayCapNhatTK { get; set; }
        public int TrangThaiTK { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập OTP")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP phải gồm 6 chữ số")]
        public string Otp { get; set; } = string.Empty;
    }
}
