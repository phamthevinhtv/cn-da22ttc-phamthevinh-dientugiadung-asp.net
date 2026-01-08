using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class LoaiSanPhamBLL
    {
        private readonly LoaiSanPhamDAL _loaiSanPhamDAL;

        public LoaiSanPhamBLL(LoaiSanPhamDAL loaiSanPhamDAL)
        {
            _loaiSanPhamDAL = loaiSanPhamDAL;
        }

        private static void MapLoaiSanPham(LoaiSanPham loaiSanPham, DataRow row)
        {
            loaiSanPham.MaLSP = row["MaLSP"] != DBNull.Value ? Convert.ToInt32(row["MaLSP"]) : 0;
            loaiSanPham.TenLSP = row["TenLSP"] != DBNull.Value ? row["TenLSP"].ToString()! : string.Empty;
            loaiSanPham.ThueGTGTLSP = row["ThueGTGTLSP"] != DBNull.Value ? Convert.ToDecimal(row["ThueGTGTLSP"]) : 0m;
            loaiSanPham.TrangThaiLSP = row["TrangThaiLSP"] != DBNull.Value ? Convert.ToInt32(row["TrangThaiLSP"]) : 0;
        }

        public List<LoaiSanPham> LayDanhSachLoaiSanPham(int quyen)
        {
            var dataTable = _loaiSanPhamDAL.LayDanhSachLoaiSanPham(quyen);
            var result = new List<LoaiSanPham>();

            foreach (DataRow row in dataTable.Rows)
            {
                var loaiSanPham = new LoaiSanPham();
                MapLoaiSanPham(loaiSanPham, row);
                result.Add(loaiSanPham);
            }

            return result;
        }

        public LoaiSanPham? LayLoaiSanPham(int maLSP)
        {
            var dataTable = _loaiSanPhamDAL.LayLoaiSanPham(maLSP);
            
            if (dataTable.Rows.Count == 0)
                return null;

            var loaiSanPham = new LoaiSanPham();
            MapLoaiSanPham(loaiSanPham, dataTable.Rows[0]);
            return loaiSanPham;
        }

        public int ThemLoaiSanPham(string tenLSP, decimal thueGTGTLSP)
        {
            return _loaiSanPhamDAL.ThemLoaiSanPham(tenLSP, thueGTGTLSP);
        }

        public int CapNhatLoaiSanPham(int maLSP, string tenLSP, decimal thueGTGTLSP, int trangThaiLSP)
        {
            return _loaiSanPhamDAL.CapNhatLoaiSanPham(maLSP, tenLSP, thueGTGTLSP, trangThaiLSP);
        }

        public bool KiemTraTenLoaiSanPhamTonTai(string tenLSP, int maLSP = 0)
        {
            return _loaiSanPhamDAL.KiemTraTenLoaiSanPhamTonTai(tenLSP, maLSP);
        }

        public bool KiemTraCoTheSuaXoa(int maLSP)
        {
            return _loaiSanPhamDAL.KiemTraCoTheSuaXoa(maLSP);
        }

        public int XoaLoaiSanPham(int maLSP)
        {
            return _loaiSanPhamDAL.XoaLoaiSanPham(maLSP);
        }
    }
}
