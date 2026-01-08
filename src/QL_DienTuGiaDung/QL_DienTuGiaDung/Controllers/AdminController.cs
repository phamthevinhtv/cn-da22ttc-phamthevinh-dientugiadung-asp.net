using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.Controllers
{
    public class AdminController : Controller
    {
         private readonly TaiKhoanBLL _taiKhoanBLL;
         private readonly ThongKeBLL _thongKeBLL;
         private readonly QuocGiaBLL _quocGiaBLL;
         private readonly ThuongHieuBLL _thuongHieuBLL;
         private readonly DonHangBLL _donHangBLL;
         private readonly LoaiSanPhamBLL _loaiSanPhamBLL;
         private readonly SanPhamBLL _sanPhamBLL;
         private readonly AnhBLL _anhBLL;

        public AdminController(TaiKhoanBLL taiKhoanBLL, ThongKeBLL thongKeBLL, QuocGiaBLL quocGiaBLL, ThuongHieuBLL thuongHieuBLL, DonHangBLL donHangBLL, LoaiSanPhamBLL loaiSanPhamBLL, SanPhamBLL sanPhamBLL, AnhBLL anhBLL)
        {
            _taiKhoanBLL = taiKhoanBLL;
            _thongKeBLL = thongKeBLL;
            _quocGiaBLL = quocGiaBLL;
            _thuongHieuBLL = thuongHieuBLL;
            _donHangBLL = donHangBLL;
            _loaiSanPhamBLL = loaiSanPhamBLL;
            _sanPhamBLL = sanPhamBLL;
            _anhBLL = anhBLL;
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult Index(string loaiThongKe = "nam", int? nam = null, int? quy = null)
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "Admin")),
                ("Thống kê", ""),
            };

            var filter = new ThongKeFilter 
            { 
                LoaiThongKe = loaiThongKe,
                Nam = nam,
                Quy = quy
            };

            var thongKe = _thongKeBLL.LayThongKeDoanhThu(filter);
            var cacNam = _thongKeBLL.LayCacNamTonTai();

            var tongDoanhThu = _thongKeBLL.LayTongDoanhThu(thongKe);
            var tongDonHang = _thongKeBLL.LayTongDonHang(thongKe);
            var tongSanPhamBan = _thongKeBLL.LayTongSanPhamBan(thongKe);

            ViewBag.Filter = filter;
            ViewBag.AvailableYears = cacNam;
            ViewBag.TongDoanhThu = tongDoanhThu;
            ViewBag.TongDonHang = tongDonHang;
            ViewBag.TongSanPhamBan = tongSanPhamBan;

            return View(thongKe);
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult DonHang()
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Đơn hàng", Url.Action("DonHang", "Admin"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            var donHangs = _donHangBLL.LayDanhSachDonHang(1);

            ViewBag.TrangThaiDonHang = _donHangBLL.LayDanhSachTrangThaiDonHang();
            ViewBag.TrangThaiThanhToan = _donHangBLL.LayDanhSachTrangThaiThanhToan();

            return View(donHangs);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CapNhatTrangThaiDonHang(IFormCollection form)
        {
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("MaTTDH"))
                {
                    int maDH = int.Parse(key.Replace("MaTTDH", ""));
                    int maTTDH = int.Parse(form[key]!);

                    int result = _donHangBLL.CapNhatTrangThaiDonHang(maDH, maTTDH);
                    TempData["Message"] = result == 1 ? "Cập nhật trạng thái đơn hàng thành công" : "Cập nhật trạng thái đơn hàng thất bại";
                }
            }

            return RedirectToAction("DonHang");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CapNhatTrangThaiThanhToan(IFormCollection form)
        {
            string? maGiaoDich = form["MaGiaoDichTT"];

            foreach (var key in form.Keys)
            {
                if (key.StartsWith("MaTTTT"))
                {
                    int maDH = int.Parse(key.Replace("MaTTTT", ""));
                    int maTTTT = int.Parse(form[key]!);

                    int result = _donHangBLL.CapNhatTrangThaiThanhToan(maDH, maTTTT, maGiaoDich!);
                    TempData["Message"] = result == 1 ? "Cập nhật trạng thái thanh toán thành công" : "Cập nhật trạng thái thanh toán thất bại";
                }
            }

            return RedirectToAction("DonHang");
        }

        [HttpGet]
        public IActionResult DangNhap()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang chủ", Url.Action("Index", "Admin")),
                ("Đăng nhập trang quản trị", ""),
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhapAsync(TaiKhoan taiKhoanModel)
        {
            var taiKhoan = _taiKhoanBLL.LayTaiKhoanBangTenDangNhap(taiKhoanModel.TenTK.Trim());

            if (taiKhoan == null)
            {
                TempData["Message"] = "Tên đăng nhập không tồn tại";
                return RedirectToAction("DangNhap");
            } else
            {
                var hasher = new PasswordHasher<object>();
                var result = hasher.VerifyHashedPassword(
                    new object(),
                    (string)taiKhoan.MatKhauTK!,
                    taiKhoanModel.MatKhauTK.Trim()
                );

                if (result == PasswordVerificationResult.Failed)
                {
                    TempData["Message"] = "Sai mật khẩu";
                    return RedirectToAction("DangNhap");
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
        public IActionResult QuocGia()
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Quốc gia", ""),
            };

            var danhSachQuocGia = _quocGiaBLL.LayDanhSachQuocGia(1);
            var coTheSuaXoa = new Dictionary<int, bool>();
            
            foreach (var quocGia in danhSachQuocGia)
            {
                coTheSuaXoa[quocGia.MaQG] = _quocGiaBLL.KiemTraCoTheSuaXoa(quocGia.MaQG);
            }
            
            ViewBag.CoTheSuaXoa = coTheSuaXoa;

            return View(danhSachQuocGia);
        }

        [Authorize(Roles = "1")]
        public IActionResult ChiTietQuocGia(int maQG = 0)
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Quốc gia", Url.Action("QuocGia", "Admin"))
            };

            QuocGia quocGia;
            if (maQG > 0)
            {
                quocGia = _quocGiaBLL.LayQuocGia(maQG) ?? new QuocGia();
                breadcrumb.Add((quocGia.TenQG, ""));
            }
            else
            {
                quocGia = new QuocGia();
                breadcrumb.Add(("Thêm quốc gia", ""));
            }

            ViewBag.Breadcrumb = breadcrumb;
            return View(quocGia);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult ChiTietQuocGia(QuocGia model)
        {
            if (model.MaQG == 0)
            {
                var result = _quocGiaBLL.ThemQuocGia(model.TenQG);
                if (result > 0)
                {
                    TempData["Message"] = "Thêm quốc gia thành công";
                }
                else
                {
                    TempData["Message"] = "Thêm quốc gia thất bại";
                }
            }
            else
            {
                var result = _quocGiaBLL.CapNhatQuocGia(model.MaQG, model.TenQG);
                if (result > 0)
                {
                    TempData["Message"] = "Cập nhật quốc gia thành công";
                }
                else
                {
                    TempData["Message"] = "Cập nhật quốc gia thất bại";
                }
            }
            return RedirectToAction("QuocGia");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult XoaQuocGia(int maQG)
        {
            if (!_quocGiaBLL.KiemTraCoTheSuaXoa(maQG))
            {
                TempData["Message"] = "Không thể xóa quốc gia này vì đang được sử dụng";
                return RedirectToAction("QuocGia");
            }

            var result = _quocGiaBLL.XoaQuocGia(maQG);
            if (result > 0)
            {
                TempData["Message"] = "Xóa quốc gia thành công";
            }
            else
            {
                TempData["Message"] = "Xóa quốc gia thất bại";
            }

            return RedirectToAction("QuocGia");
        }

        [Authorize(Roles = "1")]
        public IActionResult ThuongHieu()
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Danh mục thương hiệu", ""),
            };

            var danhSachThuongHieu = _thuongHieuBLL.LayDanhSachThuongHieu(1);
            var coTheSuaXoa = new Dictionary<int, bool>();
            
            foreach (var thuongHieu in danhSachThuongHieu)
            {
                coTheSuaXoa[thuongHieu.MaTH] = _thuongHieuBLL.KiemTraCoTheSuaXoa(thuongHieu.MaTH);
            }
            
            ViewBag.CoTheSuaXoa = coTheSuaXoa;

            return View(danhSachThuongHieu);
        }

        [Authorize(Roles = "1")]
        public IActionResult ChiTietThuongHieu(int maTH = 0)
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Danh mục thương hiệu", Url.Action("ThuongHieu", "Admin"))
            };

            ThuongHieu thuongHieu;
            if (maTH > 0)
            {
                thuongHieu = _thuongHieuBLL.LayThuongHieu(maTH) ?? new ThuongHieu();
                breadcrumb.Add((thuongHieu.TenTH, ""));
            }
            else
            {
                thuongHieu = new ThuongHieu();
                breadcrumb.Add(("Thêm thương hiệu", ""));
            }

            var danhSachQuocGia = _quocGiaBLL.LayDanhSachQuocGia(1);
            ViewBag.DanhSachQuocGia = danhSachQuocGia;
            ViewBag.Breadcrumb = breadcrumb;
            return View(thuongHieu);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult ChiTietThuongHieu(ThuongHieu model)
        {
            if (model.MaTH == 0)
            {
                var result = _thuongHieuBLL.ThemThuongHieu(model.MaQG, model.TenTH);
                if (result > 0)
                {
                    TempData["Message"] = "Thêm thương hiệu thành công";
                }
                else
                {
                    TempData["Message"] = "Thêm thương hiệu thất bại";
                }
            }
            else
            {
                var result = _thuongHieuBLL.CapNhatThuongHieu(model.MaTH, model.MaQG, model.TenTH);
                if (result > 0)
                {
                    TempData["Message"] = "Cập nhật thương hiệu thành công";
                }
                else
                {
                    TempData["Message"] = "Cập nhật thương hiệu thất bại";
                }
            }
            return RedirectToAction("ThuongHieu");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult XoaThuongHieu(int maTH)
        {
            if (!_thuongHieuBLL.KiemTraCoTheSuaXoa(maTH))
            {
                TempData["Message"] = "Không thể xóa thương hiệu này vì đang được sử dụng";
                return RedirectToAction("ThuongHieu");
            }

            var result = _thuongHieuBLL.XoaThuongHieu(maTH);
            if (result > 0)
            {
                TempData["Message"] = "Xóa thương hiệu thành công";
            }
            else
            {
                TempData["Message"] = "Xóa thương hiệu thất bại";
            }

            return RedirectToAction("ThuongHieu");
        }

        [Authorize(Roles = "1")]
        public IActionResult LoaiSanPham()
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Danh mục loại sản phẩm", ""),
            };

            var danhSachLoaiSanPham = _loaiSanPhamBLL.LayDanhSachLoaiSanPham(1);
            var coTheSuaXoa = new Dictionary<int, bool>();
            
            foreach (var loaiSanPham in danhSachLoaiSanPham)
            {
                coTheSuaXoa[loaiSanPham.MaLSP] = _loaiSanPhamBLL.KiemTraCoTheSuaXoa(loaiSanPham.MaLSP);
            }
            
            ViewBag.CoTheSuaXoa = coTheSuaXoa;

            return View(danhSachLoaiSanPham);
        }

        [Authorize(Roles = "1")]
        public IActionResult ChiTietLoaiSanPham(int maLSP = 0)
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Danh mục loại sản phẩm", Url.Action("LoaiSanPham", "Admin"))
            };

            LoaiSanPham loaiSanPham;
            if (maLSP > 0)
            {
                loaiSanPham = _loaiSanPhamBLL.LayLoaiSanPham(maLSP) ?? new LoaiSanPham();
                breadcrumb.Add((loaiSanPham.TenLSP, ""));
            }
            else
            {
                loaiSanPham = new LoaiSanPham();
                breadcrumb.Add(("Thêm loại sản phẩm", ""));
            }

            ViewBag.Breadcrumb = breadcrumb;
            return View(loaiSanPham);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult ChiTietLoaiSanPham(LoaiSanPham model)
        {
            if (_loaiSanPhamBLL.KiemTraTenLoaiSanPhamTonTai(model.TenLSP, model.MaLSP))
            {
                TempData["Message"] = "Tên loại sản phẩm đã tồn tại";
                return RedirectToAction("ChiTietLoaiSanPham", new { maLSP = model.MaLSP });
            }

            if (model.MaLSP == 0)
            {
                var result = _loaiSanPhamBLL.ThemLoaiSanPham(model.TenLSP, model.ThueGTGTLSP);
                if (result > 0)
                {
                    TempData["Message"] = "Thêm loại sản phẩm thành công";
                }
                else
                {
                    TempData["Message"] = "Thêm loại sản phẩm thất bại";
                }
            }
            else
            {
                var result = _loaiSanPhamBLL.CapNhatLoaiSanPham(model.MaLSP, model.TenLSP, model.ThueGTGTLSP, model.TrangThaiLSP);
                if (result > 0)
                {
                    TempData["Message"] = "Cập nhật loại sản phẩm thành công";
                }
                else
                {
                    TempData["Message"] = "Cập nhật loại sản phẩm thất bại";
                }
            }
            return RedirectToAction("LoaiSanPham");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult XoaLoaiSanPham(int maLSP)
        {
            if (!_loaiSanPhamBLL.KiemTraCoTheSuaXoa(maLSP))
            {
                TempData["Message"] = "Không thể xóa loại sản phẩm này vì đang được sử dụng";
                return RedirectToAction("LoaiSanPham");
            }

            var result = _loaiSanPhamBLL.XoaLoaiSanPham(maLSP);
            if (result > 0)
            {
                TempData["Message"] = "Xóa loại sản phẩm thành công";
            }
            else
            {
                TempData["Message"] = "Xóa loại sản phẩm thất bại";
            }

            return RedirectToAction("LoaiSanPham");
        }

        [Authorize(Roles = "1")]
        public IActionResult SanPham(string searchTerm = "", int productTypeId = 0)
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Danh mục sản phẩm", ""),
            };

            var danhSachSanPham = _sanPhamBLL.LayDanhSachSanPham(1, productTypeId, searchTerm);
            var coTheSuaXoa = new Dictionary<int, bool>();
            
            foreach (var sanPham in danhSachSanPham)
            {
                coTheSuaXoa[sanPham.MaSP] = _sanPhamBLL.KiemTraCoTheSuaXoaSanPham(sanPham.MaSP);
            }
            
            ViewBag.CoTheSuaXoa = coTheSuaXoa;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CurrentProductTypeId = productTypeId;
            ViewBag.ProductTypes = _loaiSanPhamBLL.LayDanhSachLoaiSanPham(1);

            return View(danhSachSanPham);
        }

        [Authorize(Roles = "1")]
        public IActionResult ChiTietSanPham(int maSP = 0)
        {
            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Danh mục sản phẩm", Url.Action("SanPham", "Admin"))
            };

            SanPham sanPham;
            if (maSP > 0)
            {
                sanPham = _sanPhamBLL.LaySanPham(1, maSP) ?? new SanPham();
                breadcrumb.Add((sanPham.TenSP, ""));
            }
            else
            {
                sanPham = new SanPham();
                breadcrumb.Add(("Thêm sản phẩm", ""));
            }

            var danhSachQuocGia = _quocGiaBLL.LayDanhSachQuocGia(1);
            var danhSachThuongHieu = _thuongHieuBLL.LayDanhSachThuongHieu(1);
            var danhSachLoaiSanPham = _loaiSanPhamBLL.LayDanhSachLoaiSanPham(1);

            ViewBag.DanhSachQuocGia = danhSachQuocGia;
            ViewBag.DanhSachThuongHieu = danhSachThuongHieu;
            ViewBag.DanhSachLoaiSanPham = danhSachLoaiSanPham;
            ViewBag.Breadcrumb = breadcrumb;
            
            return View(sanPham);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult ChiTietSanPham(IFormCollection form)
        {
            if (!int.TryParse(form["MaLSP"], out int maLSP))
            {
                TempData["Message"] = "Loại sản phẩm không hợp lệ";
                return RedirectToAction("SanPham");
            }

            SanPham model = maLSP switch
            {
                1 => new MayLanh(),
                2 => new TuLanh(),
                3 => new MayLocKhongKhi(),
                4 => new MayLocNuoc(),
                5 => new MayRuaChen(),
                6 => new NoiComDien(),
                7 => new NoiChien(),
                8 => new TiVi(),
                _ => new SanPham()
            };

            if (int.TryParse(form["MaSP"], out int maSP)) model.MaSP = maSP;
            if (int.TryParse(form["MaQG"], out int maQG)) model.MaQG = maQG;
            if (int.TryParse(form["MaTH"], out int maTH)) model.MaTH = maTH;
            model.MaLSP = maLSP;
            model.TenSP = form["TenSP"].ToString().Trim();
            if (int.TryParse(form["SoLuongSP"], out int soLuong)) model.SoLuongSP = soLuong;
            if (decimal.TryParse(form["GiaNhapSP"], out decimal giaNhap)) model.GiaNhapSP = giaNhap;
            if (decimal.TryParse(form["GiaGocSP"], out decimal giaGoc)) model.GiaGocSP = giaGoc;
            model.PhanLoaiSP = form["PhanLoaiSP"];
            if (int.TryParse(form["NamSanXuatSP"], out int namSX)) model.NamSanXuatSP = namSX;
            model.BaoHanhSP = form["BaoHanhSP"];
            model.KichThuocSP = form["KichThuocSP"];
            model.KhoiLuongSP = form["KhoiLuongSP"];
            model.CongSuatTieuThuSP = form["CongSuatTieuThuSP"];
            model.ChatLieuSP = form["ChatLieuSP"];
            model.TienIchSP = form["TienIchSP"];
            model.CongNgheSP = form["CongNgheSP"];
            if (decimal.TryParse(form["MucGiamGiaSP"], out decimal mucGiam)) 
            {
                model.MucGiamGiaSP = mucGiam;
            }
            else
            {
                model.MucGiamGiaSP = 0;
            }
            if (DateTime.TryParse(form["NgayHetGiamGiaSP"], out DateTime ngayHet)) model.NgayHetGiamGiaSP = ngayHet;
            if (int.TryParse(form["TrangThaiSP"], out int trangThai)) model.TrangThaiSP = trangThai;

            switch (model)
            {
                case MayLanh ml:
                    ml.CongSuatLamLanhML = form["CongSuatLamLanhML"];
                    ml.PhamViLamLanhML = form["PhamViLamLanhML"];
                    ml.DoOnML = form["DoOnML"];
                    ml.LoaiGasML = form["LoaiGasML"];
                    ml.CheDoGioML = form["CheDoGioML"];
                    break;
                case TuLanh tl:
                    tl.DungTichNganDaTL = form["DungTichNganDaTL"];
                    tl.DungTichNganLanhTL = form["DungTichNganLanhTL"];
                    tl.LayNuocNgoaiTL = form["LayNuocNgoaiTL"];
                    tl.LayDaTuDongTL = form["LayDaTuDongTL"];
                    break;
                case MayLocKhongKhi mlkk:
                    mlkk.LoaiBuiLocDuocMLKK = form["LoaiBuiLocDuocMLKK"];
                    mlkk.PhamViLocMLKK = form["PhamViLocMLKK"];
                    mlkk.LuongGioMLKK = form["LuongGioMLKK"];
                    mlkk.MangLocMLKK = form["MangLocMLKK"];
                    mlkk.BangDieuKhienMLKK = form["BangDieuKhienMLKK"];
                    mlkk.DoOnMLKK = form["DoOnMLKK"];
                    mlkk.CamBienMLKK = form["CamBienMLKK"];
                    break;
                case MayLocNuoc mln:
                    mln.KieuLapMLN = form["KieuLapMLN"];
                    mln.CongSuatLocMLN = form["CongSuatLocMLN"];
                    mln.TiLeLocThaiMLN = form["TiLeLocThaiMLN"];
                    mln.ChiSoNuocMLN = form["ChiSoNuocMLN"];
                    mln.DoPHThucTeMLN = form["DoPHThucTeMLN"];
                    mln.ApLucNuocYeuCauMLN = form["ApLucNuocYeuCauMLN"];
                    if (int.TryParse(form["SoLoiLocMLN"], out int soLoi)) mln.SoLoiLocMLN = soLoi;
                    mln.BangDieuKhienMLN = form["BangDieuKhienMLN"];
                    break;
                case MayRuaChen mrc:
                    mrc.NuocTieuThuMRC = form["NuocTieuThuMRC"];
                    mrc.SoChenRuaDuocMRC = form["SoChenRuaDuocMRC"];
                    mrc.DoOnMRC = form["DoOnMRC"];
                    mrc.BangDieuKhienMRC = form["BangDieuKhienMRC"];
                    mrc.ChieuDaiOngCapNuocMRC = form["ChieuDaiOngCapNuocMRC"];
                    mrc.ChieuDaiOngThoatNuocMRC = form["ChieuDaiOngThoatNuocMRC"];
                    break;
                case NoiComDien ncd:
                    ncd.DungTichNCD = form["DungTichNCD"];
                    ncd.ChucNangNCD = form["ChucNangNCD"];
                    ncd.DoDayNCD = form["DoDayNCD"];
                    ncd.DieuKhienNCD = form["DieuKhienNCD"];
                    ncd.ChieuDaiDayDienNCD = form["ChieuDaiDayDienNCD"];
                    break;
                case NoiChien nc:
                    nc.DungTichTongNC = form["DungTichTongNC"];
                    nc.DungTichSuDungNC = form["DungTichSuDungNC"];
                    nc.NhietDoNC = form["NhietDoNC"];
                    nc.HenGioNC = form["HenGioNC"];
                    nc.BangDieuKhienNC = form["BangDieuKhienNC"];
                    nc.ChieuDaiDayDienNC = form["ChieuDaiDayDienNC"];
                    break;
                case TiVi tv:
                    tv.CoManHinhTV = form["CoManHinhTV"];
                    tv.DoPhanGiaiTV = form["DoPhanGiaiTV"];
                    tv.LoaiManHinhTV = form["LoaiManHinhTV"];
                    tv.TanSoQuetTV = form["TanSoQuetTV"];
                    tv.DieuKhienTV = form["DieuKhienTV"];
                    tv.CongKetNoiTV = form["CongKetNoiTV"];
                    break;
            }

            if (_sanPhamBLL.KiemTraTenSanPhamTonTai(model.TenSP, model.MaSP))
            {
                TempData["Message"] = "Tên sản phẩm đã tồn tại";
                return RedirectToAction("ChiTietSanPham", new { maSP = model.MaSP });
            }

            if (model.MaSP == 0)
            {
                try
                {
                    var result = _sanPhamBLL.ThemSanPham(model);
                    if (result > 0)
                    {
                        TempData["Message"] = "Thêm sản phẩm thành công";
                    }
                    else
                    {
                        TempData["Message"] = "Thêm sản phẩm thất bại";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"Lỗi thêm sản phẩm: {ex.Message}";
                }
            }
            else
            {
                var result = _sanPhamBLL.CapNhatSanPham(model);
                if (result > 0)
                {
                    TempData["Message"] = "Cập nhật sản phẩm thành công";
                }
                else
                {
                    TempData["Message"] = "Cập nhật sản phẩm thất bại";
                }
            }
            return RedirectToAction("SanPham");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> ThemAnhSanPham(int maSP, List<IFormFile> anhSanPham)
        {
            if (anhSanPham != null && anhSanPham.Count > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", "products");
                Directory.CreateDirectory(uploadsFolder);

                int successCount = 0;
                foreach (var file in anhSanPham)
                {
                    if (file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var urlAnh = $"/uploads/images/products/{fileName}";
                        
                        var anh = new Anh
                        {
                            MaSP = maSP,
                            UrlAnh = urlAnh,
                            MacDinhAnh = 0 
                        };

                        var result = _anhBLL.ThemAnh(anh);
                        if (result > 0)
                        {
                            successCount++;
                        }
                    }
                }

                if (successCount > 0)
                {
                    TempData["Message"] = $"Thêm {successCount} ảnh thành công";
                }
                else
                {
                    TempData["Message"] = "Thêm ảnh thất bại";
                }
            }
            else
            {
                TempData["Message"] = "Vui lòng chọn ảnh để thêm";
            }

            return RedirectToAction("ChiTietSanPham", new { maSP = maSP });
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult XoaSanPham(int maSP)
        {
            if (!_sanPhamBLL.KiemTraCoTheSuaXoaSanPham(maSP))
            {
                TempData["Message"] = "Không thể xóa sản phẩm này vì đang được sử dụng";
                return RedirectToAction("SanPham");
            }

            var result = _sanPhamBLL.XoaSanPham(maSP);
            if (result > 0)
            {
                TempData["Message"] = "Xóa sản phẩm thành công";
            }
            else
            {
                TempData["Message"] = "Xóa sản phẩm thất bại";
            }

            return RedirectToAction("SanPham");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult XoaAnhSanPham(int maAnh, int maSP)
        {
            var result = _anhBLL.XoaAnh(maAnh);
            if (result > 0)
            {
                TempData["Message"] = "Xóa ảnh thành công";
            }
            else
            {
                TempData["Message"] = "Xóa ảnh thất bại";
            }
            return RedirectToAction("ChiTietSanPham", new { maSP = maSP });
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult DatAnhMacDinh(int maAnh, int maSP)
        {
            var result = _anhBLL.DatAnhMacDinh(maAnh, maSP);
            if (result > 0)
            {
                TempData["Message"] = "Đặt ảnh mặc định thành công";
            }
            else
            {
                TempData["Message"] = "Đặt ảnh mặc định thất bại";
            }
            return RedirectToAction("ChiTietSanPham", new { maSP = maSP });
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult ThongTin()
        {
            ViewBag.Breadcrumb = new List<(string Text, string? Url)>
            {
                ("Trang quản trị", Url.Action("Index", "Admin")),
                ("Thông tin tài khoản", ""),
            };

            var taiKhoan = new TaiKhoan
            {
                MaTK = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                TenTK = User.FindFirst(ClaimTypes.Name)!.Value
            };
            
            return View(taiKhoan);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> HandleUpdateTenTK(TaiKhoan model)
        {
            int maTK = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            if (string.IsNullOrWhiteSpace(model.TenTK))
            {
                TempData["Message"] = "Tên đăng nhập không được để trống";
                return RedirectToAction("ThongTin");
            }

            if (_taiKhoanBLL.KiemTraTenDangNhapTonTai(model.TenTK, maTK))
            {
                TempData["Message"] = "Tên đăng nhập đã tồn tại";
                return RedirectToAction("ThongTin");
            }

            try
            {
                var result = _taiKhoanBLL.CapNhatTenDangNhap(maTK, model.TenTK);
                if (result > 0)
                {
                    var currentClaims = User.Claims.ToList();
                    var newClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, maTK.ToString()),
                        new Claim(ClaimTypes.Name, model.TenTK),
                        new Claim(ClaimTypes.Role, User.FindFirst(ClaimTypes.Role)!.Value)
                    };

                    var identity = new ClaimsIdentity(newClaims, "VElectricCookie");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignOutAsync("VElectricCookie");
                    await HttpContext.SignInAsync("VElectricCookie", principal);

                    TempData["Message"] = "Cập nhật tên đăng nhập thành công";
                }
                else
                {
                    TempData["Message"] = "Cập nhật tên đăng nhập thất bại - Không thể lưu vào database";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Lỗi cập nhật tên đăng nhập: {ex.Message}";
            }

            return RedirectToAction("ThongTin");
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult HandleChangePassword(TaiKhoan model)
        {
            int maTK = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            if (string.IsNullOrWhiteSpace(model.MatKhauTK) || model.MatKhauTK.Length < 6)
            {
                TempData["Message"] = "Mật khẩu phải có ít nhất 6 ký tự";
                return RedirectToAction("ThongTin");
            }

            var hasher = new PasswordHasher<object>();
            var hashedPassword = hasher.HashPassword(new object(), model.MatKhauTK);

            var result = _taiKhoanBLL.CapNhatMatKhau(maTK, hashedPassword);
            if (result > 0)
            {
                TempData["Message"] = "Đổi mật khẩu thành công";
            }
            else
            {
                TempData["Message"] = "Đổi mật khẩu thất bại";
            }

            return RedirectToAction("ThongTin");
        }
    }
}
  