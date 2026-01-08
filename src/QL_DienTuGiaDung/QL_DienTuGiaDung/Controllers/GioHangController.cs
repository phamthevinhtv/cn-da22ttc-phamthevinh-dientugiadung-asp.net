using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.Controllers
{
    public class GioHangController : Controller
    {
        private const string CART_KEY = "CART";
        private readonly SanPhamBLL _sanPhamBLL;
        private readonly DiaChiBLL _diaChiBLL;
        private readonly DonHangBLL _donHangBLL;

        public GioHangController(SanPhamBLL sanPhamBLL, DiaChiBLL diaChiBLL, DonHangBLL donHangBLL)
        {
            _sanPhamBLL = sanPhamBLL;
            _diaChiBLL = diaChiBLL;
            _donHangBLL = donHangBLL;
        }
        
        private List<SanPhamGioHang> LayGioHang()
        {
            return HttpContext.Session.GetObject<List<SanPhamGioHang>>(CART_KEY) ?? new List<SanPhamGioHang>();
        }

        private void LuuGioHang(List<SanPhamGioHang> cart)
        {
            HttpContext.Session.SetObject(CART_KEY, cart);
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                ("Giỏ hàng", ""),
            };

            var gioHang = LayGioHang();

            if (!gioHang.Any()) return View(new GioHang());

            var listMaSP = gioHang.Select(x => x.MaSP).ToList();

            var dsSanPham = _sanPhamBLL.LayDanhSachSanPhamBangListMa(listMaSP);

            for (int i = gioHang.Count - 1; i >= 0; i--)
            {
                var item = gioHang[i];
                var sp = dsSanPham.FirstOrDefault(x => x.MaSP == item.MaSP);
                if (sp == null || sp.SoLuongSP <= 0)
                {
                    gioHang.RemoveAt(i);
                    continue;
                }

                if (item.SoLuongSP > sp.SoLuongSP)
                {
                    item.SoLuongSP = sp.SoLuongSP;
                }

                item.TenSP = sp.TenSP;
                item.GiaBanSP = sp.GiaSauGiamVaThueSP;
                item.UrlAnh = sp.UrlAnh;
            }

            LuuGioHang(gioHang);

            var dsDiaChi = new List<DiaChiNhanHang>();

            if (User.Identity?.IsAuthenticated ?? false)
            {
                int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                dsDiaChi = _diaChiBLL.LayDanhSachDiaChi(maKH);
            }

            var gioHangModel = new GioHang()
            {
                ListSanPhamGioHang = gioHang,
                ListDiaChiNhanHang = dsDiaChi
            };

            return View(gioHangModel);
        }

        [HttpPost]
        public IActionResult ThemSanPhamVaoGio (int MaSP)
        {
            var sanPham = _sanPhamBLL.LaySanPham(0, MaSP);

            if (sanPham == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại";
                return RedirectToAction("Index", "SanPham");
            }

            var gioHang = LayGioHang();

            var item = gioHang.FirstOrDefault(x => x.MaSP == MaSP);

            if (item != null)
            {
                if(sanPham?.SoLuongSP <= 0)
                {
                    TempData["Message"] = "Sản phẩm đã hết hàng";
                    gioHang.Remove(item);
                    LuuGioHang(gioHang);
                    return RedirectToAction("Index", "SanPham");
                } 
                
                if (item.SoLuongSP + 1 > sanPham?.SoLuongSP)
                {
                    TempData["Message"] = "Sản phẩm không đủ số lượng";
                    item.SoLuongSP = sanPham.SoLuongSP;
                    LuuGioHang(gioHang);
                    return RedirectToAction("ChiTiet", "SanPham", new { masp = MaSP });
                } else
                {
                    item.SoLuongSP += 1;
                }
            }
            else
            {
                gioHang.Add(new SanPhamGioHang
                {
                    MaSP = MaSP,
                    SoLuongSP = 1
                });
            }

            LuuGioHang(gioHang);

            return RedirectToAction("ChiTiet", "SanPham", new { masp = MaSP });
        }

        [HttpPost]
        public IActionResult MuaNgay(int MaSP)
        {
            var sanPham = _sanPhamBLL.LaySanPham(0, MaSP);

            if (sanPham == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại";
                return RedirectToAction("Index", "SanPham");
            }

            var gioHang = LayGioHang();

            var item = gioHang.FirstOrDefault(x => x.MaSP == MaSP);

            if (item != null)
            {
                if (!User.Identity?.IsAuthenticated ?? false)
                {
                    HttpContext.Session.SetString("RETURN_URL", $"GioHang/Index");
                    return RedirectToAction("Index", "TaiKhoan");
                }

            } else
            {
                gioHang.Add(new SanPhamGioHang
                {
                    MaSP = MaSP,
                    SoLuongSP = 1
                });
            }

            LuuGioHang(gioHang);

            if (!User.Identity?.IsAuthenticated ?? false)
            {
                HttpContext.Session.SetString("RETURN_URL", $"GioHang/Index");
                return RedirectToAction("Index", "TaiKhoan");
            }

            return RedirectToAction("Index", "GioHang");
        }

        [HttpPost]
        public IActionResult SuaSoLuongSanPhamTrongGio(int MaSP, int SoLuongSP)
        {
            var sanPham = _sanPhamBLL.LaySanPham(0, MaSP);

             if (sanPham == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại";
                return RedirectToAction("Index", "SanPham");
            }

            var gioHang = LayGioHang();

            var item = gioHang.FirstOrDefault(x => x.MaSP == MaSP);

            if (item != null)
            {
                if(sanPham?.SoLuongSP <= 0)
                {
                    TempData["Message"] = "Sản phẩm đã hết hàng";
                    gioHang.Remove(item);
                    LuuGioHang(gioHang);
                    return RedirectToAction("Index", "SanPham");
                } 

                if (SoLuongSP <= 0)
                {
                    gioHang.Remove(item);
                }
                else
                {
                    if(SoLuongSP > sanPham?.SoLuongSP)
                    {
                        TempData["Message"] = "Sản phẩm không đủ số lượng";
                        item.SoLuongSP = sanPham.SoLuongSP;
                        LuuGioHang(gioHang);
                        return RedirectToAction("Index");
                    } else
                    {
                        item.SoLuongSP = SoLuongSP;
                    }
                }
            }

            LuuGioHang(gioHang);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult XoaSanPhamTrongGio(int MaSP)
        {
            var gioHang = LayGioHang();

            var item = gioHang.FirstOrDefault(x => x.MaSP == MaSP);

            if (item != null)
            {
                gioHang.Remove(item);
                LuuGioHang(gioHang);
                TempData["Message"] = "Xóa sản phẩm khỏi giỏ hàng thành công";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DatHang(int MaDCCT)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                HttpContext.Session.SetString("RETURN_URL", "GioHang/Index");

                return RedirectToAction("Index", "TaiKhoan");
            }

            var gioHang = LayGioHang();

            var listMaSP = gioHang.Select(x => x.MaSP).ToList();

            var dsSanPham = _sanPhamBLL.LayDanhSachSanPhamBangListMa(listMaSP);

            for (int i = gioHang.Count - 1; i >= 0; i--)
            {
                var item = gioHang[i];
                var sanPham = dsSanPham.FirstOrDefault(x => x.MaSP == item.MaSP);
                if (sanPham == null || sanPham.SoLuongSP <= 0)
                {
                    TempData["Message"] = $"Sản phẩm \"{item.TenSP}\" đã hết hàng";
                    gioHang.RemoveAt(i);
                    return RedirectToAction("Index");
                }

                if (item.SoLuongSP > sanPham.SoLuongSP)
                {
                    TempData["Message"] = $"Sản phẩm \"{item.TenSP}\" không đủ số lượng";
                    item.SoLuongSP = sanPham.SoLuongSP;
                    return RedirectToAction("Index");
                }

                LuuGioHang(gioHang);
            }

            var donHang = new DonHang();

            int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            donHang.KhachHang.MaKH = maKH;
            donHang.DiaChiNhanHang.DiaChiCuThe.MaDCCT = MaDCCT;

            var danhSachDatHang = gioHang
            .Select(sp => new SanPhamDatHang
            {
                MaSP = sp.MaSP,
                SoLuongDat = sp.SoLuongSP
            })
            .ToList();

            decimal soTien = gioHang.Sum(sp => sp.ThanhTien);

            decimal phiVanChuyen = (soTien >= 5000000) ? 0 : 30000;

            donHang.ListSanPhamDatHang = danhSachDatHang;
            donHang.PhiVanChuyenDH = phiVanChuyen;

            if(_donHangBLL.DatHang(donHang) <= 0)
            {
                TempData["Message"] = "Đặt hàng thất bại";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "Đặt hàng thành công";
            
            LuuGioHang(new List<SanPhamGioHang>());
                
            return RedirectToAction("LichSuMuaHang", "TaiKhoan");
        }
    }
}