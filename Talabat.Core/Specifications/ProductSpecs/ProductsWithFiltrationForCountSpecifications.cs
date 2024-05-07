using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductsWithFiltrationForCountSpecifications : BaseSpecifications<Product>
    {
        public ProductsWithFiltrationForCountSpecifications(ProductSpecificationsParams specParams)
            : base(P =>
                    (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                    (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
            )
        {

        }
    }
}
