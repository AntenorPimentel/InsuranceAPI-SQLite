namespace Insurance.Business.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanBeInsured { get; set; }
        public decimal SurchargeRate { get; set; }
    }
}