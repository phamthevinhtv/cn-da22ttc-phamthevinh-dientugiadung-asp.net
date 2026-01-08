using System.ComponentModel.DataAnnotations;

namespace QL_DienTuGiaDung.Models
{
    public class KhachHang
    {
        public int MaKH { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string TenKH { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public int? GioiTinhKH { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0(3|5|7|8|9)[0-9]{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoaiKH { get; set; } = string.Empty;

        public string? EmailKH { get; set; }
    }
}
