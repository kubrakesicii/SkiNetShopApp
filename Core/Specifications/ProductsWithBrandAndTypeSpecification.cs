using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductsWithBrandAndTypeSpecification()
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
        }

        public ProductsWithBrandAndTypeSpecification(int id) : base(p => p.Id == id)
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
            
        }
    }
}