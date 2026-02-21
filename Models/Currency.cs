using Currency.API.Models.DTOs;

namespace Currency.API.Models
{
    public class Currency : CurrencyDTO
    {
        public int Id { get; set; }

        //propiedades heredadas de CurrencyDTO
        //public required string Name { get; set; }
        //public required string Code { get; set; }
        //public required decimal RateToBase { get; set; }
        
    }
}
