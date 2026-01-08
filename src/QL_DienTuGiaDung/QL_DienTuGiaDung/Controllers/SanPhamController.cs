using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly SanPhamBLL _sanPhamBLL;
        private readonly DanhGiaBLL _danhGiaBLL;

        public SanPhamController(SanPhamBLL sanPhamBLL, DanhGiaBLL danhGiaBLL)
        {
           _sanPhamBLL = sanPhamBLL;
           _danhGiaBLL = danhGiaBLL;
        }

        public IActionResult Index(int maloai)
        {
            var listSP = _sanPhamBLL.LayDanhSachSanPham(0, maloai);

            string tenLSP = listSP.FirstOrDefault()?.TenLSP ?? "Sản phẩm";

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
            };

            if (maloai > 0)
            {
                breadcrumb.Add(
                    (tenLSP, "")
                );
            } else
            {
                breadcrumb.Add(
                    ("Tất cả sản phẩm", "")
                );
            }

            ViewBag.Breadcrumb = breadcrumb;

            return View(listSP);
        }

        [HttpGet]
        public IActionResult ChiTiet(int masp)
        {
            var sanPham = _sanPhamBLL.LaySanPham(0, masp);

            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                (sanPham?.TenLSP ?? "Loại sản phẩm", Url.Action("Index", "SanPham", new { maloai = sanPham?.MaLSP })),
                (sanPham?.TenSP ?? "Sản phẩm", ""),
            };

            return View(sanPham);
        }

        [HttpPost]
        public IActionResult DanhGia(int DiemDG, string NhanXetDG, int MaSP)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                HttpContext.Session.SetString("RETURN_URL", $"SanPham/ChiTiet?masp={MaSP}");

                return RedirectToAction("Index", "TaiKhoan");
            }

            int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var danhGia = new DanhGia();
            
            danhGia.DiemDG = DiemDG;
            danhGia.NhanXetDG = NhanXetDG;
            danhGia.MaSP = MaSP;
            danhGia.MaKH = maKH;

            var danhGiaDaCo = _danhGiaBLL.LayDanhGia(MaSP, maKH);

            if(danhGiaDaCo != null)
            {
                if(_danhGiaBLL.SuaDanhGia(danhGia) <= 0)
                {
                    TempData["Message"] = "Chỉnh sửa đánh giá thất bại";
                    return RedirectToAction("ChiTiet", new { masp = MaSP });
                }

                TempData["Message"] = "Chỉnh sửa đánh giá thành công";

                return RedirectToAction("ChiTiet", new { masp = MaSP });
            } 

            if(_danhGiaBLL.TaoDanhGia(danhGia) <= 0)
            {
                TempData["Message"] = "Gửi đánh giá thất bại";
                return RedirectToAction("ChiTiet", new { masp = MaSP });
            }

            TempData["Message"] = "Gửi đánh giá thành công";

            return RedirectToAction("ChiTiet", new { masp = MaSP });
        }

        public IActionResult TimKiem(string tukhoa)
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                ("Kết quả tìm kiếm", ""),
            };

            var listSP = _sanPhamBLL.LayDanhSachSanPham(0, 0, tukhoa.Trim());

            return View("Index", listSP);
        }
    }
}
