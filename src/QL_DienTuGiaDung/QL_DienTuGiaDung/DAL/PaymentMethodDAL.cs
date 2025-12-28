using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.DAL
{
    public class PaymentMethodDAL {
        private readonly DatabaseHelper _databaseHelper;

        public PaymentMethodDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<PhuongThucThanhToan> GetPaymentMethods()
        {
            string sql = "SELECT MaPTTT, TenPTTT FROM PhuongThucThanhToan ORDER BY MaPTTT ASC";

            var datas = _databaseHelper.ExecuteQuery(sql);

            var paymentMethods = DataHelper.MapToList<PhuongThucThanhToan>(datas);

            return paymentMethods;
        }
    }
}