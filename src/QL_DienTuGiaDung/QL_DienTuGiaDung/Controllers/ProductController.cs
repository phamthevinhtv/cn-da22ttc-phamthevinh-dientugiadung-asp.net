using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductBLL _productBLL;

        public ProductController(ProductBLL productBLL)
        {
            _productBLL = productBLL;
        }

        public IActionResult Index(int productTypeId)
        {
            var products = _productBLL.GetAllProductsForCustomer(productTypeId);

            var productTypeName = products.FirstOrDefault()?.TenLSP ?? "Sản phẩm";

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Tất cả sản phẩm", Url.Action("Index", "Product"))
            };

            if (productTypeId != 0)
            {
                breadcrumb.Add(
                    (productTypeName, Url.Action("Index", "Product", new { productTypeId }))
                );
            }

            ViewData["Breadcrumb"] = breadcrumb;

            return View(products);
        }

        public IActionResult Detail(int id)
        {
            var product = _productBLL.GetProductDetailForCustomer(id);

            if(product == null || product.SoLuongSP <= 0)
            {
                return RedirectToAction("Index");
            }

            var productTypeName = product?.TenLSP;
            var productTypeId = product?.MaLSP;
            var productName = product?.TenSP;
            var productId = product?.MaSP;

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Tất cả sản phẩm", Url.Action("Index", "Product"))
            };

            if (productTypeId != 0 && !string.IsNullOrEmpty(productTypeName))
            {
                breadcrumb.Add(
                    (productTypeName, Url.Action("Index", "Product", new { productTypeId }))
                );
            }

            if (productId != 0 && !string.IsNullOrEmpty(productName))
            {
                breadcrumb.Add(
                    (productName, Url.Action("Index", "Product", new { productId }))
                );
            }

            ViewData["Breadcrumb"] = breadcrumb;

            return View(product);
        }

        [Route("Search")]
        public IActionResult HandleSearch(string tenSP)
        {
            var products = _productBLL.GetAllProductsByNameForCustomer(tenSP);

            var breadcrumb = new List<(string Text, string? Url)>
            {
                ("Tất cả sản phẩm", Url.Action("Index", "Product")),
                ("Kết quả tìm kiếm", Url.Action("Index", "Product"))
            };

            ViewData["Breadcrumb"] = breadcrumb;

            return View("Index", products);
        }

        [HttpPost] 
        public IActionResult HandleRating(int MaSP, int rating, string review)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                 var phone = User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";
                var danhGia = new DanhGia();
                
                danhGia.MaSP = MaSP;
                danhGia.DiemDG = rating;
                danhGia.NhanXetDG = review;
                danhGia.SoDienThoaiKH = phone;

                _productBLL.CreateUpdateRating(danhGia);

                TempData["Message"] = "Gửi đánh giá thành công";
            } else
            {
                HttpContext.Session.SetString("RETURN_URL", $"Product|Detail|{MaSP}");

                return RedirectToAction("Index", "Account");
            }

            return RedirectToAction("Detail", "Product", new { id = MaSP });
        }
    }
}
