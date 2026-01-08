using QL_DienTuGiaDung.Models;
using QL_DienTuGiaDung.DAL;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class ThuongHieuBLL
    {
        private readonly ThuongHieuDAL _thuongHieuDAL;

        public ThuongHieuBLL(ThuongHieuDAL thuongHieuDAL)
        {
            _thuongHieuDAL = thuongHieuDAL;
        }

        private static void MapThuongHieu(ThuongHieu thuongHieu, DataRow row)
        {
            thuongHieu.MaTH = row["MaTH"] != DBNull.Value ? Convert.ToInt32(row["MaTH"]) : 0;
            thuongHieu.MaQG = row["MaQG"] != DBNull.Value ? Convert.ToInt32(row["MaQG"]) : 0;
            thuongHieu.TenTH = row["TenTH"] != DBNull.Value ? row["TenTH"].ToString()! : string.Empty;
            thuongHieu.TenQG = row["TenQG"] != DBNull.Value ? row["TenQG"].ToString()! : string.Empty;
        }

        public List<ThuongHieu> LayDanhSachThuongHieu(int quyen)
        {
            var dataTable = _thuongHieuDAL.LayDanhSachThuongHieu(quyen);
            var result = new List<ThuongHieu>();

            foreach (DataRow row in dataTable.Rows)
            {
                var thuongHieu = new ThuongHieu();
                MapThuongHieu(thuongHieu, row);
                result.Add(thuongHieu);
            }

            return result;
        }

        public ThuongHieu? LayThuongHieu(int maTH)
        {
            var dataTable = _thuongHieuDAL.LayThuongHieu(maTH);
            
            if (dataTable.Rows.Count == 0)
                return null;

            var thuongHieu = new ThuongHieu();
            MapThuongHieu(thuongHieu, dataTable.Rows[0]);
            return thuongHieu;
        }

        public int ThemThuongHieu(int maQG, string tenTH)
        {
            return _thuongHieuDAL.ThemThuongHieu(maQG, tenTH);
        }

        public int CapNhatThuongHieu(int maTH, int maQG, string tenTH)
        {
            return _thuongHieuDAL.CapNhatThuongHieu(maTH, maQG, tenTH);
        }

        public bool KiemTraCoTheSuaXoa(int maTH)
        {
            return _thuongHieuDAL.KiemTraCoTheSuaXoa(maTH);
        }

        public int XoaThuongHieu(int maTH)
        {
            return _thuongHieuDAL.XoaThuongHieu(maTH);
        }
    }
}