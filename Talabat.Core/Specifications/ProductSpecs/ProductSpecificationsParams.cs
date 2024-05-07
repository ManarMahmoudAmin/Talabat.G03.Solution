using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductSpecificationsParams
    {
        private const int MaxPageSize = 10;

        private int pageSize = 5;  
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize || value <= 0) ? MaxPageSize : value; }
        }
        private int pageIndex = 1;
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value < pageIndex ? pageIndex : value; }
        }
        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

    }
}
