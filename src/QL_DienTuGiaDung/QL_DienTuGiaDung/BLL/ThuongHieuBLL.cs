using QL_DienTuGiaDung.Models;
using QL_DienTuGiaDung.DAL;

namespace QL_DienTuGiaDung.BLL
{
    public class ThuongHieuBLL
    {
        private readonly ThuongHieuDAL _thuongHieuDAL;

        public ThuongHieuBLL(ThuongHieuDAL thuongHieuDAL)
        {
            _thuongHieuDAL = thuongHieuDAL;
        }

        public List<ThuongHieu> GetAllThuongHieu()
        {
            return _thuongHieuDAL.GetAllThuongHieu();
        }

        public List<ThuongHieu> GetAllThuongHieuForAdmin()
        {
            return _thuongHieuDAL.GetAllThuongHieuForAdmin();
        }

        public ThuongHieu? GetThuongHieuById(int id)
        {
            return _thuongHieuDAL.GetThuongHieuById(id);
        }

        public void CreateThuongHieu(ThuongHieu thuongHieu)
        {
            _thuongHieuDAL.CreateThuongHieu(thuongHieu);
        }

        public void UpdateThuongHieu(ThuongHieu thuongHieu)
        {
            _thuongHieuDAL.UpdateThuongHieu(thuongHieu);
        }

        public bool CanDeleteThuongHieu(int id)
        {
            return _thuongHieuDAL.CanDeleteThuongHieu(id);
        }

        public void DeleteThuongHieu(int id)
        {
            _thuongHieuDAL.DeleteThuongHieu(id);
        }
    }
}