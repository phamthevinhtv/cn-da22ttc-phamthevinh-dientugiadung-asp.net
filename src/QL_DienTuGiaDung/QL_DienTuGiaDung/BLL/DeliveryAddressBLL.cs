using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.BLL
{
    public class DeliveryAddressBLL
    {
        private readonly DeliveryAddressDAL _deliveryAddressDAL;

        public DeliveryAddressBLL(DeliveryAddressDAL deliveryAddressDAL)
        {
            _deliveryAddressDAL = deliveryAddressDAL;
        }

        public List<DiaChiDayDu>? GetAddressCustomer(string phone)
        {
            return _deliveryAddressDAL.GetAddressCustomer(phone);       
        }
    }
}