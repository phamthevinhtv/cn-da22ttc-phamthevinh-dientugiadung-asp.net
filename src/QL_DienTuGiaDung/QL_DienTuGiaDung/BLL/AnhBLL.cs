using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class AnhBLL
    {
        private readonly AnhDAL _anhDAL;

        public AnhBLL(AnhDAL anhDAL)
        {
            _anhDAL = anhDAL;
        }

        public List<Anh> LayDanhSachAnhSanPham(int quyen, int masp)
        {
            List<Anh> list = new();

            var table = _anhDAL.LayDanhSachAnhSanPham(quyen, masp);

            foreach (DataRow row in table.Rows)
            {
                Anh anh = new Anh
                {
                    MaAnh = row["MaAnh"] != DBNull.Value ? Convert.ToInt32(row["MaAnh"]) : 0,
                    MaSP = row["MaSP"] != DBNull.Value ? Convert.ToInt32(row["MaSP"]) : 0,
                    UrlAnh = row["UrlAnh"] != DBNull.Value ? row["UrlAnh"].ToString()! : string.Empty,
                    MacDinhAnh = row["MacDinhAnh"] != DBNull.Value ? Convert.ToInt32(row["MacDinhAnh"]) : 0,
                    TrangThaiAnh = row["TrangThaiAnh"] != DBNull.Value ? Convert.ToInt32(row["TrangThaiAnh"]) : 0
                };

                list.Add(anh);
            }

            return list;
        }

        public int XoaAnh(int maAnh)
        {
            return _anhDAL.XoaAnh(maAnh);
        }

        public int ThemAnh(Anh anh)
        {
            return _anhDAL.ThemAnh(anh);
        }

        public int DatAnhMacDinh(int maAnh, int maSP)
        {
            return _anhDAL.DatAnhMacDinh(maAnh, maSP);
        }
    }
}
