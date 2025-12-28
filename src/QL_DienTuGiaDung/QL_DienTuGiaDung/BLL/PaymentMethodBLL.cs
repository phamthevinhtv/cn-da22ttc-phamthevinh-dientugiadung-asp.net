using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.BLL
{
    public class PaymentMethodBLL
    {
        private readonly PaymentMethodDAL _paymentMethodDAL;

        public PaymentMethodBLL(PaymentMethodDAL paymentMethodDAL)
        {
            _paymentMethodDAL = paymentMethodDAL;
        }

        public List<PhuongThucThanhToan> GetPaymentMethods()
        {
            return _paymentMethodDAL.GetPaymentMethods();
        }
    }
}