using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class DiaChiBLL
    {
        private readonly DiaChiDAL _diachiDAL;

        public DiaChiBLL(DiaChiDAL diachiDAL)
        {
            _diachiDAL = diachiDAL;
        }

        public List<DiaChiNhanHang> LayDanhSachDiaChi(int maKH)
        {
            List<DiaChiNhanHang> list = new();

            var table = _diachiDAL.LayDanhSachDiaChi(maKH);

            foreach (DataRow row in table.Rows)
            {
                DiaChiNhanHang dc = new DiaChiNhanHang
                {
                    DiaChiCuThe = new DiaChiCuThe
                    {
                        MaDCCT = row["MaDCCT"] != DBNull.Value ? Convert.ToInt32(row["MaDCCT"]) : 0,
                        TenDCCT = row["TenDCCT"] != DBNull.Value ? row["TenDCCT"].ToString()! : string.Empty,
                        MacDinhDCCT = row["MacDinhDCCT"] != DBNull.Value ? Convert.ToInt32(row["MacDinhDCCT"]) : 0,
                    },

                    DuocSuaXoa = row["DuocSuaXoa"] != DBNull.Value ? Convert.ToInt32(row["DuocSuaXoa"]) : 0,

                    XaPhuong = new XaPhuong 
                    {
                        MaXP = row["MaXP"] != DBNull.Value ? row["MaXP"].ToString()! : string.Empty,
                        TenXP = row["TenXP"] != DBNull.Value ? row["TenXP"].ToString()! : string.Empty
                    },

                    TinhThanhPho = new TinhThanhPho
                    {
                        MaTTP = row["MaTTP"] != DBNull.Value ? row["MaTTP"].ToString()! : string.Empty,
                        TenTTP = row["TenTTP"] != DBNull.Value ? row["TenTTP"].ToString()! : string.Empty
                    }
                };

                list.Add(dc);
            }

            return list;
        }

        public DiaChiNhanHang? LayDiaChi(int maDC, int maKH)
        {
            var table = _diachiDAL.LayDiaChi(maDC, maKH);

            if (table.Rows.Count == 0)
                return null;

            DataRow row = table.Rows[0];

            var maXP = row["MaXP"] != DBNull.Value ? row["MaXP"].ToString()! : string.Empty;
            var tenXP = row["TenXP"] != DBNull.Value ? row["TenXP"].ToString()! : string.Empty;

            var maTTP = row["MaTTP"] != DBNull.Value ? row["MaTTP"].ToString()! : string.Empty;
            var tenTTP = row["TenTTP"] != DBNull.Value ? row["TenTTP"].ToString()! : string.Empty;

            DiaChiNhanHang dcnh = new DiaChiNhanHang
            {
                DiaChiCuThe = new DiaChiCuThe
                {
                    MaDCCT = row["MaDCCT"] != DBNull.Value ? Convert.ToInt32(row["MaDCCT"]) : 0,
                    TenDCCT = row["TenDCCT"] != DBNull.Value ? row["TenDCCT"].ToString()! : string.Empty,
                    MacDinhDCCT = row["MacDinhDCCT"] != DBNull.Value ? Convert.ToInt32(row["MacDinhDCCT"]) : 0,
                },
                DuocSuaXoa = row["DuocSuaXoa"] != DBNull.Value ? Convert.ToInt32(row["DuocSuaXoa"]) : 0,
                XaPhuong = new XaPhuong 
                {
                    MaXP = maXP,
                    TenXP = tenXP
                },
                TinhThanhPho = new TinhThanhPho
                {
                    MaTTP = maTTP,
                    TenTTP = tenTTP
                },
                XaPhuongChon = $"{maXP}|{tenXP}",
                TinhTPChon = $"{maTTP}|{tenTTP}"
            };

            return dcnh;
        }

        public int ThemDiaChiNhanHang(DiaChiNhanHang diaChiNhanHangModel)
        {
            return _diachiDAL.ThemDiaChiNhanHang(diaChiNhanHangModel);
        }

        public int SuaDiaChiNhanHang(DiaChiNhanHang diaChiNhanHangModel)
        {
            return _diachiDAL.SuaDiaChiNhanHang(diaChiNhanHangModel);
        }

        public int XoaDiaChi(int maDC)
        {
            return _diachiDAL.XoaDiaChi(maDC);
        }

        public int DatDiaChiMacDinh(int MaKH, int MaDCCT)
        {
            return _diachiDAL.DatDiaChiMacDinh(MaKH, MaDCCT);
        }
    }
}
