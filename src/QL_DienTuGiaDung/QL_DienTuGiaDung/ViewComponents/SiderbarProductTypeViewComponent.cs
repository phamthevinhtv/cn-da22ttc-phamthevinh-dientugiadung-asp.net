using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;

namespace QL_DienTuGiaDung.ViewComponents
{
    public class SiderbarProductType : ViewComponent
    {
        private readonly ProductBLL _productBLL;

        public SiderbarProductType(ProductBLL productBLL)
        {
            _productBLL = productBLL;
        }

        public IViewComponentResult Invoke()
        {
            var productTypes = _productBLL.GetAllProductTypesForCustomer();
            
            return View(productTypes);
        }
    }
}
