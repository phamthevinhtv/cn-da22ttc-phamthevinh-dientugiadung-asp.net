using System.ComponentModel.DataAnnotations;

namespace QL_DienTuGiaDung.Models
{
    public class DiaChiCuThe
    {
        public int MaDCCT { get; set; }
        public string MaXP { get; set; } = string.Empty;
        public int MaKH { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số nhà, tên đường/ấp/khóm")]
        public string TenDCCT { get; set; } = string.Empty;
        public int? MacDinhDCCT { get; set; }
    }
}
