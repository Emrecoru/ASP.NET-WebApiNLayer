using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.API.Controllers
{
    public class ProductsController : CustomControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _productService.GetAllListAsync();

            return CreateActionResult(serviceResult);
        }

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize)
        {
            var serviceResult = await _productService.GetPagedAllListAsync(pageNumber, pageSize);

            return CreateActionResult(serviceResult);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var serviceResult = await _productService.GetByIdAsync(id);

            return CreateActionResult(serviceResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var serviceResult =  await _productService.CreateAsync(request);

            return CreateActionResult(serviceResult);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductRequest request)
        {
            var serviceResult = await _productService.UpdateAsync(request);

            return CreateActionResult(serviceResult);
        }

        [HttpPatch("stock")]
        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request)
        {
            return CreateActionResult(await _productService.UpdateStockAsync(request));
        }

        //[HttpPut("UpdateStock")]
        //public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request)
        //{
        //    return CreateActionResult(await _productService.UpdateStockAsync(request));
        //}

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceResult = await _productService.DeleteAsync(id);

            return CreateActionResult(serviceResult);
        }
    }
}
