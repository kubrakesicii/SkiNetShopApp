using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductsWithBrandAndTypeSpecification(ProductSpecParams productParams)
            :base(x => 
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);

            AddOrderBy(x => x.Name);

            if(!string.IsNullOrEmpty(productParams.Sort)){
                switch(productParams.Sort){
                    case "priceAsc" : 
                        AddOrderBy(x => x.Price);
                        break;
                    case "priceDesc" : 
                        AddOrderByDescending(x => x.Price);
                        break;
                    default : 
                        AddOrderBy(x => x.Name);
                        break;
                }
            }

            ApplyPaging(productParams.PageSize,
                        productParams.PageSize*(productParams.PageIndex-1));
        }

        public ProductsWithBrandAndTypeSpecification(int id) : base(p => p.Id == id)
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);

        }
    }
}