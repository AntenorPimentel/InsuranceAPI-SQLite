namespace Insurance.Business.DTOs
{
    public class InsuranceDto
    {
        public int ProductId { get; set; }
        public decimal InsuranceValue { get; set; }
        public string ProductTypeName { get; set; }
        public bool ProductTypeHasInsurance { get; set; }
        public double SalesPrice { get; set; }
        public int ProductTypeId { get; set; }
    }
}