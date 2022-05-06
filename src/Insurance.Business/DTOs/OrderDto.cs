using System.Collections.Generic;

namespace Insurance.Business.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
