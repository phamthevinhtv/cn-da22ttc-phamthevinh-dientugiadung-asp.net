using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.ViewComponents
{
    public class CartNumberItem : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.GetObject<List<SanPhamTrongGio>>("CART");
            var count = cart?.Sum(x => x.SoLuongSP) ?? 0;

            return View(count);
        }
    }

}