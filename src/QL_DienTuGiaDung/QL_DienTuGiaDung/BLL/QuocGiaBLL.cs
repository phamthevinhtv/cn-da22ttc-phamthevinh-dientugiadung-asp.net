using QL_DienTuGiaDung.Models;
using QL_DienTuGiaDung.DAL;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class QuocGiaBLL
    {
        private readonly QuocGiaDAL _quocGiaDAL;

        public QuocGiaBLL(QuocGiaDAL quocGiaDAL)
        {
            _quocGiaDAL = quocGiaDAL;
        }

        private static void MapQuocGia(QuocGia quocGia, DataRow row)
        {
            quocGia.MaQG = row["MaQG"] != DBNull.Value ? Convert.ToInt32(row["MaQG"]) : 0;
            quocGia.TenQG = row["TenQG"] != DBNull.Value ? row["TenQG"].ToString()! : string.Empty;
        }

        public List<QuocGia> LayDanhSachQuocGia(int quyen)
        {
            var dataTable = _quocGiaDAL.LayDanhSachQuocGia(quyen);
            var result = new List<QuocGia>();

            foreach (DataRow row in dataTable.Rows)
            {
                var quocGia = new QuocGia();
                MapQuocGia(quocGia, row);
                result.Add(quocGia);
            }

            return result;
        }

        public QuocGia? LayQuocGia(int maQG)
        {
            var dataTable = _quocGiaDAL.LayQuocGia(maQG);
            
            if (dataTable.Rows.Count == 0)
                return null;

            var quocGia = new QuocGia();
            MapQuocGia(quocGia, dataTable.Rows[0]);
            return quocGia;
        }

        public int ThemQuocGia(string tenQG)
        {
            return _quocGiaDAL.ThemQuocGia(tenQG);
        }

        public int CapNhatQuocGia(int maQG, string tenQG)
        {
            return _quocGiaDAL.CapNhatQuocGia(maQG, tenQG);
        }

        public bool KiemTraCoTheSuaXoa(int maQG)
        {
            return _quocGiaDAL.KiemTraCoTheSuaXoa(maQG);
        }

        public int XoaQuocGia(int maQG)
        {
            return _quocGiaDAL.XoaQuocGia(maQG);
        }
    }
}