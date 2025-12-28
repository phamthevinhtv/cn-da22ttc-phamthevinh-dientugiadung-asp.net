using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.BLL
{
    public class OrderBLL
    {
        private readonly OrderDAL _orderDAL;

        public OrderBLL(OrderDAL orderDAL)
        {
            _orderDAL = orderDAL;
        }

        public void DatHang(DonHang donHang)
        {
            _orderDAL.DatHang(donHang);
        }

        public List<DonHang> GetOrdersByPhone(string phone)
        {
            var orders = _orderDAL.GetOrdersByPhone(phone);

            foreach (var order in orders)
            {
                order.ListSanPhamDatHang = _orderDAL.GetProductsInOrder(order.MaDH);
            }

            return orders;
        }

        public List<DonHang> GetOrders()
        {
            var orders = _orderDAL.GetOrders();

            foreach (var order in orders)
            {
                order.ListSanPhamDatHang = _orderDAL.GetProductsInOrder(order.MaDH);
            }

            return orders;
        }

        public void CancelOrder(int orderId)
        {
            _orderDAL.CancelOrder(orderId);
        }

        public List<TrangThaiDonHang> GetAllTrangThaiDonHang()
        {
            return _orderDAL.GetAllTrangThaiDonHang();
        }

        public void UpdateTrangThaiDonHang(int MaDH, int MaTTDH)
        {
            _orderDAL.UpdateTrangThaiDonHang(MaDH, MaTTDH);
        }

        public List<TrangThaiThanhToan> GetAllTrangThaiThanhToan()
        {
            return _orderDAL.GetAllTrangThaiThanhToan();
        }

        public void UpdateTrangThaiThanhToan(int MaDH, int MaTTDH, string MaGiaoDichTT)
        {
            _orderDAL.UpdateTrangThaiThanhToan(MaDH, MaTTDH, MaGiaoDichTT);
        }
    }
}