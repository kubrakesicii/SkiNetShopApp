using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        public IGenericRepository<ProductBrand> _productBrandRepo { get; }
        public IGenericRepository<ProductType> _productTypeRepo { get; }
        public IMapper _mapper { get; }
        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProducts([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithBrandAndTypeSpecification(productParams);
            var countSpec = new ProductWithFilterForCountSpecification(productParams);

            var products = await _productsRepo.ListAsync(spec);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var data = _mapper
            .Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,totalItems,data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithBrandAndTypeSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            if(product == null){
                return NotFound(new ApiResponse(404));
            }

            return _mapper.Map<Product,ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllProductBrands()
        {
            var productBrands = await _productBrandRepo.ListAllAsync();
            return Ok(productBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllProductTypes()
        {
            var productTypes = await _productTypeRepo.ListAllAsync();
            return Ok(productTypes);
            
        }
    }
}