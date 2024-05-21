using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Product;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
            //private readonly IGenericRepository<Product> productsRepository;
            //private readonly IGenericRepository<ProductCategory> _productCatigoriesRepo;
            //private readonly IGenericRepository<ProductBrand> _productsBrandsRepo;
            private readonly IMapper _mapper;
            private readonly IProductService _productService;

            public ProductsController(
                //IGenericRepository<Product> genericRepository,
                //IGenericRepository<ProductCategory> productCatigoriesRepo,
                //IGenericRepository<ProductBrand> productsBrandsRepo,
                IMapper mapper,
                IProductService productService
                )
            {
                //productsRepository = genericRepository;
                //_productCatigoriesRepo = productCatigoriesRepo;
                //_productsBrandsRepo = productsBrandsRepo;
                _mapper = mapper;
                _productService = productService;
            }

            
            [HttpGet]
            public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProducts([FromQuery] ProductSpecificationsParams productParams)
            {
                //var Products = await productsRepository.GetAllAsync();

                var Products = await _productService.GetProductsAsync(productParams);
                var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
                var Count = await _productService.GetCountAsync(productParams);
                return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, Count, Data));
            }

            [HttpGet("{id}")]
            [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
            public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
            {

                var Product = await _productService.GetProductAsync(id);
                if (Product is null)
                    return NotFound(new { Message = "Not Found", StatusCode = 404 });
                return Ok(_mapper.Map<Product, ProductToReturnDto>(Product));
            }

            [HttpGet("brands")]
            public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
            {
                var brands = await _productService.GetBrandsAsync();
                return Ok(brands);
            }

            [HttpGet("categories")]
            public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetProductCategories()
            {
                var categories = await _productService.GetCategoriesAsync();
                return Ok(categories);
            }
        }
}