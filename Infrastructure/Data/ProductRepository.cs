using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        public StoreContext _context { get; }
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            return await _context.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}