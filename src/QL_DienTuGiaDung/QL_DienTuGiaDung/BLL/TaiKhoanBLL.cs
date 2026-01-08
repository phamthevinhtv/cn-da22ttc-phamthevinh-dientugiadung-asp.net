using System.Data;
using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.BLL
{
    public class TaiKhoanBLL
    {
        private readonly HttpClient _httpClient;
        private readonly TaiKhoanDAL _taiKhoanDAL;

        public TaiKhoanBLL(HttpClient httpClient, TaiKhoanDAL taiKhoanDAL)
        {
            _httpClient = httpClient;
            _taiKhoanDAL = taiKhoanDAL;
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

        public TaiKhoanKhachHang? LayTaiKhoanBangSoDienThoai(string sodienthoai)
        {
            var table = _taiKhoanDAL.LayTaiKhoanBangTenTK(sodienthoai, 0);

            if (table.Rows.Count == 0)
                return null;

            DataRow row = table.Rows[0];

            TaiKhoanKhachHang tkkh = new TaiKhoanKhachHang
            {
                KhachHang = new KhachHang
                {
                    MaKH = row["MaKH"] != DBNull.Value ? Convert.ToInt32(row["MaKH"]) : 0,
                    TenKH = row["TenKH"] != DBNull.Value ? row["TenKH"].ToString()! : string.Empty,
                    GioiTinhKH = row["GioiTinhKH"] != DBNull.Value ? Convert.ToInt32(row["GioiTinhKH"]) : 0,
                    SoDienThoaiKH = row["SoDienThoaiKH"] != DBNull.Value ? row["SoDienThoaiKH"].ToString()! : string.Empty,
                    EmailKH = row["EmailKH"] != DBNull.Value ? row["EmailKH"].ToString()! : string.Empty
                },

                TaiKhoan = new TaiKhoan
                {
                    MaTK = row["MaTK"] != DBNull.Value ? Convert.ToInt32(row["MaTK"]) : 0,
                    TenTK = row["TenTK"] != DBNull.Value ? row["TenTK"].ToString()! : string.Empty,
                    QuyenTK = row["QuyenTK"] != DBNull.Value ? Convert.ToInt32(row["QuyenTK"]) : 0,
                    TrangThaiTK = row["TrangThaiTK"] != DBNull.Value ? Convert.ToInt32(row["TrangThaiTK"]) : 0,
                }
            };

            return tkkh;
        }

        public TaiKhoan? LayTaiKhoanBangTenDangNhap(string tendn)
        {
            var table = _taiKhoanDAL.LayTaiKhoanBangTenTK(tendn, 1);

            if (table.Rows.Count == 0)
                return null;

            DataRow row = table.Rows[0];

            TaiKhoan tk = new TaiKhoan
            {
                TenTK = row["TenTK"] != DBNull.Value ? row["TenTK"].ToString()! : string.Empty,
                MatKhauTK =  row["MatKhauTK"] != DBNull.Value ? row["MatKhauTK"].ToString()! : string.Empty,
                QuyenTK = row["QuyenTK"] != DBNull.Value ? Convert.ToInt32(row["QuyenTK"]) : 0,
            };

            return tk;
        }

        public int TaoTaiKhoanKhachHang(TaiKhoanKhachHang taiKhoanKhachHangModel)
        {
            return _taiKhoanDAL.TaoTaiKhoanKhachHang(taiKhoanKhachHangModel);
        }

        public int SuaThongTinKhachHang(TaiKhoanKhachHang taiKhoanKhachHangModel)
        {
            return _taiKhoanDAL.SuaThongTinKhachHang(taiKhoanKhachHangModel);
        }

        public TaiKhoan? LayTaiKhoanBangMa(int maTK)
        {
            var table = _taiKhoanDAL.LayTaiKhoanBangMa(maTK);

            if (table.Rows.Count == 0)
                return null;

            DataRow row = table.Rows[0];

            TaiKhoan tk = new TaiKhoan
            {
                MaTK = row["MaTK"] != DBNull.Value ? Convert.ToInt32(row["MaTK"]) : 0,
                TenTK = row["TenTK"] != DBNull.Value ? row["TenTK"].ToString()! : string.Empty,
                QuyenTK = row["QuyenTK"] != DBNull.Value ? Convert.ToInt32(row["QuyenTK"]) : 0,
                TrangThaiTK = row["TrangThaiTK"] != DBNull.Value ? Convert.ToInt32(row["TrangThaiTK"]) : 0,
            };

            return tk;
        }

        public bool KiemTraTenDangNhapTonTai(string tenTK, int maTK = 0)
        {
            return _taiKhoanDAL.KiemTraTenDangNhapTonTai(tenTK, maTK);
        }

        public int CapNhatTenDangNhap(int maTK, string tenTK)
        {
            return _taiKhoanDAL.CapNhatTenDangNhap(maTK, tenTK);
        }

        public int CapNhatMatKhau(int maTK, string matKhau)
        {
            return _taiKhoanDAL.CapNhatMatKhau(maTK, matKhau);
        }
    }
}