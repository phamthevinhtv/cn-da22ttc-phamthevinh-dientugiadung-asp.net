using QL_DienTuGiaDung.Models;
using QL_DienTuGiaDung.DAL;

namespace QL_DienTuGiaDung.BLL
{
    public class QuocGiaBLL
    {
        private readonly QuocGiaDAL _quocGiaDAL;

        public QuocGiaBLL(QuocGiaDAL quocGiaDAL)
        {
            _quocGiaDAL = quocGiaDAL;
        }

        public List<QuocGia> GetAllQuocGia()
        {
            return _quocGiaDAL.GetAllQuocGia();
        }

        public List<QuocGia> GetAllQuocGiaForAdmin()
        {
            return _quocGiaDAL.GetAllQuocGiaForAdmin();
        }

        public QuocGia? GetQuocGiaById(int id)
        {
            return _quocGiaDAL.GetQuocGiaById(id);
        }

        public void CreateQuocGia(QuocGia quocGia)
        {
            _quocGiaDAL.CreateQuocGia(quocGia);
        }

        public void UpdateQuocGia(QuocGia quocGia)
        {
            _quocGiaDAL.UpdateQuocGia(quocGia);
        }

        public bool CanDeleteQuocGia(int id)
        {
            return _quocGiaDAL.CanDeleteQuocGia(id);
        }

        public void DeleteQuocGia(int id)
        {
            _quocGiaDAL.DeleteQuocGia(id);
        }
    }
}