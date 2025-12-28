using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.BLL
{
    public class AccountBLL
    {
        private readonly HttpClient _httpClient;
        private readonly AccountDAL _accountDAL;
        private readonly DeliveryAddressDAL _deliveryAddressDAL;

        public AccountBLL(HttpClient httpClient, AccountDAL accountDAL, DeliveryAddressDAL deliveryAddressDAL)
        {
            _httpClient = httpClient;
            _accountDAL = accountDAL;
            _deliveryAddressDAL = deliveryAddressDAL;
        }

        public string GenerateOTP()
        {
            var random = new Random();
            string otp = "";
            for (int i = 0; i < 6; i++)
            {
                otp += random.Next(0, 10).ToString();
            }

            return otp;
        }

        public async Task<bool> SendOTPAsync(string phone, string otp)
        {
            var payload = new { sdt = phone, otp = otp };
            var response = await _httpClient.PostAsJsonAsync("https://api-demo-otp.vercel.app/api/otp", payload);
            
            return response.IsSuccessStatusCode;
        }

        public TaiKhoanKhachHang? GetCustomer(string phone)
        {
            return _accountDAL.GetCustomer(phone);
        }

        public TaiKhoan? GetTaiKhoanByTenTK(string tenTK)
        {
            return _accountDAL.GetTaiKhoanByTenTK(tenTK);
        }

        public void UpdateTenTK(string oldTenTK, string newTenTK)
        {
            _accountDAL.UpdateTenTK(oldTenTK, newTenTK);
        }

        public void ChangePassword(string tenTK, string password)
        {
            _accountDAL.ChangePassword(tenTK, password);
        }


        public TaiKhoanKhachHang? GetCustomerPersonal(string phone)
        {
            var customer = _accountDAL.GetCustomer(phone);

            if (customer == null) return null;

            customer.ListDiaChiDayDu = _deliveryAddressDAL.GetAddressCustomer(phone);

            return customer;
        }

        public void CreateCustomer(TaiKhoanKhachHang taiKhoanKhachHang)
        {
            _accountDAL.CreateUpdateCustomer(taiKhoanKhachHang);
        }

        public void UpdateCustomer(TaiKhoanKhachHang taiKhoanKhachHang)
        {
            _accountDAL.CreateUpdateCustomer(taiKhoanKhachHang);
        }

        public void AddAddress(TaiKhoanKhachHang taiKhoanKhachHang)
        {
            _accountDAL.CreateUpdateCustomer(taiKhoanKhachHang);
        }

        public void DeleteAddress(int MaDCCT)
        {
           _deliveryAddressDAL.DeleteAddress(MaDCCT);
        }
    }
}