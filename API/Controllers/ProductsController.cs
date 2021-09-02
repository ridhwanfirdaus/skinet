using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
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
    
    public class ProductsController : BaseAPIController
    {

        private readonly IGenericRepository<Product> _productrepo;
        private readonly IGenericRepository<ProductType> _producttyperepo;
        private readonly IGenericRepository<ProductBrand> _productbrandrepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productrepo, IGenericRepository<ProductType> producttyperepo, IGenericRepository<ProductBrand> productbrandrepo, IMapper mapper)
        {
            _mapper = mapper;
            _productbrandrepo = productbrandrepo;
            _producttyperepo = producttyperepo;
            _productrepo = productrepo;

        }

        [HttpGet]
        public async Task<ActionResult<Pagination<IReadOnlyList<ProductToReturnDto>>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec =  new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _productrepo.CountAsync(countSpec);
            
            var products = await _productrepo.ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,totalItems,data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse),StatusCodes.Status404NotFound)]
       
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productrepo.GetEntityWithSpec(spec);
            if (product == null )
            {
                return NotFound(new APIResponse(404));  
            }
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var producttypes = await _producttyperepo.ListAllAsync();
            return Ok(producttypes);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productbrands = await _productbrandrepo.ListAllAsync();
            return Ok(productbrands);
        }
























    }
}