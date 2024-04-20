using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly IGenaricRepository<Product> _productsRepo;
        private readonly IGenaricRepository<ProductBrand> _brandsRepo;
        private readonly IGenaricRepository<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenaricRepository<Product> productsRepo,
                                  IGenaricRepository<ProductBrand> brandsRepo,
                                  IGenaricRepository<ProductCategory> categoriesRepo,
                                  IMapper mapper)
        {
            _productsRepo = productsRepo;
            _brandsRepo = brandsRepo;
            _categoriesRepo = categoriesRepo;
            _mapper = mapper;
        }
       [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
       [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);
            var products = await _productsRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var CountSpec = new ProductsWithFiltrationForCountSpecifications(specParams);
            var count = await _productsRepo.GetCountAsync(CountSpec/*spec*/);

            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productsRepo.GetWithSpecAsync(spec);

            if (product is null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product)); // 200 
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();
            return Ok(brands);            
        }
        
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetAllCategories()
        {
            var categories = await _categoriesRepo.GetAllAsync();
            return Ok(categories);
        }
    }
}