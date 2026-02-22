namespace Currency.API.Models.DTOs
{
    public class CurrencyConversionDTO
    {
        public required string fromCurrencyCode { get; set; }
        public required string toCurrencyCode { get; set; }
        public decimal amount { get; set; }
    }
}
