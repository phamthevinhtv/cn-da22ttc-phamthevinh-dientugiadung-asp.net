using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly TaiKhoanBLL _taiKhoanBLL;
        private readonly DiaChiBLL _diaChiBLL;
        private readonly DonHangBLL _donHangBLL;

        public TaiKhoanController(TaiKhoanBLL taiKhoanBLL, DiaChiBLL diaChiBLL, DonHangBLL donHangBLL)
        {
            _taiKhoanBLL = taiKhoanBLL;
            _diaChiBLL = diaChiBLL;
            _donHangBLL = donHangBLL;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "SanPham");
            }

            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                ("Xác minh số điện thoại", ""),
            };

            HttpContext.Session.Remove("CUSTOMER_PHONE");
            HttpContext.Session.Remove("CUSTOMER_OTP");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> XacMinhSoDienThoaiAsync(KhachHang khachHangModel)
        {
            string otp = _taiKhoanBLL.GenerateOTP();

            bool success = await _taiKhoanBLL.SendOTPAsync(khachHangModel.SoDienThoaiKH, otp);
            if (!success)
            {
                TempData["Message"] = "Gửi OTP thất bại";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("CUSTOMER_PHONE", khachHangModel.SoDienThoaiKH);
            var otpExpire = DateTime.UtcNow.AddMinutes(5);
            var otpData = new { Code = otp, ExpireAt = otpExpire };
            HttpContext.Session.SetString("CUSTOMER_OTP", JsonSerializer.Serialize(otpData));

            return RedirectToAction("XacThucOTP");
        }

        [HttpGet]
        public IActionResult XacThucOTP()
        {
            var phone = HttpContext.Session.GetString("CUSTOMER_PHONE") ?? "";
            if (string.IsNullOrEmpty(phone))
            {
                return RedirectToAction("Index");
            }
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                ("Xác minh số điện thoại", Url.Action("Index")),
                ("Xác thực OTP", ""),
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> XacThucOTPAsync(TaiKhoan taiKhoanModel)
        {
            var json = HttpContext.Session.GetString("CUSTOMER_OTP") ?? "";
            if (string.IsNullOrEmpty(json))
            {
                TempData["Message"] = "OTP không hợp lệ";
                return RedirectToAction("Index");
            } else
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var codeInSession = root.GetProperty("Code").GetString();
                var expireAt = root.GetProperty("ExpireAt").GetDateTime();

                if (DateTime.UtcNow > expireAt)
                {
                    TempData["Message"] = "OTP đã hết hạn";
                    return RedirectToAction("XacThucOTP");
                }

                if (taiKhoanModel.Otp != codeInSession)
                {
                    TempData["Message"] = "OTP không đúng";
                    return RedirectToAction("XacThucOTP");
                }

                HttpContext.Session.Remove("CUSTOMER_OTP");
            }

            var phone = HttpContext.Session.GetString("CUSTOMER_PHONE") ?? "";

            var taiKhoan = _taiKhoanBLL.LayTaiKhoanBangSoDienThoai(phone);

            if (taiKhoan != null)
            {
                if (taiKhoan.TaiKhoan.TrangThaiTK != 1)
                {
                    TempData["Message"] = "Tài khoản đã bị khóa";
                    return RedirectToAction("Index");
                } else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, taiKhoan.KhachHang.MaKH.ToString() ?? ""), 
                        new Claim(ClaimTypes.Name, taiKhoan.KhachHang.TenKH ?? ""),              
                        new Claim(ClaimTypes.Role, taiKhoan.TaiKhoan.QuyenTK.ToString() ?? ""),
                        new Claim(ClaimTypes.MobilePhone, taiKhoan.KhachHang.SoDienThoaiKH ?? "")              
                    };

                    var identity = new ClaimsIdentity(claims, "VElectricCookie");

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("VElectricCookie", principal);

                    var returnUrl = HttpContext.Session.GetString("RETURN_URL");

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        HttpContext.Session.Remove("RETURN_URL");

                        return Redirect(returnUrl);
                    }
                    
                    HttpContext.Session.Remove("CUSTOMER_PHONE");
                    return RedirectToAction("Index", "SanPham");
                }
            }
            
            return RedirectToAction("DangKy");
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            var phone = HttpContext.Session.GetString("CUSTOMER_PHONE") ?? "";
            var otp = HttpContext.Session.GetString("CUSTOMER_OTP") ?? "";
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(otp))
            {
                return RedirectToAction("Index");
            }

            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                ("Xác minh số điện thoại", Url.Action("Index")),
                ("Đăng ký thông tin khách hàng", ""),
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangKyAsync(TaiKhoanKhachHang taiKhoanKhachHangModel)
        {
            var phone = HttpContext.Session.GetString("CUSTOMER_PHONE") ?? "";
            var tinh = taiKhoanKhachHangModel.DiaChiNhanHang.TinhTPChon.Split('|', 2);
            var xa = taiKhoanKhachHangModel.DiaChiNhanHang.XaPhuongChon.Split('|', 2);

            taiKhoanKhachHangModel.KhachHang.SoDienThoaiKH = phone;

            taiKhoanKhachHangModel.DiaChiNhanHang.TinhThanhPho.MaTTP = tinh[0];
            taiKhoanKhachHangModel.DiaChiNhanHang.TinhThanhPho.TenTTP = tinh[1];

            taiKhoanKhachHangModel.DiaChiNhanHang.XaPhuong.MaXP = xa[0];
            taiKhoanKhachHangModel.DiaChiNhanHang.XaPhuong.TenXP = xa[1];

            if(_taiKhoanBLL.TaoTaiKhoanKhachHang(taiKhoanKhachHangModel) <= 0)
            {
                TempData["Message"] = "Đăng ký thông tin khách hàng thất bại";
                return RedirectToAction("Index");
            }

            var taiKhoan = _taiKhoanBLL.LayTaiKhoanBangSoDienThoai(phone);

            if (taiKhoan == null)
            {
                TempData["Message"] = "Không tìm thấy tài khoản";
                return RedirectToAction("Index");
            }

            if (taiKhoan.TaiKhoan.TrangThaiTK != 1)
            {
                TempData["Message"] = "Tài khoản đã bị khóa";
                return RedirectToAction("Index");
            } 

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, taiKhoan.KhachHang.MaKH.ToString() ?? ""), 
                new Claim(ClaimTypes.Name, taiKhoan.KhachHang.TenKH ?? ""),              
                new Claim(ClaimTypes.Role, taiKhoan.TaiKhoan.QuyenTK.ToString() ?? ""),
                new Claim(ClaimTypes.MobilePhone, taiKhoan.KhachHang.SoDienThoaiKH ?? "")                
            };

            var identity = new ClaimsIdentity(claims, "VElectricCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("VElectricCookie", principal);

            var returnUrl = HttpContext.Session.GetString("RETURN_URL");

            if (!string.IsNullOrEmpty(returnUrl))
            {
                HttpContext.Session.Remove("RETURN_URL");

                return Redirect(returnUrl);
            }

            HttpContext.Session.Remove("CUSTOMER_PHONE");
            return RedirectToAction("Index", "SanPham");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync("VElectricCookie");
            HttpContext.Session.Remove("CUSTOMER_PHONE");
            return RedirectToAction("Index", "SanPham");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ThongTin()
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                ("Thông tin cá nhân", ""),
            };

            var phone = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";
            int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var taiKhoan = _taiKhoanBLL.LayTaiKhoanBangSoDienThoai(phone);
            
            if(taiKhoan != null)
            {
                var diaChi = _diaChiBLL.LayDanhSachDiaChi(maKH);
                taiKhoan.ListDiaChiNhanHang = diaChi;
                return View(taiKhoan);
            }

            return RedirectToAction("Index", "SanPham");
        }

        [HttpGet]
        [Authorize]
        public IActionResult LichSuMuaHang()
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                ("Lịch sử mua hàng", ""),
            };

            int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var dsDonHang = _donHangBLL.LayDanhSachDonHangBangMaKH(maKH);

            return View(dsDonHang);
        }

        [HttpPost]
        [Authorize]
        public IActionResult HuyDonHang(int maDH)
        {
            if(_donHangBLL.HuyDonHang(maDH) <= 0)
            {
                TempData["Message"] = "Hủy đơn hàng thất bại";
                return RedirectToAction("LichSuMuaHang");
            }

            TempData["Message"] = "Hủy đơn hàng thành công";
            return RedirectToAction("LichSuMuaHang");
        }

        [HttpGet]
        [Authorize]
        public IActionResult DiaChiNhanHang(int madc, string returnurl)
        {
            if (!string.IsNullOrEmpty(returnurl))
            {
                HttpContext.Session.SetString("ReturnUrl", returnurl);
            }

            if(madc > 0)
            {
                int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var diaChi = _diaChiBLL.LayDiaChi(madc, maKH);
                if(diaChi?.DuocSuaXoa != 1)
                {
                    return RedirectToAction("ThongTin");
                }

                ViewBag.Breadcrumb = new List<(string Text, string? Url)>
                {
                    ("Trang chủ", Url.Action("Index", "SanPham")),
                    ("Thông tin cá nhân", Url.Action("ThongTin")),
                    ("Chỉnh sửa địa chỉ", ""),
                };

                return View(diaChi);
            }

            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "SanPham")),
                ("Thông tin cá nhân", Url.Action("ThongTin")),
                ("Thêm địa chỉ", ""),
            };

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SuaThongTinCaNhanAsync(TaiKhoanKhachHang taiKhoanKhachHang)
        {
            int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            taiKhoanKhachHang.KhachHang.MaKH = maKH;

            if(_taiKhoanBLL.SuaThongTinKhachHang(taiKhoanKhachHang) <= 0)
            {
                TempData["Message"] = "Chỉnh sửa thông tin cá nhân thất bại";
                return RedirectToAction("ThongTin");
            }

            var phone = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";

            var taiKhoan = _taiKhoanBLL.LayTaiKhoanBangSoDienThoai(phone);

            if (taiKhoan == null)
            {
                TempData["Message"] = "Không tìm thấy tài khoản";
                return RedirectToAction("Index");
            }

            if (taiKhoan.TaiKhoan.TrangThaiTK != 1)
            {
                TempData["Message"] = "Tài khoản đã bị khóa";
                return RedirectToAction("Index");
            } 

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, taiKhoan.KhachHang.MaKH.ToString() ?? ""), 
                new Claim(ClaimTypes.Name, taiKhoan.KhachHang.TenKH ?? ""),              
                new Claim(ClaimTypes.Role, taiKhoan.TaiKhoan.QuyenTK.ToString() ?? ""),
                new Claim(ClaimTypes.MobilePhone, taiKhoan.KhachHang.SoDienThoaiKH ?? "")                
            };

            var identity = new ClaimsIdentity(claims, "VElectricCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("VElectricCookie", principal);
            
            TempData["Message"] = "Chỉnh sửa thông tin cá nhân thành công";

            return RedirectToAction("ThongTin");
        }

        [HttpPost]
        [Authorize]
        public IActionResult ThemSuaDiaChiNhanHang(DiaChiNhanHang diaChiNhanHangModel)
        {
            var tinh = diaChiNhanHangModel.TinhTPChon.Split('|', 2);
            var xa = diaChiNhanHangModel.XaPhuongChon.Split('|', 2);

            diaChiNhanHangModel.TinhThanhPho.MaTTP = tinh[0];
            diaChiNhanHangModel.TinhThanhPho.TenTTP = tinh[1];

            diaChiNhanHangModel.XaPhuong.MaXP = xa[0];
            diaChiNhanHangModel.XaPhuong.TenXP = xa[1];

            int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var danhSachDiaChi = _diaChiBLL.LayDanhSachDiaChi(maKH);

            bool tenDCCTTonTai;

            if(diaChiNhanHangModel.DiaChiCuThe.MaDCCT <= 0)
            {
                tenDCCTTonTai = danhSachDiaChi.Any(dc =>
                    dc.DiaChiCuThe.TenDCCT.Trim().Equals(diaChiNhanHangModel.DiaChiCuThe.TenDCCT.Trim(), StringComparison.OrdinalIgnoreCase)
                    && dc.TinhThanhPho.MaTTP == diaChiNhanHangModel.TinhThanhPho.MaTTP
                    && dc.XaPhuong.MaXP == diaChiNhanHangModel.XaPhuong.MaXP
                );
            } else
            {
                tenDCCTTonTai = danhSachDiaChi.Any(dc =>
                    dc.DiaChiCuThe.TenDCCT.Trim().Equals(diaChiNhanHangModel.DiaChiCuThe.TenDCCT.Trim(), StringComparison.OrdinalIgnoreCase)
                    && dc.TinhThanhPho.MaTTP == diaChiNhanHangModel.TinhThanhPho.MaTTP
                    && dc.XaPhuong.MaXP == diaChiNhanHangModel.XaPhuong.MaXP
                    && dc.DiaChiCuThe.MacDinhDCCT == diaChiNhanHangModel.DiaChiCuThe.MacDinhDCCT
                );
            }

            if (tenDCCTTonTai)
            {
                TempData["Message"] = "Địa chỉ đã tồn tại";
                if(diaChiNhanHangModel.DiaChiCuThe.MaDCCT <= 0)
                {
                    return RedirectToAction("DiaChiNhanHang");
                } else
                {
                    return RedirectToAction("DiaChiNhanHang", new { madc = diaChiNhanHangModel.DiaChiCuThe.MaDCCT });
                }
            }

            diaChiNhanHangModel.DiaChiCuThe.MaKH = maKH;

            if(diaChiNhanHangModel.DiaChiCuThe.MaDCCT <= 0)
            {
                if(_diaChiBLL.ThemDiaChiNhanHang(diaChiNhanHangModel) <= 0)
                {
                    TempData["Message"] = "Thêm địa chỉ thất bại";
                    return RedirectToAction("DiaChiNhanHang");
                }

                TempData["Message"] = "Thêm địa chỉ thành công";
            } else
            {
                if(_diaChiBLL.SuaDiaChiNhanHang(diaChiNhanHangModel) <= 0)
                {
                    TempData["Message"] = "Chỉnh sửa địa chỉ thất bại";
                    return RedirectToAction("DiaChiNhanHang", new { madc = diaChiNhanHangModel.DiaChiCuThe.MaDCCT });
                }

                TempData["Message"] = "Chỉnh sửa địa chỉ thành công";
            }

            var returnUrl = HttpContext.Session.GetString("RETURN_URL");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                HttpContext.Session.Remove("RETURN_URL");

                return Redirect(returnUrl);
            }

            return RedirectToAction("ThongTin");
        }

        [HttpPost]
        [Authorize]
        public IActionResult XoaDiaChi(int MaDCCT)
        {
            if(_diaChiBLL.XoaDiaChi(MaDCCT) <= 0)
            {
                TempData["Message"] = "Xóa địa chỉ thất bại";
                return RedirectToAction("ThongTin");
            }

            TempData["Message"] = "Xóa địa chỉ thành công";

            return RedirectToAction("ThongTin");
        }

        [HttpPost]
        [Authorize]
        public IActionResult DatDiaChiMacDinh(int MaDCCT, string returnurl)
        {
            int maKH = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if(_diaChiBLL.DatDiaChiMacDinh(maKH, MaDCCT) <= 0)
            {
                TempData["Message"] = "Đặt địa chỉ mặc định thất bại";
                return RedirectToAction("ThongTin");
            }

            TempData["Message"] = "Đặt địa chỉ mặc định thành công";

            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }

            return RedirectToAction("ThongTin");
        }
    }
}
