using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Nomenklatura.Product;
using AccountingApi.Helpers.Extentions;
using AccountingApi.Models;
using AutoMapper;
using EOfficeAPI.Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly INomenklaturaRepository _repo;
        private readonly IMapper _mapper;

        public ProductController(INomenklaturaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        //Post [baseUrl]/api/product/addproduct
        [HttpPost]
        [Route("addproduct")]
        public async Task<IActionResult> AddProduct(ProductPostDto productStock, [FromHeader]int? companyId)
        {
            //Yoxlamaq
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckProduct(currentUserId, companyId))
                return Unauthorized();
            //if (!ModelState.IsValid)
            //    return BadRequest();
            #endregion

            //productpostdto-nu worker table-na mapp etmek
            Product product = _mapper.Map<Product>(productStock);
            //mapp olunmus productu CreateProduct reposuna gonderirik.
            Product productToGet = await _repo.CreateProduct(product, companyId);
            //Stock hissesi
            Stock stocks = _mapper.Map<Stock>(productStock);
            //mapp olunmus stocku CreateStock reposuna gonderirik.
            Stock stockToGet = await _repo.CreateStock(stocks, product.Id);
            //database elave olunmus workeri qaytarmaq ucun 
            //ProductGetDto productForReturn = _mapper.Map<ProductGetDto>(productToGet);

            //2 obyekti  bir obyekte birlesdirmek
            var productToReturn = _mapper.MergeInto<ProductGetDto>(productToGet, stockToGet);
            return Ok(productToReturn);
        }
        //Get [baseUrl]/api/product/getproducts
        [HttpGet]
        [Route("getproducts")]
        public async Task<IActionResult> GetProducts([FromQuery]PaginationParam productParam, [FromHeader]int? companyId)
        {
            //Yoxlamaq
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckProduct(currentUserId, companyId))
                return Unauthorized();

            var producstock = await _repo.GetProducts(productParam, companyId);

            //var stocks = await _repo.GetStocks(productParam, companyId);

            var productsToReturn = _mapper.Map<IEnumerable<ProductGetDto>>(producstock);

            //2 obyekti  bir obyekte birlesdirmek
            //var productsToReturn = _mapper.MergeInto<IEnumerable<ProductGetDto>>(products);

            //Response.AddPagination(products.CurrentPage, products.PageSize,
            //  products.TotalCount, products.TotalPages);

            return Ok(productsToReturn);
        }
        //Get [baseUrl]/api/product/getpurchaseproducts
        [HttpGet]
        [Route("getpurchaseproducts")]
        public async Task<IActionResult> GetPurchaseProducts([FromQuery]PaginationParam productParam, [FromHeader]int? companyId)
        {
            //Checking
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckProduct(currentUserId, companyId))
                return Unauthorized();

            var producstock = await _repo.GetPurchaseProducts(productParam, companyId);

            //var stocks = await _repo.GetStocks(productParam, companyId);

            var productsToReturn = _mapper.Map<IEnumerable<ProductGetDto>>(producstock);

            //2 obyekti  bir obyekte birlesdirmek
            //var productsToReturn = _mapper.MergeInto<IEnumerable<ProductGetDto>>(products);

            //Response.AddPagination(products.CurrentPage, products.PageSize,
            //  products.TotalCount, products.TotalPages);

            return Ok(productsToReturn);
        }
        //Get [baseUrl]/api/product/getsaleproducts
        [HttpGet]
        [Route("getsaleproducts")]
        // pagination yazilmsdi                                                      
        public async Task<IActionResult> GetsSaleProducts([FromQuery]PaginationParam productParam, [FromHeader]int? companyId)
        {
            //Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckProduct(currentUserId, companyId))
                return Unauthorized();

            var producstock = await _repo.GetSaleProducts(productParam, companyId);

            //var stocks = await _repo.GetStocks(productParam, companyId);

            var productsToReturn = _mapper.Map<IEnumerable<ProductGetDto>>(producstock);

            //2 obyekti  bir obyekte birlesdirmek
            //var productsToReturn = _mapper.MergeInto<IEnumerable<ProductGetDto>>(products);

            //Response.AddPagination(products.CurrentPage, products.PageSize,
            //  products.TotalCount, products.TotalPages);

            return Ok(productsToReturn);
        }
        //Get [baseUrl]/api/product/geteditproduct
        [HttpGet]
        [Route("geteditproduct")]
        public async Task<IActionResult> GetEditProduct([FromHeader]int? productId, [FromHeader] int? companyId)
        {
            //Check
            #region Check

            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (productId == null)
                return StatusCode(409, "productId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckProduct(currentUserId, companyId))
                return NotFound();
            if (await _repo.CheckProductId(productId, companyId))
                return StatusCode(406, "Not Acceptable");

            #endregion

            Product product = await _repo.GetEditProduct(productId, companyId);

            Stock stock = await _repo.GetEditStock(productId);

            //2 obyekti  bir obyekte birlesdirmek
            var productToReturn = _mapper.MergeInto<ProductGetEditDto>(product, stock);

            return Ok(productToReturn);
        }
        //Put [baseUrl]/api/product/updateproduct
        [HttpPut]
        [Route("updateproduct")]
        //Update zamani bu metodla dolduracayiq
        public async Task<IActionResult> UpdateProduct([FromBody] ProductPutDto productPut, [FromHeader]int? productId, [FromHeader]int? companyId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!ModelState.IsValid)
                return BadRequest();
            if (productId == null)
                return StatusCode(409, "productId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckProduct(currentUserId, companyId))
                return NotFound();
            if (await _repo.CheckProductId(productId, companyId))
                return StatusCode(406, "Not Acceptable");
            #endregion


            //repoya id gonderirik ve o bize lazim olan mehsulu getirir.
            Product product_FromRepo = await _repo.GetEditProduct(productId, companyId);
            Stock stock_FromRepo = await _repo.GetEditStock(productId);
            //mapp olunmus mehsul
            Product productMapped = _mapper.Map(productPut, product_FromRepo);
            Stock stockToMapped = _mapper.Map(productPut, stock_FromRepo);
            //repoda mehsulu yenileyirik 
            Product updatedProduct = await _repo.EditProduct(product_FromRepo, productId);
            Stock updatedStock = await _repo.EditStock(stock_FromRepo, productId);
            //2 obyekti  bir obyekte birlesdirmek
            var productToReturn = _mapper.MergeInto<ProductGetDto>(product_FromRepo, stock_FromRepo);

            return Ok(productToReturn);
        }
        //Delete [baseUrl]/api/product/deleteproduct
        [HttpGet]
        [Route("deleteproduct")]
        public async Task<IActionResult> DeleteProduct([FromHeader]int? productId, [FromHeader]int? companyId)
        {
            //Check
            #region Check
            if (productId == null)
                return StatusCode(409, "workerId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");

            if (await _repo.DeleteProduct(productId, companyId) == null)
                return NotFound();
            #endregion

            Product DeletedProduct = await _repo.DeleteProduct(productId, companyId);

            return Ok();
        }
    }
}