using AutoMapper;
using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string> I
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))

                return $"{"https://localhost:7139"}/{source.PictureUrl}";
        }
    }
}
