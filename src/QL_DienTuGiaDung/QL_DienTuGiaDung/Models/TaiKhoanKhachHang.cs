using System.ComponentModel.DataAnnotations;

namespace QL_DienTuGiaDung.Models
{
    public class TaiKhoanKhachHang
    {
        public KhachHang KhachHang { get; set; } = new KhachHang();
        public TaiKhoan TaiKhoan { get; set; } = new TaiKhoan();
        public DiaChiNhanHang DiaChiNhanHang { get; set; } = new DiaChiNhanHang();
        public List<DiaChiNhanHang>? ListDiaChiNhanHang { get; set; } = new List<DiaChiNhanHang>();
    }
}
