using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;

namespace QL_DienTuGiaDung.ViewComponents
{
    public class LoaiSanPhamSidebarViewComponent : ViewComponent
    {
        private readonly LoaiSanPhamBLL _loaiSanPhamBLL;

        public LoaiSanPhamSidebarViewComponent(LoaiSanPhamBLL loaiSanPhamBLL)
        {
            _loaiSanPhamBLL = loaiSanPhamBLL;
        }

        public IViewComponentResult Invoke()
        {
            var list = _loaiSanPhamBLL.LayDanhSachLoaiSanPham(0);

            return View(list);
        }
    }
}
