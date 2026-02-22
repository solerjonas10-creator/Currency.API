namespace Currency.API.Models
{
    public class CurrencyConversion
    {
        public required string fromCurrencyCode { get; set; }
        public required string toCurrencyCode { get; set; }
        public decimal amount { get; set; }
        public decimal convertedAmount { get; set; }
    }
}
