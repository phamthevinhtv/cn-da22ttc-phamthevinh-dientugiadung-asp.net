using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.BLL
{
    public class ProductBLL
    {
        private readonly ProductDAL _productDAL;

        public ProductBLL(ProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public List<SanPhamKhachHang> GetAllProductsForCustomer(int maLSP)
        {
            return _productDAL.GetAllProductsForCustomer(maLSP);
        }

        public List<SanPham> GetAllProductsOrigin(int maLSP)
        {
            return _productDAL.GetAllProductsOrigin(maLSP);
        }

        public List<SanPhamKhachHang> GetAllProductsByNameForCustomer(string tenSP)
        {
            return _productDAL.GetAllProductsByNameForCustomer(tenSP);
        }

        public List<SanPhamKhachHang> GetByProductIdsForCustomer(List<int> ids)
        {
            return _productDAL.GetByProductIdsForCustomer(ids);
        }

        public SanPhamKhachHang? GetByProductIdForCustomer(int id)
        {
            return _productDAL.GetByProductIdForCustomer(id);
        }

        public List<LoaiSanPham> GetAllProductTypesForCustomer()
        {
            return _productDAL.GetAllProductTypesForCustomer();
        }

        public void CreateUpdateRating(DanhGia danhGia)
        {
            _productDAL.CreateUpdateRating(danhGia);
        }

        public ChiTietSanPhamKhachHang? GetProductDetailForCustomer(int productId)
        {
            var chiTietSanPhamKhachHang = _productDAL.GetProductDetailForCustomer(productId);

            if (chiTietSanPhamKhachHang == null) return null;

            var images = _productDAL.GetImagesProduct(productId);

            var ratings = _productDAL.GetRatingProduct(productId);

            chiTietSanPhamKhachHang.ListAnh = images ?? new List<Anh>();

            chiTietSanPhamKhachHang.ListDanhGia = ratings ?? new List<DanhGia>();

            switch(chiTietSanPhamKhachHang.MaLSP) 
            {
            case 1:
                chiTietSanPhamKhachHang.MayLanh = _productDAL.GetMayLanhDetailForCustomer(productId) ?? new MayLanh { MaSP = productId };
                break;
            case 2:
                chiTietSanPhamKhachHang.TuLanh = _productDAL.GetTuLanhDetailForCustomer(productId) ?? new TuLanh { MaSP = productId };
                break;
            case 3:
                chiTietSanPhamKhachHang.MayLocKhongKhi = _productDAL.GetMayLocKhongKhiDetailForCustomer(productId) ?? new MayLocKhongKhi { MaSP = productId };
                break;
            case 4:
                chiTietSanPhamKhachHang.MayLocNuoc = _productDAL.GetMayLocNuocDetailForCustomer(productId) ?? new MayLocNuoc { MaSP = productId };
                break;
            case 5:
                chiTietSanPhamKhachHang.MayRuaChen = _productDAL.GetMayRuaChenDetailForCustomer(productId) ?? new MayRuaChen { MaSP = productId };
                break;
            case 6:
                chiTietSanPhamKhachHang.NoiComDien = _productDAL.GetNoiComDienDetailForCustomer(productId) ?? new NoiComDien { MaSP = productId };
                break;
            case 7:
                chiTietSanPhamKhachHang.NoiChien = _productDAL.GetNoiChienDetailForCustomer(productId) ?? new NoiChien { MaSP = productId };
                break;
            case 8:
                chiTietSanPhamKhachHang.TiVi = _productDAL.GetTiViDetailForCustomer(productId) ?? new TiVi { MaSP = productId };
                break;
            default:
                chiTietSanPhamKhachHang = null;
                break;
            }

            return chiTietSanPhamKhachHang;
        }

        public SanPham? GetByProductIdOrigin(int productId)
        {
            var product = _productDAL.GetByProductIdOrigin(productId);

            if (product == null) return null;

            switch(product.MaLSP) 
            {
            case 1:
                product.MayLanh = _productDAL.GetMayLanhDetailForCustomer(productId) ?? new MayLanh { MaSP = productId };
                break;
            case 2:
                product.TuLanh = _productDAL.GetTuLanhDetailForCustomer(productId) ?? new TuLanh { MaSP = productId };
                break;
            case 3:
                product.MayLocKhongKhi = _productDAL.GetMayLocKhongKhiDetailForCustomer(productId) ?? new MayLocKhongKhi { MaSP = productId };
                break;
            case 4:
                product.MayLocNuoc = _productDAL.GetMayLocNuocDetailForCustomer(productId) ?? new MayLocNuoc { MaSP = productId };
                break;
            case 5:
                product.MayRuaChen = _productDAL.GetMayRuaChenDetailForCustomer(productId) ?? new MayRuaChen { MaSP = productId };
                break;
            case 6:
                product.NoiComDien = _productDAL.GetNoiComDienDetailForCustomer(productId) ?? new NoiComDien { MaSP = productId };
                break;
            case 7:
                product.NoiChien = _productDAL.GetNoiChienDetailForCustomer(productId) ?? new NoiChien { MaSP = productId };
                break;
            case 8:
                product.TiVi = _productDAL.GetTiViDetailForCustomer(productId) ?? new TiVi { MaSP = productId };
                break;
            default:
                product = null;
                break;
            }

            return product;
        }

        public void CreateProduct(SanPham sanPham)
        {
            int productId = _productDAL.CreateProduct(sanPham);
            
            switch(sanPham.MaLSP) 
            {
                case 1:
                    if (sanPham.MayLanh != null)
                    {
                        sanPham.MayLanh.MaSP = productId;
                        _productDAL.CreateMayLanh(sanPham.MayLanh);
                    }
                    break;
                case 2:
                    if (sanPham.TuLanh != null)
                    {
                        sanPham.TuLanh.MaSP = productId;
                        _productDAL.CreateTuLanh(sanPham.TuLanh);
                    }
                    break;
                case 3:
                    if (sanPham.MayLocKhongKhi != null)
                    {
                        sanPham.MayLocKhongKhi.MaSP = productId;
                        _productDAL.CreateMayLocKhongKhi(sanPham.MayLocKhongKhi);
                    }
                    break;
                case 4:
                    if (sanPham.MayLocNuoc != null)
                    {
                        sanPham.MayLocNuoc.MaSP = productId;
                        _productDAL.CreateMayLocNuoc(sanPham.MayLocNuoc);
                    }
                    break;
                case 5:
                    if (sanPham.MayRuaChen != null)
                    {
                        sanPham.MayRuaChen.MaSP = productId;
                        _productDAL.CreateMayRuaChen(sanPham.MayRuaChen);
                    }
                    break;
                case 6:
                    if (sanPham.NoiComDien != null)
                    {
                        sanPham.NoiComDien.MaSP = productId;
                        _productDAL.CreateNoiComDien(sanPham.NoiComDien);
                    }
                    break;
                case 7:
                    if (sanPham.NoiChien != null)
                    {
                        sanPham.NoiChien.MaSP = productId;
                        _productDAL.CreateNoiChien(sanPham.NoiChien);
                    }
                    break;
                case 8:
                    if (sanPham.TiVi != null)
                    {
                        sanPham.TiVi.MaSP = productId;
                        _productDAL.CreateTiVi(sanPham.TiVi);
                    }
                    break;
            }
        }

        public void UpdateProduct(SanPham sanPham)
        {
            _productDAL.UpdateProduct(sanPham);
            
            switch(sanPham.MaLSP) 
            {
                case 1:
                    if (sanPham.MayLanh != null)
                    {
                        sanPham.MayLanh.MaSP = sanPham.MaSP;
                        _productDAL.UpdateMayLanh(sanPham.MayLanh);
                    }
                    break;
                case 2:
                    if (sanPham.TuLanh != null)
                    {
                        sanPham.TuLanh.MaSP = sanPham.MaSP;
                        _productDAL.UpdateTuLanh(sanPham.TuLanh);
                    }
                    break;
                case 3:
                    if (sanPham.MayLocKhongKhi != null)
                    {
                        sanPham.MayLocKhongKhi.MaSP = sanPham.MaSP;
                        _productDAL.UpdateMayLocKhongKhi(sanPham.MayLocKhongKhi);
                    }
                    break;
                case 4:
                    if (sanPham.MayLocNuoc != null)
                    {
                        sanPham.MayLocNuoc.MaSP = sanPham.MaSP;
                        _productDAL.UpdateMayLocNuoc(sanPham.MayLocNuoc);
                    }
                    break;
                case 5:
                    if (sanPham.MayRuaChen != null)
                    {
                        sanPham.MayRuaChen.MaSP = sanPham.MaSP;
                        _productDAL.UpdateMayRuaChen(sanPham.MayRuaChen);
                    }
                    break;
                case 6:
                    if (sanPham.NoiComDien != null)
                    {
                        sanPham.NoiComDien.MaSP = sanPham.MaSP;
                        _productDAL.UpdateNoiComDien(sanPham.NoiComDien);
                    }
                    break;
                case 7:
                    if (sanPham.NoiChien != null)
                    {
                        sanPham.NoiChien.MaSP = sanPham.MaSP;
                        _productDAL.UpdateNoiChien(sanPham.NoiChien);
                    }
                    break;
                case 8:
                    if (sanPham.TiVi != null)
                    {
                        sanPham.TiVi.MaSP = sanPham.MaSP;
                        _productDAL.UpdateTiVi(sanPham.TiVi);
                    }
                    break;
            }
        }

        public void CreateProductWithStoredProc(SanPham sanPham)
        {
            int productId = _productDAL.CreateProductWithStoredProc(sanPham);
            
            switch(sanPham.MaLSP) 
            {
                case 1:
                    if (sanPham.MayLanh != null)
                    {
                        sanPham.MayLanh.MaSP = productId;
                        _productDAL.CreateMayLanh(sanPham.MayLanh);
                    }
                    break;
                case 2:
                    if (sanPham.TuLanh != null)
                    {
                        sanPham.TuLanh.MaSP = productId;
                        _productDAL.CreateTuLanh(sanPham.TuLanh);
                    }
                    break;
                case 3:
                    if (sanPham.MayLocKhongKhi != null)
                    {
                        sanPham.MayLocKhongKhi.MaSP = productId;
                        _productDAL.CreateMayLocKhongKhi(sanPham.MayLocKhongKhi);
                    }
                    break;
                case 4:
                    if (sanPham.MayLocNuoc != null)
                    {
                        sanPham.MayLocNuoc.MaSP = productId;
                        _productDAL.CreateMayLocNuoc(sanPham.MayLocNuoc);
                    }
                    break;
                case 5:
                    if (sanPham.MayRuaChen != null)
                    {
                        sanPham.MayRuaChen.MaSP = productId;
                        _productDAL.CreateMayRuaChen(sanPham.MayRuaChen);
                    }
                    break;
                case 6:
                    if (sanPham.NoiComDien != null)
                    {
                        sanPham.NoiComDien.MaSP = productId;
                        _productDAL.CreateNoiComDien(sanPham.NoiComDien);
                    }
                    break;
                case 7:
                    if (sanPham.NoiChien != null)
                    {
                        sanPham.NoiChien.MaSP = productId;
                        _productDAL.CreateNoiChien(sanPham.NoiChien);
                    }
                    break;
                case 8:
                    if (sanPham.TiVi != null)
                    {
                        sanPham.TiVi.MaSP = productId;
                        _productDAL.CreateTiVi(sanPham.TiVi);
                    }
                    break;
            }
        }

        public void UpdateProductWithStoredProc(SanPham sanPham)
        {
            _productDAL.UpdateProductWithStoredProc(sanPham);
            
            switch(sanPham.MaLSP) 
            {
                case 1:
                    if (sanPham.MayLanh != null)
                    {
                        sanPham.MayLanh.MaSP = sanPham.MaSP;
                        _productDAL.UpdateMayLanh(sanPham.MayLanh);
                    }
                    break;
                case 2:
                    if (sanPham.TuLanh != null)
                    {
                        sanPham.TuLanh.MaSP = sanPham.MaSP;
                        _productDAL.UpdateTuLanh(sanPham.TuLanh);
                    }
                    break;
                case 3:
                    if (sanPham.MayLocKhongKhi != null)
                    {
                        sanPham.MayLocKhongKhi.MaSP = sanPham.MaSP;
                        _productDAL.UpdateMayLocKhongKhi(sanPham.MayLocKhongKhi);
                    }
                    break;
                case 4:
                    if (sanPham.MayLocNuoc != null)
                    {
                        sanPham.MayLocNuoc.MaSP = sanPham.MaSP;
                        _productDAL.UpdateMayLocNuoc(sanPham.MayLocNuoc);
                    }
                    break;
                case 5:
                    if (sanPham.MayRuaChen != null)
                    {
                        sanPham.MayRuaChen.MaSP = sanPham.MaSP;
                        _productDAL.UpdateMayRuaChen(sanPham.MayRuaChen);
                    }
                    break;
                case 6:
                    if (sanPham.NoiComDien != null)
                    {
                        sanPham.NoiComDien.MaSP = sanPham.MaSP;
                        _productDAL.UpdateNoiComDien(sanPham.NoiComDien);
                    }
                    break;
                case 7:
                    if (sanPham.NoiChien != null)
                    {
                        sanPham.NoiChien.MaSP = sanPham.MaSP;
                        _productDAL.UpdateNoiChien(sanPham.NoiChien);
                    }
                    break;
                case 8:
                    if (sanPham.TiVi != null)
                    {
                        sanPham.TiVi.MaSP = sanPham.MaSP;
                        _productDAL.UpdateTiVi(sanPham.TiVi);
                    }
                    break;
            }
        }

        public List<SanPham> GetAllProductsByNameForAdmin(string? tenSP, int productTypeId = 0)
        {
            return _productDAL.GetAllProductsByNameForAdmin(tenSP, productTypeId);
        }

        public List<Anh> GetProductImages(int productId)
        {
            return _productDAL.GetProductImages(productId);
        }

        public void AddProductImage(int productId, string imageUrl, bool isDefault = false)
        {
            _productDAL.AddProductImage(productId, imageUrl, isDefault);
        }

        public void DeleteProductImage(int imageId)
        {
            _productDAL.DeleteProductImage(imageId);
        }

        public void SetDefaultImage(int imageId, int productId)
        {
            _productDAL.SetDefaultImage(imageId, productId);
        }

        public bool CanDeleteProduct(int productId)
        {
            return _productDAL.CanDeleteProduct(productId);
        }

        public void DeleteProduct(int productId)
        {
            _productDAL.DeleteProduct(productId);
        }

        public SanPham? GetProductWithDetailsForAdmin(int productId)
        {
            var product = _productDAL.GetByProductIdOrigin(productId);

            if (product == null) return null;

            switch(product.MaLSP) 
            {
                case 1:
                    product.MayLanh = _productDAL.GetMayLanhDetailForCustomer(productId) ?? new MayLanh { MaSP = productId };
                    break;
                case 2:
                    product.TuLanh = _productDAL.GetTuLanhDetailForCustomer(productId) ?? new TuLanh { MaSP = productId };
                    break;
                case 3:
                    product.MayLocKhongKhi = _productDAL.GetMayLocKhongKhiDetailForCustomer(productId) ?? new MayLocKhongKhi { MaSP = productId };
                    break;
                case 4:
                    product.MayLocNuoc = _productDAL.GetMayLocNuocDetailForCustomer(productId) ?? new MayLocNuoc { MaSP = productId };
                    break;
                case 5:
                    product.MayRuaChen = _productDAL.GetMayRuaChenDetailForCustomer(productId) ?? new MayRuaChen { MaSP = productId };
                    break;
                case 6:
                    product.NoiComDien = _productDAL.GetNoiComDienDetailForCustomer(productId) ?? new NoiComDien { MaSP = productId };
                    break;
                case 7:
                    product.NoiChien = _productDAL.GetNoiChienDetailForCustomer(productId) ?? new NoiChien { MaSP = productId };
                    break;
                case 8:
                    product.TiVi = _productDAL.GetTiViDetailForCustomer(productId) ?? new TiVi { MaSP = productId };
                    break;
            }

            return product;
        }
    }
}