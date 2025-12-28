using Microsoft.AspNetCore.Mvc;
using QL_DienTuGiaDung.BLL;

namespace QL_DienTuGiaDung.ViewComponents
{
    public class PaymentMethodViewComponent : ViewComponent
    {
        private readonly PaymentMethodBLL _paymentMethodBLL;

        public PaymentMethodViewComponent(PaymentMethodBLL paymentMethodBLL)
        {
            _paymentMethodBLL = paymentMethodBLL;
        }

        public IViewComponentResult Invoke()
        {
            var paymentMethods = _paymentMethodBLL.GetPaymentMethods();
            
            return View(paymentMethods);
        }
    }
}
