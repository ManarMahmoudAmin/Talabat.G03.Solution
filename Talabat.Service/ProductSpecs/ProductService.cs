using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecs;
using Talabat.Core;

namespace Talabat.Application.ProductSpecs
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
            => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();

        public async Task<int> GetCountAsync(ProductSpecificationsParams productParams)
        {
            var productSpecsForCount = new ProductsWithFiltrationForCountSpecifications(productParams);
            return await _unitOfWork.Repository<Product>().GetCountAsync(productSpecsForCount);
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            var productSpec = new ProductWithBrandAndCategorySpecifications(id);
            return await _unitOfWork.Repository<Product>().GetWithSpecAsync(productSpec);
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationsParams productParams)
        {
            var productSpecs = new ProductWithBrandAndCategorySpecifications(productParams);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(productSpecs);
            return Products;
        }

    }
}
