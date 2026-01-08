using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class DanhGiaBLL
    {
        private readonly DanhGiaDAL _danhGiaDAL;

        public DanhGiaBLL(DanhGiaDAL danhGiaDAL)
        {
            _danhGiaDAL = danhGiaDAL;
        }

        public List<DanhGia> LayDanhSachDanhGia(int masp)
        {
            List<DanhGia> list = new();

            var table = _danhGiaDAL.LayDanhSachDanhGia(masp);

            foreach (DataRow row in table.Rows)
            {
                DanhGia danhGia = new DanhGia
                {
                    MaSP = row["MaSP"] != DBNull.Value ? Convert.ToInt32(row["MaSP"]) : 0,
                    MaKH = row["MaKH"] != DBNull.Value ? Convert.ToInt32(row["MaKH"]) : 0,
                    DiemDG = row["DiemDG"] != DBNull.Value ? Convert.ToInt32(row["DiemDG"]) : 0,
                    NhanXetDG = row["NhanXetDG"] != DBNull.Value ? row["NhanXetDG"].ToString() : null,
                    TenKH = row["TenKH"] != DBNull.Value ? row["TenKH"].ToString()! : string.Empty,
                    NgayTaoDG = row["NgayTaoDG"] != DBNull.Value ? Convert.ToDateTime(row["NgayTaoDG"]) : DateTime.MinValue,
                    NgayCapNhatDG = row["NgayCapNhatDG"] != DBNull.Value ? Convert.ToDateTime(row["NgayCapNhatDG"]) : DateTime.MinValue
                };

                list.Add(danhGia);
            }

            return list;
        }

        public DanhGia? LayDanhGia(int masp, int maKH)
        {
            var table = _danhGiaDAL.LayDanhGia(masp, maKH);

            if (table.Rows.Count == 0)
                return null;

            DataRow row = table.Rows[0];

            DanhGia danhGia = new DanhGia
            {
                MaSP = row["MaSP"] != DBNull.Value ? Convert.ToInt32(row["MaSP"]) : 0,
                MaKH = row["MaKH"] != DBNull.Value ? Convert.ToInt32(row["MaKH"]) : 0,
                DiemDG = row["DiemDG"] != DBNull.Value ? Convert.ToInt32(row["DiemDG"]) : 0,
                NhanXetDG = row["NhanXetDG"] != DBNull.Value ? row["NhanXetDG"].ToString() : null,
                TenKH = row["TenKH"] != DBNull.Value ? row["TenKH"].ToString()! : string.Empty,
                NgayTaoDG = row["NgayTaoDG"] != DBNull.Value ? Convert.ToDateTime(row["NgayTaoDG"]) : DateTime.MinValue,
                NgayCapNhatDG = row["NgayCapNhatDG"] != DBNull.Value ? Convert.ToDateTime(row["NgayCapNhatDG"]) : DateTime.MinValue
            };

            return danhGia;
        }

        public int TaoDanhGia(DanhGia danhGia)
        {
            return _danhGiaDAL.TaoDanhGia(danhGia);
        }

        public int SuaDanhGia(DanhGia danhGia)
        {
            return _danhGiaDAL.SuaDanhGia(danhGia);
        }
    }
}
