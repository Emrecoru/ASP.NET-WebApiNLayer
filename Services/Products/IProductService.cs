﻿using App.Services.Products.Create;
using App.Services.Products.Dto;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    public interface IProductService
    {
        Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count);

        Task<ServiceResult<ProductDto?>> GetByIdAsync(int id);

        Task<ServiceResult<List<ProductDto>>> GetAllListAsync();

        Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize);

        Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);

        Task<ServiceResult> UpdateAsync(UpdateProductRequest request);

        Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest updateProductStockRequest);

        Task<ServiceResult> DeleteAsync(int id);
    }
}
