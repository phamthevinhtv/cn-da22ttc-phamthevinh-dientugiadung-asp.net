using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;
using System.Data;

namespace QL_DienTuGiaDung.BLL
{
    public class SanPhamBLL
    {
        private readonly SanPhamDAL _sanPhamDAL;
        private readonly AnhBLL _anhBLL;
        private readonly DanhGiaBLL _danhGiaBLL;

        public SanPhamBLL(SanPhamDAL sanPhamDAL, AnhBLL anhBLL, DanhGiaBLL danhGiaBLL)
        {
            _sanPhamDAL = sanPhamDAL;
            _anhBLL = anhBLL;
            _danhGiaBLL = danhGiaBLL;
        }

        private static void MapSanPham(SanPham sanPham, DataRow row)
        {
            sanPham.MaSP = row["MaSP"] != DBNull.Value ? Convert.ToInt32(row["MaSP"]) : 0;
            sanPham.MaQG = row["MaQG"] != DBNull.Value ? Convert.ToInt32(row["MaQG"]) : 0;
            sanPham.MaTH = row["MaTH"] != DBNull.Value ? Convert.ToInt32(row["MaTH"]) : 0;
            sanPham.MaLSP = row["MaLSP"] != DBNull.Value ? Convert.ToInt32(row["MaLSP"]) : 0;
            sanPham.TenSP = row["TenSP"] != DBNull.Value ? row["TenSP"].ToString()! : string.Empty;
            sanPham.SoLuongSP = row["SoLuongSP"] != DBNull.Value ? Convert.ToInt32(row["SoLuongSP"]) : 0;
            sanPham.GiaNhapSP = row["GiaNhapSP"] != DBNull.Value ? Convert.ToDecimal(row["GiaNhapSP"]) : 0;
            sanPham.GiaGocSP = row["GiaGocSP"] != DBNull.Value ? Convert.ToDecimal(row["GiaGocSP"]) : 0;
            sanPham.PhanLoaiSP = row["PhanLoaiSP"] != DBNull.Value ? row["PhanLoaiSP"].ToString()! : string.Empty;
            sanPham.NamSanXuatSP = row["NamSanXuatSP"] != DBNull.Value ? Convert.ToInt32(row["NamSanXuatSP"]) : 0;
            sanPham.BaoHanhSP = row["BaoHanhSP"] != DBNull.Value ? row["BaoHanhSP"].ToString()! : string.Empty;
            sanPham.KichThuocSP = row["KichThuocSP"] != DBNull.Value ? row["KichThuocSP"].ToString()! : string.Empty;
            sanPham.KhoiLuongSP = row["KhoiLuongSP"] != DBNull.Value ? row["KhoiLuongSP"].ToString()! : string.Empty;
            sanPham.CongSuatTieuThuSP = row["CongSuatTieuThuSP"] != DBNull.Value ? row["CongSuatTieuThuSP"].ToString()! : string.Empty;
            sanPham.ChatLieuSP = row["ChatLieuSP"] != DBNull.Value ? row["ChatLieuSP"].ToString()! : string.Empty;
            sanPham.TienIchSP = row["TienIchSP"] != DBNull.Value ? row["TienIchSP"].ToString()! : string.Empty;
            sanPham.CongNgheSP = row["CongNgheSP"] != DBNull.Value ? row["CongNgheSP"].ToString()! : string.Empty;
            sanPham.MucGiamGiaSP = row["MucGiamGiaSP"] != DBNull.Value ? Convert.ToDecimal(row["MucGiamGiaSP"]) : 0;
            sanPham.NgayHetGiamGiaSP = row["NgayHetGiamGiaSP"] != DBNull.Value ? Convert.ToDateTime(row["NgayHetGiamGiaSP"]) : DateTime.Now;
            sanPham.NgayTaoSP = row["NgayTaoSP"] != DBNull.Value ? Convert.ToDateTime(row["NgayTaoSP"]) : DateTime.MinValue;
            sanPham.NgayCapNhatSP = row["NgayCapNhatSP"] != DBNull.Value ? Convert.ToDateTime(row["NgayCapNhatSP"]) : DateTime.MinValue;
            sanPham.TrangThaiSP = row["TrangThaiSP"] != DBNull.Value ? Convert.ToInt32(row["TrangThaiSP"]) : 0;
            sanPham.GiaGocSauThueSP = row["GiaGocSauThueSP"] != DBNull.Value ? Convert.ToDecimal(row["GiaGocSauThueSP"]) : 0;
            sanPham.GiaSauGiamVaThueSP = row["GiaSauGiamVaThueSP"] != DBNull.Value ? Convert.ToDecimal(row["GiaSauGiamVaThueSP"]) : 0;
            sanPham.SoLuongDaBanSP = row["SoLuongDaBanSP"] != DBNull.Value ? Convert.ToInt32(row["SoLuongDaBanSP"]) : 0;
            sanPham.DiemTrungBinhSP = row["DiemTrungBinhSP"] != DBNull.Value ? Convert.ToDecimal(row["DiemTrungBinhSP"]) : 0;
            sanPham.SoLuotDGSP = row["SoLuotDGSP"] != DBNull.Value ? Convert.ToInt32(row["SoLuotDGSP"]) : 0;
            sanPham.UrlAnh = row["UrlAnh"] != DBNull.Value ? row["UrlAnh"].ToString()! : string.Empty;
            sanPham.TrangThaiLSP = row["TrangThaiLSP"] != DBNull.Value ? Convert.ToInt32(row["TrangThaiLSP"]) : 0;
            sanPham.ThueGTGTLSP = row["ThueGTGTLSP"] != DBNull.Value ? Convert.ToDecimal(row["ThueGTGTLSP"]) : 0;
            sanPham.TenLSP = row["TenLSP"] != DBNull.Value ? row["TenLSP"].ToString()! : string.Empty;
            sanPham.TenQG = row["TenQG"] != DBNull.Value ? row["TenQG"].ToString()! : string.Empty;
            sanPham.TenTH = row["TenTH"] != DBNull.Value ? row["TenTH"].ToString()! : string.Empty;
        }

        private static void MapMayLanh(MayLanh mayLanh, DataRow row)
        {
            mayLanh.MaML = row["MaML"] != DBNull.Value ? Convert.ToInt32(row["MaML"]) : 0;
            mayLanh.CongSuatLamLanhML = row["CongSuatLamLanhML"] != DBNull.Value ? row["CongSuatLamLanhML"].ToString()! : string.Empty;
            mayLanh.PhamViLamLanhML = row["PhamViLamLanhML"] != DBNull.Value ? row["PhamViLamLanhML"].ToString()! : string.Empty;
            mayLanh.DoOnML = row["DoOnML"] != DBNull.Value ? row["DoOnML"].ToString()! : string.Empty;
            mayLanh.LoaiGasML = row["LoaiGasML"] != DBNull.Value ? row["LoaiGasML"].ToString()! : string.Empty;
            mayLanh.CheDoGioML = row["CheDoGioML"] != DBNull.Value ? row["CheDoGioML"].ToString()! : string.Empty;
        }

        private static void MapTuLanh(TuLanh tuLanh, DataRow row)
        {
            tuLanh.MaTL = row["MaTL"] != DBNull.Value ? Convert.ToInt32(row["MaTL"]) : 0;
            tuLanh.DungTichNganDaTL = row["DungTichNganDaTL"] != DBNull.Value ? row["DungTichNganDaTL"].ToString()! : string.Empty;
            tuLanh.DungTichNganLanhTL = row["DungTichNganLanhTL"] != DBNull.Value ? row["DungTichNganLanhTL"].ToString()! : string.Empty;
            tuLanh.LayNuocNgoaiTL = row["LayNuocNgoaiTL"] != DBNull.Value ? row["LayNuocNgoaiTL"].ToString()! : string.Empty;
            tuLanh.LayDaTuDongTL = row["LayDaTuDongTL"] != DBNull.Value ? row["LayDaTuDongTL"].ToString()! : string.Empty;
        }

        private static void MapMayLocKhongKhi(MayLocKhongKhi mlkk, DataRow row)
        {
            mlkk.MaMLKK = row["MaMLKK"] != DBNull.Value ? Convert.ToInt32(row["MaMLKK"]) : 0;
            mlkk.LoaiBuiLocDuocMLKK = row["LoaiBuiLocDuocMLKK"] != DBNull.Value ? row["LoaiBuiLocDuocMLKK"].ToString()! : string.Empty;
            mlkk.PhamViLocMLKK = row["PhamViLocMLKK"] != DBNull.Value ? row["PhamViLocMLKK"].ToString()! : string.Empty;
            mlkk.LuongGioMLKK = row["LuongGioMLKK"] != DBNull.Value ? row["LuongGioMLKK"].ToString()! : string.Empty;
            mlkk.MangLocMLKK = row["MangLocMLKK"] != DBNull.Value ? row["MangLocMLKK"].ToString()! : string.Empty;
            mlkk.BangDieuKhienMLKK = row["BangDieuKhienMLKK"] != DBNull.Value ? row["BangDieuKhienMLKK"].ToString()! : string.Empty;
            mlkk.DoOnMLKK = row["DoOnMLKK"] != DBNull.Value ? row["DoOnMLKK"].ToString()! : string.Empty;
            mlkk.CamBienMLKK = row["CamBienMLKK"] != DBNull.Value ? row["CamBienMLKK"].ToString()! : string.Empty;
        }

        private static void MapMayLocNuoc(MayLocNuoc mln, DataRow row)
        {
            mln.MaMLN = row["MaMLN"] != DBNull.Value ? Convert.ToInt32(row["MaMLN"]) : 0;
            mln.KieuLapMLN = row["KieuLapMLN"] != DBNull.Value ? row["KieuLapMLN"].ToString()! : string.Empty;
            mln.CongSuatLocMLN = row["CongSuatLocMLN"] != DBNull.Value ? row["CongSuatLocMLN"].ToString()! : string.Empty;
            mln.TiLeLocThaiMLN = row["TiLeLocThaiMLN"] != DBNull.Value ? row["TiLeLocThaiMLN"].ToString()! : string.Empty;
            mln.ChiSoNuocMLN = row["ChiSoNuocMLN"] != DBNull.Value ? row["ChiSoNuocMLN"].ToString()! : string.Empty;
            mln.DoPHThucTeMLN = row["DoPHThucTeMLN"] != DBNull.Value ? row["DoPHThucTeMLN"].ToString()! : string.Empty;
            mln.ApLucNuocYeuCauMLN = row["ApLucNuocYeuCauMLN"] != DBNull.Value ? row["ApLucNuocYeuCauMLN"].ToString()! : string.Empty;
            mln.SoLoiLocMLN = row["SoLoiLocMLN"] != DBNull.Value ? Convert.ToInt32(row["SoLoiLocMLN"]) : 0;
            mln.BangDieuKhienMLN = row["BangDieuKhienMLN"] != DBNull.Value ? row["BangDieuKhienMLN"].ToString()! : string.Empty;
        }

        private static void MapMayRuaChen(MayRuaChen mrc, DataRow row)
        {
            mrc.MaMRC = row["MaMRC"] != DBNull.Value ? Convert.ToInt32(row["MaMRC"]) : 0;
            mrc.NuocTieuThuMRC = row["NuocTieuThuMRC"] != DBNull.Value ? row["NuocTieuThuMRC"].ToString()! : string.Empty;
            mrc.SoChenRuaDuocMRC = row["SoChenRuaDuocMRC"] != DBNull.Value ? row["SoChenRuaDuocMRC"].ToString()! : string.Empty;
            mrc.DoOnMRC = row["DoOnMRC"] != DBNull.Value ? row["DoOnMRC"].ToString()! : string.Empty;
            mrc.BangDieuKhienMRC = row["BangDieuKhienMRC"] != DBNull.Value ? row["BangDieuKhienMRC"].ToString()! : string.Empty;
            mrc.ChieuDaiOngCapNuocMRC = row["ChieuDaiOngCapNuocMRC"] != DBNull.Value ? row["ChieuDaiOngCapNuocMRC"].ToString()! : string.Empty;
            mrc.ChieuDaiOngThoatNuocMRC = row["ChieuDaiOngThoatNuocMRC"] != DBNull.Value ? row["ChieuDaiOngThoatNuocMRC"].ToString()! : string.Empty;
        }

        private static void MapNoiComDien(NoiComDien ncd, DataRow row)
        {
            ncd.MaNCD = row["MaNCD"] != DBNull.Value ? Convert.ToInt32(row["MaNCD"]) : 0;
            ncd.DungTichNCD = row["DungTichNCD"] != DBNull.Value ? row["DungTichNCD"].ToString()! : string.Empty;
            ncd.ChucNangNCD = row["ChucNangNCD"] != DBNull.Value ? row["ChucNangNCD"].ToString()! : string.Empty;
            ncd.DoDayNCD = row["DoDayNCD"] != DBNull.Value ? row["DoDayNCD"].ToString()! : string.Empty;
            ncd.DieuKhienNCD = row["DieuKhienNCD"] != DBNull.Value ? row["DieuKhienNCD"].ToString()! : string.Empty;
            ncd.ChieuDaiDayDienNCD = row["ChieuDaiDayDienNCD"] != DBNull.Value ? row["ChieuDaiDayDienNCD"].ToString()! : string.Empty;
        }

        private static void MapNoiChien(NoiChien nc, DataRow row)
        {
            nc.MaNC = row["MaNC"] != DBNull.Value ? Convert.ToInt32(row["MaNC"]) : 0;
            nc.DungTichTongNC = row["DungTichTongNC"] != DBNull.Value ? row["DungTichTongNC"].ToString()! : string.Empty;
            nc.DungTichSuDungNC = row["DungTichSuDungNC"] != DBNull.Value ? row["DungTichSuDungNC"].ToString()! : string.Empty;
            nc.NhietDoNC = row["NhietDoNC"] != DBNull.Value ? row["NhietDoNC"].ToString()! : string.Empty;
            nc.HenGioNC = row["HenGioNC"] != DBNull.Value ? row["HenGioNC"].ToString()! : string.Empty;
            nc.BangDieuKhienNC = row["BangDieuKhienNC"] != DBNull.Value ? row["BangDieuKhienNC"].ToString()! : string.Empty;
            nc.ChieuDaiDayDienNC = row["ChieuDaiDayDienNC"] != DBNull.Value ? row["ChieuDaiDayDienNC"].ToString()! : string.Empty;
        }

        private static void MapTiVi(TiVi tv, DataRow row)
        {
            tv.MaTV = row["MaTV"] != DBNull.Value ? Convert.ToInt32(row["MaTV"]) : 0;
            tv.CoManHinhTV = row["CoManHinhTV"] != DBNull.Value ? row["CoManHinhTV"].ToString()! : string.Empty;
            tv.DoPhanGiaiTV = row["DoPhanGiaiTV"] != DBNull.Value ? row["DoPhanGiaiTV"].ToString()! : string.Empty;
            tv.LoaiManHinhTV = row["LoaiManHinhTV"] != DBNull.Value ? row["LoaiManHinhTV"].ToString()! : string.Empty;
            tv.TanSoQuetTV = row["TanSoQuetTV"] != DBNull.Value ? row["TanSoQuetTV"].ToString()! : string.Empty;
            tv.DieuKhienTV = row["DieuKhienTV"] != DBNull.Value ? row["DieuKhienTV"].ToString()! : string.Empty;
            tv.CongKetNoiTV = row["CongKetNoiTV"] != DBNull.Value ? row["CongKetNoiTV"].ToString()! : string.Empty;
        }

        private static void MapSanPhamDatHang(SanPhamDatHang spdh, DataRow row)
        {
            spdh.MaSP = row["MaSP"] != DBNull.Value ? Convert.ToInt32(row["MaSP"]) : 0;
            spdh.SoLuongDat = row["SoLuongDat"] != DBNull.Value ? Convert.ToInt32(row["SoLuongDat"]) : 0;
            spdh.TenSP = row["TenSP"] != DBNull.Value ? row["TenSP"].ToString()! : string.Empty;
            spdh.GiaDatSauGiamVaThue = row["GiaDatSauGiamVaThue"] != DBNull.Value ? Convert.ToDecimal(row["GiaDatSauGiamVaThue"]) : 0;
            spdh.UrlAnh = row["UrlAnh"] != DBNull.Value ? row["UrlAnh"].ToString()! : string.Empty;
        }

        public List<SanPham> LayDanhSachSanPham(int quyen, int maloai, string? tukhoa = null)
        {
            List<SanPham> list = new();

            var table = _sanPhamDAL.LayDanhSachSanPham(quyen, maloai, tukhoa);

            foreach (DataRow row in table.Rows)
            {
                SanPham sp = new SanPham();
                MapSanPham(sp, row);
                list.Add(sp);
            }

            return list;
        }

        public List<SanPham> LayDanhSachSanPhamBangListMa(List<int> listMaSP)
        {
            List<SanPham> list = new();

            var table = _sanPhamDAL.LayDanhSachSanPhamBangListMa(listMaSP);

            foreach (DataRow row in table.Rows)
            {
                SanPham sp = new SanPham();
                MapSanPham(sp, row);
                list.Add(sp);
            }

            return list;
        }

        public List<SanPhamDatHang> LayDanhSachSanPhamTrongDonHang(int maDH)
        {
            List<SanPhamDatHang> list = new();

            var table = _sanPhamDAL.LayDanhSachSanPhamTrongDonHang(maDH);

            foreach (DataRow row in table.Rows)
            {
                SanPhamDatHang sp = new SanPhamDatHang();
                MapSanPhamDatHang(sp, row);
                list.Add(sp);
            }

            return list;
        }
        
        public SanPham? LaySanPham(int quyen, int masp)
        {
            var sanPham = _sanPhamDAL.LaySanPham(quyen, masp);

            if (sanPham.Rows.Count == 0)
                return null;

            DataRow row = sanPham.Rows[0];

            int maloai = row["MaLSP"] != DBNull.Value ? Convert.ToInt32(row["MaLSP"]) : 0;

            if (maloai <= 0)
                return null;
            
            switch (maloai)
            {
                case 1:
                    var ml = new MayLanh();
                    MapSanPham(ml, row);

                    ml.ListAnh = _anhBLL.LayDanhSachAnhSanPham(quyen, ml.MaSP);
                    ml.ListDanhGia = _danhGiaBLL.LayDanhSachDanhGia(masp);

                    var mayLanh = _sanPhamDAL.LayMayLanh(masp);
                    if (mayLanh.Rows.Count == 0)
                        return ml;
                    DataRow rowML = mayLanh.Rows[0];
                    MapMayLanh(ml, rowML);

                    return ml;
                case 2:
                    var tl = new TuLanh();
                    MapSanPham(tl, row);

                    tl.ListAnh = _anhBLL.LayDanhSachAnhSanPham(quyen, tl.MaSP);
                    tl.ListDanhGia = _danhGiaBLL.LayDanhSachDanhGia(masp);

                    var tuLanh = _sanPhamDAL.LayTuLanh(masp);
                    if (tuLanh.Rows.Count == 0)
                        return tl;
                    DataRow rowTL = tuLanh.Rows[0];
                    MapTuLanh(tl, rowTL);

                    return tl;
                case 3:
                    var mlkk = new MayLocKhongKhi();
                    MapSanPham(mlkk, row);

                    mlkk.ListAnh = _anhBLL.LayDanhSachAnhSanPham(quyen, mlkk.MaSP);
                    mlkk.ListDanhGia = _danhGiaBLL.LayDanhSachDanhGia(masp);

                    var mayLocKhongKhi = _sanPhamDAL.LayMayLocKhongKhi(masp);
                    if (mayLocKhongKhi.Rows.Count == 0)
                        return mlkk; 
                    DataRow rowMLKK = mayLocKhongKhi.Rows[0];
                    MapMayLocKhongKhi(mlkk, rowMLKK);

                    return mlkk;
                case 4:
                    var mln = new MayLocNuoc();
                    MapSanPham(mln, row);

                    mln.ListAnh = _anhBLL.LayDanhSachAnhSanPham(quyen, mln.MaSP);
                    mln.ListDanhGia = _danhGiaBLL.LayDanhSachDanhGia(masp);

                    var mayLocNuoc = _sanPhamDAL.LayMayLocNuoc(masp);
                    if (mayLocNuoc.Rows.Count == 0)
                        return mln; 
                    DataRow rowMLN = mayLocNuoc.Rows[0];
                    MapMayLocNuoc(mln, rowMLN);

                    return mln;
                case 5:
                    var mrc = new MayRuaChen();
                    MapSanPham(mrc, row);

                    mrc.ListAnh = _anhBLL.LayDanhSachAnhSanPham(quyen, mrc.MaSP);
                    mrc.ListDanhGia = _danhGiaBLL.LayDanhSachDanhGia(masp);

                    var mayRuaChen = _sanPhamDAL.LayMayRuaChen(masp);
                    if (mayRuaChen.Rows.Count == 0)
                        return mrc; 
                    DataRow rowMRC = mayRuaChen.Rows[0];
                    MapMayRuaChen(mrc, rowMRC);

                    return mrc;
                case 6:
                    var ncd = new NoiComDien();
                    MapSanPham(ncd, row);

                    ncd.ListAnh = _anhBLL.LayDanhSachAnhSanPham(quyen, ncd.MaSP);
                    ncd.ListDanhGia = _danhGiaBLL.LayDanhSachDanhGia(masp);

                    var noiComDien = _sanPhamDAL.LayNoiComDien(masp);
                    if (noiComDien.Rows.Count == 0)
                        return ncd; 
                    DataRow rowNCD = noiComDien.Rows[0];
                    MapNoiComDien(ncd, rowNCD);

                    return ncd;
                case 7:
                    var nc = new NoiChien();
                    MapSanPham(nc, row);

                    nc.ListAnh = _anhBLL.LayDanhSachAnhSanPham(quyen, nc.MaSP);
                    nc.ListDanhGia = _danhGiaBLL.LayDanhSachDanhGia(masp);

                    var noiChien = _sanPhamDAL.LayNoiChien(masp);
                    if (noiChien.Rows.Count == 0)
                        return nc; 
                    DataRow rowNC = noiChien.Rows[0];
                    MapNoiChien(nc, rowNC);

                    return nc;
                case 8:
                    var tv = new TiVi();
                    MapSanPham(tv, row);

                    tv.ListAnh = _anhBLL.LayDanhSachAnhSanPham(quyen, tv.MaSP);
                    tv.ListDanhGia = _danhGiaBLL.LayDanhSachDanhGia(masp);

                    var tiVi = _sanPhamDAL.LayTiVi(masp);
                    if (tiVi.Rows.Count == 0)
                        return tv; 
                    DataRow rowTV = tiVi.Rows[0];
                    MapTiVi(tv, rowTV);

                    return tv;
                default:
                    return null;
            }

        }

        public bool KiemTraTenSanPhamTonTai(string tenSP, int maSP = 0)
        {
            return _sanPhamDAL.KiemTraTenSanPhamTonTai(tenSP, maSP);
        }

        public int ThemSanPham(SanPham sanPham)
        {
            int maSP = _sanPhamDAL.ThemSanPham(sanPham);
            if (maSP > 0)
            {
                sanPham.MaSP = maSP;
                try
                {
                    switch (sanPham.MaLSP)
                    {
                        case 1:
                            if (sanPham is MayLanh ml)
                                _sanPhamDAL.ThemMayLanh(ml);
                            break;
                        case 2:
                            if (sanPham is TuLanh tl)
                                _sanPhamDAL.ThemTuLanh(tl);
                            break;
                        case 3:
                            if (sanPham is MayLocKhongKhi mlkk)
                                _sanPhamDAL.ThemMayLocKhongKhi(mlkk);
                            break;
                        case 4:
                            if (sanPham is MayLocNuoc mln)
                                _sanPhamDAL.ThemMayLocNuoc(mln);
                            break;
                        case 5:
                            if (sanPham is MayRuaChen mrc)
                                _sanPhamDAL.ThemMayRuaChen(mrc);
                            break;
                        case 6:
                            if (sanPham is NoiComDien ncd)
                                _sanPhamDAL.ThemNoiComDien(ncd);
                            break;
                        case 7:
                            if (sanPham is NoiChien nc)
                                _sanPhamDAL.ThemNoiChien(nc);
                            break;
                        case 8:
                            if (sanPham is TiVi tv)
                                _sanPhamDAL.ThemTiVi(tv);
                            break;
                    }
                }
                catch (Exception)
                {
                }
            }
            return maSP > 0 ? 1 : 0;
        }

        public int CapNhatSanPham(SanPham sanPham)
        {
            int result = _sanPhamDAL.CapNhatSanPham(sanPham);
            if (result > 0)
            {
                switch (sanPham.MaLSP)
                {
                    case 1:
                        if (sanPham is MayLanh ml)
                            _sanPhamDAL.CapNhatMayLanh(ml);
                        break;
                    case 2:
                        if (sanPham is TuLanh tl)
                            _sanPhamDAL.CapNhatTuLanh(tl);
                        break;
                    case 3:
                        if (sanPham is MayLocKhongKhi mlkk)
                            _sanPhamDAL.CapNhatMayLocKhongKhi(mlkk);
                        break;
                    case 4:
                        if (sanPham is MayLocNuoc mln)
                            _sanPhamDAL.CapNhatMayLocNuoc(mln);
                        break;
                    case 5:
                        if (sanPham is MayRuaChen mrc)
                            _sanPhamDAL.CapNhatMayRuaChen(mrc);
                        break;
                    case 6:
                        if (sanPham is NoiComDien ncd)
                            _sanPhamDAL.CapNhatNoiComDien(ncd);
                        break;
                    case 7:
                        if (sanPham is NoiChien nc)
                            _sanPhamDAL.CapNhatNoiChien(nc);
                        break;
                    case 8:
                        if (sanPham is TiVi tv)
                            _sanPhamDAL.CapNhatTiVi(tv);
                        break;
                }
            }
            return result;
        }

        public bool KiemTraCoTheSuaXoaSanPham(int maSP)
        {
            return _sanPhamDAL.KiemTraCoTheSuaXoaSanPham(maSP);
        }

        public int XoaSanPham(int maSP)
        {
            return _sanPhamDAL.XoaSanPham(maSP);
        }
    }
}
