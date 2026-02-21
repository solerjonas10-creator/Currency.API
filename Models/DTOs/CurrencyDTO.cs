namespace Currency.API.Models.DTOs
{
    public class CurrencyDTO
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required decimal RateToBase { get; set; }
    }
}
