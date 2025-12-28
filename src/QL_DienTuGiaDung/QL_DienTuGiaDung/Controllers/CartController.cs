using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.Controllers
{
    public class CartController : Controller
    {
        private const string CART_KEY = "CART";

        private readonly ProductBLL _productBLL;

        private readonly OrderBLL _orderBLL;

        private readonly AccountBLL _accountBLL;

        public CartController(ProductBLL productBLL, OrderBLL orderBLL, AccountBLL accountBLL)
        {
            _productBLL = productBLL;
            _orderBLL = orderBLL;
            _accountBLL = accountBLL;
        }
        
        private List<SanPhamTrongGio> GetCart()
        {
            return HttpContext.Session.GetObject<List<SanPhamTrongGio>>(CART_KEY) ?? new List<SanPhamTrongGio>();
        }

        private void SaveCart(List<SanPhamTrongGio> cart)
        {
            HttpContext.Session.SetObject(CART_KEY, cart);
        }

        public IActionResult Index()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "Product")),
                ("Giỏ hàng", Url.Action("Index", "Cart"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            var cart = GetCart();

            if (!cart.Any()) return View(new GioHang());

            var listMaSP = cart.Select(x => x.MaSP).ToList();

            var products = _productBLL.GetByProductIdsForCustomer(listMaSP);

            for (int i = cart.Count - 1; i >= 0; i--)
            {
                var item = cart[i];
                var sp = products.FirstOrDefault(x => x.MaSP == item.MaSP);
                if (sp == null || sp.SoLuongSP <= 0)
                {
                    cart.RemoveAt(i);
                    continue;
                }

                if (item.SoLuongSP > sp.SoLuongSP)
                {
                    item.SoLuongSP = sp.SoLuongSP ?? 0;
                }

                item.TenSP = sp.TenSP;
                item.GiaBanSP = sp.GiaSauGiamVaThue;
                item.UrlAnh = sp.UrlAnh;
            }

            SaveCart(cart);

            var gioHang = new GioHang()
            {
                ListSanPhamTrongGio = cart
            };

            return View(gioHang);
        }


        [HttpPost]
        public IActionResult HandleAddToCart (int maSP)
        {
            var product = _productBLL.GetByProductIdForCustomer(maSP);

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MaSP == maSP);

            if (item != null)
            {
                if(product?.SoLuongSP <= 0)
                {
                    TempData["Error"] = "Sản phẩm đã hết hàng";
                    item.SoLuongSP = 0;
                    SaveCart(cart);
                    return RedirectToAction("Index", "Product");
                } 
                
                if (item.SoLuongSP + 1 > product?.SoLuongSP)
                {
                    TempData["Error"] = "Sản phẩm không đủ số lượng";
                    item.SoLuongSP = product?.SoLuongSP ?? 0;
                    SaveCart(cart);
                    return RedirectToAction("Detail", "Product", new { id = maSP });
                } else
                {
                    item.SoLuongSP += 1;
                }
            }
            else
            {
                cart.Add(new SanPhamTrongGio
                {
                    MaSP = maSP,
                    SoLuongSP = 1
                });
            }

            SaveCart(cart);

            return RedirectToAction("Detail", "Product", new { id = maSP });
        }

        [HttpPost]
        public IActionResult HandleBuyNow(int maSP)
        {
            var product = _productBLL.GetByProductIdForCustomer(maSP);

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MaSP == maSP);

            if (item == null)
            {
                if (product?.SoLuongSP <= 0)
                {
                    TempData["Error"] = "Sản phẩm đã hết hàng";
                    return RedirectToAction("Index", "Product");
                } else
                {
                    cart.Add(new SanPhamTrongGio
                    {
                        MaSP = maSP,
                        SoLuongSP = 1
                    });

                    SaveCart(cart);
                }
            } else
            {
                if (item.SoLuongSP > product?.SoLuongSP)
                {
                    item.SoLuongSP = product?.SoLuongSP ?? 0;
                    SaveCart(cart);
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult HandleRemoveItem(int maSP)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MaSP == maSP);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult HandleUpdateQuantity(int maSP, int soLuongSP)
        {
            var product = _productBLL.GetByProductIdForCustomer(maSP);

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MaSP == maSP);

            if (item != null)
            {
                if(product?.SoLuongSP <= 0)
                {
                    TempData["Error"] = "Sản phẩm đã hết hàng";
                    item.SoLuongSP = 0;
                    SaveCart(cart);
                    return RedirectToAction("Index", "Product");
                } 

                if (soLuongSP <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    if(soLuongSP > product?.SoLuongSP)
                    {
                        TempData["Error"] = "Sản phẩm không đủ số lượng";
                        item.SoLuongSP = product?.SoLuongSP ?? 0;
                        SaveCart(cart);
                        return RedirectToAction("Index");
                    } else
                    {
                        item.SoLuongSP = soLuongSP;
                    }
                }
            }

            SaveCart(cart);

            return RedirectToAction("Index", "Cart");
        }

        [Authorize]
        [HttpPost]
        public IActionResult HandleAddUpdateAddress(string TenDCCT, string provinceSelect, string communeSelect, int MaDCCT, int MacDinhDCCT)
        {
            var phone = User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";
            var province = provinceSelect.Split('|', 2);
            var cummune = communeSelect.Split('|', 2);

            var taiKhoanKhachHang = new TaiKhoanKhachHang();
            var diaChiCuThe = new DiaChiCuThe();

            diaChiCuThe.MaDCCT = MaDCCT;
            diaChiCuThe.TenDCCT = TenDCCT;
            diaChiCuThe.MacDinhDCCT = MacDinhDCCT;

            taiKhoanKhachHang.SoDienThoaiKH = phone;

            taiKhoanKhachHang.DiaChiCuThe = diaChiCuThe;

            taiKhoanKhachHang.TinhThanhPho ??= new TinhThanhPho();
            taiKhoanKhachHang.TinhThanhPho.MaTTP = province[0];
            taiKhoanKhachHang.TinhThanhPho.TenTTP = province[1];

            taiKhoanKhachHang.XaPhuong ??= new XaPhuong();
            taiKhoanKhachHang.XaPhuong.MaXP = cummune[0];
            taiKhoanKhachHang.XaPhuong.TenXP = cummune[1];
            taiKhoanKhachHang.SoDienThoaiKH = phone;

            _accountBLL.AddAddress(taiKhoanKhachHang);
            
            if(MaDCCT > 0)
            {
                TempData["Message"] = "Cập nhật địa chỉ thành công";
            } else
            {
                TempData["Message"] = "Thêm địa chỉ thành công";
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult HandleCheckout(int MaDCCT, int MaPTTT, int MaTTTT)
        {
            var cart = GetCart();

            var listMaSP = cart.Select(x => x.MaSP).ToList();

            var products = _productBLL.GetByProductIdsForCustomer(listMaSP);

            for (int i = cart.Count - 1; i >= 0; i--)
            {
                var item = cart[i];
                var sp = products.FirstOrDefault(x => x.MaSP == item.MaSP);
                if (sp == null || sp.SoLuongSP <= 0)
                {
                    TempData["Error"] = $"Sản phẩm \"{item.TenSP}\" đã hết hàng";
                    cart.RemoveAt(i);
                    return RedirectToAction("Index");
                }

                if (item.SoLuongSP > sp.SoLuongSP)
                {
                    TempData["Error"] = $"Sản phẩm \"{sp?.TenSP}\" không đủ số lượng";
                    item.SoLuongSP = sp?.SoLuongSP ?? 0;
                    return RedirectToAction("Index");
                }

                SaveCart(cart);
            }

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var donHang = new DonHang();

                var phone = User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";

                donHang.SoDienThoaiKH = phone;
                donHang.MaDCCT = MaDCCT;
                donHang.MaPTTT = MaPTTT;
                donHang.MaTTTT = MaTTTT;

                var danhSachDatHang = cart
                .Select(sp => new SanPhamDatHang
                {
                    MaSP = sp.MaSP,
                    SoLuongDat = sp.SoLuongSP
                })
                .ToList();

                decimal soTien = cart.Sum(sp => sp.ThanhTien) ?? 0;

                decimal phiVanChuyen = (soTien >= 5000000) ? 0 : 30000;

                donHang.ListSanPhamDatHang = danhSachDatHang;
                donHang.PhiVanChuyenDH = phiVanChuyen;

                _orderBLL.DatHang(donHang);

                TempData["Message"] = "Đặt hàng thành công";
                
                SaveCart(new List<SanPhamTrongGio>());
            } else
            {
                HttpContext.Session.SetString("RETURN_URL", "Cart|Index");

                return RedirectToAction("Index", "Account");
            }

            return RedirectToAction("History", "Account");
        }
    }
}