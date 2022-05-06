using System.Collections.Generic;

namespace Insurance.Business.DTOs
{
    public class InsuranceOrderDto
    {
        public decimal InsuranceTotalValue { get; set; }
        public IEnumerable<InsuranceDto> InsuranceDtos { get; set; }

        public InsuranceOrderDto(IEnumerable<InsuranceDto> toInsure, decimal insuranceTotalValue)
        {
            InsuranceDtos = toInsure;
            InsuranceTotalValue = insuranceTotalValue;
        }
    }
}