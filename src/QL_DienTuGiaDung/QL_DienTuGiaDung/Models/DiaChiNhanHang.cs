using System.ComponentModel.DataAnnotations;

namespace QL_DienTuGiaDung.Models
{
    public class DiaChiNhanHang
    {
        public DiaChiCuThe DiaChiCuThe { get; set; } = new DiaChiCuThe();         
        public TinhThanhPho TinhThanhPho { get; set; } = new TinhThanhPho();
        public XaPhuong XaPhuong { get; set; } = new XaPhuong();

        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố")]
        public string TinhTPChon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn Xã/Phường")]
        public string XaPhuongChon { get; set; } = string.Empty;

        public int DuocSuaXoa { get; set; }
        public string DiaChiString
        {
            get
            {
                var tendc = DiaChiCuThe.TenDCCT ?? "!";
                var xa = XaPhuong?.TenXP ?? "!";
                var tinh = TinhThanhPho?.TenTTP ?? "!";

                return $"{tendc}, {xa}, {tinh}";
            }
        }

    }
}
