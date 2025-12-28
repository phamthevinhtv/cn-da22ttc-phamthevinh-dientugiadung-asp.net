using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.Controllers
{
    public class AdminController : Controller
    {
        private readonly AccountBLL _accountBLL;
        private readonly ProductBLL _productBLL;
        private readonly OrderBLL _orderBLL;
        private readonly ThuongHieuBLL _thuongHieuBLL;
        private readonly QuocGiaBLL _quocGiaBLL;
        private readonly ThongKeBLL _thongKeBLL;

        public AdminController(AccountBLL accountBLL, ProductBLL productBLL, OrderBLL orderBLL, ThuongHieuBLL thuongHieuBLL, QuocGiaBLL quocGiaBLL, ThongKeBLL thongKeBLL)
        {
            _accountBLL = accountBLL;
            _productBLL = productBLL;
            _orderBLL = orderBLL;
            _quocGiaBLL = quocGiaBLL;
            _thuongHieuBLL = thuongHieuBLL;
            _thongKeBLL = thongKeBLL;
        }

        [Authorize(Roles = "1")]
        public IActionResult Index(string loaiThongKe = "nam", int? nam = null, int? quy = null)
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Thống kê", Url.Action("Index", "Admin"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            var availableYears = _thongKeBLL.GetAvailableYears();
            if (!nam.HasValue && availableYears.Any())
            {
                nam = availableYears.First();
            }

            var filter = new ThongKeFilter
            {
                LoaiThongKe = loaiThongKe,
                Nam = nam,
                Quy = quy
            };

            var thongKeData = _thongKeBLL.GetThongKeDoanhThu(filter);

            ViewBag.Filter = filter;
            ViewBag.AvailableYears = availableYears;
            ViewBag.TongDoanhThu = _thongKeBLL.GetTongDoanhThu(thongKeData);
            ViewBag.TongDonHang = _thongKeBLL.GetTongDonHang(thongKeData);
            ViewBag.TongSanPhamBan = _thongKeBLL.GetTongSanPhamBan(thongKeData);

            return View(thongKeData);
        }

        [Authorize(Roles = "1")]
        public IActionResult Personal()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Tài khoản", Url.Action("Personal", "Admin"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            string tenTK = User.FindFirstValue(ClaimTypes.Name) ?? "";

            var taiKhoan = _accountBLL.GetTaiKhoanByTenTK(tenTK);

            return View(taiKhoan);
        }

        [Authorize(Roles = "1")]
        public IActionResult Product(int productTypeId, string? searchTerm)
        {
            List<SanPham> products;
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = _productBLL.GetAllProductsByNameForAdmin(searchTerm, productTypeId);
            }
            else
            {
                products = _productBLL.GetAllProductsOrigin(productTypeId);
            }

            var productTypeName = products.FirstOrDefault()?.TenLSP ?? "Sản phẩm"; 

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Sản phẩm", Url.Action("Product", "Admin"))
            };

            if (productTypeId != 0)
            {
                breadcrumb.Add(
                    (productTypeName, Url.Action("Product", "Admin", new { productTypeId }))
                );
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                breadcrumb.Add(
                    ($"Tìm kiếm: \"{searchTerm}\"", null)
                );
            }

            ViewData["Breadcrumb"] = breadcrumb;

            ViewBag.ProductTypes = _productBLL.GetAllProductTypesForCustomer();
            ViewBag.CurrentProductTypeId = productTypeId;
            ViewBag.SearchTerm = searchTerm;

            var canDeleteProducts = new Dictionary<int, bool>();
            foreach (var product in products)
            {
                canDeleteProducts[product.MaSP] = _productBLL.CanDeleteProduct(product.MaSP);
            }
            ViewBag.CanDeleteProducts = canDeleteProducts;

            return View(products);
        }

        [Authorize(Roles = "1")]
        public IActionResult ProductDetail(int productId, int productTypeId)
        {            
            var productType = _productBLL.GetAllProductTypesForCustomer()
                .Where(lsp => lsp.MaLSP == productTypeId)
                .FirstOrDefault();

            string productTypeName = productType?.TenLSP ?? "Sản phẩm";

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Sản phẩm", Url.Action("Product", "Admin"))
            };

            SanPham sanPham;

            if (productId > 0)
            {
                sanPham = _productBLL.GetProductWithDetailsForAdmin(productId);
                if (sanPham == null)
                {
                    TempData["Error"] = "Không tìm thấy sản phẩm";
                    return RedirectToAction("Product");
                }
                
                breadcrumb.Add((productTypeName, Url.Action("Product", "Admin", new { productTypeId = productTypeId })));
                breadcrumb.Add((sanPham!.TenSP!, null));
            }
            else
            {
                sanPham = new SanPham
                {
                    MaLSP = productTypeId
                };
                
                switch (productTypeId)
                {
                    case 1:
                        sanPham.MayLanh = new MayLanh { MaSP = 0 };
                        break;
                    case 2:
                        sanPham.TuLanh = new TuLanh { MaSP = 0 };
                        break;
                    case 3:
                        sanPham.MayLocKhongKhi = new MayLocKhongKhi();
                        break;
                    case 4:
                        sanPham.MayLocNuoc = new MayLocNuoc();
                        break;
                    case 5:
                        sanPham.MayRuaChen = new MayRuaChen();
                        break;
                    case 6:
                        sanPham.NoiComDien = new NoiComDien();
                        break;
                    case 7:
                        sanPham.NoiChien = new NoiChien();
                        break;
                    case 8:
                        sanPham.TiVi = new TiVi();
                        break;
                }
                
                breadcrumb.Add(("Thêm " + productTypeName.ToLower(), null));
            }

            ViewData["Breadcrumb"] = breadcrumb;
            ViewBag.ProductTypeId = productTypeId;
            ViewBag.ThuongHieu = _thuongHieuBLL.GetAllThuongHieu();
            ViewBag.QuocGia = _quocGiaBLL.GetAllQuocGia();
            
            if (productId > 0)
            {
                ViewBag.ProductImages = _productBLL.GetProductImages(productId);
            }
            else
            {
                ViewBag.ProductImages = new List<Anh>();
            }

            return View(sanPham);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult ProductDetail(SanPham model)
        {
            try
            {
                if (model.MaSP == 0)
                {
                    _productBLL.CreateProductWithStoredProc(model);
                    TempData["Message"] = "Thêm sản phẩm thành công";
                }
                else
                {
                    _productBLL.UpdateProductWithStoredProc(model);
                    TempData["Message"] = "Cập nhật sản phẩm thành công";
                }

                return RedirectToAction("ProductDetail", new { productTypeId = model.MaLSP, productId = model.MaSP });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                
                ViewBag.ProductTypeId = model.MaLSP;
                ViewBag.ThuongHieu = _thuongHieuBLL.GetAllThuongHieu();
                ViewBag.QuocGia = _quocGiaBLL.GetAllQuocGia();
                
                return View(model);
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> HandleUpdateTenTKAsync(string TenTK)
        {
            string oldTenTK = User.FindFirstValue(ClaimTypes.Name) ?? "";

            _accountBLL.UpdateTenTK(oldTenTK, TenTK);
            
            var taiKhoan = _accountBLL.GetTaiKhoanByTenTK(TenTK) ?? new TaiKhoan();
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, taiKhoan.MaTK.ToString() ?? ""), 
                new Claim(ClaimTypes.Name, taiKhoan.TenTK ?? ""),              
                new Claim(ClaimTypes.Role, taiKhoan.QuyenTK.ToString() ?? ""),
            };

            var identity = new ClaimsIdentity(claims, "VElectricCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("VElectricCookie", principal);

            TempData["Message"] = "Cập nhật tên đăng nhập thành công";

            return RedirectToAction("Personal", "Admin");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult HandleChangePassword(string MatKhauTK)
        {
            string tenTK = User.FindFirstValue(ClaimTypes.Name) ?? "";

            _accountBLL.ChangePassword(tenTK, MatKhauTK);

            TempData["Message"] = "Đổi mật khẩu thành công";

            return RedirectToAction("Personal", "Admin");
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult Order()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Đơn hàng", Url.Action("Oreder", "Admin"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            var donHangs = _orderBLL.GetOrders();

            ViewBag.TrangThaiDonHang = _orderBLL.GetAllTrangThaiDonHang();
            ViewBag.TrangThaiThanhToan = _orderBLL.GetAllTrangThaiThanhToan();

            return View(donHangs);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult UpdateTrangThaiDonHang(IFormCollection form)
        {
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("MaTTDH"))
                {
                    int maDH = int.Parse(key.Replace("MaTTDH", ""));
                    int maTTDH = int.Parse(form[key]!);

                    _orderBLL.UpdateTrangThaiDonHang(maDH, maTTDH);
                }
            }

            return RedirectToAction("Order");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        [HttpPost]
        public IActionResult UpdateTrangThaiThanhToan(IFormCollection form)
        {
            string? maGiaoDich = form["MaGiaoDichTT"];

            foreach (var key in form.Keys)
            {
                if (key.StartsWith("MaTTTT"))
                {
                    int maDH = int.Parse(key.Replace("MaTTTT", ""));
                    int maTTTT = int.Parse(form[key]!);

                    _orderBLL.UpdateTrangThaiThanhToan(maDH, maTTTT, maGiaoDich!);
                }
            }

            return RedirectToAction("Order");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Đăng nhập", Url.Action("Login", "Admin"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HandleLoginAsync(string userName, string password)
        {
            var taiKhoan = _accountBLL.GetTaiKhoanByTenTK(userName);

            if (taiKhoan == null)
            {
                TempData["Error"] = "Tên đăng nhập không tồn tại";
                return RedirectToAction("Login");
            } else
            {
                var hasher = new PasswordHasher<object>();
                var result = hasher.VerifyHashedPassword(
                    new object(),
                    (string)taiKhoan.MatKhauTK!,
                    password
                );

                if (result == PasswordVerificationResult.Failed)
                {
                    TempData["Error"] = "Sai mật khẩu";
                    return RedirectToAction("Login");
                } else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, taiKhoan.MaTK.ToString() ?? ""), 
                        new Claim(ClaimTypes.Name, taiKhoan.TenTK ?? ""),              
                        new Claim(ClaimTypes.Role, taiKhoan.QuyenTK.ToString() ?? ""),
                    };

                    var identity = new ClaimsIdentity(claims, "VElectricCookie");

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("VElectricCookie", principal);

                    return RedirectToAction("Index", "Admin");
                }
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> AddProductImage(int productId, int productTypeId, IFormFile imageFile, bool isDefault = false)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    TempData["Error"] = "Vui lòng chọn file ảnh";
                    return RedirectToAction("ProductDetail", new { productId, productTypeId });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["Error"] = "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif, .webp)";
                    return RedirectToAction("ProductDetail", new { productId, productTypeId });
                }

                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "File ảnh không được vượt quá 5MB";
                    return RedirectToAction("ProductDetail", new { productId, productTypeId });
                }

                var fileName = $"{productId}_{Guid.NewGuid()}{fileExtension}";
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", "products");
                
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                var imageUrl = $"/uploads/images/products/{fileName}";
                _productBLL.AddProductImage(productId, imageUrl, isDefault);
                
                TempData["Message"] = "Thêm ảnh thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction("ProductDetail", new { productId, productTypeId });
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult DeleteProductImage(int imageId, int productId, int productTypeId)
        {
            try
            {
                var images = _productBLL.GetProductImages(productId);
                var imageToDelete = images.FirstOrDefault(x => x.MaAnh == imageId);
                
                _productBLL.DeleteProductImage(imageId);
                
                if (imageToDelete != null && !string.IsNullOrEmpty(imageToDelete.UrlAnh) && 
                    imageToDelete.UrlAnh.StartsWith("/uploads/images/products/"))
                {
                    var fileName = Path.GetFileName(imageToDelete.UrlAnh);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", "products", fileName);
                    
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                
                TempData["Message"] = "Xóa ảnh thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction("ProductDetail", new { productId, productTypeId });
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult SetDefaultImage(int imageId, int productId, int productTypeId)
        {
            try
            {
                _productBLL.SetDefaultImage(imageId, productId);
                TempData["Message"] = "Đặt ảnh mặc định thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction("ProductDetail", new { productId, productTypeId });
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult DeleteProduct(int productId, int productTypeId)
        {
            try
            {
                if (!_productBLL.CanDeleteProduct(productId))
                {
                    TempData["Error"] = "Không thể xóa sản phẩm này vì đã có trong đơn hàng";
                    return RedirectToAction("Product", new { productTypeId });
                }

                var images = _productBLL.GetProductImages(productId);
                
                _productBLL.DeleteProduct(productId);
                
                foreach (var image in images)
                {
                    if (!string.IsNullOrEmpty(image.UrlAnh) && image.UrlAnh.StartsWith("/uploads/images/products/"))
                    {
                        var fileName = Path.GetFileName(image.UrlAnh);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", "products", fileName);
                        
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                
                TempData["Message"] = "Xóa sản phẩm thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction("Product", new { productTypeId });
        }

        [Authorize(Roles = "1")]
        public IActionResult Country()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Quốc gia", Url.Action("Country", "Admin"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            var countries = _quocGiaBLL.GetAllQuocGiaForAdmin();
            var canDeleteCountries = new Dictionary<int, bool>();
            foreach (var country in countries)
            {
                canDeleteCountries[country.MaQG] = _quocGiaBLL.CanDeleteQuocGia(country.MaQG);
            }
            ViewBag.CanDeleteCountries = canDeleteCountries;

            return View(countries);
        }

        [Authorize(Roles = "1")]
        public IActionResult CountryDetail(int id = 0)
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Quốc gia", Url.Action("Country", "Admin"))
            };

            QuocGia country;
            if (id > 0)
            {
                country = _quocGiaBLL.GetQuocGiaById(id) ?? new QuocGia();
                breadcrumb.Add((country.TenQG ?? "Quốc gia", null));
            }
            else
            {
                country = new QuocGia();
                breadcrumb.Add(("Thêm quốc gia", null));
            }

            ViewData["Breadcrumb"] = breadcrumb;
            return View(country);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CountryDetail(QuocGia model)
        {
            try
            {
                if (model.MaQG == 0)
                {
                    _quocGiaBLL.CreateQuocGia(model);
                    TempData["Message"] = "Thêm quốc gia thành công";
                }
                else
                {
                    _quocGiaBLL.UpdateQuocGia(model);
                    TempData["Message"] = "Cập nhật quốc gia thành công";
                }
                return RedirectToAction("Country");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return View(model);
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult DeleteCountry(int id)
        {
            try
            {
                if (!_quocGiaBLL.CanDeleteQuocGia(id))
                {
                    TempData["Error"] = "Không thể xóa quốc gia này vì đã có thương hiệu";
                    return RedirectToAction("Country");
                }

                _quocGiaBLL.DeleteQuocGia(id);
                TempData["Message"] = "Xóa quốc gia thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction("Country");
        }

        [Authorize(Roles = "1")]
        public IActionResult Brand()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Thương hiệu", Url.Action("Brand", "Admin"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            var brands = _thuongHieuBLL.GetAllThuongHieuForAdmin();
            var canDeleteBrands = new Dictionary<int, bool>();
            foreach (var brand in brands)
            {
                canDeleteBrands[brand.MaTH] = _thuongHieuBLL.CanDeleteThuongHieu(brand.MaTH);
            }
            ViewBag.CanDeleteBrands = canDeleteBrands;

            return View(brands);
        }

        [Authorize(Roles = "1")]
        public IActionResult BrandDetail(int id = 0)
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Thương hiệu", Url.Action("Brand", "Admin"))
            };

            ThuongHieu brand;
            if (id > 0)
            {
                brand = _thuongHieuBLL.GetThuongHieuById(id) ?? new ThuongHieu();
                breadcrumb.Add((brand.TenTH ?? "Thương hiệu", null));
            }
            else
            {
                brand = new ThuongHieu();
                breadcrumb.Add(("Thêm thương hiệu", null));
            }

            ViewData["Breadcrumb"] = breadcrumb;
            ViewBag.QuocGia = _quocGiaBLL.GetAllQuocGia();
            return View(brand);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult BrandDetail(ThuongHieu model)
        {
            try
            {
                if (model.MaTH == 0)
                {
                    _thuongHieuBLL.CreateThuongHieu(model);
                    TempData["Message"] = "Thêm thương hiệu thành công";
                }
                else
                {
                    _thuongHieuBLL.UpdateThuongHieu(model);
                    TempData["Message"] = "Cập nhật thương hiệu thành công";
                }
                return RedirectToAction("Brand");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.QuocGia = _quocGiaBLL.GetAllQuocGia();
                return View(model);
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult DeleteBrand(int id)
        {
            try
            {
                if (!_thuongHieuBLL.CanDeleteThuongHieu(id))
                {
                    TempData["Error"] = "Không thể xóa thương hiệu này vì đã có sản phẩm";
                    return RedirectToAction("Brand");
                }

                _thuongHieuBLL.DeleteThuongHieu(id);
                TempData["Message"] = "Xóa thương hiệu thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction("Brand");
        }
    }
}