using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.DAL
{
    public class AccountDAL
    {
        private readonly DatabaseHelper _databaseHelper;

        public AccountDAL(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public TaiKhoanKhachHang? GetCustomer(string phone)
        {
            string sql = 
            @"SELECT tk.MaTK, tk.TenTK, tk.QuyenTK, tk.TrangThaiTK, kh.TenKH, kh.GioiTinhKH, kh.SoDienThoaiKH, kh.EmailKH
            FROM TaiKhoan tk JOIN KhachHang kh ON tk.MaKH = kh.MaKH WHERE tk.TenTK = @phone AND tk.QuyenTK = 0;";

            var data = _databaseHelper.ExecuteQuery(sql, parameters: new[] { new SqlParameter("@phone", phone) });

            var customer = DataHelper.MapToList<TaiKhoanKhachHang>(data).FirstOrDefault();
            return customer;
        }

        public void CreateUpdateCustomer(TaiKhoanKhachHang taiKhoanKhachHang)
        {
            taiKhoanKhachHang.XaPhuong ??= new XaPhuong();
            taiKhoanKhachHang.TinhThanhPho ??= new TinhThanhPho();
            taiKhoanKhachHang.DiaChiCuThe ??= new DiaChiCuThe();

            var parameters = new[]
            {
                new SqlParameter("@SoDienThoaiKH", SqlDbType.VarChar, 10)
                {
                    Value = taiKhoanKhachHang.SoDienThoaiKH
                },
                new SqlParameter("@TenKH", SqlDbType.NVarChar, 50)
                {
                    Value = (object?)taiKhoanKhachHang.TenKH ?? DBNull.Value
                },
                new SqlParameter("@GioiTinhKH", SqlDbType.Int)
                {
                    Value = (object?)taiKhoanKhachHang.GioiTinhKH ?? DBNull.Value
                },
                new SqlParameter("@EmailKH", SqlDbType.VarChar, 100)
                {
                    Value = (object?)taiKhoanKhachHang.EmailKH ?? DBNull.Value
                },

                new SqlParameter("@MaTTP", SqlDbType.Char, 2)
                {
                    Value = (object?)taiKhoanKhachHang.TinhThanhPho.MaTTP ?? DBNull.Value
                },
                new SqlParameter("@TenTTP", SqlDbType.NVarChar, 30)
                {
                    Value = (object?)taiKhoanKhachHang.TinhThanhPho.TenTTP ?? DBNull.Value
                },
                new SqlParameter("@MaXP", SqlDbType.Char, 5)
                {
                    Value = (object?)taiKhoanKhachHang.XaPhuong.MaXP ?? DBNull.Value
                },
                new SqlParameter("@TenXP", SqlDbType.NVarChar, 40)
                {
                    Value = (object?)taiKhoanKhachHang.XaPhuong.TenXP ?? DBNull.Value
                },
                new SqlParameter("@TenDCCT", SqlDbType.NVarChar, 255)
                {
                    Value = (object?)taiKhoanKhachHang.DiaChiCuThe.TenDCCT ?? DBNull.Value
                },
                new SqlParameter("@MacDinhDCCT", SqlDbType.NVarChar, 255)
                {
                    Value = (object?)taiKhoanKhachHang.DiaChiCuThe.MacDinhDCCT ?? DBNull.Value
                },
                new SqlParameter("@MaDCCT", SqlDbType.NVarChar, 255)
                {
                    Value = (object?)taiKhoanKhachHang.DiaChiCuThe.MaDCCT ?? DBNull.Value
                }
            };

            _databaseHelper.ExecuteNonQuery("Procedure_TaoSuaKhachHang", CommandType.StoredProcedure, parameters);
        }

        public TaiKhoan? GetTaiKhoanByTenTK(string tenTK)
        {
            var data = _databaseHelper.ExecuteQuery("SELECT * FROM TaiKhoan WHERE TenTK = @tenTK", parameters: new[] { new SqlParameter("@tenTK", tenTK) });

            var taiKhoan = DataHelper.MapToList<TaiKhoan>(data).FirstOrDefault();

            return taiKhoan;
        }

        public void UpdateTenTK(string oldTenTK, string newTenTK)
        {
            var parameters = new[]
            {
                new SqlParameter("@oldTenTK", oldTenTK),
                new SqlParameter("@newTenTK", newTenTK)
            };

            _databaseHelper.ExecuteQuery("UPDATE TaiKhoan SET TenTK = @newTenTK, NgayCapNhatTK = GETDATE() WHERE TenTK = @oldTenTK", parameters: parameters);
        }

        public void ChangePassword(string tenTK, string password)
        {
            var hasher = new PasswordHasher<object>();

            string matKhauHash = hasher.HashPassword(new object(), password);

            var parameters = new[]
            {
                new SqlParameter("@tenTK", tenTK),
                new SqlParameter("@matKhauHash", matKhauHash)
            };

            _databaseHelper.ExecuteQuery("UPDATE TaiKhoan SET MatKhauTK = @matKhauHash, NgayCapNhatTK = GETDATE() WHERE TenTK = @tenTK", parameters: parameters);
        }
    }
}
