using System;

namespace Insurance.Business.Infrastructure
{
    public class ProductNotFoundException : Exception
    {
        public int ProductId { get; }

        public ProductNotFoundException(int productId)
        {
            ProductId = productId;
        }
    }

    public class ProductTypeNotFoundException : Exception
    {
        public int ProductTypeId { get; }

        public ProductTypeNotFoundException(int productTypeId)
        {
            ProductTypeId = productTypeId;
        }
    }
}