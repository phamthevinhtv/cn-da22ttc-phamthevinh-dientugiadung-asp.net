using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using QL_DienTuGiaDung.Models;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace QL_DienTuGiaDung.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountBLL _accountBLL;

        private readonly OrderBLL _orderBLL;

        public AccountController(AccountBLL accountBLL, OrderBLL orderBLL)
        {
            _accountBLL = accountBLL;
            _orderBLL = orderBLL;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Product");
            }

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "Product")),
                ("Xác minh số điện thoại", Url.Action("Index", "Account"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HandlePhoneAsync(string phone)
        {
            string otp = _accountBLL.GenerateOTP();

            bool success = await _accountBLL.SendOTPAsync(phone, otp);
            if (!success)
            {
                TempData["Error"] = "Gửi OTP thất bại";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("CUSTOMER_PHONE", phone);
            var otpExpire = DateTime.UtcNow.AddMinutes(5);
            var otpData = new { Code = otp, ExpireAt = otpExpire };
            HttpContext.Session.SetString("CUSTOMER_OTP", JsonSerializer.Serialize(otpData));

            return RedirectToAction("VerifyOTP");
        }

         [HttpGet]
        public IActionResult VerifyOTP()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Product");
            }

            var phone = HttpContext.Session.GetString("CUSTOMER_PHONE") ?? "";
            if (string.IsNullOrEmpty(phone))
            {
                return RedirectToAction("Index");
            }

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "Product")),
                ("Xác minh số điện thoại", Url.Action("Index", "Account")),
                ("Xác thực OTP", Url.Action("Index", "VerifyOTP"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HandleOTPAsync(string otp)
        {
            var json = HttpContext.Session.GetString("CUSTOMER_OTP") ?? "";
            if (string.IsNullOrEmpty(json))
            {
                TempData["Error"] = "OTP không hợp lệ";
                return RedirectToAction("Index");
            } else
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var codeInSession = root.GetProperty("Code").GetString();
                var expireAt = root.GetProperty("ExpireAt").GetDateTime();

                if (DateTime.UtcNow > expireAt)
                {
                    TempData["Error"] = "OTP đã hết hạn";
                    return RedirectToAction("VerifyOTP");
                }

                if (otp != codeInSession)
                {
                    TempData["Error"] = "OTP không đúng";
                    return RedirectToAction("VerifyOTP");
                }

                HttpContext.Session.Remove("CUSTOMER_OTP");
            }

            var phone = HttpContext.Session.GetString("CUSTOMER_PHONE") ?? "";

            var customer = _accountBLL.GetCustomer(phone);

            if (customer != null)
            {
                if (customer.TrangThaiTK != 1)
                {
                    TempData["Error"] = "Tài khoản đã bị khóa";
                    return RedirectToAction("Index");
                } else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, customer.MaTK.ToString() ?? ""), 
                        new Claim(ClaimTypes.Name, customer.TenKH ?? ""),              
                        new Claim(ClaimTypes.Role, customer.QuyenTK.ToString() ?? ""),
                        new Claim(ClaimTypes.MobilePhone, customer.SoDienThoaiKH ?? "")              
                    };

                    var identity = new ClaimsIdentity(claims, "VElectricCookie");

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("VElectricCookie", principal);

                    var returnUrl = HttpContext.Session.GetString("RETURN_URL");

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        HttpContext.Session.Remove("RETURN_URL");

                        string[] parts = returnUrl.Split('|');

                        if (parts.Length == 3)
                        {
                            return RedirectToAction(parts[1], parts[0], new { id = parts[2] });
                        }
                        return RedirectToAction(parts[1], parts[0]);
                    }

                    return RedirectToAction("Index", "Product");
                }
            }

            return RedirectToAction("Register");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Product");
            }

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "Product")),
                ("Xác minh số điện thoại", Url.Action("Index", "Account")),
                ("Đăng ký thông tin khách hàng", Url.Action("Index", "Register"))
            };

            ViewData["Breadcrumb"] = breadcrumb;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HandleRegisterAsync(TaiKhoanKhachHang taiKhoanKhachHang, string provinceSelect, string communeSelect)
        {
            var phone = HttpContext.Session.GetString("CUSTOMER_PHONE") ?? "";
            var province = provinceSelect.Split('|', 2);
            var cummune = communeSelect.Split('|', 2);

            taiKhoanKhachHang.SoDienThoaiKH = phone;

            taiKhoanKhachHang.DiaChiCuThe ??= new DiaChiCuThe();
            taiKhoanKhachHang.DiaChiCuThe.MaDCCT = 1;

            taiKhoanKhachHang.TinhThanhPho ??= new TinhThanhPho();
            taiKhoanKhachHang.TinhThanhPho.MaTTP = province[0];
            taiKhoanKhachHang.TinhThanhPho.TenTTP = province[1];

            taiKhoanKhachHang.XaPhuong ??= new XaPhuong();
            taiKhoanKhachHang.XaPhuong.MaXP = cummune[0];
            taiKhoanKhachHang.XaPhuong.TenXP = cummune[1];

            _accountBLL.CreateCustomer(taiKhoanKhachHang);
            
            var customer = _accountBLL.GetCustomer(phone);

            if (customer != null)
            {
                if (customer.TrangThaiTK != 1)
                {
                    TempData["Error"] = "Tài khoản đã bị khóa";
                    return RedirectToAction("Index");
                } else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, customer.MaTK.ToString() ?? ""), 
                        new Claim(ClaimTypes.Name, customer.TenKH ?? ""),              
                        new Claim(ClaimTypes.Role, customer.QuyenTK.ToString() ?? ""),
                        new Claim(ClaimTypes.MobilePhone, customer.SoDienThoaiKH ?? "")              
                    };

                    var identity = new ClaimsIdentity(claims, "VElectricCookie");

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("VElectricCookie", principal);

                    TempData["Message"] = "Đăng ký thành công";

                    var returnUrl = HttpContext.Session.GetString("RETURN_URL");

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        HttpContext.Session.Remove("RETURN_URL");

                        string[] parts = returnUrl.Split('|');

                        if (parts.Length == 3)
                        {
                            return RedirectToAction(parts[1], parts[0], new { id = parts[3] });
                        }
                        return RedirectToAction(parts[1], parts[0]);
                    }

                    return RedirectToAction("Index", "Product");
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Personal()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "Product")),
                ("Cá nhân", Url.Action("Personal", "Account"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            string phone = User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";

            var customer = _accountBLL.GetCustomerPersonal(phone);
            return View(customer);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> HandleUpdateProfileAsync(TaiKhoanKhachHang taiKhoanKhachHang)
        {
            _accountBLL.UpdateCustomer(taiKhoanKhachHang);

            var phone = User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";

            var customer = _accountBLL.GetCustomer(phone);

            if (customer != null)
            {
                if (customer.TrangThaiTK != 1)
                {
                    TempData["Error"] = "Tài khoản đã bị khóa";
                    return RedirectToAction("Index");
                } else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, customer.MaTK.ToString() ?? ""), 
                        new Claim(ClaimTypes.Name, customer.TenKH ?? ""),              
                        new Claim(ClaimTypes.Role, customer.QuyenTK.ToString() ?? ""),
                        new Claim(ClaimTypes.MobilePhone, customer.SoDienThoaiKH ?? "")              
                    };

                    var identity = new ClaimsIdentity(claims, "VElectricCookie");

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("VElectricCookie", principal);
                }
            }

            TempData["Message"] = "Cập nhật thông tin thành công";
            return RedirectToAction("Personal");
        }

        [Authorize]
        [HttpPost]
        public IActionResult HandleDeleteAddress(int MaDCCT)
        {
            _accountBLL.DeleteAddress(MaDCCT);
            
            TempData["Message"] = "Xóa địa chỉ thành công";
            
            return RedirectToAction("Personal");
        }

        [Authorize]
        [HttpPost]
        public IActionResult HandleAddUpdateAddress(string TenDCCT, string provinceSelect, string communeSelect, int MaDCCT)
        {
            var taiKhoanKhachHang = new TaiKhoanKhachHang();

            var phone = User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";
            var province = provinceSelect.Split('|', 2);
            var cummune = communeSelect.Split('|', 2);

            taiKhoanKhachHang.SoDienThoaiKH = phone;

            taiKhoanKhachHang.DiaChiCuThe ??= new DiaChiCuThe();
            taiKhoanKhachHang.DiaChiCuThe.MaDCCT = MaDCCT;
            taiKhoanKhachHang.DiaChiCuThe.TenDCCT = TenDCCT;

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

            return RedirectToAction("Personal");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("VElectricCookie");

            return RedirectToAction("Index", "Product");
        }

        [Authorize]
        [HttpGet]
        public IActionResult History()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "Product")),
                ("Lịch sửa mua hàng", Url.Action("History", "Account"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            var phone = User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";

            var orders = _orderBLL.GetOrdersByPhone(phone);

            return View(orders);
        }

        [Authorize]
        [HttpPost]
        public IActionResult HandleCancelOrder(int MaDH)
        {
            _orderBLL.CancelOrder(MaDH);

            TempData["Message"] = $"Hủy đơn hàng VE{MaDH} thành công";

            return RedirectToAction("History", "Account");
        }

        public IActionResult Denied()
        {
            return RedirectToAction("Index", "Product");
        }
    }
}
