using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.ViewComponents
{
    public class DeliveryAddress : ViewComponent
    {
        private readonly DeliveryAddressBLL _deliveryAddressBLL;

        public DeliveryAddress(DeliveryAddressBLL deliveryAddressBLL)
        {
            _deliveryAddressBLL = deliveryAddressBLL;
        }
        public IViewComponentResult Invoke()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var user = (ClaimsPrincipal)User;
                var phone = user.FindFirstValue(ClaimTypes.MobilePhone) ?? "";

                var diaChiDayDu = _deliveryAddressBLL.GetAddressCustomer(phone);
                
                return View(diaChiDayDu);
            }
            return View();
        }
    }

}