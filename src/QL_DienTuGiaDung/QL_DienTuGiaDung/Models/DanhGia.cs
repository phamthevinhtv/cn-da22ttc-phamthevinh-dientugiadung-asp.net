using System.ComponentModel.DataAnnotations;

namespace QL_DienTuGiaDung.Models
{
    public class DanhGia
    {
        public int MaSP { get; set; }
        public int MaKH { get; set; }
        public int DiemDG { get; set; }
        public string? NhanXetDG { get; set; }
        public string TenKH { get; set; } = string.Empty;
        public DateTime NgayTaoDG { get; set; }
        public DateTime NgayCapNhatDG { get; set; }

    }
}
